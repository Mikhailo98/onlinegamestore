using AutoFixture;
using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Domain;
using Domain.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Tests
{
    [TestFixture]
    public class CommentServiceTest
    {
        private Mock<IUnitOfWork> unitofworkMoq;
        private Mock<IMapper> mapperMoq;
        private Mock<IRepository<Game>> gameRepositoryMoq;
        private Mock<IRepository<Comment>> commentRepositoryMoq;

        ICommentService service;
        Fixture fixture = new Fixture();

        public CommentServiceTest()
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }



        [SetUp]
        public void SetUp()
        {
            unitofworkMoq = new Mock<IUnitOfWork>();
            mapperMoq = new Mock<IMapper>();

            gameRepositoryMoq = new Mock<IRepository<Game>>();
            commentRepositoryMoq = new Mock<IRepository<Comment>>();

            //repositories mock
            unitofworkMoq.Setup(tr => tr.GameRepository)
                   .Returns(gameRepositoryMoq.Object);

        
            unitofworkMoq.Setup(tr => tr.CommentRepository)
                   .Returns(commentRepositoryMoq.Object);
            
            service = new CommentService(unitofworkMoq.Object, mapperMoq.Object);
        }



        [Test]
        public void AnswerOnComment_ValidMethodParameter_True()
        {

            //assing
            var comment = new Fixture().Create<CreateAnswerCommentDto>();
            Game game = new Game() { Id = comment.GameId };
            var commentEntity = new Fixture()
                .Build<Comment>()
                .With(p => p.Game, game)
                .With(p => p.GameId, game.Id)
                .Without(p => p.ParentComment)
                .Without(p => p.ChildComments)
                .Create<Comment>();


            commentRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                  .Returns(Task.FromResult(commentEntity));
            gameRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                   .Returns(Task.FromResult(game));

            //act
            service.AnswerOnComment(comment);


            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
            commentRepositoryMoq.Verify(p => p.Create(It.IsAny<Comment>()), Times.Once);

        }

        [Test]
        public void AnswerOnComment_InValidGameId_ThrowsArgumentException()
        {

            //assing
            var comment = new CreateAnswerCommentDto() { GameId = 2, ParentCommentId = 1 };
            Game returnedNullGame = null;
            Comment commentEntity = new Comment() { Id = 1 };


            commentRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                  .Returns(Task.FromResult(commentEntity));
            gameRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                   .Returns(Task.FromResult(returnedNullGame));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.AnswerOnComment(comment);

            });

            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }


        [Test]
        public void AnswerOnComment_InvalidParentCommentId_ThrowsArgumentException()
        {

            //assing
            var comment = new CreateAnswerCommentDto() { GameId = 2, ParentCommentId = 1 };
            Game gameEntity = new Game() { Id = 2 };
            Comment nullCommentEntity = null;


            commentRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                  .Returns(Task.FromResult(nullCommentEntity));
            gameRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                   .Returns(Task.FromResult(gameEntity));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.AnswerOnComment(comment);

            });

            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }

        [Test]
        public void AnswerOnComment_MethodParameterIsNull_ThrowsArgumetnException()
        {

            //assing
            CreateAnswerCommentDto comment = null;
            Game gameEntity = null;
            Comment nullCommentEntity = null;


            commentRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                  .Returns(Task.FromResult(nullCommentEntity));
            gameRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                   .Returns(Task.FromResult(gameEntity));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.AnswerOnComment(comment);
            });

            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }


    }
}

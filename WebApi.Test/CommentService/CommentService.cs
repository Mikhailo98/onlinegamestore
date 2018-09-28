using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFixture;
using AutoMapper;
using BusinessLogicLayer.Configuration;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

using Domain;
using Domain.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Test
{
    [TestFixture]
    public class CommentServiceTest
    {


        private Mock<IUnitOfWork> unitofworkMoq;
        private Mock<IMapper> mapperMoq;
        private Mock<IRepository<Game>> gameRepositoryMoq;
        private Mock<IRepository<Comment>> commentRepositoryMoq;

        ICommentService service;//= new GameService(unitofworkMoq.Object, Mapper.Instance);


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
        public void AnswerOnComment_()
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
        public void AnswerOnComment__Game_id_isnot_valid()
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
        public void AnswerOnComment_Comment_id_isnot_valid()
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
        public void AnswerOnComment_CreateAnswerCommentDto_null()
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

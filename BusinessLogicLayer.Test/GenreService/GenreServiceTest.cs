using AutoFixture;
using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.GenreDto;
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

namespace BusinessLogicLayer.Test.GenreService
{
    [TestFixture]
    class GenreServiceTest
    {
        Publisher publisher = new Publisher() { Id = 1, Name = "Ubisoft" };
        Comment comment = new Comment() { Id = 1, Body = "Geat Game!", GameId = 1 };

        Game game;
        Game game_Null_Entity = null;


        private IGenreService service;

        private Mock<IUnitOfWork> unitofworkMoq;
        private Mock<IMapper> mapperMoq;

        private Mock<IRepository<Game>> gameRepositoryMoq;
        private Mock<IRepository<Publisher>> publisherRepositoryMoq;
        private Mock<IRepository<Comment>> commentRepositoryMoq;
        private Mock<IRepository<PlatformType>> platformRepositoryMoq;
        private Mock<IRepository<GenreGame>> gameGenreRepositoryMoq;
        private Mock<IRepository<GamePlatformType>> gamePlatformpositoryMoq;
        private Mock<IRepository<Genre>> genreRepositoryMoq;

        Fixture fixture = new Fixture();

        public GenreServiceTest()
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }


        [SetUp]
        public void SetUp()
        {
            unitofworkMoq = new Mock<IUnitOfWork>();
            mapperMoq = new Mock<IMapper>();

            publisherRepositoryMoq = new Mock<IRepository<Publisher>>();
            gameRepositoryMoq = new Mock<IRepository<Game>>();
            genreRepositoryMoq = new Mock<IRepository<Genre>>();
            commentRepositoryMoq = new Mock<IRepository<Comment>>();
            platformRepositoryMoq = new Mock<IRepository<PlatformType>>();
            gameGenreRepositoryMoq = new Mock<IRepository<GenreGame>>();
            gamePlatformpositoryMoq = new Mock<IRepository<GamePlatformType>>();

            //repositories mock
            unitofworkMoq.Setup(tr => tr.GameRepository)
                   .Returns(gameRepositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.PublisherRepository)
                    .Returns(publisherRepositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.GenreRepository)
                   .Returns(genreRepositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.CommentRepository)
                   .Returns(commentRepositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.GameGenreRepository)
                   .Returns(gameGenreRepositoryMoq.Object);


            unitofworkMoq.Setup(tr => tr.GamePlatformTypeRepository)
                   .Returns(gamePlatformpositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.PlatformTypeRepository)
                            .Returns(platformRepositoryMoq.Object);


            service = new Services.GenreService(unitofworkMoq.Object, mapperMoq.Object);
        }

        #region AddGenre

        [Test]
        public void AddGenre_ReceivesValidParameter_True()
        {
            //assign
            GenreCreateDto newGenre = new GenreCreateDto() { Name = "Genre", HeadGenreId = null };
            Genre nullGenre = null;
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(nullGenre));

            //act
            service.AddGenre(newGenre);

            //assert
            genreRepositoryMoq.Verify(p => p.Create(It.IsAny<Genre>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
        }


        [Test]
        public void AddGenre_ReceivesInValidNameOfGenre_ThrowsArgumentException()
        {
            //assign
            GenreCreateDto newGenre = new GenreCreateDto() { Name = "Genre", HeadGenreId = null };
            Genre returnedGenre = new Genre() { };
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(returnedGenre));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.AddGenre(newGenre);
            });

            //assert
            genreRepositoryMoq.Verify(p => p.Create(It.IsAny<Genre>()), Times.Never);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }
        #endregion


        #region DeleteGenre
        [Test]
        public void DeleteGenre_ReceivesValidParameter_DeletesEntity()
        {
            //assign
            Genre genreEntity = new Genre() { Id = 1, Name = "Genre" };
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(genreEntity));

            //act
            service.DeleteGenre(1);

            //assert
            genreRepositoryMoq.Verify(p => p.Delete(It.IsAny<Genre>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
        }




        [Test]
        public void DeleteGenre_ReceivesInValidIDofGenre_ThrowsArgumentException()
        {
            //assign
            Genre NullreturnedGenre = null;
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(NullreturnedGenre));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.DeleteGenre(1);
            });

            //assert
            genreRepositoryMoq.Verify(p => p.Delete(It.IsAny<Genre>()), Times.Never);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }
        #endregion


        #region EditGenre

        [Test]
        public void EditGenre_ReceivedInvalidId_ThrowsArgumentException()
        {
            EditGenreDto editedGenre = new EditGenreDto() { Id = 1, Name = "NewGenre" };
            Genre NullreturnedGenre = null;
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(NullreturnedGenre));


            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.EditGenre(editedGenre);
            });

            //assert
            genreRepositoryMoq.Verify(p => p.Delete(It.IsAny<Genre>()), Times.Never);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);



        }

        [Test]
        public void EditGenre_ReceivedInvalidId()
        {
            EditGenreDto editedGenre = new EditGenreDto() { Id = 1, Name = "NewGenre" };
            Genre returnedGenre = new Genre() { Id = 1, Name = "MotorSport" };
            GenreDto dto = new GenreDto() { Id = 1, Name = "MotorSport", SubGenres = new List<GenreDto>() };
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                .Returns(Task.FromResult(returnedGenre));
            mapperMoq.Setup(p => p.Map<GenreDto>(returnedGenre)).Returns(dto);

            //act

            service.EditGenre(editedGenre);

            //assert
            genreRepositoryMoq.Verify(p => p.Update(It.IsAny<Genre>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);

        }

        #endregion


        #region GetAll
        [Test]
        public void GetAll()
        {
            //assign
            Genre genre1 = new Genre() { Id = 1, Name = "MotorSport" };
            Genre genre2 = new Genre() { Id = 2, Name = "AutoSport", HeadGenre = genre1, HeadGenreId = genre1.Id };
            Genre genre3 = new Genre() { Id = 3, Name = "Strategy" };
            IEnumerable<Genre> ei = new List<Genre>() { genre1, genre2, genre3 };

            genreRepositoryMoq.Setup((p) => p.GetAsync(It.IsAny<Expression<Func<Genre, bool>>>(),
                                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>()))
            .Returns(Task.FromResult(ei));

            //act
            service.GetAll();

            //assert
            genreRepositoryMoq.Verify((p) => p.GetAsync(It.IsAny<Expression<Func<Genre, bool>>>(),
                                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>()), Times.Once);

        }


        #endregion

        #region GetInfo

        [Test]
        public async Task GetInfo_ReceiveID_ReturnGenreDto()
        {
            //assign
            Genre genre3 = new Genre() { Id = 3, Name = "Strategy" };
            var gg = fixture.Build<GenreGame>().With(p => p.Genre, genre3).With(p => p.Game,
                   fixture.Build<Game>().OmitAutoProperties()
                   .With(p => p.Id).With(p => p.Name).Create())
                   .CreateMany(5);
            genre3.GenreGames = gg.ToList();
            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
              .Returns(Task.FromResult(genre3));
            mapperMoq.Setup(p => p.Map<GenreDto>(genre3)).Returns(new GenreDto() { Id = 3, Name = "Strategy" });


            //act
            var strategy = await service.GetInfo(3);

            //assert
            Assert.NotNull(strategy);
            genreRepositoryMoq.Verify((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);

        }



        [Test]
        public void GetInfo_ReceiveInvalidID_ThrowArgumentException()
        {
            //assign
            Genre nullGenreEntity = null;

            genreRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
              .Returns(Task.FromResult(nullGenreEntity));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetInfo(123);
            });

            //assert
            genreRepositoryMoq.Verify((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);

        }




        #endregion




    }
}

using Autofac;
using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper;
using BusinessLogicLayer.Configuration;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.CommentDto;
using BusinessLogicLayer.Services;
using Domain;
using Domain.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Test.Unit_Of_Work;

namespace BusinessLogicLayer.Test
{ 

    [TestFixture]
    public class GameServiceTest
    {
        Publisher publisher = new Publisher() { Id = 1, Name = "Ubisoft" };
        Comment comment = new Comment() { Id = 1, Body = "Geat Game!", GameId = 1 };

        Game game;
        Game game_Null_Entity = null;


        private IGameService service;

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

        public GameServiceTest()
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


            service = new GameService(unitofworkMoq.Object, mapperMoq.Object);

            game = new Game()
            {
                Id = 1,
                Name = "Just Dance",
                Publisher = publisher,
                PublisherId = publisher.Id,
                Comments = new List<Comment>() { comment }
            };
        }



        #region GetInfo
        [Test]
        public async Task GetInfo_Give_ID_1_Should_Return_GameDto()
        {


            GameDto gamedto = fixture
                .Build<GameDto>()
                .With(p => p.Publisher, new PublisherDto() { Id = 1, Name = "Publisher" })
                .With(p => p.Comments)
                .Create();

            //assign
            gameRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game));
            mapperMoq.Setup(p => p.Map<GameDto>(It.IsAny<Game>()))
                .Returns(gamedto);



            var gameDto = await service.GetInfo(1);


            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }


        [Test]
        public void GetInfo_Should_Throw_ArgumentException()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game_Null_Entity));


            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetInfo(1);
            });

            //assert
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }
        #endregion


        #region AddGame

        [Test]
        public void AddGame_Give_CreategameDto_Return_true()
        {
            //assign
            var gameDto = new Fixture().Build<CreateGameDto>()
                .With(e => e.Platforms, new List<int>() { 1 })
                .With(e => e.Genres, new List<int>() { 1 })
                .Create();


            Genre genre = new Genre() { Id = 1, };
            PlatformType platform = new PlatformType() { Id = 1, };


            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                 .Returns(Task.FromResult(game_Null_Entity));

            publisherRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                   .Returns(Task.FromResult(publisher));

            genreRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
                   .Returns(Task.FromResult(genre));

            platformRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()))
                   .Returns(Task.FromResult(platform));



            //act
            service.AddGame(gameDto);

            //if game not exists
            gameRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);

            //if publisher exists
            publisherRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Once);

            platformRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<PlatformType, bool>>>()), Times.Once);

            gamePlatformpositoryMoq.Verify(p => p.Create(It.IsAny<GamePlatformType>()),
                Times.Once);
            gameGenreRepositoryMoq.Verify(p => p.Create(It.IsAny<GenreGame>()), Times.Once);


            gameRepositoryMoq.Verify(p => p.Create(It.IsAny<Game>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
        }


        #endregion


        #region DeleteGame

        [Test]
        public void DeleteGame_Give_id_return_GameDto()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .Returns(Task.FromResult(game));

            //act
            service.DeleteGame(1);

            //assert
            gameRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);

        }


        [Test]
        public void DeleteGame_Give_id_throw_ArgumentException()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
               .Returns(Task.FromResult(game_Null_Entity));


            // IGameService service = new GameService(unitofworkMoq.Object, Mapper.Instance);

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.DeleteGame(1);
            });

            //assert
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
        }

        #endregion


        #region GetGameLocalPath

        [Test]
        public void GetGameLocalPath_take_throw_ArgumentException()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game_Null_Entity));


            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.GetGameLocalPath(1);
            });

            //assert
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }


        [Test]
        public async Task GetGameLocalPath_take_id_return_string()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game));

            //act
            string localpath = await service.GetGameLocalPath(1);

            //assert
            Assert.NotNull(localpath);
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }


        #endregion


        #region CommentGame

        [Test]
        public void CommentGame_take_CreateCommentDto_throw_ArgumentException()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game_Null_Entity));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.CommentGame(new CreateCommentDto() { GameId = 1 });
            });

            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }



        [Test]
        public void CommentGame_take_CreateCommentDto_Assert_True()
        {
            //assign
            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                    .Returns(Task.FromResult(game));
            var comment = new Fixture()
                .Create<CreateCommentDto>();

            //act
            service.CommentGame(comment);

            //assert
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
            commentRepositoryMoq.Verify(p => p.Create(It.IsAny<Comment>()), Times.Once);
            gameRepositoryMoq.Verify(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }


        #endregion

        internal class DomainCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                fixture.Customizations.Add(
                    new Postprocessor<Game>(((Fixture)fixture).Engine, new CarCommand())
                );
            }
        }

        internal class CarCommand : ISpecimenCommand
        {
            public void Execute(object specimen, ISpecimenContext context)
            {
                var car = specimen as Game;
                if (car != null)
                {
                    //Null reference exeption here, no tires, no values in car sett
                    foreach (var tire in car.Comments)
                    {
                        tire.Game = car;
                    }
                }
            }
        }


        #region GetGenres
        [Test]
        public async Task GetGenres_take_id_returns_list_of_genres()
        {
            //assign
            var Genre1 = new Genre() { Id = 1, Name = "Racing" };
            var Genre2 = new Genre() { Id = 2, Name = "Autosport", HeadGenreId = 2, HeadGenre = Genre1 };
            var Genre3 = new Genre() { Id = 3, Name = "F1", HeadGenreId = 3, HeadGenre = Genre2 };

            Genre1.SubGenres.Add(Genre2);
            Genre2.SubGenres.Add(Genre3);
            var genregame1 = new GenreGame() { Genre = Genre1, GameId = 1, GenreId = 1 };
            var genregame2 = new GenreGame() { Genre = Genre2, GameId = 1, GenreId = 2 };
            var genregame3 = new GenreGame() { Genre = Genre3, GameId = 1, GenreId = 3 };
            Game game = new Game()
            {
                Id = 1,
                GenreGames = new List<GenreGame>()
                {
                    genregame1,
                    genregame2,
                    genregame3,
                }
            };

            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .Returns(Task.FromResult(game));
            mapperMoq.Setup(p => p.Map<List<GenreDto>>(game.GenreGames))
                .Returns(new List<GenreDto>());


            //act
            List<GenreDto> genres = await service.GetGenres(1);

            //assert
            Assert.NotNull(genres);
            gameRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }


        #endregion


        #region GetAllComments


        [Test]
        public async Task GetAllComments_ReturnComments_True()
        {
            //assign
            var Comment1 = new Comment() { Id = 1, Name = "What a great Game" };
            var dummyList = new Fixture()
    .Build<Comment>()
    .OmitAutoProperties()
    .With(p => p.Id, 1)
    .With(p => p.Body)
    .With(p => p.ParentComment, Comment1)
    .With(p => p.ParentCommentId, Comment1.Id)
    .CreateMany<Comment>(7).ToList();

            Game uniqueGame = new Game()
            {
                Id = 1,
                Comments = new List<Comment>(dummyList) { Comment1 }
            };

            gameRepositoryMoq.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .Returns(Task.FromResult(uniqueGame));
            mapperMoq.Setup(p => p.Map<List<CommentDto>>(uniqueGame.Comments))
                .Returns(new List<CommentDto>());


            //act
            List<CommentDto> comments = await service.GetAllComments(1);

            //assert
            Assert.NotNull(comments);
            gameRepositoryMoq.Verify(p =>
            p.GetSingleAsync(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
        }

        #endregion


        //TODO: Implement Unit test
        #region GetAll

        #endregion

        //TODO: Implement Unit test
        #region EditGame

        #endregion


    }






}





using AutoFixture;
using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
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

namespace BusinessLogicLayer.Test.PublisherService
{
    [TestFixture]
    public class PublisherServiceTest
    {

        private IPublisherService service;

        private Mock<IUnitOfWork> unitofworkMoq;
        private Mock<IMapper> mapperMoq;

        private Mock<IRepository<Game>> gameRepositoryMoq;
        private Mock<IRepository<Publisher>> publisherRepositoryMoq;

        Fixture fixture = new Fixture();

        public PublisherServiceTest()
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

            //repositories mock
            unitofworkMoq.Setup(tr => tr.GameRepository)
                   .Returns(gameRepositoryMoq.Object);

            unitofworkMoq.Setup(tr => tr.PublisherRepository)
                    .Returns(publisherRepositoryMoq.Object);

            service = new Services.PublisherService(unitofworkMoq.Object, mapperMoq.Object);

        }


        #region CreatePublisher
        [Test]
        public void CreatePublisher_ValidMethodParameter_True()
        {
            //assign
            Publisher returnNull = null;
            CreatePublisherDto newPublisher = new CreatePublisherDto() { Name = "Ubisoft" };
            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                    .Returns(Task.FromResult(returnNull));

            //act
            service.CreatePublisher(newPublisher);

            //assign
            publisherRepositoryMoq.Verify(p => p.Create(It.IsAny<Publisher>()), Times.Once);
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
        }

        [Test]
        public void CreatePublisher_PublisherNameAlreadyexists_ThrowArgumentException()
        {
            //assign
            Publisher existedEntity = new Publisher() { Id = 1, Name = "Ubisoft" };
            CreatePublisherDto newPublisher = new CreatePublisherDto() { Name = "Ubisoft" };
            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                    .Returns(Task.FromResult(existedEntity));

            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.CreatePublisher(newPublisher);
            });

            //assign
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);

        }

        #endregion


        #region EditPublisher

        [Test]
        public void EditPublisher_ValidMethodParameter_True()
        {
            //assign
            Publisher existedEntity = new Publisher() { Id = 1, Name = "Ubisoft" };
            EditPublisherDto newPublisher = new EditPublisherDto() { Id = 1, Name = "Sega" };

            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
                        .Returns(Task.FromResult(existedEntity));

            mapperMoq.Setup(p => p.Map(newPublisher, existedEntity))
                .Returns(new Publisher() { Id = 1, Name = "Sega" });

            //act
            service.EditPublisher(newPublisher);

            //assign
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Once);
            publisherRepositoryMoq.Verify(p => p.Update(It.IsAny<Publisher>()), Times.Once);

        }



        [Test]
        public void EditPublisher_EntityWithInvalidID_ThrowsArgumentException()
        {
            //assign
            Publisher returnedNUllEntity = null;
            EditPublisherDto newPublisher = new EditPublisherDto() { Id = 1, Name = "Sega" };
            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
            .Returns(Task.FromResult(returnedNUllEntity));


            //act
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.EditPublisher(newPublisher);
            });

            //assign
            unitofworkMoq.Verify(p => p.CommitAsync(), Times.Never);
            publisherRepositoryMoq.Verify(p => p.Update(It.IsAny<Publisher>()), Times.Never);

        }


        #endregion



        #region GetAll

        [Test]
        public void GetAll_ReturnsEnteties()
        {
            //assign
            var publishers = fixture.Build<Publisher>().Without(p => p.Games).CreateMany(6);
            publisherRepositoryMoq.Setup((p) => p.GetAsync(It.IsAny<Expression<Func<Publisher, bool>>>(),
                                It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>()))
            .Returns(Task.FromResult(publishers));

            //act
            service.GetAll();

            //assign
            publisherRepositoryMoq.Verify((p) => p.GetAsync(It.IsAny<Expression<Func<Publisher, bool>>>(),
                             It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>()), Times.Once);
        }


        #endregion


        #region GetGamesOfPublisher


        [Test]
        public void GetGamesOfPublisher_ReturnsEntities()
        {
            //assign
            var publisher = fixture.Build<Publisher>().Without(p => p.Games).Create();
            var games = fixture.Build<Game>()
                .OmitAutoProperties()
                .With(p => p.Id).With(p => p.Name).With(p => p.Publisher, publisher)
                .CreateMany<Game>(6);
            publisher.Games = games.ToList();

            gameRepositoryMoq.Setup((p) => p.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(),
                                   It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>()))
                                   .Returns(Task.FromResult(games));

            //act
            service.GetGamesOfPublisher(publisher.Id);

            //assign
            gameRepositoryMoq.Verify((p) => p.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(),
                                     It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>()), Times.Once);

        }

        #endregion


        #region GetInfo
        public async Task GetInfo_TakesValidId_ReturnsExistedPublisher()
        {
            //assign
            var publisher = fixture.Build<Publisher>().Without(p => p.Games).Create();
            var games = fixture.Build<Game>()
                .OmitAutoProperties()
                .With(p => p.Id).With(p => p.Name).With(p => p.Publisher, publisher)
                .CreateMany<Game>(6);
            publisher.Games = games.ToList();
            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
            .Returns(Task.FromResult(publisher));

            var publisherDto = new PublisherDto()
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Games = games.Select(p => new GameDto()
                { Id = p.Id, Name = p.Name }).ToList()
            };

            mapperMoq.Setup(p => p.Map<PublisherDto>(publisher)).Returns(publisherDto);

            //act
            var returnedDto = await service.GetInfo(publisher.Id);

            //assert
            Assert.IsNotNull(returnedDto);
            publisherRepositoryMoq.Verify((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Once);
        }


        public async Task GetInfo_TakesInValidId_ReturnsNull()
        {
            //assign
            Publisher NullPublisher = null;

            publisherRepositoryMoq.Setup((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()))
            .Returns(Task.FromResult(NullPublisher));

            PublisherDto bullPublisherDto = null;

            mapperMoq.Setup(p => p.Map<PublisherDto>(NullPublisher)).Returns(bullPublisherDto);

            //act
            var returnedDto = await service.GetInfo(NullPublisher.Id);

            //assert
            Assert.IsNull(returnedDto);
            publisherRepositoryMoq.Verify((p) => p.GetSingleAsync(It.IsAny<Expression<Func<Publisher, bool>>>()), Times.Once);
        }


        #endregion






    }
}

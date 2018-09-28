using Domain;
using Domain.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Test.Unit_Of_Work
{
    [TestFixture]
    public class UntOfWorkMoq
    {

        public Mock<IUnitOfWork> unitofworkMoq;
        public Mock<IRepository<Game>> gameRepositoryMoq;
        public Mock<IRepository<Publisher>> publisherRepositoryMoq;
        public Mock<IRepository<Comment>> commentRepositoryMoq;
        public Mock<IRepository<PlatformType>> platformRepositoryMoq;
        public Mock<IRepository<GenreGame>> gameGenreRepositoryMoq;
        public Mock<IRepository<GamePlatformType>> gamePlatformpositoryMoq;
        public Mock<IRepository<Genre>> genreRepositoryMoq;


        [SetUp]
        public void SetUp()
        {

            unitofworkMoq = new Mock<IUnitOfWork>();

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
        }



    }
}

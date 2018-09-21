using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Game> GameRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IRepository<Publisher> PublisherRepository { get; }
        IRepository<PlatformType> PlatformTypeRepository { get; }
        IRepository<GenreGame> GameGenreRepository { get;  }
        IRepository<GamePlatformType> GamePlatformTypeRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}

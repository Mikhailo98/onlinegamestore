using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository GameRepository { get; }
        ICommentRepository CommentRepository { get; }
        IGenreRepository GenreRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        IPlatformTypeRepository PlatformTypeRepository { get; }
        IGameGenreRepository GameGenreRepository { get;  }
        IGamePlatformTypeRepository GamePlatformTypeRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}

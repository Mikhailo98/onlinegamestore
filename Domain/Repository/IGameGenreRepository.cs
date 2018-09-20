using Domain.Repository;

namespace Domain
{
    public interface IGameGenreRepository : IRepository<GenreGame, int>
    {
    }
}
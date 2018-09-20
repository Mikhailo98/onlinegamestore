using DAL.Repository;
using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL
{
    internal class GenreRepository : Repository<Genre, int>, IGenreRepository
    {

        public GenreRepository(ApplicationContext context) : base(context)
        {
        }

        public Genre GetGenreFullInfo(int id)
        {
            IQueryable<Genre> query = dbSet;

            return query
                    .Include(p => p.HeadGenre)
                    .Include(p => p.GenreGames)
                        .ThenInclude(g => g.Game)                           
                    .SingleOrDefault(p => p.Id == id);
        }
    }

}
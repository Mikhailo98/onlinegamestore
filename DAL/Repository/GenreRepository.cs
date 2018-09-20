using DAL.Repository;
using Domain;
using Domain.Repository;

namespace DAL
{
    internal class GenreRepository : Repository<Genre, int>, IGenreRepository
    {
        private ApplicationContext context;

        public GenreRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }

}
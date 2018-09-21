using DAL;
using DAL.Repository;
using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    class GameGenreRepository : IRepository<GenreGame>
    {
        private readonly ApplicationContext context;
        private readonly DbSet<GenreGame> dbSet;


        public GameGenreRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<GenreGame>();

        }

        public void Create(GenreGame entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(GenreGame entity)
        {
            throw new NotImplementedException();
        }

        public List<GenreGame> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GenreGame> Get(Expression<Func<GenreGame, bool>> filter = null, Func<IQueryable<GenreGame>, IOrderedQueryable<GenreGame>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public GenreGame GetSingle(Expression<Func<GenreGame, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Update(GenreGame entity)
        {
            throw new NotImplementedException();
        }

        Task<List<GenreGame>> IRepository<GenreGame>.Get()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<GenreGame>> IRepository<GenreGame>.Get(Expression<Func<GenreGame, bool>> filter, Func<IQueryable<GenreGame>, IOrderedQueryable<GenreGame>> orderBy)
        {
            throw new NotImplementedException();
        }

        Task<GenreGame> IRepository<GenreGame>.GetSingle(Expression<Func<GenreGame, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}

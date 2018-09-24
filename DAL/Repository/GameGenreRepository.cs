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
            dbSet.Add(entity);
        }

        public void Delete(GenreGame entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }


        public async Task<IEnumerable<GenreGame>> GetAsync(Expression<Func<GenreGame, bool>> filter = null, Func<IQueryable<GenreGame>, IOrderedQueryable<GenreGame>> orderBy = null)
        {
            IQueryable<GenreGame> query = dbSet;


            query = query
               .Include(p => p.Genre)
                .Include(p => p.Game);

            if (filter != null)
            {
                query = query.Where(filter);
            }



            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<GenreGame> GetSingleAsync(Expression<Func<GenreGame, bool>> filter)
        {
            IQueryable<GenreGame> query = dbSet;

            return await query
                .Include(p => p.Genre)
                .Include(p => p.Game)
                .FirstOrDefaultAsync(filter);
        }

        public void Update(GenreGame entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

    }
}

using System.Collections.Generic;
using Domain;
using Domain.Repository;
using DAL.Repository;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DAL
{
    class GameRepository : IRepository<Game>
    {
        private readonly ApplicationContext context;
        private readonly DbSet<Game> dbSet;



        public GameRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<Game>();

        }

        public void Create(Game entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(Game entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<List<Game>> GetAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAsync(Expression<Func<Game, bool>> filter = null,
            Func<IQueryable<Game>, IOrderedQueryable<Game>> orderBy = null)
        {
            IQueryable<Game> query = dbSet;

            query = query
                 .Include(p => p.GenreGames)
                 .ThenInclude(p => p.Genre)
                 .Include(p => p.Publisher)
                 .Include(p => p.GamePlatformTypes)
                 .ThenInclude(p => p.PlatformType)
                 .Include(p => p.Comments);



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

        public async Task<Game> GetSingleAsync(Expression<Func<Game, bool>> filter)
        {
            IQueryable<Game> query = dbSet;

            return await query
                .Include(p => p.GenreGames)
                       .ThenInclude(p => p.Genre)
                    .Include(p => p.Publisher)
                    .Include(p => p.GamePlatformTypes)
                    .ThenInclude(p => p.PlatformType)
                    .Include(p => p.Comments)
                .FirstOrDefaultAsync(filter);
        }

        public void Update(Game entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
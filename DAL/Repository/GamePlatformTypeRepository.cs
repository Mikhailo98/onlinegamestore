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
    class GamePlatformTypeRepository : IRepository<GamePlatformType>
    {
        private readonly ApplicationContext context;
        private readonly DbSet<GamePlatformType> dbSet;


        public GamePlatformTypeRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<GamePlatformType>();
        }

        public void Create(GamePlatformType entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(GamePlatformType entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }


        public async Task<IEnumerable<GamePlatformType>> GetAsync(Expression<Func<GamePlatformType, bool>> filter = null, Func<IQueryable<GamePlatformType>, IOrderedQueryable<GamePlatformType>> orderBy = null)
        {
            IQueryable<GamePlatformType> query = dbSet;

            query = query
                 .Include(p => p.Game)
                 .Include(p => p.PlatformType);

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
 
                return await query.ToListAsync();

        }

        public async Task<GamePlatformType> GetSingleAsync(Expression<Func<GamePlatformType, bool>> filter)
        {
            IQueryable<GamePlatformType> query = dbSet;

            return await query
                 .Include(p => p.Game)
                 .Include(p => p.PlatformType)
             .FirstOrDefaultAsync(filter);
        }

        public void Update(GamePlatformType entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

    }
}
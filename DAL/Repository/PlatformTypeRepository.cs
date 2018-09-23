using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Repository;
using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class PlatformTypeRepository : IRepository<PlatformType>
    {


        private readonly ApplicationContext context;
        private readonly DbSet<PlatformType> dbSet;

        public PlatformTypeRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<PlatformType>();
        }

        public void Create(PlatformType entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(PlatformType entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<List<PlatformType>> GetAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<PlatformType>> GetAsync(Expression<Func<PlatformType, bool>> filter = null, Func<IQueryable<PlatformType>, IOrderedQueryable<PlatformType>> orderBy = null)
        {
            IQueryable<PlatformType> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter)
                    .Include(p => p.GamePlatformtypes)
                    .ThenInclude(p => p.Game);
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

        public async Task<PlatformType> GetSingleAsync(Expression<Func<PlatformType, bool>> filter)
        {
            IQueryable<PlatformType> query = dbSet;

            return await query
                .Include(p => p.GamePlatformtypes)
                    .ThenInclude(p => p.Game)
                .FirstOrDefaultAsync(filter);
        }

        public void Update(PlatformType entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
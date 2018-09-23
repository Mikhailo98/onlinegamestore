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

        public Task Create(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<GamePlatformType>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GamePlatformType>> GetAsync(Expression<Func<GamePlatformType, bool>> filter = null, Func<IQueryable<GamePlatformType>, IOrderedQueryable<GamePlatformType>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<GamePlatformType> GetSingleAsync(Expression<Func<GamePlatformType, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePlatformType>.Create(GamePlatformType entity)
        {
            dbSet.Add(entity);
        }

        void IRepository<GamePlatformType>.Delete(GamePlatformType entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        void IRepository<GamePlatformType>.Update(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }
    }
}
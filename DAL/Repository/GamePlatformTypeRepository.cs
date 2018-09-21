using DAL;
using DAL.Repository;
using Domain;
using Domain.Repository;
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

        public GamePlatformTypeRepository(ApplicationContext context)
        {

        }

        public Task Create(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<GamePlatformType>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GamePlatformType>> Get(Expression<Func<GamePlatformType, bool>> filter = null, Func<IQueryable<GamePlatformType>, IOrderedQueryable<GamePlatformType>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<GamePlatformType> GetSingle(Expression<Func<GamePlatformType, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePlatformType>.Create(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePlatformType>.Delete(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePlatformType>.Update(GamePlatformType entity)
        {
            throw new NotImplementedException();
        }
    }
}
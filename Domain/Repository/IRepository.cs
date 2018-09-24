using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRepository<T>
    {

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAsync(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

    }
}

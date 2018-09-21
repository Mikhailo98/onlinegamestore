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
        Task<List<T>> Get();

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T> GetSingle(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> Get(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Repository
{
    public interface IRepository<T, K>
    {
        List<T> Get();

        T GetById(K id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        T GetSingle(Expression<Func<T, bool>> filter, string includeProperties = "");
        IEnumerable<T> Get(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = "");

    }
}

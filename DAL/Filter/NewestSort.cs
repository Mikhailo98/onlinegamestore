using System.Linq;
using Domain;
using Domain.Filter;

namespace DataAccessLayer.Filter
{
    internal class NewestSortStrategy : ISortStrategy
    {
        public IOrderedQueryable<Game> DoAlgorithm(IQueryable<Game> query)
        {
            return query.OrderByDescending(p => p.PublishDate);
        }
    }
}
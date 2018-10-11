using Domain;
using Domain.Filter;
using System.Linq;

namespace DataAccessLayer.Filter
{
    internal class MostViewedStrategy : ISortStrategy
    {
        public IOrderedQueryable<Game> DoAlgorithm(IQueryable<Game> query)
        {
            return query.OrderByDescending(p => p.Comments.Count);
        }
    }
}

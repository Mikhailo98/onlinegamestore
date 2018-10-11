using System.Linq;
using Domain;
using Domain.Filter;

namespace DataAccessLayer.Filter
{
    internal class MostCommentedStrategy : ISortStrategy
    {
        public IOrderedQueryable<Game> DoAlgorithm(IQueryable<Game> query)
        {
            return query.OrderByDescending(p => p.Comments.Count);
        }
    }
}
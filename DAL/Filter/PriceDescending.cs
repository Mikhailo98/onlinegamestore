using Domain;
using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Filter
{
    public class PriceDescending : ISortStrategy
    {
        public IOrderedQueryable<Game> DoAlgorithm(IQueryable<Game> query)
        {
            return query.OrderByDescending(j => j.Price);
        }
    }

}

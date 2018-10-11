using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Filter
{
    public interface ISortStrategy
    {
        IOrderedQueryable<Game> DoAlgorithm(IQueryable<Game> query);
    }
}

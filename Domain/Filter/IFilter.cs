using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Filter
{
    public interface IFilter<T>
    {
        Expression<Func<T, bool>> FilterExpression { get; }
    }
}

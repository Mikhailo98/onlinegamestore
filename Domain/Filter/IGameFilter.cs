using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain;

namespace Domain.Filter
{
    public interface IGameFilter
    {
        Expression<Func<Game, bool>> GameExpression { get; }

        IGameFilter IncludeGenres(List<int> ids);
        IGameFilter IncludePlatforms(List<int> ids);
        IGameFilter SetMinPrice(decimal minimal);
        IGameFilter SetMaxPrice(decimal maximun);
    }
}
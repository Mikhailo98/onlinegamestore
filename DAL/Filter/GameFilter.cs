using Domain;
using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace DataAccessLayer.Filter
{
    class GameFilter : IGameFilter
    {
        protected Expression expression = Expression.Default(typeof(BinaryExpression));
        public Expression<Func<Game, bool>> GameExpression
        {
            get
            {
                return Expression.Lambda<Func<Game, bool>>(expression);
            }
        }


        public IGameFilter IncludeGenres(List<int> ids)
        {
            Expression<Func<Game, bool>> expr1 = p => p.GenreGames.Select(s => s.GenreId)
            .Intersect(ids).Count() == ids.Count;
   
            expression = Expression.AndAlso(expression, expr1);

            //expression = p => p.GenreGames.Select(s => s.GenreId)
            //.Intersect(ids).Count() == ids.Count;
            return this;
        }

        public IGameFilter IncludePlatforms(List<int> ids)
        {

            Expression<Func<Game, bool>> expr1 = p => p.GamePlatformTypes
            .Select(s => s.PlatformTypeId)
            .Intersect(ids).Count() == ids.Count;

            expression = Expression.AndAlso(expression, expr1);

            //expression = p => p.GamePlatformTypes.Select(s => s.PlatformTypeId)
            //.Intersect(ids).Count() == ids.Count;

            return this;
        }

        public IGameFilter SetMinPrice(decimal minimal)
        {
            //expression = p => p.Price >= minimal;
            return this;
        }

        public IGameFilter SetMaxPrice(decimal maximun)
        {
           // expression = p => p.Price <= maximun;
            return this;
        }
    }
}

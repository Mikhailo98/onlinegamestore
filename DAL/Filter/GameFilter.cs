using Domain;
using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic;
using DataAccessLayer.Instastucture;

namespace DataAccessLayer.Filter
{
    class GameFilter : IGameFilter
    {
        Expression<Func<Game, bool>> defaultExp = p => p.Id != 0;
        
        public Expression<Func<Game, bool>> FilterExpression
        {
            set
            {
                defaultExp = value;
            }
            get
            {
                return defaultExp;
            }
        }


     

        public IGameFilter IncludeGenres(List<int> ids)
        {
            Expression<Func<Game, bool>> expr1 = p => p.GenreGames.Select(s => s.GenreId)
                                                       .Intersect(ids).Count() == ids.Count;
            FilterExpression = FilterExpression.AlsoAnd(expr1);
            return this;
        }

        public IGameFilter IncludePlatforms(List<int> ids)
        {

            Expression<Func<Game, bool>> expr1 = p => p.GamePlatformTypes
                                                       .Select(s => s.PlatformTypeId)
                                                       .Intersect(ids).Count() == ids.Count;
            FilterExpression = FilterExpression.AlsoAnd(expr1);

            return this;
        }

        public IGameFilter SetMinPrice(decimal minimal)
        {
            Expression<Func<Game, bool>> expr1 = p => p.Price >= minimal;
            FilterExpression = FilterExpression.AlsoAnd(expr1);
            return this;
        }


        public IGameFilter SetMaxPrice(decimal maximun)
        {
            Expression<Func<Game, bool>> expr1 = p => p.Price <= maximun;
            FilterExpression = FilterExpression.AlsoAnd(expr1);


            return this;
        }

        public IGameFilter IncludeNameSubstring(string substring)
        {
            Expression<Func<Game, bool>> expr1 = p => p.Name.Contains(substring);
            FilterExpression = FilterExpression.AlsoAnd(expr1);
            return this;
        }
    }

}

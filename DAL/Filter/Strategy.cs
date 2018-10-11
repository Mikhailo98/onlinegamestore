using Domain;
using Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Filter
{
    public enum SortType
    {
        mostViewed,
        MostCommented,
        ByPriceAsc,
        byPriceDesc,
        New
    }       
    

    public class Context
    {

        private static Dictionary<SortType, ISortStrategy> _strategies =
            new Dictionary<SortType, ISortStrategy>();


        static Context()
        {
            _strategies.Add(SortType.ByPriceAsc, new PriceAscending());
            _strategies.Add(SortType.byPriceDesc, new PriceDescending());
            _strategies.Add(SortType.MostCommented, new MostCommentedStrategy());
            _strategies.Add(SortType.mostViewed, new MostViewedStrategy());
            _strategies.Add(SortType.New, new NewestSortStrategy());
        }



        public static Func<IQueryable<Game>, IOrderedQueryable<Game>> DoContext(SortType title,
            IQueryable<Game> query)
        {
            return _strategies[title].DoAlgorithm;
        }
    }   
}

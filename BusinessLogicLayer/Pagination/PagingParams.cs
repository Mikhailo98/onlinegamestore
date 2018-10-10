using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Pagination
{
    public class PagingParamsBll
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 3;

        public List<int> Genres { get; set; }

        public List<int> Platforms { get; set; } 

        public SortDropDownList DropdownList { get; set; }

        public decimal MaxPrice { get; set; } 

        public decimal MinPrice { get; set; }

        public string SubName { get; set; } 

    }

    public enum SortDropDownList
    {
        mostViewed,
        MostCommented,
        ByPriceAsc,
        byPriceDesc,
        New
    }
}

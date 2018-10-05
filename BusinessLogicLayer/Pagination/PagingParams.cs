using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Pagination
{
    public class PagingParamsBll
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 3;

        public List<int> Genres { get; set; } = new List<int>();

        public List<int> Platforms { get; set; } = new List<int>();

        public DropdownList DropdownList { get; set; }

        public decimal MaxPrice { get; set; } = Decimal.MaxValue;

        public decimal MinPrice { get; set; }

        public string SubName { get; set; } = string.Empty;

    }

    public enum DropdownList
    {
        mostViewed,
        MostCommented,
        ByPriceAsc,
        byPriceDesc,
        New
    }
}

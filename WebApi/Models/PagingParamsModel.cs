using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastucture;

namespace WebApi.Models
{
    public class PagingParamsModel
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 3;

        [CustomIDsValidation]
        public List<int> Genres { get; set; } = new List<int>();

        [CustomIDsValidation]
        public List<int> Platforms { get; set; } = new List<int>();

        public SortDropDownList DropdownList { get; set; }

        public decimal MaxPrice { get; set; } = Decimal.MaxValue;

        public decimal MinPrice { get; set; }

        public string SubName { get; set; } = string.Empty;

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


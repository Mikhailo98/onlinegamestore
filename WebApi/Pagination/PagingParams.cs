using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastucture;

namespace WebApi.Pagination
{
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        [CustomIDsValidation]
        public List<int> Genres { get; set; }

        [CustomIDsValidation]
        public List<int> Platforms { get; set; }

        public DropdownList DropdownList { get; set; } = DropdownList.New;

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public string SubName { get; set; }

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

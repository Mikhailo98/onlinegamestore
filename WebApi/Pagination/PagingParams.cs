using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public SortType DropdownList { get; set; } = SortType.New;

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public string SubName { get; set; }

    }

    public enum SortType
    {
        [Display(Name = "Most Viewed")]
        mostViewed,

        [Display(Name = "Most Commented")]
        MostCommented,

        [Display(Name = "Price: Low to High")]
        ByPriceAsc,

        [Display(Name = "Price: High to Low")]
        byPriceDesc,

        [Display(Name = "The Newest")]
        New
    }
}

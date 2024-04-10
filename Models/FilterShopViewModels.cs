using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class FilterShopViewModels
    {
        public string CurrentFilter { get; set; }
        public string Keyword { get; set; }
        public int? Page { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string SortBy { get; set; }

        public FilterShopViewModels()
        {
            SortBy = "newProduct"; // Default value
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class FilterBlogViewModel
    {
        public string SearchTxt { get; set; }
        public int? Page { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
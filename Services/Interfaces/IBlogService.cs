using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<Blogs> GetAll();
        IEnumerable<Blogs> FilterBlog(FilterBlogViewModel modelRequest);
        IEnumerable<BlogCategories> GetBlogCategories();
        IEnumerable<Blogs> GetRecentNews();
        Blogs GetBlogById(int id);
        IEnumerable<Blogs> GetRelatedBlog(int id);
        BlogCategories GetCategoryById(int? id);
        BlogCategories GetCategoryByMeta(string meta);


    }
}

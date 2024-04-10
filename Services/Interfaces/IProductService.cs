using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Products> GetAll();
        Task<ListCategories> GetCategoryByMeta(string meta);
        IEnumerable<Products> GetProductsByCategory(int id);
        Task<ListCategories> GetCategoryByProductId(int? id);
        IEnumerable<Products> GetRelatedProducts(int id, int categoryId);
        IEnumerable<Products> GetLastestProducts();
        IEnumerable<ListCategories> GetCategoriesDesc();
        Task<IEnumerable<Products>> GetProductOrderBy(FilterShopViewModels models);
        Task<Products> GetProductById(int id);
        IEnumerable<Products> GetLastestProduct();
        IEnumerable<Products> GetTopProduct();
        IEnumerable<Products> GetReviewProduct();

    }
}

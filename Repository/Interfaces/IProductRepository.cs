using System.Collections.Generic;
using WebDemo.Models;

namespace WebDemo.Repository.Interfaces
{
    public interface IProductRepository: IGenericRepository<Products>
    {
        ListCategories GetCategoryByMeta(string meta);
        IEnumerable<Products> GetFavoriteProductsByEmail(string email);
        IEnumerable<Products> GetProductsByCategory(int id);
        ListCategories GetCategoryByProductId(int? id);
        IEnumerable<Products> GetRelatedProducts(int id, int categoryId);
        IEnumerable<Products> GetLastestProducts();
        IEnumerable<ListCategories> GetCategoriesDesc();
        IEnumerable<Products> GetProductOrderBy(string sortBy, string searchString, int? minPrice, int? maxPrice, int? page);
        Products GetProductById(int id);
    }
}

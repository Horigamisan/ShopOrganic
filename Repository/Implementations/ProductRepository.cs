using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Helper;
using WebDemo.Models;
using WebDemo.Repository.Interfaces;

namespace WebDemo.Repository.Implementations
{
    public class ProductRepository : GenericRepository<Products>, IProductRepository
    {
        public ProductRepository(ShopOnlineEntities context) : base(context)
        {
            
        }

        public ListCategories GetCategoryByMeta(string meta)
        {
            return _context.ListCategories.Where(x => x.hide == false).Where(x => x.meta == meta).FirstOrDefault();
        }

        public IEnumerable<Products> GetFavoriteProductsByEmail(string email)
        {
            var favoriteProducts = _context.Favorites.Where(i => i.AspNetUsers.Email == email).ToList();
            var products = favoriteProducts.Select(fp => fp.Products).ToList();

            return products;
        }

        public IEnumerable<Products> GetProductsByCategory(int id)
        {
            return _context.Products.Where(x => x.hide == false).Where(x => x.categoryid == id).ToList();
        }

        public ListCategories GetCategoryByProductId(int? id)
        {
            return _context.ListCategories.Where(x => x.id == id).FirstOrDefault();
        }

        public IEnumerable<Products> GetRelatedProducts(int id, int categoryId)
        {
            return _context.Products.Where(x => x.hide == false).Where(x => x.id != id && x.categoryid == categoryId).ToList();
        }

        public IEnumerable<Products> GetProductOrderBy(string sortBy, string searchString, int? minPrice, int? maxPrice, int? page)
        {
            IEnumerable<Products> productList = _context.Products.Where(x => x.hide == false);

            switch (sortBy)
            {
                case "newProduct":
                    // Sắp xếp danh sách sản phẩm theo sản phẩm mới nhất
                    productList = productList.OrderByDescending(p => p.id);
                    break;
                case "oldProduct":
                    // Sắp xếp danh sách sản phẩm theo sản phẩm cũ nhất
                    productList = productList.OrderBy(p => p.id);
                    break;
                case "highPrice":
                    // Sắp xếp danh sách sản phẩm theo giá cao nhất
                    productList = productList.OrderByDescending(p => p.price);
                    break;
                case "lowPrice":
                    // Sắp xếp danh sách sản phẩm theo giá thấp nhất
                    productList = productList.OrderBy(p => p.price);
                    break;
                default:
                    // Mặc định sắp xếp theo sản phẩm mới nhất
                    productList = productList.OrderByDescending(p => p.id);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                string newText = NormalizeTwoTextVN.Normalize(searchString);
                productList = productList.Where(p =>
                        (p.name != null && NormalizeTwoTextVN.Normalize(p.name).Contains(newText)) ||
                        (p.description != null && NormalizeTwoTextVN.Normalize(p.description).Contains(newText))
                        );
            }

            if (minPrice != null && maxPrice != null)
            {
                productList = productList.Where(p => p.price >= minPrice && p.price <= maxPrice);
            }

            return productList.ToList();
        }

        public IEnumerable<ListCategories> GetCategoriesDesc()
        {
            return _context.ListCategories.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Products> GetLastestProducts()
        {
            return _context.Products.Where(x => x.hide == false && x.latest_product == true).OrderByDescending(x => x.order).ToList();
        }

        public Products GetProductById(int id)
        {
            return _context.Products.Where(x => x.hide == false).Where(x => x.id == id).FirstOrDefault();
        }
    }
}
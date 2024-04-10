using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using WebDemo.Helper;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class ProductService : BaseService, IProductService
    {
        private readonly DbSet<ListCategories> _listCtgRepo;
        private readonly DbSet<Products> _productsRepo;

        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _listCtgRepo = unitOfWork.Repository<ListCategories>();
            _productsRepo = unitOfWork.Repository<Products>();
        }
        public async Task<ListCategories> GetCategoryByMeta(string meta)
        {
            return await _listCtgRepo.Where(x => x.hide == false && x.meta == meta).FirstOrDefaultAsync();
        }

        public IEnumerable<Products> GetProductsByCategory(int id)
        {
            return _productsRepo.Where(x => x.hide == false && x.categoryid == id).ToList();
        }

        public async Task<ListCategories> GetCategoryByProductId(int? id)
        {
            return await _listCtgRepo.Where(x => x.id == id).FirstOrDefaultAsync();
        }

        public IEnumerable<Products> GetRelatedProducts(int id, int categoryId)
        {
            return _productsRepo.Where(x => x.hide == false && x.id != id && x.categoryid == categoryId).ToList();
        }

        public async Task<IEnumerable<Products>> GetProductOrderBy(FilterShopViewModels models)
        {
            var productList = _productsRepo.Where(x => x.hide == false);

            switch (models.SortBy)
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

            var products = await productList.ToListAsync();

            if (!string.IsNullOrEmpty(models.Keyword))
            {
                string newText = NormalizeTwoTextVN.Normalize(models.Keyword);
                products = products.Where(p =>
                        (p.name != null && NormalizeTwoTextVN.Normalize(p.name).Contains(newText)) ||
                        (p.description != null && NormalizeTwoTextVN.Normalize(p.description).Contains(newText))
                        ).ToList();
            }

            if (models.MinPrice != null && models.MaxPrice != null)
            {
                products = products.Where(p => p.price >= models.MinPrice && p.price <= models.MaxPrice).ToList();
            }

            return products;
        }

        public IEnumerable<ListCategories> GetCategoriesDesc()
        {
            return _listCtgRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Products> GetLastestProducts()
        {
            return _productsRepo.Where(x => x.hide == false && x.latest_product == true).OrderByDescending(x => x.order).ToList();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _productsRepo.Where(x => x.hide == false && x.id == id).FirstOrDefaultAsync();
        }

        public IEnumerable<Products> GetAll()
        {
            return _productsRepo.Where(x => x.hide == false).ToList();
        }

        public IEnumerable<Products> GetLastestProduct()
        {
            return _productsRepo.Where(x => x.hide == false && x.latest_product == true).OrderByDescending(x => x.order).ToList();
        }
        
        public IEnumerable<Products> GetTopProduct()
        {
            return _productsRepo.Where(x => x.hide == false && x.top_product == true).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Products> GetReviewProduct()
        {
            return _productsRepo.Where(x => x.hide == false && x.review_product == true).OrderByDescending(x => x.order).ToList();
        }
    }
}
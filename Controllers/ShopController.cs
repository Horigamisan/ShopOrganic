using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using PagedList;
using System.Data.SqlClient;
using WebDemo.Helper;
using WebDemo.Services.Interfaces;
using System.Threading.Tasks;

namespace WebDemo.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;

        public ShopController(IUserService userService, IFavoriteService favoriteService, IProductService productService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
            _productService = productService;
        }

        // GET: Shop
        public async Task<ActionResult> Index(FilterShopViewModels models)
        {
            ViewBag.meta = "san-pham";
            ViewBag.CurrentSort = models.SortBy;
            ViewBag.MinPrice = models.MinPrice ?? 1000;
            ViewBag.MaxPrice = models.MaxPrice ?? 500000;

            ViewBag.Keyword = models.Keyword;

            IEnumerable<Products> productList = await _productService.GetProductOrderBy(models);

            var email = User.Identity.Name;
            if (email != null)
            {
                var products = _favoriteService.GetFavoriteProductsByEmail(email);
                ViewBag.products = products.Select(x => x.id).ToList();
            }

            ViewBag.Sort = models.SortBy;
            int pageSize = 6;
            int pageNumber = (models.Page ?? 1);
            ViewBag.Page = pageNumber;
            ViewBag.TotalProduct = productList.Count();
            
            return View(productList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListCategory()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetCategoriesDesc();
            return PartialView(model);
        }

        public ActionResult GetLastestProduct()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetLastestProducts();
            return PartialView(model);
        }

    }
}
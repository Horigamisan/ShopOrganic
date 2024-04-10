using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();

        private readonly ILayoutService _layoutService;
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;
        private readonly IBlogService _blogService;

        public DefaultController(ILayoutService layoutService, IProductService productService, IFavoriteService favoriteService, IBlogService blogService)
        {
            _layoutService = layoutService;
            _productService = productService;
            _favoriteService = favoriteService;
            _blogService = blogService;
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBanner()
        {
            var model = _layoutService.GetBanner();
            return PartialView(model);
        }

        public ActionResult GetFeaturedCategory()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetCategoriesDesc();
            return PartialView(model);
        }

        public ActionResult GetProducts(int id, string meta)
        {
            ViewBag.meta = meta;
            var model = _productService.GetProductsByCategory(id);

            var email = User.Identity.Name;
            if (email != null)
            {
                var favoriteProducts = _favoriteService.GetFavoriteProductsByEmail(email);
                var products = favoriteProducts.Select(fp => fp).ToList();
                ViewBag.products = products.Select(x => x.id).ToList();
            }

            return PartialView(model);
        }

        public ActionResult GetListCategories()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetCategoriesDesc();
            return PartialView(model);
        }

        public ActionResult GetLastestProduct()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetLastestProduct();
            return PartialView(model);
        }

        public ActionResult GetTopProduct()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetTopProduct();
            return PartialView(model);
        }

        public ActionResult GetReviewProduct()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetReviewProduct();
            return PartialView(model);
        }

        public ActionResult GetBlogs()
        {
            var model = _blogService.GetAll();
            return PartialView(model);
        }
    }
}

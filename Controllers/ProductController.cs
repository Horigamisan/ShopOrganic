using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebDemo.Services.Interfaces;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WebDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;
        public ProductController(IUserService userService, IFavoriteService favoriteService, IProductService productService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
            _productService = productService;
        }
        // GET: Product
        public async Task<ActionResult> Index(string meta)
        {
            var model = await _productService.GetCategoryByMeta(meta);
            return View(model);
        }

        public ActionResult GetProductByCategory(int id, string title)
        {
            ViewBag.title = title;
            ViewBag.meta = "san-pham";
            
            var model = _productService.GetProductsByCategory(id);
            var email = User.Identity.Name;
            
            if (email != null)
            {
                var products = _favoriteService.GetFavoriteProductsByEmail(email);
                ViewBag.products = products.Select(x => x.id).ToList();
            }

            return PartialView(model);
        }

        public async Task<ActionResult> GetDetailProduct(int id)
        {
            var model = await _productService.GetProductById(id);
            var category = await _productService.GetCategoryByProductId(model.categoryid);
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userService.GetUserByEmail(User.Identity.Name).Id;
                ViewBag.isFavorite = await _favoriteService.GetFavoriteByProductIdAndUId(userId, id);
            }
            ViewBag.category = category;
            return View(model);
        }

        public ActionResult GetRelatedProduct(int id, int categoryId)
        {
            ViewBag.meta = "san-pham";
            var email = User.Identity.Name;

            if (email != null)
            {
                var products = _favoriteService.GetFavoriteProductsByEmail(email);
                ViewBag.products = products.Select(x => x.id).ToList();
            }
            
            var model = _productService.GetRelatedProducts(id, categoryId);
            return PartialView(model);
        }

        [Authorize]
        public ActionResult GetFavoriteProduct()
        {
            ViewBag.meta = "san-pham";
            var products = _favoriteService.GetFavoriteProductsByEmail(User.Identity.Name);
            return View(products);
        }

        public async Task<ActionResult> UpdateFavoriteProduct(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = 401, success = false});
            }

            var user = _userService.GetUserByEmail(User.Identity.Name);

            if (user != null)
            {
                bool updated = await _favoriteService.UpdateFavoriteProduct(user.Id, productId);

                // Trả về một phản hồi thành công
                /*if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("GetFavoriteProduct");*/
                return Json(new { status = 200, success = true,  isFavorite = updated}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Người dùng không tồn tại, trả về thông báo lỗi
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }
    }
}
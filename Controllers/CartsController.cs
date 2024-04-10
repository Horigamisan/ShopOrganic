using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CartsController(ICartService cartService, IUserService userService, IProductService productService)
        {
            _cartService = cartService;
            _userService = userService;
            _productService = productService;
        }
        
        // GET: Carts
        public ActionResult Index()
        {
            var emailUser = User.Identity.Name;
            var userId = _userService.GetUserByEmail(emailUser).Id;
            var model = _cartService.GetUserCart(userId);
            ViewBag.Products = _productService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCarts(List<CartItems> cartItems)
        {

            await _cartService.UpdateCarts(cartItems);

            // Trả về một phản hồi thành công (nếu cần)
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCartItem(int cartItemId)
        {
            await _cartService.RemoveCartItem(cartItemId);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> AddToCarts(int productId, int quantity)
        {
            var emailUser = User.Identity.Name;
            var userId = _userService.GetUserByEmail(emailUser).Id;
            await _cartService.AddToCart(userId, productId, quantity);

            return Json(new { success = true });
        }
    }
}
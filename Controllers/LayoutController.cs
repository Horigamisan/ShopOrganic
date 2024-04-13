using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Data.Entity;
using WebDemo.Services.Interfaces;
using System.Threading.Tasks;

namespace WebDemo.Controllers
{
    public class LayoutController : BaseController
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        private readonly ILayoutService _layoutService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;
        private readonly ICartService _cartService;
        private readonly IOrdersService _ordersService;

        public LayoutController(ILayoutService layoutService,
            IProductService productService,
            IUserService userService,
            IFavoriteService favoriteService,
            ICartService cartService,
            IOrdersService ordersService
            )
        {
            _layoutService = layoutService;
            _productService = productService;
            _userService = userService;
            _favoriteService = favoriteService;
            _cartService = cartService;
            _ordersService = ordersService;
        }

        // GET: Layout
        public ActionResult Index()
        {
            ViewBag.NumberPhone = _layoutService.GetPersonalInfo().phone;
            return View();
        }
        
        public ActionResult GetCategory()
        {
            ViewBag.meta = "san-pham";
            var model = _productService.GetCategoriesDesc();
            return PartialView(model);
        }
        
        public ActionResult GetMenu()
        {
            var model = _layoutService.GetMenu();
            return PartialView(model);
        }
        public ActionResult GetAboutInFooter()
        {
            var model = _layoutService.GetPersonalInfo();
            return PartialView(model);
        }

        public ActionResult GetUsefulLinksInFooter()
        {
            var model = _layoutService.GetUsefulLinks();
            return PartialView(model);
        }

        public ActionResult RenderCarts()
        {
            var emailUser = User.Identity.Name;
            
            if (!User.Identity.IsAuthenticated)
            {
                return PartialView();
            }
            
            var user = _userService.GetUserByEmail(emailUser);
            var model = _cartService.GetUserCart(user.Id);
            var orderHistory = _ordersService.GetUserOrdersByStatus(user.Id, "Đã thanh toán");
            var favProducts = _favoriteService.GetFavoriteProductsByEmail(emailUser);

            ViewBag.Products = _productService.GetAll().Count();
            ViewBag.FavoriteProducts = favProducts.Count();
            ViewBag.OrderHistory = orderHistory.Count();
            
            return PartialView(model);
        }
    }
}
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebDemo.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebDemo.Services.Interfaces;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IOrdersService _ordersService;
        private readonly IProductService _productService;

        public CheckoutController(IUserService userService, ICartService cartService, IOrdersService ordersService, IProductService productService)
        {
            _userService = userService;
            _cartService = cartService;
            _ordersService = ordersService;
            _productService = productService;
        }

        // GET: Checkout
        public ActionResult Index()
        {
            var model = GetCurrentCart();

            if (model.Count() == 0)
            {
                return RedirectToAction("Index", "Carts");
            }

            ViewBag.Products = _productService.GetAll();
            ViewBag.Carts = model;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(OrderViewModel orderPost)
        {
            var carts = GetCurrentCart();
            
            if (carts.Count() == 0)
            {
                return RedirectToAction("Index", "Carts");
            }
            
            orderPost.products = _productService.GetAll().ToList();
            orderPost.carts = carts.ToList();
            
            if (ModelState.IsValid == false)
            {
                return View(orderPost);
            }
            return Redirect(await CreateStripeCheckoutSession(orderPost));
        }

        public ActionResult OrderHistory()
        {
            var userId = _userService.GetUserByEmail(User.Identity.Name).Id;
            var model = _ordersService.GetUserOrdersHistory(userId);
            ViewBag.Products = _productService.GetAll();
            return View(model);
        }

        private string GetCurrentUserId()
        {
            var email = User.Identity.Name;
            return _userService.GetUserByEmail(email).Id;
        }

        private List<Carts> GetCurrentCart()
        {
            var userId = GetCurrentUserId();
            return _cartService.GetUserCart(userId).ToList();
        }

        private async Task<string> CreateStripeCheckoutSession(OrderViewModel orderPost)
        {
            var httpLink = "https://localhost:44356/thanh-toan";
            var carts = GetCurrentCart();

            var createOrder = await _ordersService.CreateNewOrder(orderPost, GetCurrentUserId(), carts);
            var orderId = createOrder.OrderID;

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "vnd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Thanh toán đơn hàng",
                            },
                            UnitAmount = (long)(carts.Sum(x => x.Price)),
                        },
                        Quantity = 1
                    },
                },
                Metadata = new Dictionary<string, string>()
                {
                    { "UserId", GetCurrentUserId() },
                    { "OrderId", orderId.ToString() }
                },
                Mode = "payment",
                SuccessUrl = httpLink,
                CancelUrl = httpLink,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session.Url;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> StripeWebhook()
        {
            // Điều này chỉ định email thành thông báo webhook có thể
            // gửi qua HTTP
            var json = await new StreamReader(HttpContext.Request.InputStream).ReadToEndAsync();

            try
            {
                // Verify the event by getting it from Stripe's API directly
                var stripeEvent = Stripe.EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "whsec_8f4e2201975bfda168148a2086d931fce664da596466bb791c7341fc20b34c7d");

                // Kiểm tra xem loại sự kiện có trùng khớp với một trong những loại mà chúng ta sẽ quản lý không
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    var userId = session.Metadata["UserId"];
                    var orderId = Convert.ToInt32(session.Metadata["OrderId"]);

                    var order = _ordersService.GetOrderByStatus(orderId, "Chưa thanh toán");
                    var carts = _cartService.GetUserCart(userId).ToList();

                    await _ordersService.CreateDetailOrder(order, carts);
                }

                // Return a 200 OK
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                // Return a 400 Bad Request error to Stripe if something goes wrong
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
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
using System.Reflection;
using PagedList;
using System.Globalization;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
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
            ViewBag.Tax = _ordersService.GetTax(model);
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
            ViewBag.Tax = _ordersService.GetTax(carts);

            if (ModelState.IsValid == false)
            {
                return View(orderPost);
            }
            return Redirect(await CreateStripeCheckoutSession(orderPost));
        }

        public ActionResult OrderHistory(int? page)
        {
            var userId = _userService.GetUserByEmail(User.Identity.Name).Id;
            var model = _ordersService.GetUserOrdersHistory(userId);
            ViewBag.Products = _productService.GetAll();
            int pageSize = 2;
            int pageNumber = page ?? 1;
            ViewBag.page = pageNumber;
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        
        public ActionResult GetOrderById(int id)
        {
            var order = _ordersService.GetOrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var orderRes = new OrderResModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate.GetValueOrDefault().ToString("dddd, dd/MM/yyyy", new CultureInfo("vi-VN")),
                UserID = order.UserID,
                NameCustomer = order.NameCustomer,
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                NoteOrder = order.NoteOrder,
                TotalAmount = order.TotalAmount,
                PaymentStatus = order.PaymentStatus,
                OrderProduct = order.OrderProduct.Select(o => new OrderProductResModel
                {
                    ProductId = o.ProductID ?? 1,
                    Meta = o.Products.meta,
                    ProductName = o.Products.name,
                    Quantity = o.Quantity.Value,
                    Price = o.Price.Value,
                    Image = o.Products.img
                }).ToList()
            };

            return Json(orderRes, JsonRequestBehavior.AllowGet);
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
            var httpLink = "https://shoporganic.azurewebsites.net/thanh-toan";
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
                            UnitAmount = (long)(carts.Sum(x => x.Price)) + (long)_ordersService.GetTax(carts)
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
                var stripeEvent = Stripe.EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "whsec_CuEDawNP9a8o10qUtxKLkWEmI6qpUZKH");

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
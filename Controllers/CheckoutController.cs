using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        private bool isPaymentSuccess = false;
        // GET: Checkout
        public ActionResult Index()
        {
            var model = GetCurrentCart();

            if (model.Count() == 0)
            {
                return RedirectToAction("Index", "Carts");
            }

            ViewBag.Products = db.Products.ToList();
            ViewBag.Carts = model;
            return View();
        }

        [HttpPost]
        public ActionResult Index(OrderViewModel orderPost)
        {
            var carts = GetCurrentCart();
            if (carts.Count() == 0)
            {
                return RedirectToAction("Index", "Carts");
            }
            orderPost.products = db.Products.ToList();
            orderPost.carts = carts.ToList();
            if (ModelState.IsValid == false)
            {

                return View(orderPost);
            }

            if (isPaymentSuccess)
            {
                SaveOrder(orderPost, carts);
            }

            return CreateStripeCheckoutSession(carts);
        }

        public ActionResult OrderHistory()
        {
            var userId = GetCurrentUserId();
            var model = db.Orders.Where(x => x.UserID == userId).OrderByDescending(x => x.OrderDate).ToList();
            ViewBag.Products = db.Products.ToList();
            return View(model);
        }

        private string GetCurrentUserId()
        {
            var email = User.Identity.Name;
            return db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault().Id;
        }

        private IQueryable<Carts> GetCurrentCart()
        {
            var userId = GetCurrentUserId();
            return db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");
        }

        private Orders SaveOrder(OrderViewModel orderPost, IQueryable<Carts> carts)
        {
            var order = new Orders
            {
                UserID = GetCurrentUserId(),
                NameCustomer = $"{orderPost.FirstName} {orderPost.LastName}",
                ShippingAddress = $"{orderPost.AddressLine1}, {orderPost.AddressLine2}, {orderPost.City}",
                PhoneNumber = orderPost.PhoneNumber,
                NoteOrder = orderPost.Notes,
                OrderDate = DateTime.Now,
                PaymentStatus = "Đang xử lý",
                TotalAmount = carts.Sum(x => x.Price) + 4000
            };
            db.Orders.Add(order);

            foreach (var item in carts)
            {
                var orderDetail = new OrderProduct
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                db.OrderProduct.Add(orderDetail);
                item.Status = "Đã thanh toán";
            }
            db.SaveChanges();
            return order;
        }

        private ActionResult CreateStripeCheckoutSession(IQueryable<Carts> carts)
        {
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
                            UnitAmount = 1000000,
                        },
                        Quantity = 1
                    },
                },
                Mode = "payment",
                SuccessUrl = Url.Action("CheckOutSuccess", "Checkout", null, Request.Url.Scheme),
                CancelUrl = Url.Action("CheckOutFailed", "Checkout", null, Request.Url.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return Redirect(session.Url);
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
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    // Đơn hàng đã được thanh toán, giờ ta có thể lưu lại thông tin hoặc cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
                    // Lưu ý: Giả định rằng chúng ta lưu ID đơn hàng trong metadata của phiên
                    isPaymentSuccess = true;
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
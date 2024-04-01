using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Checkout
        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var userId = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault().Id;
            var model = db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");

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
            var email = User.Identity.Name;
            var userId = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault().Id;
            var carts = db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");

            orderPost.products = db.Products.ToList();
            orderPost.carts = carts.ToList();

            if (ModelState.IsValid == false)
            {
                return View(orderPost);
            }

            var order = new Orders
            {
                UserID = userId,
                NameCustomer = orderPost.FirstName + " " + orderPost.LastName,
                ShippingAddress = orderPost.AddressLine1 + ", " + orderPost.AddressLine2 + ", " + orderPost.City,
                PhoneNumber = orderPost.PhoneNumber,
                NoteOrder = orderPost.Notes,
                OrderDate = DateTime.Now,
                PaymentStatus = "Đang xử lý",
                TotalAmount = carts.Sum(x => x.Price) + 4000
            };
            db.Orders.Add(order);
            db.SaveChanges();
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
            return RedirectToAction("OrderHistory", "Checkout");
        }

        public ActionResult OrderHistory()
        {
            var email = User.Identity.Name;
            var userId = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault().Id;
            var model = db.Orders.Where(x => x.UserID == userId).OrderByDescending(x => x.OrderDate).ToList();
            ViewBag.Products = db.Products.ToList();
            return View(model);
        }
    }
}
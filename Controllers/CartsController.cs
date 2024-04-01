using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Carts
        public ActionResult Index()
        {
            var emailUser = User.Identity.Name;
            var userId = db.AspNetUsers.Where(x => x.Email == emailUser).FirstOrDefault().Id;
            var model = db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");
            ViewBag.Products = db.Products.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCarts(List<CartItems> cartItems)
        {

            foreach (var item in cartItems)
            {
                var cartItem = db.Carts.Find(item.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity = item.Quantity;
                }
                cartItem.Price = (double?)(cartItem.Quantity * cartItem.Products.price);
            }

            db.SaveChanges();

            // Trả về một phản hồi thành công (nếu cần)
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveCartItem(int cartItemId)
        {
            var cartItem = db.Carts.Find(cartItemId);

            if (cartItem != null)
            {
                // Xóa cartItem khỏi cơ sở dữ liệu
                cartItem.Status = "Huỷ";
                db.SaveChanges();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddToCarts(int productId, int quantity)
        {
            var emailUser = User.Identity.Name;
            var userId = db.AspNetUsers.Where(x => x.Email == emailUser).FirstOrDefault().Id;
            var cartItem = db.Carts.Where(x => x.UserID == userId && x.ProductID == productId && x.Status != "Huỷ" && x.Status != "Đã thanh toán").FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.Price = (double?)(cartItem.Quantity * cartItem.Products.price);
            }
            else
            {
                cartItem = new Carts
                {
                    UserID = userId,
                    ProductID = productId,
                    Quantity = quantity,
                    Price = (double?)(quantity * db.Products.Find(productId).price),
                    Status = "Đang chờ"
                };
                db.Carts.Add(cartItem);
            }
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}
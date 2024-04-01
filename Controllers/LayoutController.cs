using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using System.Web.Mvc;

namespace WebDemo.Controllers
{
    public class LayoutController : Controller
    {
        ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Layout
        public ActionResult Index()
        {
            ViewBag.NumberPhone = db.PersonalInfo.Where(x => x.hide == false).FirstOrDefault().phone;
            return View();
        }
        
        public ActionResult getCategory()
        {
            ViewBag.meta = "san-pham";
            var model = db.ListCategories.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }
        
        public ActionResult getMenu()
        {
            var model = db.menu.Where(x => x.hide == false).OrderBy(x => x.order).ToList();
            return PartialView(model);
        }
        public ActionResult getAboutInFooter()
        {
            var model = db.PersonalInfo.Where(x => x.hide == false).OrderByDescending(x => x.order).FirstOrDefault();
            return PartialView(model);
        }

        public ActionResult getUsefulLinksInFooter()
        {
            var model = db.UsefulLinks.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }

        public ActionResult renderCarts()
        {
            var emailUser = User.Identity.Name;
            if (!User.Identity.IsAuthenticated)
            {
                return PartialView();
            }
            var userId = db.AspNetUsers.Where(x => x.Email == emailUser).FirstOrDefault().Id;
            var model = db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");
            var orderHistory = db.Orders.Where(x => x.UserID == userId);
                ViewBag.Products = db.Products.ToList();
            ViewBag.OrderHistory = orderHistory.Count();
            return PartialView(model);
        }
    }
}
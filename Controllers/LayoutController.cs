using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Data.Entity;
using WebDemo.Repository.Implementations;

namespace WebDemo.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
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
            var userId = _unitOfWork.UserRepo.GetUserByEmail(emailUser).Id;
            var model = db.Carts.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");
            var orderHistory = db.Orders.Where(x => x.UserID == userId && x.PaymentStatus == "Đã thanh toán").ToList();
            ViewBag.Products = _unitOfWork.ProductsRepo.GetAll().Count();

            var products = _unitOfWork.ProductsRepo.GetFavoriteProductsByEmail(emailUser);
            ViewBag.FavoriteProducts = products.Count();
            ViewBag.OrderHistory = orderHistory.Count();
            return PartialView(model);
        }
    }
}
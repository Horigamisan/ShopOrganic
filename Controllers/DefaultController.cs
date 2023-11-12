using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class DefaultController : Controller
    {
        ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult getBanner()
        {
            var model = db.Banner.Where(x => x.hide == false).FirstOrDefault();
            return PartialView(model);
        }
        
        public ActionResult getFeaturedCategory()
        {
            ViewBag.meta = "san-pham";
            var model = db.ListCategories.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }

        public ActionResult getProducts(long id, string meta)
        {
            ViewBag.meta = meta;
            var model = db.Products.Where(x => x.hide == false).Where(x => x.categoryid == id).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }

        public ActionResult getListCategories()
        {
            ViewBag.meta = "san-pham";
            var model = db.ListCategories.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }

        public ActionResult getLastestProduct()
        {
                ViewBag.meta = "san-pham";
                var model = db.Products.Where(x => x.hide == false && x.latest_product == true).OrderByDescending(x => x.order).ToList();
                return PartialView(model);
        }

        public ActionResult getTopProduct()
        {
                ViewBag.meta = "san-pham";
                var model = db.Products.Where(x => x.hide == false && x.top_product == true).OrderByDescending(x => x.order).ToList();
                return PartialView(model);
        }

        public ActionResult getReviewProduct()
        {
                ViewBag.meta = "san-pham";
                var model = db.Products.Where(x => x.hide == false && x.review_product == true).OrderByDescending(x => x.order).ToList();
                return PartialView(model);
        }

        public ActionResult getBlogs()
        {
            var model = db.Blogs.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
            return PartialView(model);
        }
    }
}

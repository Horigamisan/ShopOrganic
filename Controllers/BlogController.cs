using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class BlogController : Controller
    {
        ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getBlogs()
        {
            var model = db.Blogs.Where(x => x.hide == false);
            return PartialView(model);
        }

        public ActionResult getBlogCategories()
        {
            var model = db.BlogCategories.Where(x => x.hide == false).OrderByDescending(x => x.order);
            return PartialView(model);
        }

        public ActionResult getRecentNews()
        {
            var model = db.Blogs.Where(x => x.hide == false).OrderByDescending(x => x.order).Take(5);
            return PartialView(model);
        }

        public ActionResult getDetailBlog(long id)
        {
            var model = db.Blogs.Where(x => x.hide == false && x.id == id).FirstOrDefault();
            var category = db.BlogCategories.Where(x => x.id == model.categoryid).FirstOrDefault();
            ViewBag.category = category;
            //var detail = db.DetailProduct.Where(x => x.productid == id).FirstOrDefault();
            //ViewBag.detail = detail;
            return View(model);
        }

        public ActionResult getRelatedBlog(long id, int categoryid)
        {
            var model = db.Blogs.Where(x => x.hide == false).Where(x => x.id != id).ToList();
            return PartialView(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Product
        public ActionResult Index(string meta)
        {
            var model = db.ListCategories.Where(x => x.hide == false).Where(x => x.meta == meta).FirstOrDefault();
            return View(model);
        }

        public ActionResult getProductByCategory(long id, string title)
        {
            ViewBag.title = title;
            ViewBag.meta = "san-pham";
            var model = db.Products.Where(x => x.hide == false).Where(x => x.categoryid == id).ToList();
            return PartialView(model);
        }
        
        public ActionResult getDetailProduct(long id)
        {
            var model = db.Products.Where(x => x.hide == false && x.id == id).FirstOrDefault();
            var category = db.ListCategories.Where(x => x.id == model.categoryid).FirstOrDefault();
            ViewBag.category = category;
            //var detail = db.DetailProduct.Where(x => x.productid == id).FirstOrDefault();
            //ViewBag.detail = detail;
            return View(model);
        }

        public ActionResult getRelatedProduct(long id, int categoryid)
        {
            ViewBag.meta = "san-pham";
            var model = db.Products.Where(x => x.hide == false).Where(x => x.id != id && x.categoryid == categoryid).ToList();
            return PartialView(model);
        }
    }
}
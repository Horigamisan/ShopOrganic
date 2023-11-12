using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class ShopController : Controller
    {
        ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Shop
        public ActionResult Index(string sortBy)
        {
            ViewBag.meta = "san-pham";
            IEnumerable<Products> productList;
            switch (sortBy)
            {
                case "newProduct":
                    // Sắp xếp danh sách sản phẩm theo sản phẩm mới nhất
                    productList = db.Products.OrderByDescending(p => p.id);
                    break;
                case "oldProduct":
                    // Sắp xếp danh sách sản phẩm theo sản phẩm cũ nhất
                    productList = db.Products.OrderBy(p => p.id);
                    break;
                case "highPrice":
                    // Sắp xếp danh sách sản phẩm theo giá cao nhất
                    productList = db.Products.OrderByDescending(p => p.price);
                    break;
                case "lowPrice":
                    // Sắp xếp danh sách sản phẩm theo giá thấp nhất
                    productList = db.Products.OrderBy(p => p.price);
                    break;
                default:
                    // Mặc định sắp xếp theo sản phẩm mới nhất
                    productList = db.Products.OrderByDescending(p => p.id);
                    break;
            }
            ViewBag.Sort = sortBy;
            return View(productList);
        }

        public ActionResult ListCategory()
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
        
    }
}
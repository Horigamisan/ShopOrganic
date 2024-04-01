using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using PagedList;
using System.Data.SqlClient;
using WebDemo.Helper;

namespace WebDemo.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Shop
        public ActionResult Index( string currentFilter, string searchString, int? page, int? minPrice, int? maxPrice, string sortBy = "newProduct")
        {
            ViewBag.meta = "san-pham";
            IEnumerable<Products> productList;
            ViewBag.CurrentSort = sortBy;
            ViewBag.MinPrice = minPrice ?? 1000;
            ViewBag.MaxPrice = maxPrice ?? 500000;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

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

            if (!string.IsNullOrEmpty(searchString))
            {
                string newText = NormalizeTwoTextVN.Normalize(searchString);
                productList = productList.Where(p =>
                        (p.name != null && NormalizeTwoTextVN.Normalize(p.name).Contains(newText)) ||
                        (p.description != null && NormalizeTwoTextVN.Normalize(p.description).Contains(newText))
                        );
            }

            if (minPrice != null && maxPrice != null)
            {
                productList = productList.Where(p => p.price >= minPrice && p.price <= maxPrice);
            }



            ViewBag.Sort = sortBy;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.Page = pageNumber;
            ViewBag.TotalProduct = productList.Count();
            return View(productList.ToPagedList(pageNumber, pageSize));
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
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebDemo.Helper;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class BlogController : Controller
    {
        private readonly ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/Blog
        public ActionResult Index(int? page, string meta, string searchTxt)
        {
            ViewBag.page = page;
            ViewBag.meta = meta;
            ViewBag.searchTxt = searchTxt;
            var model = db.BlogCategories.Where(x => x.hide == false).Where(x => x.meta == meta).FirstOrDefault();
            return View(model);
        }

        public ActionResult getBlogs(string currentFilter, string searchTxt, int? page, int id, string title)
        {
            IEnumerable<Blogs> model = db.Blogs.Where(x => x.hide == false).OrderByDescending(x => x.order);

            if (searchTxt != null)
            {
                page = 1;
            }
            else
            {
                currentFilter = searchTxt;
            }
            ViewBag.searchTxt = searchTxt;

            if (id != 0 || title != "Tất cả")
            {
                model = db.Blogs.Where(x => x.hide == false).Where(x => x.categoryid == id).OrderByDescending(x => x.order);
            }

            if (!string.IsNullOrEmpty(searchTxt))
            {
                string newText = NormalizeTwoTextVN.Normalize(searchTxt);
                model = model.Where(p =>
                        (p.description != null && NormalizeTwoTextVN.Normalize(p.description).Contains(newText)) ||
                        (p.name != null && NormalizeTwoTextVN.Normalize(p.name).Contains(newText))
                        );
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            ViewBag.title = title;
            ViewBag.Page = pageNumber;

            return PartialView(model.ToPagedList(pageNumber, pageSize));
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
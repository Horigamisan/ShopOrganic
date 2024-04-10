using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebDemo.Helper;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ActionResult Index(int? page, string meta, string searchTxt)
        {
            ViewBag.page = page;
            ViewBag.meta = meta;
            ViewBag.searchTxt = searchTxt;
            var model = _blogService.GetCategoryByMeta(meta);
            return View(model);
        }

        public ActionResult GetBlogs(FilterBlogViewModel modelRequest)
        {
            IEnumerable<Blogs> model = _blogService.FilterBlog(modelRequest);

            int pageSize = 2;
            int pageNumber = (modelRequest.Page ?? 1);
            ViewBag.title = modelRequest.Title;
            ViewBag.Page = pageNumber;

            return PartialView(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetBlogCategories()
        {
            var model = _blogService.GetBlogCategories();
            return PartialView(model);
        }

        public ActionResult GetRecentNews()
        {
            var model = _blogService.GetRecentNews();
            return PartialView(model);
        }

        public ActionResult GetDetailBlog(int id)
        {
            var model = _blogService.GetBlogById(id);
            var category = _blogService.GetCategoryById(model?.categoryid);
            ViewBag.category = category;
            return View(model);
        }

        public ActionResult GetRelatedBlog(int id)
        {
            var model = _blogService.GetRelatedBlog(id);
            return PartialView(model);
        }
    }
}
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
using WebDemo.Repository.Interfaces;
using WebDemo.Repository.Implementations;

namespace WebDemo.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        // GET: Shop
        public ActionResult Index( string currentFilter, string searchString, int? page, int? minPrice, int? maxPrice, string sortBy = "newProduct")
        {
            ViewBag.meta = "san-pham";
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

            IEnumerable<Products> productList = _unitOfWork.ProductsRepo.GetProductOrderBy(sortBy, searchString, minPrice, maxPrice, page);

            var email = User.Identity.Name;
            if (email != null)
            {
                var products = _unitOfWork.ProductsRepo.GetFavoriteProductsByEmail(email);
                ViewBag.products = products.Select(x => x.id).ToList();
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
            var model = _unitOfWork.ProductsRepo.GetCategoriesDesc();
            return PartialView(model);
        }

        public ActionResult getLastestProduct()
        {
            ViewBag.meta = "san-pham";
            var model = _unitOfWork.ProductsRepo.GetLastestProducts();
            return PartialView(model);
        }

    }
}
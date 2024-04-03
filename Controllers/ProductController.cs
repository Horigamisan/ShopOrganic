using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

            var email = User.Identity.Name;
            if (email != null)
            {
                var favoriteProducts = db.Favorites.Where(i => i.AspNetUsers.Email == email).ToList();
                var products = favoriteProducts.Select(fp => fp.Products).ToList();
                ViewBag.products = products.Select(x => x.id).ToList();
            }

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

        public List<Products> GetProducts()
        {
            var email = User.Identity.Name;
            var favoriteProducts = db.Favorites.Where(i => i.AspNetUsers.Email == email).ToList();
            var products = favoriteProducts.Select(fp => fp.Products).ToList();

            return products;
        }

        [Authorize]
        public ActionResult GetFavoriteProduct()
        {
            var products = GetProducts();
            return View(products);
        }

        //update fav 
        [Authorize]
        public ActionResult UpdateFavoriteProduct(int productId, string returnUrl)
        {
            var email = User.Identity.Name;
            var user = db.AspNetUsers.SingleOrDefault(u => u.Email == email);

            if (user != null)
            {
                var favoriteProduct = db.Favorites.SingleOrDefault(fp => fp.UserID == user.Id && fp.ProductID == productId);

                if (favoriteProduct != null)
                {
                    // Nếu sản phẩm đã nằm trong danh sách yêu thích, hãy xóa nó
                    db.Favorites.Remove(favoriteProduct);
                }
                else
                {
                    // Nếu sản phẩm chưa nằm trong danh sách yêu thích, hãy thêm nó vào
                    var newFavoriteProduct = new Favorites
                    {
                        UserID = user.Id,
                        ProductID = productId
                    };
                    db.Favorites.Add(newFavoriteProduct);
                }

                // Lưu thay đổi
                db.SaveChanges();

                // Trả về một phản hồi thành công
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("GetFavoriteProduct");
            }
            else
            {
                // Người dùng không tồn tại, trả về thông báo lỗi
                return RedirectToAction("Index");
            }
        }
    }
}
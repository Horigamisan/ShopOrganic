using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebDemo.Models;
using WebDemo.Repository.Implementations;
using WebDemo.Repository.Interfaces;

namespace WebDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        // GET: Product
        public ActionResult Index(string meta)
        {
            var model = _unitOfWork.ProductsRepo.GetCategoryByMeta(meta);
            return View(model);
        }

        public ActionResult getProductByCategory(int id, string title)
        {
            ViewBag.title = title;
            ViewBag.meta = "san-pham";
            var model = _unitOfWork.ProductsRepo.GetProductsByCategory(id);
            var email = User.Identity.Name;
            
            if (email != null)
            {
                var products = _unitOfWork.ProductsRepo.GetFavoriteProductsByEmail(email);
                ViewBag.products = products.Select(x => x.id).ToList();
            }

            return PartialView(model);
        }

        public ActionResult getDetailProduct(int id)
        {
            var model = _unitOfWork.ProductsRepo.GetProductById(id);
            var category = _unitOfWork.ProductsRepo.GetCategoryByProductId(model.categoryid);
            ViewBag.category = category;
            return View(model);
        }

        public ActionResult getRelatedProduct(int id, int categoryId)
        {
            ViewBag.meta = "san-pham";
            var model = _unitOfWork.ProductsRepo.GetRelatedProducts(id, categoryId);
            return PartialView(model);
        }

        [Authorize]
        public ActionResult GetFavoriteProduct()
        {
            var products = _unitOfWork.ProductsRepo.GetFavoriteProductsByEmail(User.Identity.Name);
            return View(products);
        }

        //update fav 
        [Authorize]
        public ActionResult UpdateFavoriteProduct(int productId, string returnUrl)
        {
            var user = _unitOfWork.UserRepo.GetUserByEmail(User.Identity.Name);

            if (user != null)
            {
                var favoriteProduct = _unitOfWork.FavoriteRepo.GetFavoriteByProductIdAndUId(user.Id, productId);

                if (favoriteProduct != null)
                {
                    // Nếu sản phẩm đã nằm trong danh sách yêu thích, hãy xóa nó
                    _unitOfWork.FavoriteRepo.Remove(favoriteProduct);
                }
                else
                {
                    // Nếu sản phẩm chưa nằm trong danh sách yêu thích, hãy thêm nó vào
                    var newFavoriteProduct = new Favorites
                    {
                        UserID = user.Id,
                        ProductID = productId
                    };
                    _unitOfWork.FavoriteRepo.Add(newFavoriteProduct);
                }

                // Lưu thay đổi
                _unitOfWork.Complete();

                // Trả về một phản hồi thành công
                if (!string.IsNullOrEmpty(returnUrl))
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
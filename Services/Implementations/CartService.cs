using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class CartService : BaseService, ICartService
    {
        private readonly DbSet<Carts> _cartsRepo;
        public CartService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _cartsRepo = _unitOfWork.Repository<Carts>();

        }

        public IEnumerable<Carts> GetUserCart(string userId)
        {
            return _cartsRepo.Where(x => x.UserID == userId && x.Status != "Huỷ" && x.Status != "Đã thanh toán");
        }

        public async Task<bool> UpdateCarts(List<CartItems> cartItems)
        {
            foreach (var item in cartItems)
            {
                var cartItem = _cartsRepo.Find(item.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity = item.Quantity;
                    cartItem.Price = (double?)(cartItem.Quantity * cartItem.Products.price);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveCartItem(int cartItemId)
        {
            var cartItem = _cartsRepo.Find(cartItemId);

            if (cartItem != null)
            {
                cartItem.Status = "Huỷ";
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<Carts> AddToCart(string userId, int productId, int quantity)
        {
            var cartItem = _cartsRepo.Where(x => x.UserID == userId && x.ProductID == productId && x.Status != "Huỷ" && x.Status != "Đã thanh toán").FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.Price = (double?)(cartItem.Quantity * cartItem.Products.price);
            }
            else
            {
                cartItem = new Carts
                {
                    UserID = userId,
                    ProductID = productId,
                    Quantity = quantity,
                    Price = (double?)(quantity * _unitOfWork.Repository<Products>().Find(productId).price),
                    Status = "Chờ xác nhận"
                };
                _cartsRepo.Add(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();

            return cartItem;
        }
    }
}
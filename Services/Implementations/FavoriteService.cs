using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class FavoriteService : BaseService, IFavoriteService
    {
        private readonly DbSet<Favorites> _favoriteRepo;

        public FavoriteService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _favoriteRepo = unitOfWork.Repository<Favorites>();
        }

        public IEnumerable<Products> GetFavoriteProductsByEmail(string email)
        {
            var favoriteProducts = _favoriteRepo.Where(i => i.AspNetUsers.Email == email).ToList();
            var products = favoriteProducts.Select(fp => fp.Products).ToList();

            return products;
        }

        public async Task<Favorites> GetFavoriteByProductIdAndUId(string userId, int id)
        {
            return await _favoriteRepo.SingleOrDefaultAsync(fp => fp.UserID == userId && fp.ProductID == id);
        }

        public async Task<bool> UpdateFavoriteProduct(string userId, int id)
        {
            var favoriteProduct = await GetFavoriteByProductIdAndUId(userId, id);

            if (favoriteProduct != null)
            {
                // Nếu sản phẩm đã nằm trong danh sách yêu thích, hãy xóa nó
                _favoriteRepo.Remove(favoriteProduct);
            }
            else
            {
                // Nếu sản phẩm chưa nằm trong danh sách yêu thích, hãy thêm nó vào
                var newFavoriteProduct = new Favorites
                {
                    UserID = userId,
                    ProductID = id
                };
                _favoriteRepo.Add(newFavoriteProduct);
            }

            // Lưu thay đổi
            await _unitOfWork.SaveChangesAsync();

            return favoriteProduct == null;
        }

        public bool IsFavoriteById(int id)
        {
            return _favoriteRepo.Any(fp => fp.ProductID == id);
        }
    }
}
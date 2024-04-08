using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    }
}
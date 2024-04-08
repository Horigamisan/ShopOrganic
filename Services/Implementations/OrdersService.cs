using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class OrdersService : BaseService, IOrdersService
    {
        private readonly DbSet<Orders> _ordersRepo;
        public OrdersService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _ordersRepo = _unitOfWork.Repository<Orders>();
        }

        public IEnumerable<Orders> GetUserOrdersByStatus(string userId, string status)
        {
            return _ordersRepo.Where(x => x.UserID == userId && x.PaymentStatus == status).ToList();
        }
    }
}
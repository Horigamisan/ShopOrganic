using Stripe.Climate;
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
    public class OrdersService : BaseService, IOrdersService
    {
        private readonly DbSet<Orders> _ordersRepo;
        private readonly DbSet<OrderProduct> _ordersProductRepo;
        public OrdersService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _ordersRepo = _unitOfWork.Repository<Orders>();
            _ordersProductRepo = _unitOfWork.Repository<OrderProduct>();
        }

        public async Task<Orders> CreateNewOrder(OrderViewModel orderPost, string userId, List<Carts> carts)
        {
            var order = new Orders()
            {
                UserID = userId,
                NameCustomer = $"{orderPost.FirstName} {orderPost.LastName}",
                ShippingAddress = $"{orderPost.AddressLine1}, {orderPost.AddressLine2}, {orderPost.City}",
                PhoneNumber = orderPost.PhoneNumber,
                NoteOrder = orderPost.Notes,
                OrderDate = DateTime.Now,
                PaymentStatus = "Chưa thanh toán",
                TotalAmount = carts.Sum(x => x.Price) + 4000
            };

            _ordersRepo.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public Orders GetOrderByStatus(int orderId, string status)
        {
            return _ordersRepo.FirstOrDefault(x => x.OrderID == orderId && x.PaymentStatus == status);
        }

        public IEnumerable<Orders> GetUserOrdersHistory(string id)
        {
           return _ordersRepo.Where(x => x.UserID == id && x.PaymentStatus != "Chưa thanh toán" && x.PaymentStatus != "Đang xử lý").OrderByDescending(x => x.OrderDate).ToList();
        }

        public IEnumerable<Orders> GetUserOrdersByStatus(string userId, string status)
        {
            return _ordersRepo.Where(x => x.UserID == userId && x.PaymentStatus == status).ToList();
        }

        public async Task<int> CreateDetailOrder(Orders order, List<Carts> carts)
        {
            order.PaymentStatus = "Đã thanh toán";
            foreach (var item in carts)
            {
                var orderDetail = new OrderProduct
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _ordersProductRepo.Add(orderDetail);
                item.Status = "Đã thanh toán";
            }
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
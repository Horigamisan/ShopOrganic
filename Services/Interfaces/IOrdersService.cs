using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Orders> GetUserOrdersByStatus(string userId, string status);
        IEnumerable<Orders> GetUserOrdersHistory(string id);
        Orders GetOrderByStatus(int orderId, string status);
        Task<Orders> CreateNewOrder(OrderViewModel orderPost, string userId, List<Carts> cart);

        Task<int> CreateDetailOrder(Orders order, List<Carts> carts);
    }
}

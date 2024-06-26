﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface ICartService
    {
        IEnumerable<Carts> GetUserCart(string userId);
        Task<bool> UpdateCarts(List<CartItems> cartItems);
        Task<bool> RemoveCartItem(int cartItemId);
        Task<Carts> AddToCart(string userId, int productId, int quantity);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IFavoriteService
    {
        IEnumerable<Products> GetFavoriteProductsByEmail(string email);
        Task<Favorites> GetFavoriteByProductIdAndUId(string userId, int id);
        Task<bool> UpdateFavoriteProduct(string userId, int id);
        bool IsFavoriteById(int id);
    }
}

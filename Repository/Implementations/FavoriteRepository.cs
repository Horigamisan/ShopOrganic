using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using WebDemo.Repository.Interfaces;

namespace WebDemo.Repository.Implementations
{
    public class FavoriteRepository : GenericRepository<Favorites>, IFavoriteRepository
    {
        public FavoriteRepository(ShopOnlineEntities context) : base(context)
        {
        }

        public Favorites GetFavoriteByProductIdAndUId(string userId, int id)
        {
            return _context.Favorites.SingleOrDefault(fp => fp.UserID == userId && fp.ProductID == id);
        }
    }
}
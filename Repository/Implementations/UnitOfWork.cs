using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using WebDemo.Repository.Interfaces;

namespace WebDemo.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ShopOnlineEntities _context = new ShopOnlineEntities();
        private IProductRepository _productsRepo;
        private IUserRepository _userRepo;
        private IFavoriteRepository _favoriteRepo;

        public IProductRepository ProductsRepo
        {
            get
            {
                if (this._productsRepo == null)
                {
                    this._productsRepo = new ProductRepository(_context);
                }
                return _productsRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                if (this._userRepo == null)
                {
                    this._userRepo = new UserRepository(_context);
                }
                return _userRepo;
            }
        }

        public IFavoriteRepository FavoriteRepo
        {
            get
            {
                if (this._favoriteRepo == null)
                {
                    this._favoriteRepo = new FavoriteRepository(_context);
                }
                return _favoriteRepo;
            }
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
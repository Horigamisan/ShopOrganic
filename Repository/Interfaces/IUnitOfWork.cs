using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDemo.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductsRepo { get; }
        IUserRepository UserRepo { get; }
        IFavoriteRepository FavoriteRepo { get; }
        int Complete();
    }
}

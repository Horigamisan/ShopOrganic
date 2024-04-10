using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDemo.Services.Interfaces
{
    public interface IUnitOfWork
    {
        DbSet<TEntity> Repository<TEntity>() where TEntity : class;

        DbContext GetDbContext();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}

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
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;

        private bool _disposed = false;

        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var repository = _dbContext.Set<TEntity>();

                _repositories.Add(typeof(TEntity), repository);
            }

            return (DbSet<TEntity>)_repositories[typeof(TEntity)];
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
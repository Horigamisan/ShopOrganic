
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly DbSet<Blogs> _blogsRepo;
        public BlogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _blogsRepo = _unitOfWork.Repository<Blogs>();

        }

        public IEnumerable<Blogs> GetAll()
        {
            return _blogsRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebDemo.Helper;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly DbSet<Blogs> _blogsRepo;
        private readonly DbSet<BlogCategories> _blogCategoriesRepo;
        public BlogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _blogsRepo = _unitOfWork.Repository<Blogs>();
            _blogCategoriesRepo = _unitOfWork.Repository<BlogCategories>();
        }

        public IEnumerable<Blogs> GetAll()
        {
            return _blogsRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Blogs> FilterBlog(FilterBlogViewModel modelRequest)
        {
            IEnumerable<Blogs> model = _blogsRepo.Where(x => x.hide == false).OrderByDescending(x => x.order);

            if (modelRequest.Id != 0 || modelRequest.Title != "Tất cả")
            {
                model = _blogsRepo.Where(x => x.hide == false).Where(x => x.categoryid == modelRequest.Id).OrderByDescending(x => x.order);
            }

            if (!string.IsNullOrEmpty(modelRequest.SearchTxt))
            {
                string newText = NormalizeTwoTextVN.Normalize(modelRequest.SearchTxt);
                model = model.Where(p =>
                        (p.description != null && NormalizeTwoTextVN.Normalize(p.description).Contains(newText)) ||
                        (p.name != null && NormalizeTwoTextVN.Normalize(p.name).Contains(newText))
                        );
            }

            return model.ToList();
        }

        public IEnumerable<BlogCategories> GetBlogCategories()
        {
            return _blogCategoriesRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Blogs> GetRecentNews()
        {
            return _blogsRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).Take(5).ToList();
        }

        public Blogs GetBlogById(int id)
        {
            return _blogsRepo.Where(x => x.hide == false && x.id == id).FirstOrDefault();
        }

        public BlogCategories GetCategoryById(int? id)
        {
            return _blogCategoriesRepo.Where(x => x.id == id).FirstOrDefault();
        }

        public IEnumerable<Blogs> GetRelatedBlog(int id)
        {
            return _blogsRepo.Where(x => x.hide == false && x.id != id).OrderByDescending(x => x.order).Take(5).ToList();
        }

        public BlogCategories GetCategoryByMeta(string meta)
        {
            return _blogCategoriesRepo.Where(x => x.hide == false).Where(x => x.meta == meta).FirstOrDefault();
        }
    }
}
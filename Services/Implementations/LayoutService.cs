using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Services.Implementations
{
    public class LayoutService : BaseService, ILayoutService
    {
        private readonly DbSet<PersonalInfo> _pInfoRepo;
        private readonly DbSet<menu> _menuRepo;
        private readonly DbSet<UsefulLinks> _usefulLinks;
        private readonly DbSet<Banner> _bannerRepo;
        public LayoutService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _pInfoRepo = _unitOfWork.Repository<PersonalInfo>();
            _menuRepo = _unitOfWork.Repository<menu>();
            _usefulLinks = _unitOfWork.Repository<UsefulLinks>();
            _bannerRepo = _unitOfWork.Repository<Banner>();
        }

        public PersonalInfo GetPersonalInfo()
        {
            return _pInfoRepo.Where(x => x.hide == false).OrderByDescending(x => x.order).FirstOrDefault();
        }
        
        public IEnumerable<menu> GetMenu()
        {
            return _menuRepo.Where(x => x.hide == false).OrderBy(x => x.order).ToList();
        }

        public IEnumerable<UsefulLinks> GetUsefulLinks()
        {
            return _usefulLinks.Where(x => x.hide == false).OrderByDescending(x => x.order).ToList();
        }

        public IEnumerable<Banner> GetBanner()
        {
            return _bannerRepo.Where(x => x.hide == false);
        }
    }
}
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
    public class UserService : BaseService, IUserService
    {
        private readonly DbSet<AspNetUsers> _userRepository;
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.Repository<AspNetUsers>();
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return _userRepository.FirstOrDefault(u => u.Email == email);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Models;
using WebDemo.Repository.Interfaces;

namespace WebDemo.Repository.Implementations
{
    public class UserRepository : GenericRepository<AspNetUsers>, IUserRepository
    {
        public UserRepository(ShopOnlineEntities context) : base(context)
        {
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return _context.AspNetUsers.SingleOrDefault(u => u.Email == email);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<AspNetUsers>
    {
        AspNetUsers GetUserByEmail(string email);
    }
}

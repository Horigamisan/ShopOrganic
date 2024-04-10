using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IUserService
    {
        AspNetUsers GetUserByEmail(string email);
    }
}

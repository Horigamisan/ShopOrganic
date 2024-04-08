using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<Blogs> GetAll();
    }
}

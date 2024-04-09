using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDemo.Models;

namespace WebDemo.Services.Interfaces
{
    public interface ILayoutService
    {
        PersonalInfo GetPersonalInfo();
        IEnumerable<menu> GetMenu();
        IEnumerable<UsefulLinks> GetUsefulLinks();
        IEnumerable<Banner> GetBanner();
    }
}

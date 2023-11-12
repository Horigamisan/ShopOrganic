using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDemo.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DefaultController : Controller
    {
        // GET: admin/Default
        public ActionResult Index()
        {
            return View();
        }

    }
}
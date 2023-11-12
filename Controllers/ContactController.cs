using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class ContactController : Controller
    {
        ShopOnlineEntities db = new ShopOnlineEntities();
        // GET: Contact
        public ActionResult Index()
        {
            var model = db.PersonalInfo.Where(x => x.hide == false).OrderByDescending(x => x.order).FirstOrDefault();
            return View(model);
        }
    }
}
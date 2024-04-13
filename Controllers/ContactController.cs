using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using WebDemo.Services.Interfaces;

namespace WebDemo.Controllers
{
    public class ContactController : BaseController
    {
        private readonly ILayoutService _layoutService;
        public ContactController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        // GET: Contact
        public ActionResult Index()
        {
            var model = _layoutService.GetPersonalInfo();
            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDemo.Controllers
{
    public class ErrorHandlerController : BaseController
    {
        // GET: ErrorHandler
        public ActionResult Index(string error, int errorCode = 0)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["Error"] = error;
                ViewData["ErrorCode"] = errorCode;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }

        public ActionResult NotFound()
        {
            ViewData["Error"] = "Page not found";
            ViewData["ErrorCode"] = 404;
            return View();
        }
    }
}
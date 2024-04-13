using Microsoft.Owin.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDemo.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            Log.Error(filterContext.Exception, filterContext.Exception.Message);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler", new { error = filterContext.Exception.Message, errorCode = filterContext.HttpContext.Response.StatusCode.ToString() });
        }
    }
}
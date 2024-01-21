using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMAdmin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View("404");
        }

        public ActionResult Error500()
        {
            return View("500");
        }
    }
}
using CRM.BusinessEntities;
using CRMAdmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMAdmin.Controllers
{
    [Authorize]
    public class TestController : BaseController
    {
        public ActionResult Topics()
        {
            ViewBag.Message = "7th Grade, Math Topics";

            if (Session["cart"] == null)
            {
                Session["cart"] = new List<int>();
                Session["cart"] = new List<int>();
            }


            return View();
        }

        public ActionResult Questions()
        {
            ViewBag.Message = "7th Grade, Math, Proportional relationships";

            return View();
        }
        public ActionResult Review()
        {
            ViewBag.Message = "Review Test Questions";

            string valSubmit = Request.Form["Confirm"];
            if (valSubmit == "1")
                Response.Redirect("Download");

            return View();
        }
        public ActionResult Download()
        {
            ViewBag.Message = "Download Test Questions";

            return View();
        }

        public ActionResult UploadAnswer()
        {
            ViewBag.Message = "Grade Test Answers";

            return View();
        }

        public ActionResult Scores()
        {
            ViewBag.Message = "Test Score Summary";

            return View();
        }

        public ActionResult PastTests()
        {
            ViewBag.Message = "Past Tests";

            return View();
        }
    }
}
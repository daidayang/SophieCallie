using CRM.BusinessEntities;
using CRMAdmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMAdmin.Controllers
{
    public class GeneralController : BaseController
    {
        // GET: General
        [Authorize]
        public ActionResult Index()
        {
            #region role permissions
            if (Util.CheckAccess(SimpleSessionPersister.CurrentUser, UserAccessCheckPointEnum.RPT_General) == UserAccessRightEnum.None)
            {
                return RedirectToAction("About", "Home");
            }
            #endregion
            return View();
        }
    }
}
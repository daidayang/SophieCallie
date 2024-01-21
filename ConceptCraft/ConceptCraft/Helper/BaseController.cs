using CRM.BusinessEntities;
using CRM.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CRMAdmin.Helper
{
    public class BaseController : Controller
    {
        private static string BUILD_VERSION = string.Empty;
        public static string BuildVersion
        {
            get
            {
                if (string.IsNullOrEmpty(BUILD_VERSION))
                {
                    string version = System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];
                    if (string.IsNullOrEmpty(version))
                    {
                        version = DateTime.Now.Date.Ticks.ToString();
                    }

                    BUILD_VERSION = version;
                }

                return BUILD_VERSION;
            }
        }

        public byte[] ExportFile(DataTable dtExport)
        {

            byte[] contents = null;
            if (dtExport != null)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbTitle = new StringBuilder();
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    string title = dtExport.Columns[i].ColumnName.ToString();
                    sbTitle.Append(Util.EncodeColumn(title));
                    if (i < dtExport.Columns.Count - 1)
                    {
                        sbTitle.Append(",");
                    }
                }
                sb.Append(sbTitle);
                sb.Append(Environment.NewLine);

                StringBuilder sbContent = new StringBuilder();
                for (int i = 0; i < dtExport.Rows.Count; i++)
                {
                    for (int j = 0; j < dtExport.Columns.Count; j++)
                    {
                        string content = "";
                        if (dtExport.Columns[j].DataType.Name == "DateTime")
                        {
                            DateTime date;
                            if (DateTime.TryParse(dtExport.Rows[i][j].ToString(), out date))
                            {
                                content = date.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                content = dtExport.Rows[i][j].ToString();
                            }
                        }
                        else
                        {
                            content = dtExport.Rows[i][j].ToString();
                        }

                        sbContent.Append(Util.EncodeColumn(content));
                        if (j < dtExport.Columns.Count - 1)
                        {
                            sbContent.Append(",");
                        }
                    }
                    sb.Append(sbContent);
                    sb.Append(Environment.NewLine);
                    sbContent.Remove(0, sbContent.Length);
                }
                contents = Encoding.Default.GetBytes(sb.ToString());
            }
            return contents;
           
        }

        public bool IsPost
        {
            get
            {
                return Request.HttpMethod.ToUpper().Equals("POST");
            }
        }

        public bool IsGet
        {
            get
            {
                return Request.HttpMethod.ToUpper().Equals("GET");
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SimpleSessionPersister.CurrentUser != null)
            {
                filterContext.HttpContext.User = new CustomPrincipal(new CustomIdentity(SimpleSessionPersister.CurrentUser.FieldName, SimpleSessionPersister.CurrentUser));

                #region " Set User default language "
                try
                {
                    Lst_LanguageBll bllLanguage = new Lst_LanguageBll();
                    short clusterID = SimpleSessionPersister.CurrentUser.ClusterDB;
                    short languageID =Convert.ToInt16(SimpleSessionPersister.CurrentUser.Locale);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(bllLanguage.Select(clusterID,languageID).LCID);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(bllLanguage.Select(clusterID,languageID).LCID);
                }
                catch
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                }
                #endregion

                #region edit ssocookie

                HttpCookie authCookie = Request.Cookies["loginAuth"];

                if (authCookie != null)
                {
                    StringBuilder UserCookiesInfo = new StringBuilder();

                    string uName = SimpleSessionPersister.CurrentUser.FieldName;
                    if (SimpleSessionPersister.CurrentUser.FieldName.ToUpper().Contains("ADMIN"))
                    {
                        uName = SimpleSessionPersister.CurrentUser.FieldName + "_" + SimpleSessionPersister.CurrentUser.ClientID;
                    }
                    UserCookiesInfo.Append(uName).Append("*");
                    //Expiration timestamp
                    DateTime time = new DateTime(1970, 1, 1);
                    string timestamp = Convert.ToString((DateTime.UtcNow.AddMinutes(30).Ticks - time.Ticks) / 10000000);
                    UserCookiesInfo.Append(timestamp).Append("*");

                    string cookieMd5 = Util.GetMd5Hash(UserCookiesInfo + ConfigHelper.GetStringFromConfig("EncryCookieValue", ""));
                    authCookie.Value = UserCookiesInfo + cookieMd5;
                    authCookie.HttpOnly = true;
                    authCookie.Domain = ConfigHelper.GetStringFromConfig("SsoCookieDomain", "");
                    authCookie.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Add(authCookie);
                }
                else
                {
                    SimpleSessionPersister.Clear();
                    //clear cookie NET_SessionId
                    HttpCookie ASPNETCookie = new HttpCookie("ASP.NET_SessionId", "");
                    ASPNETCookie.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(ASPNETCookie);
                }

                #endregion
            }
            base.OnAuthorization(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AuthorizeAttribute), false) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AuthorizeAttribute), true))
            {
                if (SimpleSessionPersister.CurrentUser == null && filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false) == false)
                {
                    var controller = (BaseController)filterContext.Controller;
                    filterContext.Result = controller.RedirectToAction("Login", "Account");
                }
            }
            ViewBag.BuildVersion = BuildVersion;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //ViewBag.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            //ViewBag.ActionName = filterContext.ActionDescriptor.ActionName.ToLower();
            //base.OnActionExecuted(filterContext);
        }

    }
}
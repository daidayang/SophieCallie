using CRMAdmin.Controllers;
using CRMAdmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CRMAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static readonly log4net.ILog mylogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            LogHelper.SetConfig();
            LogHelper.LogInfo("CRMAdmin Start");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = false;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        {
                            routeData.Values.Add("action", "Error404");
                            break;
                        }
                    case 500:
                        {
                            routeData.Values.Add("action", "Error500");
                            break;
                        }

                    default:
                        {
                            routeData.Values.Add("action", "Index");
                            break;
                        }
                }
            }

            routeData.Values.Add("error", exception.Message);
            routeData.Values.Add("trace", exception.StackTrace);
            Server.ClearError();

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!mylogger.IsDebugEnabled)
                return;

            StringBuilder sb = new StringBuilder();
            HttpContext context = base.Context;

            try
            {
                if (context.Request != null && context.Request.Form != null && context.Request.Form.Keys != null && context.Request.Form.Keys.Count > 0)
                {
                    sb.Append("Form");
                    sb.Append(Environment.NewLine);
                    foreach (string s in context.Request.Form.Keys)
                    {
                        sb.AppendFormat("{0}:{1}{2}", s, context.Request.Form[s], Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }

                if (context.Request != null && context.Request.QueryString != null && context.Request.QueryString.AllKeys != null && context.Request.QueryString.AllKeys.Length > 0)
                {
                    bool AppendHeader = true;
                    foreach (string s in context.Request.QueryString.AllKeys)
                    {
                        if (string.Compare(s, "version") == 0)
                            continue;
                        if (AppendHeader)
                        {
                            sb.Append("QueryString");
                            sb.Append(Environment.NewLine);
                            AppendHeader = false;
                        }
                        sb.AppendFormat("{0}:{1}{2}", s, context.Request.QueryString[s], Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }

                if (context.Session != null && context.Session.Keys != null && context.Session.Keys.Count > 0)
                {
                    sb.Append("Session");
                    sb.Append(Environment.NewLine);
                    foreach (string s in Session.Keys)
                    {
                        sb.AppendFormat("{0}:{1}{2}", s, context.Session[s], Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
                if (sb.Length > 0)
                    mylogger.DebugFormat("{0}{1}{2}", context.Request.Url.ToString(), Environment.NewLine, sb.ToString());
            }
            catch (Exception ex)
            {
                mylogger.ErrorFormat("Application_BeginRequest() exception.  Error={0}, StackTrace={1}", ex.Message, ex.StackTrace);
            }
        }
    }
}

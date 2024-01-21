using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Helper
{
    public static class HtmlRequestHelper
    {
        public static string Id(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("id"))
            {
                return (string)routeValues["id"];
            }
            else if (HttpContext.Current.Request.QueryString.AllKeys.Contains("id"))
            {
                return HttpContext.Current.Request.QueryString["id"];
            }

            return string.Empty;
        }

        public static string Controller(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
            {
                return (string)routeValues["controller"];
            }

            return string.Empty;
        }

        public static string Action(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
            {
                return (string)routeValues["action"];
            }

            return string.Empty;
        }

        public static string FormatedPhone(this System.Web.Mvc.HtmlHelper htmlHelper, string phone)
        {
      
            return Util.FormatPhone(phone);

        }


    }
}
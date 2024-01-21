using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMAdmin.Models;

namespace CRMAdmin.Helper
{
    [Serializable()]
    public static class SimpleSessionPersister
    {
        static string userSessionVar = "currentuser";

        public static AppUser CurrentUser
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }
                var sessionVar = HttpContext.Current.Session[userSessionVar];
                if (sessionVar != null)
                {
                    return sessionVar as AppUser;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session[userSessionVar] = value;
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session[userSessionVar] = null;
            HttpContext.Current.Session.Abandon();
            // Delete the authentication ticket and sign out.

            System.Web.Security.FormsAuthentication.SignOut();

            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
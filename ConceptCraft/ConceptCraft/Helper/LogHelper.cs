using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Helper
{
    public class LogHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected LogHelper()
        {
        }

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void LogInfo(string info)
        {
            log.Info(info);
        }

        public static void LogError(string error)
        {
            log.Error(error);
        }

        public static void LogException(Exception ex)
        {
            string msg = (ex.InnerException == null) ? ex.ToString() : ex.InnerException.ToString();
            string s = string.Empty;
            try
            {
                string ip = string.Empty;

                if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables != null)
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                s = string.Format(@"{0}, {1} {2}", ip, HttpContext.Current.Request.RawUrl, msg);
            }
            catch
            {
            }
            LogHelper.LogError(s);
        }
    }

}
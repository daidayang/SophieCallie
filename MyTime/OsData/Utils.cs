using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

using System.Security.Principal;

namespace OsData
{
    public class Utils
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int HttpGet(string url, out string xmlResponse)
        {
            int rc = 0;
            xmlResponse = null;

            try
            {
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(url);
                loHttp.Timeout = 5000;      //  5 sec delay
                loHttp.Method = "GET";

                HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
                Encoding enc = Common.NoBomUTF8;
                StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
                xmlResponse = loResponseStream.ReadToEnd();
                loWebResponse.Close();
                loResponseStream.Close();
            }
            catch (Exception e)
            {
                log.WarnFormat("HttpGetPost: Error={0}, Resp={1}", e.Message, xmlResponse);
                rc = -1;
            }
            return rc;
        }

        /// <summary>
        /// Updates the ServicePointManager SecurityProtocol and returns the previous value.
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static SecurityProtocolType SetSecurityProtocol(SecurityProtocolType protocol)
        {
            SecurityProtocolType old = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = protocol;
            if (log.IsDebugEnabled)
                log.DebugFormat("SetSecurityProtocol: ServicePointManager.SecurityProtocol set to {0}", protocol.ToString().Replace(' ', '|').Trim('|'));
            return old;
        }

        /// <summary>
        /// Sets the security protocol
        /// </summary>
        /// <param name="protocol"></param>
        public static SecurityProtocolType SetSecurityProtocol(string protocol = "TLS11|TLS12")
        {
            if (string.IsNullOrEmpty(protocol))
            {
                // If we have a null input, use default values.
                return SetSecurityProtocol();
            }

            List<string> protocols = protocol.ToUpper().Split('|').ToList();
            SecurityProtocolType newType = 0;
            string finalType = "";
            foreach (string p in protocols)
            {
                switch (p)
                {
                    case "SSL3":
                        newType = newType | SecurityProtocolType.Ssl3;
                        finalType += p + "|";
                        log.Warn("SetSecurityProtocol: SSL3 requested; Possible PCI violation.");
                        break;

                    case "TLS":
                    case "TLS1":
                    case "TLS10":
                        newType = newType | SecurityProtocolType.Tls;
                        finalType += p + "|";
                        log.Warn("SetSecurityProtocol: TLS 1.0 requested; Possible PCI violation.");
                        break;

                    case "TLS11":
                        newType = newType | SecurityProtocolType.Tls11;
                        finalType += p + "|";
                        break;

                    // .NET 4.5 and higher
                    case "TLS12":
                        newType = newType | SecurityProtocolType.Tls12;
                        finalType += p + "|";
                        break;

                        /*
											// .NET 4.7 and higher
											case "TLS13":
												newType = newType | SecurityProtocolType.Tls13;
												finalType += p + "|";
												break;
						*/
                }
            }

            if (newType != 0)
            {
                return SetSecurityProtocol(newType);
            }

            // Request defined no valid types. Use default.
            return SetSecurityProtocol();
        }
    }

    public static class Common
    {
        private static System.Text.Encoding utf8Encoding;
        public static System.Text.Encoding NoBomUTF8
        {
            get
            {
                if (utf8Encoding == null) utf8Encoding = new System.Text.UTF8Encoding(false);
                return utf8Encoding;
            }
        }
    }

    public class NativeMethods
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
                                            int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, out IntPtr DuplicateTokenHandle);

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int SecurityImpersonation = 2;
    }

}

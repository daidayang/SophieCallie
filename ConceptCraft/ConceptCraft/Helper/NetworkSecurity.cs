using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;

namespace CRMAdmin.Helper
{
    public enum LogonType : int
    {
        LOGON32_LOGON_INTERACTIVE = 2,
        LOGON32_LOGON_NETWORK = 3,
        LOGON32_LOGON_BATCH = 4,
        LOGON32_LOGON_SERVICE = 5,
        LOGON32_LOGON_UNLOCK = 7,
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,	// Only for Win2K or higher
        LOGON32_LOGON_NEW_CREDENTIALS = 9		// Only for Win2K or higher
    };

    public enum LogonProvider : int
    {
        LOGON32_PROVIDER_DEFAULT = 0,
        LOGON32_PROVIDER_WINNT35 = 1,
        LOGON32_PROVIDER_WINNT40 = 2,
        LOGON32_PROVIDER_WINNT50 = 3
    };

    class SecuUtil32
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr TokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
    }

    /// <summary>
    /// Summary description for NetworkSecurity.
    /// </summary>
    public class NetworkSecurity
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NetworkSecurity()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static WindowsImpersonationContext ImpersonateUser(string strDomain,
                                             string strLogin,
                                             string strPwd,
                                             LogonType logonType,
                                             LogonProvider logonProvider)
        {
            IntPtr tokenHandle = new IntPtr(0);
            IntPtr dupeTokenHandle = new IntPtr(0);
            try
            {
                const int SecurityImpersonation = 2;

                tokenHandle = IntPtr.Zero;
                dupeTokenHandle = IntPtr.Zero;

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = SecuUtil32.LogonUser(
                    strLogin,
                    strDomain,
                    strPwd,
                    (int)logonType,
                    (int)logonProvider,
                    ref tokenHandle);
                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    string strErr = String.Format("LogonUser failed with error code : {0}", ret);
                    throw new ApplicationException(strErr, null);
                }

                bool retVal = SecuUtil32.DuplicateToken(tokenHandle, SecurityImpersonation, ref dupeTokenHandle);
                if (false == retVal)
                {
                    SecuUtil32.CloseHandle(tokenHandle);
                    throw new ApplicationException("Failed to duplicate token", null);
                }

                // The token that is passed to the following constructor must 
                // be a primary token in order to use it for impersonation.
                WindowsIdentity newId = new WindowsIdentity(dupeTokenHandle);
                WindowsImpersonationContext impersonatedUser = newId.Impersonate();

                if (log.IsDebugEnabled)
                    log.DebugFormat("Successful impersonate to {0}", newId.Name);

                return impersonatedUser;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }

            //return null;
        }
    }
}
using CRMAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CRMAdmin.Helper
{
    public class CustomIdentity : System.Security.Principal.IIdentity
    {
        public CustomIdentity(string name, AppUser user)
        {
            this.Name = name;
            this.Detail = user;
        }

        #region Identity Members

        public string AuthenticationType
        {
            get
            {
                return "Custom";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.Name);
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public AppUser Detail
        {
            get;
            private set;
        }

        #endregion
    }
}
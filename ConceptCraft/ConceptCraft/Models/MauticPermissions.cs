using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Models
{
    public class MauticPermissions
    {
        private string _PermissionName;
        private int _AccessType;

        public string PermissionName
        {
            get
            {
                return _PermissionName;
            }

            set
            {
                _PermissionName = value;
            }
        }

        public int AccessType
        {
            get
            {
                return _AccessType;
            }

            set
            {
                _AccessType = value;
            }
        }
    }
}
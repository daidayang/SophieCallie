using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessEntities
{
    [Serializable()]
    public class AdminUserAccessInfo
    {
        private System.Int16 _EntityID;
        private System.Int32 _UserAccessRoleID;
        private UserAccessCheckPointEnum _CheckPoint;
        private UserAccessRightEnum _AccessRight;


        #region GETs and SETs

        public System.Int16 EntityID
        {
            get
            {
                return _EntityID;
            }

            set
            {
                _EntityID = value;
            }
        }

        public System.Int32 UserAccessRoleID
        {
            get
            {
                return _UserAccessRoleID;
            }

            set
            {
                _UserAccessRoleID = value;
            }
        }

        public UserAccessCheckPointEnum CheckPoint
        {
            get
            {
                return _CheckPoint;
            }

            set
            {
                _CheckPoint = value;
            }
        }

        public UserAccessRightEnum AccessRight
        {
            get
            {
                return _AccessRight;
            }

            set
            {
                _AccessRight = value;
            }
        }

        #endregion
    }
}

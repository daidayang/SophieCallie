using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{

    public partial class UserAccessInfo
    {

        public const string SQLSELECT = "u.AccessID,u.UserID,u.ClientID,u.UserRoleID";
        public const string SQLINSERTCOLUMNS = "UserID,ClientID,UserRoleID";

        #region Database fields
        private System.Int32 _AccessID;
        private System.Int32 _UserID;
        private System.Int16 _ClientID;
        private System.Int32 _UserRoleID;
        #endregion

        #region GETs and SETs

        public System.Int32 AccessID
        {
            get { return _AccessID; }
            set { _AccessID = value; }
        }

        public System.Int32 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public System.Int16 ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; }
        }

        public System.Int32 UserRoleID
        {
            get { return _UserRoleID; }
            set { _UserRoleID = value; }
        }
        #endregion



        public static UserAccessInfo LoadDbRecord(IDataReader rdr)
        {
            UserAccessInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new UserAccessInfo();
                obj.AccessID = rdr.GetInt32(0);
                obj.UserID = rdr.GetInt32(1);
                obj.ClientID = rdr.GetInt16(2);
                obj.UserRoleID = rdr.GetInt32(3);
            }
            return obj;
        }
        public static List<UserAccessInfo> LoadDbRecords(IDataReader rdr)
        {
            List<UserAccessInfo> ret = new List<UserAccessInfo>();

            while (true)
            {
                UserAccessInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{

    public partial class UserRoleDetailInfo
    {

        public const string SQLSELECT = "u.UserRoleID,u.CheckPointID,u.AccessType";
        public const string SQLINSERTCOLUMNS = "UserRoleID,CheckPointID,AccessType";

        #region Database fields
        private System.Int32 _UserRoleID;
        private System.Int16 _CheckPointID;
        private System.Int32 _AccessType;
        #endregion

        #region GETs and SETs

        public System.Int32 UserRoleID
        {
            get { return _UserRoleID; }
            set { _UserRoleID = value; }
        }

        public System.Int16 CheckPointID
        {
            get { return _CheckPointID; }
            set { _CheckPointID = value; }
        }

        public System.Int32 AccessType
        {
            get { return _AccessType; }
            set { _AccessType = value; }
        }
        #endregion



        public static UserRoleDetailInfo LoadDbRecord(IDataReader rdr)
        {
            UserRoleDetailInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new UserRoleDetailInfo();
                obj.UserRoleID = rdr.GetInt32(0);
                obj.CheckPointID = rdr.GetInt16(1);
                obj.AccessType = rdr.GetInt32(2);
            }
            return obj;
        }
        public static List<UserRoleDetailInfo> LoadDbRecords(IDataReader rdr)
        {
            List<UserRoleDetailInfo> ret = new List<UserRoleDetailInfo>();

            while (true)
            {
                UserRoleDetailInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

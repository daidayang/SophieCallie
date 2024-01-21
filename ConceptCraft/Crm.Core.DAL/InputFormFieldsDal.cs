using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess
{
   public partial class InputFormFieldsDal:SqlBase
    {
        #region SQL Parameters

        private const string PARAM_REMOTEIP = "@IP";
        private const string PARAM_SEARCHTEXT = "@SearchText";

        #endregion

        #region SQL Statements 

        private const string SQL_USERLOGIN = "usp_User_Login";

        private const string SQL_UserSelectByUsername = "usp_User_SelectByUsername";
        private const string SQL_UserModifyPassword = "usp_UserModifyPassword";

        private const string usp_UserModifyPasswordLog_InsertUpdateDelete = "usp_UserModifyPasswordLog_InsertUpdateDelete";
        private const string SQL_LoyaltyProgramSelByUserID = "LoyaltyProgram_SelByUserID";
        private const string SQL_UserSelectByUserId = "InputFormFields_SelByUserID";
        private const string SQL_InputFormFields_SearchBySuperUser = "InputFormFields_SearchBySuperUser";
        
        #endregion

        public InputFormFieldsInfo UserLogin(string username, string password, string ip)
        {
            InputFormFieldsInfo ret = null;

            SqlParameter[] Params = GetParameters_UserLogin();
            Params[0].Value = username;
            Params[1].Value = password;
            Params[2].Value = ip;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_USERLOGIN, Params))
            {
                ret = InputFormFieldsInfo.LoadDbRecord(rdr);
            }
            return ret;
        }

        public InputFormFieldsInfo UserSelectByUsername(string username)
        {
            InputFormFieldsInfo ret = null;

            SqlParameter[] Params = new SqlParameter[] {
                                               new SqlParameter(PARAM_FIELDNAME,SqlDbType.VarChar, 32)
                                           };
            Params[0].Value = username;
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_UserSelectByUsername, Params))
            {
                ret = InputFormFieldsInfo.LoadDbRecord(rdr);
            }
            return ret;
        }

        public List<UserAccessInfo> GetUserAccess(int userID)
        {
            List<UserAccessInfo> ret = null;

            const string sql = "SELECT " + UserAccessInfo.SQLSELECT + " FROM UserAccess u WHERE u.UserID=@UserID";
            SqlParameter[] Params = new SqlParameter[] {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
            };
            Params[0].Value = userID;
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params))
            {
                ret = UserAccessInfo.LoadDbRecords(rdr);
            }
            return ret;
        }

        public List<UserRoleDetailInfo> GetUserAccessRoleDetails(int userID, short clientID)
        {

            List<UserRoleDetailInfo> ret = new List<UserRoleDetailInfo>();
            const string sql = "SELECT " + UserRoleDetailInfo.SQLSELECT + @"
FROM UserAccess ua JOIN UserRoleDetail u ON ua.UserRoleID=u.UserRoleID
WHERE ua.UserID=@UserID AND ua.ClientID=@ClientID";

            SqlParameter[] Params = new SqlParameter[] {
                    new SqlParameter("@UserID", SqlDbType.Int, 4),
                    new SqlParameter("@ClientID",  SqlDbType.SmallInt, 2)
            };
            Params[0].Value = userID;
            Params[1].Value = clientID;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params))
            {
                ret = UserRoleDetailInfo.LoadDbRecords(rdr);
            }
            return ret;
        }

        public List<LoyaltyProgramInfo> SelByUserID(int userid)
        {
            List<LoyaltyProgramInfo> ret = null;
            SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@UserID",SqlDbType.Int,4)
            };
            param[0].Value = userid;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_LoyaltyProgramSelByUserID, param))
            {
                ret = LoyaltyProgramInfo.LoadDbRecords(rdr);
            }
            return ret;
        }

        public InputFormFieldsInfo UserSelectByUserID(int userId)
        {
            InputFormFieldsInfo ret = null;

            SqlParameter[] Params = new SqlParameter[] {
                                               new SqlParameter(PARAM_USERID,SqlDbType.Int, 4)
                                           };
            Params[0].Value = userId;
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_UserSelectByUserId, Params))
            {
                ret = InputFormFieldsInfo.LoadDbRecord(rdr);
            }
            return ret;
        }

        public List<InputFormFieldsInfo> SearchPagedBySuperUser(string searchText)
        {
            List<InputFormFieldsInfo> List = new List<InputFormFieldsInfo>();
          
            SqlParameter[] Params = GetParameters_SearchBySuperUser();
          
            Params[0].Value = searchText;
            
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_InputFormFields_SearchBySuperUser, Params))
            {
                List = InputFormFieldsInfo.LoadDbRecords(rdr);               
            }
          
            return List;
        }
        #region Build Parameters

        private static SqlParameter[] GetParameters_UserLogin()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_USERLOGIN);

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter(PARAM_FIELDNAME, SqlDbType.VarChar, 32),
                    new SqlParameter(PARAM_ACCESS,  SqlDbType.VarChar, 128),
                    new SqlParameter(PARAM_REMOTEIP,    SqlDbType.VarChar, 16)
                };
                SQLHelper.CacheParameters(SQL_USERLOGIN, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetParameters_SearchBySuperUser()
        {
            SqlParameter[] parameter = SQLHelper.GetCacheParameters(SQL_InputFormFields_SearchBySuperUser);

            if (parameter == null)
            {
                parameter = new SqlParameter[] {                                              
                                               new SqlParameter(PARAM_SEARCHTEXT,SqlDbType.NVarChar,255),
                                              
                                           };
                SQLHelper.CacheParameters(SQL_InputFormFields_SearchBySuperUser, parameter);
            }

            return parameter;
        }

        #endregion


    }
}

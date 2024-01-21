using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using CRM.BusinessEntities;

namespace CRM.DataAccess
{
    public partial class InputFormFieldsDal : SqlBase
    {

		#region SQL Statements

	    private const string SQL_INSERT = "InputFormFields_add";
     private const string SQL_SELECT = "InputFormFields_Sel";
     private const string SQL_UPDATE = "InputFormFields_edt";
		private const string SQL_DELETE = "InputFormFields_del";
		private const string SQL_DEACTIVATE = "InputFormFields_DeActivate";



		#endregion

        #region Constructors

        public InputFormFieldsDal() { }

        public InputFormFieldsDal(SqlConnection conn) : base(conn) { }

        public InputFormFieldsDal(SqlConnection conn, SqlTransaction trans) : base(conn, trans) { }

        #endregion

         #region SQL Parameters

         private const string PARAM_FIELDID = "@FieldID";
         private const string PARAM_FIELDTYPE = "@FieldType";
         private const string PARAM_FIELDNAME = "@FieldName";
         private const string PARAM_ACCESS = "@Access";
         private const string PARAM_EMAIL = "@Email";
         private const string PARAM_LOCALE = "@Locale";
         private const string PARAM_NEXTEXPIRE = "@NextExpire";
         private const string PARAM_LOCKUNTILTIME = "@LockUntilTime";
         private const string PARAM_LASTUSETIME = "@LastUseTime";
         private const string PARAM_DATECREATED = "@DateCreated";
         private const string PARAM_DATEDEACTIVATED = "@DateDeactivated";
         private const string PARAM_ACTIVE = "@Active";
         private const string PARAM_FIRSTNAME = "@Firstname";
         private const string PARAM_LASTNAME = "@Lastname";
         private const string PARAM_SETTING = "@Setting";
         private const string PARAM_ACTIVEONLY = "@ActiveOnly";
         private const string PARAM_USERID = "@UserID";
        #endregion


        #region	UPDATEs


        public int Insert( CRM.BusinessEntities.InputFormFieldsInfo inputformfields )
		{
			SqlParameter[] Param_Insert = GetParameters_Insert();
            Param_Insert[0].Value = inputformfields.FieldType;
            if ( inputformfields.FieldName == null )
                Param_Insert[1].Value = string.Empty;
            else
                Param_Insert[1].Value = inputformfields.FieldName;
            if ( inputformfields.Access == null )
                Param_Insert[2].Value = string.Empty;
            else
                Param_Insert[2].Value = inputformfields.Access;
            if ( inputformfields.Email == null )
                Param_Insert[3].Value = string.Empty;
            else
                Param_Insert[3].Value = inputformfields.Email;
            if ( inputformfields.Locale == null )
                Param_Insert[4].Value = string.Empty;
            else
                Param_Insert[4].Value = inputformfields.Locale;
            Param_Insert[5].Value = inputformfields.NextExpire;
            Param_Insert[6].Value = inputformfields.LockUntilTime;
            Param_Insert[7].Value = inputformfields.LastUseTime;
            Param_Insert[8].Value = inputformfields.DateCreated;
            Param_Insert[9].Value = inputformfields.DateDeactivated;
            Param_Insert[10].Value = inputformfields.Active;
            if ( inputformfields.Firstname == null )
                Param_Insert[11].Value = string.Empty;
            else
                Param_Insert[11].Value = inputformfields.Firstname;
            if ( inputformfields.Lastname == null )
                Param_Insert[12].Value = string.Empty;
            else
                Param_Insert[12].Value = inputformfields.Lastname;
            Param_Insert[13].Value = inputformfields.Setting;
            SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_INSERT, Param_Insert);
            return (int)(Param_Insert[14].Value);
		}


        public int Update( CRM.BusinessEntities.InputFormFieldsInfo inputformfields )
		{
			SqlParameter[] Param_Update = GetParameters_Update();
            Param_Update[0].Value = inputformfields.FieldID;
            Param_Update[1].Value = inputformfields.FieldType;
            if ( inputformfields.FieldName == null )
                Param_Update[2].Value = string.Empty;
            else
                Param_Update[2].Value = inputformfields.FieldName;
            if ( inputformfields.Access == null )
                Param_Update[3].Value = string.Empty;
            else
                Param_Update[3].Value = inputformfields.Access;
            if ( inputformfields.Email == null )
                Param_Update[4].Value = string.Empty;
            else
                Param_Update[4].Value = inputformfields.Email;
            if ( inputformfields.Locale == null )
                Param_Update[5].Value = string.Empty;
            else
                Param_Update[5].Value = inputformfields.Locale;
            Param_Update[6].Value = inputformfields.NextExpire;
            Param_Update[7].Value = inputformfields.LockUntilTime;
            Param_Update[8].Value = inputformfields.LastUseTime;
            Param_Update[9].Value = inputformfields.DateCreated;
            Param_Update[10].Value = inputformfields.DateDeactivated;
            Param_Update[11].Value = inputformfields.Active;
            if ( inputformfields.Firstname == null )
                Param_Update[12].Value = string.Empty;
            else
                Param_Update[12].Value = inputformfields.Firstname;
            if ( inputformfields.Lastname == null )
                Param_Update[13].Value = string.Empty;
            else
                Param_Update[13].Value = inputformfields.Lastname;
            Param_Update[14].Value = inputformfields.Setting;
            return SQLHelper.ExecuteNonQuery( base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_UPDATE, Param_Update);
		}


		public int Delete( System.Int32 fieldID )
		{
            const string sql = "DELETE FROM InputFormFields WHERE [FieldID]=@FieldID "; 

            SqlParameter[] Params = GetParameters_Delete();
			            Params[0].Value = fieldID;


            return SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params);
		}



		public int DeActivate( System.Int32 fieldID )
		{
            const string sql = "UPDATE InputFormFields SET Active=0 WHERE [FieldID]=@FieldID "; 

            SqlParameter[] Params = GetParameters_Delete();
			            Params[0].Value = fieldID;


            return SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params);
		}


            		#endregion

		   #region SELECTs


        public CRM.BusinessEntities.InputFormFieldsInfo Select(System.Int32 fieldID)
        {
            CRM.BusinessEntities.InputFormFieldsInfo ret = null;

            SqlParameter[] Params = GetParameters_Select();
                        Params[0].Value = fieldID;


            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_SELECT, Params)) 
            {
                ret = CRM.BusinessEntities.InputFormFieldsInfo.LoadDbRecord(rdr);
            }
            return ret;
        }


        #endregion

		#region Build Parameters


		private static SqlParameter[] GetParameters_Insert() 
		{
			SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_INSERT);

			if (parms == null) 
			{
				parms = new SqlParameter[] {
                                               new SqlParameter(PARAM_FIELDTYPE,			SqlDbType.TinyInt, 1),
                                               new SqlParameter(PARAM_FIELDNAME,			SqlDbType.NVarChar, 32),
                                               new SqlParameter(PARAM_ACCESS,			SqlDbType.VarChar, 128),
                                               new SqlParameter(PARAM_EMAIL,			SqlDbType.VarChar, 64),
                                               new SqlParameter(PARAM_LOCALE,			SqlDbType.VarChar, 8),
                                               new SqlParameter(PARAM_NEXTEXPIRE,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_LOCKUNTILTIME,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_LASTUSETIME,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_DATECREATED,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_DATEDEACTIVATED,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_ACTIVE,			SqlDbType.Bit, 1),
                                               new SqlParameter(PARAM_FIRSTNAME,			SqlDbType.NVarChar, 64),
                                               new SqlParameter(PARAM_LASTNAME,			SqlDbType.NVarChar, 64),
                                               new SqlParameter(PARAM_SETTING,			SqlDbType.Int, 4),

                                               new SqlParameter("@RETURN_VALUE", SqlDbType.Int)
										   };
                parms[14].Direction = ParameterDirection.ReturnValue;
				SQLHelper.CacheParameters(SQL_INSERT, parms);
			}
			return parms;
		}


		private static SqlParameter[] GetParameters_Select() 
		{
			SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_SELECT);

			if (parms == null) 
			{
				parms = new SqlParameter[] {
                                               new SqlParameter(PARAM_FIELDID,			SqlDbType.Int, 4)
										   };
				SQLHelper.CacheParameters(SQL_SELECT, parms);
			}
			return parms;
		}


		private static SqlParameter[] GetParameters_Update() 
		{
			SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_UPDATE);

			if (parms == null) 
			{
				parms = new SqlParameter[] {
                                               new SqlParameter(PARAM_FIELDID,			SqlDbType.Int, 4),
                                               new SqlParameter(PARAM_FIELDTYPE,			SqlDbType.TinyInt, 1),
                                               new SqlParameter(PARAM_FIELDNAME,			SqlDbType.NVarChar, 32),
                                               new SqlParameter(PARAM_ACCESS,			SqlDbType.VarChar, 128),
                                               new SqlParameter(PARAM_EMAIL,			SqlDbType.VarChar, 64),
                                               new SqlParameter(PARAM_LOCALE,			SqlDbType.VarChar, 8),
                                               new SqlParameter(PARAM_NEXTEXPIRE,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_LOCKUNTILTIME,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_LASTUSETIME,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_DATECREATED,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_DATEDEACTIVATED,			SqlDbType.SmallDateTime, 4),
                                               new SqlParameter(PARAM_ACTIVE,			SqlDbType.Bit, 1),
                                               new SqlParameter(PARAM_FIRSTNAME,			SqlDbType.NVarChar, 64),
                                               new SqlParameter(PARAM_LASTNAME,			SqlDbType.NVarChar, 64),
                                               new SqlParameter(PARAM_SETTING,			SqlDbType.Int, 4)
										   };
				SQLHelper.CacheParameters(SQL_UPDATE, parms);
			}
			return parms;
		}


		private static SqlParameter[] GetParameters_Delete() 
		{
			SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_DELETE);

			if (parms == null) 
			{
				parms = new SqlParameter[] {
                                               new SqlParameter(PARAM_FIELDID,			SqlDbType.Int, 4)
										   };
				SQLHelper.CacheParameters(SQL_DELETE, parms);
			}
			return parms;
		}

		#endregion


    }
}

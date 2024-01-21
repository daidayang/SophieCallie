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
    public partial class Lst_LanguageDal : SqlBase
    {

		#region SQL Statements

	    private const string SQL_INSERT = "Lst_Language_add";
     private const string SQL_SELECT = "Lst_Language_Sel";
     private const string SQL_UPDATE = "Lst_Language_edt";
		private const string SQL_DELETE = "Lst_Language_del";



		#endregion

        #region Constructors

        public Lst_LanguageDal() { }

        public Lst_LanguageDal(SqlConnection conn) : base(conn) { }

        public Lst_LanguageDal(SqlConnection conn, SqlTransaction trans) : base(conn, trans) { }

        #endregion

         #region SQL Parameters

         private const string PARAM_LANGUAGEID = "@LanguageID";
         private const string PARAM_DESCRIPTION = "@Description";
         private const string PARAM_DATEFORMAT = "@DateFormat";
         private const string PARAM_SHORTDATEFORMAT = "@ShortDateFormat";
         private const string PARAM_ISO639 = "@ISO639";
         private const string PARAM_LCID = "@LCID";
         private const string PARAM_LOCALEDECVAL = "@LocaleDecVal";

         #endregion


        #region	UPDATEs


        public void Insert(LanguageInfo lst_language )
		{
			SqlParameter[] Param_Insert = GetParameters_Insert();
            Param_Insert[0].Value = lst_language.LanguageID;
            if ( lst_language.Description == null )
                Param_Insert[1].Value = string.Empty;
            else
                Param_Insert[1].Value = lst_language.Description;
            if ( lst_language.DateFormat == null )
                Param_Insert[2].Value = string.Empty;
            else
                Param_Insert[2].Value = lst_language.DateFormat;
            if ( lst_language.ShortDateFormat == null )
                Param_Insert[3].Value = string.Empty;
            else
                Param_Insert[3].Value = lst_language.ShortDateFormat;
            if ( lst_language.ISO639 == null )
                Param_Insert[4].Value = string.Empty;
            else
                Param_Insert[4].Value = lst_language.ISO639;
            if ( lst_language.LCID == null )
                Param_Insert[5].Value = string.Empty;
            else
                Param_Insert[5].Value = lst_language.LCID;
            Param_Insert[6].Value = lst_language.LocaleDecVal;
            SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_INSERT, Param_Insert);
		}


        public int Update(LanguageInfo lst_language )
		{
			SqlParameter[] Param_Update = GetParameters_Update();
            Param_Update[0].Value = lst_language.LanguageID;
            if ( lst_language.Description == null )
                Param_Update[1].Value = string.Empty;
            else
                Param_Update[1].Value = lst_language.Description;
            if ( lst_language.DateFormat == null )
                Param_Update[2].Value = string.Empty;
            else
                Param_Update[2].Value = lst_language.DateFormat;
            if ( lst_language.ShortDateFormat == null )
                Param_Update[3].Value = string.Empty;
            else
                Param_Update[3].Value = lst_language.ShortDateFormat;
            if ( lst_language.ISO639 == null )
                Param_Update[4].Value = string.Empty;
            else
                Param_Update[4].Value = lst_language.ISO639;
            if ( lst_language.LCID == null )
                Param_Update[5].Value = string.Empty;
            else
                Param_Update[5].Value = lst_language.LCID;
            Param_Update[6].Value = lst_language.LocaleDecVal;
            return SQLHelper.ExecuteNonQuery( base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_UPDATE, Param_Update);
		}


		public int Delete( System.Int16 languageID )
		{
            const string sql = "DELETE FROM Lst_Language WHERE [LanguageID]=@LanguageID "; 

            SqlParameter[] Params = GetParameters_Delete();
			            Params[0].Value = languageID;


            return SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params);
		}


            		#endregion

		   #region SELECTs


        public LanguageInfo Select(System.Int16 languageID)
        {
            LanguageInfo ret = null;

            SqlParameter[] Params = GetParameters_Select();
                        Params[0].Value = languageID;


            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_SELECT, Params)) 
            {
                ret = LanguageInfo.LoadDbRecord(rdr);
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
                                               new SqlParameter(PARAM_LANGUAGEID,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_DESCRIPTION,			SqlDbType.VarChar, 50),
                                               new SqlParameter(PARAM_DATEFORMAT,			SqlDbType.VarChar, 16),
                                               new SqlParameter(PARAM_SHORTDATEFORMAT,			SqlDbType.VarChar, 16),
                                               new SqlParameter(PARAM_ISO639,			SqlDbType.Char, 2),
                                               new SqlParameter(PARAM_LCID,			SqlDbType.VarChar, 5),
                                               new SqlParameter(PARAM_LOCALEDECVAL,			SqlDbType.Int, 4)
										   };
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
                                               new SqlParameter(PARAM_LANGUAGEID,			SqlDbType.SmallInt, 2)
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
                                               new SqlParameter(PARAM_LANGUAGEID,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_DESCRIPTION,			SqlDbType.VarChar, 50),
                                               new SqlParameter(PARAM_DATEFORMAT,			SqlDbType.VarChar, 16),
                                               new SqlParameter(PARAM_SHORTDATEFORMAT,			SqlDbType.VarChar, 16),
                                               new SqlParameter(PARAM_ISO639,			SqlDbType.Char, 2),
                                               new SqlParameter(PARAM_LCID,			SqlDbType.VarChar, 5),
                                               new SqlParameter(PARAM_LOCALEDECVAL,			SqlDbType.Int, 4)
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
                                               new SqlParameter(PARAM_LANGUAGEID,			SqlDbType.SmallInt, 2)
										   };
				SQLHelper.CacheParameters(SQL_DELETE, parms);
			}
			return parms;
		}

		#endregion


    }
}

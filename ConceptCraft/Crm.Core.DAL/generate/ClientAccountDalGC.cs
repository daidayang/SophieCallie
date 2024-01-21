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
    public partial class ClientAccountDal : SqlBase
    {

		#region SQL Statements

	    private const string SQL_INSERT = "ClientAccount_add";
     private const string SQL_SELECT = "ClientAccount_Sel";
     private const string SQL_UPDATE = "ClientAccount_edt";
		private const string SQL_DELETE = "ClientAccount_del";



		#endregion

        #region Constructors

        public ClientAccountDal() { }

        public ClientAccountDal(SqlConnection conn) : base(conn) { }

        public ClientAccountDal(SqlConnection conn, SqlTransaction trans) : base(conn, trans) { }

        #endregion

         #region SQL Parameters

         private const string PARAM_CLIENTID = "@ClientID";
         private const string PARAM_NAME = "@Name";
         private const string PARAM_NUMOFPROGRAMS = "@NumOfPrograms";
         private const string PARAM_CLUSTERDB = "@ClusterDB";
         private const string PARAM_LANGUAGEIDS = "@LanguageIDs";
         private const string PARAM_CONFIGURATION = "@Configuration";
         private const string PARAM_SERVICEBEGINDATE = "@ServiceBeginDate";
         private const string PARAM_SERVICEENDDATE = "@ServiceEndDate";
         private const string PARAM_URLSUBDOMAIN = "@UrlSubDomain";

         #endregion


        #region	UPDATEs


        public void Insert( CRM.BusinessEntities.ClientAccountInfo clientaccount )
		{
			SqlParameter[] Param_Insert = GetParameters_Insert();
            Param_Insert[0].Value = clientaccount.ClientID;
            if ( clientaccount.Name == null )
                Param_Insert[1].Value = string.Empty;
            else
                Param_Insert[1].Value = clientaccount.Name;
            Param_Insert[2].Value = clientaccount.NumOfPrograms;
            Param_Insert[3].Value = clientaccount.ClusterDB;
            if ( clientaccount.LanguageIDs == null )
                Param_Insert[4].Value = string.Empty;
            else
                Param_Insert[4].Value = clientaccount.LanguageIDs;
            Param_Insert[5].Value = clientaccount.Configuration;
            Param_Insert[6].Value = clientaccount.ServiceBeginDate;
            Param_Insert[7].Value = clientaccount.ServiceEndDate;
            if ( clientaccount.UrlSubDomain == null )
                Param_Insert[8].Value = string.Empty;
            else
                Param_Insert[8].Value = clientaccount.UrlSubDomain;
            SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_INSERT, Param_Insert);
		}


        public int Update( CRM.BusinessEntities.ClientAccountInfo clientaccount )
		{
			SqlParameter[] Param_Update = GetParameters_Update();
            Param_Update[0].Value = clientaccount.ClientID;
            if ( clientaccount.Name == null )
                Param_Update[1].Value = string.Empty;
            else
                Param_Update[1].Value = clientaccount.Name;
            Param_Update[2].Value = clientaccount.NumOfPrograms;
            Param_Update[3].Value = clientaccount.ClusterDB;
            if ( clientaccount.LanguageIDs == null )
                Param_Update[4].Value = string.Empty;
            else
                Param_Update[4].Value = clientaccount.LanguageIDs;
            Param_Update[5].Value = clientaccount.Configuration;
            Param_Update[6].Value = clientaccount.ServiceBeginDate;
            Param_Update[7].Value = clientaccount.ServiceEndDate;
            if ( clientaccount.UrlSubDomain == null )
                Param_Update[8].Value = string.Empty;
            else
                Param_Update[8].Value = clientaccount.UrlSubDomain;
            return SQLHelper.ExecuteNonQuery( base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_UPDATE, Param_Update);
		}


		public int Delete( System.Int16 clientID )
		{
            const string sql = "DELETE FROM ClientAccount WHERE [ClientID]=@ClientID "; 

            SqlParameter[] Params = GetParameters_Delete();
			            Params[0].Value = clientID;


            return SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, sql, Params);
		}


            		#endregion

		   #region SELECTs


        public CRM.BusinessEntities.ClientAccountInfo Select(System.Int16 clientID)
        {
            CRM.BusinessEntities.ClientAccountInfo ret = null;

            SqlParameter[] Params = GetParameters_Select();
                        Params[0].Value = clientID;


            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_SELECT, Params)) 
            {
                ret = CRM.BusinessEntities.ClientAccountInfo.LoadDbRecord(rdr);
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
                                               new SqlParameter(PARAM_CLIENTID,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_NAME,			SqlDbType.NVarChar, 2048),
                                               new SqlParameter(PARAM_NUMOFPROGRAMS,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_CLUSTERDB,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_LANGUAGEIDS,			SqlDbType.VarChar, 128),
                                               new SqlParameter(PARAM_CONFIGURATION,			SqlDbType.BigInt, 8),
                                               new SqlParameter(PARAM_SERVICEBEGINDATE,			SqlDbType.Date, 2),
                                               new SqlParameter(PARAM_SERVICEENDDATE,			SqlDbType.Date, 2),
                                               new SqlParameter(PARAM_URLSUBDOMAIN,			SqlDbType.VarChar, 64)
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
                                               new SqlParameter(PARAM_CLIENTID,			SqlDbType.SmallInt, 2)
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
                                               new SqlParameter(PARAM_CLIENTID,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_NAME,			SqlDbType.NVarChar, 2048),
                                               new SqlParameter(PARAM_NUMOFPROGRAMS,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_CLUSTERDB,			SqlDbType.SmallInt, 2),
                                               new SqlParameter(PARAM_LANGUAGEIDS,			SqlDbType.VarChar, 128),
                                               new SqlParameter(PARAM_CONFIGURATION,			SqlDbType.BigInt, 8),
                                               new SqlParameter(PARAM_SERVICEBEGINDATE,			SqlDbType.Date, 2),
                                               new SqlParameter(PARAM_SERVICEENDDATE,			SqlDbType.Date, 2),
                                               new SqlParameter(PARAM_URLSUBDOMAIN,			SqlDbType.VarChar, 64)
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
                                               new SqlParameter(PARAM_CLIENTID,			SqlDbType.SmallInt, 2)
										   };
				SQLHelper.CacheParameters(SQL_DELETE, parms);
			}
			return parms;
		}

		#endregion


    }
}

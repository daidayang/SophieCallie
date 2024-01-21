using System;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessEntities;

namespace CRM.DataAccess
{
    public class SqlBase
    {
        //  Default to my own cluster db
        public SqlBase()
        {
            //            SqlConnString = SQLHelper.CONN_STRING_CLUSTERDB;
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public SqlBase(SqlConnection conn)
        {
            _internalConnection = conn;
            _internalADOTransaction = null;
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public SqlBase(SqlConnection conn, SqlTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        protected SqlConnection _internalConnection;

        //Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In SQLHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        protected SqlTransaction _internalADOTransaction;

        //Used only when doing ADO.NET transactions.
        public void BeginADOTransaction()
        {
            _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

       

        public void BeginADOTransaction(System.Data.IsolationLevel isolationLevel)
        {
            _internalADOTransaction = _internalConnection.BeginTransaction(isolationLevel);
        }

        //Used only when doing ADO.NET transactions.
        public void RollBackTransaction()
        {
            _internalADOTransaction.Rollback();
            _internalADOTransaction = null;
        }

        //Used only when doing ADO.NET transactions.
        public void CommitADOTransaction()
        {
            _internalADOTransaction.Commit();
            _internalADOTransaction = null;
        }

        public void Open(string connString)
        {
            if (_internalConnection == null)
            {
                if (string.IsNullOrEmpty(connString))
                    throw new Exception("Connection string is empty");
                _internalConnection = new SqlConnection(connString);
            }

            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }

      

        public void OpenClusterDbByClusterID(short clusterID)
        {
           
            if (_internalConnection == null)
                _internalConnection = new SqlConnection(SQLHelper.GetClusterConnStringByCluster(clusterID));
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();

        }

        public void OpenReportDbByClusterID(short clusterID)
        {
            if (_internalConnection == null)
                _internalConnection = new SqlConnection(SQLHelper.GetReportConnStringByCluster(clusterID));
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }

        public void OpenClusterDbByClientID(short clientID)
        {
            if (_internalConnection == null)
                _internalConnection = new SqlConnection(SQLHelper.GetClusterConnStringByClient(clientID));
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }

        public void OpenReportDbByClientID(short clientID)
        {
            if (_internalConnection == null)
                _internalConnection = new SqlConnection(SQLHelper.GetReportConnStringByCluster(clientID));
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }

        public void OpenCRM()
        {
            if (_internalConnection == null)
            {
                _internalConnection = new SqlConnection(SQLHelper.ConnStringCRMCOMMON);
            }
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }



        public void Close()
        {
            if (_internalConnection != null && _internalConnection.State != ConnectionState.Closed)
                _internalConnection.Close();
        }

        public SqlConnection Connection
        {
            get { return _internalConnection; }
        }

        public SqlTransaction ADOTransaction
        {
            get { return _internalADOTransaction; }
        }


        #region Most Common DB operations

        #region SQL Statements

        private const string SQL_HOTELCACHEDIRTYLEVELADD = "usp_HotelCacheDirtyLevel_add";
        private const string SQL_CHAINCACHEDIRTYLEVELADD = "usp_ChainCacheDirtyLevel_add";
        private const string SQL_GETEXTCOUNTERVALUE = "GetNextCounterValue";
        private const string SQL_GETEXTCOUNTERVALUEBYBATCH = "GetNextCounterValueByBatch";
        #endregion

        private const string PARAM_ENTITYID = "@EntityID";
        private const string PARAM_CACHEABLEOBJECTTYPE = "@CacheableObjectType";
        private const string PARAM_OBJECTID = "@ObjectID";
        private const string PARAM_OPERATION = "@Operation";

        public int UpdateControl(int contorlID, int intValue, DateTime dtValue, string sValue)
        {
            const string sql = "UPDATE Controls SET sValue=@sValue,intValue=@intValue,dtValue=@dtValue WHERE ControlID=@ControlID";

            SqlParameter[] Params = GetParameters_UpdateControl();
            Params[0].Value = contorlID;
            Params[1].Value = intValue;
            Params[2].Value = dtValue;
            Params[3].Value = sValue;

            return SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, sql, Params);
        }

        //public int UpdateControlBigInt(int contorlID, long bigIntValue)
        //{
        //    const string sql = "UPDATE Controls SET bigValue=@Value WHERE ControlID=@ControlID";
        //    SqlParameter[] Params = GetParameters_UpdateControlBigInt();
        //    Params[0].Value = contorlID;
        //    Params[1].Value = bigIntValue;
        //    return SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, sql, Params);
        //}

        private static SqlParameter[] GetParameters_UpdateControl()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("UpdateControlValues");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ControlID",   SqlDbType.Int, 4),
                    new SqlParameter("@intValue",    SqlDbType.Int, 4),
                    new SqlParameter("@dtValue",     SqlDbType.DateTime),
                    new SqlParameter("@sValue",      SqlDbType.VarChar, 4096)
                };
                SQLHelper.CacheParameters("UpdateControlValues", parms);
            }
            return parms;
        }

        //private static SqlParameter[] GetParameters_UpdateControlBigInt()
        //{
        //    SqlParameter[] parms = SQLHelper.GetCacheParameters("UpdateControlValueBigInt");

        //    if (parms == null)
        //    {
        //        parms = new SqlParameter[] {
        //            new SqlParameter("@ControlID",  SqlDbType.Int, 4),
        //            new SqlParameter("@Value",      SqlDbType.BigInt, 8)
        //        };
        //        SQLHelper.CacheParameters("UpdateControlValueBigInt", parms);
        //    }
        //    return parms;
        //}

        public int GetNextCounterValue(short counterID)
        {
            SqlParameter[] Params = GetParameters_GetNextCounter();
            Params[0].Value = counterID;
            //            Params[1].Value = 0;

            SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_GETEXTCOUNTERVALUE, Params);

            return (int)(Params[1].Value);
        }

        public int GetNextCounterValueByBatch(short counterID,int batchSize)
        {
            SqlParameter[] Params = GetParameters_GetNextCounterByBatch();
            Params[0].Value = counterID;
            Params[1].Value = batchSize;

            SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_GETEXTCOUNTERVALUEBYBATCH, Params);

            return (int)(Params[2].Value);
        }

        private static SqlParameter[] GetParameters_GetNextCounter()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_GETEXTCOUNTERVALUE);

            if (parms == null)
            {
                parms = new SqlParameter[] {
                                               new SqlParameter("@CounterID",           SqlDbType.SmallInt, 2),
                                               new SqlParameter("@CounterValue",        SqlDbType.Int, 4)
                                           };
                parms[1].Direction = ParameterDirection.Output;
                SQLHelper.CacheParameters(SQL_GETEXTCOUNTERVALUE, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetParameters_GetNextCounterByBatch()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_GETEXTCOUNTERVALUEBYBATCH);

            if (parms == null)
            {
                parms = new SqlParameter[] {
                                               new SqlParameter("@CounterID",           SqlDbType.SmallInt, 2),
                                               new SqlParameter("@BatchSize",        SqlDbType.Int, 4),
                                               new SqlParameter("@CounterValue",        SqlDbType.Int, 4)
                                           };
                parms[2].Direction = ParameterDirection.Output;
                SQLHelper.CacheParameters(SQL_GETEXTCOUNTERVALUEBYBATCH, parms);
            }
            return parms;
        }

        public int AddCacheDirtyLevel(EntityTypeEnum entityType, int entityID, int objectID, CachableObjectTypeEnum CachableObjectTypeEnum, char operation)
        {
            SqlParameter[] Params = GetParameters_AddCacheDirtyLevel();

            Params[0].Value = entityID;
            Params[1].Value = CachableObjectTypeEnum;
            Params[2].Value = objectID;
            Params[3].Value = operation;

            if (entityType == EntityTypeEnum.Hotel)
                return SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_HOTELCACHEDIRTYLEVELADD, Params);
            else
                return SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_CHAINCACHEDIRTYLEVELADD, Params);
        }

        private static SqlParameter[] GetParameters_AddCacheDirtyLevel()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_HOTELCACHEDIRTYLEVELADD);

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter(PARAM_ENTITYID,                SqlDbType.Int, 4),
                    new SqlParameter(PARAM_CACHEABLEOBJECTTYPE,     SqlDbType.SmallInt, 2),
                    new SqlParameter(PARAM_OBJECTID,                SqlDbType.Int, 4),
                    new SqlParameter(PARAM_OPERATION,               SqlDbType.Char, 1)
                };
                SQLHelper.CacheParameters(SQL_HOTELCACHEDIRTYLEVELADD, parms);
            }
            return parms;
        }
        public int GetControl(string sqlStr)
        {
            return SQLHelper.ExecuteNonQueryNoParm(_internalConnection, _internalADOTransaction, CommandType.Text, sqlStr);
        }

        protected string GetPmsProfileID(char profileType, int crsProfileID, short pmsHotelIDasShort)
        {
            const string sql = "SELECT PmsProfileID FROM PmsProfileMapping WHERE HotelID=@HotelID AND CrsProfileID=1 AND ProfileType='T'";
            SqlParameter[] Params = GetParameters_GetPmsProfileID();
            Params[0].Value = pmsHotelIDasShort;
            Params[1].Value = crsProfileID;
            Params[2].Value = profileType;

            object rc = SQLHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, sql, Params);
            if (rc == null)
                return string.Empty;
            else
                return (string)rc;
        }

        private static SqlParameter[] GetParameters_GetPmsProfileID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("SQLBASEGETPMSPROFILEID");
            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",    SqlDbType.SmallInt, 2),
                    new SqlParameter("@ID",         SqlDbType.Int, 4),
                    new SqlParameter("@Type",       SqlDbType.Char, 1)
                };
                SQLHelper.CacheParameters("SQLBASEGETPMSPROFILEID", parms);
            }
            return parms;
        }

        #endregion

    }
}

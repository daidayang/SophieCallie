using System;
using System.Data;
using CRM.BusinessEntities;
using MySql.Data.MySqlClient;

namespace CRM.DataAccess
{
    public class MySqlBase
    {
        private const string SQL_GETEXTCOUNTERVALUE = "GetNextCounterValue";
        private const string SQL_HOTELCACHEDIRTYLEVELADD = "usp_HotelCacheDirtyLevel_add";
        private const string SQL_CHAINCACHEDIRTYLEVELADD = "usp_ChainCacheDirtyLevel_add";
        private const string PARAM_ENTITYID = "@EntityID";
        private const string PARAM_CACHEABLEOBJECTTYPE = "@CacheableObjectType";
        private const string PARAM_OBJECTID = "@ObjectID";
        private const string PARAM_OPERATION = "@Operation";
        private const string SQL_GETEXTCOUNTERVALUEBYBATCH = "GetNextCounterValueByBatch";
        public MySqlBase()
        {
           
        }
        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public MySqlBase(MySqlConnection conn)
        {
            _internalConnection = conn;
            _internalADOTransaction = null;
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public MySqlBase(MySqlConnection conn, MySqlTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        protected MySqlConnection _internalConnection;

        //Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In MYSQLHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        protected MySqlTransaction _internalADOTransaction;

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
                _internalConnection = new MySqlConnection(connString);
            }

            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }


        public void OpenCRMMySql(string connStr)
        {
            if (_internalConnection == null)
            {
                _internalConnection = new MySqlConnection(connStr);
            }
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }



        public void Close()
        {
            if (_internalConnection != null && _internalConnection.State != ConnectionState.Closed)
                _internalConnection.Close();
        }

        public MySqlConnection Connection
        {
            get { return _internalConnection; }
        }

        public MySqlTransaction ADOTransaction
        {
            get { return _internalADOTransaction; }
        }


        public int GetNextCounterValue(short counterID)
        {
            MySqlParameter[] Params = GetParameters_GetNextCounter();
            Params[0].Value = counterID;
            //            Params[1].Value = 0;

            MySQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_GETEXTCOUNTERVALUE, Params);

            return (int)(Params[1].Value);
        }

        private static MySqlParameter[] GetParameters_GetNextCounter()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters(SQL_GETEXTCOUNTERVALUE);

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                                               new MySqlParameter("@CounterID",           MySqlDbType.Int16, 2),
                                               new MySqlParameter("@CounterValue",        MySqlDbType.Int32, 4)
                                           };
                parms[1].Direction = ParameterDirection.Output;
                MySQLHelper.CacheParameters(SQL_GETEXTCOUNTERVALUE, parms);
            }
            return parms;
        }


        public int AddCacheDirtyLevel(EntityTypeEnum entityType, int entityID, int objectID, CachableObjectTypeEnum CachableObjectTypeEnum, char operation)
        {
            MySqlParameter[] Params = GetParameters_AddCacheDirtyLevel();

            Params[0].Value = entityID;
            Params[1].Value = CachableObjectTypeEnum;
            Params[2].Value = objectID;
            Params[3].Value = operation;

            if (entityType == EntityTypeEnum.Hotel)
                return MySQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_HOTELCACHEDIRTYLEVELADD, Params);
            else
                return MySQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_CHAINCACHEDIRTYLEVELADD, Params);
        }


        private static MySqlParameter[] GetParameters_AddCacheDirtyLevel()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters(SQL_HOTELCACHEDIRTYLEVELADD);

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter(PARAM_ENTITYID,                MySqlDbType.Int32, 4),
                    new MySqlParameter(PARAM_CACHEABLEOBJECTTYPE,     MySqlDbType.Int16, 2),
                    new MySqlParameter(PARAM_OBJECTID,                MySqlDbType.Int32, 4),
                    new MySqlParameter(PARAM_OPERATION,               MySqlDbType.VarString, 1)
                };
                MySQLHelper.CacheParameters(SQL_HOTELCACHEDIRTYLEVELADD, parms);
            }
            return parms;
        }


        public int GetNextCounterValueByBatch(short counterID, int batchSize)
        {
            MySqlParameter[] Params = GetParameters_GetNextCounterByBatch();
            Params[0].Value = counterID;
            Params[1].Value = batchSize;

            MySQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.StoredProcedure, SQL_GETEXTCOUNTERVALUEBYBATCH, Params);

            return (int)(Params[2].Value);
        }

        private static MySqlParameter[] GetParameters_GetNextCounterByBatch()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters(SQL_GETEXTCOUNTERVALUEBYBATCH);

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                                               new MySqlParameter("@CounterID",           MySqlDbType.Int16, 2),
                                               new MySqlParameter("@BatchSize",        MySqlDbType.Int32, 4),
                                               new MySqlParameter("@CounterValue",        MySqlDbType.Int32, 4)
                                           };
                parms[2].Direction = ParameterDirection.Output;
                MySQLHelper.CacheParameters(SQL_GETEXTCOUNTERVALUEBYBATCH, parms);
            }
            return parms;
        }
    }
}

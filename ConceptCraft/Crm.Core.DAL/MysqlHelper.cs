using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Collections;

using System.Threading;
using MySql.Data.MySqlClient;

namespace CRM.DataAccess
{

    /// <summary>
    /// The SQLHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public static class MySQLHelper
    {
       
        //Database connection strings
        private static string CONN_STRING_mysql = null;
        
        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        /// <summary>
        /// Create and execute a command to return DataReader after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">The single SqlParameter object to bind to the query.</param>
        public static MySqlDataReader ExecuteReaderSingleParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter singleParm)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return DataReader after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">The single SqlParameter object to bind to the query.</param>
        public static MySqlDataReader ExecuteReaderSingleParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText, MySqlParameter singleParm)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            MySqlDataReader rdr = cmd.ExecuteReader(behavior);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader, no parameters used in the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        public static MySqlDataReader ExecuteReaderNoParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader, no parameters used in the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        public static MySqlDataReader ExecuteReaderNoParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            MySqlDataReader rdr = cmd.ExecuteReader(behavior);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of SqlParameter objects to bind to the query.</param>
        public static MySqlDataReader ExecuteReader(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
            //cmd.Parameters.Clear();
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of SqlParameter objects to bind to the query.</param>
        public static MySqlDataReader ExecuteReader(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            MySqlDataReader rdr = cmd.ExecuteReader(behavior);
            cmd.Parameters.Clear();
            return rdr;
        }


        /// <summary>
        /// Create and execute a command to return a DataSet after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of SqlParameter objects to bind to the query.</param>
        public static DataSet ExecuteDataSet(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dsTables);

            return dsTables;
        }

        public static DataTable ExecuteDataTable(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dsTables);

            return dsTables.Tables[0];

        }

        public static DataTable ExecuteDataTableNoParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;


            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dsTables);

            return dsTables.Tables[0];

        }


        /// <summary>
        /// Create and execute a command to return a single scalar (int) value after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of SqlParameter objects to bind to the query.</param>
        public static object ExecuteScalar(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            PrepareCommand(cmd, cmdParms);
            object val = cmd.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// Create and execute a command to return a single scalar (int) value after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">A SqlParameter object to bind to the query.</param>
        public static object ExecuteScalarSingleParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter singleParm)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.Parameters.Add(singleParm);
            object val = cmd.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// Create and execute a command to return a single scalar (int) value. No parameters will be bound to the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">A SqlParameter object to bind to the query.</param>
        public static object ExecuteScalarNoParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            object val = cmd.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of SqlParameter objects to bind to the query.</param>
        public static int ExecuteNonQuery(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        public static int ExecuteNonQuery(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, int cmdTimeoutInSec, params MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            if (cmdTimeoutInSec > 0)
                cmd.CommandTimeout = cmdTimeoutInSec;
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParam">A SqlParameter object to bind to the query.</param>
        public static int ExecuteNonQuerySingleParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter singleParam)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParam);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParam">A SqlParameter object to bind to the query.</param>
        public static int ExecuteNonQueryNoParm(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        public static bool ExecuteSqlTran(MySqlConnection conn, CommandType cmdType, List<string> SQLStringList)
        {
            bool IsSuccess = false;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            using (MySqlTransaction trans = conn.BeginTransaction())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;
                cmd.Connection = conn;
                if (trans != null)
                    cmd.Transaction = trans;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string cmdText = SQLStringList[n].ToString();
                        cmd.CommandText = cmdText;
                        PrepareCommand(cmd, null);
                        int val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                    IsSuccess = true;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    IsSuccess = false;
                }
            }
            return IsSuccess;
        }




        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params MySqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static MySqlParameter[] GetCacheParameters(string cacheKey)
        {
            MySqlParameter[] cachedParms = (MySqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            MySqlParameter[] clonedParms = new MySqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (MySqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(MySqlCommand cmd, MySqlParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    MySqlParameter parm = (MySqlParameter)cmdParms[i];
                    cmd.Parameters.Add(parm);
                }
            }
        }

        public static string ConnStringCRMMYSQL
        {
            get
            {
                if (CONN_STRING_mysql == null)
                    CONN_STRING_mysql = ConfigurationManager.AppSettings["SQLConnString_CRMMYSQL"];
                return CONN_STRING_mysql;
            }
        }
    }
}




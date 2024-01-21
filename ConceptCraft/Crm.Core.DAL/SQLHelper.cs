using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using System.Threading;
using CRM.BusinessEntities;

namespace CRM.DataAccess
{

    /// <summary>
    /// The SQLHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public static class SQLHelper
    {
        private static String[] ClusterConnStrings = null;
        private static String[] ReportConnStrings = null;
        private static int _NumOfCluster = 0;
        private static bool _ClusterByEntityLoaded = false;

        //Database connection strings
        private static string CONN_STRING_CRMCOMMON = null;
        //private static string CONN_STRING_CLUSTERDB = null;
        //private static string CONN_STRING_REPORTDB = null;

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        static SQLHelper()
        {
            int tmpID = 0;
            for (int id = 1; id < 100; id++)
            {
                string constr = ConfigurationManager.AppSettings["SQLConnString_CRMCLUSTERDB" + id.ToString()];
                if (string.IsNullOrEmpty(constr))
                    break;
                _NumOfCluster = id;
            }

            ClusterConnStrings = new string[_NumOfCluster + 1];
            ReportConnStrings = new string[_NumOfCluster + 1];
            for (int id = 1; id <= _NumOfCluster; id++)
            {
                ClusterConnStrings[id] = ConfigurationManager.AppSettings["SQLConnString_CRMClusterDB" + id.ToString()];
                ReportConnStrings[id] = ConfigurationManager.AppSettings["SQLConnString_CRMREPORTDB" + id.ToString()];
            }
            _ClusterByEntityLoaded = false;
        }


        /// <summary>
        /// Create and execute a command to return DataReader after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">The single SqlParameter object to bind to the query.</param>
        public static SqlDataReader ExecuteReaderSingleParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter singleParm)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
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
        public static SqlDataReader ExecuteReaderSingleParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText, SqlParameter singleParm)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            SqlDataReader rdr = cmd.ExecuteReader(behavior);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader, no parameters used in the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        public static SqlDataReader ExecuteReaderNoParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader, no parameters used in the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        public static SqlDataReader ExecuteReaderNoParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            SqlDataReader rdr = cmd.ExecuteReader(behavior);
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
        public static SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default);
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
        public static SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction trans, CommandType cmdType, CommandBehavior behavior, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            SqlDataReader rdr = cmd.ExecuteReader(behavior);
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
        public static DataSet ExecuteDataSet(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsTables);

            return dsTables;
        }

        public static DataTable ExecuteDataTable(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsTables);

            return dsTables.Tables[0];

        }

        public static DataTable ExecuteDataTableNoParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            DataSet dsTables = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;

            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
         

            SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public static object ExecuteScalar(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
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
        public static object ExecuteScalarSingleParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter singleParm)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
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
        public static object ExecuteScalarNoParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
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
        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, int cmdTimeoutInSec, params SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
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
        public static int ExecuteNonQuerySingleParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter singleParam)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
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
        public static int ExecuteNonQueryNoParm(SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = cmdType;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandText = cmdText;
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        public static bool ExecuteSqlTran(SqlConnection conn, CommandType cmdType, List<string> SQLStringList)
        {
            bool IsSuccess = false;
            if(conn.State != ConnectionState.Open)
                conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
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
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCacheParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

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
        private static void PrepareCommand(SqlCommand cmd, SqlParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    SqlParameter parm = (SqlParameter)cmdParms[i];
                    cmd.Parameters.Add(parm);
                }
            }
        }


        private static System.Int16[] HotelCluster = null;
        private static System.Int16[] ChainCluster = null;
        private static System.Int16[] ClientCluster = null;

        static ReaderWriterLock rwl = new ReaderWriterLock();
        static int readerTimeouts = 0;
        static int writerTimeouts = 0;

        //http://msdn2.microsoft.com/en-us/library/6sh2ey19.aspx
        //Thread Safety 
        //Public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not guaranteed to be thread safe.

        //A List<(Of <(T>)>) can support multiple readers concurrently, as long as the collection is not modified. Enumerating through a collection 
        //is intrinsically not a thread-safe procedure. In the rare case where an enumeration contends with one or more write accesses, the only 
        //way to ensure thread safety is to lock the collection during the entire enumeration. To allow the collection to be accessed by multiple 
        //threads for reading and writing, you must implement your own synchronization.

 
        public static string GetClusterConnStringByCluster(int clusterID)
        {
            if (ClusterConnStrings != null)
            {
                if (clusterID <= _NumOfCluster && clusterID > 0)
                    return ClusterConnStrings[clusterID];
            }

            string ErrorMessage = string.Format("Unable to retrieve Database connection string for cluster[{0}]", clusterID);
            throw new Exception(ErrorMessage);
        }

        public static string GetReportConnStringByCluster(int clusterID)
        {
            if (ReportConnStrings != null)
            {
                if (clusterID <= _NumOfCluster && clusterID > 0)
                    return ReportConnStrings[clusterID];
            }

            string ErrorMessage = string.Format("Unable to retrieve Database connection string for cluster[{0}]", clusterID);
            throw new Exception(ErrorMessage);
        }


        //  Thread Safe Logic
        public static string GetClusterConnStringByClient(short clientID)
        {
            string StrConn = string.Empty;
            short AppClusterID = 0;

            if (clientID == 0)
                return SQLHelper.ConnStringCRMCOMMON;

            try
            {
                rwl.AcquireReaderLock(500);
                try
                {
                    // It is safe for this thread to read from the shared resource.
                    if (ClientCluster != null && clientID < ClientCluster.Length)
                    {
                        AppClusterID = ClientCluster[clientID];
                        StrConn = ClusterConnStrings[AppClusterID];
                    }
                    else
                    {
                        try
                        {
                            LockCookie lc = rwl.UpgradeToWriterLock(500);
                            try
                            {
                                // It is safe for this thread to read or write from the shared resource.
                                RefreshClientClusterInfo();
                            }
                            finally
                            {
                                // Ensure that the lock is released.
                                rwl.DowngradeFromWriterLock(ref lc);
                            }
                        }
                        catch (ApplicationException)
                        {
                            // The upgrade request timed out.
                            Interlocked.Increment(ref writerTimeouts);
                        }

                        if (ClientCluster != null && clientID < ClientCluster.Length)
                        {
                            AppClusterID = ClientCluster[clientID];
                            StrConn = ClusterConnStrings[AppClusterID];
                        }
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }

            if (StrConn == string.Empty)
            {
                string ErrorMessage = string.Format("Unable to retrieve cluster db connection string for client[{0}]", clientID);
                throw new Exception(ErrorMessage);
            }
            return StrConn;
        }

        //  Thread Safe Logic
        public static string GetReportConnStringByClient(short clientID)
        {
            string StrConn = string.Empty;
            short AppClusterID = 0;

            if (clientID == 0)
                return SQLHelper.ConnStringCRMCOMMON;

            try
            {
                rwl.AcquireReaderLock(500);
                try
                {
                    // It is safe for this thread to read from the shared resource.
                    if (ClientCluster != null && clientID < ClientCluster.Length)
                    {
                        AppClusterID = ClientCluster[clientID];
                        StrConn = ReportConnStrings[AppClusterID];
                    }
                    else
                    {
                        try
                        {
                            LockCookie lc = rwl.UpgradeToWriterLock(500);
                            try
                            {
                                // It is safe for this thread to read or write from the shared resource.
                                RefreshClientClusterInfo();
                            }
                            finally
                            {
                                // Ensure that the lock is released.
                                rwl.DowngradeFromWriterLock(ref lc);
                            }
                        }
                        catch (ApplicationException)
                        {
                            // The upgrade request timed out.
                            Interlocked.Increment(ref writerTimeouts);
                        }

                        if (ClientCluster != null && clientID < ClientCluster.Length)
                        {
                            AppClusterID = ClientCluster[clientID];
                            StrConn = ReportConnStrings[AppClusterID];
                        }
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }

            if (StrConn == string.Empty)
            {
                string ErrorMessage = string.Format("Unable to retrieve report db connection string for ClientCluster[{0}]", clientID);
                throw new Exception(ErrorMessage);
            }
            return StrConn;
        }

        private static void RefreshClientClusterInfo()
        {
            CRM.DataAccess.Utils dalUtils = new Utils();
            dalUtils.OpenCRM();
            try
            {
                ClientCluster = dalUtils.GetClientCluster();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                dalUtils.Close();
            }
        }

        public static DataTable GetClientMysqlConnStr()
        {
            DataTable accountDt = new DataTable();
            CRM.DataAccess.Utils dalUtils = new Utils();
            dalUtils.OpenCRM();
            try
            {
                accountDt = dalUtils.GetClientMysqlConnStr();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                dalUtils.Close();
            }
            return accountDt;
        }

        public static string ConnStringCRMCOMMON
        {
            get
            {
                if (CONN_STRING_CRMCOMMON == null)
                    CONN_STRING_CRMCOMMON = ConfigurationManager.AppSettings["CRMSQLConnection"];
                return CONN_STRING_CRMCOMMON;
            }
        }

        public static int NumOfCluster
        {
            get
            {
                return _NumOfCluster;
            }
        }

    }
}




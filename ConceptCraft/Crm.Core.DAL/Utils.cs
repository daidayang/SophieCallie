using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using CRM.BusinessEntities;
using System.Text;

namespace CRM.DataAccess
{
    public class Utils : SqlBase
    {
        public static DateTime Date20000102 = new DateTime(2000, 1, 2);

        #region Constructors

        public Utils() { }

        public Utils(SqlConnection conn) : base(conn) { }

        public Utils(SqlConnection conn, SqlTransaction trans) : base(conn, trans) { }

        #endregion

        public bool ClearConnectionPool()
        {
            if (base._internalConnection == null)
                return false;
            SqlConnection.ClearPool(base._internalConnection);
            return true;
        }

        public System.Int16[] GetHotelCluster()
        {
            System.Int16[] LstClusterIds = null;
            string SQL = "SELECT MAX(HotelID) AS MaxId FROM Hotel; SELECT HotelID,AppCluster FROM Hotel";

            using (SqlDataReader rdr = SQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {
                if (rdr.Read())
                {
                    int cnt = rdr.GetInt32(0);
                    LstClusterIds = new System.Int16[cnt + 1];

                    rdr.NextResult();

                    while (rdr.Read())
                    {
                        LstClusterIds[rdr.GetInt32(0)] = rdr.GetInt16(1);
                    }
                }
            }
            return LstClusterIds;
        }

        public System.Int16[] GetChainCluster()
        {
            System.Int16[] LstClusterIds = null;
            string SQL = @"SELECT MAX(HotelGroupID) FROM HotelGroup WHERE GroupType IN (10,51,56) AND Active=1;
SELECT HotelGroupID,AppCluster FROM HotelGroup WHERE GroupType IN (10,51,56) AND Active=1";

            using (SqlDataReader rdr = SQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {
                if (rdr.Read())
                {
                    int cnt = rdr.GetInt32(0);
                    LstClusterIds = new System.Int16[cnt + 1];

                    rdr.NextResult();

                    while (rdr.Read())
                    {
                        LstClusterIds[rdr.GetInt32(0)] = rdr.GetInt16(1);
                    }
                }
            }
            return LstClusterIds;
        }

        public System.Int16[] GetClientCluster()
        {
            System.Int16[] LstClusterIds = null;
            string SQL = @"SELECT MAX(ClientID) FROM ClientAccount WHERE Configuration>0;
SELECT ClientID, ClusterDB FROM ClientAccount WHERE Configuration>0";

            using (SqlDataReader rdr = SQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {
                if (rdr.Read())
                {
                    int cnt = rdr.GetInt16(0);
                    LstClusterIds = new System.Int16[cnt + 1];

                    rdr.NextResult();

                    while (rdr.Read())
                    {
                        LstClusterIds[rdr.GetInt16(0)] = rdr.GetInt16(1);
                    }
                }
            }
            return LstClusterIds;
        }

        public DataTable GetClientMysqlConnStr()
        {
            DataTable accountDt = new DataTable();
            string SQL = @"SELECT distinct MYSQLConnString FROM ClientAccount WHERE Configuration>0";

            using (DataTable dt = SQLHelper.ExecuteDataTable(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {

                accountDt = dt;
                
            }
            return accountDt;
        }


        public string GetCrsResvIDFromDB(int hotelID, int resvID)
        {
            //  This should only happen when ResvID is less than 1020000 for the old i5 migrated reservations.
            string strSql = "SELECT CrsResvID FROM Reservation WHERE HotelID=@HotelID AND ResvID=@ResvID";

            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYHOTELIDRESVID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",	SqlDbType.Int, 4),
                    new SqlParameter("@ResvID",		SqlDbType.Int, 4)
                };
                SQLHelper.CacheParameters("BYHOTELIDRESVID", parms);
            }
            parms[0].Value = hotelID;
            parms[1].Value = resvID;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.Text, strSql, parms))
            {
                if (rdr.Read())
                {
                    return rdr.GetString(0);
                }
            }
            return string.Empty;
        }

        public List<LanguageInfo> GetLanguageNames()
        {
            List<LanguageInfo> lngs = new List<LanguageInfo>();

            string SQL = "SELECT " + LanguageInfo.SQLSELECT + " FROM Lst_Language l WHERE LanguageID>0 ORDER BY l.Description";

            using (SqlDataReader rdr = SQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {
                return LanguageInfo.LoadDbRecords(rdr);
            }
        }



        public int Run_usp(string sql)
        {
            return SQLHelper.ExecuteNonQueryNoParm(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, sql);
        }

        public int ControlAdvanceToTomorrow(int contorlID)
        {
            string strSql = "UPDATE Controls SET dtValue=@NewTime WHERE ControlID=@ID AND dtValue<@Today";

            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@ID",         SqlDbType.Int,      4),
                new SqlParameter("@NewTime",    SqlDbType.DateTime, 8),
                new SqlParameter("@Today",      SqlDbType.DateTime, 8)
            };
            parms[0].Value = contorlID;
            parms[1].Value = DateTime.Now;
            parms[2].Value = DateTime.Today;

            return SQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, strSql, parms);
        }

        public static SqlParameter GetParameter_String(int len)
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYSTRING" + len.ToString());

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@Str",SqlDbType.VarChar, len)
                };
                SQLHelper.CacheParameters("BYSTRING" + len.ToString(), parms);
            }
            return parms[0];
        }

        public static SqlParameter GetParameter_DateTime()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYDATETIME");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@DateTime",SqlDbType.DateTime, 8)
                };
                SQLHelper.CacheParameters("BYDATETIME", parms);
            }
            return parms[0];
        }

        public static SqlParameter GetParameter_SmallDateTime()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYSMALLDATETIME");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@DateTime",SqlDbType.SmallDateTime, 4)
                };
                SQLHelper.CacheParameters("BYSMALLDATETIME", parms);
            }
            return parms[0];
        }

        public static SqlParameter[] GetParameters_EntityTypeEntityID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("EntityTypeEntityID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@EntityType",     SqlDbType.SmallInt, 2),
                    new SqlParameter("@EntityID",       SqlDbType.Int,      4)
                };
                SQLHelper.CacheParameters("EntityTypeEntityID", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_EntityTypeEntityIDCode()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("EntityTypeEntityIDCode");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@EntityType", SqlDbType.SmallInt, 2),
                    new SqlParameter("@EntityID",   SqlDbType.Int,      4),
                    new SqlParameter("@Code",       SqlDbType.VarChar,128)
                };
                SQLHelper.CacheParameters("EntityTypeEntityIDCode", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_HotelIDEntityTypeEntityID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("HotelIDEntityTypeEntityID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",        SqlDbType.Int,      4),
                    new SqlParameter("@EntityType",     SqlDbType.SmallInt, 2),
                    new SqlParameter("@EntityID",       SqlDbType.Int,      4)
                };
                SQLHelper.CacheParameters("HotelIDEntityTypeEntityID", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_IDEntityTypeEntityID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("IDEntityTypeEntityID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ID",             SqlDbType.Int,      4),
                    new SqlParameter("@EntityType",     SqlDbType.SmallInt, 2),
                    new SqlParameter("@EntityID",       SqlDbType.Int,      4)
                };
                SQLHelper.CacheParameters("IDEntityTypeEntityID", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_HotelIDEntityType()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("HotelIDEntityType");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",        SqlDbType.Int,      4),
                    new SqlParameter("@EntityType",     SqlDbType.SmallInt, 2)
                };
                SQLHelper.CacheParameters("HotelIDEntityType", parms);
            }
            return parms;
        }

        public static SqlParameter GetParameters_ProgramID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYPROGRAMID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ProgramID",			SqlDbType.SmallInt, 2)
                };
                SQLHelper.CacheParameters("BYPROGRAMID", parms);
            }
            return parms[0];
        }

        public static SqlParameter GetParameters_HotelID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYHOTELID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",			SqlDbType.Int, 4)
                };
                SQLHelper.CacheParameters("BYHOTELID", parms);
            }
            return parms[0];
        }

        public static SqlParameter[] GetParameters_HotelIDActiveOnly()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYHOTELIDACTIVEONLY");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@HotelID",	SqlDbType.Int, 4),
                    new SqlParameter("@ActiveOnly", SqlDbType.Bit, 1)
                };
                SQLHelper.CacheParameters("BYHOTELIDACTIVEONLY", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_ChainIDActiveOnly()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYCHAINIDACTIVEONLY");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ChainID",	SqlDbType.Int, 4),
                    new SqlParameter("@Active",	    SqlDbType.Bit, 1)
                };
                SQLHelper.CacheParameters("BYCHAINIDACTIVEONLY", parms);
            }
            return parms;
        }

        public static SqlParameter[] GetParameters_ChainID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYCHAINID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ChainID",	SqlDbType.Int, 4)
                };
                SQLHelper.CacheParameters("BYCHAINID", parms);
            }
            return parms;
        }

        public static SqlParameter GetParameters_ResvID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYRESVID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ResvID",			SqlDbType.Int, 4)
                };
                SQLHelper.CacheParameters("BYRESVID", parms);
            }
            return parms[0];
        }

        public static SqlParameter[] GetParameters_CrsResvID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYCRSRESVID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@CrsResvID",	SqlDbType.VarChar, 16)
                };
                SQLHelper.CacheParameters("BYCRSRESVID", parms);
            }
            return parms;
        }

        public static SqlParameter GetParameter_ID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ID",	SqlDbType.Int, 4)
                };
                SQLHelper.CacheParameters("BYID", parms);
            }
            return parms[0];
        }

        public static SqlParameter GetParameter_LongID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYLONGID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ID",	SqlDbType.BigInt, 8)
                };
                SQLHelper.CacheParameters("BYLONGID", parms);
            }
            return parms[0];
        }

        public static SqlParameter GetParameters_ClientID()
        {
            SqlParameter[] parms = SQLHelper.GetCacheParameters("BYCLIENTID");

            if (parms == null)
            {
                parms = new SqlParameter[] {
                    new SqlParameter("@ClientID",          SqlDbType.SmallInt, 2)
                };
                SQLHelper.CacheParameters("BYCLIENTID", parms);
            }
            return parms[0];
        }

        public List<SearchResult> GetSearchList(int clientID, string searchText, int pageNumber, int pageSize, out int total, string sortColumn, string sortDirection)
        {
            List<SearchResult> ret = new List<SearchResult>();
            total = 0;
            SqlParameter[] Params = GetParameters_SearchPaged();
            Params[0].Value = clientID;
            Params[1].Value = searchText;
            Params[2].Value = pageNumber;
            Params[3].Value = pageSize;
            Params[4].Value = sortColumn;
            Params[5].Value = sortDirection;
            Params[6].Direction = ParameterDirection.Output;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_GlobalSearchPaged, Params))
            {
                while (rdr.Read())
                {
                    SearchResult searchInfo = new SearchResult();
                    searchInfo.ID = rdr.GetString(0);
                    searchInfo.Name = rdr.GetString(1);
                    searchInfo.Description = rdr.GetString(2);
                    searchInfo.Type = rdr.GetString(3);
                    ret.Add(searchInfo);
                }
            }
            total = (int)Params[6].Value;
            return ret;
        }

        #region SQL Statements
        private const string SQL_GlobalSearchPaged = "GlobalQuery_SqlSearchPaged";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_PAGESIZE = "@PageSize";
        private const string PARAM_PAGENUMBER = "@PageNumber";
        private const string PARAM_SORTCOLUMN = "@SortColumn";
        private const string PARAM_SORTDIRECTION = "@SortDirection";
        private const string PARAM_TOTAL = "@Total";
        private const string PARAM_SEARCHTEXT = "@SearchText";

        #endregion

        private static SqlParameter[] GetParameters_SearchPaged()
        {
            SqlParameter[] parameter = SQLHelper.GetCacheParameters(SQL_GlobalSearchPaged);

            if (parameter == null)
            {
                parameter = new SqlParameter[] {
                                               new SqlParameter(PARAM_CLIENTID,SqlDbType.Int,4),
                                               new SqlParameter(PARAM_SEARCHTEXT,SqlDbType.NVarChar,255),
                                               new SqlParameter(PARAM_PAGENUMBER,SqlDbType.Int,4),
                                               new SqlParameter(PARAM_PAGESIZE,SqlDbType.Int,4),
                                               new SqlParameter(PARAM_SORTCOLUMN, SqlDbType.VarChar, 128),
                                               new SqlParameter(PARAM_SORTDIRECTION,SqlDbType.VarChar, 4),
                                               new SqlParameter(PARAM_TOTAL,SqlDbType.Int,4),
                                           };
                SQLHelper.CacheParameters(SQL_GlobalSearchPaged, parameter);
            }

            return parameter;
        }

        public string DataTableToCsv(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}



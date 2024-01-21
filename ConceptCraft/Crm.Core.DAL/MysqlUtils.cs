using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;

using CRM.BusinessEntities;
using MySql.Data.MySqlClient;

namespace CRM.DataAccess
{
    public class MysqlUtils : MySqlBase
    {
        public static DateTime Date20000102 = new DateTime(2000, 1, 2);

        #region Constructors

        public MysqlUtils() { }

        public MysqlUtils(MySqlConnection conn) : base(conn) { }

        public MysqlUtils(MySqlConnection conn, MySqlTransaction trans) : base(conn, trans) { }

        #endregion

        public bool ClearConnectionPool()
        {
            if (base._internalConnection == null)
                return false;
            MySqlConnection.ClearPool(base._internalConnection);
            return true;
        }

        public System.Int16[] GetHotelCluster()
        {
            System.Int16[] LstClusterIds = null;
            string SQL = "SELECT MAX(HotelID) AS MaxId FROM Hotel; SELECT HotelID,AppCluster FROM Hotel";

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
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

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
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

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
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



        public string GetCrsResvIDFromDB(int hotelID, int resvID)
        {
            //  This should only happen when ResvID is less than 1020000 for the old i5 migrated reservations.
            string strSql = "SELECT CrsResvID FROM Reservation WHERE HotelID=@HotelID AND ResvID=@ResvID";

            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYHOTELIDRESVID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@HotelID",    MySqlDbType.Int32, 4),
                    new MySqlParameter("@ResvID",     MySqlDbType.Int32, 4)
                };
                MySQLHelper.CacheParameters("BYHOTELIDRESVID", parms);
            }
            parms[0].Value = hotelID;
            parms[1].Value = resvID;

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.Text, strSql, parms))
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

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReaderNoParm(base._internalConnection, base._internalADOTransaction, CommandType.Text, SQL))
            {
                return LanguageInfo.LoadDbRecords(rdr);
            }
        }



        public int Run_usp(string sql)
        {
            return MySQLHelper.ExecuteNonQueryNoParm(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, sql);
        }

        public int ControlAdvanceToTomorrow(int contorlID)
        {
            string strSql = "UPDATE Controls SET dtValue=@NewTime WHERE ControlID=@ID AND dtValue<@Today";

            MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@ID",         MySqlDbType.Int32,      4),
                new MySqlParameter("@NewTime",    MySqlDbType.DateTime, 8),
                new MySqlParameter("@Today",      MySqlDbType.DateTime, 8)
            };
            parms[0].Value = contorlID;
            parms[1].Value = DateTime.Now;
            parms[2].Value = DateTime.Today;

            return MySQLHelper.ExecuteNonQuery(base._internalConnection, base._internalADOTransaction, CommandType.Text, strSql, parms);
        }

        public static MySqlParameter GetParameter_String(int len)
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYSTRING" + len.ToString());

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@Str",MySqlDbType.VarChar, len)
                };
                MySQLHelper.CacheParameters("BYSTRING" + len.ToString(), parms);
            }
            return parms[0];
        }

        public static MySqlParameter GetParameter_DateTime()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYDATETIME");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@DateTime",MySqlDbType.DateTime, 8)
                };
                MySQLHelper.CacheParameters("BYDATETIME", parms);
            }
            return parms[0];
        }

        public static MySqlParameter GetParameter_SmallDateTime()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYSMALLDATETIME");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@DateTime",MySqlDbType.DateTime, 4)
                };
                MySQLHelper.CacheParameters("BYSMALLDATETIME", parms);
            }
            return parms[0];
        }

        public static MySqlParameter[] GetParameters_EntityTypeEntityID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("EntityTypeEntityID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@EntityType",     MySqlDbType.Int16, 2),
                    new MySqlParameter("@EntityID",       MySqlDbType.Int32,      4)
                };
                MySQLHelper.CacheParameters("EntityTypeEntityID", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_EntityTypeEntityIDCode()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("EntityTypeEntityIDCode");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@EntityType", MySqlDbType.Int16, 2),
                    new MySqlParameter("@EntityID",   MySqlDbType.Int32,      4),
                    new MySqlParameter("@Code",       MySqlDbType.VarChar,128)
                };
                MySQLHelper.CacheParameters("EntityTypeEntityIDCode", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_HotelIDEntityTypeEntityID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("HotelIDEntityTypeEntityID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@HotelID",        MySqlDbType.Int32,      4),
                    new MySqlParameter("@EntityType",     MySqlDbType.Int16, 2),
                    new MySqlParameter("@EntityID",       MySqlDbType.Int32,      4)
                };
                MySQLHelper.CacheParameters("HotelIDEntityTypeEntityID", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_IDEntityTypeEntityID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("IDEntityTypeEntityID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ID",             MySqlDbType.Int32,      4),
                    new MySqlParameter("@EntityType",     MySqlDbType.Int16, 2),
                    new MySqlParameter("@EntityID",       MySqlDbType.Int32,      4)
                };
                MySQLHelper.CacheParameters("IDEntityTypeEntityID", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_HotelIDEntityType()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("HotelIDEntityType");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@HotelID",        MySqlDbType.Int32,      4),
                    new MySqlParameter("@EntityType",     MySqlDbType.Int16, 2)
                };
                MySQLHelper.CacheParameters("HotelIDEntityType", parms);
            }
            return parms;
        }

        public static MySqlParameter GetParameters_ProgramID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYPROGRAMID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ProgramID",          MySqlDbType.Int16, 2)
                };
                MySQLHelper.CacheParameters("BYPROGRAMID", parms);
            }
            return parms[0];
        }

        public static MySqlParameter GetParameters_HotelID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYHOTELID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@HotelID",            MySqlDbType.Int32, 4)
                };
                MySQLHelper.CacheParameters("BYHOTELID", parms);
            }
            return parms[0];
        }

        public static MySqlParameter[] GetParameters_HotelIDActiveOnly()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYHOTELIDACTIVEONLY");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@HotelID",    MySqlDbType.Int32, 4),
                    new MySqlParameter("@ActiveOnly", MySqlDbType.Bit, 1)
                };
                MySQLHelper.CacheParameters("BYHOTELIDACTIVEONLY", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_ChainIDActiveOnly()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYCHAINIDACTIVEONLY");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ChainID",    MySqlDbType.Int32, 4),
                    new MySqlParameter("@Active",     MySqlDbType.Bit, 1)
                };
                MySQLHelper.CacheParameters("BYCHAINIDACTIVEONLY", parms);
            }
            return parms;
        }

        public static MySqlParameter[] GetParameters_ChainID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYCHAINID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ChainID",    MySqlDbType.Int32, 4)
                };
                MySQLHelper.CacheParameters("BYCHAINID", parms);
            }
            return parms;
        }

        public static MySqlParameter GetParameters_ResvID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYRESVID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ResvID",         MySqlDbType.Int32, 4)
                };
                MySQLHelper.CacheParameters("BYRESVID", parms);
            }
            return parms[0];
        }

        public static MySqlParameter[] GetParameters_CrsResvID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYCRSRESVID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@CrsResvID",  MySqlDbType.VarChar, 16)
                };
                MySQLHelper.CacheParameters("BYCRSRESVID", parms);
            }
            return parms;
        }

        public static MySqlParameter GetParameter_ID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", MySqlDbType.Int32, 4)
                };
                MySQLHelper.CacheParameters("BYID", parms);
            }
            return parms[0];
        }

        public static MySqlParameter GetParameter_LongID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYLONGID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", MySqlDbType.Int64, 8)
                };
                MySQLHelper.CacheParameters("BYLONGID", parms);
            }
            return parms[0];
        }

        public static MySqlParameter GetParameters_ClientID()
        {
            MySqlParameter[] parms = MySQLHelper.GetCacheParameters("BYCLIENTID");

            if (parms == null)
            {
                parms = new MySqlParameter[] {
                    new MySqlParameter("@ClientID",          MySqlDbType.Int32, 2)
                };
                MySQLHelper.CacheParameters("BYCLIENTID", parms);
            }
            return parms[0];
        }

        public List<SearchResult> GetSearchList(int clientID, string searchText, int pageNumber, int pageSize, out int total, string sortColumn, string sortDirection)
        {
            int startNo = (pageNumber - 1) * pageSize;
            List<SearchResult> ret = new List<SearchResult>();
            total = 0;
            MySqlParameter[] Params = GetParameters_SearchPaged();
            Params[0].Value = clientID;
            Params[1].Value = searchText;
            Params[2].Value = startNo;
            Params[3].Value = pageSize;
            Params[4].Value = sortColumn;
            Params[5].Value = sortDirection;
            Params[6].Direction = ParameterDirection.Output;

            using (MySqlDataReader rdr = MySQLHelper.ExecuteReader(base._internalConnection, base._internalADOTransaction, CommandType.StoredProcedure, SQL_GlobalSearchPaged, Params))
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
        private const string SQL_GlobalSearchPaged = "GlobalQuery_MySqlSearchPaged";
        private const string PARAM_CLIENTID = "@ClientID";
        private const string PARAM_PAGESIZE = "@PageSize";
        private const string PARAM_PAGENUMBER = "@PageNumber";
        private const string PARAM_SORTCOLUMN = "@SortColumn";
        private const string PARAM_SORTDIRECTION = "@SortDirection";
        private const string PARAM_TOTAL = "@Total";
        private const string PARAM_SEARCHTEXT = "@SearchText";

        #endregion

        private static MySqlParameter[] GetParameters_SearchPaged()
        {
            MySqlParameter[] parameter = MySQLHelper.GetCacheParameters(SQL_GlobalSearchPaged);

            if (parameter == null)
            {
                parameter = new MySqlParameter[] {
                                               new MySqlParameter(PARAM_CLIENTID,MySqlDbType.Int32,4),
                                               new MySqlParameter(PARAM_SEARCHTEXT,MySqlDbType.VarChar,255),
                                               new MySqlParameter(PARAM_PAGENUMBER,MySqlDbType.Int32,4),
                                               new MySqlParameter(PARAM_PAGESIZE,MySqlDbType.Int32,4),
                                               new MySqlParameter(PARAM_SORTCOLUMN, MySqlDbType.VarChar, 128),
                                               new MySqlParameter(PARAM_SORTDIRECTION,MySqlDbType.VarChar, 4),
                                               new MySqlParameter(PARAM_TOTAL,MySqlDbType.Int32,4),
                                           };
                MySQLHelper.CacheParameters(SQL_GlobalSearchPaged, parameter);
            }

            return parameter;
        }
    }
}



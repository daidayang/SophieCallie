using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{
    [Serializable()]
    public partial class LoyaltyProgramInfo
    {

        public const string SQLSELECT = "l.LoyaltyProgramID,l.ClientID,l.Name,l.ProgramType,l.JoinFormBaseFields,l.JoinFormBaseFieldsRequired,l.Configuration,l.ClusterDB,l.UrlSubDomain,l.LanguageIDs,l.DefaultGuestLevel";
        public const string SQLINSERTCOLUMNS = "LoyaltyProgramID,ClientID,Name,ProgramType,JoinFormBaseFields,JoinFormBaseFieldsRequired,Configuration,ClusterDB,UrlSubDomain,LanguageIDs,DefaultGuestLevel";

        #region Database fields
        private System.Int16 _LoyaltyProgramID;
        private System.Int16 _ClientID;
        private System.String _Name;
        private System.Int16 _ProgramType;
        private System.Int32 _JoinFormBaseFields;
        private System.Int32 _JoinFormBaseFieldsRequired;
        private System.Int64 _Configuration;
        private System.Int16 _ClusterDB;
        private System.String _UrlSubDomain;
        private System.String _LanguageIDs;
        private System.String _DefaultGuestLevel;
        #endregion

        #region GETs and SETs

        public System.Int16 LoyaltyProgramID
        {
            get { return _LoyaltyProgramID; }
            set { _LoyaltyProgramID = value; }
        }

        public System.Int16 ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; }
        }

        public System.String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public System.Int16 ProgramType
        {
            get { return _ProgramType; }
            set { _ProgramType = value; }
        }

        public System.Int32 JoinFormBaseFields
        {
            get { return _JoinFormBaseFields; }
            set { _JoinFormBaseFields = value; }
        }

        public System.Int32 JoinFormBaseFieldsRequired
        {
            get { return _JoinFormBaseFieldsRequired; }
            set { _JoinFormBaseFieldsRequired = value; }
        }

        public System.Int64 Configuration
        {
            get { return _Configuration; }
            set { _Configuration = value; }
        }

        public System.Int16 ClusterDB
        {
            get { return _ClusterDB; }
            set { _ClusterDB = value; }
        }

        public System.String UrlSubDomain
        {
            get { return _UrlSubDomain; }
            set { _UrlSubDomain = value; }
        }

        public System.String LanguageIDs
        {
            get { return _LanguageIDs; }
            set { _LanguageIDs = value; }
        }

        public System.String DefaultGuestLevel
        {
            get { return _DefaultGuestLevel; }
            set { _DefaultGuestLevel = value; }
        }
        #endregion



        public static LoyaltyProgramInfo LoadDbRecord(IDataReader rdr)
        {
            LoyaltyProgramInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new LoyaltyProgramInfo();
                obj.LoyaltyProgramID = rdr.GetInt16(0);
                obj.ClientID = rdr.GetInt16(1);
                obj.Name = rdr.GetString(2);
                obj.ProgramType = rdr.GetInt16(3);
                obj.JoinFormBaseFields = rdr.GetInt32(4);
                obj.JoinFormBaseFieldsRequired = rdr.GetInt32(5);
                obj.Configuration = rdr.GetInt64(6);
                obj.ClusterDB = rdr.GetInt16(7);
                obj.UrlSubDomain = rdr.GetString(8);
                obj.LanguageIDs = rdr.GetString(9);
                obj.DefaultGuestLevel = rdr.GetString(10);
            }
            return obj;
        }
        public static List<LoyaltyProgramInfo> LoadDbRecords(IDataReader rdr)
        {
            List<LoyaltyProgramInfo> ret = new List<LoyaltyProgramInfo>();

            while (true)
            {
                LoyaltyProgramInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

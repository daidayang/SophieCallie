using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{

    public partial class LanguageInfo
    {

        public const string SQLSELECT = "l.LanguageID,l.Description,l.DateFormat,l.ShortDateFormat,l.ISO639,l.LCID,l.LocaleDecVal";
        public const string SQLINSERTCOLUMNS = "LanguageID,Description,DateFormat,ShortDateFormat,ISO639,LCID,LocaleDecVal";

        #region Database fields
        private System.Int16 _LanguageID;
        private System.String _Description;
        private System.String _DateFormat;
        private System.String _ShortDateFormat;
        private System.String _ISO639;
        private System.String _LCID;
        private System.Int32 _LocaleDecVal;
        #endregion

        #region GETs and SETs

        public System.Int16 LanguageID
        {
            get { return _LanguageID; }
            set { _LanguageID = value; }
        }

        public System.String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public System.String DateFormat
        {
            get { return _DateFormat; }
            set { _DateFormat = value; }
        }

        public System.String ShortDateFormat
        {
            get { return _ShortDateFormat; }
            set { _ShortDateFormat = value; }
        }

        public System.String ISO639
        {
            get { return _ISO639; }
            set { _ISO639 = value; }
        }

        public System.String LCID
        {
            get { return _LCID; }
            set { _LCID = value; }
        }

        public System.Int32 LocaleDecVal
        {
            get { return _LocaleDecVal; }
            set { _LocaleDecVal = value; }
        }
        #endregion



        public static LanguageInfo LoadDbRecord(IDataReader rdr)
        {
            LanguageInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new LanguageInfo();
                obj.LanguageID = rdr.GetInt16(0);
                obj.Description = rdr.GetString(1);
                obj.DateFormat = rdr.GetString(2);
                obj.ShortDateFormat = rdr.GetString(3);
                obj.ISO639 = rdr.GetString(4);
                obj.LCID = rdr.GetString(5);
                obj.LocaleDecVal = rdr.GetInt32(6);
            }
            return obj;
        }
        public static List<LanguageInfo> LoadDbRecords(IDataReader rdr)
        {
            List<LanguageInfo> ret = new List<LanguageInfo>();

            while (true)
            {
                LanguageInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

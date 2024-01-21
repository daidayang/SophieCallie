using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{
    [Serializable()]
    public partial class ClientAccountInfo
    {

        public const string SQLSELECT = "c.ClientID,c.Name,c.NumOfPrograms,c.ClusterDB,c.LanguageIDs,c.Configuration,c.ServiceBeginDate,c.ServiceEndDate,c.UrlSubDomain,c.Flag,c.MauticUri,c.MauticCustomerKey,c.MauticCustomerSecret,c.MauticAccessToken,c.MauticAccessSecret,c.TangoCardPlatformName,c.TangoCardPlatformKey,c.ExchangeSourceID,c.BaseCurrencyID,c.GiftProviderName,c.EmailMarktingProviderName,c.MYSQLConnString,c.GoogleTracingCode";
        public const string SQLINSERTCOLUMNS = "ClientID,Name,NumOfPrograms,ClusterDB,LanguageIDs,Configuration,ServiceBeginDate,ServiceEndDate,UrlSubDomain,Flag,MauticUri,MauticCustomerKey,MauticCustomerSecret,MauticAccessToken,MauticAccessSecret,TangoCardPlatformName,TangoCardPlatformKey,ExchangeSourceID,BaseCurrencyID,GiftProviderName,EmailMarktingProviderName,MYSQLConnString,GoogleTracingCode";

        #region Database fields
        private System.Int16 _ClientID;
        private System.String _Name;
        private System.Int16 _NumOfPrograms;
        private System.Int16 _ClusterDB;
        private System.String _LanguageIDs;
        private System.Int64 _Configuration;
        private System.DateTime _ServiceBeginDate;
        private System.DateTime _ServiceEndDate;
        private System.String _UrlSubDomain;
        private System.Boolean _Flag;

        private System.String _MauticUri;
        private System.String _MauticCustomerKey;
        private System.String _MauticCustomerSecret;
        private System.String _MauticAccessToken;
        private System.String _MauticAccessSecret;
        private System.String _TangoCardPlatformName;
        private System.String _TangoCardPlatformKey;
        private System.Int16 _ExchangeSourceID;
        private System.Int16 _BaseCurrencyID;
        private System.String _GiftProviderName;
        private System.String _EmailMarktingProviderName;
        private System.String _MYSQLConnString;
        private System.String _GoogleTracingCode;
        #endregion

        #region GETs and SETs

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

        public System.Int16 NumOfPrograms
        {
            get { return _NumOfPrograms; }
            set { _NumOfPrograms = value; }
        }

        public System.Int16 ClusterDB
        {
            get { return _ClusterDB; }
            set { _ClusterDB = value; }
        }

        public System.String LanguageIDs
        {
            get { return _LanguageIDs; }
            set { _LanguageIDs = value; }
        }

        public System.Int64 Configuration
        {
            get { return _Configuration; }
            set { _Configuration = value; }
        }

        public System.DateTime ServiceBeginDate
        {
            get { return _ServiceBeginDate; }
            set { _ServiceBeginDate = value; }
        }

        public System.DateTime ServiceEndDate
        {
            get { return _ServiceEndDate; }
            set { _ServiceEndDate = value; }
        }

        public System.String UrlSubDomain
        {
            get { return _UrlSubDomain; }
            set { _UrlSubDomain = value; }
        }

        public bool Flag
        {
            get
            {
                return _Flag;
            }

            set
            {
                _Flag = value;
            }
        }

        public System.String MauticUri
        {
            get { return _MauticUri; }
            set { _MauticUri = value; }
        }
        public System.String MauticCustomerKey
        {
            get { return _MauticCustomerKey; }
            set { _MauticCustomerKey = value; }
        }
        public System.String MauticCustomerSecret
        {
            get { return _MauticCustomerSecret; }
            set { _MauticCustomerSecret = value; }
        }
        public System.String MauticAccessToken
        {
            get { return _MauticAccessToken; }
            set { _MauticAccessToken = value; }
        }
        public System.String MauticAccessSecret
        {
            get { return _MauticAccessSecret; }
            set { _MauticAccessSecret = value; }
        }
        public System.String TangoCardPlatformName
        {
            get { return _TangoCardPlatformName; }
            set { _TangoCardPlatformName = value; }
        }
        public System.String TangoCardPlatformKey
        {
            get { return _TangoCardPlatformKey; }
            set { _TangoCardPlatformKey = value; }
        }

        public System.Int16 ExchangeSourceID
        {
            get { return _ExchangeSourceID; }
            set { _ExchangeSourceID = value; }
        }

        public System.Int16 BaseCurrencyID
        {
            get { return _BaseCurrencyID; }
            set { _BaseCurrencyID = value; }
        }

        public System.String GiftProviderName
        {
            get { return _GiftProviderName; }
            set { _GiftProviderName = value; }
        }

        public System.String EmailMarktingProviderName
        {
            get { return _EmailMarktingProviderName; }
            set { _EmailMarktingProviderName = value; }
        }

        public System.String MYSQLConnString
        {
            get { return _MYSQLConnString; }
            set { _MYSQLConnString = value; }
        }

        public string GoogleTracingCode
        {
            get
            {
                return _GoogleTracingCode;
            }

            set
            {
                _GoogleTracingCode = value;
            }
        }
        #endregion



        public static ClientAccountInfo LoadDbRecord(IDataReader rdr)
        {
            ClientAccountInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new ClientAccountInfo();
                obj.ClientID = rdr.GetInt16(0);
                obj.Name = rdr.GetString(1);
                obj.NumOfPrograms = rdr.GetInt16(2);
                obj.ClusterDB = rdr.GetInt16(3);
                obj.LanguageIDs = rdr.GetString(4);
                obj.Configuration = rdr.GetInt64(5);
                obj.ServiceBeginDate = rdr.GetDateTime(6);
                obj.ServiceEndDate = rdr.GetDateTime(7);
                obj.UrlSubDomain = rdr.GetString(8);
                obj.Flag = rdr.GetBoolean(9);
                obj.MauticUri = rdr.GetString(10);
                obj.MauticCustomerKey = rdr.GetString(11);
                obj.MauticCustomerSecret = rdr.GetString(12);
                obj.MauticAccessToken = rdr.GetString(13);
                obj.MauticAccessSecret = rdr.GetString(14);
                obj.TangoCardPlatformName = rdr.GetString(15);
                obj.TangoCardPlatformKey = rdr.GetString(16);
                obj.ExchangeSourceID = rdr.GetInt16(17);
                obj.BaseCurrencyID = rdr.GetInt16(18);
                obj.GiftProviderName = rdr.GetString(19);
                obj.EmailMarktingProviderName = rdr.GetString(20);
                obj.MYSQLConnString = rdr.GetString(21);
                obj.GoogleTracingCode = rdr.GetString(22);
            }
            return obj;
        }
        public static List<ClientAccountInfo> LoadDbRecords(IDataReader rdr)
        {
            List<ClientAccountInfo> ret = new List<ClientAccountInfo>();

            while (true)
            {
                ClientAccountInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

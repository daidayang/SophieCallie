using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CRM.BusinessEntities
{

    public partial class InputFormFieldsInfo
    {

        public const string SQLSELECT = "i.FieldID,i.FieldType,i.FieldName,i.Access,i.Email,i.Locale,i.NextExpire,i.LockUntilTime,i.LastUseTime,i.DateCreated,i.DateDeactivated,i.Active,i.Firstname,i.Lastname,i.Setting,i.UserID";
        public const string SQLINSERTCOLUMNS = "FieldType,FieldName,Access,Email,Locale,NextExpire,LockUntilTime,LastUseTime,DateCreated,DateDeactivated,Active,Firstname,Lastname,Setting,UserID";

        #region Database fields
        private System.Int32 _FieldID;
        private System.Byte _FieldType;
        private System.String _FieldName;
        private System.String _Access;
        private System.String _Email;
        private System.String _Locale;
        private System.DateTime _NextExpire;
        private System.DateTime _LockUntilTime;
        private System.DateTime _LastUseTime;
        private System.DateTime _DateCreated;
        private System.DateTime _DateDeactivated;
        private System.Boolean _Active;
        private System.String _Firstname;
        private System.String _Lastname;
        private System.Int32 _Setting;
        private System.Int32 _UserID;
        #endregion

        #region GETs and SETs

        public System.Int32 FieldID
        {
            get { return _FieldID; }
            set { _FieldID = value; }
        }

        public System.Byte FieldType
        {
            get { return _FieldType; }
            set { _FieldType = value; }
        }

        public System.String FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public System.String Access
        {
            get { return _Access; }
            set { _Access = value; }
        }

        public System.String Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public System.String Locale
        {
            get { return _Locale; }
            set { _Locale = value; }
        }

        public System.DateTime NextExpire
        {
            get { return _NextExpire; }
            set { _NextExpire = value; }
        }

        public System.DateTime LockUntilTime
        {
            get { return _LockUntilTime; }
            set { _LockUntilTime = value; }
        }

        public System.DateTime LastUseTime
        {
            get { return _LastUseTime; }
            set { _LastUseTime = value; }
        }

        public System.DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        public System.DateTime DateDeactivated
        {
            get { return _DateDeactivated; }
            set { _DateDeactivated = value; }
        }

        public System.Boolean Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        public System.String Firstname
        {
            get { return _Firstname; }
            set { _Firstname = value; }
        }

        public System.String Lastname
        {
            get { return _Lastname; }
            set { _Lastname = value; }
        }

        public System.Int32 Setting
        {
            get { return _Setting; }
            set { _Setting = value; }
        }

        public System.Int32 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        #endregion



        public static InputFormFieldsInfo LoadDbRecord(IDataReader rdr)
        {
            InputFormFieldsInfo obj = null;

            if (rdr == null)
                return null;

            if (rdr.Read())
            {
                obj = new InputFormFieldsInfo();
                obj.FieldID = rdr.GetInt32(0);
                obj.FieldType = rdr.GetByte(1);
                obj.FieldName = rdr.GetString(2);
                obj.Access = rdr.GetString(3);
                obj.Email = rdr.GetString(4);
                obj.Locale = rdr.GetString(5);
                obj.NextExpire = rdr.GetDateTime(6);
                obj.LockUntilTime = rdr.GetDateTime(7);
                obj.LastUseTime = rdr.GetDateTime(8);
                obj.DateCreated = rdr.GetDateTime(9);
                obj.DateDeactivated = rdr.GetDateTime(10);
                obj.Active = rdr.GetBoolean(11);
                obj.Firstname = rdr.GetString(12);
                obj.Lastname = rdr.GetString(13);
                obj.Setting = rdr.GetInt32(14);
                obj.UserID = rdr.GetInt32(15);
            }
            return obj;
        }
        public static List<InputFormFieldsInfo> LoadDbRecords(IDataReader rdr)
        {
            List<InputFormFieldsInfo> ret = new List<InputFormFieldsInfo>();

            while (true)
            {
                InputFormFieldsInfo prog = LoadDbRecord(rdr);
                if (prog == null)
                    break;

                ret.Add(prog);
            }
            return ret;
        }            

    }
}

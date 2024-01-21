using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMAdmin.Models
{
    [Serializable()]
    public class AppUser
    {
        public System.Int32 UserID
        {
            get;
            set;
        }

        public System.Int32 CorporateID
        {
            get;
            set;
        }

        public System.Int32 HotelID
        {
            get;
            set;
        }

        public UserTypeEnum UserType
        {
            get;
            set;
        }

        public System.String UserName
        {
            get;
            set;
        }

        public System.String Pwd
        {
            get;
            set;
        }

        public System.String Email
        {
            get;
            set;
        }

        public System.String Firstname
        {
            get;
            set;
        }

        public System.String Lastname
        {
            get;
            set;
        }

        public System.Boolean EnablePasswordExpire
        {
            get;
            set;
        }
        public System.Int16 PasswordExpireDays
        {
            get;
            set;
        }

        public System.DateTime NextPasswordExpire
        {
            get;
            set;
        }

        public System.DateTime LockOutUntil
        {
            get;
            set;
        }

        public System.DateTime DateCreated
        {
            get;
            set;
        }

        public System.DateTime DateDeactivated
        {
            get;
            set;
        }

        public System.Int32 Settings
        {
            get;
            set;
        }

        public System.Boolean Active
        {
            get;
            set;
        }

        public System.DateTime LastLoginTime
        {
            get;
            set;
        }

        public System.String PhoneNumber
        {
            get;
            set;
        }

        //add by wang

        public System.Int32 FieldID
        {
            get; set;
        }
        public System.Int32 FieldType
        {
            get; set;
        }
        public System.String FieldName
        {
            get; set;
        }
        public System.String Access
        {
            get; set;
        }
        //public System.String Email { get; set; }
        public System.String Locale
        {
            get; set;
        }
        public System.DateTime NextExpire
        {
            get; set;
        }
        public System.DateTime LockUntilTime
        {
            get; set;
        }
        public System.DateTime LastUseTime
        {
            get; set;
        }
        //public System.DateTime DateCreated { get; set; }
        //public System.DateTime DateDeactivated { get; set; }
        //public System.Int32 Active { get; set; }
        //public System.String Firstname { get; set; }
        //public System.String Lastname { get; set; }
        public System.Int32 Setting
        {
            get; set;
        }

        public List<LoyaltyProgramInfo> Programs
        {
            get;set;
        }

        public List<ClientAccountInfo> Clients
        {
            get; set;
        }

        public short ClientID
        {
            get; set;
        }

        public short ClusterDB
        {
            get; set;
        }

        public List<AdminUserAccessInfo> CurrentProgramAccess
        {
            get;set;
        }
        public UserLevelEnum UserLevel_Enum
        {
            get;set;
        }

        public string LoginAuth
        {
            get;set;
        }

        public string MauticUrl
        {
            get;set;
        }
        public string MauticCustomerKey
        {
            get; set;
        }
        public string MauticCustomerSecret
        {
            get; set;
        }

        public string MauticAccessToken
        {
            get; set;
        }
        public string MauticAccessSecret
        {
            get; set;
        }

        public ClientAccountInfo AccountInfo
        {
            get;set;
        }

        public string MySqlConnstring
        {
            get;set;
        }
    }
}
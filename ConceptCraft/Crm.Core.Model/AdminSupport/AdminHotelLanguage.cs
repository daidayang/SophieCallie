using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessEntities
{
   public class AdminHotelLanguage
    {
        #region Database fields
        private System.Int32 _HotelID;
        private System.Int16 _LanguageID;
        private System.Boolean _PrimaryYN;
        private System.Boolean _Published;
        private string _Description;
        private bool _Enabled;
        #endregion

        #region GETs and SETs

        public System.Int32 HotelID
        {
            get { return _HotelID; }
            set { _HotelID = value; }
        }

        public System.Int16 LanguageID
        {
            get { return _LanguageID; }
            set { _LanguageID = value; }
        }

        public System.Boolean PrimaryYN
        {
            get { return _PrimaryYN; }
            set { _PrimaryYN = value; }
        }

        public System.Boolean Published
        {
            get { return _Published; }
            set { _Published = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public System.Boolean Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        #endregion

        #region Constructor added by Ryan 2008-10-08
        public AdminHotelLanguage() { }
        public AdminHotelLanguage(LanguageInfo li)
        {
            this.LanguageID = li.LanguageID;
            this.Description = li.Description;
        }
        #endregion
    }
}

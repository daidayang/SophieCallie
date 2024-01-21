using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessEntities
{
   public partial class InputFormFieldsInfo
    {
        #region More fields
        private string _PwdDecoded;
        private short _ClientID;

        private List<AdminUserAccessInfo> _CurrentProgramAccess = null;
        private List<LoyaltyProgramInfo> _Programs = null;
        private List<ClientAccountInfo> _Clients = null;

        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public short ClusterID { get; set; }

        #endregion

        #region GETs and SETs

        public short ClientID
        {
            get { return _ClientID; }
            set { _ClientID = value; }
        }

        public string PwdDecoded
        {
            get { return _PwdDecoded; }
            set { _PwdDecoded = value; }
        }

        public List<AdminUserAccessInfo> CurrentProgramAccess
        {
            get { return _CurrentProgramAccess; }
            set { _CurrentProgramAccess = value; }
        }


        public List<LoyaltyProgramInfo> Programs
        {
            get { return _Programs; }
            set { _Programs = value; }
        }

        public UserLevelEnum UserLevel_Enum
        {
            get { return (UserLevelEnum)_FieldType; }
            set { _FieldType = (byte)value; }
        }

        public List<ClientAccountInfo> Clients
        {
            get
            {
                return _Clients;
            }

            set
            {
                _Clients = value;
            }
        }

        #endregion
    }
}

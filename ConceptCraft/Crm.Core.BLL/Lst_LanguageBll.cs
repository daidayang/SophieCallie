using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.DataAccess;

namespace CRM.BusinessLogic
{
   public partial class Lst_LanguageBll:BLLBase
    {
        #region Selects  

        public LanguageInfo Select(short clusterID,System.Int16 languageID)
        {
            Lst_LanguageDal dalLanguage = new Lst_LanguageDal();
            dalLanguage.OpenClusterDbByClusterID(clusterID);
            try
            {
                return dalLanguage.Select(languageID);
            }
            catch (Exception e)
            {
                _ErrorMessage = e.Message;
                _ErrorCode = -1;
                log.ErrorFormat("Error: Source=BusinessLogic.Lst_LanguageBll.Select Message=InputField", _ErrorMessage);
            }
            finally
            {
                dalLanguage.Close();
            }

            return null;
        }


        #endregion
    }
}

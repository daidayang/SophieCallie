using CRM.BusinessEntities;
using CRM.DataAccess;
// using CRM.Service.Mautic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessLogic
{
   public partial class Utils:BLLBase
    {
        #region Local Vairables

        //private static CRS.
        public static readonly DateTime VeryEarlyDate = new DateTime(2000, 1, 1);
        public static readonly DateTime VeryEarlyDate2 = new DateTime(2000, 2, 2);
        public static readonly DateTime VeryLateDate2 = new DateTime(2070, 1, 1);
        public static readonly DateTime VeryLateDate = new DateTime(2070, 12, 31);

        // private static string[,] _languageTranslations;
        private static LanguageInfo[] _SystemLanguages = null;
        private static List<LanguageInfo> _SystemLanguageList = null;
        private static int _MaxLanguageID = 0;
        public static int _NumOfClusters = 1;
        private static bool SystemParamLoaded = false;
        private static string _SmtpHost = string.Empty;
        #endregion

        public static List<LanguageInfo> GetSystemLanguageList(short clusterID)
        {
            if (_SystemLanguageList == null)
                GetSystemLanguages(clusterID);

            return _SystemLanguageList;
        }

        public static LanguageInfo[] GetSystemLanguages(short clusterID)
        {
            if (_SystemLanguages != null)
                return _SystemLanguages;

            CRM.DataAccess.Utils dalUtil = new CRM.DataAccess.Utils();
            dalUtil.OpenClusterDbByClusterID(clusterID);
            try
            {
                _SystemLanguageList = dalUtil.GetLanguageNames();
            }
            catch (Exception e)
            {
                log.ErrorFormat("ERROR  Source: UtilsBll.GetSystemLanguages, Message:{0}", e.Message);
            }
            finally
            {
                dalUtil.Close();
            }

            _MaxLanguageID = int.MinValue;

            foreach (LanguageInfo l in _SystemLanguageList)
            {
                if (l.LanguageID > _MaxLanguageID)
                    _MaxLanguageID = l.LanguageID;
            }

            _SystemLanguages = new LanguageInfo[_MaxLanguageID + 1];

            foreach (LanguageInfo l in _SystemLanguageList)
            {
                _SystemLanguages[l.LanguageID] = l;
            }

            return _SystemLanguages;
        }

        public static string GetLanguageName(short clusterID,short langid, short inlangid)
        {
            if (_SystemLanguages == null)
            {
                GetSystemLanguages(clusterID);

                if (_SystemLanguages == null)
                    return string.Empty;
            }

            if (langid > 0 && langid < _MaxLanguageID+1)//edit by jing 2019-08-01 17:49
                return _SystemLanguages[langid].Description;
            else
                return string.Empty;
        }

        public static int NumOfClusters
        {
            get
            {
                if (!SystemParamLoaded)
                    Utils.LoadSystemParam();
                return _NumOfClusters;
            }
        }


        private static void LoadSystemParam()
        {
            if (SystemParamLoaded)
                return;

            if (ConfigurationSettings.AppSettings["NumOfClusters"] != null)
            {
                if (!int.TryParse(ConfigurationSettings.AppSettings["NumOfClusters"], out _NumOfClusters))
                    _NumOfClusters = 1;
            }

            if (ConfigurationSettings.AppSettings["SmtpHost"] != null)
            {
                _SmtpHost = ConfigurationSettings.AppSettings["SmtpHost"];
            }

            SystemParamLoaded = true;
        }

        public static List<SearchResult> GetSearchList(short clusterID,short clientID, string searchText, int pageNumber, int pageSize, out int total, string sortColumn, string sortDirection)
        {                  
            total = 0;
            List<SearchResult> ret = new List<SearchResult>();
            CRM.DataAccess.Utils dalUtil = new CRM.DataAccess.Utils();
            try
            {
                dalUtil.OpenClusterDbByClusterID(clusterID);
                ret = dalUtil.GetSearchList(clientID, searchText, pageNumber, pageSize, out total, sortColumn, sortDirection);
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error: Source=UtilsBll.GetSearchList Message={0}, StackTrace={1}", e.Message, e.StackTrace);
            }
            finally
            {
                dalUtil.Close();
            }

            return ret;
        }

        //public static EmailSetupInfo GetEmailByType(short clientID, EmailTypeEnum emailType, int languageID)
        //{           
        //    EmailSetupInfo ret = new EmailSetupInfo();
        //    EmailSetupBll bllEmailSetup = new BusinessLogic.EmailSetupBll();
        //    try
        //    {
        //        int eType = Convert.ToInt32(emailType);
        //        ret = bllEmailSetup.CheckByEmailType(clientID, eType, 0, languageID);
        //    }
        //    catch (Exception e)
        //    {
        //        log.ErrorFormat("Error: Source=UtilsBll.GetEmailByType Message={0}, StackTrace={1}", e.Message, e.StackTrace);
        //    }   
        //    return ret;
        //}


        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            //byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            byte[] data = md5Hasher.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        //public static void InsertLog(short clientID, int userID, string userName, string module, string type, string desc)
        //{
        //    SysLogBll bll = new BusinessLogic.SysLogBll();
        //    SysLogInfo logInfo = new SysLogInfo();
        //    logInfo.ClientID = clientID;
        //    logInfo.UserID = userID;
        //    logInfo.UserName = userName;
        //    logInfo.Module = module;
        //    logInfo.CreateTime = DateTime.Now;
        //    logInfo.Description = desc;
        //    logInfo.Type = type;
        //    bll.Insert("", clientID, logInfo);
        //}
        public static string ReplaceBadChar(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string strBadChar, tempChar;
            string[] arrBadChar;
            strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            arrBadChar = strBadChar.Split(',');
            tempChar = str;
            for (int i = 0; i < arrBadChar.Length; i++)
            {
                if (arrBadChar[i].Length > 0)
                    tempChar = tempChar.Replace(arrBadChar[i], "");
            }
            return tempChar;
        }

        public static int GetNextCounterValueByBatch(short clientID, short counterID, int batchSize)
        {
            int ret = -1;
            ClientAccountDal dal = new ClientAccountDal();
            dal.OpenClusterDbByClientID(clientID);
            try
            {
                ret = dal.GetNextCounterValueByBatch(counterID, batchSize);
            }
            catch (Exception e)
            {
                log.ErrorFormat("BLLBase.GetNextCounterValueByBatch() Message={0}, StackTrace={1}", e.Message, e.StackTrace);
            }
            finally
            {
                dal.Close();
            }
            return ret;
        }

        public static int GetNextCounterValue(short clientID,short counterID)
        {
            int ret = -1;
            ClientAccountDal dal = new ClientAccountDal();
            dal.OpenClusterDbByClientID(clientID);
            try
            {
                ret = dal.GetNextCounterValue(counterID);
            }
            catch (Exception e)
            {
                log.ErrorFormat("BLLBase.GetNextCounterValue() Message={0}, StackTrace={1}", e.Message, e.StackTrace);
            }
            finally
            {
                dal.Close();
            }
            return ret;
        }


        //public static emailsInfo GainEmailTemplateByEmailName(int languageID,string mysqlConnStr,string emailName,int clientID)

        //{
        //    emailsBll emailBll = new emailsBll();
        //    emailsInfo emailInfo = new emailsInfo();
        //    if (languageID == 7)
        //    {
        //        emailName = emailName+"_Cn";
        //    }
        //    else
        //    {
        //        emailName = emailName+"_En";
        //    }
        //    try
        //    {
        //        emailInfo = emailBll.GetEmailsByName(mysqlConnStr, emailName,clientID);
        //    }
        //    catch (Exception e)
        //    {

        //        log.Info("CRM.BusinessLogic Utils: get emailInfo by emailName error"+emailName + e.Message + e.StackTrace);
        //    }

        //    return emailInfo;
        //}
        public static List<SearchResult> GetSearchMySqlList(string mysqlConnStr, short clientID, string searchText, int pageNumber, int pageSize, out int total, string sortColumn, string sortDirection)
        {
            total = 0;
            List<SearchResult> ret = new List<SearchResult>();
            CRM.DataAccess.MysqlUtils dalUtil = new CRM.DataAccess.MysqlUtils();
            try
            {
                dalUtil.OpenCRMMySql(mysqlConnStr);
                ret = dalUtil.GetSearchList(clientID, searchText, pageNumber, pageSize, out total, sortColumn, sortDirection);
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error: Source=UtilsBll.GetSearchMySqlList Message={0}, StackTrace={1}", e.Message, e.StackTrace);
            }
            finally
            {
                dalUtil.Close();
            }

            return ret;
        }

        public static string InitEmailName(int languageID, string emailName)
        {
            if (languageID == 7)
            {
                emailName = emailName + "_Cn";
            }
            else
            {
                emailName = emailName + "_En";
            }
            return emailName;
        }
    }
}

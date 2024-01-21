using CRM.DataAccess;
using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BusinessLogic
{
    public partial class InputFormFieldsBll : BLLBase
    {
        #region SELECTs


        public CRM.BusinessEntities.InputFormFieldsInfo Select(short ClusterDB, System.Int32 fieldID)
        {
            InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
            dalInputFormFields.OpenClusterDbByClusterID(ClusterDB);

            try
            {
                return dalInputFormFields.Select(fieldID);
            }
            catch (Exception e)
            {
                _ErrorMessage = e.Message;
                _ErrorCode = -1;
                log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.Select Message=InputFormFields", _ErrorMessage);
            }
            finally
            {
                dalInputFormFields.Close();
            }

            return null;
        }


        #endregion


        public InputFormFieldsInfo UserLogin(string username, string password, string ip)
        {
            InputFormFieldsInfo result = new InputFormFieldsInfo();

            for (short clusterID = 1; clusterID <= Utils.NumOfClusters; clusterID++)
            {
                InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
                dalInputFormFields.OpenClusterDbByClusterID(clusterID);
                try
                {

                    result = dalInputFormFields.UserLogin(username, password, ip);

                    if (result != null)
                    {

                        if (result.UserLevel_Enum == UserLevelEnum.ProgramLevel)
                        {
                            #region Hotel Level User

                            result.CurrentProgramAccess = null;
                            result.ClientID = 0;
                            result.ClusterID = clusterID;

                            //get programs by userid
                            result.Programs = new List<LoyaltyProgramInfo>();
                            result.Programs = dalInputFormFields.SelByUserID(result.FieldID);

                            List<UserAccessInfo> uas = dalInputFormFields.GetUserAccess(result.FieldID);
                            if (uas != null && uas.Count > 0)
                            {
                                foreach (UserAccessInfo uai in uas)
                                {
                                    //if (result.Programs == null)
                                    //    result.Programs = new List<LoyaltyProgramInfo>();

                                    //LoyaltyProgramInfo lpi = new LoyaltyProgramInfo();
                                    //lpi.ClientID = uai.ClientID;
                                    //lpi.ClusterDB = result.ClusterID;
                                    //result.Programs.Add(lpi);

                                    if (result.CurrentProgramAccess == null)
                                    {
                                        List<UserRoleDetailInfo> ads = dalInputFormFields.GetUserAccessRoleDetails(result.FieldID, uai.ClientID);

                                        foreach (UserRoleDetailInfo ad in ads)
                                        {
                                            if (result.ClientID == 0)
                                            {
                                                result.ClientID = uai.ClientID;
                                                result.CurrentProgramAccess = new List<AdminUserAccessInfo>();
                                            }

                                            AdminUserAccessInfo aua = new AdminUserAccessInfo();
                                            //aua.AccessRight = (ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full;
                                            aua.AccessRight = (ad.AccessType == 0) ? UserAccessRightEnum.None : ((ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full);
                                            aua.CheckPoint = (UserAccessCheckPointEnum)ad.CheckPointID;
                                            aua.EntityID = result.ClientID;
                                            result.CurrentProgramAccess.Add(aua);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        break;
                    }
                }
                catch (Exception e)
                {
                    _ErrorMessage = e.Message;
                    _ErrorCode = -1;
                    log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.UserLogin Message={0}, StackTrace={1}", _ErrorMessage, e.StackTrace);
                }
                finally
                {
                    dalInputFormFields.Close();
                }
            }
            if (result != null && result.UserLevel_Enum == UserLevelEnum.SystemLevel)
            {
                //LoyaltyProgramBll bllLP = new LoyaltyProgramBll();
                result.ClientID = result.Programs[0].ClientID;
                result.ClusterID = result.Programs[0].ClusterDB;
            }

            return result;
        }

        public InputFormFieldsInfo UserSelectByUsername(string username)
        {
            InputFormFieldsInfo result = new InputFormFieldsInfo();

            for (short clusterID = 1; clusterID <= Utils.NumOfClusters; clusterID++)
            {
                InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
                dalInputFormFields.OpenClusterDbByClusterID(clusterID);
                try
                {
                    result = dalInputFormFields.UserSelectByUsername(username);
                    if (result != null)
                    {
                        if (result.UserLevel_Enum == UserLevelEnum.ProgramLevel)
                        {
                            #region Hotel Level User

                            result.CurrentProgramAccess = null;
                            result.ClientID = 0;
                            result.ClusterID = clusterID;

                            //get programs by userid
                            result.Programs = new List<LoyaltyProgramInfo>();
                            result.Programs = dalInputFormFields.SelByUserID(result.FieldID);

                            List<UserAccessInfo> uas = dalInputFormFields.GetUserAccess(result.FieldID);
                            if (uas != null && uas.Count > 0)
                            {
                                foreach (UserAccessInfo uai in uas)
                                {
                                    if (result.CurrentProgramAccess == null)
                                    {
                                        List<UserRoleDetailInfo> ads = dalInputFormFields.GetUserAccessRoleDetails(result.FieldID, uai.ClientID);

                                        foreach (UserRoleDetailInfo ad in ads)
                                        {
                                            if (result.ClientID == 0)
                                            {
                                                result.ClientID = uai.ClientID;
                                                result.CurrentProgramAccess = new List<AdminUserAccessInfo>();
                                            }

                                            AdminUserAccessInfo aua = new AdminUserAccessInfo();
                                            //aua.AccessRight = (ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full;
                                            aua.AccessRight = (ad.AccessType == 0) ? UserAccessRightEnum.None : ((ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full);
                                            aua.CheckPoint = (UserAccessCheckPointEnum)ad.CheckPointID;
                                            aua.EntityID = result.ClientID;
                                            result.CurrentProgramAccess.Add(aua);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        break;
                    }
                }
                catch (Exception e)
                {
                    _ErrorMessage = e.Message;
                    _ErrorCode = -1;
                    log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.UserSelectByUsername Message={0}, StackTrace={1}", _ErrorMessage, e.StackTrace);
                }
                finally
                {
                    dalInputFormFields.Close();
                }
            }
            if (result != null && result.UserLevel_Enum == UserLevelEnum.SystemLevel)
            {
                //ClientAccountBll bllClient = new ClientAccountBll();

                //LoyaltyProgramBll bllLP = new LoyaltyProgramBll();
                result.ClientID = result.Programs[0].ClientID;
                result.ClusterID = result.Programs[0].ClusterDB;
            }
            return result;
        }

        public int Update(short clusterID, InputFormFieldsInfo user)
        {
            int ret = 0;
            InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
            dalInputFormFields.OpenClusterDbByClusterID(clusterID);
            try
            {
                InputFormFieldsInfo result = dalInputFormFields.UserSelectByUsername(user.FieldName);
                if (result != null)
                {
                    ret = dalInputFormFields.Update(user);
                }
            }
            catch (Exception e)
            {
                _ErrorMessage = e.Message;
                _ErrorCode = -1;
                log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.UserSelectByUsername Message={0}, StackTrace={1}", _ErrorMessage, e.StackTrace);
            }
            finally
            {
                dalInputFormFields.Close();
            }

            return ret;
        }

        public InputFormFieldsInfo UserSelectByUserID(int userId)
        {
            InputFormFieldsInfo result = new InputFormFieldsInfo();

            for (short clusterID = 1; clusterID <= Utils.NumOfClusters; clusterID++)
            {
                InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
                dalInputFormFields.OpenClusterDbByClusterID(clusterID);
                try
                {
                    result = dalInputFormFields.UserSelectByUserID(userId);
                    if (result != null)
                    {
                        if (result.UserLevel_Enum == UserLevelEnum.ProgramLevel)
                        {
                            #region Hotel Level User

                            result.CurrentProgramAccess = null;
                            result.ClientID = 0;
                            result.ClusterID = clusterID;

                            //get programs by userid
                            result.Programs = new List<LoyaltyProgramInfo>();
                            result.Programs = dalInputFormFields.SelByUserID(result.FieldID);

                            List<UserAccessInfo> uas = dalInputFormFields.GetUserAccess(result.FieldID);
                            if (uas != null && uas.Count > 0)
                            {
                                foreach (UserAccessInfo uai in uas)
                                {
                                    //if (result.Programs == null)
                                    //    result.Programs = new List<LoyaltyProgramInfo>();

                                    //LoyaltyProgramInfo lpi = new LoyaltyProgramInfo();
                                    //lpi.ClientID = uai.ClientID;
                                    //lpi.ClusterDB = result.ClusterID;
                                    //result.Programs.Add(lpi);

                                    if (result.CurrentProgramAccess == null)
                                    {
                                        List<UserRoleDetailInfo> ads = dalInputFormFields.GetUserAccessRoleDetails(result.FieldID, uai.ClientID);

                                        foreach (UserRoleDetailInfo ad in ads)
                                        {
                                            if (result.ClientID == 0)
                                            {
                                                result.ClientID = uai.ClientID;
                                                result.CurrentProgramAccess = new List<AdminUserAccessInfo>();
                                            }

                                            AdminUserAccessInfo aua = new AdminUserAccessInfo();
                                            //aua.AccessRight = (ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full;
                                            aua.AccessRight = (ad.AccessType == 0) ? UserAccessRightEnum.None : ((ad.AccessType == 1) ? UserAccessRightEnum.ReadOnly : UserAccessRightEnum.Full);
                                            aua.CheckPoint = (UserAccessCheckPointEnum)ad.CheckPointID;
                                            aua.EntityID = result.ClientID;
                                            result.CurrentProgramAccess.Add(aua);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        break;
                    }
                }
                catch (Exception e)
                {
                    _ErrorMessage = e.Message;
                    _ErrorCode = -1;
                    log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.UserSelectByUsername Message={0}, StackTrace={1}", _ErrorMessage, e.StackTrace);
                }
                finally
                {
                    dalInputFormFields.Close();
                }
            }
            if (result != null && result.UserLevel_Enum == UserLevelEnum.SystemLevel)
            {
                result.ClientID = result.Programs[0].ClientID;
                result.ClusterID = result.Programs[0].ClusterDB;
            }
            return result;
        }

        public CRM.BusinessEntities.InputFormFieldsInfo SelectByClientID(short clientID, System.Int32 fieldID)
        {
            InputFormFieldsDal dalInputFormFields = new InputFormFieldsDal();
            dalInputFormFields.OpenClusterDbByClientID(clientID);

            try
            {
                return dalInputFormFields.Select(fieldID);
            }
            catch (Exception e)
            {
                _ErrorMessage = e.Message;
                _ErrorCode = -1;
                log.ErrorFormat("Error: Source=BusinessLogic.InputFormFieldsBll.SelectByClientID Message=InputFormFields", _ErrorMessage);
            }
            finally
            {
                dalInputFormFields.Close();
            }

            return null;
        }

        public List<InputFormFieldsInfo> SearchPagedBySuperUser(string searchText)
        {           
            List<InputFormFieldsInfo> ret = new List<InputFormFieldsInfo>();
            for (short clusterID = 1; clusterID <= Utils.NumOfClusters; clusterID++)
            {
                InputFormFieldsDal inputFormFieldsDal = new InputFormFieldsDal();

                try
                {
                    List<InputFormFieldsInfo> retCluster = new List<InputFormFieldsInfo>();
                    inputFormFieldsDal.OpenClusterDbByClusterID(clusterID);
                    retCluster = inputFormFieldsDal.SearchPagedBySuperUser(searchText);
                    ret.AddRange(retCluster);
                }
                catch (Exception e)
                {
                    _ErrorMessage = e.Message;
                    _ErrorCode = -1;
                    log.ErrorFormat("Error: Source=Core.BLL.InputFormFieldsBll.SearchPagedBySuperUser Message={0}, StackTrace={1}", _ErrorMessage, e.StackTrace);
                }
                finally
                {
                    inputFormFieldsDal.Close();
                }
            }
            return ret;
        }
    }
}

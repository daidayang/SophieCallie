using CRM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.BusinessLogic;
using System.Web.Security;
using CRMAdmin.Helper;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Globalization;
using CRMAdmin.Models;
// using CRM.GlobalResources;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
// using CRM.Utility;
using Antlr.Runtime.Misc;
using System.Management.Instrumentation;

namespace CRMAdmin.Controllers
{
    public class AccountController : BaseController
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Account
        public ActionResult Login(string username, string password, string returnUrl)
        {
            string ip = Util.GetIp();
            InputFormFieldsBll dalInputFormFields = new InputFormFieldsBll();

            if (IsGet)
            {
                //#region -----check user login------               
                //var loginAuth = Request.Cookies["loginAuth"];

                //if (loginAuth != null)
                //{

                //    string[] userDataArry = loginAuth.Value.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);

                //    if (userDataArry.Length == 3 && userDataArry[2] == Util.GetMd5Hash(userDataArry[0] + "*" + userDataArry[1] + "*" + ConfigHelper.GetStringFromConfig("EncryCookieValue", "")))
                //    {
                //        int timeStampExpire = Convert.ToInt32(userDataArry[1]);
                //        int timeStampNow = Convert.ToInt32(CRM.Service.Mautic.Helper.Util.timestampPHP());
                //        if (timeStampExpire >= timeStampNow)
                //        {
                //            short clientID = 0;
                //            string uName = userDataArry[0];
                //            try
                //            {
                //                if (userDataArry[0].ToUpper().Contains("ADMIN"))
                //                {
                //                    string[] userStr = userDataArry[0].ToString().Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                //                    clientID = Convert.ToInt16(userStr[1]);
                //                    uName = userStr[0];
                //                }
                //            }
                //            catch (Exception e)
                //            {
                //                log.ErrorFormat("super user get clientID error Message={0},StackTrace={1}", e.Message, e.StackTrace);
                //            }
                //            InputFormFieldsInfo userResult = new InputFormFieldsInfo();
                //            userResult.Active = true;
                //            //InputFormFieldsInfo userResult = dalInputFormFields.UserSelectByUsername(uName);
                //            if (userResult != null && userResult.Active)
                //            {                           
                //                SaveCurrentUser(userResult, clientID);
                //                if (string.IsNullOrEmpty(returnUrl))
                //                {
                //                    return Redirect("~/UserProfile/Search");
                //                }
                //                if (Url.IsLocalUrl(returnUrl))
                //                {
                //                    return Redirect(returnUrl);
                //                }
                //                return Redirect("~/UserProfile/Search");
                //            }
                //        }
                //    }
                //}
                //#endregion

                return View();
            }
            else
            {
                int userProhibitLoginTime = Helper.ConfigHelper.GetIntFromConfig("UserProhibitLoginTime", 0);
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return View();
                }

                //#region Lock user

                //if (CheckLockUser(username.Trim()))
                //{
                //    TempData["ErrorMessage"] = string.Format(CRM.GlobalResources.Login.UnableToLogYouOnBecauseYourAccountHasBeenLockedOutFor0Min, userProhibitLoginTime);//Unable to log you on because your account has been locked out for 5 min
                //    return View();
                //}

                //#endregion

                #region check user and pwd           

                string pwdEncrypted = string.Empty;
                pwdEncrypted = Util.GetMd5Hash(password.Trim() + username.Trim().ToLower());
                //InputFormFieldsInfo userResult = dalInputFormFields.UserLogin(username.Trim(), pwdEncrypted, ip);
                InputFormFieldsInfo userResult = new InputFormFieldsInfo();
                userResult.Active = true;
                userResult.FieldName = "ddaiadmin";
                if (userResult != null && userResult.Active)
                {
                    // save current user and set cookie
                    SaveCurrentUser(userResult,0);

                    #region ------modify pwd------

                    bool isModify = false;
                    string type = string.Empty;
                    ////First Time Login System Please Modify The Initial Password
                    //if (Util.CompareDays(userResult.NextExpire, userResult.DateCreated))
                    //{
                    //    isModify = true;
                    //    type = "First";
                    //    //TempData["ErrorMessage"] = CRM.GlobalResources.Login.YouAreFirstTimeLoginSystemPleaseModifyTheInitialPassword;//"You Are First Time Login System Please Modify The Initial Password That Meets The Following Requirements";
                    //}

                    //// Password expires every 90 days
                    //if (Util.CompareDate(userResult.NextExpire, DateTime.Now.Date))
                    //{
                    //    isModify = true;
                    //    type = "Expires";
                    //    //TempData["ErrorMessage"] = CRM.GlobalResources.Login.YourPasswordHasExpiredPleaseEnterANewPasswordThatMeetsTheFollowingRequirements;// "Your Password Has Expired Please Modify A New Password That Meets The Following Requirements";
                    //}

                    #endregion

                    #region------returnUrl------

                    if (isModify)
                    {
                        return Redirect("~/Account/ModifyPwd?Type=" + type);
                    }

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("~/Home/Index");
                    }
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return Redirect("~/UserProfile/Search");

                    #endregion
                }
                else
                {
                    //if (CheckLockUser(username))
                    //{
                    //    TempData["ErrorMessage"] = string.Format(CRM.GlobalResources.Login.UnableToLogYouOnBecauseYourAccountHasBeenLockedOutFor0Min, userProhibitLoginTime);// "Unable to log you on because your account has been locked out for " + UserProhibitLoginTime + " min ";
                    //    return View();
                    //}
                    //TempData["ErrorMessage"] = CRM.GlobalResources.Login.InvalidCredentialsPleaseReEnter;// "Invalid credentials  Please re-enter";
                }

                #endregion
            }

            return View();
        }

        public bool CheckLockUser(string username)
        {
            bool flag = false;
            //UserFailedlLoginBll bllUserFailedLogin = new UserFailedlLoginBll();
            //int ret = bllUserFailedLogin.GetFailedLoginCount(username, ConfigHelper.GetIntFromConfig("UserProhibitLoginTime", 0));
            //if (ret >= 3)
            //flag = true;
            return flag;
        }


        public ActionResult Logout()
        {
            SimpleSessionPersister.Clear();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            //clear cookies
            HttpCookie LoginOutCookie = new HttpCookie("loginAuth", "");
            LoginOutCookie.Domain = ConfigHelper.GetStringFromConfig("SsoCookieDomain", "");
            LoginOutCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(LoginOutCookie);

            HttpCookie ASPNETCookie = new HttpCookie("ASP.NET_SessionId", "");
            ASPNETCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(ASPNETCookie);


            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1));
            return RedirectToAction("Login");
        }

        private void SaveCurrentUser(InputFormFieldsInfo userResult,short clientID)
        {
            if (clientID==0)
            {
                clientID = userResult.ClientID;
            }

            #region ------set cookie------

            //super user (contains ADMIN), add clientID to cookie
            string uName = userResult.FieldName;
            if (userResult.FieldName.ToUpper().Contains("ADMIN"))
            {
                uName = userResult.FieldName + "_" + clientID;
            }

            StringBuilder userCookiesInfo = new StringBuilder();

            userCookiesInfo.Append(uName).Append("*");
            //Expiration timestamp
            DateTime time = new DateTime(1970, 1, 1);
            string timestamp = Convert.ToString((DateTime.UtcNow.AddMinutes(30).Ticks - time.Ticks) / 10000000);
            userCookiesInfo.Append(timestamp).Append("*");

            string cookieMd5 = Util.GetMd5Hash(userCookiesInfo + ConfigHelper.GetStringFromConfig("EncryCookieValue", ""));
            HttpCookie authCookie = new HttpCookie("loginAuth", userCookiesInfo + cookieMd5);
            authCookie.HttpOnly = true;
            authCookie.Domain = ConfigHelper.GetStringFromConfig("SsoCookieDomain", "");
            authCookie.Expires = DateTime.Now.AddMinutes(30);
            Response.Cookies.Add(authCookie);

            #endregion

            #region------save currentuser------



            var currentUser = new AppUser();
            //currentUser.FieldID = userResult.FieldID;
            //currentUser.FieldType = userResult.FieldType;
            //currentUser.FieldName = userResult.FieldName;
            ////currentUser.Access = UserResult.Access;
            //currentUser.Email = userResult.Email;
            //currentUser.Locale = userResult.Locale;
            //currentUser.NextExpire = userResult.NextExpire;
            //currentUser.LockUntilTime = userResult.LockUntilTime;

            //currentUser.LastUseTime = userResult.LastUseTime;
            //currentUser.DateCreated = userResult.DateCreated;
            //currentUser.DateDeactivated = userResult.DateDeactivated;
            //currentUser.Active = userResult.Active;
            //currentUser.Firstname = userResult.Firstname;
            //currentUser.Lastname = userResult.Lastname;
            //currentUser.Setting = userResult.Setting;

            //currentUser.Programs = userResult.Programs;
            //currentUser.Clients = userResult.Clients;
            //currentUser.ClientID = clientID;
            //currentUser.ClusterDB = userResult.ClusterID;
            //currentUser.CurrentProgramAccess = userResult.CurrentProgramAccess;
            //currentUser.UserLevel_Enum = userResult.UserLevel_Enum;
            //currentUser.UserID = userResult.UserID;
            currentUser = Util.NewAppUser();



            //select mautic
            //ClientAccountBll bllClientAccount = new ClientAccountBll();
            //ClientAccountInfo accountInfo = bllClientAccount.Select(clientID);
            ClientAccountInfo accountInfo = Util.NewAccountInfo();
            currentUser.AccountInfo = accountInfo;
            currentUser.MauticUrl = accountInfo.MauticUri;
            currentUser.MauticCustomerKey = accountInfo.MauticCustomerKey;
            currentUser.MauticCustomerSecret = accountInfo.MauticCustomerSecret;
            currentUser.MauticAccessToken = accountInfo.MauticAccessToken;
            currentUser.MauticAccessSecret = accountInfo.MauticAccessSecret;
            currentUser.MySqlConnstring = accountInfo.MYSQLConnString;
            SimpleSessionPersister.CurrentUser = currentUser;
            #endregion


        }
    }
}
using CRM.BusinessEntities;
using CRM.BusinessLogic;
// using CRM.Utility;
using CRMAdmin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CRMAdmin.Helper
{
    public class Util
    {
        public static AppUser NewAppUser()
        {
            AppUser currentUser = new AppUser();

            currentUser.FieldID = 1;
            currentUser.FieldType = 1;
            currentUser.FieldName = "ddaiAdmin";
            //currentUser.Access = UserResult.Access;
            currentUser.Email = "Email";
            currentUser.Locale = "Locale";
            currentUser.NextExpire = DateTime.Now;
            currentUser.LockUntilTime = DateTime.Now;

            currentUser.LastUseTime = DateTime.Now;
            currentUser.DateCreated = DateTime.Now;
            currentUser.DateDeactivated = DateTime.Now;
            currentUser.Active = true;
            currentUser.Firstname = "Sophie";
            currentUser.Lastname = "Dai";
            currentUser.Setting = 1;

            currentUser.Programs = null;
            currentUser.Clients = null;
            currentUser.ClientID = 1;
            currentUser.ClusterDB = 1;
            currentUser.CurrentProgramAccess = null;
            currentUser.UserLevel_Enum = 0;
            currentUser.UserID = 1;
            return currentUser;
        }

        public static CRM.BusinessEntities.ClientAccountInfo NewAccountInfo()
        {
            ClientAccountInfo currentUser = new ClientAccountInfo();
            //currentUser.MauticUrl = "MauticUri";
            currentUser.MauticCustomerKey = "MauticCustomerKey";
            currentUser.MauticCustomerSecret = "MauticCustomerSecret";
            currentUser.MauticAccessToken = "MauticAccessToken";
            currentUser.MauticAccessSecret = "MauticAccessSecret";
            //currentUser.MySqlConnstring = "MYSQLConnString";
            return currentUser;
        }


        public static string EncodeColumn(string content)
        {
            string ret = content;
            if (content.IndexOf("\"") > -1)
            {
                content = content.Replace("\"", "\"\"");
            }
            if (content.IndexOf(",") > -1 || content.IndexOf("\n") > -1)
            {
                ret = string.Format("\"{0}\"", content);
            }
            return ret;
        }

        public static DateTime MiniDate
        {
            get { return DateTime.Parse("1/1/2000"); }
        }

        public static DateTime MaxDate
        {
            get { return DateTime.Parse("1/1/2070"); }
        }

        public static string ToMoney(decimal money)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"),
                        "{0:C}", money);
        }

        public static string ToMoney(double money)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-US"),
                        "{0:C}", money);
        }

        public static string FormatPrice(decimal price, string cultureName)
        {
            string formatted = null;
            CultureInfo info = new CultureInfo(cultureName);
            switch (info.Name)
            {
                case "en-US":
                    formatted = string.Format(CultureInfo.CreateSpecificCulture("en-US"),
                        "{0:C}", price);
                    break;
                case "en-GB":
                    formatted = string.Format(CultureInfo.CreateSpecificCulture("en-GB"),
                        "{0:C}", price);
                    break;
                case "sq-AL":
                    formatted = string.Format(CultureInfo.CreateSpecificCulture("sq-AL"),
                        "{0:C}", price);
                    break;
                case "se-SE":
                    formatted = string.Format(CultureInfo.CreateSpecificCulture("se-SE"),
                        "{0:C}", price);
                    break;
            }

            return formatted;
        }


        public static string FormatPhone(string phone)
        {
            string formatedPhone = string.Empty;
            if (string.IsNullOrEmpty(phone) == false)
            {
                if (phone.IndexOf("-") > -1)
                {
                    phone = phone.Replace("-", "");
                }
                if (phone.IndexOf(" ") > -1)
                {
                    phone = phone.Replace(" ", "");
                }
                if (phone.IndexOf(" ") > -1)
                {
                    phone = phone.Replace(" ", "");
                }
                if (phone.IndexOf("(") > -1)
                {
                    phone = phone.Replace("(", "");
                }
                if (phone.IndexOf(")") > -1)
                {
                    phone = phone.Replace(")", "");
                }
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\d{1})(\d{3})(\d{3})(\d{4})", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.Regex regNoAreaCode = new System.Text.RegularExpressions.Regex(@"(\d{3})(\d{3})(\d{4})", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (string.IsNullOrEmpty(phone) == false)
                {
                    if (reg.IsMatch(phone))
                    {
                        formatedPhone = System.Text.RegularExpressions.Regex.Replace(phone, @"(\d{1})(\d{3})(\d{3})(\d{4})", "($2) $3-$4");
                    }
                    else if (regNoAreaCode.IsMatch(phone))
                    {
                        formatedPhone = System.Text.RegularExpressions.Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                    }
                    else
                    {
                        formatedPhone = phone;
                    }
                }
            }

            return formatedPhone;
        }

        public static double GetDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double degToRad = 57.29577951;
            double ans = 0;
            double miles = 0;



            if (lat1 == 0 || lat2 == 0 || lon1 == 0 || lon2 == 0)
            {
                return miles;
            }

            ans = Math.Sin(lat1 / degToRad) * Math.Sin(lat2 / degToRad) + Math.Cos(lat1 / degToRad) * Math.Cos(lat2 / degToRad) * Math.Cos(Math.Abs(lon2 - lon1) / degToRad);

            miles = 3959 * Math.Atan(Math.Sqrt(1 - ans * ans) / ans);

            //miles = Math.Ceiling(miles);
            miles = Math.Abs(miles);
            double dist = miles;
            /*
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            */
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }


        /// <summary>
        /// Hash an input string and return the hash as a 32 character hexadecimal string.
        /// </summary>
        /// <param name="input">an input string</param>
        /// <returns>a 32 character hexadecimal string</returns>
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

        #region UserLogin Compare Date
        /// <summary>
        /// Compare date same
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static bool CompareDays(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);

            if (ts1.Subtract(ts3).Duration().TotalDays == ts2.Subtract(ts3).Duration().TotalDays)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Compare datetime
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static bool CompareMinutes(DateTime dt, int minutes)
        {
            TimeSpan ts1 = new TimeSpan(dt.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts2.Subtract(ts1);

            //ConfigHelper.GetIntFromConfig("UserProhibitLoginTime",0)
            if (ts.TotalMinutes > minutes)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Compare date
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static bool CompareDate(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1);

            if (ts.TotalDays > 0)
                return true;
            else
                return false;
        }
        #endregion

        public static string GetIp()
        {
            string ip = String.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.Request.UserHostAddress))
            {
                ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(ip) || ip.Equals("::1"))
            {
                ip = "127.0.0.1";
            }
            return ip;
        }

        public static string RandomString(int size, bool lowerCase, Random random)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        #region user pwd rule
        public static string CheckPasswordRulesText(string password)
        {
            string rStr = string.Empty;

            if (password.IndexOf(" ") > 0)
            {
                rStr = "Your password can not contain space characters";
            }
            else
            {
                if (string.IsNullOrEmpty(password))
                    rStr = "NameCanNotForEmpty";
                else
                {
                    if (password.Length > 0 && password.Length < 7)
                    {
                        rStr = "Your password must be at least 7 characters";
                    }
                    else
                    {
                        if (password.Length > 20)
                        {
                            rStr = "Your password must be at max 20 characters";
                        }
                        else
                        {
                            Regex reg = new Regex(@"^(?:(?=.*[a-z])(?=.*[0-9])|(?=.*[a-z])(?=.*[^a-z0-9])|(?=.*[0-9])(?=.*[^a-z0-9])|(?=.*[a-z])(?=.*[0-9])(?=.*[^a-z0-9])).{7,20}$");
                            if (!reg.IsMatch(password))
                            {
                                rStr = "Your password must contain both numeric and alpha characters";
                            }
                        }
                    }
                }
            }
            return rStr;
        }
        #endregion

        public static UserAccessRightEnum CheckAccess(AppUser usr, UserAccessCheckPointEnum acp)
        {
            if (usr.UserLevel_Enum == UserLevelEnum.SystemLevel)
                return UserAccessRightEnum.Full;

            if (usr != null && usr.CurrentProgramAccess != null && usr.CurrentProgramAccess.Count > 0)
                foreach (AdminUserAccessInfo ua in usr.CurrentProgramAccess)
                    if (ua.CheckPoint == acp || acp == UserAccessCheckPointEnum.ANY)
                        return ua.AccessRight;
            return UserAccessRightEnum.None;
        }

        public static async Task<string> GetHttpClient(string api, string emailSend)
        {
            string url = ConfigurationManager.AppSettings["RestUrl"] + api;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = null;
                if (emailSend == null)
                {
                    response = await httpClient.GetAsync(new Uri(url));
                }
                else
                {
                    response = await httpClient.GetAsync(new Uri(url + "/" + emailSend));
                }
                return response.ToString();

            }
        }

        public static async Task<string> PostHttpClient(string api, Dictionary<string, string> param)
        {
            string url = ConfigurationManager.AppSettings["RestUrl"];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));


                var content = new FormUrlEncodedContent(param);


                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                return response.ToString();

            }
        }

        public static string GetResultHttpClient(string api, string paramData)
        {
            string result = null;
            string url = ConfigurationManager.AppSettings["RestUrl"] + api;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.GetAsync(new Uri(url)).Result;

                if (response.IsSuccessStatusCode)
                    result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public static string SaveFile(HttpPostedFileBase file)
        {
            try
            {
                var fileName = file.FileName.Insert(file.FileName.LastIndexOf('.'), "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string RootPath = ConfigurationManager.AppSettings["MediaLocalPath"];
                string filePath = RootPath + "\\ImportFile";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + "\\" + fileName;
                file.SaveAs(filePath);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int ErrorCode = 0;
        public static string ErrorRepsonse = null;
        public static string GetWebRequest(string getUrl, string paramData)
        {

            HttpWebRequest request = null;
            string responseContent = string.Empty;
            Encoding dataEncode = Encoding.GetEncoding("utf-8");

            try
            {
                if (string.IsNullOrEmpty(paramData))
                {
                    request = (HttpWebRequest)WebRequest.Create(getUrl);
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(getUrl + "?" + paramData);
                }
                request.ContentType = "application/json";
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream resStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                    {
                        responseContent = response.ResponseUri.AbsoluteUri;
                        resStream.Close();
                        response.Close();
                        request.Abort();
                    }
                }
            }
            catch (WebException we)
            {
                var webResponse = ((HttpWebResponse)we.Response);
                switch (webResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound: //404
                        {
                            ErrorCode = 404;
                            break;
                        }
                    case HttpStatusCode.BadRequest: //400
                        {
                            ErrorCode = 400;
                            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                                if (string.IsNullOrEmpty(result) == false)
                                {
                                    ErrorRepsonse = result;

                                }
                            }
                            break;
                        }
                    case HttpStatusCode.Forbidden: //403
                        {
                            ErrorCode = 403;
                            break;
                        }
                }
            }
            return responseContent;
        }
    }
}
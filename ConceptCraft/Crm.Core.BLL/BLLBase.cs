using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CRM.DataAccess;

namespace CRM.BusinessLogic
{
    public class BLLBase
    {
        protected string _ErrorMessage = string.Empty;
        protected int _ErrorCode = 0;

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static BLLBase()
        {
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
        }


        //public static int GetNextCounterValue(short counterID)
        //{
        //    int ret = -1;
        //    ReservationDal dalReservation = new ReservationDal();
        //    dalReservation.OpenLodgingIQ();
        //    try
        //    {
        //        ret = dalReservation.GetNextCounterValue(counterID);
        //    }
        //    catch (Exception e)
        //    {
        //        log.ErrorFormat("BLLBase.GetNextCounterValue() Message={0}, StackTrace={1}", e.Message, e.StackTrace);
        //    }
        //    finally
        //    {
        //        dalReservation.Close();
        //    }
        //    return ret;
        //}

        public String ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        public int ErrorCode
        {
            get { return _ErrorCode; }
        }

        public string FormatPhoneNumber( string phone)
        {
            string ret = string.Empty;

            if( string.IsNullOrEmpty(phone) || string.IsNullOrWhiteSpace(phone))
            {

            }
            else
            {
               ret = Regex.Replace(phone, "\\D", "");
            }

            return ret;
        }


    }
}

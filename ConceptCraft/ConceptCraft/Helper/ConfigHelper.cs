using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CRMAdmin.Helper
{
    public class ConfigHelper
    {
        public static int GetIntFromConfig(string paramName, int defalutVal)
        {
            int tmpInt = defalutVal;

            if (ConfigurationManager.AppSettings[paramName] != null)
            {
                if (int.TryParse(ConfigurationManager.AppSettings[paramName], out tmpInt))
                {
                    return tmpInt;
                }
                tmpInt = defalutVal;
            }
            return tmpInt;
        }

        public static short GetShortFromConfig(string paramName, short defalutVal)
        {
            short tmpShort = defalutVal;

            if (ConfigurationManager.AppSettings[paramName] != null)
            {
                if (short.TryParse(ConfigurationManager.AppSettings[paramName], out tmpShort))
                {

                    return tmpShort;
                }

                tmpShort = defalutVal;
            }

            return tmpShort;
        }

        public static bool GetBoolFromConfig(string paramName)
        {
            bool tmpBool = false;
            if (ConfigurationManager.AppSettings[paramName] != null)
            {
                if (ConfigurationManager.AppSettings[paramName] == "1" || ConfigurationManager.AppSettings[paramName].ToLower() == "true")
                {
                    tmpBool = true;
                }
                return tmpBool;
            }
            return tmpBool;
        }

        public static String GetStringFromConfig(string paramName, string defaultVal)
        {
            string tmpString = defaultVal;
            if (ConfigurationManager.AppSettings[paramName] != null)
            {
                tmpString = ConfigurationManager.AppSettings[paramName];

                return tmpString;
            }

            return tmpString;
        }
    }
}
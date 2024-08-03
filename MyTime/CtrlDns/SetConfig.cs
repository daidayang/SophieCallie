using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace ElansoEmail.Service
{
    public class SetConfig
    {
        // Harish Naik : 10/05/2016 : To Add Windows,Web Start Stop Application From Deploy Helper
        public static void UpdateConfig(string filePath,string Xname,string Xvalue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + Xname + "']");
            XmlElement ele = (XmlElement)node;
            ele.SetAttribute("value", Xvalue);
            doc.Save(filePath);
        }
        public static string GetConfig(string filePath, string Xname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + Xname + "']");
            XmlElement ele = (XmlElement)node;
            return ele.GetAttributeNode("value").Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml;

namespace Cleaner.NET
{
    public class XmlReader
    {
        public XmlDocument XmlDoc;

        public XmlReader()
        {
            XmlDoc = new XmlDocument();
        }

        public bool LoadFile(string PathFile)
        {
            try
            {
                XmlDoc.Load(PathFile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetSettingByName(string Name)
        {
            XmlNodeList Shops = XmlDoc.SelectNodes("//userSettings//Cleaner.NET.Properties.AppSettings//setting[@name = '" + Name + "']//value");
            foreach (XmlNode xn in Shops)
            {
                if (xn.HasChildNodes)
                {
                    return xn.InnerText;
                }
            }
            return null;
        }
    }
}

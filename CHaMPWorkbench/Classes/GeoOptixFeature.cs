using System;
using System.Xml;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    public class GeoOptixFeature
    {
        public readonly string GeoOptixClientSecret;
        public readonly string GeoOptixClientID;

        public GeoOptixFeature()
        {
            FileInfo geooptix_config = new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "geooptix_config.xml"));
            if (!geooptix_config.Exists)
            {
                Exception ex = new Exception("Failed to find GeoOptix configuration XML file.");
                ex.Data["GeoOptix Config Path"] = geooptix_config.FullName;
                throw ex;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(geooptix_config.FullName);
                GeoOptixClientSecret = xmlDoc.SelectSingleNode("GeoOptixConfig/ClientSecret").InnerText;
                GeoOptixClientID = xmlDoc.SelectSingleNode("GeoOptixConfig/ClientID").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading GeoOptix configuration XML file.", ex);
            }
        }
    }
}

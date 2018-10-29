using System;
using System.Xml;
using System.IO;

namespace CHaMPWorkbench.Classes
{
    public class Secrets
    {
        public readonly string GeoOptixClientSecret;
        public readonly string GeoOptixClientID;

        public readonly string AWSRegion;
        public readonly string AWSGroupName;
        public readonly string AWSKey;
        public readonly string AWSSecret;

        public Secrets()
        {
            FileInfo secrets = new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "secrets.xml"));
            if (!secrets.Exists)
            {
                Exception ex = new Exception("Failed to find secrets configuration XML file.");
                ex.Data["Secrets Config Path"] = secrets.FullName;
                throw ex;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(secrets.FullName);
                GeoOptixClientSecret = xmlDoc.SelectSingleNode("Secrets/GeoOptix/ClientSecret").InnerText;
                GeoOptixClientID = xmlDoc.SelectSingleNode("Secrets/GeoOptix/ClientID").InnerText;

                AWSRegion = xmlDoc.SelectSingleNode("Secrets/AWS/Region").InnerText;
                AWSGroupName = xmlDoc.SelectSingleNode("Secrets/AWS/GroupName").InnerText;
                AWSKey = xmlDoc.SelectSingleNode("Secrets/AWS/Key").InnerText;
                AWSSecret = xmlDoc.SelectSingleNode("Secrets/AWS/Secret").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading secrets configuration XML file.", ex);
            }
        }
    }
}

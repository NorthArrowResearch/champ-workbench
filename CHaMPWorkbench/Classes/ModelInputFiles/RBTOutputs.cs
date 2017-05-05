using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using naru.xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public class RBTOutputs
    {

        #region "Members"

        private string m_sOutputFolder = ".";
        private string m_sTempFolder;
        private string m_sResult = "Results.xml";

        private string m_sLogFile = "Log.xml";
        #endregion

        #region "Properties"

        public string OutputFolder
        {
            get { return m_sOutputFolder; }
            set { m_sOutputFolder = value; }
        }

        public string TempFolder
        {
            get { return m_sTempFolder; }
            set { m_sTempFolder = value; }
        }

        public string ResultFile
        {
            get { return m_sResult; }
            set { m_sResult = value; }
        }

        public string LogFile
        {
            get { return m_sLogFile; }
            set { m_sLogFile = value; }
        }

        #endregion

        public XmlNode CreateXMLNode(ref XmlDocument xmlDoc, string sOutputFolder)
        {
            XmlNode nodOutputs = xmlDoc.CreateElement("outputs");
            XMLHelpers.AddNode(ref xmlDoc, ref nodOutputs, "results", System.IO.Path.Combine(sOutputFolder, ResultFile));
            XMLHelpers.AddNode(ref xmlDoc, ref nodOutputs, "log", System.IO.Path.Combine(sOutputFolder, LogFile));
            XMLHelpers.AddNode(ref xmlDoc, ref nodOutputs, "temp_workspace", TempFolder);
            XMLHelpers.AddNode(ref xmlDoc, ref nodOutputs, "artifacts_path", sOutputFolder);

            return nodOutputs;
        }
    }
}

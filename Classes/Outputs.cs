using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Classes
{
    public class Outputs
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


        public void WriteToXML(System.Xml.XmlTextWriter xmlFile)
        {
            if (string.IsNullOrEmpty(OutputFolder))
            {
                throw new Exception("The output folder should be set before this method is called.");
            }

            xmlFile.WriteStartElement("outputs");
            xmlFile.WriteElementString("results", System.IO.Path.Combine(OutputFolder, ResultFile));
            xmlFile.WriteElementString("log", System.IO.Path.Combine(OutputFolder, LogFile));
            xmlFile.WriteElementString("temp_workspace", TempFolder);
            xmlFile.WriteElementString("artifacts_path", OutputFolder);
            xmlFile.WriteEndElement();
            // outputs
        }
    }
}

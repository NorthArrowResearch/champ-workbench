using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class InputFileBuilder
    {
        protected Config m_Config;
        protected Outputs m_Outputs;

        public InputFileBuilder(Config theConfig, Outputs theOutputs)
        {
            m_Config = theConfig;
            m_Outputs = theOutputs;
        }

        public void CreateFile(string sRBTInputFilePath, out XmlTextWriter xmlInput)
        {
            // Ensure that the directory exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sRBTInputFilePath));

            xmlInput = new System.Xml.XmlTextWriter(sRBTInputFilePath, System.Text.Encoding.UTF8);
            xmlInput.Formatting = System.Xml.Formatting.Indented;
            xmlInput.WriteStartElement("rbt");

            xmlInput.WriteStartElement("metadata");
            xmlInput.WriteStartElement("created");
            xmlInput.WriteElementString("tool", System.Reflection.Assembly.GetExecutingAssembly().FullName);
            xmlInput.WriteElementString("date", DateTime.Now.ToString());
            xmlInput.WriteEndElement(); // created
            xmlInput.WriteEndElement(); // metadata
        }

        public void CloseFile(ref XmlTextWriter xmlInput, String sOutputFolder)
        {
            m_Outputs.WriteToXML(xmlInput, sOutputFolder);
            m_Config.WriteToXML(xmlInput);

            xmlInput.WriteEndElement(); // rbt

            xmlInput.Close();
        }

    }
}

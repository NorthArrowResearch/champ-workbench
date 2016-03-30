using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public class RBT_InputFileBuilder
    {
        protected RBTConfig m_Config;
        protected RBTOutputs m_Outputs;

        public const string m_sDefaultRBTInputXMLFileName = "rbt_input.xml";

        public RBT_InputFileBuilder(RBTConfig theConfig, RBTOutputs theOutputs)
        {
            m_Config = theConfig;
            m_Outputs = theOutputs;
        }

        public RBTConfig Config
        {
            get { return m_Config; }
        }

        public void CreateFile(string sRBTInputFilePath, out XmlTextWriter xmlInput)
        {
            // Ensure that the directory exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sRBTInputFilePath));

            xmlInput = new System.Xml.XmlTextWriter(sRBTInputFilePath, System.Text.Encoding.UTF8);
            xmlInput.Formatting = System.Xml.Formatting.Indented;
            xmlInput.Indentation = 4;
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

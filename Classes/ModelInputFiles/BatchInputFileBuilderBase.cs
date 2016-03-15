using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    public abstract class BatchInputFileBuilderBase
    {
        public string DBCon { get; internal set; }
        public string BatchName { get; internal set; }
        public bool MakeOnlyBatch { get; internal set; }
        public System.IO.DirectoryInfo MonitoringDataFolder { get; internal set; }
        public System.IO.DirectoryInfo OutputFolder { get; internal set; }
        public string InputFileName { get; internal set; }

        protected List<BatchInputFileBuilderBase.BatchVisits> Visits;

        public BatchInputFileBuilderBase(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref List<int> lVisits, string sInputFileName)
        {
            DBCon = sDBCon;
            BatchName = sBatchName;
            MakeOnlyBatch = bMakeOnlyBatch;

            MonitoringDataFolder = new System.IO.DirectoryInfo(sMonitoringDataFolder);
            if (!MonitoringDataFolder.Exists)
                throw new Exception("The monitoring data folder must already exist.");

            OutputFolder = new System.IO.DirectoryInfo(sOutputFolder);

            Visits = new List<BatchVisits>();
            foreach (int nVisitID in lVisits)
                Visits.Add(new BatchVisits(nVisitID));

            if (string.IsNullOrEmpty(sInputFileName))
                throw new Exception("The input file name cannot be empty.");
            else
            {
                if (!sInputFileName.ToLower().EndsWith(".xml"))
                    throw new Exception("The input XML file name must end with .xml");
            }
            InputFileName = sInputFileName;
        }

        protected System.Xml.XmlDocument CreateInputXMLDoc(string sTopLevelNode, out XmlNode nodTopLevel)
        {
            // Build all the input files here.
            XmlDocument xmlDoc = new XmlDocument();

            nodTopLevel = xmlDoc.CreateElement(sTopLevelNode);
            xmlDoc.AppendChild(nodTopLevel);

            //Create an XML declaration. 
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(xmldecl, nodTopLevel);

            // Write Metadata group
            XmlNode nodMetadata = xmlDoc.CreateElement("metadata");
            nodTopLevel.AppendChild(nodMetadata);

            XmlNode nodCreated = xmlDoc.CreateElement("created");
            nodMetadata.AppendChild(nodCreated);

            XmlNode nodTool = xmlDoc.CreateElement("tool");
            nodTool.InnerText = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            nodCreated.AppendChild(nodTool);

            XmlNode nodDate = xmlDoc.CreateElement("date");
            nodDate.InnerText = DateTime.Now.ToString();
            nodCreated.AppendChild(nodTool);
 
            return xmlDoc
        }

        /// <summary>
        /// All derived classes must implement a Run method that actually generates the input files.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// This class represents a successfully generated model input file on disk.
        /// Create and store a list of these objects then insert them into the database.
        /// </summary>
        public class BatchVisits
        {
            public int VisitID { get; internal set; }
            public string InputFile { get; set; }
            public string Description { get; set; }

            public BatchVisits(int nVisitID)
            {
                System.Diagnostics.Debug.Assert(nVisitID > 0, "The visit ID must be provided.");
                VisitID = nVisitID;
                InputFile = string.Empty;
                Description = string.Empty;
            }

            public BatchVisits(int nVisitID, string sInputFile, string sDescription)
            {
                System.Diagnostics.Debug.Assert(nVisitID > 0, "The visit ID must be provided.");
                VisitID = nVisitID;

                System.Diagnostics.Debug.Assert(System.IO.File.Exists(sInputFile), "The input file should already be created and exist byt this point.");
                InputFile = sInputFile;

                Description = sDescription;
            }
        }
    }
}

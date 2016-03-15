using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CHaMPWorkbench.Classes.ModelInputFiles
{
    class GUT_BatchInputFileBuilder : BatchInputFileBuilderBase
    {
        private GUTInputProperties m_Inputs;
        
        public GUT_BatchInputFileBuilder(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref List<int> lVisits, string sInputFileName, GUTInputProperties theInputs)
            : base(sDBCon, sBatchName, bMakeOnlyBatch, sMonitoringDataFolder, sOutputFolder, ref  lVisits, sInputFileName)
        {
            m_Inputs = new GUTInputProperties(theInputs);
        }

        public override void Run()
        {

            foreach (BatchVisits aVisit in Visits)
            {
                // Build all the input files here.
                XmlNode nodTopLevel;
                XmlDocument xmlDoc = CreateInputXMLDoc("gut", out nodTopLevel);

                XmlNode nodInputs = xmlDoc.CreateElement("inputs");
                nodTopLevel.AppendChild(nodInputs);

                System.IO.DirectoryInfo dVisit = null;
                if (DataFolders.Visit(MonitoringDataFolder, aVisit.VisitID, out dVisit))
                {
                    System.IO.DirectoryInfo dSurveyGDB = null;
                    if (DataFolders.SurveyGDB(MonitoringDataFolder, aVisit.VisitID, out dSurveyGDB))
                    {
                        System.IO.DirectoryInfo dOutput = DataFolders.RBTOutputFolder(OutputFolder.FullName, dVisit);
                        dOutput.Create();

                        XmlNode nodOutput = xmlDoc.CreateElement("output_directory");
                           nodOutput.InnerText = dOutput.FullName;
                        nodInputs.AppendChild(nodOutput);

                        XmlNode nodGDB = xmlDoc.CreateElement("gdb_path");
                        nodGDB.InnerText = dSurveyGDB.FullName;
                        nodInputs.AppendChild(nodGDB);

                        XmlNode nodSiteName = xmlDoc.CreateElement("site_name");
                        nodSiteName.InnerText = "TODO: No site name yet";
                        nodInputs.AppendChild(nodSiteName);

                        m_Inputs.Serialize(ref xmlDoc, ref nodInputs);

                        System.IO.FileInfo fiSubstrate = GenerateSubstrateCSV(aVisit.VisitID);
                        if (fiSubstrate is System.IO.FileInfo)
                        {
                           XmlNode nodSubstate = xmlDoc.CreateElement("substrate_csv_path");
                            nodSubstate.InnerText=fiSubstrate.FullName;
                            nodInputs.AppendChild(nodSubstate);

                            System.IO.FileInfo fiWood = GenerateWoodCSV(aVisit.VisitID);
                            if (fiWood is System.IO.FileInfo)
                            {
                                XmlNode nodWood = xmlDoc.CreateElement("lwp_csv_path");
                                nodWood.InnerText=fiWood.FullName;
                                nodInputs.AppendChild(nodWood);
                            }
                        }
                    }
                }
            }


            // Generate the batch here.

            // Generate the batch runs here.

        }

        private System.IO.FileInfo GenerateSubstrateCSV(int nVisitID)
        {
            System.IO.FileInfo fiSubstrate = null;
            System.IO.DirectoryInfo dVisitFolder = null;
            if (DataFolders.Visit(MonitoringDataFolder, nVisitID, out dVisitFolder))
            {
                string sSubstrateCSV = System.IO.Path.Combine(DataFolders.GUTOutputFolder(OutputFolder.FullName, dVisitFolder).FullName, string.Format("substrate_visit_{0}.csv", nVisitID));
               CSVGenerators.ChannelUnitCSVGenerator csvGen = new CSVGenerators.ChannelUnitCSVGenerator(DBCon);
                fiSubstrate = csvGen.Run(nVisitID, sSubstrateCSV);
            }
            return fiSubstrate;
        }

        private System.IO.FileInfo GenerateWoodCSV(int nVisitID)
        {
            System.IO.FileInfo fiWood = null;
            System.IO.DirectoryInfo dVisitFolder = null;
            if (DataFolders.Visit(MonitoringDataFolder, nVisitID, out dVisitFolder))
            {
                string sSubstrateCSV = System.IO.Path.Combine(DataFolders.GUTOutputFolder(OutputFolder.FullName, dVisitFolder).FullName, string.Format("substrate_visit_{0}.csv", nVisitID));
                CSVGenerators.WoodCSVGenerator csvGen = new CSVGenerators.WoodCSVGenerator(DBCon);
                fiWood = csvGen.Run(nVisitID, sSubstrateCSV);
            }
            return fiWood;

        }

    }
}

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

        public GUT_BatchInputFileBuilder(string sDBCon, string sBatchName, bool bMakeOnlyBatch, string sMonitoringDataFolder, string sOutputFolder, ref Dictionary<long, string> dVisits, string sInputFileName, GUTInputProperties theInputs)
            : base(CHaMPWorkbench.Properties.Settings.Default.ModelType_GUT, sDBCon, sBatchName, bMakeOnlyBatch, sMonitoringDataFolder, sOutputFolder, ref  dVisits, sInputFileName)
        {
            m_Inputs = new GUTInputProperties(theInputs);
        }

        public override int Run(out List<string> lExceptionMessages)
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

                         // Generate a GUT input file name
                        string sGUTInputXMLFilePath = System.IO.Path.Combine(dOutput.FullName, InputFileName);

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

                        System.IO.FileInfo fiSubstrate = GenerateSubstrateCSV(aVisit.VisitID, dVisit);
                        if (fiSubstrate is System.IO.FileInfo)
                        {
                            XmlNode nodSubstate = xmlDoc.CreateElement("substrate_csv_path");
                            nodSubstate.InnerText = fiSubstrate.FullName;
                            nodInputs.AppendChild(nodSubstate);

                            System.IO.FileInfo fiWood = GenerateWoodCSV(aVisit.VisitID, dVisit);
                            if (fiWood is System.IO.FileInfo)
                            {
                                XmlNode nodWood = xmlDoc.CreateElement("lwp_csv_path");
                                nodWood.InnerText = fiWood.FullName;
                                nodInputs.AppendChild(nodWood);

                                aVisit.InputFile = sGUTInputXMLFilePath;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(aVisit.InputFile))
                    xmlDoc.Save(aVisit.InputFile);
            }

            GenerateBatchDBRecord();

            return GetResults(out lExceptionMessages);
        }

        private System.IO.FileInfo GenerateSubstrateCSV(int nVisitID, System.IO.DirectoryInfo dVisitFolder)
        {
            System.IO.FileInfo fiSubstrate = null;
            string sSubstrateCSV = System.IO.Path.Combine(DataFolders.GUTOutputFolder(OutputFolder.FullName, dVisitFolder).FullName, string.Format("substrate_visit_{0}.csv", nVisitID));
            CSVGenerators.ChannelUnitCSVGenerator csvGen = new CSVGenerators.ChannelUnitCSVGenerator(DBCon);
            fiSubstrate = csvGen.Run(nVisitID, sSubstrateCSV);
            return fiSubstrate;
        }

        private System.IO.FileInfo GenerateWoodCSV(int nVisitID, System.IO.DirectoryInfo dVisitFolder)
        {
            System.IO.FileInfo fiWood = null;
            string sSubstrateCSV = System.IO.Path.Combine(DataFolders.GUTOutputFolder(OutputFolder.FullName, dVisitFolder).FullName, string.Format("wood_visit_{0}.csv", nVisitID));
            CSVGenerators.WoodCSVGenerator csvGen = new CSVGenerators.WoodCSVGenerator(DBCon);
            fiWood = csvGen.Run(nVisitID, sSubstrateCSV);
            return fiWood;
        }
    }
}

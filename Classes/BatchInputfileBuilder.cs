using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class BatchInputfileBuilder : InputFileBuilder
    {
        private OleDbConnection m_dbCon;
        private RBTWorkbenchDataSet m_ds;

        public BatchInputfileBuilder(OleDbConnection dbCon, List<short> lFieldSeasons, Classes.Config rbtConfig, Classes.Outputs rbtOutputs)
            : base(rbtConfig, rbtOutputs)
        {
            m_ds = new RBTWorkbenchDataSet();
            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daW = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            daW.Connection = dbCon;
            daW.Fill(m_ds.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter daS = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            daS.Connection = dbCon;
            daS.Fill(m_ds.CHAMP_Sites);
            
            foreach (short nVisitYear in lFieldSeasons)
            {
                RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter daV = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
                daV.Connection = dbCon;
                daV.ClearBeforeFill = false;
                daV.FillByVisitYear(m_ds.CHAMP_Visits, nVisitYear);

                RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSeg = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
                daSeg.Connection = dbCon;
                daSeg.ClearBeforeFill = false;
                daSeg.FillByVisitYear(m_ds.CHaMP_Segments, nVisitYear);

                RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daC = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
                daC.Connection = dbCon;
                daC.ClearBeforeFill = false;
                daC.FillByVisitYear(m_ds.CHAMP_ChannelUnits, nVisitYear);
            }
            m_dbCon = dbCon;
        }

        public String Run(String sBatchName, String sDefaultInputFileName, String sParentTopoDataFolder , Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits, bool bRequireWSTIN)
        {
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
            int nSuccess = 0;
            string sResult;
            try
            {
                OleDbCommand dbInsert = new OleDbCommand("INSERT INTO RBT_Batches (BatchName, Run) Values (?, 1)", m_dbCon, dbTrans);
                dbInsert.Parameters.AddWithValue("BatchName", sBatchName);
                dbInsert.ExecuteNonQuery();

                dbInsert = new OleDbCommand("SELECT @@Identity", m_dbCon, dbTrans);
                long nBatchID = (int)dbInsert.ExecuteScalar();

                dbInsert = new OleDbCommand("INSERT INTO RBT_BatchRuns (BatchID, Summary, Inputfile, PrimaryVisitID) Values (?, ?, ?, ?)", m_dbCon, dbTrans);
                dbInsert.Parameters.AddWithValue("BatchID", nBatchID);
                OleDbParameter pSummary = dbInsert.Parameters.Add("Summary", OleDbType.VarChar);
                OleDbParameter pInputfile = dbInsert.Parameters.Add("InputFile", OleDbType.VarChar);
                OleDbParameter pPrimaryVisitID = dbInsert.Parameters.Add("VisitID", OleDbType.Integer);

                foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in m_ds.CHAMP_Visits)
                {
                    string sVisitTopofolder = m_ds.CHAMP_Visits.BuildVisitDataFolder(rVisit, sParentTopoDataFolder);
                    string sOutputfolder = m_ds.CHAMP_Visits.BuildVisitDataFolder(rVisit, this.m_Outputs.OutputFolder);
                    string sInputFile = System.IO.Path.Combine(sOutputfolder, sDefaultInputFileName);
                    sInputFile = System.IO.Path.ChangeExtension(sInputFile, "xml");

                    //pSummary.Value = DBNull.Value; //rVisit.VisitYear.ToString() + ", " + rVisit.;
                    pInputfile.Value = sInputFile;
                    pPrimaryVisitID.Value = rVisit.VisitID;

                    if (System.IO.Directory.Exists(sVisitTopofolder))
                    {
                        XmlTextWriter xmlInput;
                        CreateFile(sInputFile, out xmlInput);

                        Site theSite = new Site(rVisit.CHAMP_SitesRow);                    

                        Visit v = new Visit(rVisit, bCalculateMetrics, bChangeDetection, bMakeDEMOrthogonal, m_Config.Mode == Classes.Config.RBTModes.Hydraulic_Model_Preparation);
                        theSite.AddVisit(v);
                     
                        if (bIncludeOtherVisits)
                        {
                            foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rOtherVisit in m_ds.CHAMP_Visits)
                            {
                                if (rOtherVisit.SiteID == rVisit.SiteID && rOtherVisit.VisitID != rVisit.VisitID)
                                {
                                    Visit vOther = new Visit(rOtherVisit, false, false, bMakeDEMOrthogonal, false);
                                    theSite.AddVisit(vOther);
                                    //vOther.WriteToXML(ref xmlInput, sVisitTopofolder);
                                }
                            }
                        }
                        pSummary.Value = theSite.NameForDatabaseBatch;

                        xmlInput.WriteStartElement("sites");
                        theSite.WriteToXML(xmlInput, sParentTopoDataFolder, bRequireWSTIN);
                        xmlInput.WriteEndElement(); // sites

                        // Write the end of the file
                        CloseFile(ref xmlInput, sOutputfolder);

                        dbInsert.ExecuteNonQuery();
                        nSuccess += 1;
                    }
                }

                dbTrans.Commit();
                sResult = nSuccess.ToString("#,##0") + " input files generated successfully.";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                sResult = nSuccess.ToString("#,##0") + " input files were generated successfully, but then an error occurred and none of the records were stored in the workbench database. The error was: " + ex.Message;
            }

            return sResult;
        }
    }
}

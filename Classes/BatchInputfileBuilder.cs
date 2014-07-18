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

        public String Run(String sBatchName, String sDefaultInputFileName, String sParentTopoDataFolder , Boolean bCalculateMetrics, Boolean bChangeDetection, Boolean bMakeDEMOrthogonal, bool bIncludeOtherVisits)
        {
            OleDbTransaction dbTrans = m_dbCon.BeginTransaction();

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
                    string sInputFile = m_ds.CHAMP_Visits.BuildVisitDataFolder(rVisit, this.m_Outputs.OutputFolder);
                    sInputFile = System.IO.Path.Combine(sInputFile, sDefaultInputFileName);
                    sInputFile = System.IO.Path.ChangeExtension(sInputFile, "xml");

                    pSummary.Value = DBNull.Value; //rVisit.VisitYear.ToString() + ", " + rVisit.;
                    pInputfile.Value = sInputFile;
                    pPrimaryVisitID.Value = rVisit.VisitID;

                    if (System.IO.Directory.Exists(sVisitTopofolder))
                    {
                        XmlTextWriter xmlInput;
                        CreateFile(sInputFile, out xmlInput);

                        Visit v = new Visit(rVisit, bCalculateMetrics, bChangeDetection, bChangeDetection || bMakeDEMOrthogonal);
                        v.WriteToXML(ref xmlInput, sVisitTopofolder);

                        if (bIncludeOtherVisits)
                        {
                            foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rOtherVisit in m_ds.CHAMP_Visits)
                            {
                                if (rOtherVisit.SiteID == rVisit.SiteID && rOtherVisit.VisitID != rVisit.VisitID)
                                {
                                    Visit vOther = new Visit(rOtherVisit, false, bChangeDetection && rOtherVisit.IsPrimary, bChangeDetection || bMakeDEMOrthogonal);
                                    vOther.WriteToXML(ref xmlInput, sVisitTopofolder);
                                }
                            }
                        }

                        // Write the end of the file
                        CloseFile(ref xmlInput);

                        dbInsert.ExecuteNonQuery();
                    }
                }

                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                throw;
            }

            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class frmScavengeVisitInfo : Form
    {
        private OleDbConnection m_dbCon;

        public frmScavengeVisitInfo(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select CHaMP All Measurements Access Database";
            dlg.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb";

            if (!String.IsNullOrWhiteSpace(txtDatabase.Text) && System.IO.File.Exists(txtDatabase.Text))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(txtDatabase.Text);
                dlg.FileName = System.IO.Path.GetFileName(txtDatabase.Text);
            }
            else
            {
                System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtDatabase.Text = dlg.FileName;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtDatabase.Text) || !System.IO.File.Exists(txtDatabase.Text))
            {
                MessageBox.Show("Please enter a valid path to the CHaMP exported 'All Measurements' Access database.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
              String sMsg =  ScavengeVisitInfo(txtDatabase.Text);
              MessageBox.Show(sMsg, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Processing VisitInfo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private String ScavengeVisitInfo(String sDatabasePath)
        {        
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            RBTWorkbenchDataSet ds = new RBTWorkbenchDataSet();

            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daWatersheds = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            daWatersheds.Connection = m_dbCon;
            daWatersheds.Fill(ds.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter daSites = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            daSites.Connection = m_dbCon;
            daSites.Fill(ds.CHAMP_Sites);

            RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter daVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
            daVisits.Connection = m_dbCon;
            daVisits.Fill(ds.CHAMP_Visits);

            RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSegments = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
            daSegments.Connection = m_dbCon;
            daSegments.Fill(ds.CHaMP_Segments);

            RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daChannelUnits = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
            daChannelUnits.Connection = m_dbCon;
            daChannelUnits.Fill(ds.CHAMP_ChannelUnits);


            String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sDatabasePath);
            using (OleDbConnection dbCHaMP = new OleDbConnection(sDB))
            {
                dbCHaMP.Open();
                UpdateWatersheds(dbCHaMP, daWatersheds, ds.CHAMP_Watersheds);
                UpdateSites(dbCHaMP, daSites, ds.CHAMP_Sites);
                UpdateVisits(dbCHaMP, daVisits, ds.CHAMP_Visits);
                UpdateSegmentsAndUnits(dbCHaMP, daSegments, daChannelUnits, ds);
            }

            String sMsg = "Process completed successfully.";
            sMsg += ds.CHAMP_Watersheds.Rows.Count.ToString("#,##0") + " watersheds";
            sMsg += ", " + ds.CHAMP_Sites.Rows.Count.ToString("#,##0") + " sites";
            sMsg += ", " + ds.CHAMP_Visits.Rows.Count.ToString("#,##0") + " visits";
            sMsg += ", " + ds.CHaMP_Segments.Rows.Count.ToString("#,##0") + " segments";
            sMsg += ", " + ds.CHAMP_ChannelUnits.Rows.Count.ToString("#,##0") + " channel units.";
            return sMsg;
        }

        private void UpdateWatersheds(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daWatersheds, RBTWorkbenchDataSet.CHAMP_WatershedsDataTable dtWorkbench)
        {
            String sSQL = "SELECT WatershedID, WatershedName FROM ChannelUnit WHERE (WatershedID IS NOT NULL) AND (WatershedName IS NOT NULL) GROUP BY WatershedID, WatershedName";
            using (OleDbCommand dbCom = new OleDbCommand(sSQL, dbCHaMP))
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    RBTWorkbenchDataSet.CHAMP_WatershedsRow r = dtWorkbench.FindByWatershedID((int)dbRead["WatershedID"]);
                    if (r == null)
                    {
                        r = dtWorkbench.NewCHAMP_WatershedsRow();
                        r.WatershedID= (int)dbRead["WatershedID"];
                        r.WatershedName = (string)dbRead["WatershedName"];
                        r.SetFolderNull();
                        dtWorkbench.AddCHAMP_WatershedsRow(r);
                    }
                    else
                    {
                        r.BeginEdit();
                        r.WatershedName = (string)dbRead["WatershedName"];
                        r.EndEdit();
                    }
                }
                daWatersheds.Update(dtWorkbench);
            }
        }

        private void UpdateSites(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter da, RBTWorkbenchDataSet.CHAMP_SitesDataTable dtWorkbench)
        {
            String sSQL = "SELECT ProgramSiteID AS ID, SiteName AS Title, WatershedID FROM VisitInformation WHERE (ProgramSiteID IS NOT NULL) AND (SiteName IS NOT NULL) GROUP BY ProgramSiteID, SiteName, WatershedID";
            using (OleDbCommand dbCom = new OleDbCommand(sSQL, dbCHaMP))
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    RBTWorkbenchDataSet.CHAMP_SitesRow r = dtWorkbench.FindBySiteID((int)dbRead["ID"]);
                    if (r == null)
                    {
                        r = dtWorkbench.NewCHAMP_SitesRow();
                        r.SiteID = (int)dbRead["ID"];
                        r.SiteName = (string)dbRead["Title"];
                        r.WatershedID = (int)dbRead["WatershedID"];
                        dtWorkbench.AddCHAMP_SitesRow(r);
                    }
                    else
                    {
                        r.BeginEdit();
                        r.SiteName = (string)dbRead["Title"];
                        r.EndEdit();
                    }
                }
                da.Update(dtWorkbench);
            }
        }

        private void UpdateVisits(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter da, RBTWorkbenchDataSet.CHAMP_VisitsDataTable dtWorkbench)
        {
            String sSQL = "SELECT VisitID AS ID, HitchName, CrewName, VisitDate, ProgramSiteID AS SiteID, [Primary Visit] FROM VisitInformation WHERE (VisitID IS NOT NULL) AND (ProgramSiteID IS NOT NULL)";
            using (OleDbCommand dbCom = new OleDbCommand(sSQL, dbCHaMP))
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    RBTWorkbenchDataSet.CHAMP_VisitsRow r = dtWorkbench.FindByVisitID((int)dbRead["ID"]);
                    if (r == null)
                        r = dtWorkbench.NewCHAMP_VisitsRow();

                    r.VisitID = (int)dbRead["ID"];
                    r.SiteID = (int)dbRead["SiteID"];

                    if (System.Convert.IsDBNull(dbRead["CrewName"]))
                        r.SetCrewNameNull();
                    else
                        r.CrewName = (string)dbRead["CrewName"];

                    if (System.Convert.IsDBNull(dbRead["HitchName"]))
                        r.HitchName = "None";
                    else
                        r.HitchName = (string)dbRead["HitchName"];

                    r.IsPrimary = System.Convert.IsDBNull(dbRead["Primary Visit"]) || string.Compare((string)dbRead["Primary Visit"], "Yes", true) != 0;

                    //if (System.Convert.IsDBNull(dbRead["HitchID"]))
                        r.SetHitchIDNull();
                   // else
                    //    r.HitchID = (int)dbRead["HitchID"];

                    r.SampleDate = (DateTime)dbRead["VisitDate"];
                    r.VisitYear = (short)r.SampleDate.Year;

                    if (r.RowState == DataRowState.Detached)
                        dtWorkbench.AddCHAMP_VisitsRow(r);
                }
                da.Update(dtWorkbench);
            }
        }

        private void UpdateSegmentsAndUnits(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSegments, RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daUnits, RBTWorkbenchDataSet ds)
        {
            // Delete all the existing channel segments. This will cascade to channel units.
            String sSQL = "DELETE FROM CHaMP_Segments";
            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);

            dbCom.ExecuteNonQuery();

            // Now force a clea by trying to refill the datasets
            daSegments.Fill(ds.CHaMP_Segments);
            //daUnits.Fill(ds.CHAMP_ChannelUnits);
  
            sSQL = "SELECT VisitID, SegmentNumber, SegmentType FROM ChannelSegment WHERE (VisitID IS NOT NULL) AND (SegmentNumber IS NOT NULL) AND (SegmentType IS NOT NULL)";
            dbCom = new OleDbCommand(sSQL, dbCHaMP);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                RBTWorkbenchDataSet.CHaMP_SegmentsRow r = ds.CHaMP_Segments.NewCHaMP_SegmentsRow();
                r.SegmentNumber = (int)dbRead["SegmentNumber"];
                r.SegmentName = (string)dbRead["SegmentType"];
                r.VisitID = (int)dbRead["VisitID"];
                ds.CHaMP_Segments.AddCHaMP_SegmentsRow(r);
            }
            daSegments.Update(ds.CHaMP_Segments);
            dbRead.Close();
            
            // Now generate fake segments for prior year visits

            //sSQL = "INSERT INTO CHAMP_Segments (VisitID, SegmentNumber, SegmentName) SELECT VisitID, 1, 'Main Channel' FROM CHaMP_Visits WHERE (VisitYear < 2014) AND VisitID NOT IN (SELECT VisitID FROM CHaMP_Segments GROUP BY VisitID)";
           // dbCom = new OleDbCommand(sSQL, m_dbCon);
           // dbCom.ExecuteNonQuery();

            // Reload the datasets
            daSegments.Fill(ds.CHaMP_Segments);



            daUnits.ClearBeforeFill = true;
            daUnits.Fill(ds.CHAMP_ChannelUnits);

            OleDbConnection conInsert = new OleDbConnection(m_dbCon.ConnectionString);
            conInsert.Open();

            OleDbCommand comInsert = new OleDbCommand("INSERT INTO CHaMP_ChannelUnits (SegmentID, ChannelUnitNumber, Tier1, Tier2) VALUES (?, ?, ?, ?)", conInsert);
            OleDbParameter pSegmentID = comInsert.Parameters.Add("SegmentID", OleDbType.Integer);
            OleDbParameter pCUNumber = comInsert.Parameters.Add("ChannelUnitNumber", OleDbType.Integer);
            OleDbParameter pTier1 = comInsert.Parameters.Add("Tier1", OleDbType.VarChar);
            OleDbParameter pTier2 = comInsert.Parameters.Add("Tier2", OleDbType.VarChar);
                        
            // Now process the Channel Units
            sSQL = "SELECT VisitID, ChannelSegment, Tier1, Tier2, ChannelUnitNumber FROM ChannelUnit WHERE (VisitID IS NOT NULL) AND (ChannelSegment IS NOT NULL) AND (ChannelUnitNumber IS NOT NULL)";
            dbCom = new OleDbCommand(sSQL, dbCHaMP);
            dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                RBTWorkbenchDataSet.CHAMP_ChannelUnitsRow r = ds.CHAMP_ChannelUnits.NewCHAMP_ChannelUnitsRow();

                foreach (RBTWorkbenchDataSet.CHaMP_SegmentsRow rSeg in ds.CHaMP_Segments)
                {
                    if (rSeg.VisitID == (int)dbRead["VisitID"] && rSeg.SegmentNumber == (int)dbRead["ChannelSegment"])
                    {
                        System.Diagnostics.Debug.WriteLine(rSeg.VisitID.ToString() + ", " + rSeg.SegmentID.ToString());// + ", " + r.ChannelUnitNumber.ToString() + ", " + r.Tier1 + ", " + r.Tier2);

                        try
                        {
                            pSegmentID.Value = rSeg.SegmentID;
                            pCUNumber.Value = (int)dbRead["ChannelUnitNumber"];
                            pTier1.Value = (string)dbRead["Tier1"];
                            pTier1.Size = ((string)dbRead["Tier1"]).Length;
                            pTier2.Value = (string)dbRead["Tier2"];
                            pTier2.Size = ((string)dbRead["Tier2"]).Length;

                            comInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                        //r.SegmentID = rSeg.SegmentID;
                        //r.ChannelUnitNumber = (int)dbRead["ChannelUnitNumber"];
                        //r.Tier1 = (string)dbRead["Tier1"];
                        //r.Tier2 = (string)dbRead["Tier2"];
                        //ds.CHAMP_ChannelUnits.AddCHAMP_ChannelUnitsRow(r);


                    }
                }
            }
           // daUnits.Update(ds.CHAMP_ChannelUnits);
            daUnits.Fill(ds.CHAMP_ChannelUnits);
        }
    }
}

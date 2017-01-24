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
    public partial class frmImportCHaMPInfo : Form
    {
        private OleDbConnection m_dbCon;

        public frmImportCHaMPInfo(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtDatabase.Text) || !System.IO.File.Exists(txtDatabase.Text))
            {
                MessageBox.Show("Please enter a valid path to the CHaMP exported 'All Measurements' Access database.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            string sSurveyDesign = txtSurveyDesign.Text;
            if (chkImportFish.Checked && (String.IsNullOrWhiteSpace(txtSurveyDesign.Text) || !System.IO.File.Exists(txtSurveyDesign.Text)))
            {
                MessageBox.Show("Please enter a valid path to the CHaMP exported 'Survey Design' Access database.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            string sProjectMetricsDB = txtProgramMetrics.Text;
            if (chkExtendedSiteInfo.Checked && (String.IsNullOrWhiteSpace(txtProgramMetrics.Text) || !System.IO.File.Exists(txtProgramMetrics.Text)))
            {
                MessageBox.Show("Please enter a valid path to the CHaMP exported 'Program Metrics' Access database.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                String sMsg = ScavengeVisitInfo(txtDatabase.Text, sSurveyDesign, sProjectMetricsDB);
                MessageBox.Show(sMsg, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private String ScavengeVisitInfo(String sDatabasePath, string sSurveyDesignDB, string sProjectMetricsDB)
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
                UpdateLargeWoodCount(dbCHaMP.ConnectionString, daWatersheds.Connection.ConnectionString);


                if (chkImportFish.Checked)
                    UpdateSiteFishInfo(daSites, ref ds, sSurveyDesignDB);

                if (chkExtendedSiteInfo.Checked)
                    UpdateExtendedSiteInfo(sProjectMetricsDB);
            }

            String sMsg = "Process completed successfully.";
            sMsg += ds.CHAMP_Watersheds.Rows.Count.ToString("#,##0") + " watersheds";
            sMsg += ", " + ds.CHAMP_Sites.Rows.Count.ToString("#,##0") + " sites";
            sMsg += ", " + ds.CHAMP_Visits.Rows.Count.ToString("#,##0") + " visits";
            sMsg += ", " + ds.CHaMP_Segments.Rows.Count.ToString("#,##0") + " segments";
            sMsg += ", " + ds.CHAMP_ChannelUnits.Rows.Count.ToString("#,##0") + " channel units.";
            return sMsg;
        }

        private void LogCHaMPDataUpdate(string sDBCon, string sTable)
        {
            using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT MAX(Version) AS MaxVersion FROM VersionChangeLog", dbCon);
                object objVersion = dbCom.ExecuteScalar();
                if (DBNull.Value != objVersion && objVersion is int)
                {
                    string sDescription = string.Format("CHaMP {0} information updated by {1} using the computer {2} on {3:dd MMM yyyy}.", sTable.ToLower(), Environment.UserName, Environment.MachineName, DateTime.Now);

                    dbCom = new OleDbCommand("INSERT INTO VersionChangeLog (Version, Description) VALUES (@Version, @Description)", dbCon);
                    dbCom.Parameters.AddWithValue("@Version", (int)objVersion);
                    OleDbParameter pDescription = dbCom.Parameters.AddWithValue("@Description", sDescription);
                    pDescription.Size = sDescription.Length;

                    dbCom.ExecuteNonQuery();
                }
            }
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
                        r.WatershedID = (int)dbRead["WatershedID"];
                        r.WatershedName = (string)dbRead["WatershedName"];
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
                LogCHaMPDataUpdate(daWatersheds.Connection.ConnectionString, "Watersheds");
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
                LogCHaMPDataUpdate(da.Connection.ConnectionString, "sites");
            }
        }

        private void UpdateSiteFishInfo(RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter da, ref RBTWorkbenchDataSet dsWorkbench, string sSurveyDesignDB)
        {
            dsWorkbench.AcceptChanges();

            String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sSurveyDesignDB);
            OleDbConnection conSurveyDesign = new OleDbConnection(sDB);
            conSurveyDesign.Open();

            OleDbCommand comSurveyDesign = new OleDbCommand("SELECT UC_CHIN, SN_CHIN, LC_STEEL, MC_STEEL, UC_STEEL, SN_STEEL FROM 2_MasterSample_All WHERE (Site_ID = ?) AND (WatershedName = ?)", conSurveyDesign);
            OleDbParameter pSiteID = comSurveyDesign.Parameters.Add("SiteID", OleDbType.VarChar);
            OleDbParameter pWatershedName = comSurveyDesign.Parameters.Add("WatershedName", OleDbType.VarChar);

            string[] sFishFields = { "UC_Chin", "SN_Chin", "LC_Steel", "MC_STeel", "UC_Steel", "SN_Steel" };

            // Loop over all sites stored in the workbench database
            foreach (RBTWorkbenchDataSet.CHAMP_SitesRow rSite in dsWorkbench.CHAMP_Sites)
            {
                // Query the record for this site in the survey design database
                pSiteID.Value = rSite.SiteName;
                pWatershedName.Value = rSite.CHAMP_WatershedsRow.WatershedName;
                OleDbDataReader dbRead = comSurveyDesign.ExecuteReader();
                if (dbRead.Read())
                {
                    // Loop over all the fish presence fields (must be named same in both DBs)
                    foreach (string aField in sFishFields)
                    {
                        if (string.Compare(aField, "UC_Chin", true) == 0 && rSite.RowState != DataRowState.Unchanged)
                        {
                            System.Diagnostics.Debug.Print("warning");
                        }

                        if (DBNull.Value == dbRead[aField])
                            rSite[aField] = DBNull.Value;
                        else
                            rSite[aField] = !string.IsNullOrWhiteSpace((string)dbRead[aField]);
                    }
                }
                else
                {
                    // No site in survey design. Set all fish related information to Null
                    foreach (string aField in sFishFields)
                        rSite[aField] = DBNull.Value;
                }
                dbRead.Close();

                try
                {
                    da.Update(dsWorkbench.CHAMP_Sites);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }

            LogCHaMPDataUpdate(da.Connection.ConnectionString, "fish information");
        }

        private void UpdateVisits(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter da, RBTWorkbenchDataSet.CHAMP_VisitsDataTable dtWorkbench)
        {
            String sSQL = "SELECT V.VisitID AS ID, V.HitchName, V.CrewName, V.VisitDate, V.ProgramSiteID AS SiteID, V.[Primary Visit], V.PanelName, M.VisitPhase, M.VisitStatus, V.AEM, V.HasStreamTempLogger, V.[Has Fish Data], V.[QC Visit], V.CategoryName, V.ProtocolID" +
                        " FROM VisitInformation AS V LEFT JOIN MetricAndCovariates AS M ON M.VisitID = V.VisitID" +
                        " WHERE ( (V.[VisitID] Is Not Null) AND (V.[ProgramSiteID] Is Not Null) )" +
                        " GROUP BY V.VisitID, V.HitchName, V.CrewName, V.VisitDate, V.ProgramSiteID, V.[Primary Visit], V.PanelName, M.VisitPhase, M.VisitStatus, M.Organization, V.AEM, V.HasStreamTempLogger, V.[Has Fish Data], V.[QC Visit], V.CategoryName, V.ProtocolID";

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

                    if (dbRead["Primary Visit"] == DBNull.Value)
                        r.IsPrimary = false;
                    else
                        r.IsPrimary = string.Compare((string)dbRead["Primary Visit"], "Yes", true) == 0;

                    //if (System.Convert.IsDBNull(dbRead["HitchID"]))
                    r.SetHitchIDNull();
                    // else
                    //    r.HitchID = (int)dbRead["HitchID"];

                    r.SampleDate = (DateTime)dbRead["VisitDate"];
                    r.VisitYear = (short)r.SampleDate.Year;

                    if (System.Convert.IsDBNull(dbRead["PanelName"]))
                        r.SetPanelNameNull();
                    else
                        r.PanelName = (string)dbRead["PanelName"];

                    if (System.Convert.IsDBNull(dbRead["VisitPhase"]))
                        r.SetVisitPhaseNull();
                    else
                        r.VisitPhase = (string)dbRead["VisitPhase"];

                    // Organization is stored as long text field in cm.org export. Long text fields 
                    // seem to appear as garbled text in left join queries. So default this field 
                    // to null here and then update this field separately below.
                    r.SetOrganizationNull();

                    if (System.Convert.IsDBNull(dbRead["AEM"]))
                        r.SetAEMNull();
                    else
                        r.AEM = string.Compare((string)dbRead["AEM"], "Yes", true) == 0;

                    if (System.Convert.IsDBNull(dbRead["HasStreamTempLogger"]))
                        r.SetHasStreamTempLoggerNull();
                    else
                        r.HasStreamTempLogger = (bool)dbRead["HasStreamTempLogger"];

                    if (System.Convert.IsDBNull(dbRead["Has Fish Data"]))
                        r.SetHasFishDataNull();
                    else
                        r.HasFishData = string.Compare((string)dbRead["Has Fish Data"], "Yes", true) == 0;

                    if (System.Convert.IsDBNull(dbRead["QC Visit"]))
                        r.SetQCVisitNull();
                    else
                        r.QCVisit = string.Compare((string)dbRead["QC Visit"], "Yes", true) == 0;

                    if (System.Convert.IsDBNull(dbRead["VisitStatus"]))
                        r.SetVisitStatusNull();
                    else
                        r.VisitStatus = (string)dbRead["VisitStatus"];

                    if (System.Convert.IsDBNull(dbRead["CategoryName"]))
                        r.SetCategoryNameNull();
                    else
                        r.CategoryName = (string)dbRead["CategoryName"];

                    if (System.Convert.IsDBNull(dbRead["ProtocolID"]))
                        r.SetProtocolIDNull();
                    else
                        r.ProtocolID = dbRead.GetInt32(dbRead.GetOrdinal("ProtocolID"));

                    if (r.RowState == DataRowState.Detached)
                        dtWorkbench.AddCHAMP_VisitsRow(r);
                }

                // See comment above about long text fields in left joins                
                using (OleDbCommand dbCom2 = new OleDbCommand("SELECT VisitID, Organization FROM MetricAndCovariates WHERE Organization Is Not NULL", dbCHaMP))
                {
                    OleDbDataReader dbRead2 = dbCom2.ExecuteReader();
                    while (dbRead2.Read())
                    {
                        RBTWorkbenchDataSet.CHAMP_VisitsRow r = dtWorkbench.FindByVisitID((int)dbRead2["VisitID"]);
                        if (r != null)
                        {
                            string sOrganization = dbRead2.GetString(dbRead2.GetOrdinal("Organization"));
                            r.Organization = sOrganization.Substring(0, Math.Min(100, sOrganization.Length));
                        }
                    }
                }

                da.Update(dtWorkbench);
                LogCHaMPDataUpdate(da.Connection.ConnectionString, "visits");
            }
        }

        private void UpdateSegmentsAndUnits(OleDbConnection dbCHaMP, RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSegments, RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daUnits, RBTWorkbenchDataSet ds)
        {
            // Delete all the existing channel segments. This will cascade to channel units.
            String sSQL = "DELETE FROM CHaMP_Segments";
            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);

            dbCom.ExecuteNonQuery();

            // Now force a clea by trying to refill the datasets
            ds.CHAMP_ChannelUnits.Clear();
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
                if (ds.CHAMP_Visits.FindByVisitID(r.VisitID) != null)
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

            OleDbCommand comInsert = new OleDbCommand("INSERT INTO CHaMP_ChannelUnits (SegmentID, ChannelUnitNumber, Tier1, Tier2, BouldersGT256, Cobbles65255, CoarseGravel1764, FineGravel316, Sand0062, FinesLT006, SumSubstrateCover) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conInsert);
            OleDbParameter pSegmentID = comInsert.Parameters.Add("SegmentID", OleDbType.Integer);
            OleDbParameter pCUNumber = comInsert.Parameters.Add("ChannelUnitNumber", OleDbType.Integer);
            OleDbParameter pTier1 = comInsert.Parameters.Add("Tier1", OleDbType.VarChar);
            OleDbParameter pTier2 = comInsert.Parameters.Add("Tier2", OleDbType.VarChar);

            OleDbParameter pBouldersGT256 = comInsert.Parameters.Add("BouldersGT256", OleDbType.Integer);
            OleDbParameter pCobbles65255 = comInsert.Parameters.Add("Cobbles65255", OleDbType.Integer);
            OleDbParameter pCoarseGravel1764 = comInsert.Parameters.Add("CoarseGravel1764", OleDbType.Integer);
            OleDbParameter pFineGravel316 = comInsert.Parameters.Add("FineGravel316", OleDbType.Integer);
            OleDbParameter pSand0062 = comInsert.Parameters.Add("Sand0062", OleDbType.Integer);
            OleDbParameter pFinesLT006 = comInsert.Parameters.Add("FinesLT006", OleDbType.Integer);
            OleDbParameter pSumSubstrateCover = comInsert.Parameters.Add("SumSubstrateCover", OleDbType.Integer);

            // Now process the Channel Units
            sSQL = "SELECT ChannelUnit.VisitID, ChannelUnit.ChannelSegment_SegmentNumber AS SegmentNumber, ChannelUnit.Tier1, ChannelUnit.Tier2, ChannelUnit.ChannelUnitNumber, SubstrateCover.BouldersGT256, SubstrateCover.Cobbles65255, SubstrateCover.CoarseGravel1764, SubstrateCover.FineGravel316, SubstrateCover.Sand0062, SubstrateCover.FinesLT006, SubstrateCover.SumSubstrateCover FROM ChannelUnit LEFT JOIN SubstrateCover ON (ChannelUnit.VisitID = SubstrateCover.VisitID) AND (ChannelUnit.ChannelUnitID = SubstrateCover.ChannelUnitID) WHERE (((ChannelUnit.[VisitID]) Is Not Null) AND ((ChannelUnit.[ChannelSegment]) Is Not Null) AND ((ChannelUnit.[ChannelUnitNumber]) Is Not Null))";
            dbCom = new OleDbCommand(sSQL, dbCHaMP);
            dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                RBTWorkbenchDataSet.CHAMP_ChannelUnitsRow r = ds.CHAMP_ChannelUnits.NewCHAMP_ChannelUnitsRow();

                foreach (RBTWorkbenchDataSet.CHaMP_SegmentsRow rSeg in ds.CHaMP_Segments)
                {
                    if (rSeg.VisitID == (int)dbRead["VisitID"] && rSeg.SegmentNumber == (int)dbRead["SegmentNumber"])
                    {
                        //System.Diagnostics.Debug.WriteLine(rSeg.VisitID.ToString() + ", " + rSeg.SegmentID.ToString());// + ", " + r.ChannelUnitNumber.ToString() + ", " + r.Tier1 + ", " + r.Tier2);

                        try
                        {
                            pSegmentID.Value = rSeg.SegmentID;
                            pCUNumber.Value = (int)dbRead["ChannelUnitNumber"];
                            pTier1.Value = (string)dbRead["Tier1"];
                            pTier1.Size = ((string)dbRead["Tier1"]).Length;
                            pTier2.Value = (string)dbRead["Tier2"];
                            pTier2.Size = ((string)dbRead["Tier2"]).Length;

                            if (DBNull.Value == dbRead["BouldersGT256"])
                                pBouldersGT256.Value = DBNull.Value;
                            else
                                pBouldersGT256.Value = (int)dbRead["BouldersGT256"];

                            if (DBNull.Value == dbRead["Cobbles65255"])
                                pCobbles65255.Value = DBNull.Value;
                            else
                                pCobbles65255.Value = (int)dbRead["Cobbles65255"];

                            if (DBNull.Value == dbRead["CoarseGravel1764"])
                                pCoarseGravel1764.Value = DBNull.Value;
                            else
                                pCoarseGravel1764.Value = (int)dbRead["CoarseGravel1764"];

                            if (DBNull.Value == dbRead["FineGravel316"])
                                pFineGravel316.Value = DBNull.Value;
                            else
                                pFineGravel316.Value = (int)dbRead["FineGravel316"];

                            if (DBNull.Value == dbRead["Sand0062"])
                                pSand0062.Value = DBNull.Value;
                            else
                                pSand0062.Value = (int)dbRead["Sand0062"];

                            if (DBNull.Value == dbRead["FinesLT006"])
                                pFinesLT006.Value = DBNull.Value;
                            else
                                pFinesLT006.Value = (int)dbRead["FinesLT006"];

                            if (DBNull.Value == dbRead["SumSubstrateCover"])
                                pSumSubstrateCover.Value = DBNull.Value;
                            else
                                pSumSubstrateCover.Value = (int)dbRead["SumSubstrateCover"];

                            comInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            // daUnits.Update(ds.CHAMP_ChannelUnits);
            daUnits.Fill(ds.CHAMP_ChannelUnits);
            LogCHaMPDataUpdate(daSegments.Connection.ConnectionString, "segments and channel units");
        }

        #region BrowseEvents

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            BrowseDatabase(ref txtDatabase, "Select CHaMP All Measurements Access Database");
        }

        private void cmdBrowseSurveyDesign_Click(object sender, EventArgs e)
        {
            BrowseDatabase(ref txtSurveyDesign, "CHaMP Survey Design Database");
        }

        private void cmdBrowseProgramMetrics_Click(object sender, EventArgs e)
        {
            BrowseDatabase(ref txtProgramMetrics, "CHaMP Program Metrics Database");
        }

        private void BrowseDatabase(ref TextBox txt, string sTitle)
        {
            frmOpen.Title = sTitle;

            if (!String.IsNullOrWhiteSpace(txt.Text) && System.IO.File.Exists(txt.Text))
            {
                frmOpen.InitialDirectory = System.IO.Path.GetDirectoryName(txt.Text);
                frmOpen.FileName = System.IO.Path.GetFileName(txt.Text);
            }

            if (frmOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                frmOpen.InitialDirectory = System.IO.Path.GetDirectoryName(frmOpen.FileName);
                txt.Text = frmOpen.FileName;
            }
        }

        #endregion

        private void chkImportFish_CheckedChanged(object sender, EventArgs e)
        {
            lblSurveyDesign.Enabled = chkImportFish.Checked;
            txtSurveyDesign.Enabled = chkImportFish.Checked;
            cmdBrowseSurveyDesign.Enabled = chkImportFish.Checked;
        }

        private void frmImportCHaMPInfo_Load(object sender, EventArgs e)
        {
            chkImportFish_CheckedChanged(sender, e);
            checkBox1_CheckedChanged(sender, e);

            frmOpen.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb";
            frmOpen.RestoreDirectory = false;
            System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
            frmOpen.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lblProgramMetrics.Enabled = chkExtendedSiteInfo.Checked;
            txtProgramMetrics.Enabled = chkExtendedSiteInfo.Checked;
            cmdBrowseProgramMetrics.Enabled = chkExtendedSiteInfo.Checked;
        }

        private void UpdateExtendedSiteInfo(string sDatabaseExport)
        {

            String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sDatabaseExport);
            using (OleDbConnection conExport = new OleDbConnection(sDB))
            {
                conExport.Open();
                //
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // MetricAndCovariates fields
                //
                OleDbCommand dbUpdate = new OleDbCommand("UPDATE CHAMP_Sites S INNER JOIN CHAMP_Visits V ON S.SiteID = V.SiteID SET Latitude = @Latitude, Longitude = @Longitude, StreamName = @StreamName WHERE (V.VisitID = @VisitID)", m_dbCon);
                OleDbParameter pLatitude = dbUpdate.Parameters.Add("@Latitude", OleDbType.Single);
                OleDbParameter pLongitude = dbUpdate.Parameters.Add("@Longitude", OleDbType.Single);
                OleDbParameter pStreamName = dbUpdate.Parameters.Add("@StreamName", OleDbType.VarChar);
                OleDbParameter pVisitID = dbUpdate.Parameters.Add("@VisitID", OleDbType.Single);

                OleDbCommand comSurveyDesign = new OleDbCommand("SELECT VisitID, LAT_DD, LON_DD, Stream FROM MetricAndCovariates WHERE (VISITID IS NOT NULL) AND (LAT_DD IS NOT NULL) AND (LON_DD IS NOT NULL)", conExport);
                OleDbDataReader dbRead = comSurveyDesign.ExecuteReader();
                while (dbRead.Read())
                {
                    pLatitude.Value = (Double)dbRead["LAT_DD"];
                    pLongitude.Value = (Double)dbRead["LON_DD"];
                    pVisitID.Value = (int)dbRead["VisitID"];

                    if (dbRead.IsDBNull(dbRead.GetOrdinal("Stream")))
                        pStreamName.Value = DBNull.Value;
                    else
                    {
                        pStreamName.Value = dbRead.GetString(dbRead.GetOrdinal("Stream"));
                        //pStreamName.Size = dbRead.GetString(dbRead.GetOrdinal("Stream")).Length;
                    }

                    dbUpdate.ExecuteNonQuery();
                }
                //
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // MetricVisitInformation fields
                //
                dbUpdate = new OleDbCommand("UPDATE CHAMP_Visits SET Discharge = @Discharge, D84 = @D84 WHERE (VisitID = @VisitID)", m_dbCon);
                OleDbParameter pDischarge = dbUpdate.Parameters.Add("@Discharge", OleDbType.Single);
                OleDbParameter pD84 = dbUpdate.Parameters.Add("@D84", OleDbType.Single);
                pVisitID = dbUpdate.Parameters.Add("@VisitID", OleDbType.Integer);

                comSurveyDesign = new OleDbCommand("SELECT VisitID, SubD84, Q FROM MetricVisitInformation WHERE VisitID IS NOT NULL", conExport);
                dbRead = comSurveyDesign.ExecuteReader();
                while (dbRead.Read())
                {
                    pVisitID.Value = (int)dbRead["VisitID"];

                    if (dbRead.IsDBNull(dbRead.GetOrdinal("SubD84")))
                        pD84.Value = DBNull.Value;
                    else
                        pD84.Value = Convert.ToSingle(dbRead.GetDouble(dbRead.GetOrdinal("SubD84")));

                    if (dbRead.IsDBNull(dbRead.GetOrdinal("Q")))
                        pDischarge.Value = DBNull.Value;
                    else
                        pDischarge.Value = Convert.ToSingle(dbRead.GetDouble(dbRead.GetOrdinal("Q")));

                    dbUpdate.ExecuteNonQuery();
                }

            }
            LogCHaMPDataUpdate(m_dbCon.ConnectionString, "extended site information");
        }

        /// <summary>
        /// Populate the large wood column on the channel unit table.
        /// </summary>
        /// <param name="sdbCHaMP">Connection string to the CHaMP All Measurements database</param>
        /// <param name="sWorkbenchDB">Connection string to the Worbench database</param>
        private void UpdateLargeWoodCount(string sdbCHaMP, string sWorkbenchDB)
        {
            using (OleDbConnection conCHaMP = new OleDbConnection(sdbCHaMP))
            {
                conCHaMP.Open();

                using (OleDbConnection conWorkbench = new OleDbConnection(sWorkbenchDB))
                {
                    conWorkbench.Open();

                    OleDbCommand comUpdate = new OleDbCommand("UPDATE CHaMP_Segments AS S INNER JOIN CHAMP_ChannelUnits AS U ON S.SegmentID = U.SegmentID SET U.LargeWoodCount = @LargeWoodCount WHERE (S.VisitID = @VisitID) AND (U.ChannelUnitNumber = @ChannelUnitNumber)", conWorkbench);
                    OleDbParameter pLargeWoodCount = comUpdate.Parameters.Add("@LargeWoodCount", OleDbType.Integer);
                    OleDbParameter pVisitID = comUpdate.Parameters.Add("@VisitID", OleDbType.Integer);
                    OleDbParameter pChannelUnitNumber = comUpdate.Parameters.Add("@ChannelUnitNumber", OleDbType.Integer);

                    // 2011-2013 stored wood in the debris table. 2014 onward stores individual pieces. 
                    string[] sWoodSQLStatements = {
                                                      "SELECT VisitID, ChannelUnit_ChannelUnitNumber AS ChannelUnitNumber, Sum(SumLWDCount) AS LargeWoodCount FROM LargeWoodyDebris WHERE (VisitID IS NOT NULL) AND (ChannelUnit_ChannelUnitNumber IS NOT NULL) GROUP BY VisitID, ChannelUnit_ChannelUnitNumber",
                                                      "SELECT VisitID, ChannelUnit_ChannelUnitNumber AS ChannelUnitNumber, Count(ChannelUnit_ChannelUnitNumber) AS LargeWoodCount FROM LargeWoodPiece WHERE (VisitID IS NOT NULL) AND (ChannelUnit_ChannelUnitNumber IS NOT NULL) GROUP BY VisitID, ChannelUnit_ChannelUnitNumber"
                                                  };

                    foreach (string sSQL in sWoodSQLStatements)
                    {
                        OleDbCommand comSelect = new OleDbCommand(sSQL, conCHaMP);
                        OleDbDataReader dbRead = comSelect.ExecuteReader();
                        while (dbRead.Read())
                        {
                            pVisitID.Value = dbRead.GetInt32(dbRead.GetOrdinal("VisitID"));
                            pChannelUnitNumber.Value = dbRead.GetInt32(dbRead.GetOrdinal("ChannelUnitNumber"));

                            if (dbRead.GetFieldType(dbRead.GetOrdinal("LargeWoodCount")) == System.Type.GetType("System.Double"))
                                pLargeWoodCount.Value = (int)dbRead.GetDouble(dbRead.GetOrdinal("LargeWoodCount"));
                            else if (dbRead.GetFieldType(dbRead.GetOrdinal("LargeWoodCount")) == System.Type.GetType("System.Int32"))
                                pLargeWoodCount.Value = dbRead.GetInt32(dbRead.GetOrdinal("LargeWoodCount"));
                            else
                                throw new Exception("Unhandled LargeWoodCount field type");
                            comUpdate.ExecuteNonQuery();
                        }
                        dbRead.Close();
                    }
                }
            }
        }
    }
}

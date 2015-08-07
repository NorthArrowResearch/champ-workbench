﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CHaMPWorkbench.RBTInputFile;
using System.Deployment.Application;
using System.Data.OleDb;
using System.IO;

namespace CHaMPWorkbench
{
    public partial class MainForm : Form
    {

        private System.Data.OleDb.OleDbConnection m_dbCon;

        public MainForm()
        {
            InitializeComponent();

            string sPath = GetDatabasePathFromConnectionString(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
            if (!String.IsNullOrWhiteSpace(sPath))
            {
                if (System.IO.File.Exists(sPath))
                {
                    m_dbCon = new System.Data.OleDb.OleDbConnection(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
                    m_dbCon.Open();
                }
            }
        }

        private string GetDatabasePathFromConnectionString(string sConnectionString)
        {
            string sPath = "";
            if (!String.IsNullOrWhiteSpace(sConnectionString))
            {
                string sKey = "Source=";
                int nStart = sConnectionString.IndexOf(sKey);
                if (nStart > 0)
                {
                    int nEnd = sConnectionString.IndexOf(";", nStart);
                    if (nEnd > nStart)
                    {
                        sPath = sConnectionString.Substring(nStart + sKey.Length, nEnd - nStart - sKey.Length);
                        if (!System.IO.File.Exists(sPath))
                            sPath = string.Empty;
                    }
                }

            }
            return sPath;
        }

        private void UpdateMenuItemStatus(ToolStripItemCollection aMenu)
        {
            foreach (ToolStripItem subMenu in aMenu)
            {
                if (subMenu is ToolStripMenuItem)
                //if we get the desired object type.
                {
                    if (((ToolStripMenuItem)subMenu).HasDropDownItems) // if subMenu has children
                    {
                        if (subMenu.Name != "aboutToolStripMenuItem")
                            UpdateMenuItemStatus(((ToolStripMenuItem)subMenu).DropDownItems); // Call recursive Method.
                    }
                    else // Do the desired operations here.
                    {
                        switch (subMenu.Name)
                        {
                            case "optionsToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "openDatabaseToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "exitToolStripMenuItem":
                                break; // do nothing. Always enabled.

                            case "closeDatabaseToolStripMenuItem":
                                subMenu.Enabled = m_dbCon != null;
                                break;

                            default:
                                subMenu.Enabled = m_dbCon is System.Data.OleDb.OleDbConnection;
                                break;
                        }
                    }
                }

                // Now update the tool status strip
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                    tssDatabasePath.Text = oCon.DataSource;
                }
                else
                    tssDatabasePath.Text = string.Empty;
            }
        }

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select " + CHaMPWorkbench.Properties.Resources.MyApplicationNameLong + " Database";
            dlg.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb|All Files (*.*)|*.*";

            if (m_dbCon is System.Data.OleDb.OleDbConnection)
            {
                System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
                dlg.FileName = System.IO.Path.GetFileName(oCon.DataSource);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.DBConnection))
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
                    dlg.FileName = System.IO.Path.GetFileName(oCon.DataSource);
                }
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                    if (string.Compare(dlg.FileName, oCon.DataSource, true) == 0)
                        return;
                    else
                        m_dbCon.Close();
                }

                String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + dlg.FileName);

                try
                {
                    Console.WriteLine("Attempting to open database: " + sDB);
                    m_dbCon = new System.Data.OleDb.OleDbConnection(sDB);
                    m_dbCon.Open();
                    CHaMPWorkbench.Properties.Settings.Default.DBConnection = sDB;
                    CHaMPWorkbench.Properties.Settings.Default.Save();
                    UpdateMenuItemStatus(menuStrip1.Items);

                    LoadVisits();
                }
                catch (Exception ex)
                {
                    m_dbCon = null;
                    MessageBox.Show("Error opening database: " + sDB, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dbCon = null;
            GC.Collect();
            UpdateMenuItemStatus(menuStrip1.Items);
        }

        private void individualFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon != null)
            {
                frmRBTInputSingle frm = new frmRBTInputSingle(m_dbCon);
            }
        }

        private void scavengeVisitInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmScavengeVisitTopoInfo2 frm = new Data.frmScavengeVisitTopoInfo2(m_dbCon);
            frm.ShowDialog();
        }

        private void selectBatchesToRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelectRBTBatches frm = new frmSelectRBTBatches(m_dbCon);
            frm.ShowDialog();
        }

        private void runRBTConsoleBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RBT.frmRunBatches frm = new RBT.frmRunBatches(m_dbCon);
            frm.ShowDialog();
        }

        private void scavengeRBTResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.frmRBTScavenger rbt = new frmRBTScavenger(m_dbCon);
            rbt.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptions frm = new frmOptions();
            frm.ShowDialog();
        }

        private void batchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<int> lvisitIDs = new List<int>();
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                lvisitIDs.Add((int) r["VisitID"]);
            }

            if (lvisitIDs.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon, lvisitIDs);

                frm.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("You must have at least one visit in the main grid view to create an RBT batch.");
        }

        private void scavengeVisitTopoDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmScavengeVisitTopoInfo2 frm = new Data.frmScavengeVisitTopoInfo2(m_dbCon);
            frm.ShowDialog();
        }

        private void unpackMonitoringData7ZipArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmDataUnPacker frm = new Data.frmDataUnPacker();
            frm.ShowDialog();
        }

        private void aboutTheCHaMPWorkbenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout(m_dbCon);
            frm.ShowDialog();
        }

        private void cHaMPWorkbenchWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CHaMPWorkbench.Properties.Resources.WebSiteURL);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dbCon is System.Data.OleDb.OleDbConnection)
                    if (m_dbCon.State == ConnectionState.Open)
                        m_dbCon.Close();
            }
            catch (Exception ex)
            {
                // Do nothing. Let the application quitting try to release DB connection & resoures.
            }

            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateMenuItemStatus(menuStrip1.Items);

            LoadVisits();

            this.lstFieldSeason.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstSite.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstWatershed.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
        }

        private void scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmImportCHaMPInfo frm = new Data.frmImportCHaMPInfo(m_dbCon);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadVisits();
        }

        private void prepareDatabaseForDeploymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmClearDatabase frm = new Data.frmClearDatabase(m_dbCon);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadVisits();
        }

        private void aboutExperimentalToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This menu contains tools that are still under development, or only intended for a select number of people. Developers should" +
                " place experimental tools under their own name until they are robust and tested.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void hydroModelInputGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Kelly.frmHydroModelInputs frm = new Experimental.Kelly.frmHydroModelInputs(m_dbCon);
            frm.ShowDialog();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;
            Cursor.Current = Cursors.WaitCursor;

            if ((ApplicationDeployment.IsNetworkDeployed))
            {
                ApplicationDeployment AD = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = AD.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time.\n\nPlease check your network connection, or try again later. Error: " + dde.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong);
                    return;
                }

                if ((info.UpdateAvailable))
                {
                    bool doUpdate = true;

                    if ((!info.IsUpdateRequired))
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if ((!(System.Windows.Forms.DialogResult.OK == dr)))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " + "version to version " + info.MinimumRequiredVersion.ToString() + ". The application will now install the update and restart.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if ((doUpdate))
                    {
                        try
                        {
                            AD.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application.\n\nPlease check your network connection, or try again later.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("The application is not deployed over the internet and therefore cannot be updated automatically.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }

        private void openDatabaseInAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dbCon is System.Data.OleDb.OleDbConnection)
            {
                string sPath = GetDatabasePathFromConnectionString(m_dbCon.ConnectionString);
                if (!string.IsNullOrWhiteSpace(sPath))
                    System.Diagnostics.Process.Start(sPath);
            }
        }

        private void testXPathReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Philip.frmTestXPath frm = new Experimental.Philip.frmTestXPath(m_dbCon);
            frm.ShowDialog();
        }

        private void queueBridgeCreekBatchesRBTRunsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Philip.frmBridgeBatchRuns frm = new Experimental.Philip.frmBridgeBatchRuns(m_dbCon);
            frm.ShowDialog();
        }

        private void extractRBTErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Kelly.frmExtractRBTErrors frm = new Experimental.Kelly.frmExtractRBTErrors(m_dbCon);
            frm.ShowDialog();
        }

        private void generateBatchHabitatProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Habitat.frmHabitatBatch frm = new Habitat.frmHabitatBatch(m_dbCon);
            frm.ShowDialog();
        }

        private void LoadVisits()
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sSQL = "SELECT W.WatershedID, W.WatershedName," +
                " S.SiteID, S.SiteName," +
                " V.VisitID, V.VisitYear, V.HitchName, V.CrewName, V.SampleDate, V.IsPrimary, V.PanelName," +
                " Count(C.SegmentID) AS ChannelUnits" +
                " FROM ((CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID) LEFT JOIN CHaMP_Segments AS Seg ON V.VisitID = Seg.VisitID) LEFT JOIN CHAMP_ChannelUnits AS C ON Seg.SegmentID = C.SegmentID" +
                " GROUP BY W.WatershedID, W.WatershedName, S.SiteID, S.SiteName, V.VisitID, V.VisitYear, V.HitchName, V.CrewName, V.SampleDate, V.IsPrimary, V.PanelName" +
                " ORDER BY W.WatershedName, S.SiteName, V.VisitID";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataAdapter daVisits = new OleDbDataAdapter(dbCom);
            DataTable dtVisits = new DataTable();
            daVisits.Fill(dtVisits);
            grdVisits.DataSource = dtVisits.AsDataView();

            // Load the field seasons
            OleDbCommand comFS = new OleDbCommand("SELECT VisitYear FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear", m_dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();
            while (dbRead.Read())
            {
                int nSel = lstFieldSeason.Items.Add(new ListItem(((Int16)dbRead[0]).ToString(), (Int16)dbRead[0]));
                lstFieldSeason.SetItemChecked(nSel, true);
            }
            dbRead.Close();

            // Load the watersheds
            comFS = new OleDbCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) GROUP BY WatershedID, WatershedName ORDER BY WatershedName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            while (dbRead.Read())
            {
                int nSel = lstWatershed.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
                lstWatershed.SetItemChecked(nSel, true);
            }
            dbRead.Close();

            // Load the Sites
            comFS = new OleDbCommand("SELECT SiteID, SiteName FROM CHAMP_Sites WHERE (SiteName Is Not Null) ORDER BY SiteName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            while (dbRead.Read())
            {
                int nSel = lstSite.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
                lstSite.SetItemChecked(nSel, true);
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void FilterVisits(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sFilter = "";

            if (chkVisitID.Checked)
            {
                if (!string.IsNullOrWhiteSpace(sFilter))
                    sFilter += " AND ";
                sFilter += string.Format(" (VisitID = {0})", (int)valVisitID.Value);
            }
            else
            {
                AddCheckedListboxFilter(ref lstFieldSeason, ref sFilter, "VisitYear");
                AddCheckedListboxFilter(ref lstWatershed, ref sFilter, "WatershedID");
                AddCheckedListboxFilter(ref lstSite, ref sFilter, "SiteID");
            }

            if (grdVisits.DataSource is DataView)
            {
                DataView dv = (DataView)grdVisits.DataSource;
                System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                dv.RowFilter = sFilter;
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

        }

        private void AddCheckedListboxFilter(ref CheckedListBox lst, ref string sFilter, string sPropertyName, bool bUseNameInsteadOfValue = false)
        {
            // Only add filter if not all the items are checked and there is some filtering to do
            if (lst.CheckedItems.Count == lst.Items.Count)
                return;

            string sValueList = "";
            foreach (ListItem l in lst.CheckedItems)
            {
                if (bUseNameInsteadOfValue)
                    sValueList += "'" + l.ToString() + "', ";
                else
                    sValueList += l.Value.ToString() + ", ";
            }

            if (!string.IsNullOrWhiteSpace(sValueList))
            {
                if (!string.IsNullOrWhiteSpace(sFilter))
                    sFilter += " AND ";

                sFilter += String.Format(" {0} IN ({1})", sPropertyName, sValueList.Substring(0, sValueList.Length - 2));
            }
        }

        private void valVisitID_ValueChanged(object sender, EventArgs e)
        {
            chkVisitID.Checked = true;
            FilterVisits(sender, e);
        }

        private void visitPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void grdVisits_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
            {
                grdVisits.Rows[e.RowIndex].Selected = true;
                cmsVisit.Show(Cursor.Position);
            }
        }

        private void browseMonitoringDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExploreVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
        }

        private void browseModelInputOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExploreVisitFolder(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder);
        }

        private void ExploreVisitFolder(string sParentFolder)
        {
            string sPath = RetrieveVisitFolder(sParentFolder);
            if (!string.IsNullOrEmpty(sPath))
            {
                if (System.IO.Directory.Exists(sPath))
                    System.Diagnostics.Process.Start(sPath);
                else
                {
                    if (System.Windows.Forms.MessageBox.Show(string.Format("The specified folder does not exist. Do you want to create It? {0}", sPath),
                        CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                        System.Diagnostics.Process.Start(sPath);
                    }
                }
            }
        }

        private void CopyClipboardVisitFolder(string sParentFolder)
        {
            string sPath = RetrieveVisitFolder(sParentFolder);
            if (!string.IsNullOrEmpty(sPath))
            {
                if (System.IO.Directory.Exists(sPath))
                    Clipboard.SetText(sPath);
                else
                {
                    if (System.Windows.Forms.MessageBox.Show(string.Format("The specified folder does not exist. Do you want to create It? {0}", sPath),
                        CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                        Clipboard.SetText(sPath);
                    }
                }
            }
        }

        private string RetrieveVisitFolder(string sParentFolder)
        {
            string sPath = string.Empty;
            DataRow dr = RetrieveVisitInfo();
            if (dr is DataRow)
            {
                sPath = System.IO.Path.Combine(sParentFolder, ((Int16)dr["VisitYear"]).ToString());
                sPath = System.IO.Path.Combine(sPath, (string)dr["WatershedName"]);
                sPath = System.IO.Path.Combine(sPath, (string)dr["SiteName"]);
                sPath = System.IO.Path.Combine(sPath, string.Format("VISIT_{0}", dr["VisitID"]));
                sPath = sPath.Replace(" ", "");
            }
            return sPath;
        }

        private DataRow RetrieveVisitInfo()
        {
            DataRow r = null;
            if (grdVisits.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)grdVisits.SelectedRows[0].DataBoundItem;
                r = drv.Row;
            }
            return r;
        }

        private void copyMonitoringDataFolderPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyClipboardVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
        }

        private void copyModelInputOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyClipboardVisitFolder(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder);
        }

        private void downloadTopoAndHydroDataFromCmorgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow r = RetrieveVisitInfo();
            if (r is DataRow)
            {
                string sTopoFolder = RetrieveVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
                string sFTPFolder = "ftp://" + RetrieveVisitFolder("ftp.geooptix.com/ByYear").Replace("\\", "/");
                Data.frmFTPVisit frm = new Data.frmFTPVisit((int)r["VisitID"], sFTPFolder, sTopoFolder);
                frm.ShowDialog();
            }
        }

        private void filterForAllVisitsToThisSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkVisitID.CheckedChanged -= FilterVisits;
            chkVisitID.Checked = false;

            DataRow r = RetrieveVisitInfo();
            if (r is DataRow)
            {
                // turn off event handling
                valVisitID.ValueChanged -= valVisitID_ValueChanged;
                lstSite.ItemCheck -= FilterListBoxCheckChanged;
                lstWatershed.ItemCheck -= FilterListBoxCheckChanged;

                int nSiteID = (int)r["SiteID"];
                int nWatershedID = (int)r["WatershedID"];

                for (int i = 0; i < lstFieldSeason.Items.Count; i++)
                    lstFieldSeason.SetItemChecked(i, true);

                for (int i = 0; i < lstWatershed.Items.Count; i++)
                {
                    lstWatershed.SetItemChecked(i, ((ListItem)lstWatershed.Items[i]).Value == nWatershedID);
                    if (((ListItem)lstWatershed.Items[i]).Value == nWatershedID)
                        lstWatershed.TopIndex = i;
                }

                for (int i = 0; i < lstSite.Items.Count; i++)
                {
                    lstSite.SetItemChecked(i, ((ListItem)lstSite.Items[i]).Value == nSiteID);
                    if (((ListItem)lstSite.Items[i]).Value == nSiteID)
                        lstSite.TopIndex = i;

                }

                // turn on event handling
                lstSite.ItemCheck += FilterListBoxCheckChanged;
                valVisitID.ValueChanged += valVisitID_ValueChanged;
                lstWatershed.ItemCheck += FilterListBoxCheckChanged;
            }

            FilterVisits(sender, e);
            chkVisitID.CheckedChanged += FilterVisits;

        }

        private void AllNoneSitesClick(object sender, EventArgs e)
        {
            // turn off event handling
            lstSite.ItemCheck -= FilterListBoxCheckChanged;

            for (int i = 0; i < lstSite.Items.Count; i++)
                lstSite.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));

            // Turn on event handling
            lstSite.ItemCheck += FilterListBoxCheckChanged;

            FilterVisits(sender, e);
        }

        private void AllNoneWatershedsClick(object sender, EventArgs e)
        {
            // turn off event handling
            lstWatershed.ItemCheck -= FilterListBoxCheckChanged;

            for (int i = 0; i < lstWatershed.Items.Count; i++)
                lstWatershed.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));

            // Turn on event handling
            lstWatershed.ItemCheck += FilterListBoxCheckChanged;

            FilterVisits(sender, e);
        }

        private void FilterListBoxCheckChanged(object sender, ItemCheckEventArgs e)
        {
            ((CheckedListBox)sender).ItemCheck -= FilterListBoxCheckChanged;
            ((CheckedListBox)sender).SetItemChecked(e.Index, e.NewValue == CheckState.Checked);
            ((CheckedListBox)sender).ItemCheck += FilterListBoxCheckChanged;

            FilterVisits(sender, e);

        }

        private void generateChannelUnitCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow r = RetrieveVisitInfo();
            if (r is DataRow)
            {
                string sTopoFolder = RetrieveVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
                sTopoFolder = System.IO.Path.Combine(sTopoFolder, "Topo");
                System.IO.Directory.CreateDirectory(sTopoFolder);
                sTopoFolder = System.IO.Path.Combine(sTopoFolder, "ChannelUnits.csv");
                Classes.ChannelUnitCSVGenerator csv = new Classes.ChannelUnitCSVGenerator(ref m_dbCon);
                csv.Run((int)r["VisitID"], sTopoFolder);

                if (System.Windows.Forms.MessageBox.Show("The channel unit file was created successfully. Do you want to view the file?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    System.Diagnostics.Process.Start(sTopoFolder);
            }
        }

        private void generateRBTRunForThisVisitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> lvisitIDs = new List<int>();
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                lvisitIDs.Add((int)r["VisitID"]);
            }

            if (lvisitIDs.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon, lvisitIDs);

                frm.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("You must have at least one visit selected in the main table of visits to create an RBT batch.");
        }

        private void viewSiteLocationMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                OleDbCommand dbCom = new OleDbCommand("SELECT Latitude, Longitude FROM CHAMP_Sites S INNER JOIN CHAMP_Visits V ON S.SiteID = V.SiteID WHERE (V.VisitID = @VisitID)", m_dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", (int)r["VisitID"]);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                if (dbRead.Read() && (dbRead["Latitude"] != DBNull.Value) && (dbRead["Longitude"] != DBNull.Value))
                {
                    string sURL = string.Format("http://maps.google.com/?q={0},{1}&t=h&z={2}", (double)dbRead["Latitude"], (double)dbRead["Longitude"], CHaMPWorkbench.Properties.Settings.Default.GoogleMapZoom);
                    System.Diagnostics.Process.Start(sURL);
                }
                else
                    System.Windows.Forms.MessageBox.Show("The visit information does not possess latitude and longitude coordinates. Use the import visit information tool to refres the workbench with coordinates from CHaMP data exports.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
    }
}

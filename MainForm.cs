using System;
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

            // Check the status of the AWS logging and create the singleton AWS logger if active
            if (CHaMPWorkbench.Properties.Settings.Default.AWSLoggingEnabled)
            {
                try
                {
                    Classes.AWSCloudWatch.AWSCloudWatchSingleton theWather = new Classes.AWSCloudWatch.AWSCloudWatchSingleton();
                }
                catch
                {
                    // Failed to instantiate the cloud watcher. Do nothing.
                }
            }

            string sPath = GetDatabasePathFromConnectionString(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
            OpenDatabase(sPath);
        }

        private void CheckDBVersion()
        {
            OleDbCommand dbCom = new OleDbCommand("SELECT ValueInfo FROM VersionInfo WHERE Key = 'DatabaseVersion'", m_dbCon);
            String sVersion = (string)dbCom.ExecuteScalar();
            if (String.IsNullOrWhiteSpace(sVersion))
                throw new Exception("Error retrieving database version");

            int nVersion;
            if (Int32.TryParse(sVersion, out nVersion) && Int32.Parse(sVersion) < CHaMPWorkbench.Properties.Settings.Default.MinimumDBVersion)
            {
                DialogResult dialogResult = MessageBox.Show(String.Format("The database you are trying to load has a version of \"{0}\" however the minimum version required by workbench is \"{1}\". If you continue you may experience problems. \n\n Do you want to continue loading the database anyway?", sVersion, CHaMPWorkbench.Properties.Settings.Default.MinimumDBVersion.ToString()), "Version Error", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    return;
                }
                else if (dialogResult == DialogResult.No)
                {
                    m_dbCon = null;
                    return;
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

                            case "createNewWorkbenchDatabaseToolStripMenuItem":
                                break; // do nothing. Always enabled

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

            // Kelly's Hydro prep and RBT error tool are deprecated. Hide it in release build
#if DEBUG

#else

            extractRBTErrorsToolStripMenuItem.Visible = false;
#endif

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

                OpenDatabase(dlg.FileName);
            }
        }

        private void OpenDatabase(string sDatabasePath)
        {
            if (!string.IsNullOrEmpty(sDatabasePath) && System.IO.File.Exists(sDatabasePath))
            {
                String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sDatabasePath);

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
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dbCon = null;
            GC.Collect();

            try
            {
                lstFieldSeason.Items.Clear();
                lstSite.Items.Clear();
                lstWatershed.Items.Clear();

                DataView dv = (System.Data.DataView)grdVisits.DataSource;
                dv.Table.Clear();

                UpdateMenuItemStatus(menuStrip1.Items);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void selectBatchesToRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmSelectBatches frm = new frmSelectBatches(m_dbCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_RBT);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runRBTConsoleBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RBT.frmRunBatches frm = new RBT.frmRunBatches(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void scavengeRBTResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CHaMPWorkbench.frmRBTScavenger rbt = new frmRBTScavenger(m_dbCon);
                rbt.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmOptions frm = new frmOptions();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void unpackMonitoringData7ZipArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmDataUnPacker frm = new Data.frmDataUnPacker();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void aboutTheCHaMPWorkbenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmAbout frm = new frmAbout(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
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
            try
            {
                UpdateMenuItemStatus(menuStrip1.Items);

                LoadVisits();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            this.lstFieldSeason.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstSite.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstWatershed.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
        }

        private void scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmImportCHaMPInfo frm = new Data.frmImportCHaMPInfo(m_dbCon);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LoadVisits();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void prepareDatabaseForDeploymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmClearDatabase frm = new Data.frmClearDatabase(m_dbCon);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LoadVisits();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void aboutExperimentalToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This menu contains tools that are still under development, or only intended for a select number of people. Developers should" +
                " place experimental tools under their own name until they are robust and tested.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // NOTE: This is possibly depprecated.
        //private void hydroModelInputGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Experimental.Kelly.frmHydroModelInputs frm = new Experimental.Kelly.frmHydroModelInputs(m_dbCon);
        //        frm.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        Classes.ExceptionHandling.NARException.HandleException(ex);
        //    }
        //}

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
            try
            {
                Experimental.Philip.frmTestXPath frm = new Experimental.Philip.frmTestXPath(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void queueBridgeCreekBatchesRBTRunsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Philip.frmBridgeBatchRuns frm = new Experimental.Philip.frmBridgeBatchRuns(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void extractRBTErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Kelly.frmExtractRBTErrors frm = new Experimental.Kelly.frmExtractRBTErrors(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void generateBatchHabitatProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Habitat.frmHabitatBatch frm = new Habitat.frmHabitatBatch(m_dbCon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void LoadVisits()
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sGroupFields = " W.WatershedID, W.WatershedName, V.VisitID, V.VisitYear, V.SampleDate,V.HitchName,V.CrewName,V.PanelName, S.SiteID, S.SiteName, V.Organization, V.QCVisit, V.CategoryName, V.VisitPhase,V.VisitStatus,V.AEM,V.HasStreamTempLogger,V.HasFishData";

            string sSQL = "SELECT " + sGroupFields + ", Count(C.SegmentID) AS ChannelUnits" +
                " FROM ((CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID) LEFT JOIN CHaMP_Segments AS Seg ON V.VisitID = Seg.VisitID) LEFT JOIN CHAMP_ChannelUnits AS C ON Seg.SegmentID = C.SegmentID" +
                " GROUP BY " + sGroupFields +
                " ORDER BY W.WatershedName, S.SiteName, V.VisitID";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataAdapter daVisits = new OleDbDataAdapter(dbCom);
            DataTable dtVisits = new DataTable();

            try
            {
                daVisits.Fill(dtVisits);
                grdVisits.DataSource = dtVisits.AsDataView();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error loading visits", ex);
                ex2.Data["SQL Select Command"] = dbCom.CommandText;
                throw ex2;
            }
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
            try
            {
                chkVisitID.Checked = true;
                FilterVisits(sender, e);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
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
            try
            {
                ExploreVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void browseModelInputOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExploreVisitFolder(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
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
            try
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
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void filterForAllVisitsToThisSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkVisitID.CheckedChanged -= FilterVisits;
            chkVisitID.Checked = false;

            try
            {
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
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void AllNoneSitesClick(object sender, EventArgs e)
        {
            // turn off event handling
            lstSite.ItemCheck -= FilterListBoxCheckChanged;

            try
            {
                for (int i = 0; i < lstSite.Items.Count; i++)
                    lstSite.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));

                // Turn on event handling
                lstSite.ItemCheck += FilterListBoxCheckChanged;

                FilterVisits(sender, e);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void AllNoneWatershedsClick(object sender, EventArgs e)
        {
            // turn off event handling
            lstWatershed.ItemCheck -= FilterListBoxCheckChanged;

            try
            {
                for (int i = 0; i < lstWatershed.Items.Count; i++)
                    lstWatershed.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));

                // Turn on event handling
                lstWatershed.ItemCheck += FilterListBoxCheckChanged;

                FilterVisits(sender, e);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void FilterListBoxCheckChanged(object sender, ItemCheckEventArgs e)
        {
            ((CheckedListBox)sender).ItemCheck -= FilterListBoxCheckChanged;
            ((CheckedListBox)sender).SetItemChecked(e.Index, e.NewValue == CheckState.Checked);
            ((CheckedListBox)sender).ItemCheck += FilterListBoxCheckChanged;

            try
            {
                FilterVisits(sender, e);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

        }

        private void generateChannelUnitCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow r = RetrieveVisitInfo();
                if (r is DataRow)
                {
                    string sTopoFolder = RetrieveVisitFolder(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
                    sTopoFolder = System.IO.Path.Combine(sTopoFolder, "Topo");
                    System.IO.Directory.CreateDirectory(sTopoFolder);
                    sTopoFolder = System.IO.Path.Combine(sTopoFolder, "ChannelUnits.csv");
                    Classes.CSVGenerators.ChannelUnitCSVGenerator csv = new Classes.CSVGenerators.ChannelUnitCSVGenerator(m_dbCon.ConnectionString);
                    csv.Run((int)r["VisitID"], sTopoFolder);

                    if (System.Windows.Forms.MessageBox.Show("The channel unit file was created successfully. Do you want to view the file?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        System.Diagnostics.Process.Start(sTopoFolder);
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void generateRBTRunForThisVisitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon, dVisits);
                try
                {
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
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

                try
                {
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
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Creates a dictionary of visits that are selected in the main grid.
        /// </summary>
        /// <remarks>Use this method for any tool that should take a list of visits.
        /// e.g. input file batch builders</remarks>
        /// <returns>Dictionary, Key is Visit ID and value is a string name for the visit:
        /// YYYY, WatershedName, Site, Visit XXXX</returns>
        private Dictionary<int, string> GetSelectedVisits()
        {
            Dictionary<int, string> dVisits = new Dictionary<int, string>();
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;
                string sLabel = string.Format("{0}, {1}, {2}, Visit {3}", r["WatershedName"], r["VisitYear"], r["SiteName"], r["VisitID"]);
                dVisits.Add((int)r["VisitID"], sLabel);
            }

            return dVisits;
        }

        private void buildInputFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(m_dbCon, dVisits);

                try
                {
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("You must have at least one visit in the main grid view to create an RBT batch.");
        }

        private void writeSimulationResultsToCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Habitat.frmScavengeHabitatResults frm = new Habitat.frmScavengeHabitatResults();
            frm.ShowDialog();
        }

        private void exportAWSLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save CHaMP Data For AWS";
            frm.Filter = "JSON Files (*.json)|*.json";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Classes.AWSExporter aws = new Classes.AWSExporter();
                    System.IO.FileInfo fiExport = new System.IO.FileInfo(frm.FileName);
                    int nExported = aws.Run(ref m_dbCon, fiExport);

                    if (MessageBox.Show(string.Format("{0:#,##0} records exported to file. Do you want to browse to the file created?", nExported), "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(fiExport.Directory.FullName);
                    }
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void filterVisitsFromVisitIDCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (grdVisits.DataSource is DataView)
                {
                    OpenFileDialog frm = new OpenFileDialog();
                    frm.Title = "Visit ID Comma Separated Value (CSV) file";
                    frm.Filter = "Comma Separated Value Files (*.csv)|*.csv";
                    frm.CheckFileExists = true;

                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        List<string> lValidVisitIDs = new List<string>();
                        string[] sVisitIDs = File.ReadAllText(frm.FileName).Split(',');
                        foreach (string sVisitID in sVisitIDs)
                        {
                            int nVisitID = 0;
                            if (!string.IsNullOrEmpty(sVisitID))
                            {
                                if (int.TryParse(sVisitID, out nVisitID))
                                {
                                    if (nVisitID > 0)
                                    {
                                        lValidVisitIDs.Add(nVisitID.ToString());
                                    }
                                }
                            }
                        }

                        if (lValidVisitIDs.Count < 1)
                        {
                            MessageBox.Show("No valid visit IDs found in file.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Clear the user interface of filtering items.
                        chkVisitID.CheckedChanged -= FilterVisits;
                        chkVisitID.Checked = false;
                        chkVisitID.CheckedChanged += new EventHandler(FilterVisits);

                        ClearCheckedItems(ref lstWatershed);
                        ClearCheckedItems(ref lstSite);
                        ClearCheckedItems(ref lstFieldSeason);

                        string sFilter = string.Format("VisitID IN ({0})", string.Join(",", lValidVisitIDs));
                        DataView dv = (DataView)grdVisits.DataSource;
                        System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                        dv.RowFilter = sFilter;
                    }
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        /// <summary>
        /// Clears the checkbox on all items in the three visit filtering checked listboxs
        /// </summary>
        /// <param name="lst"></param>
        /// <remarks>Note that the three listboxes have events on the itemcheck. These events
        /// need to be turned off and then turned back on after the work is done (to prevent
        /// the events firing every time that an item check is changed.</remarks>
        private void ClearCheckedItems(ref CheckedListBox lst)
        {
            lst.ItemCheck -= FilterListBoxCheckChanged;
            for (int i = 0; i < lst.Items.Count; i++)
                lst.SetItemChecked(i, false);

            lst.ItemCheck += new ItemCheckEventHandler(FilterListBoxCheckChanged);
        }

        private void validationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveFileDialog frm = new SaveFileDialog();
            //frm.Title = "RBT Validation Report Output Path";
            //frm.Filter = "HTML Files (*.html, *.htm)|*.htm|XML Files (*.xml)|*.xml";

            //if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    try
            //    {
            //        Cursor.Current = Cursors.WaitCursor;
            //        Classes.ValidationReport report = new Classes.ValidationReport(m_dbCon.ConnectionString, new System.IO.FileInfo(frm.FileName));
            //        Classes.ValidationReport.ValidationReportResults theResults = report.Run();

            //        if (theResults.Visits < 1)
            //            MessageBox.Show("The database does not contain any visits with manual metric values against which to validate RBT runs.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        else
            //        {
            //            if (System.IO.File.Exists(frm.FileName))
            //            {
            //                System.Diagnostics.Process.Start(frm.FileName);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Classes.ExceptionHandling.NARException.HandleException(ex);
            //    }
            //    finally
            //    {
            //        Cursor.Current = Cursors.Default;
            //    }
            //}
        }

        private void selectRandomNumberOfVisitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelectVisits frm = new frmSelectVisits();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (frm.VisitsToSelect >= grdVisits.Rows.Count)
                {
                    grdVisits.SelectAll();
                }
                else
                {
                    grdVisits.ClearSelection();
                    Random random = new Random();

                    while (grdVisits.SelectedRows.Count < frm.VisitsToSelect)
                    {
                        int randomNumber = random.Next(0, grdVisits.Rows.Count);
                        grdVisits.Rows[randomNumber].Selected = true;
                    }
                }
            }
        }

        private void modelValidationReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListItem> lVisits = new List<ListItem>();

            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                lVisits.Add(new ListItem(string.Format("{0} - {1} - Visit ID {2}", (string)r["WatershedName"], (string)r["SiteName"], (int)r["VisitID"]), (int)r["VisitID"]));
            }

            Validation.frmModelValidation frm = new Validation.frmModelValidation(m_dbCon.ConnectionString, ref lVisits);
            frm.ShowDialog();
        }

        private void selectBatchesToRunToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmSelectBatches frm = new frmSelectBatches(m_dbCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_GUT);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runSelectedBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUT.frmGUTRun frm = new GUT.frmGUTRun(m_dbCon.ConnectionString);
            frm.ShowDialog();
        }

        private void recordPostGCDQAQCRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.James.frmEnterPostGCD_QAQC_Record frm = new Experimental.James.frmEnterPostGCD_QAQC_Record(m_dbCon);
            frm.ShowDialog();

        }

        private void gCDAnalysisWatershedLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.James.frmGCD_MetricsViewer frm = new Experimental.James.frmGCD_MetricsViewer(m_dbCon.ConnectionString);
            frm.ShowDialog();
        }

        private void exploreSiteLevelUSGSStreamGageDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (grdVisits.SelectedRows.Count > 1)
            {
                MessageBox.Show("USGS stream gage data can only be explored for one site at a time. Please select only one record from the table.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (grdVisits.SelectedRows.Count == 1)
            {

                DataRowView drv = (DataRowView)grdVisits.SelectedRows[0].DataBoundItem;
                //DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;

                int nWatershedID = (int)r["WatershedID"];
                int nSiteID = (int)r["SiteID"];

                Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(m_dbCon.ConnectionString, nSiteID, nWatershedID);
                frm.ShowDialog();
            }
        }

        private void buildInputFilesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                HydroPrep.frmHydroPrepBatchBuilder frm = new HydroPrep.frmHydroPrepBatchBuilder(m_dbCon.ConnectionString, dVisits);

                try
                {
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("You must have at least one visit in the main grid view to create an RBT batch.");
        }

        private void selectBatchesToRunToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                frmSelectBatches frm = new frmSelectBatches(m_dbCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_HydroPrep);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runSelectedBatchesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HydroPrep.frmHydroPrepRun frm = new HydroPrep.frmHydroPrepRun(m_dbCon.ConnectionString);
            frm.ShowDialog();
        }

        private void runHabitatBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Habitat.frmHabitatRun frm = new Habitat.frmHabitatRun(m_dbCon.ConnectionString);
            frm.ShowDialog();
        }

        private void scavengeHabitatResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    CHaMPWorkbench.frmRBTScavenger rbt = new frmRBTScavenger(m_dbCon);
            //    rbt.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    Classes.ExceptionHandling.NARException.HandleException(ex);
            //}
        }

        private void filterVisitsFromCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (grdVisits.DataSource is DataView)
                {
                    OpenFileDialog frm = new OpenFileDialog();
                    frm.Title = "Visit ID Comma Separated Value (CSV) file";
                    frm.Filter = "Comma Separated Value Files (*.csv)|*.csv";
                    frm.CheckFileExists = true;

                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        List<string> lValidVisitIDs = new List<string>();
                        string[] sVisitIDs = File.ReadAllText(frm.FileName).Split(',');
                        foreach (string sVisitID in sVisitIDs)
                        {
                            int nVisitID = 0;
                            if (!string.IsNullOrEmpty(sVisitID))
                            {
                                if (int.TryParse(sVisitID, out nVisitID))
                                {
                                    if (nVisitID > 0)
                                    {
                                        lValidVisitIDs.Add(nVisitID.ToString());
                                    }
                                }
                            }
                        }

                        if (lValidVisitIDs.Count < 1)
                        {
                            MessageBox.Show("No valid visit IDs found in file.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Clear the user interface of filtering items.
                        chkVisitID.CheckedChanged -= FilterVisits;
                        chkVisitID.Checked = false;
                        chkVisitID.CheckedChanged += new EventHandler(FilterVisits);

                        ClearCheckedItems(ref lstWatershed);
                        ClearCheckedItems(ref lstSite);
                        ClearCheckedItems(ref lstFieldSeason);

                        string sFilter = string.Format("VisitID IN ({0})", string.Join(",", lValidVisitIDs));
                        DataView dv = (DataView)grdVisits.DataSource;
                        System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                        dv.RowFilter = sFilter;
                    }
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        private void exportSeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdVisits.SelectedRows.Count < 1)
            {
                MessageBox.Show("Select one or more rows in the main grid before using this feature.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save Visit IDs CSV File";
            frm.Filter = "Comma Separated Value (CSV) Files (*.csv)|*.csv";
            frm.FileName = string.Format("{0:yyyy_mm_dd}_WorkbenchVisits.csv", DateTime.Now);
            frm.OverwritePrompt = true;

            // Default location is the Workbench InputOutputFiles folder
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder))
                frm.InitialDirectory = CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> lVisitIDs = new List<string>();
                foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
                {
                    DataRowView drv = (DataRowView)aRow.DataBoundItem;
                    DataRow r = drv.Row;
                    lVisitIDs.Add(((int)r["VisitID"]).ToString());
                }

                File.WriteAllText(frm.FileName, string.Join(",", lVisitIDs.ToArray<string>()));
            }
        }

        private void selectAllVisitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grdVisits.SelectAll();
        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grdVisits.ClearSelection();
        }

        private string CreateNewWorkbenchDB()
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Create New Workbench Database";
            frm.Filter = "Access Databases (*.mdb)|*.mdb";
            frm.FileName = "Workbench";
            frm.AddExtension = true;
            frm.OverwritePrompt = true;

            string sNewDatabasePath = string.Empty;
            if (m_dbCon is OleDbConnection)
            {
                sNewDatabasePath = GetDatabasePathFromConnectionString(m_dbCon.ConnectionString);
                if (!string.IsNullOrEmpty(sNewDatabasePath))
                {
                    sNewDatabasePath = System.IO.Path.GetDirectoryName(sNewDatabasePath);
                    if (System.IO.Directory.Exists(sNewDatabasePath))
                        frm.InitialDirectory = sNewDatabasePath;
                }
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // This is now the file path desired by the user.
                sNewDatabasePath = frm.FileName;

                // Build the path to the zipped master copy of the Workbench database
                string sMaster = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                sMaster = System.IO.Path.Combine(sMaster, "WorkbenchMaster.zip");
                if (System.IO.File.Exists(sMaster))
                {
                    // Build a temporary file path where the master will be unzipped. This needs to be unique so used datetime stamp in %TEMP%
                    string sTempFolder = string.Format("{0}_{1:yyyyMMddHHmmss}", System.IO.Path.GetFileNameWithoutExtension(CHaMPWorkbench.Properties.Settings.Default.WorkbenchMasterFileName), DateTime.Now);
                    sTempFolder = System.IO.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), sTempFolder);

                    // Unzip the master copy to the folder desired by the user.
                    Data.frmDataUnPacker.UnZipArchive(sMaster, sTempFolder);

                    // If the user has requested a different name than the master then rename the file
                    string sTempFile = System.IO.Path.Combine(sTempFolder, CHaMPWorkbench.Properties.Settings.Default.WorkbenchMasterFileName);
                    if (System.IO.File.Exists(sTempFile))
                    {
                        if (string.Compare(sTempFile, sNewDatabasePath, true) != 0)
                            System.IO.File.Move(sTempFile, frm.FileName);

                        if (!System.IO.File.Exists(frm.FileName))
                            MessageBox.Show("Failed to extract master database from software deployment.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show(string.Format("Failed to find master workbench database called '{0}' in unzipped temporary folder.", 
                            CHaMPWorkbench.Properties.Settings.Default.WorkbenchMasterFileName),
                            CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Failed to find master database with software deployment.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return frm.FileName;
        }

        private void createNewWorkbenchDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sNewDB = CreateNewWorkbenchDB();
            if (System.IO.File.Exists(sNewDB))
            {
                OpenDatabase(sNewDB);
            }
        }
    }
}

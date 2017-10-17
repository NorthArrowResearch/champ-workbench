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
using System.IO;
using System.Xml;
using System.Data.SQLite;
using naru.db.sqlite;

namespace CHaMPWorkbench
{
    public partial class MainForm : Form
    {
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

            // Developers can put an XML file in their code folder that defines certain default properties for the application.
            LoadDeveloperCredentials();

            string sPath = GetDatabasePathFromConnectionString(CHaMPWorkbench.Properties.Settings.Default.DBConnection);
            OpenDatabase(sPath);
        }

        private bool VerifyDBVersion(string sFilePath)
        {
            bool bSuccess = false;

            DBManager db = new DBManager(sFilePath, Properties.Resources.DBVersionQuery,
                  Properties.Settings.Default.DBMinSupportedVersion, Properties.Resources.DBFolder, Properties.Resources.DBStructureFile, Properties.Resources.DBUpdatePattern);

            int nVersion = db.GetDBVersion();
            DBManager.UpgradeStates eState = db.CheckUpgradeStatus(CHaMPWorkbench.Properties.Settings.Default.DBVersionRequired);
            string msgCreate = "You can create a new copy of the compatible database version using the 'Create New Workbench Database...' item on the main window file menu.";

            switch (eState)
            {
                case DBManager.UpgradeStates.MatchesCurrentVersion:
                    bSuccess = true;
                    break;

                case DBManager.UpgradeStates.BelowMinimumVersion:
                    MessageBox.Show(string.Format("The version of this Workbench database ({0}) is less than the minimum version ({1}) that is compatible with the Workbench software. {2}"
                        , nVersion, db.MinimumSupportedVersion, msgCreate),
                        "Unsupported Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case DBManager.UpgradeStates.ExceedsCurrentVersion:
                    MessageBox.Show(string.Format("Attempting to open the database {2}, but the version of this Workbench database ({0})" +
                        " is newer than the version that is compatible with this version of the Workbench software ({1}). {3}",
                        nVersion, CHaMPWorkbench.Properties.Settings.Default.DBVersionRequired, sFilePath, msgCreate), "Incompatible Workbench Database Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;

                case DBManager.UpgradeStates.RequiresUpgrade:

                    string sMessage = string.Format("You are attempting to open a {0} database that is version {1}." +
                        " Would you like to upgrade your database to version {2} to be compatible with the current CHaMP workbench software? Content will be unaffected by the upgrade.\n\n{3}",
                        CHaMPWorkbench.Properties.Resources.MyApplicationNameLong
                        , db.GetDBVersion(), CHaMPWorkbench.Properties.Settings.Default.DBVersionRequired
                        , sFilePath);

                    if (MessageBox.Show(sMessage, "Database Upgrade Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        try
                        {
                            db.Upgrade(CHaMPWorkbench.Properties.Settings.Default.DBVersionRequired);
                            MessageBox.Show(string.Format("Database upgraded to version {0}.", CHaMPWorkbench.Properties.Settings.Default.DBVersionRequired), "Database Upgrade Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bSuccess = true;
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show("The database update failed. This is likely because you have changed data in read only lookup tables that conflicts with the latest version." +
                                " Your database has been reverted to its original state." +
                                " You should create a new Workbench database and then manually compare the lookup table data to migrate your changes to the latest version.", "Database Update Failed",
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            bSuccess = false;
                        }
                    }
                    break;

                default:
                    throw new Exception(string.Format("Unhandled database state {0}", eState.ToString()));
            }

            return bSuccess;
        }

        public static string GetDatabasePathFromConnectionString(string sConnectionString)
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

        private string ReportFolder
        {
            get
            {
                string sReportFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                sReportFolder = System.IO.Path.Combine(sReportFolder, "Validation");
                sReportFolder = System.IO.Path.Combine(sReportFolder, "ReportTransforms");
                System.Diagnostics.Debug.Assert(System.IO.Directory.Exists(sReportFolder), "The XSL Validation Report Folder does not exist.");
                return sReportFolder;
            }
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
                                subMenu.Enabled = !string.IsNullOrEmpty(DBCon.DatabasePath);
                                break;

                            case "createNewWorkbenchDatabaseToolStripMenuItem":
                                break; // do nothing. Always enabled

                            default:
                                subMenu.Enabled = !string.IsNullOrEmpty(DBCon.DatabasePath);
                                break;
                        }
                    }
                }

                // Now update the tool status strip
                if (!string.IsNullOrEmpty(DBCon.DatabasePath))
                {
                    System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(DBCon.ConnectionString);
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
            dlg.Filter = "SQLite Databases (*.db, *.sqlite)|*.db;*.sqlite|All Files (*.*)|*.*";

            if (!string.IsNullOrEmpty(DBCon.DatabasePath))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(DBCon.DatabasePath);

                // Don't default to Access paths
                if (!(DBCon.DatabasePath.ToLower().EndsWith(".mdb") || DBCon.DatabasePath.ToLower().EndsWith("accdb")))
                    dlg.FileName = System.IO.Path.GetFileName(DBCon.DatabasePath);
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
                if (!string.IsNullOrEmpty(DBCon.DatabasePath))
                {
                    if (string.Compare(dlg.FileName, DBCon.DatabasePath, true) == 0)
                        return;
                }

                OpenDatabase(dlg.FileName);
            }
        }

        private void OpenDatabase(string sDatabasePath)
        {
            if (!string.IsNullOrEmpty(sDatabasePath) && System.IO.File.Exists(sDatabasePath))
            {
                // Temporary code to avoid attempting to open Access databases by mistake.
                if (sDatabasePath.ToLower().EndsWith(".mdb") || sDatabasePath.ToLower().EndsWith("accdb"))
                    return;

                try
                {
                    if (!VerifyDBVersion(sDatabasePath))
                        return;

                    Console.WriteLine("Attempting to open database: " + sDatabasePath);
                    DBCon.ConnectionString = sDatabasePath;

                    if (!string.IsNullOrEmpty(DBCon.ConnectionString))
                    {
                        CHaMPWorkbench.Properties.Settings.Default.DBConnection = DBCon.ConnectionString;
                        CHaMPWorkbench.Properties.Settings.Default.Save();
                        UpdateMenuItemStatus(menuStrip1.Items);
                        AddProgramsToMenu();
                    }
                    LoadVisits();
                }
                catch (Exception ex)
                {
                    DBCon.ConnectionString = string.Empty;
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBCon.CloseDatabase();

            try
            {
                lstFieldSeason.Items.Clear();
                lstWatershed.Items.Clear();
                txtNameFilter.Text = string.Empty;

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
                frmSelectBatches frm = new frmSelectBatches(DBCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_RBT);
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
                RBT.frmRunBatches frm = new RBT.frmRunBatches();
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
                CHaMPWorkbench.frmRBTScavenger rbt = new frmRBTScavenger();
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
                frmAbout frm = new frmAbout();
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
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                AddXSLReportsToMenu();
                AddUserQueriesToMenu();
                AddProgramsToMenu();
                UpdateMenuItemStatus(menuStrip1.Items);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            this.lstFieldSeason.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstWatershed.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstPrograms.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);
            this.lstProtocols.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FilterListBoxCheckChanged);

            grdVisits.AutoGenerateColumns = false;
        }

        private void AddXSLReportsToMenu()
        {
            // Now add dynimic reports into the mix
            foreach (string sReportXSLPath in System.IO.Directory.GetFiles(ReportFolder, "*.xsl", System.IO.SearchOption.TopDirectoryOnly))
            {
                XmlDocument xslDoc = new XmlDocument();
                xslDoc.Load(sReportXSLPath);

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(xslDoc.NameTable);
                nsMgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                XmlNode nodTitle = xslDoc.SelectSingleNode(@"/xsl:stylesheet/xsl:template[@match='report']/html/head/title", nsMgr);

                if (nodTitle is XmlNode && !string.IsNullOrEmpty(nodTitle.InnerText))
                {
                    ToolStripMenuItem addReport = new ToolStripMenuItem(nodTitle.InnerText);
                    CHaMPWorkbench.Classes.MetricValidation.ReportGenerator.ReportItem reportTag = new CHaMPWorkbench.Classes.MetricValidation.ReportGenerator.ReportItem(nodTitle.InnerText, sReportXSLPath);
                    addReport.Tag = reportTag;
                    addReport.Click += (sender, e) =>
                    {
                        ToolStripMenuItem reportMenu = (ToolStripMenuItem)sender;
                        CHaMPWorkbench.Classes.MetricValidation.ReportGenerator.ReportItem reportClickTag = (CHaMPWorkbench.Classes.MetricValidation.ReportGenerator.ReportItem)reportMenu.Tag;
                        List<CHaMPData.VisitBasic> visits = GetSelectedVisitsList();
                        CHaMPWorkbench.Classes.MetricValidation.ReportGenerator reportGenerator = new CHaMPWorkbench.Classes.MetricValidation.ReportGenerator(reportClickTag, visits);
                        try
                        {
                            reportGenerator.GenerateXML();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //Classes.ExceptionHandling.NARException.HandleException(ex);
                        }
                    };
                    modelValidationReportsToolStripMenuItem.DropDownItems.Add(addReport);
                }
                else
                {
                    //lstReports.Items.Add(new ReportItem(System.IO.Path.GetFileNameWithoutExtension(sReportXSLPath), sReportXSLPath));
                }
            }
        }

        private void AddUserQueriesToMenu()
        {
            // This method is called when the main menu first loads and also 
            // whenever the user query management form is closed and changes were made.
            // Clear out any user queries before inserting them back in. 
            // Work backwards through the list and also keep the first two items
            // which are the management form and a separator
            for (int i = userQueriesToolStripMenuItem.DropDownItems.Count - 1; i > 1; i--)
                userQueriesToolStripMenuItem.DropDownItems.RemoveAt(i);

            if (!string.IsNullOrEmpty(DBCon.DatabasePath))
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT QueryID, Title, QueryText FROM User_Queries", dbCon);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        try
                        {
                            ToolStripMenuItem mnuQuery = new ToolStripMenuItem(dbRead.GetString(dbRead.GetOrdinal("Title")));

                            // Build a tag that contains everything the query needs to run
                            mnuQuery.Tag = new UserQueries.frmQueryProperties.UserQueryTag(
                                DBCon.ConnectionString,
                                dbRead.GetString(dbRead.GetOrdinal("QueryText")),
                                dbRead.GetInt64(dbRead.GetOrdinal("QueryID")),
                                dbRead.GetString(dbRead.GetOrdinal("Title")));

                            mnuQuery.Click += UserQueries.frmQueryProperties.RunUserQuery;
                            userQueriesToolStripMenuItem.DropDownItems.Add(mnuQuery);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print("Error adding user query menu item: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmSynchronizeCHaMPData frm = new Data.frmSynchronizeCHaMPData();
                frm.ShowDialog();
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
                Data.frmClearDatabase frm = new Data.frmClearDatabase();
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
                else
                {
                    MessageBox.Show(string.Format("There are no updates available. The {0} software is up to date.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("The application is not deployed over the internet and therefore cannot be updated automatically.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }

        private void testXPathReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Philip.frmTestXPath frm = new Experimental.Philip.frmTestXPath();
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
                MessageBox.Show("Feature unavailable in this version of the CHaMP Workbench", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Experimental.Kelly.frmExtractRBTErrors frm = new Experimental.Kelly.frmExtractRBTErrors();
                //frm.ShowDialog();
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
                Habitat.frmHabitatBatch frm = new Habitat.frmHabitatBatch();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void LoadVisits()
        {
            if (string.IsNullOrEmpty(DBCon.DatabasePath))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                SQLiteCommand dbCom = new SQLiteCommand("SELECT * FROM vwMainVisitList ORDER BY WatershedName, SiteName, VisitID", dbCon);
                SQLiteDataAdapter daVisits = new SQLiteDataAdapter(dbCom);
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

                // Load the field seasons and watersheds
                naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstFieldSeason, DBCon.ConnectionString, "SELECT VisitYear, CAST(VisitYear AS text) FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear DESC", false);
                naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstWatershed, DBCon.ConnectionString, "SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) GROUP BY WatershedID, WatershedName ORDER BY WatershedName", false);
                naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstPrograms, DBCon.ConnectionString, "SELECT ProgramID, Title FROM LookupPrograms ORDER BY Title", false);
                naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstProtocols, DBCon.ConnectionString, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8 ORDER BY Title", false);
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
                AddCheckedListboxFilter(ref lstPrograms, ref sFilter, "ProgramID");
                AddCheckedListboxFilter(ref lstProtocols, ref sFilter, "ProtocolID");

                if (!string.IsNullOrEmpty(txtNameFilter.Text))
                {
                    if (!string.IsNullOrWhiteSpace(sFilter))
                        sFilter += " AND ";

                    sFilter += string.Format(" ( (SiteName LIKE '*{0}*') OR (StreamName LIKE '*{0}*') OR (Organization LIKE '*{0}*') )", CleanFilterString(txtNameFilter.Text));
                }

                if (rdoPrimary.Checked)
                {
                    if (!string.IsNullOrWhiteSpace(sFilter))
                        sFilter += " AND ";

                    sFilter += " (IsPrimary <> 0) ";
                }
            }

            if (grdVisits.DataSource is DataView)
            {
                DataView dv = (DataView)grdVisits.DataSource;
                System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                dv.RowFilter = sFilter;
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

        }

        private string CleanFilterString(string sOriginal)
        {
            return sOriginal.Replace("'", "").Replace("\"", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "");
        }

        private void AddCheckedListboxFilter(ref CheckedListBox lst, ref string sFilter, string sPropertyName, bool bUseNameInsteadOfValue = false)
        {
            // Only add filter if not all the items are checked and there is some filtering to do
            if (lst.CheckedItems.Count == 0 || lst.CheckedItems.Count == lst.Items.Count)
                return;

            string sValueList = "";
            foreach (naru.db.NamedObject l in lst.CheckedItems)
            {
                if (bUseNameInsteadOfValue)
                    sValueList += "'" + l.ToString() + "', ";
                else
                    sValueList += l.ID.ToString() + ", ";
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
                sPath = System.IO.Path.Combine(sParentFolder, ((Int64)dr["VisitYear"]).ToString());
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
                    List<CHaMPData.VisitBasic> lVisits = new List<CHaMPData.VisitBasic>();
                    lVisits.Add(new CHaMPData.VisitBasic((long)r["VisitID"], (long)r["WatershedID"], (string)r["WatershedName"], (long)r["SiteID"], (string)r["SiteName"], (long)r["VisitYear"], string.Empty, (long)r["ProgramID"], naru.db.DBState.Unchanged));
                    Data.frmFTPVisit frm = new Data.frmFTPVisit(lVisits);
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
                    lstWatershed.ItemCheck -= FilterListBoxCheckChanged;
                    txtNameFilter.TextChanged -= ControlChange_FilterVisits;

                    long nSiteID = (long)r["SiteID"];
                    txtNameFilter.Text = (string)r["SiteName"];
                    long nWatershedID = (long)r["WatershedID"];

                    for (int i = 0; i < lstFieldSeason.Items.Count; i++)
                        lstFieldSeason.SetItemChecked(i, true);

                    for (int i = 0; i < lstWatershed.Items.Count; i++)
                    {
                        lstWatershed.SetItemChecked(i, ((naru.db.NamedObject)lstWatershed.Items[i]).ID == nWatershedID);
                        if (((naru.db.NamedObject)lstWatershed.Items[i]).ID == nWatershedID)
                            lstWatershed.TopIndex = i;
                    }

                    // turn on event handling
                    valVisitID.ValueChanged += valVisitID_ValueChanged;
                    lstWatershed.ItemCheck += FilterListBoxCheckChanged;
                    txtNameFilter.TextChanged += ControlChange_FilterVisits;
                }

                FilterVisits(sender, e);
                chkVisitID.CheckedChanged += FilterVisits;
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

        private void AllNoneProtocolsClick(object sender, EventArgs e)
        {
            // turn off event handling
            lstProtocols.ItemCheck -= FilterListBoxCheckChanged;

            try
            {
                for (int i = 0; i < lstProtocols.Items.Count; i++)
                    lstProtocols.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));

                // Turn on event handling
                lstProtocols.ItemCheck += FilterListBoxCheckChanged;

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
                    Classes.CSVGenerators.ChannelUnitCSVGenerator csv = new Classes.CSVGenerators.ChannelUnitCSVGenerator(DBCon.ConnectionString);
                    csv.Run((long)r["VisitID"], sTopoFolder);

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
            Dictionary<long, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(dVisits);
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

                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
                {
                    dbCon.Open();
                    try
                    {
                        SQLiteCommand dbCom = new SQLiteCommand("SELECT Latitude, Longitude FROM CHAMP_Sites S INNER JOIN CHAMP_Visits V ON S.SiteID = V.SiteID WHERE (V.VisitID = @VisitID)", dbCon);
                        dbCom.Parameters.AddWithValue("@VisitID", (long)r["VisitID"]);
                        SQLiteDataReader dbRead = dbCom.ExecuteReader();
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
        }

        /// <summary>
        /// Creates a dictionary of visits that are selected in the main grid.
        /// </summary>
        /// <remarks>Use this method for any tool that should take a list of visits.
        /// e.g. input file batch builders</remarks>
        /// <returns>Dictionary, Key is Visit ID and value is a string name for the visit:
        /// YYYY, WatershedName, Site, Visit XXXX</returns>
        private Dictionary<long, string> GetSelectedVisits()
        {
            Dictionary<long, string> dVisits = new Dictionary<long, string>();
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;
                string sLabel = string.Format("{0}, {1}, {2}, Visit {3}", r["WatershedName"], r["VisitYear"], r["SiteName"], r["VisitID"]);
                dVisits.Add((long)r["VisitID"], sLabel);
            }

            return dVisits;
        }

        private List<CHaMPData.VisitBasic> GetSelectedVisitsList()
        {
            List<CHaMPData.VisitBasic> lVisits = new List<CHaMPData.VisitBasic>();
            foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
            {
                DataRowView drv = (DataRowView)aRow.DataBoundItem;
                DataRow r = drv.Row;
                lVisits.Add(new CHaMPData.VisitBasic((long)r["VisitID"], (long)r["WatershedID"], (string)r["WatershedName"], (long)r["SiteID"], (string)r["SiteName"], (long)r["VisitYear"], string.Empty, (long)r["ProgramID"], naru.db.DBState.Unchanged));
            }

            return lVisits;
        }


        private void buildInputFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<long, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                frmRBTInputBatch frm = new frmRBTInputBatch(dVisits);

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
                    int nExported = aws.Run(fiExport);

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
                        ClearCheckedItems(ref lstFieldSeason);
                        txtNameFilter.Text = string.Empty;

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

        private void selectBatchesToRunToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmSelectBatches frm = new frmSelectBatches(DBCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_GUT);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runSelectedBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUT.frmGUTRun frm = new GUT.frmGUTRun(DBCon.ConnectionString);
            frm.ShowDialog();
        }

        private void recordPostGCDQAQCRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.James.frmEnterPostGCD_QAQC_Record frm = new Experimental.James.frmEnterPostGCD_QAQC_Record();
            frm.ShowDialog();
        }

        private void gCDAnalysisWatershedLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.James.frmGCD_MetricsViewer frm = new Experimental.James.frmGCD_MetricsViewer(DBCon.ConnectionString);
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

                long nWatershedID = (long)r["WatershedID"];
                long nSiteID = (long)r["SiteID"];

                Experimental.James.frmUSGS_StreamDataViewer frm = new Experimental.James.frmUSGS_StreamDataViewer(nSiteID, nWatershedID);
                frm.ShowDialog();
            }
        }

        private void buildInputFilesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Dictionary<long, string> dVisits = GetSelectedVisits();

            if (dVisits.Count > 0)
            {
                HydroPrep.frmHydroPrepBatchBuilder frm = new HydroPrep.frmHydroPrepBatchBuilder(DBCon.ConnectionString, dVisits);

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
                frmSelectBatches frm = new frmSelectBatches(DBCon.ConnectionString, CHaMPWorkbench.Properties.Settings.Default.ModelType_HydroPrep);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runSelectedBatchesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HydroPrep.frmHydroPrepRun frm = new HydroPrep.frmHydroPrepRun(DBCon.ConnectionString);
            frm.ShowDialog();
        }

        private void runHabitatBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Habitat.frmHabitatRun frm = new Habitat.frmHabitatRun(DBCon.ConnectionString);
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
                        ClearCheckedItems(ref lstFieldSeason);
                        txtNameFilter.Text = string.Empty;

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
            frm.FileName = string.Format("{0:yyyy_MM_dd}_WorkbenchVisits.csv", DateTime.Now);
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
                    lVisitIDs.Add(((long)r["VisitID"]).ToString());
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

        private string CreateAndOpenNewDatabase()
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Create New Workbench Database";
            frm.Filter = "Workbench Databases (*.db)|*.db";
            frm.FileName = "Workbench";
            frm.AddExtension = true;
            frm.OverwritePrompt = true;

            string sNewDatabasePath = string.Empty;
            if (!string.IsNullOrEmpty(DBCon.DatabasePath))
            {
                sNewDatabasePath = GetDatabasePathFromConnectionString(DBCon.ConnectionString);
                if (!string.IsNullOrEmpty(sNewDatabasePath))
                {
                    sNewDatabasePath = System.IO.Path.GetDirectoryName(sNewDatabasePath);
                    if (System.IO.Directory.Exists(sNewDatabasePath))
                        frm.InitialDirectory = sNewDatabasePath;
                }
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DBManager db = new DBManager(frm.FileName, Properties.Resources.DBVersionQuery,
                    Properties.Settings.Default.DBMinSupportedVersion, Properties.Resources.DBFolder, Properties.Resources.DBStructureFile, Properties.Resources.DBUpdatePattern);

                db.CreateDatabase();
                if (System.IO.File.Exists(frm.FileName))
                {
                    OpenDatabase(frm.FileName);
                }
                Cursor.Current = System.Windows.Forms.Cursors.Default;

                if (MessageBox.Show("The new database was created successfully. You should now download the latest list of watersheds, sites and visits from CHaMP Monitoring." +
                    " Do you want to open the CHaMP data synchronization tool?", "New Database Creation Successful",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click(this, null);
                }
            }

            return frm.FileName;
        }

        private void createNewWorkbenchDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateAndOpenNewDatabase();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void scavengeMetricsFromCmorgDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Models.frmScavengeMetrics frm = new Models.frmScavengeMetrics(DBCon.ConnectionString);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        #region VisitProperties

        private void grdVisits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRowView drv = (DataRowView)grdVisits.Rows[e.RowIndex].DataBoundItem;
                ShowVisitProperties(drv.Row);
            }
        }

        private void visitPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow r = null;
            if (grdVisits.SelectedRows.Count == 1)
            {
                DataRowView drv = (DataRowView)grdVisits.SelectedRows[0].DataBoundItem;
                ShowVisitProperties(drv.Row);
            }
        }

        private void ShowVisitProperties(DataRow visitRow)
        {
            if (visitRow is DataRow)
            {
                Data.frmVisitDetails frm = new Data.frmVisitDetails(DBCon.ConnectionString, (long)visitRow["VisitID"]);
                frm.ShowDialog();
            }
        }

        #endregion

        private void createCustomVisitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmCustomVisit frm = new Data.frmCustomVisit(DBCon.ConnectionString);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    LoadVisits();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    // TODO Scroll the main grid to the newly inserted visit.
                    //DataTable dtVisits = (DataTable) grdVisits.DataSource;
                    //dtVisits.fi
                    //grdVisits.FirstDisplayedScrollingRowIndex = 
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void ControlChange_FilterVisits(object sender, EventArgs e)
        {
            FilterVisits(sender, e);
        }

        private void manageQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserQueries.frmManageQueries frm = new UserQueries.frmManageQueries(DBCon.ConnectionString);
            frm.ShowDialog();

            if (frm.UserQueriesChanged)
            {
                AddUserQueriesToMenu();
            }
        }

        private void generateChannelUnitCSVFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdVisits.SelectedRows.Count < 1)
            {
                MessageBox.Show("There are no visits selected in the main grid. Select one or more visits for which you want to generate channel unit CSV files.", "No Visits Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (MessageBox.Show(string.Format("This process will generate {0} new channel unit CSV files for the selected visits. It will overwrite any existing channel unit CSV files for the selected visits, should they already exist," +
                    " and create the necessary folders should they not already exist. Are you sure that you want to create {0} channel unit CSV files?", grdVisits.SelectedRows.Count), "Generate Channel Unit CSVs", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK)
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) || !System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
            {
                MessageBox.Show("The top level monitoring data path must be set before this tool is run. Go to the Tools > Options menu to set this folder before attempting to use this tool.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

                Classes.CSVGenerators.ChannelUnitCSVGenerator csv = new Classes.CSVGenerators.ChannelUnitCSVGenerator(DBCon.ConnectionString);
                int nComplete = 0;
                foreach (DataGridViewRow r in grdVisits.SelectedRows)
                {
                    DataRowView drv = (DataRowView)r.DataBoundItem;
                    System.IO.DirectoryInfo dTopoFolder = null;
                    System.IO.DirectoryInfo dMonitoringDataFolder = new DirectoryInfo(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder);
                    Classes.DataFolders.Topo(dMonitoringDataFolder, (long)drv["VisitID"], out dTopoFolder);
                    if (dTopoFolder is System.IO.DirectoryInfo)
                    {
                        if (!dTopoFolder.Exists)
                            dTopoFolder.Create();

                        System.IO.FileInfo fiCSV = new FileInfo(System.IO.Path.Combine(dTopoFolder.FullName, "ChannelUnits.csv"));
                        fiCSV = csv.Run((long)drv["VisitID"], fiCSV.FullName);
                        if (fiCSV.Exists)
                            nComplete++;
                    }
                }

                MessageBox.Show(string.Format("Process complete. {0} channel unit CSV file(s) created.", nComplete), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
        }

        private void createNewUserFeedbackItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmUserFeedback frm = new Data.frmUserFeedback(DBCon.ConnectionString);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void metricResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmMetricGrid frm = new Data.frmMetricGrid(DBCon.ConnectionString, GetSelectedVisitsList());
            frm.ShowDialog();
        }

        private void ShowMetricReviewForm(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                try
                {
                    naru.db.NamedObject aProgram = (sender as ToolStripMenuItem).Tag as naru.db.NamedObject;
                    Data.frmMetricReview frm = new Data.frmMetricReview(DBCon.ConnectionString, GetSelectedVisitsList(), aProgram);
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }


        private void AddProgramsToMenu()
        {
            metricReviewToolStripMenuItem.DropDownItems.Clear();
            if (!string.IsNullOrEmpty(DBCon.DatabasePath))
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT ProgramID, Title FROM LookupPrograms ORDER BY Title", dbCon);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        ToolStripMenuItem mnuQuery = new ToolStripMenuItem(dbRead.GetString(dbRead.GetOrdinal("Title")));

                        // Build a tag that contains everything the query needs to run
                        mnuQuery.Tag = new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("ProgramID")), dbRead.GetString(dbRead.GetOrdinal("Title")));
                        mnuQuery.Click += this.ShowMetricReviewForm;
                        metricReviewToolStripMenuItem.DropDownItems.Add(mnuQuery);
                    }
                }
            }
        }

        private void userFeedbackItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmUserFeedbackGrid frm = new Data.frmUserFeedbackGrid(DBCon.ConnectionString, GetSelectedVisitsList());
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void userFeedbackForAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.frmUserFeedbackGrid frm = new Data.frmUserFeedbackGrid(DBCon.ConnectionString, null);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

        }

        private void exportSelectedVisitInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog frm = new System.Windows.Forms.SaveFileDialog();
                frm.Title = "Save Visit Info CSV";
                frm.Filter = "Comma Separated Value (CSV) Files|*.csv";
                frm.FileName = string.Format("visit_info_{0:yyyyMMdd_HHmm}.csv", DateTime.Now);
                frm.AddExtension = true;

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var csv = new StringBuilder();
                    csv.AppendLine("year,watershed,site,visitid,relativepath,s3");
                    foreach (DataGridViewRow aRow in grdVisits.SelectedRows)
                    {
                        DataRowView drv = (DataRowView)aRow.DataBoundItem;
                        DataRow r = drv.Row;
                        System.IO.DirectoryInfo dirVisit = null;

                        CHaMPData.VisitBasic visit = new CHaMPData.VisitBasic((long)r["VisitID"], (long)r["WatershedID"], (string)r["WatershedName"], (long)r["SiteID"], (string)r["SiteName"], (long)r["VisitYear"], string.Empty, (long)r["ProgramID"], naru.db.DBState.Unchanged);
                        Classes.DataFolders.Visit(new System.IO.DirectoryInfo(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder), (long)r["VisitID"], out dirVisit);

                        string s3 = string.Format("QA/{0}", visit.VisitFolderRelative.Replace(" ", "").Replace(System.IO.Path.DirectorySeparatorChar, '/'));
                        csv.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", visit.VisitYear, visit.Site.Watershed, visit.Site, visit.ID, visit.VisitFolderRelative, s3));

                    }
                    File.WriteAllText(frm.FileName, csv.ToString());
                    if (MessageBox.Show("Do you want to open the CSV file?", "Process Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        System.Diagnostics.Process.Start(frm.FileName);
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

        }

        private void downloadTopoDataAndHydraulicModelFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CHaMPData.VisitBasic> lVisits = GetSelectedVisitsList();
            try
            {
                Data.frmFTPVisit frm = new Data.frmFTPVisit(lVisits);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void LoadDeveloperCredentials()
        {

#if !DEBUG
            return;
#endif
            // The executable runs in different folder on x64 and AnyCPU (needed for user control development).
            // So look for the developer config file up as many as three levels of the folder hierarchy
            // providing that those folders exist (and a debug copy is not sitting somewhere very high up on a 
            // testers computer) where there aren't necessarily enough levels of the hierarchy.
            System.IO.FileInfo fiEXE = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().FullName);
            System.IO.DirectoryInfo diSearch = fiEXE.Directory;
            if (diSearch.Parent is System.IO.DirectoryInfo)
            {
                if (diSearch.Parent.Parent.Parent is System.IO.DirectoryInfo)
                    diSearch = diSearch.Parent.Parent.Parent;
                else
                    diSearch = diSearch.Parent;
            }

            string[] sConfigFiles = System.IO.Directory.GetFiles(diSearch.FullName, "workbench_config.xml", SearchOption.AllDirectories);

            if (sConfigFiles.Count<string>() > 0)
            {
                string sConfigPath = sConfigFiles[0];
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sConfigPath);
                StoreDeveloperSetting(ref xmlDoc, "WorkbenchConfig/MonitoringDataUnzipped", "MonitoringDataFolder");
                StoreDeveloperSetting(ref xmlDoc, "WorkbenchConfig/MonitoringDataZipped", "ZippedMonitoringDataFolder");
                StoreDeveloperSetting(ref xmlDoc, "WorkbenchConfig/InputOutputFiles", "InputOutputFolder");
                StoreDeveloperSetting(ref xmlDoc, "WorkbenchConfig/TempFolder", "LastTempFolder");
                StoreDeveloperSetting(ref xmlDoc, "WorkbenchConfig/GeoOptixUserName", "DefaultUserName");

                XmlNode nod = xmlDoc.SelectSingleNode("WorkbenchConfig/GeoOptixPassword");
                if (nod is XmlNode)
                {
                    if (!string.IsNullOrEmpty(nod.InnerText))
                    {
                        DBCon.SetSessionSetting("KeystonePassword", nod.InnerText);
                    }
                }
                CHaMPWorkbench.Properties.Settings.Default.Save();
            }
        }

        private void StoreDeveloperSetting(ref XmlDocument xmlDoc, string xPath, string sSettingName)
        {
            XmlNode nod = xmlDoc.SelectSingleNode(xPath);
            if (nod is XmlNode)
            {
                if (!string.IsNullOrEmpty(nod.InnerText))
                {
                    CHaMPWorkbench.Properties.Settings.Default[sSettingName] = nod.InnerText;
                }
            }
        }

        private void scavengeTopoMetricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Philip.frmMetricScraper frm = new Experimental.Philip.frmMetricScraper();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void uploadTopoSurveyProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.frmAPIUpload frm = new Data.frmAPIUpload();
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void metricDefinitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.MetricDefinitions.frmMetricDefinitions frm = new Data.MetricDefinitions.frmMetricDefinitions();
            frm.ShowDialog();
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void metricDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.Metrics.frmMetricDownload frm = new Data.Metrics.frmMetricDownload(GetSelectedVisitsList());
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void runAWSAutomationWorkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experimental.Philip.frmLambdaInvoke frm = new Experimental.Philip.frmLambdaInvoke(GetSelectedVisitsList());
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void copyMetricValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.Metrics.CopyMetrics.frmCopyMetrics frm = new Data.Metrics.CopyMetrics.frmCopyMetrics();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void scrapValidationLogXMLFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Philip.ValidationScraper.Run();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void exportMetricValuesForSelectedVisitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Experimental.Philip.frmExportMetricValues frm = new Experimental.Philip.frmExportMetricValues(GetSelectedVisitsList());
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void metricUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Data.Metrics.Upload.frmMetricUpload frm = new Data.Metrics.Upload.frmMetricUpload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void selectProtocolsWithTopoDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bTopoData = string.Compare(((ToolStripItem)sender).Name, "selectProtocolsWithTopoDataToolStripMenuItem", true) == 0;
            
            List<long> protocolsWithTopoData = new List<long>() { 416, 806, 1880, 1955, 1966, 2020, 2030, 9999, 10036 };

            for (int i = 0; i < lstProtocols.Items.Count; i++)
            {
                if (bTopoData)
                {
                    lstProtocols.SetItemChecked(i, protocolsWithTopoData.Contains(((naru.db.NamedObject)lstProtocols.Items[i]).ID));
                }
                else
                {
                    lstProtocols.SetItemChecked(i, !protocolsWithTopoData.Contains(((naru.db.NamedObject)lstProtocols.Items[i]).ID));
                }
            }

            FilterVisits(sender, e);
        }
    }
}

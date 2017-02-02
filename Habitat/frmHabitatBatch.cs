using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Habitat
{
    public partial class frmHabitatBatch : Form
    {
        private const int m_nSelectionColumnIndex = 0;

        public frmHabitatBatch()
        {
            InitializeComponent();
        }

        private void frmHabitatBatch_Load(object sender, EventArgs e)
        {
            grdVisits.AutoGenerateColumns = false;

            LoadFieldSeasons();
            LoadWatersheds();
            LoadVisitTypes();

            // Load all the species into the checked listbox BEFORE loading all the visit data
            chkSpecies.Items.Add(new SpeciesListItem("Upper Columbia Chinook", "UC_Chin", 1), false);
            chkSpecies.Items.Add(new SpeciesListItem("Snake River Chinook", "SN_Chin", 2), false);
            chkSpecies.Items.Add(new SpeciesListItem("Lower Columbia Steelhead", "LC_Steel", 3), false);
            chkSpecies.Items.Add(new SpeciesListItem("Mid Columbia Steelhead", "MC_Steel", 4), false);
            chkSpecies.Items.Add(new SpeciesListItem("Upper Columbia Steelhead", "UC_Steel", 5), false);
            chkSpecies.Items.Add(new SpeciesListItem("Snake River Steelhead", "SN_Steel", 6), false);

            try
            {
                LoadAllVisits();
                FilterVisits(sender, e);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            if (!string.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
            {
                txtMonitoringFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
                txtD50TopLevel.Text = txtMonitoringFolder.Text;
            }
        }

        private void LoadFieldSeasons()
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref chkFieldSeasons, DBCon.ConnectionString, "SELECT VisitYear FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear", true);
        }

        private void LoadWatersheds()
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref chkWatersheds, DBCon.ConnectionString, "SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) ORDER BY WatershedName", true);
        }

        private void LoadVisitTypes()
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref chkVisitTypes, DBCon.ConnectionString, "SELECT 1, PanelName FROM CHAMP_Visits WHERE (CHAMP_Visits.PanelName Is Not Null) GROUP BY PanelName ORDER BY PanelName", false);
        }

        private void LoadAllVisits()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Selected", typeof(bool)));
            table.Columns.Add(new DataColumn("VisitID", typeof(int)));
            table.Columns.Add(new DataColumn("FieldSeason", typeof(int)));
            table.Columns.Add(new DataColumn("IsPrimary", typeof(bool)));
            table.Columns.Add(new DataColumn("PanelName", typeof(string)));
            //table.Columns.Add(new DataColumn("SurveyGDB", typeof(string)));
            //table.Columns.Add(new DataColumn("VisitFolder", typeof(string)));
            //table.Columns.Add(new DataColumn("HydraulicModelCSV", typeof(string)));
            table.Columns.Add(new DataColumn("SiteName", typeof(string)));
            table.Columns.Add(new DataColumn("WatershedID", typeof(int)));
            table.Columns.Add(new DataColumn("WatershedName", typeof(string)));
            //table.Columns.Add(new DataColumn("ICRPath", typeof(string)));

            string sSQL = "SELECT VisitID, VisitYear AS FieldSeason, IsPrimary, PanelName, CHAMP_Sites.SiteName, CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName";

            // Add the species to the SQL query and also to the receiving database table
            foreach (SpeciesListItem sli in chkSpecies.Items)
            {
                table.Columns.Add(new DataColumn(sli.FieldName, typeof(bool)));
                sSQL += ", " + sli.FieldName;
            }

            sSQL += " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN CHAMP_Visits ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
                   //" WHERE ((CHAMP_Visits.Folder Is Not Null) AND (CHAMP_Visits.HydraulicModelCSV Is Not Null))" +
                   " ORDER BY CHAMP_Visits.VisitYear, CHAMP_Watersheds.WatershedName";

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();

                object[] rowArray = new object[table.Columns.Count];
                while (dbRead.Read())
                {
                    //System.Diagnostics.Debug.Print(dbRead["VisitID"].ToString());

                    rowArray[0] = false;
                    rowArray[1] = (int)dbRead["VisitID"];
                    rowArray[2] = (int)(Int16)dbRead["FieldSeason"];
                    rowArray[3] = (bool)dbRead["IsPrimary"];

                    if (DBNull.Value == dbRead["PanelName"])
                        rowArray[4] = "";
                    else
                        rowArray[4] = (string)dbRead["PanelName"];

                    //if (DBNull.Value == dbRead["SurveyGDB"])
                    //    rowArray[5] = "";
                    //else
                    //    rowArray[5] = (string)dbRead["SurveyGDB"];

                    //rowArray[6] = (string)dbRead["VisitFolder"];
                    //rowArray[7] = (string)dbRead["HydraulicModelCSV"];
                    rowArray[5] = (string)dbRead["SiteName"];
                    rowArray[6] = (int)dbRead["WatershedID"];
                    rowArray[7] = (string)dbRead["WatershedName"];

                    //if (DBNull.Value == dbRead["ICRPath"])
                    //    rowArray[11] = DBNull.Value;
                    //else
                    //    rowArray[11] = (string)dbRead["ICRPath"];

                    int i = 8;
                    foreach (SpeciesListItem sli in chkSpecies.Items)
                    {
                        rowArray[i] = (bool)dbRead[sli.FieldName];
                        i++;
                    }

                    table.Rows.Add(rowArray);
                }

                //grdVisits.DataSource = null;
                //bindingSourceSelectedVisits.DataSource = dt;
                grdVisits.DataSource = table.AsDataView(); // lVisits;
            }
        }

        private void FilterVisits(object sender, EventArgs e)
        {
            // Filter the binding source.

            bindingSourceSelectedVisits.Filter = "";

            string sFilter = "";
            AddCheckedListboxFilter(ref chkFieldSeasons, ref sFilter, "FieldSeason");
            AddCheckedListboxFilter(ref chkWatersheds, ref sFilter, "WatershedID");
            AddCheckedListboxFilter(ref chkVisitTypes, ref sFilter, "PanelName", true);

            if (chkSpecies.CheckedItems.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(sFilter))
                    sFilter += " AND ";
                sFilter += "(";

                for (int i = 0; i < chkSpecies.CheckedItems.Count; i++)
                {
                    if (i > 0)
                        sFilter += " OR ";

                    sFilter += string.Format("{0} = 1", ((SpeciesListItem)chkSpecies.CheckedItems[i]).FieldName);
                }
                sFilter += ") ";
            }

            if (chkPrimary.Checked)
            {
                if (!string.IsNullOrWhiteSpace(sFilter))
                    sFilter += " AND ";
                sFilter += "IsPrimary = true";
            }

            if (grdVisits.DataSource is DataView)
            {
                try
                {
                    DataView dv = (DataView)grdVisits.DataSource;
                    System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                    dv.RowFilter = sFilter;
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void AddCheckedListboxFilter(ref CheckedListBox lst, ref string sFilter, string sPropertyName, bool bUseNameInsteadOfValue = false)
        {
            // Checking no items omits the filter.
            if (lst.CheckedItems.Count < 1)
                return;

            string sValueList = "";
            foreach (naru.db.CheckedItem l in lst.CheckedItems)
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

        private void cmdHabitatModelDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Title = "Habitat Model Project Database";
            frm.Filter = "Habitat Model Databases (*.xml)|*.xml";
            frm.CheckFileExists = true;

            if (!string.IsNullOrWhiteSpace(txtHabitatModelDB.Text) && System.IO.File.Exists(txtHabitatModelDB.Text))
            {
                frm.InitialDirectory = System.IO.Path.GetDirectoryName(txtHabitatModelDB.Text);
                frm.FileName = System.IO.Path.GetFileNameWithoutExtension(txtHabitatModelDB.Text);
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtHabitatModelDB.Text = frm.FileName;
        }

        private void cmdBrowseMonitoringDataFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select the top level folder containing the monitoring data. The first level of folders inside the selected folder should represent field seasons.";
            frm.ShowNewFolderButton = false;

            if (!string.IsNullOrWhiteSpace(txtMonitoringFolder.Text) && System.IO.Directory.Exists(txtMonitoringFolder.Text))
                frm.SelectedPath = txtMonitoringFolder.Text;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bool bChangeD50Folder = string.Compare(txtMonitoringFolder.Text, txtD50TopLevel.Text, true) == 0;
                txtMonitoringFolder.Text = frm.SelectedPath;
                if (bChangeD50Folder)
                    txtD50TopLevel.Text = txtMonitoringFolder.Text;
            }
        }

        private void txtHabitatModelDB_TextChanged(object sender, EventArgs e)
        {
            chkModels.Items.Clear();
            if (string.IsNullOrWhiteSpace(txtHabitatModelDB.Text) || !System.IO.File.Exists(txtHabitatModelDB.Text))
                return;

            try
            {
                using (dsHabitat theHabitatProject = new dsHabitat())
                {
                    theHabitatProject.ReadXml(txtHabitatModelDB.Text);

                    foreach (dsHabitat.HSIRow rHSI in theHabitatProject.HSI.Rows)
                    {
                        string sSpecies = theHabitatProject.LookupListItems.FindByItemID(rHSI.SpeciesID).ItemName;
                        string sLifeStage = theHabitatProject.LookupListItems.FindByItemID(rHSI.LifestageID).ItemName;
                        chkModels.Items.Add(new HabitatModelDef(rHSI.HSIID, HabitatModelDef.ModelTypes.HSI, rHSI.Title, sSpecies, sLifeStage));
                    }

                    foreach (dsHabitat.FISRow rFIS in theHabitatProject.FIS.Rows)
                    {
                        string sSpecies = theHabitatProject.LookupListItems.FindByItemID(rFIS.SpeciesID).ItemName;
                        string sLifeStage = theHabitatProject.LookupListItems.FindByItemID(rFIS.LifeStageID).ItemName;
                        chkModels.Items.Add(new HabitatModelDef(rFIS.FISID, HabitatModelDef.ModelTypes.FIS, rFIS.Title, sSpecies, sLifeStage));
                    }
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void cmdSelectAll_Click(object sender, EventArgs e)
        {
            ChangeSelection(true);
        }

        private void cmdSelectNone_Click(object sender, EventArgs e)
        {
            ChangeSelection(false);
        }

        private void ChangeSelection(bool bSelect)
        {
            foreach (DataGridViewRow r in grdVisits.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)r.Cells[m_nSelectionColumnIndex];
                chk.Value = bSelect;
                //DataRow dr = (DataRow)r.DataBoundItem;
                //dr.ItemArray["VisitID"] = bSelect;
            }
        }

        private class SpeciesListItem : naru.db.NamedObject
        {
            private string m_sFieldName;

            public string FieldName { get { return m_sFieldName; } }

            public SpeciesListItem(string sName, string sFieldName, long nValue)
                : base(nValue, sName)
            {
                m_sFieldName = sFieldName;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            HabitatBatchBuilder theBuilder = null;
            int nSuccess = 0;
            int nError = 0;
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (string.IsNullOrWhiteSpace(txtHabitatModelDB.Text))
                {
                    CHaMPWorkbench.Properties.Settings.Default.Habitat_Project = string.Empty;
                }
                else
                {
                    if (System.IO.File.Exists(txtHabitatModelDB.Text) && txtHabitatModelDB.Text.ToLower().EndsWith(".xml"))
                    {
                        theBuilder = new HabitatBatchBuilder(txtHabitatModelDB.Text, txtMonitoringFolder.Text, txtD50TopLevel.Text, txtD50RasterFileName.Text);
                        CHaMPWorkbench.Properties.Settings.Default.Habitat_Project = txtHabitatModelDB.Text;
                    }
                    else
                    {
                        MessageBox.Show("The habitat model must be a valid XML file (e.g. C:\\CHaMP\\MySimulations.xml)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        return;
                    }
                }

                List<long> lVisitIDs = new List<long>();
                foreach (DataGridViewRow r in grdVisits.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)r.Cells[m_nSelectionColumnIndex];
                    if ((bool)chk.Value)
                    {
                        DataRowView dr = (DataRowView)r.DataBoundItem;
                        lVisitIDs.Add((long)dr["VisitID"]);
                    }
                }

                List<HabitatModelDef> lModels = new List<HabitatModelDef>();
                foreach (object obj in chkModels.CheckedItems)
                    lModels.Add((HabitatModelDef)obj);

                try
                {
                    theBuilder.BuildBatch(lVisitIDs, lModels, ref nSuccess, ref nError);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(String.Format("Complete. {0} successful simulations added, and {1} simulations encountered errors.", nSuccess, nError), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            if (grdVisits.Rows.Count < 1)
            {
                MessageBox.Show("The current filters return zero CHaMP visits. Adjust the filters so that at least one CHaMP visit is returned.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            Boolean bAtLeastOneVisitSelected = false;
            foreach (DataGridViewRow r in grdVisits.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)r.Cells[m_nSelectionColumnIndex];
                if ((bool)chk.Value)
                {
                    bAtLeastOneVisitSelected = true;
                    break;
                }
            }

            if (!bAtLeastOneVisitSelected)
            {
                MessageBox.Show("You must check the box next to at least one visit.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtHabitatModelDB.Text) || !System.IO.File.Exists(txtHabitatModelDB.Text))
            {
                MessageBox.Show("You must select a habitat model database to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (chkModels.CheckedItems.Count < 1)
            {
                MessageBox.Show("You must select at least one habitat model to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtMonitoringFolder.Text) || !System.IO.Directory.Exists(txtMonitoringFolder.Text))
            {
                MessageBox.Show("You must specify the top level monitoring data folder to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtD50RasterFileName.Text))
            {
                MessageBox.Show("You must specify a name for the D50 substrate raster files contained under the D50 substrate raster top level folder.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select the top level folder containing the D50 substrate rasters. The first level of folders inside the selected folder should represent field seasons.";
            frm.ShowNewFolderButton = false;

            if (!string.IsNullOrWhiteSpace(txtMonitoringFolder.Text) && System.IO.Directory.Exists(txtMonitoringFolder.Text))
                frm.SelectedPath = txtD50TopLevel.Text;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtD50TopLevel.Text = frm.SelectedPath;
        }

        private void txtMonitoringFolder_TextChanged(object sender, EventArgs e)
        {
        }

        private void SelectAllNone(object sender, EventArgs e)
        {
            ToolStripMenuItem myItem = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)myItem.Owner;

            CheckedListBox lst = (CheckedListBox)cms.SourceControl;
            for (int i = 0; i <= lst.Items.Count - 1; i++)
            {
                lst.SetItemChecked(i, myItem.Text.ToLower().Contains("all"));
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://habitat.northarrowresearch.com/wiki/Online_Help/champ_batch_process.html");
        }

        private void cmdFilterSelectNone_Click(object sender, EventArgs e)
        {
            FilterSelectAllNone(false);
        }
        private void cmdFilterSelectAll_Click(object sender, EventArgs e)
        {
            FilterSelectAllNone(true);
        }

        private void FilterSelectAllNone(bool bAll)
        {
            // Only clear the control that we are looking at.
            switch (tabFilter.SelectedTab.Text)
            {

                case "Field Seasons":
                    for (int idx = 0; idx < chkFieldSeasons.Items.Count; idx++)
                        chkFieldSeasons.SetItemCheckState(idx, ValueCheck(bAll));
                    break;
                case "Watersheds":
                    for (int idx = 0; idx < chkWatersheds.Items.Count; idx++)
                        chkWatersheds.SetItemCheckState(idx, ValueCheck(bAll));
                    break;
                case "Visit Types":
                    for (int idx = 0; idx < chkVisitTypes.Items.Count; idx++)
                        chkVisitTypes.SetItemCheckState(idx, ValueCheck(bAll));
                    break;
                case "Species Present":
                    for (int idx = 0; idx < chkSpecies.Items.Count; idx++)
                        chkSpecies.SetItemCheckState(idx, ValueCheck(bAll));
                    break;
                default:
                    Console.WriteLine("NOPE");
                    break;
            }
        }

        private CheckState ValueCheck(bool bCheck)
        {
            if (bCheck)
                return CheckState.Checked;
            else
                return CheckState.Unchecked;
        }


    }
}

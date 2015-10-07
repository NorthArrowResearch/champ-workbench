using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Habitat
{
    public partial class frmHabitatBatch : Form
    {
        private OleDbConnection m_dbCon;
        private const int m_nSelectionColumnIndex = 0;

        public frmHabitatBatch(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
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

            LoadAllVisits();
            FilterVisits(sender, e);

            if (!string.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
            {
                txtMonitoringFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
                txtD50TopLevel.Text = txtMonitoringFolder.Text;
            }
        }

        private void LoadFieldSeasons()
        {
            OleDbCommand dbCom = new OleDbCommand("SELECT VisitYear FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
                chkFieldSeasons.Items.Add(new ListItem(dbRead.GetInt16(0).ToString(), dbRead.GetInt16(0)), true);
        }

        private void LoadWatersheds()
        {
            OleDbCommand dbCom = new OleDbCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) ORDER BY WatershedName", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
                chkWatersheds.Items.Add(new ListItem((string)dbRead["WatershedName"], (int)dbRead["WatershedID"]), true);
        }

        private void LoadVisitTypes()
        {
            OleDbCommand dbCom = new OleDbCommand("SELECT PanelName FROM CHAMP_Visits WHERE (CHAMP_Visits.PanelName Is Not Null) GROUP BY PanelName ORDER BY PanelName", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            int i = 1;
            while (dbRead.Read())
            {
                ListItem l = new ListItem((string)dbRead["PanelName"], i);
                chkVisitTypes.Items.Add(l, false);
                i++;
            }
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

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();

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
                // bindingSourceSelectedVisits.Filter = sFilter;
                DataView dv = (DataView)grdVisits.DataSource;
                System.Diagnostics.Debug.Print(String.Format("Filtering Visits: {0}", sFilter));
                dv.RowFilter = sFilter;
                //grdVisits.Refresh();
            }
        }

        private void AddCheckedListboxFilter(ref CheckedListBox lst, ref string sFilter, string sPropertyName, bool bUseNameInsteadOfValue = false)
        {
            // Checking no items omits the filter.
            if (lst.CheckedItems.Count < 1)
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

        //private class ViewVisit
        //{
        //    private int m_nVisitID;
        //    private Int16 m_nFieldSeason;
        //    private string m_sPanel;
        //    private Boolean m_bIsPrimary;

        //    private string m_sWatershed;
        //    private int m_nWatershedID;
        //    private string m_sWatershedFolder;

        //    private int m_nSiteID;
        //    private string m_sSite;
        //    private string m_sSiteFolder;

        //    private bool m_bSelected;

        //    public int VisitID { get { return m_nVisitID; } }
        //    private string m_sTopoFolder;
        //    private string m_sSurveyGDB;
        //    private string m_sCSVFile;

        //    public int FieldSeason { get { return (int)m_nFieldSeason; } }
        //    public string TopoFolder { get { return System.IO.Path.Combine(m_nFieldSeason.ToString(), m_sWatershedFolder, m_sSiteFolder, m_sTopoFolder); } }
        //    public string SurveyGDB { get { return System.IO.Path.Combine(TopoFolder, m_sSurveyGDB); } }
        //    public string HydraulicCSV { get { return System.IO.Path.Combine(TopoFolder, m_sCSVFile); } }

        //    public string Watershed { get { return m_sWatershed; } }
        //    public int WatershedID { get { return m_nWatershedID; } }

        //    public string Site { get { return m_sSite; } }
        //    public int SiteID { get { return m_nSiteID; } }

        //    public bool Selected { get { return m_bSelected; } set { m_bSelected = value; } }
        //    public string Panel { get { return m_sPanel; } }
        //    public bool IsPrimary { get { return m_bIsPrimary; } }

        //    public ViewVisit(int nVisitID, string sVisitTopoFolder, string sSurveyGDB, string sCSVFile, Int16 nFieldSeason,
        //        int nWatershedID, string sWatershedFolder, string sWatershed,
        //        int nSiteID, string sSite, string sSiteFolder,
        //        string sPanel, Boolean bIsPrimary)
        //    {
        //        m_nVisitID = nVisitID;
        //        m_sTopoFolder = sVisitTopoFolder;
        //        m_sSurveyGDB = sSurveyGDB;
        //        m_sCSVFile = sCSVFile;
        //        m_nFieldSeason = nFieldSeason;

        //        m_nWatershedID = nWatershedID;
        //        m_sWatershed = sWatershed;
        //        m_sWatershedFolder = sWatershedFolder;

        //        m_nSiteID = nSiteID;
        //        m_sSite = sSite;
        //        m_sSiteFolder = sSiteFolder;

        //        m_sPanel = sPanel;
        //        m_bIsPrimary = bIsPrimary;
        //        m_bSelected = true;
        //    }
        //}

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

        private class SpeciesListItem : ListItem
        {
            private string m_sFieldName;

            public string FieldName { get { return m_sFieldName; } }

            public SpeciesListItem(string sName, string sFieldName, int nValue)
                : base(sName, nValue)
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

            int nSuccess = 0;
            int nError = 0;
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                HabitatBatchBuilder theBuilder = new HabitatBatchBuilder(ref m_dbCon, txtHabitatModelDB.Text, txtMonitoringFolder.Text, txtD50TopLevel.Text, txtD50RasterFileName.Text);
                List<int> lVisitIDs = new List<int>();
                foreach (DataGridViewRow r in grdVisits.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)r.Cells[m_nSelectionColumnIndex];
                    if ((bool)chk.Value)
                    {
                        DataRowView dr = (DataRowView)r.DataBoundItem;
                        lVisitIDs.Add((int)dr["VisitID"]);
                    }
                }

                List<HabitatModelDef> lModels = new List<HabitatModelDef>();
                foreach (object obj in chkModels.CheckedItems)
                    lModels.Add((HabitatModelDef)obj);

                theBuilder.BuildBatch(lVisitIDs, lModels, ref nSuccess, ref nError);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(String.Format("Complete. {0} successful simulations added, and {1} simulations encountered errors.", nSuccess, nError), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}

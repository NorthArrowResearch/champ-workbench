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
            chkSpecies.Items.Add(new SpeciesListItem("Upper Columbia Chinook", "UC_Chin", 1),true);
            chkSpecies.Items.Add(new SpeciesListItem("Snake River Chinook", "SN_Chin", 2), true);
            chkSpecies.Items.Add(new SpeciesListItem("Lower Columbia Steelhead", "LC_Steel", 3), true);
            chkSpecies.Items.Add(new SpeciesListItem("Mid Columbia Steelhead", "MC_Steel", 4), true);
            chkSpecies.Items.Add(new SpeciesListItem("Upper Columbia Steelhead", "UC_Steel", 5), true);
            chkSpecies.Items.Add(new SpeciesListItem("Snake River Steelhead", "SN_Steel", 6), true);

            LoadAllVisits();

            if (!string.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
                txtMonitoringFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
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
            OleDbCommand dbCom = new OleDbCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) AND (Folder Is Not Null) ORDER BY WatershedName", m_dbCon);
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
                chkVisitTypes.Items.Add(l, string.Compare(l.ToString(), "Annual", true) == 0);
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
            table.Columns.Add(new DataColumn("SurveyGDB", typeof(string)));
            table.Columns.Add(new DataColumn("VisitFolder", typeof(string)));
            table.Columns.Add(new DataColumn("HydraulicModelCSV", typeof(string)));
            table.Columns.Add(new DataColumn("SiteName", typeof(string)));
            table.Columns.Add(new DataColumn("SiteFolder", typeof(string)));
            table.Columns.Add(new DataColumn("WatershedID", typeof(int)));     
            table.Columns.Add(new DataColumn("WatershedName", typeof(string)));
            table.Columns.Add(new DataColumn("WatershedFolder", typeof(string)));
            table.Columns.Add(new DataColumn("TopoFolder", typeof(string)));

            string sSQL = "SELECT VisitID, VisitYear AS FieldSeason, IsPrimary, PanelName, SurveyGDB, CHAMP_Visits.Folder AS VisitFolder, HydraulicModelCSV, " +
             " CHAMP_Sites.SiteName, CHAMP_Sites.Folder AS SiteFolder, CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Watersheds.Folder AS WatershedFolder";

            // Add the species to the SQL query and also to the receiving database table
            foreach (SpeciesListItem sli in chkSpecies.Items)
            {
                table.Columns.Add(new DataColumn(sli.FieldName, typeof(bool)));
                sSQL += ", " + sli.FieldName;
            }

            sSQL += " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN CHAMP_Visits ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
                   " WHERE (((CHAMP_Visits.SurveyGDB) Is Not Null) AND ((CHAMP_Visits.Folder) Is Not Null) AND ((CHAMP_Visits.HydraulicModelCSV) Is Not Null) AND ((CHAMP_Sites.Folder) Is Not Null) AND ((CHAMP_Watersheds.Folder) Is Not Null))" +
                   " ORDER BY CHAMP_Visits.VisitYear, CHAMP_Watersheds.WatershedName";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();

            object[] rowArray = new object[table.Columns.Count];
            while (dbRead.Read())
            {
                rowArray[0] = false;
                rowArray[1] = (int)dbRead["VisitID"];
                rowArray[2] = (int)(Int16)dbRead["FieldSeason"];
                rowArray[3] = (bool)dbRead["IsPrimary"];
                rowArray[4] = (string)dbRead["PanelName"];
                rowArray[5] = (string)dbRead["SurveyGDB"];
                rowArray[6] = (string)dbRead["VisitFolder"];
                rowArray[7] = (string)dbRead["HydraulicModelCSV"];
                rowArray[8] = (string)dbRead["SiteName"];
                rowArray[9] = (string)dbRead["SiteFolder"];
                rowArray[10] = (int)dbRead["WatershedID"];
                rowArray[11] = (string)dbRead["WatershedName"];
                rowArray[12] = (string)dbRead["WatershedFolder"];
                rowArray[13] = System.IO.Path.Combine(((Int16)dbRead["FieldSeason"]).ToString(), (string)dbRead["WatershedFolder"], (string)dbRead["SiteFolder"], (string)dbRead["VisitFolder"]);

                // TODO: load the species presence absence attributes into the table

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

            for (int i = 0; i < chkSpecies.Items.Count; i++)
               // sFilter += string.Format(" AND {0} = {1}", ((SpeciesListItem)chkSpecies.Items[i]).FieldName, chkSpecies.GetItemChecked(i).ToString());

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
                dv.RowFilter = sFilter;
                //grdVisits.Refresh();
            }
        }

        private void AddCheckedListboxFilter(ref CheckedListBox lst, ref string sFilter, string sPropertyName, bool bUseNameInsteadOfValue = false)
        {
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
            frm.Filter = "Habitat Model Databases (*.accdb)|*.accdb";
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
                txtMonitoringFolder.Text = frm.SelectedPath;
        }

        private void txtHabitatModelDB_TextChanged(object sender, EventArgs e)
        {
            cboHabitatModel.Items.Clear();
            if (string.IsNullOrWhiteSpace(txtHabitatModelDB.Text) || !System.IO.File.Exists(txtHabitatModelDB.Text))
                return;

            string sConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtHabitatModelDB.Text;// CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + txtHabitatModelDB.Text);
            using (OleDbConnection dbCon = new OleDbConnection(sConString))
            {
                dbCon.Open();

                OleDbCommand dbCom = new OleDbCommand("SELECT HSIID, Title FROM HSI", dbCon);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    cboHabitatModel.Items.Add(new ListItem((string)dbRead["Title"], (int)dbRead["HSIID"]));
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
                HabitatBatchBuilder theBuilder = new HabitatBatchBuilder(ref m_dbCon, txtHabitatModelDB.Text, txtMonitoringFolder.Text);
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

                theBuilder.BuildBatch(lVisitIDs, ((ListItem)cboHabitatModel.SelectedItem).Value, ref nSuccess, ref nError);

                MessageBox.Show(String.Format("Complete. {0} successful simulations added, and {1} simulations encountered errors.", nSuccess, nError), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateForm()
        {


            return true;
        }
    }
}

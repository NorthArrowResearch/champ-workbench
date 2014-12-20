﻿using System;
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
            LoadAllSites(sender, e);

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

        private void LoadAllSites(object sender, EventArgs e)
        {

            OleDbCommand dbCom = new OleDbCommand("SELECT VisitID, VisitYear, IsPrimary, PanelName, SurveyGDB, CHAMP_Visits.Folder AS TopoFolder, HydraulicModelCSV, CHAMP_Sites.SiteID, CHAMP_Sites.SiteName, CHAMP_Sites.Folder AS SiteFolder, CHAMP_Watersheds.WatershedID, CHAMP_Watersheds.WatershedName, CHAMP_Watersheds.Folder AS WatershedFolder" +
                " FROM CHAMP_Watersheds INNER JOIN (CHAMP_Sites INNER JOIN CHAMP_Visits ON CHAMP_Sites.SiteID = CHAMP_Visits.SiteID) ON CHAMP_Watersheds.WatershedID = CHAMP_Sites.WatershedID" +
                " WHERE (((CHAMP_Visits.SurveyGDB) Is Not Null) AND ((CHAMP_Visits.Folder) Is Not Null) AND ((CHAMP_Visits.HydraulicModelCSV) Is Not Null) AND ((CHAMP_Sites.Folder) Is Not Null) AND ((CHAMP_Watersheds.Folder) Is Not Null))" +
                " ORDER BY CHAMP_Visits.VisitYear, CHAMP_Watersheds.WatershedName", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            List<ViewVisit> lVisits = new List<ViewVisit>();
            while (dbRead.Read())
            {
                ViewVisit v = new ViewVisit(
                    (int)dbRead["VisitID"],
                    (string)dbRead["TopoFolder"],
                    (string)dbRead["SurveyGDB"],
                    (string)dbRead["HydraulicModelCSV"],
                    (Int16)dbRead["VisitYear"],
                    (int)dbRead["WatershedID"],
                    (string)dbRead["WatershedFolder"],
                    (string)dbRead["WatershedName"],
                    (int)dbRead["SiteID"],
                    (string)dbRead["SiteFolder"],
                    (string)dbRead["SiteName"],
                    (string)dbRead["PanelName"],
                    (bool)dbRead["IsPrimary"]
              );

                lVisits.Add(v);
            }

            grdVisits.DataSource = null;
            bindingSourceSelectedVisits.DataSource = lVisits;
            grdVisits.DataSource = lVisits;
        }

        private class ViewVisit
        {
            private int m_nVisitID;
            private Int16 m_nFieldSeason;
            private string m_sPanel;
            private Boolean m_bIsPrimary;

            private string m_sWatershed;
            private int m_nWatershedID;
            private string m_sWatershedFolder;

            private int m_nSiteID;
            private string m_sSite;
            private string m_sSiteFolder;

            private bool m_bSelected;

            public int VisitID { get { return m_nVisitID; } }
            private string m_sTopoFolder;
            private string m_sSurveyGDB;
            private string m_sCSVFile;

            public Int16 FieldSeason { get { return m_nFieldSeason; } }
            public string TopoFolder { get { return System.IO.Path.Combine(m_nFieldSeason.ToString(), m_sWatershedFolder, m_sSiteFolder, m_sTopoFolder); } }
            public string SurveyGDB { get { return System.IO.Path.Combine(TopoFolder, m_sSurveyGDB); } }
            public string HydraulicCSV { get { return System.IO.Path.Combine(TopoFolder, m_sCSVFile); } }

            public string Watershed { get { return m_sWatershed; } }
            public int WatershedID { get { return m_nWatershedID; } }

            public string Site { get { return m_sSite; } }
            public int SiteID { get { return m_nSiteID; } }

            public bool Selected { get { return m_bSelected; } set { m_bSelected = value; } }
            public string Panel { get { return m_sPanel; } }
            public bool IsPrimary { get { return m_bIsPrimary; } }

            public ViewVisit(int nVisitID, string sVisitTopoFolder, string sSurveyGDB, string sCSVFile, Int16 nFieldSeason,
                int nWatershedID, string sWatershedFolder, string sWatershed,
                int nSiteID, string sSite, string sSiteFolder,
                string sPanel, Boolean bIsPrimary)
            {
                m_nVisitID = nVisitID;
                m_sTopoFolder = sVisitTopoFolder;
                m_sSurveyGDB = sSurveyGDB;
                m_sCSVFile = sCSVFile;
                m_nFieldSeason = nFieldSeason;

                m_nWatershedID = nWatershedID;
                m_sWatershed = sWatershed;
                m_sWatershedFolder = sWatershedFolder;

                m_nSiteID = nSiteID;
                m_sSite = sSite;
                m_sSiteFolder = sSiteFolder;

                m_sPanel = sPanel;
                m_bIsPrimary = bIsPrimary;
                m_bSelected = true;
            }
        }

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
    }
}

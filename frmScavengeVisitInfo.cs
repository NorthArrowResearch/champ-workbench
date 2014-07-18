using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace CHaMPWorkbench
{
    public partial class frmScavengeVisitInfo : Form
    {
        private OleDbConnection m_dbCon;

        public frmScavengeVisitInfo(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            OleDbCommand dbCom = new OleDbCommand("SELECT V.VisitID, W.Folder AS WFolder, S.Folder AS SFolder, V.Folder AS VFolder, V.HitchName, V.CrewName, V.VisitYear" +
                " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID", m_dbCon);

            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                String sSiteFolder = Path.Combine(txtMonitoringDataFolder.Text, dbRead["VisitYear"].ToString());
                sSiteFolder = Path.Combine(sSiteFolder, (String)dbRead["WFolder"]);
                sSiteFolder = Path.Combine(sSiteFolder, (String)dbRead["SFolder"]);

                if (System.IO.Directory.Exists(sSiteFolder))
                {
                    String sVisitTopo = "";

                    String[] sSubfolders = System.IO.Directory.GetDirectories(sSiteFolder);
                    if (sSubfolders.Count<String>() > 0)
                    {
                        if (sSubfolders.Count<String>() > 1)
                        {
                            // ToDo get the folder that matches the hitch
                        }
                        else
                            sVisitTopo = sSubfolders[0];
                    }

                    if (!String.IsNullOrWhiteSpace(sVisitTopo))
                    {
                        sSiteFolder = Path.Combine(sSiteFolder, sVisitTopo);
                        if (System.IO.Directory.Exists(sSiteFolder))
                        {
                            sSubfolders = System.IO.Directory.GetDirectories(sVisitTopo);
                            if (sSubfolders.Count<String>() == 1)
                            {
                                sVisitTopo = Path.Combine(sVisitTopo, sSubfolders[0]);

                                String sVisitFileGDB = "";
                                String[] sGDBs = System.IO.Directory.GetDirectories(sVisitTopo, "*orthog*.gdb");
                                if (sGDBs.Count<String>() > 0)
                                {
                                    if (sGDBs.Count<String>() == 1)
                                        sVisitFileGDB = System.IO.Path.Combine(sVisitTopo, sGDBs[0]);
                                    else
                                    {
                                        // multiple orthog GDBs
                                    }
                                }
                                else
                                {
                                    String[] sAnyGDBs = System.IO.Directory.GetDirectories(sVisitTopo, "*.gdb");
                                    if (sAnyGDBs.Count<String>() > 0)
                                    {
                                        if (sAnyGDBs.Count<String>() == 1)
                                            sVisitFileGDB = System.IO.Path.Combine(sVisitTopo, sAnyGDBs[0]);
                                        else
                                        {
                                            // multiple orthog GDBs
                                            System.Diagnostics.Debug.Print("Warning: Skipping multiple GDBs in " + sVisitTopo);
                                        }
                                    }
                                }
                                System.Diagnostics.Debug.Print(sVisitFileGDB);

                                // Topo TIN
                                String sTopoTIN = "";

                                String[] sTopoDirs = System.IO.Directory.GetDirectories(sVisitTopo, "tin*");
                                if (sTopoDirs.Count<String>() > 0)
                                    sTopoTIN = sTopoDirs[0];

                                // WS TIN
                                String sWSTIN = "";
                                String[] sWSDirs = System.IO.Directory.GetDirectories(sVisitTopo, "ws*");
                                if (sWSDirs.Count<String>() > 0)
                                    sWSTIN = sWSDirs[0];


                                if (!String.IsNullOrWhiteSpace(sVisitFileGDB) && !String.IsNullOrWhiteSpace(sTopoTIN) && !String.IsNullOrWhiteSpace(sWSTIN))
                                {
                                    OleDbCommand dbUpdate = new OleDbCommand("UPDATE CHaMP_Visits SET Folder = @Folder, TopoTIN = @TopoTIN, WSTIN = @WSTIN, SurveyGDB = @SurveyGDB WHERE VisitID = " + ((int)dbRead["VisitID"]).ToString(), m_dbCon);
                                    dbUpdate.Parameters.AddWithValue("Folder", sVisitTopo);
                                    dbUpdate.Parameters.AddWithValue("TopoTIN", System.IO.Path.GetFileName(sTopoTIN));
                                    dbUpdate.Parameters.AddWithValue("WSTIN", System.IO.Path.GetFileName(sWSTIN));
                                    dbUpdate.Parameters.AddWithValue("SurveyGDB", System.IO.Path.GetFileName(sVisitFileGDB));
                                    dbUpdate.ExecuteNonQuery();
                                }
                                else
                                    System.Diagnostics.Debug.Print("Warning: Not all folders found for " + sVisitTopo);

                            }
                        }
                    }
                }
            }

            CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder = txtMonitoringDataFolder.Text;
            CHaMPWorkbench.Properties.Settings.Default.Save();

            MessageBox.Show("Process completed successfully.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmScavengeVisitInfo_Load(object sender, EventArgs e)
        {
            txtMonitoringDataFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder;
        }
    }
}

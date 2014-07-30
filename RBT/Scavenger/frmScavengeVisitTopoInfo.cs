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
    public partial class frmScavengeVisitTopoInfo : Form
    {
        private OleDbConnection m_dbCon;

        public frmScavengeVisitTopoInfo(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_dbCon.State == ConnectionState.Closed)
                m_dbCon.Open();

            ScavengeVisitTopoInfo(m_dbCon, txtMonitoringDataFolder.Text);

     

            //OleDbCommand dbCom = new OleDbCommand("SELECT V.VisitID, S.SiteName, W.WatershedName, W.Folder AS WFolder, S.Folder AS SFolder, V.Folder AS VFolder, V.HitchName, V.CrewName, V.VisitYear" +
            //    " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID", m_dbCon);

            //OleDbDataReader dbRead = dbCom.ExecuteReader();
            //while (dbRead.Read())
            //{
            //    String sSiteFolder = Path.Combine(txtMonitoringDataFolder.Text, dbRead["VisitYear"].ToString());
            //    if (!System.IO.Directory.Exists(sSiteFolder))
            //        throw new Exception("The field season folder does not exist: " + sSiteFolder);

            //    if (System.Convert.IsDBNull(dbRead["WFolder"]))
            //        throw new Exception("The " + dbRead["VisitYear"] + " watershed folder does not exist: " + dbRead["WatershedName"]);

            //    sSiteFolder = Path.Combine(sSiteFolder, (String)dbRead["WFolder"]);

            //    if (System.Convert.IsDBNull(dbRead["SFolder"]))
            //    {
            //        string sSiteName = (String)dbRead["SiteName"];
            //        //sSiteFolder = GetSiteFolder(sSiteFolder, sSiteName);
            //        continue;
            //    }
            //    else
            //        sSiteFolder = System.IO.Path.Combine(sSiteFolder, (string)dbRead["SFolder"]);


            //    if (System.IO.Directory.Exists(sSiteFolder))
            //    {
            //        String sVisitTopo = "";

            //        String[] sSubfolders = System.IO.Directory.GetDirectories(sSiteFolder);
            //        if (sSubfolders.Count<String>() > 0)
            //        {
            //            if (sSubfolders.Count<String>() > 1)
            //            {
            //                // ToDo get the folder that matches the hitch
            //            }
            //            else
            //                sVisitTopo = sSubfolders[0];
            //        }

            //        if (!String.IsNullOrWhiteSpace(sVisitTopo))
            //        {
            //            String sHitchFolder = Path.Combine(sSiteFolder, sVisitTopo);
            //            if (System.IO.Directory.Exists(sHitchFolder))
            //            {
            //                sSubfolders = System.IO.Directory.GetDirectories(sHitchFolder);
            //                if (sSubfolders.Count<String>() == 1)
            //                {
            //                    String sRelativeVisitTopoFolder = sSubfolders[0].Remove(0,sSiteFolder.Length+1);
            //                    sVisitTopo = Path.Combine(sVisitTopo, sSubfolders[0]);

            //                    String sVisitFileGDB = "";
            //                    String[] sGDBs = System.IO.Directory.GetDirectories(sVisitTopo, "*orthog*.gdb");
            //                    if (sGDBs.Count<String>() > 0)
            //                    {
            //                        if (sGDBs.Count<String>() == 1)
            //                            sVisitFileGDB = System.IO.Path.Combine(sVisitTopo, sGDBs[0]);
            //                        else
            //                        {
            //                            // multiple orthog GDBs
            //                        }
            //                    }
            //                    else
            //                    {
            //                        String[] sAnyGDBs = System.IO.Directory.GetDirectories(sVisitTopo, "*.gdb");
            //                        if (sAnyGDBs.Count<String>() > 0)
            //                        {
            //                            if (sAnyGDBs.Count<String>() == 1)
            //                                sVisitFileGDB = System.IO.Path.Combine(sVisitTopo, sAnyGDBs[0]);
            //                            else
            //                            {
            //                                // multiple orthog GDBs
            //                                System.Diagnostics.Debug.Print("Warning: Skipping multiple GDBs in " + sVisitTopo);
            //                            }
            //                        }
            //                    }
            //                    System.Diagnostics.Debug.Print(sVisitFileGDB);

            //                    // Topo TIN
            //                    String sTopoTIN = "";

            //                    String[] sTopoDirs = System.IO.Directory.GetDirectories(sVisitTopo, "tin*");
            //                    if (sTopoDirs.Count<String>() > 0)
            //                        sTopoTIN = sTopoDirs[0];

            //                    // WS TIN
            //                    String sWSTIN = "";
            //                    String[] sWSDirs = System.IO.Directory.GetDirectories(sVisitTopo, "ws*");
            //                    if (sWSDirs.Count<String>() > 0)
            //                        sWSTIN = sWSDirs[0];


            //                    if (!String.IsNullOrWhiteSpace(sVisitFileGDB) && !String.IsNullOrWhiteSpace(sTopoTIN) && !String.IsNullOrWhiteSpace(sWSTIN))
            //                    {
            //                        OleDbCommand dbUpdate = new OleDbCommand("UPDATE CHaMP_Visits SET Folder = @Folder, TopoTIN = @TopoTIN, WSTIN = @WSTIN, SurveyGDB = @SurveyGDB WHERE VisitID = " + ((int)dbRead["VisitID"]).ToString(), m_dbCon);
            //                        dbUpdate.Parameters.AddWithValue("Folder", sRelativeVisitTopoFolder);
            //                        dbUpdate.Parameters.AddWithValue("TopoTIN", System.IO.Path.GetFileName(sTopoTIN));
            //                        dbUpdate.Parameters.AddWithValue("WSTIN", System.IO.Path.GetFileName(sWSTIN));
            //                        dbUpdate.Parameters.AddWithValue("SurveyGDB", System.IO.Path.GetFileName(sVisitFileGDB));
            //                        dbUpdate.ExecuteNonQuery();
            //                    }
            //                    else
            //                        System.Diagnostics.Debug.Print("Warning: Not all folders found for " + sVisitTopo);

            //                }
            //            }
            //        }
            //    }
            //}

            CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder = txtMonitoringDataFolder.Text;
            CHaMPWorkbench.Properties.Settings.Default.Save();

            MessageBox.Show("Process completed successfully.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmScavengeVisitInfo_Load(object sender, EventArgs e)
        {
            txtMonitoringDataFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder;
        }

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select the top level data containing the monitoring topo data. i.e. it should contain a subfolder for each field season.";


            if (!String.IsNullOrWhiteSpace(txtMonitoringDataFolder.Text) && System.IO.Directory.Exists(txtMonitoringDataFolder.Text))
                dlg.SelectedPath = txtMonitoringDataFolder.Text;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtMonitoringDataFolder.Text = dlg.SelectedPath;
        }

        private Boolean LookForFolder(string sParentFolder, string sChaMPItemName, out string sCHaMPItemFolder)
        {
            sCHaMPItemFolder = System.IO.Path.Combine(sParentFolder, sChaMPItemName);
            if (System.IO.Directory.Exists(sCHaMPItemFolder))
                return true;

            sCHaMPItemFolder = System.IO.Path.Combine(sParentFolder, sChaMPItemName.Replace(" ", ""));
            if (System.IO.Directory.Exists(sCHaMPItemFolder))
                return true;
           
            sCHaMPItemFolder = System.IO.Path.Combine(sParentFolder, "Visit");
            if (System.IO.Directory.Exists(sCHaMPItemFolder))
                return true;



            return false;
        }

        private void ScavengeVisitTopoInfo(OleDbConnection dbCon, string sMonitoringDataFolder)
        {
            if (dbCon.State == ConnectionState.Closed)
                dbCon.Open();

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

            foreach (RBTWorkbenchDataSet.CHAMP_WatershedsRow rWatershed in ds.CHAMP_Watersheds)
            {
                foreach(RBTWorkbenchDataSet.CHAMP_SitesRow rSite in rWatershed.GetCHAMP_SitesRows())
                {
                    foreach (RBTWorkbenchDataSet.CHAMP_VisitsRow rVisit in rSite.GetCHAMP_VisitsRows())
                    {
                        string sVisitYearFolder =  System.IO.Path.Combine(sMonitoringDataFolder, rVisit.VisitYear.ToString());
                       System.IO.DirectoryInfo dVisitYear = System.IO.Directory.CreateDirectory(sVisitYearFolder);

                        string sWatershedFolder;
                        if (LookForFolder(sVisitYearFolder, rWatershed.WatershedName, out sWatershedFolder))
                        {
                            rWatershed.Folder = System.IO.Path.GetFileNameWithoutExtension(sWatershedFolder);

                            string sSiteFolder;
                            if (LookForFolder(sWatershedFolder, rSite.SiteName, out sSiteFolder))
                            {
                                rSite.Folder = System.IO.Path.GetFileNameWithoutExtension(sSiteFolder);

                                bool bFoundTopoData = false;
                                string sHitchFolder;
                                if (LookForFolder(sSiteFolder, rVisit.HitchName, out sHitchFolder))
                                {
                                    string sTopoFolder;
                                    if (LookForFolder(sHitchFolder, "Topo", out sTopoFolder))
                                    {
                                        rVisit.Folder = System.IO.Path.Combine(System.IO.Path.GetFileNameWithoutExtension(sHitchFolder), System.IO.Path.GetFileNameWithoutExtension(sTopoFolder));

                                        string sSurveyGDB, sTopoTIN, sWSTIN;
                                        if (LookForTopoFiles(sTopoFolder, out sSurveyGDB, out sTopoTIN, out sWSTIN))
                                        {
                                            bFoundTopoData = true;
                                            rVisit.SurveyGDB = System.IO.Path.GetFileName(sSurveyGDB);
                                            rVisit.TopoTIN = System.IO.Path.GetFileName(sTopoTIN);
                                            rVisit.WSTIN = System.IO.Path.GetFileName(sWSTIN);
                                        }
                                    }
                                }

                                if (!bFoundTopoData)
                                {
                                    rVisit.SetFolderNull();
                                    rVisit.SetTopoTINNull();
                                    rVisit.SetWSTINNull();
                                    rVisit.SetSurveyGDBNull();
                                }
                            }
                            else
                                rSite.SetFolderNull();
                        }
                        else
                            rWatershed.SetFolderNull();

                    }
                }
            }

            daWatersheds.Update(ds.CHAMP_Watersheds);
            daSites.Update(ds.CHAMP_Sites);
            daVisits.Update(ds.CHAMP_Visits);
        }

        private bool LookForTopoFiles(string sVisitTopoFolder, out string sSurveyGDB, out string sTopoTIN, out string sWSTIN)
        {
            sSurveyGDB = "";
            String[] sGDBs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "*orthog*.gdb");
           if (sGDBs.Count<String>() > 0)
           {
               if (sGDBs.Count<String>() == 1)
                   sSurveyGDB = System.IO.Path.Combine(sVisitTopoFolder, sGDBs[0]);
               else
               {
                   // multiple orthog GDBs
               }
           }
           else
           {
               String[] sAnyGDBs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "*.gdb");
               if (sAnyGDBs.Count<String>() > 0)
               {
                   if (sAnyGDBs.Count<String>() == 1)
                       sSurveyGDB = System.IO.Path.Combine(sVisitTopoFolder, sAnyGDBs[0]);
                   else
                   {
                       // multiple orthog GDBs
                       System.Diagnostics.Debug.Print("Warning: Skipping multiple GDBs in " + sVisitTopoFolder);
                   }
               }
           }
           System.Diagnostics.Debug.Print(sSurveyGDB);

           // Topo TIN
           sTopoTIN = "";
           String[] sTopoDirs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "tin*");
           if (sTopoDirs.Count<String>() > 0)
               sTopoTIN = sTopoDirs[0];

           // WS TIN
           sWSTIN = "";
           String[] sWSDirs = System.IO.Directory.GetDirectories(sVisitTopoFolder, "ws*");
           if (sWSDirs.Count<String>() > 0)
               sWSTIN = sWSDirs[0];

           return (!String.IsNullOrWhiteSpace(sSurveyGDB) && !String.IsNullOrWhiteSpace(sTopoTIN) && !String.IsNullOrWhiteSpace(sWSTIN));
        }
    }
}

﻿using System;
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

            ScavengeVisitTopoInfo(m_dbCon, txtMonitoringDataFolder.Text, chkSetNull.Checked);

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

            sCHaMPItemFolder = System.IO.Path.Combine(sParentFolder,sChaMPItemName.Replace(":",""));
                  if (System.IO.Directory.Exists(sCHaMPItemFolder))
                return true;
            
            return false;
        }

        private Boolean LookForHitchFolder(string sSiteFolder, string sHitchName, string sCrewName, out string sHitchFolder)
        {
            sHitchFolder = "";
            DirectoryInfo dSiteFolder = new DirectoryInfo(sSiteFolder);
            DirectoryInfo[] dHitchFolders = dSiteFolder.GetDirectories("*");
            if (dHitchFolders.Count() == 1)
            {
                sHitchFolder = dHitchFolders[0].FullName;
                return true;
            }
            else if (dHitchFolders.Count() > 1)
            {
                foreach (DirectoryInfo aDir in dHitchFolders)
                {
                    string sDirStripped = StripString(aDir.Name);
                    string sCrewNameStripped = StripString(sCrewName);
                    string sHitchNameStripped = StripString(sHitchName);

                    if (string.Compare(sDirStripped, "Visit", true) == 0)
                    {
                        if (string.IsNullOrEmpty(sCrewName) || sCrewNameStripped.Contains("local"))
                        {
                            System.Diagnostics.Debug.WriteLine("Crew: " + sCrewName + " matched to " + aDir.FullName);
                            sHitchFolder = aDir.FullName;
                            return true;
                        }
                    }
                    else
                    {
                        if (sDirStripped.Contains(sCrewNameStripped) || sDirStripped.Contains(sHitchNameStripped))
                        {
                            System.Diagnostics.Debug.WriteLine("Crew: " + sCrewName + " matched to " + aDir.FullName);
                            sHitchFolder = aDir.FullName;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private string StripString(string sInput)
        {
            string sResult = sInput.Replace("_", "").Replace("-","").Replace(" ","").Replace(":","").Replace("#","").ToLower();
            return sResult;
        }

        private void ScavengeVisitTopoInfo(OleDbConnection dbCon, string sMonitoringDataFolder, bool bSetMissingDataNULL)
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
                        if (LookForFolder(sVisitYearFolder, rWatershed.WatershedName.Trim(), out sWatershedFolder))
                        {
                            rWatershed.Folder = System.IO.Path.GetFileNameWithoutExtension(sWatershedFolder);

                            string sSiteFolder;
                            if (LookForFolder(sWatershedFolder, rSite.SiteName, out sSiteFolder))
                            {
                                rSite.Folder = System.IO.Path.GetFileNameWithoutExtension(sSiteFolder);


                                string sCrewName = "";
                                if (!rVisit.IsCrewNameNull())
                                    sCrewName = rVisit.CrewName;

                                string sHitchName = "";
                                if (!rVisit.IsHitchNameNull())
                                    sHitchName = rVisit.HitchName;

                                bool bFoundTopoData = false;
                                string sHitchFolder;
                                if (LookForHitchFolder(sSiteFolder, sHitchName, sCrewName, out sHitchFolder))
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
                                else
                                {
                                    int i = 4;
                                }

                                if (!bFoundTopoData)
                                {
                                    if (bSetMissingDataNULL)
                                    {
                                        rVisit.SetFolderNull();
                                        rVisit.SetTopoTINNull();
                                        rVisit.SetWSTINNull();
                                        rVisit.SetSurveyGDBNull();
                                    }
                                }
                            }
                            else
                            {
                                if (bSetMissingDataNULL)
                                    rSite.SetFolderNull();
                            }
                        }
                        else
                        {
                            if (bSetMissingDataNULL)
                                rWatershed.SetFolderNull();
                        }
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench
{
    public partial class frmRBTRun : Form
    {
        private System.Data.OleDb.OleDbConnection m_dbCon;

        public frmRBTRun(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void frmRBTRun_Load(object sender, EventArgs e)
        {
            // Load the watersheds
            using (OleDbCommand dbCom = new OleDbCommand("SELECT WatershedID, WatershedName, FolderName FROM CHaMP_Watersheds ORDER BY WatershedName", m_dbCon))
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    cboWatershed.Items.Add(new Classes.Watershed((int)dbRead["WatershedID"], (String)dbRead["WatershedName"], (String)dbRead["FolderName"]));
                }

                if (cboWatershed.Items.Count > 0)
                    cboWatershed.SelectedIndex = 0;
            }

            // Folders
            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder))
                txtSourceFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastSourceFolder;

            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder))
                txtOutputFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder;

            ucConfig.ManualInitialization();

        }

        private void cboWatershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSite.Items.Clear();

            using (OleDbCommand dbCom = new OleDbCommand("SELECT SiteID, SiteName, UTMZone, FolderName FROM CHaMP_Sites WHERE WatershedID = " + ((Classes.Watershed)cboWatershed.SelectedItem).ID.ToString() + " ORDER BY SiteName", m_dbCon))
            {
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    Classes.Watershed aWatershed = (Classes.Watershed) cboWatershed.SelectedItem;
                    String sUTMZone ="";
                    if (!System.Convert.IsDBNull(dbRead["UTMZone"]))
                        sUTMZone = (String) dbRead["UTMZone"];

                    cboSite.Items.Add(new Classes.Site((int) dbRead["SiteID"], (String) dbRead["SiteName"], (String) dbRead["FolderName"], sUTMZone, ref aWatershed));
                }

                if (cboSite.Items.Count > 0)
                    cboSite.SelectedIndex = 0;
  
            }
        }

        private void cboSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboVisit.Items.Clear();

            String sSQL = "SELECT VisitID, Folder, HitchName, CrewName, SampleDate, IsPrimary, SurveyGDB, TopoTIN, WSTIN, VisitYear FROM CHaMP_Visits WHERE (Folder IS NOT NULL) AND SiteID = " + ((Classes.Site)cboSite.SelectedItem).ID.ToString();

            //if (cboFieldSeason.SelectedIndex>=0)
              //  sSQL += " AND VisitYear = " + cboFieldSeason.Text;

            sSQL += " ORDER BY VisitYear DESC, IsPrimary, HitchName, CrewName";

            using (OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon))
            {
                int nSelectIndex = 0;
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    String sVisit = "";
                    bool bPrimary = false;

                    if (!System.Convert.IsDBNull(dbRead["HitchName"]))
                        sVisit = (String) dbRead["HitchName"];

                    if (!System.Convert.IsDBNull(dbRead["CrewName"]))
                    {
                        if (!String.IsNullOrWhiteSpace(sVisit))
                            sVisit += ", ";

                        sVisit += dbRead["CrewName"];                        
                    }

                    if (String.IsNullOrWhiteSpace(sVisit))
                        sVisit = "Unknown Visit";

                    if (!System.Convert.IsDBNull(dbRead["IsPrimary"]))
                    {
                        if (!String.IsNullOrWhiteSpace(sVisit))
                            sVisit += ", ";

                        if ((bool)dbRead["IsPrimary"])
                        {
                            sVisit += "Primary";
                            bPrimary = true;
                        }
                    }

                    // Only add visit if the file GDB and TINs are defined in the DB
                    if (!System.Convert.IsDBNull(dbRead["SurveyGDB"]) && !System.Convert.IsDBNull(dbRead["TopoTIN"]) && !System.Convert.IsDBNull(dbRead["WSTIN"]))
                    {
                        String sSiteFolder = System.IO.Path.Combine(txtSourceFolder.Text, ((int) dbRead["VisitYear"]).ToString());
                        sSiteFolder  = System.IO.Path.Combine(sSiteFolder , ((Classes.Watershed)cboWatershed.SelectedItem).Folder);
                        sSiteFolder  = System.IO.Path.Combine(sSiteFolder , ((Classes.Site)cboSite.SelectedItem).Folder);
                        String sVisitFolder = System.IO.Path.Combine(sSiteFolder , (String)dbRead["Folder"]);

                        String sRelativevisitFolder = sVisitFolder.Substring(sSiteFolder.Length+1, sVisitFolder.Length - sSiteFolder.Length-1);
  
                        if (System.IO.Directory.Exists(sVisitFolder))
                        {
                            int nIndex = cboVisit.Items.Add(new Classes.Visit((int)dbRead["VisitID"], sRelativevisitFolder,  (String)dbRead["HitchName"], (String)dbRead["CrewName"], (int)dbRead["VisitYear"], (String)dbRead["SurveyGDB"], (String)dbRead["TopoTIN"], (String)dbRead["WSTIN"], (bool) dbRead["IsPrimary"], m_dbCon));
                            if (bPrimary)
                                nSelectIndex = nIndex;
                        }
                    }
                }

                if (cboVisit.Items.Count > 0)
                    cboVisit.SelectedIndex = nSelectIndex;
              }
        }

        private void cmdBrowseInputFile_Click(object sender, EventArgs e)
        {

        }

        private void cboVisit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtOutputFolder.Text))
            {
                String sInputFile = System.IO.Path.Combine(txtOutputFolder.Text,  ((Classes.Visit)cboVisit.SelectedItem).FieldSeason.ToString());
                sInputFile = System.IO.Path.Combine(sInputFile, ((Classes.Watershed)cboWatershed.SelectedItem).Folder);
                sInputFile = System.IO.Path.Combine(sInputFile, ((Classes.Site)cboSite.SelectedItem).Folder);
                sInputFile = System.IO.Path.Combine(sInputFile, ((Classes.Visit)cboVisit.SelectedItem).Folder);
                txtInputFile.Text = System.IO.Path.Combine(sInputFile, "input.xml");
            }
        }

        private String SourceDataFolder()
        {
            String sFolder =txtSourceFolder.Text;
            if (cboVisit.SelectedItem is Classes.Visit)
            {
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Visit)cboVisit.SelectedItem).FieldSeason.ToString());
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Watershed)cboWatershed.SelectedItem).Folder);
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Site)cboSite.SelectedItem).Folder);
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Visit)cboVisit.SelectedItem).Folder);
            }

            return sFolder;
        }

        private String OutputDataFolder()
        {
            String sFolder = txtOutputFolder.Text;
            if (cboVisit.SelectedItem is Classes.Visit)
            {
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Visit)cboVisit.SelectedItem).FieldSeason.ToString());
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Watershed)cboWatershed.SelectedItem).Folder);
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Site)cboSite.SelectedItem).Folder);
                sFolder = System.IO.Path.Combine(sFolder, ((Classes.Visit)cboVisit.SelectedItem).Folder);
            }
            return sFolder;
        }

        private void CreateFile()
        {

            if (!ValidateForm())
                return;

            Classes.Site aSite = (Classes.Site) cboSite.SelectedItem;

            Classes.Visit theMainVisit = (Classes.Visit)cboVisit.SelectedItem;
            theMainVisit.CalculateMetrics = chkCalculateMetrics.Checked;
            theMainVisit.ChangeDetection = chkChangeDetection.Checked;
            theMainVisit.MakeDEMsOrthogonal = chkOrthogonal.Checked;
            aSite.AddVisit(theMainVisit);

            if (!rdoSelectedOnly.Checked)
            {
                foreach(Classes.Visit aVisit in cboVisit.Items)
                {
                    if (rdoAll.Checked || aVisit.Primary)
                        if (aVisit.ID != ((Classes.Visit)cboVisit.SelectedItem).ID)
                            aSite.AddVisit(aVisit);
                }
            }

            // Ensure that the directory exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(txtInputFile.Text));

            using (System.Xml.XmlTextWriter xmlInput = new System.Xml.XmlTextWriter(txtInputFile.Text, System.Text.Encoding.UTF8))
            {
                xmlInput.Formatting = System.Xml.Formatting.Indented;
                xmlInput.WriteStartElement("rbt");

                xmlInput.WriteStartElement("metadata");
                xmlInput.WriteStartElement("created");
                xmlInput.WriteElementString("tool", System.Reflection.Assembly.GetExecutingAssembly().FullName);
                xmlInput.WriteElementString("date", DateTime.Now.ToString());
                xmlInput.WriteEndElement();
                // created
                xmlInput.WriteEndElement();
                // metadata

                aSite.WriteToXML(xmlInput, SourceDataFolder());

                Classes.Outputs anOutput = new Classes.Outputs();
                anOutput.OutputFolder =OutputDataFolder();
                System.IO.Directory.CreateDirectory(anOutput.OutputFolder);
                anOutput.TempFolder = ucConfig.txtTempFolder.Text;
                anOutput.ResultFile = ucConfig.txtResults.Text;
                anOutput.LogFile = ucConfig.txtLog.Text;
                anOutput.WriteToXML(xmlInput);

                Classes.Config aConfig = new Classes.Config();
                aConfig.Mode = ((ListItem)ucConfig.cboRBTMode.SelectedItem).Value;
                aConfig.ESRIProduct = ((ListItem)ucConfig.cboESRIProduct.SelectedItem).Value;
                aConfig.ArcGISLicense = ((ListItem)ucConfig.cboLicense.SelectedItem).Value;
                aConfig.PrecisionFormatString = ucConfig.txtPrecisionFormatString.Text;
                aConfig.ChartHeight = Convert.ToInt32(ucConfig.valChartHeight.Value);
                aConfig.ChartWidth = Convert.ToInt32(ucConfig.valChartWidth.Value);
                aConfig.ClearTempWorkspaceAfter = ucConfig.chkClearTempWorkspace.Checked;
                aConfig.RequireOrthogDEMs = ucConfig.chkRequireOrthogonalRasters.Checked;
                aConfig.PreserveArtifcats = ucConfig.chkPreserveArtifacts.Checked;
                aConfig.CreateZip = ucConfig.chkZipChangeDetection.Checked;
                aConfig.CellSize = (double) ucConfig.valCellSize.Value;
                aConfig.Precision = Convert.ToInt32(ucConfig.valRasterPrecision.Value);
                aConfig.RasterBuffer = Convert.ToInt32(ucConfig.valRasterBuffer.Value);
                aConfig.CrossSectionSpacing = (double) ucConfig.valXSSpacing.Value;
                aConfig.MaxRiverWidth = Convert.ToInt32(ucConfig.valMaxRiverWidth.Value);
                aConfig.CrossSectionFiltering = Convert.ToInt32(ucConfig.valXSStdDevFiltering.Value);
                aConfig.MinBarArea = Convert.ToInt32(ucConfig.valMinBarArea.Value);
                aConfig.ThalwegPoolWeight = Convert.ToInt32(ucConfig.valThalwegPoolWeight.Value);
                aConfig.ThalwegSmoothWeight = Convert.ToInt32(ucConfig.valThalwegSmoothingTolerance.Value);
                aConfig.ErrorRasterKernal = Convert.ToInt32(ucConfig.valErrorKernel.Value);
                aConfig.BankAngleBuffer = Convert.ToInt32(ucConfig.valBankAngleBuffer.Value);
                aConfig.InitialCrossSectionLength = Convert.ToDouble(ucConfig.valInitialCrossSectionLength.Value);
                aConfig.WriteToXML(xmlInput);

                xmlInput.WriteEndElement();
                // rbt
                xmlInput.Close();
            }

        }

        private bool ValidateForm()
        {
            if (String.IsNullOrWhiteSpace(txtInputFile.Text))
            {
                MessageBox.Show("Please enter a file path for the RBT input file that will be created.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (System.IO.File.Exists(txtInputFile.Text))
                {
                    DialogResult r = MessageBox.Show("The RBT input file already exists. Do you want to overwrite it?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    switch (r)
                    {
                        case System.Windows.Forms.DialogResult.No:
                            return false;

                        case System.Windows.Forms.DialogResult.Cancel:
                            this.Close();
                            return false;
                    }
                }
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (cboVisit.SelectedItem is Classes.Visit)
                CreateFile();
        }
    }
}

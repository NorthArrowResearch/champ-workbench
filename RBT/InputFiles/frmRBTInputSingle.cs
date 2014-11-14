using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench
{
    public partial class frmRBTInputSingle : Form
    {
        private System.Data.OleDb.OleDbConnection m_dbCon;

        public frmRBTInputSingle(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void frmRBTRun_Load(object sender, EventArgs e)
        {
            this.cHAMP_WatershedsTableAdapter.Connection = m_dbCon;
            this.cHAMP_WatershedsTableAdapter.Fill(this.rBTWorkbenchDataSet.CHAMP_Watersheds);

            this.cHAMP_SitesTableAdapter.Connection = m_dbCon;
            this.cHAMP_SitesTableAdapter.Fill(this.rBTWorkbenchDataSet.CHAMP_Sites); //, ((RBTWorkbenchDataSet.CHAMP_WatershedsRow) ((DataRowView) cboWatershed.SelectedItem).Row).WatershedID);
            cHAMPSitesBindingSource.Filter = "WatershedID = " + cboWatershed.SelectedValue;

            this.cHAMP_VisitsTableAdapter.Connection = m_dbCon;
            this.cHAMP_VisitsTableAdapter.Fill(this.rBTWorkbenchDataSet.CHAMP_Visits);
            cHAMPVisitsBindingSource.Filter = "SiteID = " + cboSite.SelectedValue;

            RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSeg = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
            daSeg.Connection=m_dbCon;
            daSeg.Fill(this.rBTWorkbenchDataSet.CHaMP_Segments);

            RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daCU = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
            daCU.Connection = m_dbCon;
            daCU.Fill(this.rBTWorkbenchDataSet.CHAMP_ChannelUnits);

            // Folders
            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
                txtSourceFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;

            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder))
                txtOutputFolder.Text = CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder;

            ucConfig.ManualInitialization();
            UpdateInputfilePath();
            UpdateBatchControlStatus();
            txtBatchName.Text = DateTime.Now.ToShortDateString();
        }

        private void cmdBrowseInputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "RBT Input XML File";
            dlg.Filter = "RBT Input XML Files (*.xml)|*.xml;All Files (*.*);*.*";
            dlg.CheckPathExists = true;
            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.OverwritePrompt = true;

            if (!String.IsNullOrWhiteSpace(txtInputFile.Text) && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtInputFile.Text)))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(txtInputFile.Text);
                if (String.Compare(System.IO.Path.GetExtension(txtInputFile.Text), ".xml",true)==0)
                    dlg.FileName = System.IO.Path.GetFileName(txtInputFile.Text);
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtInputFile.Text = dlg.FileName;
        }

        private void cboVisit_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInputfilePath();
        }

        private void UpdateInputfilePath()
        {
            if (!String.IsNullOrWhiteSpace(txtOutputFolder.Text) && cboVisit.SelectedItem is DataRowView)
            {
                DataRowView r = (DataRowView)cboVisit.SelectedItem;
                RBTWorkbenchDataSet.CHAMP_VisitsRow v = (RBTWorkbenchDataSet.CHAMP_VisitsRow)r.Row;
                txtInputFile.Text = System.IO.Path.Combine(this.rBTWorkbenchDataSet.CHAMP_Visits.BuildVisitDataFolder(v, txtOutputFolder.Text), "input.xml");
            }
        }

        private bool ValidateForm()
        {
            if (String.IsNullOrWhiteSpace(txtInputFile.Text))
            {
                MessageBox.Show("Please enter a file path for the RBT input file that will be created.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (chkBatch.Checked)
                if (String.IsNullOrWhiteSpace(txtBatchName.Text))
                {
                    MessageBox.Show("You have chosen to create a batch for this input file and therefore you must provide a name for the batch.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            XmlTextWriter xmlInput;
            Classes.InputFileBuilder theBuilder = new Classes.InputFileBuilder(ucConfig.GetRBTConfig(), ucConfig.GetRBTOutputs(txtOutputFolder.Text));
            theBuilder.CreateFile(txtInputFile.Text, out xmlInput);
                   

            RBTWorkbenchDataSet.CHAMP_VisitsRow rMainvisit = (RBTWorkbenchDataSet.CHAMP_VisitsRow)  ((DataRowView)cboVisit.SelectedItem).Row;
            bool bCSVs = ucConfig.GetRBTConfig().Mode == Classes.Config.RBTModes.Hydraulic_Model_Preparation;
            Classes.Visit mainvisit = new Classes.Visit(rMainvisit, chkCalculateMetrics.Checked, chkChangeDetection.Checked, chkOrthogonal.Checked, bCSVs );
            Classes.Site theSite = new Classes.Site(rMainvisit.CHAMP_SitesRow);
            theSite.AddVisit(mainvisit);

            //mainvisit.WriteToXML(ref xmlInput, this.rBTWorkbenchDataSet.CHAMP_Visits.BuildVisitDataFolder(rMainvisit ,txtSourceFolder.Text));

            // other visits
            if (!rdoSelectedOnly.Checked)
            {
                foreach (object obj in cboVisit.Items)
                {
                    RBTWorkbenchDataSet.CHAMP_VisitsRow aVisit = (RBTWorkbenchDataSet.CHAMP_VisitsRow)((DataRowView)obj).Row;

                    if (aVisit.VisitID != mainvisit.ID)
                    {
                        if (rdoAll.Checked || aVisit.IsPrimary)
                        {
                            Classes.Visit anotherVisit = new Classes.Visit(aVisit, false, aVisit.IsPrimary, chkOrthogonal.Checked,false);
                            theSite.AddVisit(anotherVisit);
                        }
                    }
                }
            }

            xmlInput.WriteStartElement("sites");
            theSite.WriteToXML(xmlInput, txtSourceFolder.Text);
            xmlInput.WriteEndElement(); // sites

            theBuilder.Config.ChangeDetectionConfig.Threshold = ucRBTChangeDetection1.Threshold;
            theBuilder.Config.ChangeDetectionConfig.ClearMasks();
            foreach (Classes.BudgetSegregation aMask in ucRBTChangeDetection1.BudgetMasks.CheckedItems)
            {
                theBuilder.Config.ChangeDetectionConfig.AddMask(aMask.MaskName);
            }
            
            theBuilder.CloseFile(ref xmlInput, this.rBTWorkbenchDataSet.CHAMP_Visits.BuildVisitDataFolder(rMainvisit, txtOutputFolder.Text));

            // Create the Batch
            if (chkBatch.Checked)
            {
                OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
                try
                {
                    OleDbCommand dbCom = new OleDbCommand("INSERT INTO RBT_Batches (BatchName, Run) VALUES (?, 1)", m_dbCon, dbTrans);

                    dbCom.Parameters.AddWithValue("BatchName", txtBatchName.Text);
                    dbCom.ExecuteNonQuery();

                    dbCom = new OleDbCommand("SELECT @@IDENTITY", m_dbCon, dbTrans);
                    int nBatchID = (int) dbCom.ExecuteScalar();



                    dbCom = new OleDbCommand("INSERT INTO RBT_BatchRuns (BatchID, Summary, Run, InputFile, PrimaryVisitID) VALUES (?, ?, 1, ?, ?)", m_dbCon, dbTrans);
                    dbCom.Parameters.AddWithValue("BatchID", nBatchID);
                    dbCom.Parameters.AddWithValue("Summary", cboWatershed.Text + ", " + cboSite.Text + ", " + cboVisit.Text);
                    dbCom.Parameters.AddWithValue("InputFile", txtInputFile.Text);
                    dbCom.Parameters.AddWithValue("PrimaryVisitID", mainvisit.ID);
                    dbCom.ExecuteNonQuery();
                    
                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    MessageBox.Show("Failed to create batch.");
                }
            }


            if (chkOpenWhenComplete.Checked)
                if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.TextEditor) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.TextEditor))
                {
                    String sFile = "\"" + txtInputFile.Text + "\"";
                    System.Diagnostics.Process.Start(CHaMPWorkbench.Properties.Settings.Default.TextEditor, sFile);
                }
        }

        private void cboSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSite.SelectedValue != null)
                cHAMPVisitsBindingSource.Filter = "SiteID = " + cboSite.SelectedValue.ToString();

            UpdateInputfilePath();
            //this.cHAMP_VisitsTableAdapter.FillBySiteID(this.rBTWorkbenchDataSet.CHAMP_Visits, (int)cboSite.SelectedValue);
        }

        private void cboWatershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.rBTWorkbenchDataSet.CHAMP_Visits.Clear();
            //this.cHAMP_SitesTableAdapter.FillByWatershedID(this.rBTWorkbenchDataSet.CHAMP_Sites, (int)cboWatershed.SelectedValue);
            cHAMPSitesBindingSource.Filter = "WatershedID = " + cboWatershed.SelectedValue.ToString();
            cHAMPVisitsBindingSource.Filter = "SiteID = " + cboSite.SelectedValue;
            UpdateInputfilePath();
        }

        private void cmdBrowseSourceDataFolder_Click(object sender, EventArgs e)
        {
            dlgFolder.Description = "Select the top level, parent data folder that contains the CHaMP topo data files. This should contain the field season folders then watersheds etc.";
            if (!String.IsNullOrWhiteSpace(txtSourceFolder.Text) && System.IO.Directory.Exists(txtSourceFolder.Text))
                dlgFolder.SelectedPath = txtSourceFolder.Text;

            if (dlgFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtSourceFolder.Text = dlgFolder.SelectedPath;

        }

        private void cmdBrowseOutputDataFolder_Click(object sender, EventArgs e)
        {
            dlgFolder.Description = "Select the top level, parent data folder where the RBT output file(s) should be created.";
            if (!String.IsNullOrWhiteSpace(txtOutputFolder.Text) && System.IO.Directory.Exists(txtOutputFolder.Text))
                dlgFolder.SelectedPath = txtOutputFolder.Text;

            if (dlgFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtOutputFolder.Text = dlgFolder.SelectedPath;
        }

        private void chkBatch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBatchControlStatus();
        }

        private void UpdateBatchControlStatus()
        {
            txtBatchName.Enabled = chkBatch.Checked;
            lblBatchName.Enabled = chkBatch.Checked;
        }
    }
}

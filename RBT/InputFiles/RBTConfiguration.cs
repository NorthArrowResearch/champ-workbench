using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench
{
    public partial class RBTConfiguration : UserControl
    {
        public RBTConfiguration()
        {
            InitializeComponent();
        }

        public void ManualInitialization()
        {
            tTip.SetToolTip(txtTempFolder, "The RBT temporary workspace folder");
            tTip.SetToolTip(cmdBrowseTemp, "Sets the RBT temporary workspace folder to the system temp folder as specified by the TEMP environment variable.");
            /*
             * tTip.SetToolTip(cmdBrowseOutputFolder, "Opens a window to browse and set the RBT temporary workspace folder");
            tTip.SetToolTip(txtOutputFolder, "The output folder that will be used for the results, log and artifacts.");
            tTip.SetToolTip(cmdBrowseOutputFolder, "Opens a window to browse and set the output folder that will be used for results, log and artifacts folder.");
            */
            tTip.SetToolTip(txtResults, "The name of the RBT results XML file");
            tTip.SetToolTip(txtLog, "The name of the RBT log XML file");
            tTip.SetToolTip(chkOutputProfileValues, "Full listing of each cross section and longitudinal profiles will be written to the RBT result XML file when checked. When not checked, only summary statistics and metrics for each profile are written to the RBT result XML file.");

            /*
            string sDefault = CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder;
            if (!String.IsNullOrWhiteSpace(sDefault) && System.IO.Directory.Exists(sDefault))
                txtOutputFolder.Text = sDefault;
            */

            txtResults.Text = CHaMPWorkbench.Properties.Settings.Default.LastResultsFile;
            txtLog.Text = CHaMPWorkbench.Properties.Settings.Default.LastLogfile;


            foreach (Classes.ModelInputFiles.RBTConfig.RBTModes eMode in Enum.GetValues(typeof(Classes.ModelInputFiles.RBTConfig.RBTModes)))
                cboRBTMode.Items.Add(new naru.db.NamedObject((int)eMode, eMode.ToString().Replace("_"," ")));
            cboRBTMode.SelectedIndex = 1;

            cboESRIProduct.Items.Add(new naru.db.NamedObject(100, "Engine or Desktop"));
            cboESRIProduct.Items.Add(new naru.db.NamedObject(1, "ArcGIS Desktop"));
            cboESRIProduct.Items.Add(new naru.db.NamedObject(2, "Engine"));
            cboESRIProduct.Items.Add(new naru.db.NamedObject(3, "ArcGIS Reader"));
            cboESRIProduct.Items.Add(new naru.db.NamedObject(5, "ArcGIS Server"));
            cboESRIProduct.SelectedIndex = 0;

            cboLicense.Items.Add(new naru.db.NamedObject(40, "Basic"));
            cboLicense.Items.Add(new naru.db.NamedObject(50, "Standard"));
            cboLicense.Items.Add(new naru.db.NamedObject(60, "Advanced"));
            cboLicense.Items.Add(new naru.db.NamedObject(30, "Server"));
            cboLicense.Items.Add(new naru.db.NamedObject(10, "Engine"));
            cboLicense.Items.Add(new naru.db.NamedObject(20, "Engine Geodatabase"));
            cboLicense.SelectedIndex = 2;

            String sTemp = CHaMPWorkbench.Properties.Settings.Default.LastTempFolder;
            if (! String.IsNullOrWhiteSpace(sTemp) && System.IO.Directory.Exists(sTemp))
                txtTempFolder.Text = sTemp;
        }

        public bool ValidateForm(string sStreamName, string sUTMZone, string sWatershed)
        {
            // If Not cboRBTMode.SelectedItem Is Nothing Then
            if (((naru.db.NamedObject)cboRBTMode.SelectedItem).ID == 30)
            {
                if (string.IsNullOrEmpty(sStreamName))
                {
                    MessageBox.Show("The stream name cannot be blank when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(sUTMZone))
                {
                    MessageBox.Show("You must select a UTM zone when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(sWatershed))
                {
                    MessageBox.Show("You must select a watershed when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //End If


            if (string.IsNullOrEmpty(txtTempFolder.Text))
            {
                switch (MessageBox.Show("The temp workspace folder is empty. Do you want to use the system default?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        txtTempFolder.Text = System.Environment.GetEnvironmentVariable("TEMP");

                        break;
                    default:
                        return false;
                }
            }

            /*
            if (string.IsNullOrEmpty(txtOutputFolder.Text))
            {
                MessageBox.Show("The output folder cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOutputFolder.Focus();
                return false;
            }
            else
            {
                if (System.IO.Directory.Exists(txtOutputFolder.Text))
                    CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder = txtOutputFolder.Text;
                else
                {
                    MessageBox.Show("The output folder must exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOutputFolder.Focus();
                    return false;
                }
            }
             */

            if (string.IsNullOrEmpty(txtResults.Text))
            {
                MessageBox.Show("The results file cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResults.Text = "Results.xml";
                txtResults.Focus();
                return false;
            }
            else
            {
                if (txtResults.Text.EndsWith(".xml"))
                    CHaMPWorkbench.Properties.Settings.Default.LastResultsFile = txtResults.Text;
                else
                {
                    txtResults.Text = System.IO.Path.ChangeExtension(txtResults.Text, ".xml");
                }
            }

            if (string.IsNullOrEmpty(txtLog.Text))
            {
                MessageBox.Show("The log file cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLog.Text = "log.xml";
                txtLog.Focus();
                return false;
            }
            else
            {
                if (txtLog.Text.EndsWith(".xml"))
                    CHaMPWorkbench.Properties.Settings.Default.LastLogfile = txtLog.Text;
                else
                {
                    txtLog.Text = System.IO.Path.ChangeExtension(txtLog.Text, ".xml");
                }
            }

            if (string.IsNullOrEmpty(txtPrecisionFormatString.Text))
            {
                MessageBox.Show("The string precision format cannot be empty. Enter a pattern for formatting strings as numbers using pound signs, commas and zeroes.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrecisionFormatString.SelectAll();
                txtPrecisionFormatString.Focus();
                return false;
            }
            else
            {
                double fTest = 3.14;
                string sTest = null;
                try
                {
                    sTest = fTest.ToString(txtPrecisionFormatString.Text);
                    if (!Double.TryParse(sTest, out fTest))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The string precision format string does not appear to convert numbers to strings correctly.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPrecisionFormatString.SelectAll();
                    txtPrecisionFormatString.Focus();
                    return false;
                }
            }

            return true;
        }

        private void cmdBrowseOutputFolder_Click(object sender, EventArgs e)
        {

        }

        private void cmdAuto_Click(object sender, EventArgs e)
        {
            String sTemp = System.Environment.GetEnvironmentVariable("TEMP");
            if (!String.IsNullOrWhiteSpace(sTemp) && System.IO.Directory.Exists(sTemp))
                txtTempFolder.Text = sTemp;
        }

        private void cmdBrowseTemp_Click(object sender, EventArgs e)
        {
           FolderBrowserDialog dlg = new FolderBrowserDialog();
           dlg.Description = "Select the RBT Tempory Workspace Folder";

           if (!String.IsNullOrWhiteSpace(txtTempFolder.Text) && System.IO.Directory.Exists(txtTempFolder.Text))
               dlg.SelectedPath = txtTempFolder.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
                txtTempFolder.Text = dlg.SelectedPath;
        }

        public CHaMPWorkbench.Classes.ModelInputFiles.RBTConfig GetRBTConfig()
        {
            Classes.ModelInputFiles.RBTConfig aConfig = new Classes.ModelInputFiles.RBTConfig();
            aConfig.Mode = (Classes.ModelInputFiles.RBTConfig.RBTModes)((naru.db.NamedObject)cboRBTMode.SelectedItem).ID;
            aConfig.ESRIProduct = ((naru.db.NamedObject)cboESRIProduct.SelectedItem).ID;
            aConfig.ArcGISLicense = ((naru.db.NamedObject)cboLicense.SelectedItem).ID;
            aConfig.PrecisionFormatString = txtPrecisionFormatString.Text;
            aConfig.ChartHeight = Convert.ToInt32(valChartHeight.Value);
            aConfig.ChartWidth = Convert.ToInt32(valChartWidth.Value);
            aConfig.ClearTempWorkspaceAfter = chkClearTempWorkspace.Checked;
            aConfig.RequireOrthogDEMs = chkRequireOrthogonalRasters.Checked;
            aConfig.PreserveArtifcats = chkPreserveArtifacts.Checked;
            aConfig.CreateZip = chkZipChangeDetection.Checked;
            aConfig.CellSize = (double) valCellSize.Value;
            aConfig.Precision = Convert.ToInt32(valRasterPrecision.Value);
            aConfig.RasterBuffer = Convert.ToInt32(valRasterBuffer.Value);
            aConfig.CrossSectionSpacing = (double)valCrossSectionSpacing.Value;
            aConfig.CrossSectionStationSpacing = (double) valXSStationSpacing.Value;
            aConfig.MaxRiverWidth = Convert.ToInt32(valMaxRiverWidth.Value);
            aConfig.CrossSectionFiltering = Convert.ToInt32(valXSStdDevFiltering.Value);
            aConfig.MinBarArea = Convert.ToInt32(valMinBarArea.Value);
            aConfig.ThalwegPoolWeight = Convert.ToInt32(valThalwegPoolWeight.Value);
            aConfig.ThalwegSmoothWeight = Convert.ToInt32(valThalwegSmoothingTolerance.Value);
            aConfig.ErrorRasterKernal = Convert.ToInt32(valErrorKernel.Value);
            aConfig.BankAngleBuffer = Convert.ToInt32(valBankAngleBuffer.Value);
            aConfig.InitialCrossSectionLength = Convert.ToInt32(valInitialCrossSectionLength.Value);
            aConfig.OutputProfileValues = chkOutputProfileValues.Checked;
            return aConfig;
        }

        public Classes.ModelInputFiles.RBTOutputs GetRBTOutputs(string sRBTOutputFolder)
        {
            Classes.ModelInputFiles.RBTOutputs anOutput = new Classes.ModelInputFiles.RBTOutputs();
            anOutput.OutputFolder = sRBTOutputFolder;
            anOutput.TempFolder = txtTempFolder.Text;
            anOutput.ResultFile = txtResults.Text;
            anOutput.LogFile = txtLog.Text;
            return anOutput;
        }
    }
}

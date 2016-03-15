using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.GUT
{
    public partial class frmGUTInputXMLBuilder : Form
    {
        private string m_sDBCon;
        private List<int> m_lVisitIDs;

        public frmGUTInputXMLBuilder(string sDBCon, List<int> lVisitIDs)
        {
            InitializeComponent();
            m_sDBCon = sDBCon;
            m_lVisitIDs = lVisitIDs;
        }

        private void frmGUTInputXMLBuilder_Load(object sender, EventArgs e)
        {
            txtBatch.Text = "Batch " + DateTime.Now.ToString("yyy_MM_dd");
#if DEBUG
            txtBatch.Text = txtBatch.Text + "_debug";
            chkClearOtherBatches.Checked = true;
#endif

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
                txtMonitoringDataFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder))
                txtOutputFolder.Text = CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder;

            try
            {
                Classes.InputFileBuilder_Helper.RefreshVisitPaths(m_sDBCon, ref m_lVisitIDs, ref lstVisits);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }








        }

        private bool ValidateForm()
        {
            if (m_lVisitIDs.Count < 1)
            {
                MessageBox.Show("You must select at least one visit to proceed. Return to the main Workbench grid and select the visits for which you want to run the model.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdCancel.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtBatch.Text))
            {
                MessageBox.Show("You must provide a name for the batch.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBatch.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtMonitoringDataFolder.Text) || !System.IO.Directory.Exists(txtMonitoringDataFolder.Text))
            {
                MessageBox.Show("You must provide a valid folder that contains the CHaMP monitoring data.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowseMonitoring.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtOutputFolder.Text) || !System.IO.Directory.Exists(txtOutputFolder.Text))
            {
                MessageBox.Show("You must provide a valid output folder where the input files will get created.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowseInputOutput.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtInputFile.Text))
            {
                MessageBox.Show("You must provide a name for the GUT input XML files that will get created.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInputFile.Select();
                return false;
            }
            else
            {
                if (!txtInputFile.Text.ToLower().EndsWith(".xml"))
                {
                    MessageBox.Show("The GUT input XML files should end with '.xml'.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInputFile.Select();
                    return false;
                }
            }

            return true;
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {

        }

        private void cmdBrowseMonitoring_Click(object sender, EventArgs e)
        {
            Classes.InputFileBuilder_Helper.BrowseFolder("Monitoring Data Folder", "Select the top level folder that contains the monitoring data.", ref txtMonitoringDataFolder);
        }

        private void cmdBrowseInputOutput_Click(object sender, EventArgs e)
        {
            Classes.InputFileBuilder_Helper.BrowseFolder("Input/Output Data Folder", "Select the top level folder that will contain the input files.", ref txtOutputFolder);
        }
    }
}

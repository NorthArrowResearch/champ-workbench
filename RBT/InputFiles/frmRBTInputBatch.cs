using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.RBTInputFile
{
    public partial class frmRBTInputBatch : Form
    {
        private Dictionary<int, string> m_dVisits;

        public frmRBTInputBatch(Dictionary<int, string> dVisits)
        {
            m_dVisits = dVisits;
            InitializeComponent();
        }

        private void frmRBTInputBatch_Load(object sender, EventArgs e)
        {
            try
            {
                Classes.InputFileBuilder_Helper.RefreshVisitPaths(DBCon.ConnectionString, ref m_dVisits, ref lstVisits);

                ucConfig.ManualInitialization();

                txtBatch.Text = "Batch " + DateTime.Now.ToString("yyy_MM_dd");
                txtInputFileRoot.Text = Classes.ModelInputFiles.RBTBatchInputfileBuilder.m_sDefaultRBTInputXMLFileName;
#if DEBUG
                txtBatch.Text = txtBatch.Text + "_debug";
#endif
                string sDefault = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
                if (string.IsNullOrEmpty(sDefault) || !System.IO.Directory.Exists(sDefault))
                {
                    sDefault = string.Empty;
                }
                txtMonitoringDataFolder.Text = sDefault;

                string sDefaultIO = CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder;
                if (string.IsNullOrEmpty(sDefaultIO) || !System.IO.Directory.Exists(sDefaultIO))
                {
                    sDefault = string.Empty;
                }
                txtOutputFolder.Text = sDefaultIO;

                if (m_dVisits.Count == 1)
                {
                    txtBatch.Text = string.Format("Visit {0}, {1} mode", m_dVisits.Keys.First<int>(), ucConfig.cboRBTMode.Text);
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }



        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            TextBox txt = null;
            if (string.Compare(((Control)sender).Name, cmdBrowseFolder.Name, true) == 0)
            {
                frm.Description = "Select top level Monitoring folder that contains the topo data.";
                txt = txtMonitoringDataFolder;
            }
            else
            {
                frm.Description = "Select top level output folder where the input XML files will be generated.";
                txt = txtOutputFolder;
            }

            if (!string.IsNullOrEmpty(txt.Text))
            {
                if (System.IO.Directory.Exists(txt.Text))
                {
                    frm.SelectedPath = txt.Text;
                }
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Directory.Exists(frm.SelectedPath))
                {
                    txt.Text = frm.SelectedPath;
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMonitoringDataFolder.Text))
            {
                MessageBox.Show("You must select the root \"monitoring\" local cloud folder.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            else
            {
                if (!System.IO.Directory.Exists(txtMonitoringDataFolder.Text))
                {
                    MessageBox.Show("The root monitoring folder does not exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtOutputFolder.Text))
            {
                MessageBox.Show("You must select an output folder.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            else
            {
                if (!System.IO.Directory.Exists(txtOutputFolder.Text))
                {
                    MessageBox.Show("The output folder does not exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            string sMessage = string.Empty;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                Classes.ModelInputFiles.RBTConfig rbtConfig = ucConfig.GetRBTConfig();
                Classes.ModelInputFiles.RBTOutputs rbtOutputs = ucConfig.GetRBTOutputs(txtOutputFolder.Text);

                rbtConfig.ChangeDetectionConfig.Threshold = ucRBTChangeDetection1.Threshold;
                rbtConfig.ChangeDetectionConfig.ClearMasks();
                foreach (Classes.BudgetSegregation aMask in ucRBTChangeDetection1.BudgetMasks.CheckedItems)
                {
                    rbtConfig.ChangeDetectionConfig.AddMask(aMask.MaskName);
                }

                Classes.ModelInputFiles.RBTBatchInputfileBuilder theBatch = new Classes.ModelInputFiles.RBTBatchInputfileBuilder(DBCon.ConnectionString, txtBatch.Text, chkClearOtherBatches.Checked,
                              txtMonitoringDataFolder.Text, txtOutputFolder.Text, ref m_dVisits, txtInputFileRoot.Text, rbtConfig, rbtOutputs, true, chkChangeDetection.Checked, true, rdoAll.Checked, true, true, chkClearOtherBatches.Checked);

                int nSuccess = 0;
                List<string> lExceptionMessages;

                nSuccess = theBatch.Run(out lExceptionMessages);

                System.Windows.Forms.Cursor.Current = Cursors.Default;

                if (nSuccess == m_dVisits.Count)
                    MessageBox.Show(string.Format("All {0} RBT input XML files were created successfully and added to the model batch called '{1}'.", nSuccess, txtBatch.Text), "Process Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    int nErrors = 0;
                    if (lExceptionMessages is List<string>)
                        nErrors = lExceptionMessages.Count;

                    frmToolResults frm = new frmToolResults("RBT Input Files", string.Format("{0} RBT input XML files were created successfully. {1} experienced errors. Information about the errors are shown below.", nSuccess, nErrors), ref lExceptionMessages);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtMonitoringDataFolder_TextChanged(object sender, EventArgs e)
        {
            Classes.InputFileBuilder_Helper.RefreshVisitPaths(DBCon.ConnectionString, ref m_dVisits, ref lstVisits);
        }
    }
}

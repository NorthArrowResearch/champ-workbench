using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.RBTInputFile
{
    public partial class frmRBTInputBatch : Form
    {
        private OleDbConnection m_dbCon;

        public frmRBTInputBatch(OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
        }

        public frmRBTInputBatch()
        {
            InitializeComponent();
        }

        private void frmRBTInputBatch_Load(object sender, EventArgs e)
        {

            for (int i = 2011; i <= DateTime.Now.Year; i++)
            {
                int index = lstFieldSeasons.Items.Add(new ListItem(i.ToString(), i));
                lstFieldSeasons.SetItemChecked(index, true);
            }

            ucConfig.ManualInitialization();

            txtBatch.Text = "Batch " + DateTime.Now.ToString("yyy_MM_dd");
#if DEBUG
            txtBatch.Text = txtBatch.Text + "_debug";
#endif
            string sDefault = "C:\\ChaMP\\MonitoringData";
            if (!System.IO.Directory.Exists(sDefault))
            {
                sDefault = string.Empty;
            }
            txtMonitoringDataFolder.Text = sDefault;
        }

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select top level Monitoring folder in local cloud";
            if (!string.IsNullOrEmpty(txtMonitoringDataFolder.Text))
            {
                if (System.IO.Directory.Exists(txtMonitoringDataFolder.Text))
                {
                    frm.SelectedPath = txtMonitoringDataFolder.Text;
                }
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Directory.Exists(frm.SelectedPath))
                {
                    txtMonitoringDataFolder.Text = frm.SelectedPath;
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {

            if (lstFieldSeasons.CheckedIndices.Count < 1)
            {
                MessageBox.Show("You must select at least one field season for which you want to build input files", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

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
                    MessageBox.Show("The output folder does not exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            string sMessage = string.Empty;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                Classes.Config rbtConfig = ucConfig.GetRBTConfig();
                Classes.Outputs rbtOutputs = ucConfig.GetRBTOutputs(txtOutputFolder.Text);
                List<short> lFieldSeasons = new List<short>();
                foreach (int nChecked in lstFieldSeasons.CheckedIndices)
                {
                    lFieldSeasons.Add(Convert.ToInt16(lstFieldSeasons.Items[nChecked].ToString()));
                }

                Classes.BatchInputfileBuilder theBatch = new Classes.BatchInputfileBuilder(m_dbCon, lFieldSeasons, rbtConfig, rbtOutputs);
                sMessage = theBatch.Run(txtBatch.Text, txtInputFileRoot.Text, txtMonitoringDataFolder.Text, chkCalculateMetrics.Checked, chkChangeDetection.Checked, true, chkIncludeOtherVisits.Checked);
            }
            catch (Exception ex)
            {
                sMessage = "Error during processing: " + ex.Message;
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                MessageBox.Show(sMessage, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

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
            InitializeComponent();
        }

        private void frmRBTInputBatch_Load(object sender, EventArgs e)
        {
            lstFieldSeasons.CheckOnClick = true;

            for (int i = 2011; i <= DateTime.Now.Year; i++)
            {
                int index = lstFieldSeasons.Items.Add(new ListItem(i.ToString(), i));
                //lstFieldSeasons.SetItemChecked(index, true);
            }

            OleDbCommand dbCom = new OleDbCommand("SELECT DISTINCT VisitYear FROM CHAMP_Visits", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                if (dbRead[0] != DBNull.Value)
                {
                    Int16 nVisitYear = (Int16) dbRead[0];
                    bool bAlreadyAdded = false;
                    foreach (ListItem l in lstFieldSeasons.Items)
                    {
                        if (l.Value == nVisitYear)
                            bAlreadyAdded = true;
                    }

                    if (!bAlreadyAdded)
                        lstFieldSeasons.Items.Add(new ListItem(nVisitYear.ToString(), nVisitYear));
                }
            }
                       
            ucConfig.ManualInitialization();

            txtBatch.Text = "Batch " + DateTime.Now.ToString("yyy_MM_dd");
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
        }

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog frm = new FolderBrowserDialog();
            TextBox txt = null;
            if (string.Compare(((Control) sender).Name, cmdBrowseFolder.Name, true) == 0)
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
                {
                    MessageBox.Show("The output folder does not exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (chkChangeDetection.Checked && !chkIncludeOtherVisits.Checked)
            {
                DialogResult eResult = MessageBox.Show("You must choose to include other visits if you want to perform change detection. Do you want to invlude all other visits?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (eResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        chkIncludeOtherVisits.Checked = true;
                        break;

                    case System.Windows.Forms.DialogResult.No:
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        return;

                    case System.Windows.Forms.DialogResult.Cancel:
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        return;
                }
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

                theBatch.Config.ChangeDetectionConfig.Threshold = ucRBTChangeDetection1.Threshold;
                theBatch.Config.ChangeDetectionConfig.ClearMasks();
                foreach (Classes.BudgetSegregation aMask in ucRBTChangeDetection1.BudgetMasks.CheckedItems)
                {
                    theBatch.Config.ChangeDetectionConfig.AddMask(aMask.MaskName);
                }

                sMessage = theBatch.Run(txtBatch.Text, txtInputFileRoot.Text, txtMonitoringDataFolder.Text, chkCalculateMetrics.Checked, chkChangeDetection.Checked, true, chkIncludeOtherVisits.Checked, chkGenerateCSVs.Checked);
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

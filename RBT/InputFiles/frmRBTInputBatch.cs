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

        private List<int> m_lVisitIDs;

        public frmRBTInputBatch(OleDbConnection dbCon, List<int> lVisitIDs)
        {
            m_dbCon = dbCon;
            m_lVisitIDs = lVisitIDs;
            InitializeComponent();
        }

        private void frmRBTInputBatch_Load(object sender, EventArgs e)
        {
            RefreshVisitPaths();




            ucConfig.ManualInitialization();

            txtBatch.Text = "Batch " + DateTime.Now.ToString("yyy_MM_dd");
            txtInputFileRoot.Text = Classes.InputFileBuilder.m_sDefaultRBTInputXMLFileName;
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

        private void RefreshVisitPaths()
        {
            lstVisits.Items.Clear();

            OleDbCommand dbCom = new OleDbCommand("SELECT V.VisitID, W.WatershedName, S.SiteName, V.VisitYear" +
               " FROM (CHAMP_Watersheds AS W INNER JOIN CHAMP_Sites AS S ON W.WatershedID = S.WatershedID) INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID" +
               " WHERE (V.VisitYear Is Not Null) AND (V.VisitID Is Not Null) AND (W.WatershedName Is Not Null) AND (S.SiteName Is Not Null)", m_dbCon);
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            while (dbRead.Read())
            {
                int nVisitID = (int)dbRead["VisitID"];
                if (m_lVisitIDs.Contains<int>(nVisitID))
                {
                    System.IO.DirectoryInfo dVisitTopoFolder = null;

                    string sPath = string.Format("{0}\\{1}\\{2}\\VISIT_{3}", dbRead["VisitYear"], dbRead["WatershedName"].ToString().Replace(" ", ""), dbRead["SiteName"].ToString().Replace(" ", ""), nVisitID);
                    lstVisits.Items.Add(new ListItem(sPath, (int)dbRead["VisitID"]));
                }
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

            //if (chkChangeDetection.Checked && !chkIncludeOtherVisits.Checked)
            //{
            //    DialogResult eResult = MessageBox.Show("You must choose to include other visits if you want to perform change detection. Do you want to invlude all other visits?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    switch (eResult)
            //    {
            //        case System.Windows.Forms.DialogResult.Yes:
            //            chkIncludeOtherVisits.Checked = true;
            //            break;

            //        case System.Windows.Forms.DialogResult.No:
            //            this.DialogResult = System.Windows.Forms.DialogResult.None;
            //            return;

            //        case System.Windows.Forms.DialogResult.Cancel:
            //            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //            return;
            //    }
            //}

            string sMessage = string.Empty;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                Classes.Config rbtConfig = ucConfig.GetRBTConfig();
                Classes.Outputs rbtOutputs = ucConfig.GetRBTOutputs(txtOutputFolder.Text);

                Classes.BatchInputfileBuilder theBatch = new Classes.BatchInputfileBuilder(m_dbCon, m_lVisitIDs, rbtConfig, rbtOutputs);

                theBatch.Config.ChangeDetectionConfig.Threshold = ucRBTChangeDetection1.Threshold;
                theBatch.Config.ChangeDetectionConfig.ClearMasks();
                foreach (Classes.BudgetSegregation aMask in ucRBTChangeDetection1.BudgetMasks.CheckedItems)
                {
                    theBatch.Config.ChangeDetectionConfig.AddMask(aMask.MaskName);
                }

                sMessage = theBatch.Run(txtBatch.Text, txtInputFileRoot.Text, new System.IO.DirectoryInfo(txtMonitoringDataFolder.Text), true, chkChangeDetection.Checked, true, rdoAll.Checked, true, true);
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtMonitoringDataFolder_TextChanged(object sender, EventArgs e)
        {
            RefreshVisitPaths();
        }
    }
}

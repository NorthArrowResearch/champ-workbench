using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Experimental.Philip
{
    public partial class frmExportMetricValues : Form
    {
        List<CHaMPData.VisitBasic> Visits { get; set; }

        public frmExportMetricValues(List<CHaMPData.VisitBasic> lVisits)
        {
            InitializeComponent();
            Visits = lVisits;
        }

        private void frmExportMetricValues_Load(object sender, EventArgs e)
        {
            txtVisits.Text = Visits.Count.ToString("#,##0");
            naru.ui.Textbox.SetTextBoxToFolder(ref txtOutputFolder, Properties.Settings.Default.MonitoringDataFolder);
            cboMetricSchema.DataSource = Data.Metrics.CopyMetrics.frmCopyMetrics.MetricBatch.Load();
            cboMetricSchema.DisplayMember = "Name";
            cboMetricSchema.ValueMember = "ID";

            cboMetricSchema.Select();
        }

        private bool ValidateForm()
        {
            if (cboMetricSchema.SelectedIndex<0)
            {
                MessageBox.Show("You must select a metric schema to continue.", "Missing Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMetricSchema.Select();
                return false;
            }

            if (!naru.ui.Textbox.ValidateTextBoxFolder(ref txtOutputFolder))
            {
                cmdBrowseOutput.Select();
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }


        }

        private void cmdBrowseOutput_Click(object sender, EventArgs e)
        {
            naru.ui.Textbox.BrowseFolder(ref txtOutputFolder);
        }
    }
}

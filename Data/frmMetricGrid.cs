using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class frmMetricGrid : Form
    {
        public string DBCon { get; set; }

        public frmMetricGrid(string sDBCon, List<CHaMPData.VisitBasic> lVisitIDs)
        {
            InitializeComponent();
            this.Text = "Metric Results";
            DBCon = sDBCon;
            ucMetricGrid1.DBCon = sDBCon;
            ucMetricGrid1.VisitIDs = lVisitIDs;
            ucMetricGrid1.Dock = DockStyle.Fill;
        }

        private void exportMetricsToCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Metric Result CSV File";
            frm.Filter = "Comma Separated Value Files (*.csv)|*.csv";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ucMetricGrid1.ExportDataToCSV(new System.IO.FileInfo(frm.FileName));
                    if (MessageBox.Show("CSV file written successfully. Do you want to open the file?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        System.Diagnostics.Process.Start(frm.FileName);
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }
    }
}

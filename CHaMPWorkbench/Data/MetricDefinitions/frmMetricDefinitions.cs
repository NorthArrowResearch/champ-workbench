using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data.MetricDefinitions
{
    public partial class frmMetricDefinitions : Form
    {
        private naru.ui.SortableBindingList<MetricDefinition> MetricDefs;

        public frmMetricDefinitions()
        {
            InitializeComponent();
        }

        private void frmMetricDefinitions_Load(object sender, EventArgs e)
        {
            MetricDefs = MetricDefinitions.MetricDefinition.Load(naru.db.sqlite.DBCon.ConnectionString);
            grdData.DataSource = MetricDefs;
            grdData.Dock = DockStyle.Fill;
            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.AutoGenerateColumns = false;
        }
    }
}

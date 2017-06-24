using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data.Metrics
{
    public partial class frmMetricDownload : Form
    {
        public frmMetricDownload()
        {
            InitializeComponent();
        }

        private void frmMetricDownload_Load(object sender, EventArgs e)
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstMetricSchemas, naru.db.sqlite.DBCon.ConnectionString, "SELECT SchemaID, Title FROM Metric_Schemas ORDER BY Title", true);
        }
    }
}

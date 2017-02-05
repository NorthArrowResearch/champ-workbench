using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class ucPrograms : UserControl
    {
        naru.ui.SortableBindingList<CHaMPData.Program> Programs;

        public ucPrograms()
        {
            InitializeComponent();
        }

        private void ucPrograms_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
            grdData.Dock = DockStyle.Fill;
            
            grdData.AutoGenerateColumns = false;
            grdData.AllowUserToResizeRows = false;

            grdData.Columns.Add(AddDataGridViewColumn("Name", "Name"));
            grdData.Columns.Add(AddDataGridViewColumn("Web Site", "WebSiteURL"));
            grdData.Columns.Add(AddDataGridViewColumn("FTP Site", "FTPURL"));
            grdData.Columns.Add(AddDataGridViewColumn("AWS Bucket", "AWSBucket"));
            grdData.Columns.Add(AddDataGridViewColumn("Remarks", "Remarks"));

            Programs = new naru.ui.SortableBindingList<CHaMPData.Program>(CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.Program>());
            grdData.DataSource = Programs;
        }

        private DataGridViewTextBoxColumn AddDataGridViewColumn(string sHeaderText, string sDataPropertyName)
        {
            DataGridViewTextBoxColumn aCol = new DataGridViewTextBoxColumn();
            aCol.HeaderText = sHeaderText;
            aCol.DataPropertyName = sDataPropertyName;
            aCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            return aCol;
        }
    }
}

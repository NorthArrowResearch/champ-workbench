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
        private List<long> DeletedIDs;

        public ucPrograms()
        {
            InitializeComponent();
            DeletedIDs = new List<long>();
        }

        private void ucPrograms_Load(object sender, EventArgs e)
        {
            grdData.Dock = DockStyle.Fill;
            grdData.AutoGenerateColumns = false;
            grdData.AllowUserToResizeRows = false;

            grdData.Columns.Add(AddDataGridViewColumn("Name", "Name", false));
            grdData.Columns.Add(AddDataGridViewColumn("Web Site", "WebSiteURL", false));
            grdData.Columns.Add(AddDataGridViewColumn("FTP Site", "FTPURL", false));
            grdData.Columns.Add(AddDataGridViewColumn("AWS Bucket", "AWSBucket", false));
            grdData.Columns.Add(AddDataGridViewColumn("API", "API", true));
            grdData.Columns.Add(AddDataGridViewColumn("Remarks", "Remarks", false));

            Programs = new naru.ui.SortableBindingList<CHaMPData.Program>(CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.Program>());
            grdData.DataSource = Programs;
        }

        private DataGridViewTextBoxColumn AddDataGridViewColumn(string sHeaderText, string sDataPropertyName, bool bReadOnly)
        {
            DataGridViewTextBoxColumn aCol = new DataGridViewTextBoxColumn();
            aCol.HeaderText = sHeaderText;
            aCol.DataPropertyName = sDataPropertyName;
            aCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            aCol.ReadOnly = bReadOnly;
            return aCol;
        }

        public void Save()
        {
            // The member list is null if the user control has never been made visible
            if (Programs != null)
                CHaMPData.Program.Save(naru.db.sqlite.DBCon.ConnectionString, Programs.ToList<CHaMPData.Program>(), DeletedIDs);
        }

        private void grdData_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.DataBoundItem is CHaMPData.Program)
            {
                CHaMPData.Program obj = (CHaMPData.Program)e.Row.DataBoundItem;
                if (obj.ID > 0)
                    DeletedIDs.Add(obj.ID);
            }
        }
    }
}

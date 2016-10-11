using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricGrid : UserControl
    {
        public string DBCon { get; set; }
        public List<ListItem> VisitIDs { get; set; }

        public ucMetricGrid()
        {
            InitializeComponent();

            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.Dock = DockStyle.Fill;
        }

        private void ucMetricGrid_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DBCon))
                return;

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string sSQL = "SELECT * FROM qryVisitMetrics_Final";
                    if (VisitIDs.Count > 0)
                        sSQL = string.Format("{0} WHERE VisitID IN ({1})", sSQL, string.Join(",", VisitIDs.Select(n => n.Value.ToString()).ToArray()));

                    sSQL += " ORDER BY VISITID";

                    OleDbDataAdapter da = new OleDbDataAdapter(sSQL, dbCon);
                    DataTable ta = new DataTable();
                    da.Fill(ta);
                    grdData.DataSource = ta;
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
            }
        }

        public void ExportDataToCSV(System.IO.FileInfo fiExport)
        {
            StringBuilder sb = new StringBuilder();

            DataTable dt = (DataTable)grdData.DataSource;
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            System.IO.File.WriteAllText(fiExport.FullName, sb.ToString());
        }
    }
}

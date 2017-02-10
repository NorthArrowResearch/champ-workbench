using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class ucMetricGrid : UserControl
    {
        public string DBCon { get; set; }
        public List<CHaMPData.VisitBasic> VisitIDs { get; set; }
        public long ProgramID { get; set; }

        public event EventHandler SelectedVisitChanged;

        public long SelectedVisit
        {
            get
            {
                long nVisitID = 0;
                if (!string.IsNullOrEmpty(DBCon))
                {
                    if (grdData.SelectedRows.Count == 1)
                    {
                        DataRowView drv = (DataRowView)grdData.SelectedRows[0].DataBoundItem;
                        DataRow aRow = drv.Row;
                        int visitColindex = aRow.Table.Columns["Visit"].Ordinal;
                        nVisitID = long.Parse(aRow.ItemArray[visitColindex].ToString(), System.Globalization.NumberStyles.Any);
                    }
                }
                return nVisitID;
            }
        }

        public ucMetricGrid()
        {
            InitializeComponent();

            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.ReadOnly = true;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.MultiSelect = false;
            grdData.Dock = DockStyle.Fill;
        }

        private void ucMetricGrid_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DBCon))
                return;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string sCols = string.Format("SELECT MetricID, DisplayNameShort FROM vwActiveVisitMetrics WHERE ProgramID = {0} GROUP BY MetricID, DisplayNameShort", ProgramID);
                string sqlRows = string.Format("Select VisitID, CAST(VisitID AS str) AS VisitTitle FROM vwActiveVisitMetrics WHERE VisitID IN ({0}) GROUP BY VisitID", string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));
                string sqlContent = string.Format("Select VisitID, MetricID, MetricValue FROM vwActiveVisitMetrics WHERE (ProgramID = {0}) AND VisitID IN ({1})", ProgramID, string.Join(",", VisitIDs.Select(n => n.ID.ToString()).ToArray()));

                DataTable dt = naru.db.sqlite.CrossTab.CreateCrossTab(DBCon, "Visit", sCols, sqlRows, sqlContent);
                grdData.DataSource = dt;

                //
                foreach (DataGridViewColumn aCol in grdData.Columns)
                {
                    if (!aCol.HeaderText.ToLower().EndsWith("id"))
                        aCol.DefaultCellStyle.Format = "#,##0.000";
                }
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

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0)
            {
                if (grdData.SelectedRows[0].Index >= 0)
                {
                    EventHandler handler = this.SelectedVisitChanged;
                    if (handler != null)
                        handler(this, e);
                }
            }
        }
    }
}

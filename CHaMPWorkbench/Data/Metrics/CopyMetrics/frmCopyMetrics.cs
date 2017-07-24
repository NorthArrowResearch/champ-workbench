using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data.Metrics.CopyMetrics
{
    public partial class frmCopyMetrics : Form
    {
        public frmCopyMetrics()
        {
            InitializeComponent();
        }

        private void frmCopyMetrics_Load(object sender, EventArgs e)
        {
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboSource, naru.db.sqlite.DBCon.ConnectionString,
                "SELECT B.BatchID, CASE WHEN B.Title IS NULL THEN '' ELSE B.Title || ' - ' END || S.title || ' - ' || L.Title AS Title" +
                " FROM Metric_Batches B INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID INNER JOIN LookupListItems L ON B.ScavengeTypeID = L.ItemID ORDER BY Title");

            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboDestination, naru.db.sqlite.DBCon.ConnectionString,
                "SELECT SchemaID, Title FROM Metric_Schemas ORDER BY Title");

            grdInfo.AutoGenerateColumns = false;
            grdInfo.AllowUserToAddRows = false;
            grdInfo.AllowUserToDeleteRows = false;
            grdInfo.AllowUserToResizeRows = false;
            grdInfo.RowHeadersVisible = false;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }



        }

        private bool ValidateForm()
        {


            return true;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {

        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdInfo.Rows.Clear();

            if (cboSource.SelectedIndex < 0)
                return;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                naru.db.NamedObject batch = (naru.db.NamedObject)cboSource.SelectedItem;
                long nVisits = naru.db.sqlite.SQLiteHelpers.GetScalarID(dbCon, string.Format("SELECT COUNT(*) AS VisitCount FROM Metric_Instances WHERE BatchID = {0}", batch.ID));
                
                int i = grdInfo.Rows.Add();
                grdInfo.Rows[i].Cells[0].Value = "Number of visits";
                grdInfo.Rows[i].Cells[1].Value = nVisits;

                long nMetrics = naru.db.sqlite.SQLiteHelpers.GetScalarID(dbCon, string.Format("SELECT COUNT(S.SchemaID) FROM Metric_Batches B INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID INNER JOIN Metric_Schema_Definitions D ON S.SchemaID = D.SchemaID WHERE BatchID = {0}", batch.ID));
                i = grdInfo.Rows.Add();
                grdInfo.Rows[i].Cells[0].Value = "Number of Metrics";
                grdInfo.Rows[i].Cells[1].Value = nMetrics;
            }
        }
    }
}

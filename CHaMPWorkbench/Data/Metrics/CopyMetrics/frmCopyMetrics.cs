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
            cboSource.DataSource = MetricBatch.Load();
            cboSource.DisplayMember = "Name";
            cboSource.ValueMember = "ID";

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


            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    MetricBatch sourceBatch = (MetricBatch)cboSource.SelectedItem;

                    // Insert the metric batch first
                    SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Batches (Title, Remarks, SchemaID, ScavengeTypeID) VALUES (@Title, @Remarks, @SchemaID, @ScavengeTypeID)", dbTrans.Connection, dbTrans);
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, txtTitle.Text, "Title");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, txtRemarks.Text, "Remarks");

                    dbCom.Parameters.AddWithValue("SchemaID", ((naru.db.NamedObject)cboDestination.SelectedItem).ID);
                    dbCom.Parameters.AddWithValue("ScavengeTypeID", 1);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    long nDestinationBatchID = naru.db.sqlite.SQLiteHelpers.GetScalarID(ref dbCom);

                    // Insert the metric instances
                    dbCom = new SQLiteCommand("INSERT INTO Metric_Instances(BatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn)" +
                        "SELECT @DestBatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn FROM Metric_Instances WHERE BatchID = @SourceBatchID", dbCon, dbTrans);
                    dbCom.Parameters.AddWithValue("SourceBatchID", sourceBatch.ID);
                    dbCom.Parameters.AddWithValue("DestBatchID", nDestinationBatchID);
                    dbCom.ExecuteNonQuery();

                    // insert the metric values
                    string sqlInsert = string.Empty;
                    switch (sourceBatch.DatabaseTable.ToLower())
                    {
                        case "metric_visitmetrics":
                            sqlInsert = "SELECT T.InstanceID, M.MetricID, M.MetricValue" +
                                " FROM Metric_VisitMetrics M" +
                                " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                                " INNER JOIN Metric_Instances T ON I.VisitID = T.VisitID" +
                                " WHERE (I.BatchID = @SourceBatchID) AND (T.BatchID = @DestinationBatchID) AND M.MetricID IN (SELECT MetricID FROM Metric_Schema_Definitions WHERE SchemaID = @DestinationSchemaID)";
                            break;

                        case "metric_tiermetrics":

                            sqlInsert = "SELECT T.InstanceID, M.MetricID, M.TierID, M.MetricValue" +
                              " FROM Metric_TierMetrics M" +
                              " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                              " INNER JOIN Metric_Instances T ON I.VisitID = T.VisitID" +
                              " WHERE (I.BatchID = @SourceBatchID) AND (T.BatchID = @DestinationBatchID) AND M.MetricID IN (SELECT MetricID FROM Metric_Schema_Definitions WHERE SchemaID = @DestinationSchemaID)";
                            break;

                        case "metric_channelunitmetricS":
                            sqlInsert = "SELECT T.InstanceID, M.MetricID, M.channelUnitNumber, M.MetricValue" +
                                " FROM Metric_ChannelUnitMetrics M" +
                                " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                                " INNER JOIN Metric_Instances T ON I.VisitID = T.VisitID" +
                                " WHERE (I.BatchID = @SourceBatchID) AND (T.BatchID = @DestinationBatchID) AND M.MetricID IN (SELECT MetricID FROM Metric_Schema_Definitions WHERE SchemaID = @DestinationSchemaID)";
                            break;

                        default:
                            throw new Exception("Unhandled database table");
                    }

                    dbCom = new SQLiteCommand(sqlInsert, dbCon, dbTrans);
                    dbCom.Parameters.AddWithValue("SourceBatchID", sourceBatch.ID);
                    dbCom.Parameters.AddWithValue("DestinationBatchID", nDestinationBatchID);
                    dbCom.Parameters.AddWithValue("DestinationSchemaID", ((naru.db.NamedObject)cboDestination.SelectedItem).ID);

                    dbTrans.Commit();
                    MessageBox.Show("Metrics copy to new schema successful.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private bool ValidateForm()
        {
            if (cboSource.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a source metric schema.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboSource.Select();
                return false;
            }

            //naru.db.NamedObject batch = (naru.db.NamedObject)cboSource.SelectedItem;
            //long nSourceSchemaID = naru.db.sqlite.SQLiteHelpers.GetScalarID(naru.db.sqlite.DBCon.ConnectionString,
            //    string.Format("SELECT SchemaID FROM Metric_Batches WHERE BatchID = {0}", batch.ID));

            //naru.db.NamedObject destSchema = (naru.db.NamedObject)cboDestination.SelectedItem;

            if (cboDestination.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a destination metric schema.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboDestination.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("You must provide a title for the destination batch." +
                    " Choose a title that describes the purpose of the new copy or the timing of this process."
                    , CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTitle.Select();
                return false;
            }

            return true;
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.OnlineHelp.FormHelp(this.Name);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdInfo.Rows.Clear();

            if (!(cboSource.SelectedItem is MetricBatch))
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

            MetricBatch selectedBatch = (MetricBatch)cboSource.SelectedItem;
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboDestination, naru.db.sqlite.DBCon.ConnectionString,
                string.Format("SELECT SchemaID, Title FROM Metric_Schemas WHERE DatabaseTable = '{0}' ORDER BY Title", selectedBatch.DatabaseTable));
        }

        public class MetricBatch : naru.db.NamedObject
        {
            public long SchemaID { get; internal set; }
            public string DatabaseTable { get; internal set; }

            public MetricBatch(long nID, string sName, long nSchemaID, string sDatabaseTable)
                : base(nID, sName)
            {
                SchemaID = nSchemaID;
                DatabaseTable = sDatabaseTable;
            }

            public static naru.ui.SortableBindingList<MetricBatch> Load()
            {
                naru.ui.SortableBindingList<MetricBatch> result = new naru.ui.SortableBindingList<MetricBatch>();

                using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT B.BatchID AS BatchID," +
                        " CASE WHEN B.Title IS NULL THEN '' ELSE B.Title || ' - ' END || S.title || ' - ' || L.Title AS Title," +
                        " S.SchemaID AS SchemaID," +
                        " DatabaseTable " +
                        " FROM Metric_Batches B INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID INNER JOIN LookupListItems L ON B.ScavengeTypeID = L.ItemID ORDER BY Title", dbCon);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        result.Add(new MetricBatch(dbRead.GetInt64(dbRead.GetOrdinal("BatchID"))
                            , dbRead.GetString(dbRead.GetOrdinal("Title"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"))
                            , dbRead.GetString(dbRead.GetOrdinal("DatabaseTable"))
                            ));
                    }
                }

                return result;
            }
        }
    }
}

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
        naru.ui.SortableBindingList<CHaMPData.MetricSchema> MetricSchemas { get; set; }

        public frmCopyMetrics()
        {
            InitializeComponent();
            MetricSchemas = new naru.ui.SortableBindingList<CHaMPData.MetricSchema>(CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.MetricSchema>());
            ucBatch.ProgramChanged += cboProgram_SelectedIndexChanged;
        }

        private void frmCopyMetrics_Load(object sender, EventArgs e)
        {
            cboDestination.DisplayMember = "Name";
            cboDestination.ValueMember = "ID";
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
                    // Insert the metric batch first
                    SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Batches (Title, Remarks, SchemaID, ScavengeTypeID) VALUES (@Title, @Remarks, @SchemaID, @ScavengeTypeID)", dbTrans.Connection, dbTrans);
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, txtTitle.Text, "Title");
                    naru.db.sqlite.SQLiteHelpers.AddStringParameterN(ref dbCom, txtRemarks.Text, "Remarks");

                    dbCom.Parameters.AddWithValue("SchemaID", ((naru.db.NamedObject)cboDestination.SelectedItem).ID);
                    dbCom.Parameters.AddWithValue("ScavengeTypeID", 10022);
                    dbCom.ExecuteNonQuery();
                    long nDestinationBatchID = naru.db.sqlite.SQLiteHelpers.GetScalarID(dbTrans, "SELECT last_insert_rowid()");

                    foreach (CHaMPData.MetricBatch batch in ucBatch.SelectedBatches.Values)
                    {
                        // Insert the metric instances
                        dbCom = new SQLiteCommand("INSERT INTO Metric_Instances (BatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn)" +
                        "SELECT @DestBatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn FROM Metric_Instances WHERE (BatchID = @SourceBatchID)" +
                        " AND ( VisitID NOT IN (SELECT VisitID FROM Metric_Instances WHERE BatchID = @DestBatchID) )", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("SourceBatchID", batch.ID);
                        dbCom.Parameters.AddWithValue("DestBatchID", nDestinationBatchID);
                        dbCom.ExecuteNonQuery();

                        // insert the metric values
                        string sqlInsert = string.Empty;
                        switch (batch.DatabaseTable.ToLower())
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

                            case "metric_channelunitmetrics":
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
                        dbCom.Parameters.AddWithValue("SourceBatchID", batch.ID);
                        dbCom.Parameters.AddWithValue("DestinationBatchID", nDestinationBatchID);
                        dbCom.Parameters.AddWithValue("DestinationSchemaID", ((naru.db.NamedObject)cboDestination.SelectedItem).ID);
                        dbCom.ExecuteNonQuery();
                    }

                    dbTrans.Commit();
                    MessageBox.Show("Metric copy to new schema successful.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!ucBatch.ValidateForm())
                return false;

            string sourceDBTable = string.Empty;
            foreach (CHaMPData.MetricBatch batch in ucBatch.SelectedBatches.Values)
            {
                if (string.IsNullOrEmpty(sourceDBTable))
                    sourceDBTable = batch.DatabaseTable;
                else if (string.Compare(sourceDBTable, batch.DatabaseTable, true) != 0)
                {
                    MessageBox.Show("You can only select multiple metric schemas that have the same dimensionality. i.e. either all Visit level metric schemas" +
                        " or all tier 1 etc.", "Invalid Source Metric Schemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                // Check that none of the source metric schemas are selected as the destination schema
                if (cboDestination.SelectedItem is CHaMPData.MetricSchema)
                {
                    if (batch.Schema.ID == ((CHaMPData.MetricSchema)cboDestination.SelectedItem).ID)
                    {
                        MessageBox.Show("You cannot select one of the source metric schemas as the destination metric schema.", "Invalid Metric Schemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(sourceDBTable))
            {
                MessageBox.Show("You must select at least one source metric schema.", "Invalid Source Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboDestination.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a destination metric schema.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboDestination.Select();
                return false;
            }

            if (string.Compare(sourceDBTable, ((CHaMPData.MetricSchema)cboDestination.SelectedItem).DatabaseTable, true) != 0)
            {
                MessageBox.Show("The source metric schemas must have the same dimensionality as the destination metric schema.", "Invalid Destination Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public class MetricBatch : naru.db.NamedObject
        {
            public naru.db.NamedObject ScavengeType { get; internal set; }
            public naru.db.NamedObject Schema { get; internal set; }
            public naru.db.NamedObject Program { get; internal set; }
            public string DatabaseTable { get; internal set; }
            public long Visits { get; internal set; }

            public bool Copy { get; set; }

            public MetricBatch(long nbatchID, long nScavengetTypeID, string sScavengetType, long nSchemaID, string sSchema,
                long nProgramID, string sProgram, string sDatabaseTable, long? nVisits)
                : base(nbatchID, string.Format("{0} - {1} - {2}", sProgram, sScavengetType, sSchema))
            {
                Schema = new naru.db.NamedObject(nSchemaID, sSchema);
                ScavengeType = new naru.db.NamedObject(nScavengetTypeID, sScavengetType);
                Program = new naru.db.NamedObject(nProgramID, sProgram);
                DatabaseTable = sDatabaseTable;
                Copy = false;

                Visits = 0;
                if (nVisits.HasValue)
                    Visits = nVisits.Value;
            }

            public static naru.ui.SortableBindingList<MetricBatch> Load()
            {
                naru.ui.SortableBindingList<MetricBatch> result = new naru.ui.SortableBindingList<MetricBatch>();

                using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT B.BatchID, B.ScavengeTypeID, L.Title AS ScavengeType, S.ProgramID, P.Title AS Program, S.SchemaID, S.Title AS Schema, S.DatabaseTable, Count(I.BatchID) AS Visits" +
                        " FROM Metric_Batches B" +
                            " INNER JOIN Metric_Schemas S ON B.SchemaID = S.SchemaID" +
                            " INNER JOIN LookupListItems L ON B.ScavengeTypeID = L.ItemID" +
                            " INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID" +
                            " LEFT JOIN Metric_Instances I ON B.BatchID = I.BatchID" +
                        " GROUP BY B.BatchID, B.ScavengeTypeID, ScavengeType, S.ProgramID, Program, S.SchemaID, Schema, S.DatabaseTable", dbCon);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        result.Add(new MetricBatch(dbRead.GetInt64(dbRead.GetOrdinal("BatchID"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("ScavengeTypeID"))
                            , dbRead.GetString(dbRead.GetOrdinal("ScavengeType"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("SchemaID"))
                            , dbRead.GetString(dbRead.GetOrdinal("Schema"))
                            , dbRead.GetInt64(dbRead.GetOrdinal("ProgramID"))
                            , dbRead.GetString(dbRead.GetOrdinal("Program"))
                            , dbRead.GetString(dbRead.GetOrdinal("DatabaseTable"))
                            , naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Visits")));
                    }
                }

                return result;
            }
        }

        private void cboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDestination.DataSource = null;
            if (ucBatch.SelectedProgram == null)
                return;

            cboDestination.DataSource = MetricSchemas.Where<CHaMPData.MetricSchema>(x => x.ProgramID == ucBatch.SelectedProgram.ID).ToList<CHaMPData.MetricSchema>();
        }
    }
}

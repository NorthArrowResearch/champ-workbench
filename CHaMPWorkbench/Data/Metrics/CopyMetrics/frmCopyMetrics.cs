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
        }

        private void frmCopyMetrics_Load(object sender, EventArgs e)
        {
            MetricSchemas = new naru.ui.SortableBindingList<CHaMPData.MetricSchema>(CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.MetricSchema>());
            cboDestination.DisplayMember = "Name";
            cboDestination.ValueMember = "ID";

            ucBatch.ProgramChanged += cboProgram_SelectedIndexChanged;
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
                    dbCom.Parameters.AddWithValue("ScavengeTypeID", 10022);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    long nDestinationBatchID = naru.db.sqlite.SQLiteHelpers.GetScalarID(ref dbCom);

                    foreach (CHaMPData.MetricBatch batch in ucBatch.SelectedBatches.Values)
                    {
                        // Insert the metric instances
                        dbCom = new SQLiteCommand("INSERT INTO Metric_Instances (BatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn)" +
                        "SELECT @DestBatchID, VisitID, ModelVersion, MetricsCalculatedOn, APIInsertionOn, WorkbenchInsertionOn FROM Metric_Instances WHERE (BatchID = @SourceBatchID)" +
                        " AND ( VisitID NOT IN (SELECT VisitID FROM Metric_Instances WHERE BatchID = @DestBatchID) )", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("SourceBatchID", batch.ID);
                        dbCom.Parameters.AddWithValue("DestBatchID", nDestinationBatchID);
                        dbCom.ExecuteNonQuery();

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
            // Validate the user control of selected batches
            if (!ucBatch.Validate())
                return false;

            string sourceDBTable = string.Empty;
            foreach (CHaMPData.MetricBatch batch in ucBatch.SelectedBatches.Values)
            {
                if (string.IsNullOrEmpty(sourceDBTable))
                    sourceDBTable = batch.DatabaseTable;

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

        private void cboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDestination.DataSource = MetricSchemas.Where<CHaMPData.MetricSchema>(x => x.ProgramID == ucBatch.SelectedProgram.ID).ToList<CHaMPData.MetricSchema>();
        }
    }
}
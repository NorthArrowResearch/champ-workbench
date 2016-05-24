using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CHaMPWorkbench.Classes
{

    public class CHaMPMetricScavenger
    {
        private string DBWorkbench { get; set; }

        public CHaMPMetricScavenger(string sDBWorkbenchCon)
        {
            DBWorkbench = sDBWorkbenchCon;
        }

        public List<String> Run(string sModelVersion, string sDBCon, string sDBFilePath, bool bClearExistingData)
        {
            List<string> lResults = new List<string>();

            using (OleDbConnection conWorkbench = new OleDbConnection(DBWorkbench))
            {
                // Workbench transaction that will be rolled back should anything go wrong
                conWorkbench.Open();
                OleDbTransaction dbTrans = conWorkbench.BeginTransaction();

                if (bClearExistingData)
                {
                    // Delete all existing scavenged cm.org metrics
                    OleDbCommand sqlDelete = new OleDbCommand("DELETE * FROM Metric_Results WHERE ScavengeTypeID = @ScavengeTypeID", conWorkbench, dbTrans);
                    sqlDelete.Parameters.AddWithValue("@ScavengeTypeID", CHaMPWorkbench.Properties.Settings.Default.ModelScavengeTypeID);
                    sqlDelete.ExecuteNonQuery();
                }

                // SQL to insert one result record for each visit for which there are metrics.
                OleDbCommand sqlInsertResult = new OleDbCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, ScavengeTypeID, VisitID) VALUES (@ResultFile, @ModelVersion, @ScavengeTypeID, @VisitID)", conWorkbench, dbTrans);
                sqlInsertResult.Parameters.AddWithValue("@ResultFile", sDBFilePath);
                sqlInsertResult.Parameters.AddWithValue("@ModelVersion", sModelVersion);
                sqlInsertResult.Parameters.AddWithValue("@ScavengeTypeID", CHaMPWorkbench.Properties.Settings.Default.ModelScavengeTypeID);
                OleDbParameter pVisitID = sqlInsertResult.Parameters.Add("@VisitID", OleDbType.Integer);

                // SQL to insert the actual metric values.
                OleDbCommand sqlInsertMetricValue = new OleDbCommand("INSERT INTO Metric_VisitMetrics (ResultID, MetricID, MetricValue) VALUES (@ResultID, @MetricID, @MetricValue)", conWorkbench, dbTrans);
                OleDbParameter pResultID = sqlInsertMetricValue.Parameters.Add("@ResultID", OleDbType.Integer);
                OleDbParameter pMetricID = sqlInsertMetricValue.Parameters.Add("@MetricID", OleDbType.Integer);
                OleDbParameter pMetricValue = sqlInsertMetricValue.Parameters.Add("@MetricValue", OleDbType.Double);

                try
                {
                    // Loop over Metric_Definitions and construct dictionary of metric IDs to export field names
                    string sResultDBCon = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sDBFilePath);
                    using (OleDbConnection dbExport = new OleDbConnection(sResultDBCon))
                    {
                        dbExport.Open();
                        OleDbDataAdapter daResults = new OleDbDataAdapter("SELECT * FROM MetricVisitInformation", dbExport);
                        System.Data.DataTable tResults = new System.Data.DataTable();
                        daResults.Fill(tResults);
                        lResults.Add(string.Format("{0} visits with metric identified in the CHaMP export database.", tResults.Rows.Count));

                        if (tResults.Rows.Count > 0)
                        {
                            // Retrieve Column names from export DB and the metric information from Metric_Definitions;
                            Dictionary<int, MetricDef> dMetrics = RetrieveMetrics(ref tResults);
                            lResults.Add(string.Format("0 metrics matched between the CHaMP export and those in the Workbench Metric_Definitions table.", dMetrics.Count));

                            // Loop over rows in export
                            int nResults = 0;
                            foreach (System.Data.DataRow rResult in tResults.Rows)
                            {
                                // Create Result record
                                pVisitID.Value = rResult["VisitID"];
                                nResults += sqlInsertResult.ExecuteNonQuery();

                                // Retrieve ResultID
                                OleDbCommand sqlResultID = new OleDbCommand("SELECT @@Identity FROM Metric_Results", conWorkbench, dbTrans);
                                object objResultID = sqlResultID.ExecuteScalar();
                                int nResultID = 0;
                                if (int.TryParse(objResultID.ToString(), out nResultID))
                                {
                                    foreach (MetricDef theMetric in dMetrics.Values)
                                    {
                                        // Loop over dictionary and insert one metric value for each item
                                        pResultID.Value = nResultID;
                                        pMetricID.Value = theMetric.MetricID;

                                        if (rResult.IsNull(theMetric.FieldIndex))
                                            pMetricValue.Value = DBNull.Value;
                                        else
                                            pMetricValue.Value = rResult[theMetric.FieldIndex];

                                        sqlInsertMetricValue.ExecuteNonQuery();
                                    }
                                }
                            }
                            
                            dbTrans.Commit();
                            lResults.Add(string.Format("{0} visits inserted into the workbench with metric values.", nResults));
                        }
                    }
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    lResults.Add(ex.Message);
                    throw;
                }
            }

            return lResults;
        }

        private Dictionary<int, MetricDef> RetrieveMetrics(ref System.Data.DataTable tResults)
        {
            Dictionary<int, MetricDef> dMetrics = new Dictionary<int, MetricDef>();

            using (OleDbConnection dbworkbench = new OleDbConnection(DBWorkbench))
            {
                dbworkbench.Open();
                OleDbCommand sqlMetrics = new OleDbCommand("SELECT MetricID, Title, DisplayNameShort FROM Metric_Definitions WHERE (CMMetricID IS NOT NULL) AND (DisplayNameShort IS NOT NULL)", dbworkbench);
                OleDbDataReader readMetrics = sqlMetrics.ExecuteReader();

                while (readMetrics.Read())
                {
                    string sMetricDisplayName = readMetrics.GetString(readMetrics.GetOrdinal("DisplayNameShort"));
                    Console.WriteLine(sMetricDisplayName);
                    try
                    {
                        if (tResults.Columns.Contains(sMetricDisplayName))
                        {
                            dMetrics.Add(
                                readMetrics.GetInt32(readMetrics.GetOrdinal("MetricID")),
                                new MetricDef(readMetrics.GetString(readMetrics.GetOrdinal("Title")),
                                readMetrics.GetInt32(readMetrics.GetOrdinal("MetricID")),
                                readMetrics.GetString(readMetrics.GetOrdinal("DisplayNameShort")),
                                tResults.Columns.IndexOf(sMetricDisplayName)));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            return dMetrics;
        }

        private class MetricDef
        {
            public string MetricTitle { get; internal set; }
            public int MetricID { get; internal set; }
            public int FieldIndex { get; internal set; }
            public string DisplayNameShort { get; internal set; }

            public MetricDef(string sMetricTitle, int nMetricID, string sDisplayNameShort, int nFieldIndex)
            {
                MetricTitle = sMetricTitle;
                MetricID = nMetricID;
                DisplayNameShort = sDisplayNameShort;
                FieldIndex = nFieldIndex;
            }

            public override string ToString()
            {
                return MetricTitle;
            }
        }
    }
}

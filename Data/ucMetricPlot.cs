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
    public partial class ucMetricPlot : UserControl
    {
        public string DBCon { get; set; }
        public int VisitID { get; set;}

        public ucMetricPlot()
        {
            InitializeComponent();
        }

        private void ucMetricPlot_Load(object sender, EventArgs e)
        {
            PlotType.LoadPlotTypes(ref cboPlotTypes, DBCon);
            ModelResult.LoadModelResults(ref cboModelResults, DBCon, VisitID);

            cboModelResults.SelectedIndexChanged += PlotChanged;
            cboPlotTypes.SelectedIndexChanged += PlotChanged;
        }

        private void PlotChanged(object sender, EventArgs e)
        {
            PlotType thePlot = null;
            if (cboPlotTypes.SelectedItem is PlotType)
                thePlot = cboPlotTypes.SelectedItem as PlotType;
            else
                return;

            ModelResult theResult = null;
            if (cboModelResults.SelectedItem is ModelResult)
                theResult = cboModelResults.SelectedItem as ModelResult;
            else
                return;

            Dictionary<int, double> dXMetricValues = GetMetricValues(thePlot.XMetricID, theResult.ID);
            Dictionary<int, double> dYMetricValues = GetMetricValues(thePlot.YMetricID, theResult.ID);

        }

        private Dictionary<int, double> GetMetricValues(int nMetricID, int nResultID)
        {
            Dictionary<int, double> dResults = new Dictionary<int, double>();
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT ResultID, MetricValue FROM Metric_VisitMetrics WHERE (MetricID = @MetricID) AND (MetricValue IS NOT NULL)", dbCon);
                dbCom.Parameters.AddWithValue("@MetricID", nMetricID);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    dResults.Add(dbRead.GetInt32(dbRead.GetOrdinal("ResultID")), dbRead.GetDouble(dbRead.GetOrdinal("MetricValue")));
            }

            return dResults;
        }


        # region HelperClasses

        private class PlotType
        {
            public int PlotID { get; internal set; }
            public string Title { get; internal set; }
            public int XMetricID { get; internal set; }
            public int YMetricID { get; internal set; }
            public int PlotTypeID { get; internal set; }

            public PlotType(int nPlotID, string sTitle, int nXMetricID, int nYMetricID, int nPlotTypeID)
            {
                PlotID = nPlotID;
                Title = sTitle;
                XMetricID = nXMetricID;
                YMetricID = nYMetricID;
                PlotTypeID = nPlotTypeID;
            }

            public override string ToString()
            {
                return Title;
            }

            public static void LoadPlotTypes(ref ComboBox cbo, string sDBCon)
            {
                cbo.Items.Clear();

                using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand("SELECT PlotID, PlotTitle, XMetricID, YMetricID, PlotTypeID FROM Metric_Plots ORDER BY PlotTitle", dbCon);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        cbo.Items.Add(new PlotType(dbRead.GetInt32(dbRead.GetOrdinal("PlotID")), dbRead.GetString(dbRead.GetOrdinal("PlotTitle")),
                            dbRead.GetInt32(dbRead.GetOrdinal("XMetricID")), dbRead.GetInt32(dbRead.GetOrdinal("YMetricID")), dbRead.GetInt32(dbRead.GetOrdinal("PlotTypeID"))));
                    }

                    if (cbo.Items.Count > 0)
                        cbo.SelectedIndex = 0;
                }
            }
        }

        private class ModelResult
        {
            public int ID {get; internal set;}
            public string ModelVersion { get; internal set;}
            public string ScavengeType { get; internal set;}
            public DateTime RunDateTime { get; internal set; }
            public bool HasDateTime { get; set; }

            public string Title
            {
                get
                {
                    return string.Format("Version {0} on {1} status {2}", ModelVersion, RunDateTime, ScavengeType);
                }
            }

            public override string ToString()
            {
                return Title;
            }
            
            public ModelResult(int nID, string sModelVersion, DateTime dtRunDateTime, string sScavengeType)
            {
                ID=nID;
                ModelVersion =sModelVersion;

                RunDateTime=dtRunDateTime;
                HasDateTime = true;
                ScavengeType=sScavengeType;
            }

            public ModelResult(int nID, string sModelVersion, string sScavengeType)
            {
                ID = nID;
                ModelVersion = sModelVersion;
                HasDateTime = false;
                ScavengeType = sScavengeType;
            }
            
            public static void LoadModelResults(ref ComboBox cbo, string sDBCon, int VisitID)
            {
               cbo.Items.Clear();

                using (OleDbConnection dbCon = new OleDbConnection(sDBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand("SELECT Metric_Results.ResultID, Metric_Results.ModelVersion, Metric_Results.RunDateTime, LookupListItems.Title AS ScavengeType FROM LookupListItems INNER JOIN Metric_Results ON LookupListItems.ItemID = Metric_Results.ScavengeTypeID WHERE VisitID = @VisitID ORDER BY Metric_Results.RunDateTime DESC;", dbCon);
                    dbCom.Parameters.AddWithValue("@VisitID", VisitID);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        ModelResult theResult = null;
                        if (dbRead.IsDBNull(dbRead.GetOrdinal("RunDateTime")))
                            theResult = new ModelResult(dbRead.GetInt32(dbRead.GetOrdinal("ResultID")), dbRead.GetString(dbRead.GetOrdinal("ModelVersion")), dbRead.GetString(dbRead.GetOrdinal("ScavengeType")));
                        else
                            theResult = new ModelResult(dbRead.GetInt32(dbRead.GetOrdinal("ResultID")), dbRead.GetString(dbRead.GetOrdinal("ModelVersion")),
                            dbRead.GetDateTime(dbRead.GetOrdinal("RunDateTime")), dbRead.GetString(dbRead.GetOrdinal("ScavengeType")));
                            
                        cbo.Items.Add(theResult);
                    }

                    if (cbo.Items.Count > 0)
                        cbo.SelectedIndex = 0;
                }
            }
        }

        #endregion
    }
}

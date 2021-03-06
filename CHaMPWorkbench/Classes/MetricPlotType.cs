﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Classes
{
    public class MetricPlotType
    {
        public long PlotID { get; internal set; }
        public string Title { get; internal set; }
        public long XMetricID { get; internal set; }
        public string XMetric { get; internal set; }
        public long YMetricID { get; internal set; }
        public string YMetric { get; internal set; }
        public long PlotTypeID { get; internal set; }

        public MetricPlotType(long nPlotID, string sTitle, long nXMetricID, string sXMetric, long nYMetricID, string sYMetric, long nPlotTypeID)
        {
            PlotID = nPlotID;
            Title = sTitle;
            XMetricID = nXMetricID;
            XMetric = sXMetric;
            YMetricID = nYMetricID;
            YMetric = sYMetric;
            PlotTypeID = nPlotTypeID;

        }

        public override string ToString()
        {
            return Title;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="sDBCon"></param>
        /// <param name="nProgramID">If ProgramID provided then only plots that used X and Y metrics that are
        /// both part of the specified program are loaded. All plots are loaded if no ProgramID provided.</param>
        public static void LoadPlotTypes(ref ComboBox cbo, string sDBCon, long nSchemaID = 0)
        {
            cbo.Items.Clear();

            using (SQLiteConnection dbCon = new SQLiteConnection(sDBCon))
            {
                dbCon.Open();

                // Base query regardless of whether filtering by ProgramID"
                string sSQL = "SELECT P.PlotID, P.PlotTitle, P.XMetricID, X.Title AS XTitle, P.YMetricID, Y.Title AS YTitle, P.PlotTypeID" +
                    " FROM ((Metric_Plots AS P" +
                    " INNER JOIN Metric_Definitions AS Y ON P.YMetricID = Y.MetricID)" +
                    " INNER JOIN Metric_Definitions AS X ON P.XMetricID = X.MetricID)";

                if (nSchemaID >0)
                {
                    sSQL += " INNER JOIN Metric_Schema_Definitions AS MDPY ON Y.MetricID = MDPY.MetricID)" +
                    " INNER JOIN Metric_Schema_Definitions AS MDPX ON X.MetricID = MDPX.MetricID)" +
                    " WHERE (MDPY.SchemaID = @SchemaID) AND (MDPX.SchemaID = @SchemaID)";

                    // Remember to insert 2 additional parenthese for the above inner joins
                    sSQL = sSQL.Replace("FROM ", "FROM ((");
                }

                sSQL += " ORDER BY P.PlotTitle";

                System.Diagnostics.Debug.Print(sSQL);
                SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon);

                if (nSchemaID > 0)
                    dbCom.Parameters.AddWithValue("SchemaID", nSchemaID);

                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    cbo.Items.Add(new MetricPlotType(
                        dbRead.GetInt64(dbRead.GetOrdinal("PlotID"))
                        , dbRead.GetString(dbRead.GetOrdinal("PlotTitle"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("XMetricID"))
                        , dbRead.GetString(dbRead.GetOrdinal("XTitle"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("YMetricID"))
                        , dbRead.GetString(dbRead.GetOrdinal("YTitle"))
                        , dbRead.GetInt64(dbRead.GetOrdinal("PlotTypeID"))));
                }

                if (cbo.Items.Count > 1)
                    cbo.SelectedIndex = 1;
            }
        }
    }
}

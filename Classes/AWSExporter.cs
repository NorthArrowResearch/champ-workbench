using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace CHaMPWorkbench.Classes
{
    /// <summary>
    /// Exports basic CHaMP information from the Workbench database to JSON text file.
    /// Technique developed from: http://stackoverflow.com/questions/17398019/how-to-convert-datatable-to-json-in-c-sharp
    /// </summary>
    class AWSExporter
    {
        /// <summary>
        /// Run the AWS exporter.
        /// </summary>
        /// <param name="dbCon">Workbench database connection</param>
        /// <param name="fiExport">File name of the file to be exported.</param>
        /// <returns></returns>
        public int Run(ref OleDbConnection dbCon, System.IO.FileInfo fiExport)
        {
            DataTable dt = new DataTable();
            using (OleDbCommand cmd = new OleDbCommand("SELECT V.VisitYear AS [Year], W.WatershedName AS Watershed, S.SiteName AS Site, V.VisitID AS Visit, V.IsPrimary, V.Discharge, V.D84" +
                " FROM CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID" +
                " GROUP BY V.VisitYear, W.WatershedName, S.SiteName, V.VisitID, V.IsPrimary, V.Discharge, V.D84" +
                " ORDER BY V.VisitYear, W.WatershedName, S.SiteName, V.VisitID", dbCon))
            {
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                using (System.IO.StreamWriter wCSV = new System.IO.StreamWriter(fiExport.FullName))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (string.Compare(col.ToString(), "Watershed", true) == 0)
                                row.Add(col.ColumnName, dr[col].ToString().Replace(" ", ""));
                            else
                                row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    wCSV.Write(serializer.Serialize(rows));
                }

                return dt.Rows.Count;
            }
        }

        public int Run(ref OleDbConnection dbCon, String sSQL_Statement, System.IO.FileInfo fiExport)
        {
            DataTable dt = new DataTable();
            using (OleDbCommand cmd = new OleDbCommand(sSQL_Statement, dbCon))
            {
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                using (System.IO.StreamWriter wCSV = new System.IO.StreamWriter(fiExport.FullName))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (string.Compare(col.ToString(), "ID", true) == 0)
                                row.Add(col.ColumnName, dr[col].ToString().Replace(" ", ""));
                            else
                                row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    wCSV.Write(serializer.Serialize(rows));
                }

                return dt.Rows.Count;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using naru.db.sqlite;

namespace CHaMPWorkbench.Classes.MetricValidation
{
    class ReportGenerator
    {
        private List<naru.db.NamedObject> m_lVisits;
        // The RBT versions is a dictionary object with the properties <formattedString, rawString>
        private List<naru.db.NamedObject> m_lRBTVersions;
        private ReportItem m_sXSLReport;

        public ReportGenerator(ReportItem xslReport, List<naru.db.NamedObject> lVisits)
        {
            m_sXSLReport = xslReport;
            m_lVisits = lVisits;
            m_lRBTVersions = GetRBTVersions(); // Default to all RBT Versions
        }

        public void GenerateXML()
        {

            string filename = System.IO.Path.GetFileName(m_sXSLReport.FilePath.FullName);
            switch (filename)
            {
                case "rbt_manual.xsl":
                    if (m_lRBTVersions.Count <= 0)
                        throw new Exception("No RBT Versions were found");

                    // Create a second list with pretty, formatted RBT Text;
                    List<naru.db.NamedObject> lRBTFormatted = m_lRBTVersions.Select(x => new naru.db.NamedObject(x.ID, Classes.MetricValidation.Metric.GetFormattedRBTVersion(x.Name))).ToList();
                    Validation.frmSelectHelper frmRBTPicker = new Validation.frmSelectHelper(m_lRBTVersions, "RBT Versions", "Choose one or more RBT Versions:", true);
                    if (frmRBTPicker.ShowDialog() == DialogResult.Cancel)
                        return;

                    // Now make equivalences between our formatted list and the unformatted one. The IDs should match so we just
                    // Have to filter one list using another.
                    m_lRBTVersions = m_lRBTVersions.Where(item => frmRBTPicker.SelectedItems.Any(formattedItem => formattedItem.ID.Equals(item.ID))).ToList();
                    if (m_lRBTVersions.Count <= 0)
                        throw new Exception("You must select at least one RBT Version");
                    Console.WriteLine("RBT MANUAL");
                    break;
                case "watershed.xsl":
                    Validation.frmSelectHelper frmWatershedPicker = new Validation.frmSelectHelper(GetWatersheds(), "Watersheds", "Choose a watershed:", false);
                    if (frmWatershedPicker.ShowDialog() == DialogResult.Cancel)
                        return;

                    if (frmWatershedPicker.SelectedItems.Count <= 0)
                        throw new Exception("You must select at least one Watershed");
                    m_lVisits = GetWatershedVisits(frmWatershedPicker.SelectedItem.ID);
                    Console.WriteLine("WATERSHED");
                    break;
                default:
                    Console.WriteLine("Nothing Good");
                    break;
            }

            if (m_lVisits.Count <= 0)
                throw new Exception("No visits were selected.");


            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Validation Report Output Path";
            frm.Filter = "HTML Files (*.html, *.htm)|*.htm|XML Files (*.xml)|*.xml";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Classes.MetricValidation.ValidationReport report = new Classes.MetricValidation.ValidationReport(DBCon.ConnectionString, m_sXSLReport.FilePath, new System.IO.FileInfo(frm.FileName));
                    Classes.MetricValidation.ValidationReport.ValidationReportResults theResults = report.Run(m_lVisits, m_lRBTVersions);

                    if (System.IO.File.Exists(frm.FileName))
                    {
                        System.Diagnostics.Process.Start(frm.FileName);
                    }
                    else
                    {
                        Exception ex = new Exception("Failed to generate validation report file");
                        ex.Data["Report File"] = frm.FileName;
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Get the Visits for one given watershed
        /// </summary>
        /// <param name="nWatershedID"></param>
        /// <returns></returns>
        private List<naru.db.NamedObject> GetWatershedVisits(long nWatershedID)
        {
            List<naru.db.NamedObject> lWatershedVisits = new List<naru.db.NamedObject>();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT V.VisitID AS VisitID FROM CHAMP_Watersheds AS W INNER JOIN(CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID WHERE(((W.WatershedID) = 12))", dbCon);
                comFS.Parameters.AddWithValue("@WATERSHEDID", nWatershedID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();
                while (dbRead.Read())
                    lWatershedVisits.Add(new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), dbRead.GetInt64(dbRead.GetOrdinal("VisitID")).ToString()));
                dbRead.Close();
            }
            return lWatershedVisits;
        }

        /// <summary>
        /// Helper class we use as a menu tag to store both the name and the XSL path
        /// </summary>
        public class ReportItem
        {
            public string Title { get; internal set; }
            public System.IO.FileInfo FilePath { get; internal set; }

            public ReportItem(string sTitle, string sFilePath)
            {
                Title = sTitle;
                FilePath = new System.IO.FileInfo(sFilePath);
            }

            public override string ToString()
            {
                return Title;
            }
        }

        /// <summary>
        /// Get a list of Watersheds for The report
        /// </summary>
        /// <returns></returns>
        private List<naru.db.NamedObject> GetWatersheds()
        {
            List<naru.db.NamedObject> lWatersheds = new List<naru.db.NamedObject>();
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) GROUP BY WatershedID, WatershedName ORDER BY WatershedName", dbCon);
                SQLiteDataReader dbRead = comFS.ExecuteReader();
                while (dbRead.Read())
                {
                    lWatersheds.Add(new naru.db.NamedObject((long)dbRead[0], (string)dbRead[1]));
                }
                dbRead.Close();
            }
            return lWatersheds;
        }

        /// <summary>
        /// Get the unique RBT versions
        /// </summary>
        /// <returns></returns>
        /// <remarks>PGB 3 Jun 2016 - Altering this SQL query to only return model versions associated with RBT model runs.
        /// This should now ignore manual validation data results and also cm.org download data that also store values
        /// in the Metric_Results table. This is being done because this method is used to retrieve RBT versions for which
        /// results exist in the database. The manual </remarks>
        private List<naru.db.NamedObject> GetRBTVersions()
        {
            List<naru.db.NamedObject> lRBTVersions = new List<naru.db.NamedObject>();
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT ModelVersion FROM Metric_Results WHERE ScavengeTypeID <> @ScavengeTypeIDManual GROUP BY ModelVersion", dbCon);
                comFS.Parameters.AddWithValue("@ScavengeTypeIDManual", CHaMPWorkbench.Properties.Settings.Default.ModelScavengeTypeID_Manual);
                SQLiteDataReader dbRead = comFS.ExecuteReader();
                long counter = 0;
                while (dbRead.Read())
                {
                    counter++;
                    string sRBTVersion = (string)dbRead[0];
                    lRBTVersions.Add(new naru.db.NamedObject(counter, sRBTVersion));
                }
                dbRead.Close();
            }
            return lRBTVersions;
        }

    }
}

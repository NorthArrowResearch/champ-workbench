using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using ZedGraph;

namespace CHaMPWorkbench.Experimental.James
{
     public partial class frmUSGS_StreamDataViewer : Form
    {
        private OleDbConnection m_dbCon;

        public frmUSGS_StreamDataViewer(OleDbConnection dbCon)
        {
            InitializeComponent();            
            m_dbCon = dbCon;
            LoadVisits();
        }

        public frmUSGS_StreamDataViewer(OleDbConnection dbCon, string sSiteName, string sWatershedName)
        {
            InitializeComponent();
            m_dbCon = dbCon;

            LoadVisits();
            cmbCHaMPSite.Text = sSiteName;
            cmbWatershed.Text = sWatershedName;
            
        }

        private void cmdGetData_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;            

            char cQuotes = (char)34;
            string sPythonScriptPath = cQuotes + System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Resources\\Python\\get_usgs_stream_data.py") + cQuotes;


            string sSiteNumber = txtUSGS_SiteNumber.Text;
            string sOutputDirectory = txtOutputDirectory.Text;
            string sOutputPath = System.IO.Path.Combine(sOutputDirectory, String.Format("site_{0}_usgs_discharge.xml", txtUSGS_SiteNumber.Text));
            int iOverwrite = 0; //1 is true and 0 is false

            if (System.IO.File.Exists(sOutputPath) == false)
            {
                string args = sSiteNumber + " " + cQuotes + sOutputPath + cQuotes + " " + iOverwrite;

                RunPythonScript(sPythonScriptPath, args);

                if (System.IO.File.Exists(sOutputPath) == false)
                {
                    throw new System.IO.FileNotFoundException(String.Format("USGS Stream Data file not created."));
                }
            }

            var xmlDoc = System.Xml.Linq.XDocument.Load(sOutputPath);
            System.Xml.Linq.XNamespace usgs = "http://www.cuahsi.org/waterML/1.1/";

            DateTime queryDate = Convert.ToDateTime("2011-01-01T12:00:00.000");

            //var flows = xmlDoc.Root.Elements(usgs + "value")
            //            .Select(elem => new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.FirstAttribute.Value.ToString()))).ToList().Where(flow => flow.Date.TimeOfDay == queryDate.TimeOfDay);

            var queryStreamData = from elem in xmlDoc.Descendants(usgs + "value")
                                  select new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.FirstAttribute.Value.ToString()));//, elem.Attribute("dateTime"));




            List<StreamFlowSample> lStreamData = queryStreamData.ToList();
            if (lStreamData.Count > 0)
            {



                var linqSiteName = from elem in xmlDoc.Descendants(usgs + "siteName")
                                   select (elem.Value);

                string siteName = linqSiteName.ToList()[0];


                //http://stackoverflow.com/questions/14418142/linq-select-group-by

                //get average flow for each day
                var aggregatedStreamFlow = from d in lStreamData
                                           group d by d.Date.Date into agg
                                           select new StreamFlowSample(agg.Average(x => x.Flow), agg.Key);

                //var testing = aggregatedStreamFlow.ToList();
                //foreach (StreamFlowSample i in aggregatedStreamFlow)
                //{
                //    System.Diagnostics.Debug.Print(String.Format("Day: {0} Flow: {1}", i.Date.Date, i.Flow));
                //}

                //get flow from 12 PM for each day
                var queryList = queryStreamData.ToList().Where(flow => flow.Date.TimeOfDay == queryDate.TimeOfDay);
                //var namespaceManager = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
                //namespaceManager.AddNamespace("ns1", @"C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS\WaterML-1.1.xsd");
                //var values = xmlDoc.Document.Elements("ns1:queryInfo");

                ZedGraph.GraphPane zPane = zedGraphControl.GraphPane;

                zPane.GraphObjList.Clear();
                zPane.CurveList.Clear();

                //set and configure the axis
                zPane.Title.Text = String.Format("USGS Gage:{0} {1} {2}", txtUSGS_SiteNumber.Text, Environment.NewLine, siteName);
                zPane.XAxis.Type = ZedGraph.AxisType.Date;
                zPane.XAxis.Scale.MajorUnit = ZedGraph.DateUnit.Year;
                zPane.XAxis.Scale.MajorStep = .5;
                zPane.XAxis.Scale.Format = "MM-yyyy";
                zPane.XAxis.Scale.MinorUnit = ZedGraph.DateUnit.Month;
                zPane.XAxis.Scale.MinorStep = 4;
                zPane.XAxis.Scale.FontSpec.Angle = 45;
                zPane.XAxis.Title.Text = "Date";
                DateTime minDate = aggregatedStreamFlow.Min(d => d.Date.Date);
                ZedGraph.XDate xMin = new ZedGraph.XDate(minDate.Year, minDate.Month, minDate.Day);
                DateTime maxDate = aggregatedStreamFlow.Max(d => d.Date.Date);
                ZedGraph.XDate xMax = new ZedGraph.XDate(maxDate.Year, maxDate.Month, maxDate.Day);
                zPane.XAxis.Scale.Min = xMin;
                zPane.XAxis.Scale.Max = xMax;
                zPane.YAxis.Title.Text = "Flow (cubic feet/second)";



                //Add the survey dates for the site in question
                string sGroupFields = " V.VisitID, V.SiteID, V.SampleDate, S.SiteID, S.SiteName";

                string sCHaMPSiteName = String.Format("\"{0}\"", cmbCHaMPSite.SelectedItem);

                string sSQL = "SELECT " + sGroupFields +
                    " FROM (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) " +
                    " WHERE S.SiteName = " + sCHaMPSiteName +
                    " ORDER BY V.SampleDate";

                // Load the field seasons
                OleDbCommand comFS = new OleDbCommand(sSQL, m_dbCon);
                OleDbDataReader dbRead = comFS.ExecuteReader();

                TupleList<ZedGraph.XDate, int> zVisitSampleDates = new TupleList<ZedGraph.XDate, int>();
                while (dbRead.Read())
                {
                    DateTime sampleDate = Convert.ToDateTime(dbRead[2]);
                    int iVisitID = Convert.ToInt32(dbRead[0]);
                    ZedGraph.XDate xdSample = new ZedGraph.XDate(sampleDate.Year, sampleDate.Month, sampleDate.Day);
                    zVisitSampleDates.Add(xdSample, iVisitID);
                }
                dbRead.Close();

                //Add stream data to PointPairList
                ZedGraph.PointPairList zList = new ZedGraph.PointPairList();
                foreach (StreamFlowSample sample in aggregatedStreamFlow)
                {
                    ZedGraph.XDate x = new ZedGraph.XDate(sample.Date.Year, sample.Date.Month, sample.Date.Day);

                    double y = sample.Flow;
                    if (y < 0)
                    {
                        y = 0;
                    }

                    for (int i = 0; i < zVisitSampleDates.Count; i++)
                    {
                        if (x == zVisitSampleDates[i].Item1)
                        {
                            ZedGraph.LineItem hightlightDate = new ZedGraph.LineItem("Visit " + zVisitSampleDates[i].Item2.ToString(), new double[] { x }, new double[] { y }, Color.Red, SymbolType.Circle);
                            hightlightDate.Symbol.Size = 20;
                            hightlightDate.Symbol.Fill = new ZedGraph.Fill(Color.Transparent);
                            hightlightDate.Label.IsVisible = false;
                            //http://stackoverflow.com/questions/13763420/zedgraph-how-to-label-lineitem-directly-in-chart-using-textobj
                            ZedGraph.TextObj visitText = new ZedGraph.TextObj("Visit " + zVisitSampleDates[i].Item2.ToString(), x, y + 5, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                            visitText.ZOrder = ZedGraph.ZOrder.A_InFront;
                            visitText.FontSpec.Border.IsVisible = false;
                            visitText.FontSpec.Fill.IsVisible = false;
                            visitText.FontSpec.Angle = 45;
                            zPane.CurveList.Add(hightlightDate);
                            zPane.GraphObjList.Add(visitText);
                        }
                    }
                    zList.Add(x, y);
                }

                //Generate time series
                ZedGraph.CurveItem zCurve = zPane.AddCurve("Stream Flow", zList, Color.Blue, ZedGraph.SymbolType.None);
                zedGraphControl.IsShowPointValues = true;
                zedGraphControl.PointValueFormat = "0.00";
                zedGraphControl.PointDateFormat = "dd-MM-yyyy";


                //Configure figure            
                zedGraphControl.AxisChange();
                zedGraphControl.IsShowHScrollBar = true;
                zedGraphControl.IsShowVScrollBar = true;
            }
            else
            {
                MessageBox.Show(String.Format("There is not data for USGS gage number {0}.", sSiteNumber),
                                CHaMPWorkbench.Properties.Resources.MyApplicationNameLong,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private string RunPythonScript(string pythonScriptPath, string args)
        {
            string sResultFile = "";
            System.Diagnostics.ProcessStartInfo initProcess = new System.Diagnostics.ProcessStartInfo();
            initProcess.FileName = String.Format("{0}", "C:\\Python27\\python.exe");
            initProcess.Arguments = string.Format("{0} {1}", pythonScriptPath, args);
            initProcess.CreateNoWindow = true;
            initProcess.UseShellExecute = false;
            initProcess.RedirectStandardOutput = true;
            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(initProcess))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    sResultFile = reader.ReadToEnd();
                }
                
            }            
            return sResultFile;
        }

        private void cmdOutputDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = @"C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS\StreamGages";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtOutputDirectory.Text = dlg.SelectedPath;
            }
        }

        private void LoadVisits()
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sGroupFields = " W.WatershedID, W.WatershedName, V.VisitID, V.VisitYear, V.SampleDate,V.HitchName,V.CrewName,V.PanelName, S.SiteID, S.SiteName, V.Organization, V.QCVisit, V.CategoryName, V.VisitPhase,V.VisitStatus,V.AEM,V.HasStreamTempLogger,V.HasFishData";

            string sSQL = "SELECT " + sGroupFields + ", Count(C.SegmentID) AS ChannelUnits" +
                " FROM ((CHAMP_Watersheds AS W INNER JOIN (CHAMP_Sites AS S INNER JOIN CHAMP_Visits AS V ON S.SiteID = V.SiteID) ON W.WatershedID = S.WatershedID) LEFT JOIN CHaMP_Segments AS Seg ON V.VisitID = Seg.VisitID) LEFT JOIN CHAMP_ChannelUnits AS C ON Seg.SegmentID = C.SegmentID" +
                " GROUP BY " + sGroupFields +
                " ORDER BY W.WatershedName, S.SiteName, V.VisitID";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);

            // Load the field seasons
            OleDbCommand comFS = new OleDbCommand("SELECT VisitYear FROM CHAMP_Visits WHERE (VisitYear Is Not Null) GROUP BY VisitYear ORDER BY VisitYear", m_dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();
            // Load the watersheds
            comFS = new OleDbCommand("SELECT WatershedID, WatershedName FROM CHAMP_Watersheds WHERE (WatershedName Is Not Null) GROUP BY WatershedID, WatershedName ORDER BY WatershedName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            cmbWatershed.Items.Add("");
            while (dbRead.Read())
            {
                int nSel = cmbWatershed.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
            }
            dbRead.Close();

            // Load the Sites
            comFS = new OleDbCommand("SELECT SiteID, SiteName FROM CHAMP_Sites WHERE (SiteName Is Not Null) ORDER BY SiteName", m_dbCon);
            dbRead = comFS.ExecuteReader();
            cmbCHaMPSite.Items.Add("");
            while (dbRead.Read())
            {
                int nSel = cmbCHaMPSite.Items.Add(new ListItem((string)dbRead[1], (int)dbRead[0]));
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private class StreamFlowSample
        {
            private double _flow;
            private DateTime _date;

            public StreamFlowSample(double dFlow, DateTime dtDate)
            {
                _flow = dFlow;
                _date = dtDate;
            }

            public double Flow
            {
                get { return _flow; }
                set { _flow = value; }
            }
            public DateTime Date
            {
                get { return _date; }
                set { _date = value; }
            }

        }

        public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        public Coordinate GetCoordinate(OleDbConnection dbCon, string sValue)
        {
            string sSQL = "SELECT Latitude, Longitude " +
                          " FROM CHAMP_Sites" +
                          " WHERE SiteName = @value";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            dbCom.Parameters.Add(new OleDbParameter("@value", sValue));
            dbCom.ExecuteNonQuery();
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            var coordinate = new Coordinate(0, 0);
            while (dbRead.Read())
            {
                coordinate = new Coordinate(Convert.ToDouble(dbRead[0]), Convert.ToDouble(dbRead[1]));
            }
            dbRead.Close();
            return coordinate;
        }

        private void cmbCHaMPSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbUSGS_Gage.Items.Clear();
            string sCHaMP_Site = cmbCHaMPSite.SelectedItem.ToString();
            Coordinate champCoordinate = GetCoordinate(m_dbCon, sCHaMP_Site);

            string sSQL = "SELECT site_number, latitude, longitude " +
                          " FROM usgs_stream_gages";

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            dbCom.ExecuteNonQuery();
            OleDbDataReader dbRead = dbCom.ExecuteReader();
            List<KeyValuePair<string, double>> lUSGS_GageSites = new List<KeyValuePair<string, double>>();
            while (dbRead.Read())
            {
                string usgsGageNumber = dbRead[0].ToString();
                Coordinate usgsCoordinate = new Coordinate(Convert.ToDouble(dbRead[1]), Convert.ToDouble(dbRead[2]));
                double distance = Math.Round(Coordinate.Distance(champCoordinate, usgsCoordinate, Coordinate.UnitsOfLength.Kilometer), 2);
                Coordinate.CardinalPoints compass = Coordinate.Bearing(champCoordinate, usgsCoordinate).ToCardinalMark();
                lUSGS_GageSites.Add(new KeyValuePair<string, double>(String.Format("{0}; Distance: {1} Direction: {2}", usgsGageNumber, distance, compass), distance));
            }
            dbRead.Close();
            
            //sort in ascending order by distance from champ site 
            var lUSGS_Sorted = lUSGS_GageSites.OrderBy(site => site.Value);

            foreach (KeyValuePair<string, double> usgsGage in lUSGS_Sorted)
            {
                cmbUSGS_Gage.Items.Add(usgsGage.Key);
            }
            
        }

        private void cmbUSGS_Gage_SelectedIndexChanged(object sender, EventArgs e)
        {
            char splitCharacter = (char)59;
            string sUSGS_GageNumber = cmbUSGS_Gage.SelectedItem.ToString().Split(splitCharacter)[0];
            txtUSGS_SiteNumber.Text = sUSGS_GageNumber;
        }

    }
}

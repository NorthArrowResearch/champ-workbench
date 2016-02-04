using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.Xml.Linq;

namespace CHaMPWorkbench.Experimental.James
{
    public partial class frmEnterPostGCD_QAQC_Record : Form
    {

        private OleDbConnection m_dbCon;
        private string[] m_sReasonsForFlag = {"", "Outlier Metric"};
        private string[] m_sErrorTypes = { "", "Rod Height Bust", "Datum Shift", "Other"};
        private string[] m_sErrorDEMs = { "", "NewVisit", "OldVisit", "Both", "Unknown"};

        public frmEnterPostGCD_QAQC_Record(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }



        private void LoadVisits(OleDbConnection dbCon)
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            //dgvGCD_Review.DataSource = null;

            string sGroupFields = "ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified";

            string sSQL = "SELECT " + sGroupFields  +
                " FROM GCD_Review" +
                " ORDER BY DateModified";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            OleDbDataAdapter daGCD_Reivew = new OleDbDataAdapter(dbCom);
            DataTable dtGCD_Review = new DataTable();
            daGCD_Reivew.Fill(dtGCD_Review);
            dgvGCD_Review.DataSource = dtGCD_Review.AsDataView();
            //dgvGCD_Review.Refresh();
            
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void frmEnterPostGCD_QAQC_Record_Load(object sender, EventArgs e)
        {
            LoadVisits(m_dbCon);
        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            if (!(m_dbCon is System.Data.OleDb.OleDbConnection))
                return;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string sGroupFields = "ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified";
            string sValues = String.Format("({0},{1},{2},'{3}',{4},'{5}','{6}','{7}','{8}','{9}')",
                valBudgetSegregationID.Value,
                valNewVisitID.Value,
                valOldVisitID.Value,
                cboReaonForFlag.SelectedItem.ToString(),
                true,
                cboErrorType.SelectedItem.ToString(),
                cboErrorDEM.SelectedItem.ToString(),
                txtComments.Text,
                txtEnteredBy.Text,
                DateTime.Now.ToString());

            string sSQL = "INSERT INTO GCD_Review (ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified)" +
                " VALUES " + sValues;

            OleDbCommand dbCom = new OleDbCommand(sSQL, m_dbCon);
            dbCom.ExecuteNonQuery();
            LoadVisits(m_dbCon);

        }

        private void cmdOutputToJSON_Click(object sender, EventArgs e)
        {

            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save CHaMP Data For AWS";
            frm.Filter = "JSON Files (*.json)|*.json";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Classes.AWSExporter aws = new Classes.AWSExporter();
                    System.IO.FileInfo fiExport = new System.IO.FileInfo(frm.FileName);
                    string sSQL_Statement = "SELECT ID,NewVisitID,OldVisitID,FlagReason,ValidResults,ErrorType,ErrorDEM,Comments,EnteredBy,DateModified " +
                        "FROM GCD_Review " +
                        "ORDER BY DateModified";


                    int nExported = aws.Run(ref m_dbCon, sSQL_Statement, fiExport);

                    if (MessageBox.Show(string.Format("{0:#,##0} records exported to file. Do you want to browse to the file created?", nExported), "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(fiExport.Directory.FullName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void cmdModifySelectedRecord_Click(object sender, EventArgs e)
        {
            int iSelectedCellCount = dgvGCD_Review.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (iSelectedCellCount == 1)
            {
                DataGridViewRow row = dgvGCD_Review.Rows[dgvGCD_Review.CurrentCell.RowIndex];
                valBudgetSegregationID.Value = System.Convert.ToDecimal(row.Cells[10].Value);
                valNewVisitID.Value = System.Convert.ToDecimal(row.Cells[11].Value);
                valOldVisitID.Value = System.Convert.ToDecimal(row.Cells[12].Value);

                PopulateComboBox(cboReaonForFlag, row, row.Cells[13].Value.ToString(), m_sReasonsForFlag);
                PopulateComboBox(cboErrorType, row, row.Cells[15].Value.ToString(), m_sErrorTypes);
                PopulateComboBox(cboErrorDEM, row, row.Cells[16].Value.ToString(), m_sErrorDEMs);
                txtComments.Text = row.Cells[17].Value.ToString();
                txtEnteredBy.Text = row.Cells[18].Value.ToString();
            }

        }

        private void PopulateComboBox(ComboBox cbo, DataGridViewRow row, string sValue, string[] sStandardValues)
        {
            cbo.Items.Clear();
            for (int i = 0; i < sStandardValues.Length; i++)
            {
                if (String.Compare(sStandardValues[i], sValue) != 0)
                {
                    cbo.Items.Add(sStandardValues[i]);
                }
                else
                {
                    cbo.Items.Add(sValue);
                    cbo.SelectedItem = sValue;
                }
            }            
        }

        private void cmdGetStreamData_Click(object sender, EventArgs e)
        {

            var xmlDoc = System.Xml.Linq.XDocument.Load(@"C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS\StreamGages\site_14044000_usgs_discharge.xml");
            System.Xml.Linq.XNamespace usgs = "http://www.cuahsi.org/waterML/1.1/";
            
            DateTime queryDate = Convert.ToDateTime("2011-01-01T12:00:00.000");

            //var flows = xmlDoc.Root.Elements(usgs + "value")
            //            .Select(elem => new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.FirstAttribute.Value.ToString()))).ToList().Where(flow => flow.Date.TimeOfDay == queryDate.TimeOfDay);

            var queryStreamData = from elem in xmlDoc.Descendants(usgs + "value")
                                  select new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.FirstAttribute.Value.ToString()));//, elem.Attribute("dateTime"));


             List<StreamFlowSample> lStreamData = queryStreamData.ToList();
            //http://stackoverflow.com/questions/14418142/linq-select-group-by

            var aggregatedStreamFlow = from d in lStreamData
                                       group d by d.Date.Date into agg
                                       select new StreamFlowSample(agg.Average(x => x.Flow), agg.Key);

            //get average flow for each day
            var testing = aggregatedStreamFlow.ToList();
            foreach (StreamFlowSample i in aggregatedStreamFlow){
                System.Diagnostics.Debug.Print(String.Format("Day: {0} Flow: {1}", i.Date.Date, i.Flow));
            }

            //get flow from 12 PM for each day
            var queryList = queryStreamData.ToList().Where(flow => flow.Date.TimeOfDay == queryDate.TimeOfDay);
            //var namespaceManager = new System.Xml.XmlNamespaceManager(new System.Xml.NameTable());
            //namespaceManager.AddNamespace("ns1", @"C:\Users\A01674762\Box Sync\CHAMP\GCD_Analysis_Meta\raw_Data\USGS\WaterML-1.1.xsd");
            //var values = xmlDoc.Document.Elements("ns1:queryInfo");
            string test = "dummy";

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
                get {return _flow;}
                set { _flow = value;} 
            }
            public DateTime Date 
            {
                get { return _date; }
                set { _date = value; } 
            }

        }
    }
}

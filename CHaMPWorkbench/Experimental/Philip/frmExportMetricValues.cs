using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SQLite;

namespace CHaMPWorkbench.Experimental.Philip
{
    public partial class frmExportMetricValues : Form
    {
        List<CHaMPData.VisitBasic> Visits { get; set; }

        private const string NODATAVALUE = "-9999";

        public frmExportMetricValues(List<CHaMPData.VisitBasic> lVisits)
        {
            InitializeComponent();
            Visits = lVisits;
        }

        private void frmExportMetricValues_Load(object sender, EventArgs e)
        {
            txtVisits.Text = Visits.Count.ToString("#,##0");
            naru.ui.Textbox.SetTextBoxToFolder(ref txtOutputFolder, Properties.Settings.Default.InputOutputFolder);
            cboMetricSchema.DataSource = CHaMPData.MetricBatch.Load();
            cboMetricSchema.DisplayMember = "Name";
            cboMetricSchema.ValueMember = "ID";

            cboMetricSchema.Select();
        }

        private bool ValidateForm()
        {
            if (cboMetricSchema.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a metric schema to continue.", "Missing Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMetricSchema.Select();
                return false;
            }

            if (!naru.ui.Textbox.ValidateTextBoxFolder(ref txtOutputFolder))
            {
                cmdBrowseOutput.Select();
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                UseWaitCursor = true;

                CHaMPData.MetricBatch batch = (CHaMPData.MetricBatch)cboMetricSchema.SelectedItem;
                CHaMPData.MetricSchema schema = CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString)[batch.Schema.ID];

                foreach (CHaMPData.VisitBasic visit in Visits)
                {
                    string sVisitFolder = visit.VisitFolderAbsolute(txtOutputFolder.Text);
                    string smetricFile = schema.MetricResultXMLFile;

                    // Create the XML document but don't add the metadata information until after the nodes are inserted.
                    XmlDocument xmlDoc = new XmlDocument();

                    switch (schema.DatabaseTable.ToLower())
                    {
                        case "metric_visitmetrics":
                            BuildVisitMetricXML(ref xmlDoc, batch.ID, visit.ID);
                            break;


                        case "metric_tiermetrics":
                            BuildTierMetricXML(ref xmlDoc, batch.ID, visit.ID);
                            break;


                        case "metric_channelunitmetrics":
                            BuildChannelUnitMetricXML(ref xmlDoc, batch.ID, visit.ID);
                            break;

                        default:
                            throw new Exception(string.Format("Unhandled metric schema database table '{0}'", schema.DatabaseTable));
                    }


                    // Create an XML declaration. 
                    XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);

                    XmlNode nodMeta = xmlDoc.CreateElement("Meta");
                    if (xmlDoc.DocumentElement == null)
                    {
                        xmlDoc.AppendChild(nodMeta);
                        xmlDoc.InsertBefore(xmldecl, nodMeta);
                    }
                    else
                    {
                        xmlDoc.DocumentElement.InsertBefore(nodMeta, xmlDoc.DocumentElement.SelectSingleNode("Metrics"));
                        xmlDoc.InsertBefore(xmldecl, xmlDoc.DocumentElement);
                    }

                    naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodMeta, CHaMPData.MetricInstance.GENERATION_DATE_METRIC_NAME, DateTime.Now.ToString("o"));
                    naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodMeta, CHaMPData.MetricInstance.MODEL_VERSION_METRIC_NAME, string.Empty);
                    naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodMeta, "VisitID", visit.ID.ToString());
                    naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodMeta, "NoData", NODATAVALUE);
                    naru.xml.XMLHelpers.AddNode(ref xmlDoc, ref nodMeta, "Tool", "Exported from CHaMP Workbench");

                    System.IO.Directory.CreateDirectory(sVisitFolder);
                    string xmlPath = System.IO.Path.Combine(sVisitFolder, smetricFile);
                    xmlPath = System.IO.Path.ChangeExtension(xmlPath, "xml");
                    xmlDoc.Save(xmlPath);
                }

                UseWaitCursor = false;
                MessageBox.Show(string.Format("Metric values exported to {0} files.", Visits.Count), "Process successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void cmdBrowseOutput_Click(object sender, EventArgs e)
        {
            naru.ui.Textbox.BrowseFolder(ref txtOutputFolder);
        }

        private void BuildVisitMetricXML(ref XmlDocument xmlDoc, long batchID, long visitID)
        {

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT XPath, MetricValue, DataTypeID, Precision" +
                    " FROM Metric_VisitMetrics M" +
                        " INNER JOIN Metric_Instances I ON M.InstanceID = I.InstanceID" +
                        " INNER JOIN Metric_Batches B ON I.BatchID = B.BatchID" +
                        " INNER JOIN Metric_Definitions D ON M.MetricID = D.MetricID" +
                    " WHERE (B.BatchID = @BatchID) AND (I.VisitID = @VisitID)", dbCon);
                dbCom.Parameters.AddWithValue("BatchID", batchID);
                dbCom.Parameters.AddWithValue("VisitID", visitID);

                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                {
                    long? precision = naru.db.sqlite.SQLiteHelpers.GetSafeValueNInt(ref dbRead, "Precision");
                    long metricDataTypeID = dbRead.GetInt64(dbRead.GetOrdinal("DataTypeID"));
                    double? metricValue = naru.db.sqlite.SQLiteHelpers.GetSafeValueNDbl(ref dbRead, "MetricValue");
                    string xpath = dbRead.GetString(dbRead.GetOrdinal("XPath"));
                    string[] xpathParts = xpath.Split('/');

                    int partIndex = 0;
                    XmlNode nod = null;
                    while (partIndex < xpathParts.Length)
                    {
                        if (!string.IsNullOrEmpty(xpathParts[partIndex]))
                            nod = InsertXMLNode(ref xmlDoc, ref nod, xpathParts[partIndex]);
                        partIndex++;
                    }

                    if (metricValue.HasValue)
                    {
                        if (metricDataTypeID == 10023 && precision.HasValue) // numeric
                            if (precision > 0)
                                nod.InnerText = metricValue.Value.ToString(string.Format("0.{0}", new string('0', (int)precision)));
                            else
                                nod.InnerText = metricValue.Value.ToString("0");
                        else
                            nod.InnerText = metricValue.ToString();
                    }
                    else
                        nod.InnerText = NODATAVALUE;
                }
            }
        }

        private void BuildTierMetricXML(ref XmlDocument xmlDoc, long batchID, long visitID)
        {

        }

        private void BuildChannelUnitMetricXML(ref XmlDocument xmlDoc, long batchID, long visitID)
        {

        }

        private XmlNode InsertXMLNode(ref XmlDocument xmlDoc, ref XmlNode nodParent, string sNewNodeName)
        {
            XmlNode nodResult = null;
            if (nodParent == null)
            {
                if (xmlDoc.DocumentElement == null)
                {
                    nodResult = xmlDoc.CreateElement(sNewNodeName);
                    xmlDoc.AppendChild(nodResult);
                    return nodResult;
                }
                else
                    return xmlDoc.DocumentElement;
            }

            nodResult = nodParent.SelectSingleNode(sNewNodeName);
            if (nodResult == null)
            {
                nodResult = xmlDoc.CreateElement(sNewNodeName);
                nodParent.AppendChild(nodResult);
            }

            return nodResult;
        }
    }
}

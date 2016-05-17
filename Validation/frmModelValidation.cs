using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;

namespace CHaMPWorkbench.Validation
{
    public partial class frmModelValidation : Form
    {
        private string m_sDBCon;
        public List<ListItem> Visits { get; internal set; }

        public frmModelValidation(string sDBCon, ref  List<ListItem> lVisits)
        {
            InitializeComponent();
            m_sDBCon = sDBCon;
            Visits = lVisits;
        }

        private void frmModelValidation_Load(object sender, EventArgs e)
        {

            foreach (string sReportXSLPath in System.IO.Directory.GetFiles(ReportFolder, "*.xsl", System.IO.SearchOption.TopDirectoryOnly))
            {
                // System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(sReportXSLPath);
                //System.Xml.XPath.XPathNavigator theNav = doc.CreateNavigator();

                //if (theNav.MoveToChild("title"))
                //{
                //    string sValue = theNav.InnerXml;
                //}

                //System.Xml.XPath.XPathNavigator nodTitle2 = theNav.SelectSingleNode("html/head/title");

                //System.Xml.Xsl.XslTransform xmlReport = new System.Xml.Xsl.XslTransform();
                //xmlReport.Load(sReportXSLPath);

                XmlNode nodTitle = null;
                //System.Xml.XmlDocument xmlReport = new System.Xml.XmlDocument();
                //xmlReport.LoadXml(sReportXSLPath);
                //XmlNode nodTitle = xmlReport.SelectSingleNode("html/head/title");
                if (nodTitle is XmlNode && !string.IsNullOrEmpty(nodTitle.InnerText))
                    lstReports.Items.Add(new ReportItem(nodTitle.InnerText, sReportXSLPath));
                else
                    lstReports.Items.Add(new ReportItem(System.IO.Path.GetFileNameWithoutExtension(sReportXSLPath), sReportXSLPath));
            }


            lstVisits.Items.AddRange(Visits.ToArray<ListItem>());
        }

        private bool ValidateForm()
        {
            if (lstReports.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select a report to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }


            return true;
        }

        private string ReportFolder
        {
            get
            {
                string sReportFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                sReportFolder = System.IO.Path.Combine(sReportFolder, "Validation");
                sReportFolder = System.IO.Path.Combine(sReportFolder, "ReportTransforms");
                System.Diagnostics.Debug.Assert(System.IO.Directory.Exists(sReportFolder), "The XSL Validation Report Folder does not exist.");
                return sReportFolder;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            ReportItem selReport = (ReportItem)lstReports.SelectedItem;

            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Validation Report Output Path";
            frm.Filter = "HTML Files (*.html, *.htm)|*.htm|XML Files (*.xml)|*.xml";

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Classes.MetricValidation.ValidationReport report = new Classes.MetricValidation.ValidationReport(m_sDBCon, selReport.FilePath, new System.IO.FileInfo(frm.FileName));
                    Classes.MetricValidation.ValidationReport.ValidationReportResults theResults = report.Run(Visits.ToList<ListItem>());

                    if (System.IO.File.Exists(frm.FileName))
                    {
                        System.Diagnostics.Process.Start(frm.FileName);
                    }
                    else
                    {
                        Exception ex = new Exception("Failed to generate validation report file");
                        ex.Data["Report File"] = frm.FileName;
                    }
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private class ReportItem
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

    }
}

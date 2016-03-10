using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Validation
{
    public partial class frmModelValidation : Form
    {
        public frmModelValidation()
        {
            InitializeComponent();
        }

        private void frmModelValidation_Load(object sender, EventArgs e)
        {

            foreach (string sReportXSLPath in System.IO.Directory.GetFiles(ReportFolder, "*.xsl", System.IO.SearchOption.TopDirectoryOnly))
            {
                System.Xml.XmlDocument xmlReport = new System.Xml.XmlDocument();
                xmlReport.LoadXml(sReportXSLPath);

            }

            // html/head//title
        }

        private string ReportFolder
        {
            get
            {
                string sReportFolder = System.Reflection.Assembly.GetExecutingAssembly().Location;
                sReportFolder = System.IO.Path.Combine(sReportFolder, "ReportTransforms");
                sReportFolder = System.IO.Path.Combine(sReportFolder, "dist");
                return sReportFolder;
            }
        }
    }
}

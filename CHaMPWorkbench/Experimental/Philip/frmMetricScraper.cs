using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Experimental.Philip
{
    public partial class frmMetricScraper : Form
    {
        public frmMetricScraper()
        {
            InitializeComponent();
        }

        private void frmMetricScraper_Load(object sender, EventArgs e)
        {
            txtFileName.Text = "topo_metrics.xml";
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.LastMetricFolder)
                && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.LastMetricFolder))
            {
                txtFolder.Text = CHaMPWorkbench.Properties.Settings.Default.LastMetricFolder;
            }

            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboScavengeType, naru.db.sqlite.DBCon.ConnectionString, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 1 ORDER BY Title");

            cboMetricSchema.DataSource = CHaMPData.MetricSchema.Load(naru.db.sqlite.DBCon.ConnectionString).Values.Where<CHaMPData.MetricSchema>(x => x.HasDatabaseTable && x.HasMetricSchemaXMLFile).ToList<CHaMPData.MetricSchema>();
            cboMetricSchema.DisplayMember = "Name";
            cboMetricSchema.ValueMember = "ID";

            rdoXMLModelVersion.Checked = true;
            rdoXMLModelVersion_CheckedChanged(null, null);
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
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

                CHaMPWorkbench.Properties.Settings.Default.LastMetricFolder = txtFolder.Text;
                CHaMPWorkbench.Properties.Settings.Default.Save();

                Classes.MetricXPathValidator x = new Classes.MetricXPathValidator(naru.db.sqlite.DBCon.ConnectionString);

                // Get the metric schema from the dropdown and then build one of the local schema objects that's used to verify metrics
                CHaMPData.MetricSchema schema = (CHaMPData.MetricSchema)cboMetricSchema.SelectedItem;
                Dictionary<string, MetricSchemaWithDefs> MetricSchemas = new Dictionary<string, MetricSchemaWithDefs>();
                MetricSchemas[schema.Name] = new MetricSchemaWithDefs(schema);

                List<string> lErros = x.Run(ref MetricSchemas);
                if (chkVerify.Checked && lErros.Count > 0)
                {
                    frmToolResults frm = new frmToolResults("Metric XPath Validation Failed",
                        "The following metrics are defined in the program XML files but failed validation in the workbench database. Metrics can only be scraped once all these issues are resolved" +
                        " and each metric defined in the XML metric schema definition XML file(s) occurs exactly once in the Workbench database.", ref lErros);
                    frm.ShowDialog();
                    DialogResult = DialogResult.None;
                    return;
                }

                Experimental.Philip.TopoMetricScavenger scraper = new Experimental.Philip.TopoMetricScavenger();
                int nFilesProcessed = scraper.Run(txtFolder.Text, txtFileName.Text, MetricSchemas, rdoXMLModelVersion.Checked, txtModelVersion.Text, ((naru.db.NamedObject)cboScavengeType.SelectedItem).ID);
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                MessageBox.Show(string.Format("{0} result XML files processed.", nFilesProcessed), "Process Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtFolder.Text)
               || !System.IO.Directory.Exists(txtFolder.Text))
            {
                MessageBox.Show("You must select an existing top level folder where your metric result XML files are stored.", "Invalid Top Level Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowse.Select();
                return false;
            }

            if (!(cboMetricSchema.SelectedItem is CHaMPData.MetricSchema))
            {
                MessageBox.Show("You must select the metric schema for the types of XML files that you want to scrape.", "Invalid Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMetricSchema.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                MessageBox.Show("You must specify the name of the metric XML files to scrape. Wildcards (* and ?) are allowed.", "Invalid Metric XML File Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFileName.Select();
                return false;
            }
            else
            {
                CHaMPData.MetricSchema schema = (CHaMPData.MetricSchema)cboMetricSchema.SelectedItem;
                if (schema.HasMetricResultXMLFile)
                {
                    if (string.Compare(txtFileName.Text, schema.MetricResultXMLFile, false) != 0)
                    {
                        if (MessageBox.Show(string.Format("The result file name '{0}' does not match the default file name {1} for the {2} metric schema. Do you want to proceed?",
                            txtFileName.Text, schema.MetricResultXMLFile, schema.Name), "Verify Result File Name",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        {
                            txtFileName.Select();
                            return false;
                        }
                    }
                }
            }

            if (cboScavengeType.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a scavenge type.", "Missing Scavenge Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboScavengeType.Select();
                return false;
            }

            if (cboMetricSchema.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a metric schema.", "Missing Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMetricSchema.Select();
                return false;
            }

            if (rdoSpecifiedModelVersion.Checked)
            {
                if (string.IsNullOrEmpty(txtModelVersion.Text))
                {
                    MessageBox.Show("You must specify a model version string or use the model versions specified in the XML files.", "Missing Model Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtModelVersion.Select();
                    return false;
                }
            }

            return true;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            naru.os.Folder.BrowseFolder(ref txtFolder, "Top Level Metric Folder", txtFolder.Text);
        }

        public class MetricSchemaWithDefs : CHaMPData.MetricSchema
        {
            public Dictionary<string, MetricDef> MetricDefs { get; internal set; }

            public MetricSchemaWithDefs(CHaMPData.MetricSchema schema)
                : base(schema.ID, schema.Name, schema.MetricResultXMLFile, schema.MetricSchemaXMLFile, schema.ProgramID, schema.ProgramName, schema.RootXPath, schema.DatabaseTable)
            {
                MetricDefs = new Dictionary<string, MetricDef>();
            }
        }

        public class MetricDef : naru.db.NamedObject
        {
            public string XPath { get; internal set; }

            public MetricDef(long nID, string sTitle, string sXPath) : base(nID, sTitle)
            {
                XPath = sXPath;
            }
        }

        private void rdoXMLModelVersion_CheckedChanged(object sender, EventArgs e)
        {
            txtModelVersion.Enabled = rdoSpecifiedModelVersion.Checked;
        }

        private void cboMetricSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMetricSchema.SelectedItem is CHaMPData.MetricSchema)
            {
                if (((CHaMPData.MetricSchema)cboMetricSchema.SelectedItem).HasMetricResultXMLFile)
                    txtFileName.Text = ((CHaMPData.MetricSchema)cboMetricSchema.SelectedItem).MetricResultXMLFile;
            }
        }
    }
}

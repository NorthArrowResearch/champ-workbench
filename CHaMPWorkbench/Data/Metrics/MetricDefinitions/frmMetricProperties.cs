using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data.MetricDefinitions
{
    public partial class frmMetricProperties : Form
    {
        public MetricDefinition MetricDef { get; internal set; }

        public frmMetricProperties()
        {
            InitializeComponent();
            MetricDef = null;
        }

        public frmMetricProperties(ref MetricDefinition aMetric)
        {
            InitializeComponent();
            MetricDef = aMetric;
        }

        private void frmMetricProperties_Load(object sender, EventArgs e)
        {
            long nModelID = 0;
            long nDataTypeID = 0;

            // Load the programs before the metric definition is loaded.
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref chkSchemas, naru.db.sqlite.DBCon.ConnectionString, "SELECT S.SchemaID, S.Title || ' (' || P.Title || ')' AS Title FROM Metric_Schemas S INNER JOIN LookupPrograms P ON S.ProgramID = P.ProgramID ORDER BY Title", false);

            if (MetricDef is MetricDefinition)
            {
                txtName.Text = MetricDef.Name;
                txtShortName.Text = MetricDef.DisplayNameShort;
                nModelID = MetricDef.ModelID;
                chkActive.Checked = MetricDef.IsActive;
                txtXPath.Text = MetricDef.XPath;

                nDataTypeID = MetricDef.DataTypeID;

                if (MetricDef.Precision.HasValue)
                    valPrecision.Value = (decimal)MetricDef.Precision;

                if (MetricDef.MinValue.HasValue)
                    valMinValue.Value = (decimal)MetricDef.MinValue;

                if (MetricDef.MaxValue.HasValue)
                    valMaxValue.Value = (decimal)MetricDef.MaxValue;

                if (MetricDef.Threshold.HasValue)
                    valThreshold.Value = (decimal)MetricDef.Threshold;

                if (!string.IsNullOrEmpty(MetricDef.MMLink))
                    txtMMLink.Text = MetricDef.MMLink;

                if (!string.IsNullOrEmpty(MetricDef.AltLink))
                    txtAltLink.Text = MetricDef.AltLink;

                txtLastUpdated.Text = MetricDef.UpdatedOn.ToString();

                foreach (long nSchemaID in MetricDef.MetricSchemas)
                {
                    for (int i = 0; i < chkSchemas.Items.Count; i++)
                    {
                        if (((naru.db.NamedObject)chkSchemas.Items[i]).ID == nSchemaID)
                        {
                            chkSchemas.SetItemChecked(i, true);
                            break;
                        }
                    }
                }

                chkValidation.Checked = MetricDef.MinValue.HasValue || MetricDef.MaxValue.HasValue || MetricDef.Threshold.HasValue;
            }
            else
            {
                // New metrics default to active
                chkActive.Checked = true;
            }

            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboModel, naru.db.sqlite.DBCon.ConnectionString, string.Format("SELECT ItemID, Title FROM LookupListItems WHERE ListID = {0} ORDER BY Title", 4), nModelID);
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboDataType, naru.db.sqlite.DBCon.ConnectionString, string.Format("SELECT ItemID, Title FROM LookupListItems WHERE ListID = {0} ORDER BY Title", 14), nDataTypeID);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            lblPrecision.Enabled = cboDataType.Text.ToLower().Contains("numeric");
            valPrecision.Enabled = lblPrecision.Enabled;

            if (valPrecision.Enabled)
            {
                valMinValue.DecimalPlaces = (int)valPrecision.Value;
                valMaxValue.DecimalPlaces = (int)valPrecision.Value;
            }

            lblMinimum.Enabled = chkValidation.Checked;
            valMinValue.Enabled = chkValidation.Checked;
            lblMaximum.Enabled = chkValidation.Checked;
            valMaxValue.Enabled = chkValidation.Checked;
            lblThreshold.Enabled = chkValidation.Checked;
            valThreshold.Enabled = chkValidation.Checked;

            cmdOnlineHelp.Enabled = !string.IsNullOrEmpty(txtMMLink.Text);
            cmdAltHelp.Enabled = !string.IsNullOrEmpty(txtAltLink.Text);
        }

        private void cmdOnlineHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtMMLink.Text);
        }

        private void cmdAltHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtAltLink.Text);
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("The metric name cannot be empty.", "Missing Metric Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtShortName.Text.Trim()))
            {
                MessageBox.Show("The metric short name cannot be empty.", "Missing Metric Short Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtShortName.Select();
                return false;
            }

            if (chkActive.Checked)
            {
                if (string.IsNullOrEmpty(txtXPath.Text.Trim()))
                {
                    MessageBox.Show("Active metrics must possess a valid XPath.", "Missing XPath", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtXPath.Select();
                    return false;
                }

                if (cboDataType.SelectedIndex < 0)
                {
                    MessageBox.Show("Active metrics must possess a data type.", "Missing Data Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboDataType.Select();
                    return false;
                }
            }

            if (cboModel.SelectedIndex < 0)
            {
                MessageBox.Show("You must select the model that produces this metric.", "Missing Model", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboModel.Select();
                return false;
            }

            return true;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                if (MetricDef == null)
                    MetricDef = new MetricDefinition(txtName.Text);

                // Name needs to be reset for updates
                MetricDef.Name = txtName.Text;
                MetricDef.DisplayNameShort = txtShortName.Text;
                MetricDef.ModelID = ((naru.db.NamedObject)cboModel.SelectedItem).ID;
                MetricDef.ModelName = cboModel.Text;
                MetricDef.DataTypeID = ((naru.db.NamedObject)cboDataType.SelectedItem).ID;
                MetricDef.DataTypeName = cboDataType.Text;
                MetricDef.IsActive = chkActive.Checked;
                MetricDef.XPath = txtXPath.Text;

                if (string.Compare(cboDataType.Text, "numeric", true) == 0)
                    MetricDef.Precision = (long)valPrecision.Value;
                else
                    MetricDef.Precision = new long?();

                MetricDef.MetricSchemas.Clear();
                foreach (naru.db.NamedObject item in chkSchemas.CheckedItems)
                    MetricDef.MetricSchemas.Add(item.ID);

                MetricDef.MMLink = txtMMLink.Text;
                MetricDef.AltLink = txtAltLink.Text;

                if (chkValidation.Checked)
                {
                    MetricDef.MinValue = (double)valMinValue.Value;
                    MetricDef.MaxValue = (double)valMaxValue.Value;
                    MetricDef.Threshold = (double)valThreshold.Value;
                }
                else
                {
                    MetricDef.MinValue = new double?();
                    MetricDef.MaxValue = new double?();
                    MetricDef.Threshold = new double?();
                }

                MetricDef.Save();
            }
            catch (SQLiteException exSQL)
            {
                if (exSQL.Message.ToLower().Contains("unique constraint failed"))
                {
                    if (exSQL.Message.ToLower().Contains("metric_definitions.title"))
                    {
                        MessageBox.Show("A metric with this name and schema already exists. Each metric must possess a unique name within a given schema.", "Non-Unique Metric Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Select();
                        this.DialogResult = DialogResult.None;
                    }
                    else if (exSQL.Message.ToLower().Contains("metric_definitions.displaynameshort"))
                    {
                        MessageBox.Show("A metric with this short name and schema already exists. Each metric must possess a unique short name within a given schema.", "Non-Unique Short Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtShortName.Select();
                        this.DialogResult = DialogResult.None;
                    }
                    else
                        throw;
                }
                else
                    throw;
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = DialogResult.None;
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.OnlineHelp.FormHelp(this.Name);
        }
    }
}

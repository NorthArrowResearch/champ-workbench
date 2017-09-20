using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CHaMPWorkbench.CHaMPData;

namespace CHaMPWorkbench.Data.Metrics
{
    public partial class ucBatchPicker : UserControl
    {
        naru.ui.SortableBindingList<MetricBatch> MetricBatches { get; set; }

        // Parent forms should subscribe to this event if they want to be notified when the selected program changes
        public event EventHandler ProgramChanged;

        public Dictionary<long, MetricBatch> SelectedBatches
        {
            get
            {
                Dictionary<long, MetricBatch> items = new Dictionary<long, MetricBatch>();
                foreach (DataGridViewRow aRow in grdInfo.Rows)
                {
                    MetricBatch batch = (MetricBatch)aRow.DataBoundItem;
                    if (batch.Copy)
                        items[batch.ID] = batch;
                }

                return items;
            }
        }

        public long SelectedProgram
        {
            get
            {
                if (cboProgram.SelectedItem is CHaMPData.Program)
                    return ((CHaMPData.Program)cboProgram.SelectedItem).ID;
                else
                    return 0;
            }
        }

        public ucBatchPicker()
        {
            InitializeComponent();

            grdInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdInfo.AutoGenerateColumns = false;
            grdInfo.AllowUserToAddRows = false;
            grdInfo.AllowUserToDeleteRows = false;
            grdInfo.AllowUserToResizeRows = false;
            grdInfo.RowHeadersVisible = false;
        }

        private void ucBatchPicker_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(naru.db.sqlite.DBCon.ConnectionString))
                return;

            MetricBatches = MetricBatch.Load();

            cboProgram.DataSource = CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.Program>();
            cboProgram.DisplayMember = "Name";
            cboProgram.ValueMember = "ID";
        }

        private void cboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdInfo.DataSource = null;

            if (!(cboProgram.SelectedItem is CHaMPData.Program))
                return;

            grdInfo.DataSource = new naru.ui.SortableBindingList<MetricBatch>(MetricBatches.Where<MetricBatch>(x => x.Program.ID == ((CHaMPData.Program)cboProgram.SelectedItem).ID).ToList<MetricBatch>());

            // Raise the program combo changed event so that parent forms are notified
            if (ProgramChanged != null)
                ProgramChanged(this, e);
        }

        public bool ValidateForm()
        {
            if (!(cboProgram.SelectedItem is CHaMPData.Program))
            {
                MessageBox.Show("You must choose a program.", "Missing Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboProgram.Select();
                return false;
            }

            string sourceDBTable = string.Empty;
            foreach (DataGridViewRow aRow in grdInfo.Rows)
            {
                MetricBatch batch = (MetricBatch)aRow.DataBoundItem;
                if (batch.Copy)
                {
                    if (string.IsNullOrEmpty(sourceDBTable))
                        sourceDBTable = batch.DatabaseTable;
                    else if (string.Compare(sourceDBTable, batch.DatabaseTable, true) != 0)
                    {
                        MessageBox.Show("You can only select multiple metric schemas that have the same dimensionality. i.e. either all Visit level metric schemas" +
                            " or all tier 1 etc.", "Invalid Source Metric Schemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grdInfo.Select();
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(sourceDBTable))
            {
                MessageBox.Show("You must select at least one source metric schema.", "Invalid Source Metric Schema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdInfo.Select();
                return false;
            }

            return true;
        }
    }
}

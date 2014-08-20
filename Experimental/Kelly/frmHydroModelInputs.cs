using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CHaMPWorkbench;
using System.Data.OleDb;

namespace CHaMPWorkbench.Experimental.Kelly
{
    public partial class frmHydroModelInputs : Form
    {

        public frmHydroModelInputs()
        {
            InitializeComponent();
        }

        private void frmHydroModelInputs_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rBTWorkbenchDataSet.CHAMP_Visits' table. You can move, or remove it, as needed.
            this.cHAMP_VisitsTableAdapter.Fill(this.rBTWorkbenchDataSet.CHAMP_Visits);

            optAllVisits.CheckedChanged += optRadioButtons_CheckChanged;
            optSelectVisits.CheckedChanged += optRadioButtons_CheckChanged;
            optBatches.CheckedChanged += optRadioButtons_CheckChanged;
            chkSaveNewBatch.CheckedChanged +=
        
        }

        private void optRadioButtons_CheckChanged(object sender, EventArgs e) 
        {
            if (!((RadioButton)sender).Checked) return;

            if (optSelectVisits.Checked)
            {
                cHAMP_VisitsDataGridView.Enabled = true;
                chkSaveNewBatch.Enabled = true;
            }
            else
            {
                cHAMP_VisitsDataGridView.Enabled = false;
                chkSaveNewBatch.Enabled = false;
            }
        }

        private void chkSaveBatch_CheckChanged(object sender, EventArgs e)
            
    }
}

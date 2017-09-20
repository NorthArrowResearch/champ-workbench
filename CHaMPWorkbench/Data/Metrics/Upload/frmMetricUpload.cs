using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data.Metrics.Upload
{
    public partial class frmMetricUpload : Form
    {
        public frmMetricUpload()
        {
            InitializeComponent();
        }

        private void frmMetricUpload_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            if (!ucBatch.Validate())
                return false;

            return true;
        }

        private void cboOK_Click(object sender, EventArgs e)
        {
            try
            {
                MetricUploader uploader = new MetricUploader();
                uploader.Run(ucBatch.SelectedBatches);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}

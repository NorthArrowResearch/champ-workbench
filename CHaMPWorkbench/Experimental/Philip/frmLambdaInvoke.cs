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
    public partial class frmLambdaInvoke : Form
    {
        List<CHaMPData.VisitBasic> Visits;
        LambdaInvoker invoker;

        public frmLambdaInvoke(List<CHaMPData.VisitBasic> lVisits)
        {
            InitializeComponent();
            Visits = lVisits;
        }

        private void frmLambdaInvoke_Load(object sender, EventArgs e)
        {
            txtSelectedVisits.Text = Visits.Count.ToString("#,##0");
            string sqlCombo = "SELECT ItemID, Title FROM LookupListItems WHERE ListID = {0} ORDER BY Title";
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboQueue, naru.db.sqlite.DBCon.ConnectionString, string.Format(sqlCombo, 15));
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboFunction, naru.db.sqlite.DBCon.ConnectionString, string.Format(sqlCombo, 16));
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboBucket, naru.db.sqlite.DBCon.ConnectionString, string.Format(sqlCombo, 17), 10029);
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboTool, naru.db.sqlite.DBCon.ConnectionString, string.Format(sqlCombo, 18));

            // The function and bucket are read only for now
            cboFunction.SelectedIndex = 0;

            cmdCancel.DialogResult = DialogResult.Cancel;

            tTip.SetToolTip(txtSelectedVisits, "The number of visits that will be processed. Return the Workbench main grid to change this selection.");
            tTip.SetToolTip(cboTool, "The name of the CHaMP automation tool that will be run. Topo metrics or validation etc. Re-run this tool multiple times to execute multiple tools.");
            tTip.SetToolTip(cboQueue, "The name of the Amazon Simple Queue Service (SQS) queue where the job will be placed. This affects whether the process occurs on production, QA. It can also be used to steer long running processes onto the correct resources.");
            tTip.SetToolTip(cboFunction, "The Amazon Lambda function that will be added to the queue.");
            tTip.SetToolTip(cboBucket, "The Amazon Simple Scalable Storage (S3) bucket where the results will be placed.");
            tTip.SetToolTip(cmdSQS, "Launch a web browser at the appropriate Amazon Simple Queue Service (SQS) queue.");
            tTip.SetToolTip(cmdCloudWatch, "Launch a web browser at Amazon Cloudwatch service.");
        }

        private bool ValidateForm()
        {
            bool bStatus = false;

            if (VerifyCombo(ref cboTool, "Tool"))
                if (VerifyCombo(ref cboQueue, "Queue"))
                    if (VerifyCombo(ref cboFunction, "Function"))
                        if (VerifyCombo(ref cboBucket, "Bucket"))
                            bStatus = true;

            return bStatus;
        }

        private bool VerifyCombo(ref ComboBox cbo, string sNoun)
        {
            if (cbo.SelectedIndex < 0)
            {
                MessageBox.Show(string.Format("You must select a valid {0}.", sNoun.ToLower()), string.Format("Missing {0}", sNoun), MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo.Select();
                return false;
            }
            else
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
                cmdCancel.Text = "Cancel";
                cmdCancel.DialogResult = DialogResult.None;
                invoker = new LambdaInvoker(ref bgWorker, cboTool.Text, cboQueue.Text, cboFunction.Text, cboBucket.Text);
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }

            lblProgress.Visible = true;
            pgr.Visible = true;
            pgr.Value = 0;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                invoker.Run(Visits);
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgr.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string sPrefix = string.Empty;

            if (e.Cancelled)
                sPrefix = "User cancelled the operation. ";

            if (invoker.SuccessVisits.Count == Visits.Count && invoker.ErrorVisits.Count == 0)
                MessageBox.Show(string.Format("{1}All {0} visits were successfully invoked using Lambda.", invoker.SuccessVisits.Count, sPrefix), "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(string.Format("{0}{1} visits were invoked successfully, while {2} visits experienced errors.", sPrefix, invoker.SuccessVisits.Count, invoker.ErrorVisits.Count), "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            cmdCancel.Text = "Close";
            cmdCancel.DialogResult = DialogResult.Cancel;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
        }

        private void cmdSQS_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://console.aws.amazon.com/sqs/home?region=us-west-2");
        }

        private void cmdCloudWatch_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://us-west-2.console.aws.amazon.com/cloudwatch/home?region=us-west-2#");
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            CHaMPWorkbench.OnlineHelp.FormHelp(this.Name);
        }
    }
}

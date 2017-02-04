using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using System.IO;
using naru.db.sqlite;

namespace CHaMPWorkbench.Habitat
{
    public partial class frmScavengeHabitatResults : Form
    {
        private Classes.ResultScavengerBatch m_scavenger;

        public frmScavengeHabitatResults()
        {
            InitializeComponent();
        }

        private void frmScavengeHabitatResults_Load(object sender, EventArgs e)
        {
            // Only set the UI to the GUTPy path if it is valid.
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.Habitat_Project_Root) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.Habitat_Project_Root))
                txtHabitatModelFolder.Text = CHaMPWorkbench.Properties.Settings.Default.Habitat_Project_Root;
            else
            {
                txtHabitatModelFolder.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
            }

            if (rdoDB.Checked)
                panelCSV.Visible = false;
            else
                panelCSV.Visible = true;
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://habitat.northarrowresearch.com/wiki/Technical_Reference/results.html");
        }


        private bool ProcessCSVFile(string sHabitatProject, string sCSVFile, out string sMessage)
        {
            dsHabitat theHabitatProject = new dsHabitat();
            theHabitatProject.ReadXml(sHabitatProject);

            if (theHabitatProject.SimulationResults.Rows.Count < 1)
            {
                sMessage = "The habitat project file does not contain any simulation results.";
                return false;
            }

            SortedDictionary<int, string> dTypes = new SortedDictionary<int, string>();
            foreach (dsHabitat.SimulationResultTypesRow rType in theHabitatProject.SimulationResultTypes.Rows)
                dTypes.Add(rType.ResultTypeID, rType.Title);

            Dictionary<string, SimulationResults> dResults = new Dictionary<string, SimulationResults>();
            foreach (dsHabitat.SimulationResultsRow rResult in theHabitatProject.SimulationResults.Rows)
            {
                if (dResults.ContainsKey(rResult.SimulationsRow.Title))
                    dResults[rResult.SimulationsRow.Title].AddResult(rResult.ResultTypeID, rResult.ResultValue);
                else
                    dResults.Add(rResult.SimulationsRow.Title, new SimulationResults(ref dTypes, rResult.SimulationsRow.Title, rResult.ResultTypeID, rResult.ResultValue));
            }

            using (System.IO.StreamWriter wCSV = new System.IO.StreamWriter(txtCSVFile.Text))
            {
                wCSV.Write("Simulation,");
                wCSV.WriteLine(string.Join(",", dTypes.Values.ToArray<string>()));

                foreach (SimulationResults aSimulation in dResults.Values)
                    wCSV.WriteLine(aSimulation.ToString());
            }

            sMessage = string.Format("Process completed successfully. {0} result types written for {1} simulation(s) written to file. Do you want to open and view the exported file?", dTypes.Count, dResults.Count);
            return true;
        }

        private void cmdBrowseProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Title = "Habitat Model Project Database";
            frm.Filter = "Habitat Model Databases (*.xml)|*.xml";
            frm.CheckFileExists = true;

            if (!string.IsNullOrWhiteSpace(txtHabitatModelFolder.Text) && System.IO.File.Exists(txtHabitatModelFolder.Text))
            {
                frm.InitialDirectory = System.IO.Path.GetDirectoryName(txtHabitatModelFolder.Text);
                frm.FileName = System.IO.Path.GetFileNameWithoutExtension(txtHabitatModelFolder.Text);
            }

            Classes.InputFileBuilder_Helper.BrowseFolder("Habitat Model Project Root Folder", "Select the top level folder that contains the habitat data.", ref txtHabitatModelFolder);

            if (!string.IsNullOrEmpty(txtHabitatModelFolder.Text) && System.IO.Directory.Exists(txtHabitatModelFolder.Text))
                CHaMPWorkbench.Properties.Settings.Default.Habitat_Project_Root = txtHabitatModelFolder.Text;
        }

        private void cmdBrowseCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Output CSV File";
            frm.Filter = "CSV Text Files (*.csv)|*.csv";
            frm.AddExtension = true;
            frm.OverwritePrompt = true;

            if (!string.IsNullOrWhiteSpace(txtCSVFile.Text) && System.IO.File.Exists(txtCSVFile.Text))
            {
                frm.InitialDirectory = System.IO.Path.GetDirectoryName(txtCSVFile.Text);
                frm.FileName = System.IO.Path.GetFileNameWithoutExtension(txtCSVFile.Text);
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtCSVFile.Text = frm.FileName;
        }


        private class SimulationResults
        {
            private string m_sSimulationName;
            private SortedDictionary<int, Nullable<Single>> m_dResults;

            public SimulationResults(ref  SortedDictionary<int, string> dResultTypes, string sSimulation, int nResultType, Single fValue)
            {
                m_sSimulationName = sSimulation;
                m_dResults = new SortedDictionary<int, Nullable<Single>>();
                m_dResults[nResultType] = fValue;

                // Ensure that the simulation has a record for each result type.
                // This is important so that all the columns in the CSV have values.
                // i.e. Some simulation types do not write results in the project
                // file for all result types.
                foreach (int nResultTypeID in dResultTypes.Keys)
                {
                    if (!m_dResults.ContainsKey(nResultTypeID))
                        m_dResults.Add(nResultTypeID,new Nullable<Single>());
                }
            }

            public void AddResult(int nResultType, Single fValue)
            {
                m_dResults[nResultType] = fValue;
            }

            public override string ToString()
            {
                string sResults = m_sSimulationName;
                foreach (Nullable<Single> fValue in m_dResults.Values)
                {
                    if (fValue.HasValue)
                        sResults += string.Format(",{0}", fValue);
                    else
                        sResults += ",";
                }

                return sResults;
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDB.Checked)
                panelCSV.Visible = false;
            else
                panelCSV.Visible = true;
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHabitatModelFolder.Text) || !System.IO.Directory.Exists(txtHabitatModelFolder.Text))
            {
                System.Windows.Forms.MessageBox.Show("You must choose a valid habitat project folder to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            
            if (rdoCSV.Checked && string.IsNullOrEmpty(txtCSVFile.Text))
            {
                System.Windows.Forms.MessageBox.Show("You must specify a CSV output file to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (rdoCSV.Checked)
            {
                using (dsHabitat theHabitatProject = new dsHabitat())
                {
                    try
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                        string sMessage = string.Empty;

                        bool bOK = ProcessCSVFile(txtHabitatModelFolder.Text, txtCSVFile.Text, out sMessage);
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                        if (MessageBox.Show(sMessage, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                            System.Diagnostics.Process.Start(txtCSVFile.Text);

                        if (!bOK)
                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                        Classes.ExceptionHandling.NARException.HandleException(ex);
                    }
                }
            }
            else if (rdoDB.Checked)
            {
                try
                {
                    // TODO: Philip, or someone with ACCESS needs to implement this.
                    //scavenger.ScavengeLogFile(m_dbCon.ConnectionString, nResultID, aNode.InnerText, sResultFile);
                    m_scavenger = new Classes.ResultScavengerBatch(txtHabitatModelFolder.Text, "HSResults*.xml", true, false, "");

                    prgBar.Visible = true;
                    cmdRun.Enabled = false;
                    cmdStop.Enabled = true;
                    cmdClose.Enabled = false;
                    BackgroundWorker1.WorkerReportsProgress = true;
                    BackgroundWorker1.WorkerSupportsCancellation = true;
                    BackgroundWorker1.RunWorkerAsync();


                }
                catch (Exception ex)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation. 
            this.BackgroundWorker1.CancelAsync();
            // Disable the Cancel button.
            //cmdRun.Enabled = true;
            //cmdStop.Enabled = false;
            //cmdCloseCancel.Enabled = true;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker object that raised this event. 
            BackgroundWorker worker = (BackgroundWorker)sender;

            // Assign the result of the computation 
            // to the Result property of the DoWorkEventArgs 
            // object. This is will be available to the  
            // RunWorkerCompleted eventhandler.

            List<Exception> Errors = new List<Exception>();
            int nProcessed = 0;
            string[] sResultFiles = Directory.GetFiles(txtHabitatModelFolder.Text, "HSResults.xml", SearchOption.AllDirectories);
                        
            //ResultScavengerSingle scavenger = new ResultScavengerSingle(ref m_dbCon);
            HabitatResultsScavenger scavengerHabitat = new HabitatResultsScavenger(DBCon.ConnectionString);

            for (int i = 0; i < sResultFiles.Count(); i++)
            {
                try
                {
                    scavengerHabitat.ScavengeHSOutputFile(sResultFiles[i]);
                }
                catch (Exception ex)
                {
                    //
                    // these are legimitate Habitat XML result files that have errors. Add them
                    // to the running list of problems and continue with next file.
                    //
                    Errors.Add(ex);
                }

                if (i < 1 || sResultFiles.Length < 1)
                    BackgroundWorker1.ReportProgress(0);
                else
                    BackgroundWorker1.ReportProgress((i) * 100 / sResultFiles.Length);

                if (BackgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    // User clicked cancel on the user interace.
                    e.Result = (sResultFiles.Count() - Errors.Count());

                }
            }

            e.Result = (sResultFiles.Count() - Errors.Count());

        }



        // This event handler updates the progress. 
        private void backgroundWorker1_ProgressChanged(System.Object sender, ProgressChangedEventArgs e)
        {

            //resultLabel.Text = (e.ProgressPercentage.ToString() + "%")
            prgBar.Value = e.ProgressPercentage;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown. 
            if ((e.Error != null))
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled the  
                // operation. 
                // Note that due to a race condition in  
                // the DoWork event handler, the Cancelled 
                // flag may not have been set, even though 
                // CancelAsync was called.
                //lblProgress.Text = "Canceled"
                MessageBox.Show("Processed cancelled by user.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if ((e.Error != null))
            {
                MessageBox.Show(e.Error.Message, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Finally, handle the case where the operation succeeded.
                //lblProgress.Text = e.Result.ToString()
                cmdClose.Focus();
                MessageBox.Show("Completed sucessfully", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            prgBar.Value = 100;

            // Disable the Cancel button.
            cmdRun.Enabled = true;
            cmdStop.Enabled = false;
            cmdClose.Enabled = true;
        }



    }
}

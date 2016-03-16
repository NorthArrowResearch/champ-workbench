using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Collections.Specialized;

namespace CHaMPWorkbench.GUT
{
    public partial class frmGUTRun : Form
    {
        private string DBCon;
        private List<QueuedRun> QueuedRuns;

        /// <summary>
        /// Create a new GUT run form.
        /// </summary>
        /// <param name="sDBCon">Workbench database connection string</param>
        public frmGUTRun(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;
        }

        private void frmGUTRun_Load(object sender, EventArgs e)
        {
            // Only set the UI to the GUTPy path if it is valid.
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.GUTPythonPath) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.GUTPythonPath))
                txtPyGUT.Text = CHaMPWorkbench.Properties.Settings.Default.GUTPythonPath;

            // Only set the UI to the Python path if it is valid.
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.Model_Python) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.Model_Python))
                txtPython.Text = CHaMPWorkbench.Properties.Settings.Default.Model_Python;

            // Add the GUT modes to the dropdown. These MUST be the actual mode arguments
            // with spaces and any desireable capitalization.
            cboModes.Items.Add("Tier 2");
            cboModes.Items.Add("Tier 3");
            cboModes.Items.Add("Evidence");
            cboModes.SelectedIndex = cboModes.Items.Add("All");

            // Default the DOS window that will appear to either normal (visible) or hidden.
            int nHidden = cboWindowStyle.Items.Add(new ListItem("Hidden", (int)System.Diagnostics.ProcessWindowStyle.Hidden));
            int nNormal = cboWindowStyle.Items.Add(new ListItem("Normal", (int)System.Diagnostics.ProcessWindowStyle.Normal));
            cboWindowStyle.SelectedIndex = nHidden;

            cmdOK.Select();

            try
            {
                // Load a list of the GUT runs that are queued to run.
                // Note that below the input files are only loaded if the input file exists on disk.
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    QueuedRuns = new List<QueuedRun>();
                    OleDbCommand dbCom = new OleDbCommand("SELECT BatchID, PrimaryVisitID, InputFile FROM Model_Batches WHERE (ModelTypeID = @ModelTypeID) AND (Run <> 0)", dbCon);
                    dbCom.Parameters.AddWithValue("ModelTypeID", CHaMPWorkbench.Properties.Settings.Default.ModelType_GUT);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string sInputFile = dbRead.GetString(dbRead.GetOrdinal("InputFile"));
                        if (System.IO.File.Exists(sInputFile))
                            QueuedRuns.Add(new QueuedRun(dbRead.GetInt32(dbRead.GetOrdinal("BatchID")), dbRead.GetInt32(dbRead.GetOrdinal("PrimaryVisitID")), sInputFile));
                    }

                    // Check that at least one run is loaded with a valid input file.
                    // This isn't essential, but it's a nice courtesy to warn the user as soon as the form opens.
                    WarnEmptyRuns();
                }
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }

        /// <summary>
        /// Check that at least one input file is loaded.
        /// </summary>
        /// <returns>True when more than one input XML file is loaded and queued.</returns>
        /// <remarks>Note that this method is here because it is called in two places.</remarks>
        private bool WarnEmptyRuns()
        {
            bool bResult = true;
            if (QueuedRuns == null || QueuedRuns.Count < 1)
            {
                MessageBox.Show("There are no model runs queued and that have valid input XML files. Use the input file builder to create batches of runs and then use the select form to queue model runs.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Check that all inputs are valid.
        /// </summary>
        /// <returns>True when all inputs are valid and the tool can proceed.</returns>
        private bool ValidateForm()
        {
            if (!WarnEmptyRuns())
                return false;

            if (string.IsNullOrEmpty(txtPyGUT.Text) || !System.IO.File.Exists(txtPyGUT.Text))
            {
                MessageBox.Show("You must specify the path to the PyGUT Python script.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowsePyGUT.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtPython.Text) || !System.IO.File.Exists(txtPython.Text))
            {
                MessageBox.Show("You must specify the path to the Python executable.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowsePython.Select();
                return false;
            }

            return true;
        }

        private void cmdBrowsePyGUT_Click(object sender, EventArgs e)
        {
            frmOptions.BrowseExecutable("Geomorphic Unit Tool (GUT) Python Script", "Python Scripts (*.py)|*.py", ref dlgBrowseExecutable, ref txtPyGUT);
        }

        private void cmdBrowsePython_Click(object sender, EventArgs e)
        {
            frmOptions.BrowseExecutable("Python", "Executables (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtPython);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                // Prepare a database command to update the batches and unqueue them when they are complete.
                OleDbCommand dbCom = new OleDbCommand("UPDATE Model_BatchRuns SET Run = 0, DateTimeCompleted = Now() WHERE BatchID = @BatchID", dbCon);
                OleDbParameter pBatchID = dbCom.Parameters.Add("@BatchID", OleDbType.Integer);

                // Loop over all the queued runs.
                foreach (QueuedRun aRun in QueuedRuns)
                {
                    try
                    {
                        System.Diagnostics.ProcessWindowStyle eWindow = (System.Diagnostics.ProcessWindowStyle)((ListItem)cboWindowStyle.SelectedItem).Value;
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                        string sGUTMode = cboModes.SelectedItem.ToString().ToLower().Replace(" ", "");

                        Console.WriteLine("\n******************************************************************************");

                        // http://gis.stackexchange.com/questions/108230/arcgis-geoprocessing-and-32-64-bit-architecture-issue/108788#108788
                        ProcessStartInfo psi = new ProcessStartInfo(txtPython.Text);
                        if (CHaMPWorkbench.Properties.Settings.Default.RBTPathVariableActive)
                        {
                            if (!String.IsNullOrWhiteSpace(CHaMPWorkbench.Properties.Settings.Default.RBTPathVariable))
                            {
                                StringDictionary dictionary = psi.EnvironmentVariables;
                                psi.EnvironmentVariables["PATH"] = "C:\\Python27\\ArcGISx6410.1;C:\\Python27\\ArcGISx6410.1\\DLLs";
                                psi.UseShellExecute = false;
                                psi.Arguments = string.Format("{0} {1}", aRun.InputFile, sGUTMode);
                                psi.WindowStyle = eWindow;
                            }
                        }
                        System.Diagnostics.Process proc = new Process();

                        proc.StartInfo = psi;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.WindowStyle = eWindow;

                        if (eWindow == System.Diagnostics.ProcessWindowStyle.Hidden)
                            proc.StartInfo.CreateNoWindow = true;

                        proc.Start();
                        proc.WaitForExit();

                        // Update the batch and set it to no longer queued. Also store the completed date time.
                        pBatchID.Value = aRun.BatchRunID;
                        dbCom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //TODO: what happens when a Python call crashes.
                    }
                    finally
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    }
                }
            }
        }

        /// <summary>
        /// Private class that represents a queued model run.
        /// </summary>
        private class QueuedRun
        {
            public int BatchRunID { get; internal set; }
            public int VisitID { get; internal set; }
            public string InputFile { get; internal set; }

            /// <summary>
            /// Create a new model run
            /// </summary>
            /// <param name="nBatchRunID">Workbench Batch run ID</param>
            /// <param name="nVisitID">CHaMP Visit ID</param>
            /// <param name="sInputFile">Model input XML file</param>
            public QueuedRun(int nBatchRunID, int nVisitID, string sInputFile)
            {
                BatchRunID = nBatchRunID;
                VisitID = nVisitID;
                InputFile = sInputFile;
            }
        }
    }
}

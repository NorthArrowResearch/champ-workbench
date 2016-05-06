﻿using System;
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
using System.Threading;

namespace CHaMPWorkbench.HydroPrep
{
    public partial class frmHydroPrepRun : Form
    {
        private string DBCon;
        private List<QueuedRun> QueuedRuns;

        /// <summary>
        /// Create a new hydro prep run form.
        /// </summary>
        /// <param name="sDBCon">Workbench database connection string</param>
        public frmHydroPrepRun(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;
        }

        private void frmRun_Load(object sender, EventArgs e)
        {
            // Only set the UI to the path if it is valid.
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep))
                txtExecutablePath.Text = CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep;

            // Default the DOS window that will appear to either normal (visible) or hidden.
            int nHidden = cboWindowStyle.Items.Add(new ListItem("Hidden", (int)System.Diagnostics.ProcessWindowStyle.Hidden));
            int nNormal = cboWindowStyle.Items.Add(new ListItem("Normal", (int)System.Diagnostics.ProcessWindowStyle.Normal));
            cboWindowStyle.SelectedIndex = nHidden;

            cmdOK.Select();

            try
            {
                // Load a list of the runs that are queued to run.
                // Note that below the input files are only loaded if the input file exists on disk.
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    QueuedRuns = new List<QueuedRun>();
                    OleDbCommand dbCom = new OleDbCommand("SELECT ID, PrimaryVisitID, InputFile FROM Model_BatchRuns WHERE (ModelTypeID = @ModelTypeID) AND (Run <> 0) ORDER BY Priority", dbCon);
                    dbCom.Parameters.AddWithValue("ModelTypeID", CHaMPWorkbench.Properties.Settings.Default.ModelType_HydroPrep);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string sInputFile = dbRead.GetString(dbRead.GetOrdinal("InputFile"));
                        if (System.IO.File.Exists(sInputFile))
                            QueuedRuns.Add(new QueuedRun(dbRead.GetInt32(dbRead.GetOrdinal("ID")), dbRead.GetInt32(dbRead.GetOrdinal("PrimaryVisitID")), sInputFile));
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

            if (string.IsNullOrEmpty(txtExecutablePath.Text) || !System.IO.File.Exists(txtExecutablePath.Text))
            {
                MessageBox.Show("You must specify the path to the hydraulic model software executable.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowseExecutable.Select();
                return false;
            }

            return true;
        }

        private void cmdBrowseHydroPrep_Click(object sender, EventArgs e)
        {
            frmOptions.BrowseExecutable("Hydraulic Model Preparation Software Executable", "Executables (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtExecutablePath);
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
                OleDbCommand dbCom = new OleDbCommand("UPDATE Model_BatchRuns SET Run = 0, DateTimeCompleted = Now() WHERE ID = @ID", dbCon);
                OleDbParameter pBatchID = dbCom.Parameters.Add("@ID", OleDbType.Integer);

                // Loop over all the queued runs.
                foreach (QueuedRun aRun in QueuedRuns)
                {
                    try
                    {
                        System.Diagnostics.ProcessWindowStyle eWindow = (System.Diagnostics.ProcessWindowStyle)((ListItem)cboWindowStyle.SelectedItem).Value;
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                        Console.WriteLine("\n******************************************************************************");

                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.FileName = txtExecutablePath.Text;
                        proc.StartInfo.Arguments = aRun.InputFile;

                        gutOutput.AppendText(String.Format("Running: {0} {1} {2}", Environment.NewLine, txtExecutablePath.Text, proc.StartInfo.Arguments));

                        System.Diagnostics.Process.Start(proc.StartInfo);

                        // Update the batch and set it to no longer queued. Also store the completed date time.
                        pBatchID.Value = aRun.BatchRunID;
                        dbCom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //Classes.ExceptionHandling.NARException.HandleException(ex);
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

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
using System.Threading;

namespace CHaMPWorkbench.Habitat
{
    public partial class frmHabitatRun : Form
    {
        private string DBCon;
        private string m_sOutputXML;
        private string m_sProjectXML;
        /// <summary>
        /// Create a new GUT run form.
        /// </summary>
        /// <param name="sDBCon">Workbench database connection string</param>
        public frmHabitatRun(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;
        }

        private void frmGUTRun_Load(object sender, EventArgs e)
        {
            // Only set the UI to the GUTPy path if it is valid.
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.Habitat_Project) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.Habitat_Project))
                txtHabitatProjectXML.Text = CHaMPWorkbench.Properties.Settings.Default.Habitat_Project;

            // Default the DOS window that will appear to either normal (visible) or hidden.
            int nHidden = cboWindowStyle.Items.Add(new ListItem("Hidden", (int)System.Diagnostics.ProcessWindowStyle.Hidden));
            int nNormal = cboWindowStyle.Items.Add(new ListItem("Normal", (int)System.Diagnostics.ProcessWindowStyle.Normal));
            cboWindowStyle.SelectedIndex = nNormal;

            cmdOK.Select();
        }

        /// <summary>
        /// Check that all inputs are valid.
        /// </summary>
        /// <returns>True when all inputs are valid and the tool can proceed.</returns>
        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(txtHabitatProjectXML.Text) || !System.IO.File.Exists(txtHabitatProjectXML.Text))
            {
                MessageBox.Show("You must specify the path to the Habitat Project XML.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowseProjectXML.Select();
                return false;
            }

            if (m_sOutputXML.Trim() == m_sProjectXML.Trim())
            {
                MessageBox.Show("The Project file has the same name as the output we are trying to write.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdBrowseProjectXML.Select();
                return false;
            }

            return true;
        }

        private void cmdBrowseProjectXML_Click(object sender, EventArgs e)
        {
            frmOptions.BrowseExecutable("Habitat Project XML", "Python Scripts (*.xml)|*.xml", ref dlgBrowseExecutable, ref txtHabitatProjectXML);
        }


        private void cmdOK_Click(object sender, EventArgs e)
        {
            String m_sProjectXML = txtHabitatProjectXML.Text;
            String sProjetXMLNameNoExt = System.IO.Path.GetFileNameWithoutExtension(m_sProjectXML);
            String sProjectRoot = System.IO.Path.GetDirectoryName(m_sProjectXML);
            String m_sOutputXML = System.IO.Path.Combine(sProjectRoot, sProjetXMLNameNoExt + "_output.xml");
            String sHabitatExe = CHaMPWorkbench.Properties.Settings.Default.Model_HabitatConsole;
            String sHabitatExeRoot = System.IO.Path.GetDirectoryName(sHabitatExe);

            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                try
                {
                    System.Diagnostics.ProcessWindowStyle eWindow = (System.Diagnostics.ProcessWindowStyle)((ListItem)cboWindowStyle.SelectedItem).Value;
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                    CHaMPWorkbench.Properties.Settings.Default.Habitat_Results = m_sOutputXML;

                    // http://gis.stackexchange.com/questions/108230/arcgis-geoprocessing-and-32-64-bit-architecture-issue/108788#108788
                    ProcessStartInfo psi = new ProcessStartInfo();
                    if (!string.IsNullOrEmpty(sHabitatExe.Trim()))
                    {
                        psi.FileName = CHaMPWorkbench.Properties.Settings.Default.Model_HabitatConsole;
                        // It goes: root def con output
                        psi.WorkingDirectory = sHabitatExeRoot;
                        psi.Arguments = string.Format("{0} {1} {2} {3}", sProjectRoot, m_sProjectXML, m_sProjectXML, m_sOutputXML);
                        psi.CreateNoWindow = false;
                        psi.UseShellExecute = true;
                        psi.RedirectStandardOutput = false;
                        psi.RedirectStandardError = false;
                    }

                    HabitatOutput.AppendText(String.Format("Running: {0}  {1} {2}", Environment.NewLine, sHabitatExe, psi.Arguments));

                    System.Diagnostics.Process proc = new Process();
                    proc.StartInfo = psi;

                    proc.Start();
                    //System.IO.StreamReader stdErr = proc.StandardError;
                    proc.WaitForExit();
                    if (proc.ExitCode != 0)
                    {
                        Exception ex = new Exception("Console Error");
                        ex.Data["Console path"] = sHabitatExe;
                        ex.Data["Project path"] = m_sProjectXML;
                        ex.Data["Output Path"] = m_sOutputXML;
                        ex.Data["Params"] = psi.Arguments;
                        //ex.Data["Standard Error"] = stdErr.ReadToEnd();
                        throw ex;
                    }

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
}

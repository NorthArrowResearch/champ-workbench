using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Habitat
{
    public partial class frmScavengeHabitatResults : Form
    {
        public frmScavengeHabitatResults()
        {
            InitializeComponent();
        }

        private void frmScavengeHabitatResults_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.Habitat_Results) && System.IO.File.Exists(CHaMPWorkbench.Properties.Settings.Default.Habitat_Results))
                txtHabitatModelDB.Text = CHaMPWorkbench.Properties.Settings.Default.Habitat_Results;

            if (rdoDB.Checked)
                panelCSV.Visible = false;
            else
                panelCSV.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://habitat.northarrowresearch.com/wiki/Technical_Reference/results.html");
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHabitatModelDB.Text) || !System.IO.File.Exists(txtHabitatModelDB.Text))
            {
                System.Windows.Forms.MessageBox.Show("You must choose a valid habitat project file to continue.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        bool bOK = ProcessFile(txtHabitatModelDB.Text, txtCSVFile.Text, out sMessage);
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
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private bool ProcessFile(string sHabitatProject, string sCSVFile, out string sMessage)
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

            if (!string.IsNullOrWhiteSpace(txtHabitatModelDB.Text) && System.IO.File.Exists(txtHabitatModelDB.Text))
            {
                frm.InitialDirectory = System.IO.Path.GetDirectoryName(txtHabitatModelDB.Text);
                frm.FileName = System.IO.Path.GetFileNameWithoutExtension(txtHabitatModelDB.Text);
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtHabitatModelDB.Text = frm.FileName;
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

    }
}

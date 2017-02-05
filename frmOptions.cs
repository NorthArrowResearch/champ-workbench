using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            txtOptions.Text = CHaMPWorkbench.Properties.Settings.Default.RBTConsole;
            txtGUT.Text = CHaMPWorkbench.Properties.Settings.Default.GUTPythonPath;
            txtPython.Text = CHaMPWorkbench.Properties.Settings.Default.Model_Python;
            txtHydroPrep.Text = CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep;
            txtHabitatConsole.Text = CHaMPWorkbench.Properties.Settings.Default.Model_HabitatConsole;

            txtMonitoring.Text = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;
            txtOutput.Text = CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder;
            txtTemp.Text = CHaMPWorkbench.Properties.Settings.Default.LastTempFolder;
            txtMonitoringDataZipped.Text = CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder;
            txtUserName.Text = CHaMPWorkbench.Properties.Settings.Default.DefaultUserName;

            valGoogleMapZoom.Value = (decimal)CHaMPWorkbench.Properties.Settings.Default.GoogleMapZoom;

            tTip.SetToolTip(txtOptions, "The path to the RBT console executable (rbtconsole.exe) that will be used when the RBT is run.");

            tTip.SetToolTip(txtMonitoring, "The top level folder containing the CHaMP survey data. Under this folder there should be a folder for each field season, then watershed etc");
            tTip.SetToolTip(txtOutput, "The top level folder containing the RBT input and output files and results. Under this folder there should be a folder for each field season, then watershed etc");

#if DEBUG
            cmdTestAWS.Visible = true;
#endif

            if (Classes.AWSCloudWatch.AWSCloudWatchSingleton.HasInstallationGUID)
                txtStreamName.Text = Classes.AWSCloudWatch.AWSCloudWatchSingleton.Instance.InstallationGUID.ToString();

            chkAWSLoggingEnabled.Checked = CHaMPWorkbench.Properties.Settings.Default.AWSLoggingEnabled;

            try
            {
                dtStart.Value = CHaMPWorkbench.Properties.Settings.Default.HydroGraphStart;
            }
            catch (Exception ex)
            {
                dtStart.Value = new DateTime(2011, 1, 1);
                Console.WriteLine(ex.Message);
            }

            try
            {
                dtEnd.Value = CHaMPWorkbench.Properties.Settings.Default.HydroGraphEnd;
            }
            catch (Exception ex)
            {
                dtEnd.Value = new DateTime(DateTime.Now.Year, 1, 1);
                Console.WriteLine(ex.Message);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtOptions.Text))
            {
                if (System.IO.File.Exists(txtOptions.Text) && txtOptions.Text.EndsWith(".exe"))
                    CHaMPWorkbench.Properties.Settings.Default.RBTConsole = txtOptions.Text;
                else
                {
                    MessageBox.Show("The RBT Console software path must point to the RBT executable file (rbtconsole.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (!String.IsNullOrWhiteSpace(txtGUT.Text))
            {
                if (System.IO.File.Exists(txtGUT.Text) && txtGUT.Text.EndsWith(".py"))
                    CHaMPWorkbench.Properties.Settings.Default.GUTPythonPath = txtGUT.Text;
                else
                {
                    MessageBox.Show("The GUT python script path must point to a Python file (e.g. C:\\CHaMP\\GUT\\gut.py)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtPython.Text))
            {
                CHaMPWorkbench.Properties.Settings.Default.Model_Python = string.Empty;
            }
            else
            {
                if (System.IO.File.Exists(txtPython.Text) && txtPython.Text.ToLower().EndsWith(".exe"))
                    CHaMPWorkbench.Properties.Settings.Default.Model_Python = txtPython.Text;
                else
                {
                    MessageBox.Show("The python path must point to the Python scripting language executable file (e.g. C:\\Python\\Python.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtHydroPrep.Text))
            {
                CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep = string.Empty;
            }
            else
            {
                if (System.IO.File.Exists(txtHydroPrep.Text) && txtHydroPrep.Text.ToLower().EndsWith(".exe"))
                    CHaMPWorkbench.Properties.Settings.Default.Model_HydroPrep = txtHydroPrep.Text;
                else
                {
                    MessageBox.Show("The hydraulic model preparation path must point to an executable file (e.g. C:\\CHaMP\\HydroPrep\\HydroPrep.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtHabitatConsole.Text))
            {
                CHaMPWorkbench.Properties.Settings.Default.Model_HabitatConsole = string.Empty;
            }
            else
            {
                if (System.IO.File.Exists(txtHabitatConsole.Text) && txtHabitatConsole.Text.ToLower().EndsWith(".exe"))
                    CHaMPWorkbench.Properties.Settings.Default.Model_HabitatConsole = txtHabitatConsole.Text;
                else
                {
                    MessageBox.Show("The habitat console path must point to an executable file (e.g. C:\\CHaMP\\HydroPrep\\HydroPrep.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }

            if (!String.IsNullOrWhiteSpace(txtMonitoring.Text) && System.IO.Directory.Exists(txtMonitoring.Text))
                CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder = txtMonitoring.Text;
            else
                CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder = string.Empty;

            if (!String.IsNullOrWhiteSpace(txtOutput.Text) && System.IO.Directory.Exists(txtOutput.Text))
                CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder = txtOutput.Text;
            else
                CHaMPWorkbench.Properties.Settings.Default.InputOutputFolder = string.Empty;

            if (!String.IsNullOrWhiteSpace(txtTemp.Text) && System.IO.Directory.Exists(txtTemp.Text))
                CHaMPWorkbench.Properties.Settings.Default.LastTempFolder = txtTemp.Text;
            else
                CHaMPWorkbench.Properties.Settings.Default.LastTempFolder = string.Empty;

            if (!String.IsNullOrWhiteSpace(txtMonitoringDataZipped.Text) && System.IO.Directory.Exists(txtMonitoringDataZipped.Text))
                CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder = txtMonitoringDataZipped.Text;
            else
                CHaMPWorkbench.Properties.Settings.Default.ZippedMonitoringDataFolder = string.Empty;
            
            CHaMPWorkbench.Properties.Settings.Default.GoogleMapZoom = (byte)valGoogleMapZoom.Value;

            try
            {
                ucPrograms1.Save();
                CHaMPWorkbench.Properties.Settings.Default.AWSCloudWatchGUID = Classes.AWSCloudWatch.AWSCloudWatchSingleton.Instance.InstallationGUID;
                CHaMPWorkbench.Properties.Settings.Default.AWSLoggingEnabled = chkAWSLoggingEnabled.Checked;
            }
            catch (Exception ex)
            {
                Exception exOuter = new Exception("Error saving software settings.", ex);
                Classes.ExceptionHandling.NARException.HandleException(exOuter);
            }

            CHaMPWorkbench.Properties.Settings.Default.DefaultUserName = txtUserName.Text;

            CHaMPWorkbench.Properties.Settings.Default.Save();
        }

        private void cmdBrowseRBT_Click(object sender, EventArgs e)
        {
            BrowseExecutable("RBT Console Executable", "Executable Files (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtOptions);
        }

        public static void BrowseExecutable(string sTitle, string sFileTypeFilter, ref OpenFileDialog dlgBrowseExecutable, ref TextBox txt)
        {
            dlgBrowseExecutable.Filter = sFileTypeFilter;
            dlgBrowseExecutable.Title = sTitle;
            if (!String.IsNullOrWhiteSpace(txt.Text) && System.IO.File.Exists(txt.Text))
            {
                dlgBrowseExecutable.InitialDirectory = System.IO.Path.GetDirectoryName(txt.Text);
                dlgBrowseExecutable.FileName = System.IO.Path.GetFileName(txt.Text);
            }

            if (dlgBrowseExecutable.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txt.Text = dlgBrowseExecutable.FileName;
        }

        private void browseFolder_Click(object sender, EventArgs e)
        {
            string sFormDescription = string.Empty;
            TextBox txt = null;
            switch (((Control)sender).Name)
            {
                case "cmdBrowseMonitoring":
                    sFormDescription = "Choose top level CHaMP monitoring data folder";
                    txt = txtMonitoring;
                    break;

                case "cmdBrowseOutput":
                    sFormDescription = "Choose top level input output data folder";
                    txt = txtOutput;
                    break;

                case "cmdMonitoringDataZipped":
                    sFormDescription = "Choose top level monitoring data zipped folder";
                    txt = txtMonitoringDataZipped;
                    break;

                case "cmdBrowseTemp":
                    sFormDescription = "Choose temp workspace folder";
                    txt = txtTemp;
                    break;

                default:
                    MessageBox.Show("Unhandled browse button.");
                    break;
            }

            naru.os.Folder.BrowseFolder(ref txt, sFormDescription, txt.Text);
        }
   
        private void chkAWSLoggingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool bExistingKey = Classes.AWSCloudWatch.AWSCloudWatchSingleton.HasInstallationGUID;

            if (!bExistingKey)
            {
                if (chkAWSLoggingEnabled.Checked)
                {
                    // If there's no existing installation key, then call the singelton to generate one.
                    // This will save the installation key to the settings.
                    txtStreamName.Text = Classes.AWSCloudWatch.AWSCloudWatchSingleton.Instance.InstallationGUID.ToString();
                }
            }

            lblStreamName.Visible = bExistingKey || chkAWSLoggingEnabled.Checked;
            txtStreamName.Visible = bExistingKey || chkAWSLoggingEnabled.Checked;
        }

        private void cmdTestAWS_Click(object sender, EventArgs e)
        {
            if (CHaMPWorkbench.Properties.Settings.Default.AWSLoggingEnabled || chkAWSLoggingEnabled.Checked)
            {
                try
                {
                    throw new Exception(string.Format("Test error message.", DateTime.Now.ToString()));
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
            else
            {
                MessageBox.Show("You must enable sharing exceptions with developers before this tool can be used.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmdBrowseGUT_Click(object sender, EventArgs e)
        {
            BrowseExecutable("Geomorphic Unit Tool (GUT) Python Script", "Python Scripts (*.py)|*.py", ref dlgBrowseExecutable, ref txtGUT);
        }

        private void cmdBrowsePython_Click(object sender, EventArgs e)
        {
            BrowseExecutable("Python", "Executables (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtPython);
        }

        private void cmdBrowseHydroPrep_Click(object sender, EventArgs e)
        {
            BrowseExecutable("Hydro Preparation", "Executables (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtHydroPrep);
        }

        private void cmdBrowseHabitatConsole_Click(object sender, EventArgs e)
        {
            BrowseExecutable("Habitat Console", "Executables (*.exe)|*.exe", ref dlgBrowseExecutable, ref txtHabitatConsole);
        }
    }
}

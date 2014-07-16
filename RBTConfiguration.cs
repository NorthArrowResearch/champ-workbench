﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench
{
    public partial class RBTConfiguration : UserControl
    {
        public RBTConfiguration()
        {
            InitializeComponent();
        }

        public void ManualInitialization()
        {
            tTip.SetToolTip(txtTempFolder, "The RBT temporary workspace folder");
            tTip.SetToolTip(cmdBrowseTemp, "Sets the RBT temporary workspace folder to the system temp folder as specified by the TEMP environment variable.");
            /*
             * tTip.SetToolTip(cmdBrowseOutputFolder, "Opens a window to browse and set the RBT temporary workspace folder");
            tTip.SetToolTip(txtOutputFolder, "The output folder that will be used for the results, log and artifacts.");
            tTip.SetToolTip(cmdBrowseOutputFolder, "Opens a window to browse and set the output folder that will be used for results, log and artifacts folder.");
            */
            tTip.SetToolTip(txtResults, "The name of the RBT results XML file");
            tTip.SetToolTip(txtLog, "The name of the RBT log XML file");

            /*
            string sDefault = CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder;
            if (!String.IsNullOrWhiteSpace(sDefault) && System.IO.Directory.Exists(sDefault))
                txtOutputFolder.Text = sDefault;
            */

            txtResults.Text = CHaMPWorkbench.Properties.Settings.Default.LastResultsFile;
            txtLog.Text = CHaMPWorkbench.Properties.Settings.Default.LastLogfile;

            cboRBTMode.Items.Add(new ListItem("Validate Data", 1));
            cboRBTMode.Items.Add(new ListItem("Calculate Metrics", 10));
            cboRBTMode.Items.Add(new ListItem("Fix Orthogonality", 20));
            cboRBTMode.Items.Add(new ListItem("Create Site Geodatabase", 30));
            cboRBTMode.SelectedIndex = 1;

            cboESRIProduct.Items.Add(new ListItem("Engine or Desktop", 100));
            cboESRIProduct.Items.Add(new ListItem("ArcGIS Desktop", 1));
            cboESRIProduct.Items.Add(new ListItem("Engine", 2));
            cboESRIProduct.Items.Add(new ListItem("ArcGIS Reader", 3));
            cboESRIProduct.Items.Add(new ListItem("ArcGIS Server", 5));
            cboESRIProduct.SelectedIndex = 0;

            cboLicense.Items.Add(new ListItem("Basic", 40));
            cboLicense.Items.Add(new ListItem("Standard", 50));
            cboLicense.Items.Add(new ListItem("Advanced", 60));
            cboLicense.Items.Add(new ListItem("Server", 30));
            cboLicense.Items.Add(new ListItem("Engine", 10));
            cboLicense.Items.Add(new ListItem("Engine Geodatabase", 20));
            cboLicense.SelectedIndex = 2;

            String sTemp = "C:\\CHaMP\\RBTTempFolder";
            if (! String.IsNullOrWhiteSpace(sTemp) && System.IO.Directory.Exists(sTemp))
                txtTempFolder.Text = sTemp;

        }

        public bool ValidateForm(string sStreamName, string sUTMZone, string sWatershed)
        {

            // If Not cboRBTMode.SelectedItem Is Nothing Then
            if (((ListItem)cboRBTMode.SelectedItem).Value == 30)
            {
                if (string.IsNullOrEmpty(sStreamName))
                {
                    MessageBox.Show("The stream name cannot be blank when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(sUTMZone))
                {
                    MessageBox.Show("You must select a UTM zone when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(sWatershed))
                {
                    MessageBox.Show("You must select a watershed when the RBT mode is \"Site GDB Generator\".", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            //End If


            if (string.IsNullOrEmpty(txtTempFolder.Text))
            {
                switch (MessageBox.Show("The temp workspace folder is empty. Do you want to use the system default?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        txtTempFolder.Text = System.Environment.GetEnvironmentVariable("TEMP");

                        break;
                    default:
                        return false;
                }
            }

            /*
            if (string.IsNullOrEmpty(txtOutputFolder.Text))
            {
                MessageBox.Show("The output folder cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOutputFolder.Focus();
                return false;
            }
            else
            {
                if (System.IO.Directory.Exists(txtOutputFolder.Text))
                    CHaMPWorkbench.Properties.Settings.Default.LastOutputFolder = txtOutputFolder.Text;
                else
                {
                    MessageBox.Show("The output folder must exist", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOutputFolder.Focus();
                    return false;
                }
            }
             */

            if (string.IsNullOrEmpty(txtResults.Text))
            {
                MessageBox.Show("The results file cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResults.Text = "Results.xml";
                txtResults.Focus();
                return false;
            }
            else
            {
                if (txtResults.Text.EndsWith(".xml"))
                    CHaMPWorkbench.Properties.Settings.Default.LastResultsFile = txtResults.Text;
                else
                {
                    txtResults.Text = System.IO.Path.ChangeExtension(txtResults.Text, ".xml");
                }
            }

            if (string.IsNullOrEmpty(txtLog.Text))
            {
                MessageBox.Show("The log file cannot be blank", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLog.Text = "log.xml";
                txtLog.Focus();
                return false;
            }
            else
            {
                if (txtLog.Text.EndsWith(".xml"))
                    CHaMPWorkbench.Properties.Settings.Default.LastLogfile = txtLog.Text;
                else
                {
                    txtLog.Text = System.IO.Path.ChangeExtension(txtLog.Text, ".xml");
                }
            }

            if (string.IsNullOrEmpty(txtPrecisionFormatString.Text))
            {
                MessageBox.Show("The string precision format cannot be empty. Enter a pattern for formatting strings as numbers using pound signs, commas and zeroes.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrecisionFormatString.SelectAll();
                txtPrecisionFormatString.Focus();
                return false;
            }
            else
            {
                double fTest = 3.14;
                string sTest = null;
                try
                {
                    sTest = fTest.ToString(txtPrecisionFormatString.Text);
                    if (!Double.TryParse(sTest, out fTest))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The string precision format string does not appear to convert numbers to strings correctly.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPrecisionFormatString.SelectAll();
                    txtPrecisionFormatString.Focus();
                    return false;
                }
            }

            return true;

        }

        private void cmdBrowseOutputFolder_Click(object sender, EventArgs e)
        {

        }

        private void cmdAuto_Click(object sender, EventArgs e)
        {
            String sTemp = System.Environment.GetEnvironmentVariable("TEMP");
            if (!String.IsNullOrWhiteSpace(sTemp) && System.IO.Directory.Exists(sTemp))
                txtTempFolder.Text = sTemp;
        }

        private void cmdBrowseTemp_Click(object sender, EventArgs e)
        {

        }

        private void RBTConfiguration_Load(object sender, EventArgs e)
        {
            ManualInitialization();
        }
    }
}

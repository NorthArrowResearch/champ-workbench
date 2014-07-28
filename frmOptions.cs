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
            txt7Zip.Text = CHaMPWorkbench.Properties.Settings.Default.ZipPath;
            txtTextEditor.Text = CHaMPWorkbench.Properties.Settings.Default.TextEditor;

            tTip.SetToolTip(txtOptions, "The path to the RBT console executable (rbtconsole.exe) that will be used when the RBT is run.");
            tTip.SetToolTip(txt7Zip, "The path to the 7-Zip (www.7-zip.org) compression software that will be used for unpacking CHaMP topo data.");
            tTip.SetToolTip(txtTextEditor, "The path to the text editor executable that will be used to view text files (e.g. NotePad, WordPad, TextPad++).");
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtOptions.Text) && System.IO.File.Exists(txtOptions.Text) && txtOptions.Text.EndsWith(".exe"))
                CHaMPWorkbench.Properties.Settings.Default.RBTConsole = txtOptions.Text;
            else
            {
                MessageBox.Show("The RBT Console software path must point to the RBT executable file (rbtconsole.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (!String.IsNullOrWhiteSpace(txt7Zip.Text) && System.IO.File.Exists(txt7Zip.Text) && txtOptions.Text.EndsWith(".exe"))
                CHaMPWorkbench.Properties.Settings.Default.ZipPath = txt7Zip.Text;
            else
            {
                MessageBox.Show("The 7-Zip software path must point to the 7-Zip executable file (7z.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (!String.IsNullOrWhiteSpace(txtTextEditor.Text) && System.IO.File.Exists(txtTextEditor.Text) && txtOptions.Text.EndsWith(".exe"))
                CHaMPWorkbench.Properties.Settings.Default.TextEditor = txtTextEditor.Text;
            else
            {
                MessageBox.Show("The text editor software path must point to an executable file (*.exe)", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            CHaMPWorkbench.Properties.Settings.Default.Save();
        }

        private void cmdBrowseRBT_Click(object sender, EventArgs e)
        {
            BrowseExecutable("RBT Console Executable", ref txtOptions);
        }

        private void BrowseExecutable(string sTitle, ref TextBox txt)
        {
            dlgBrowseExecutable.Title = sTitle;
            if (!String.IsNullOrWhiteSpace(txt.Text) && System.IO.File.Exists(txt.Text))
            {
                dlgBrowseExecutable.InitialDirectory = System.IO.Path.GetDirectoryName(txt.Text);
                dlgBrowseExecutable.FileName = System.IO.Path.GetFileName(txt.Text);
            }

            if (dlgBrowseExecutable.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txt.Text = dlgBrowseExecutable.FileName;
        }

        private void cmdBrowse7Zip_Click(object sender, EventArgs e)
        {
            BrowseExecutable("7 Zip Executable", ref txt7Zip);
        }

        private void cmdBrowseTextEditor_Click(object sender, EventArgs e)
        {
            BrowseExecutable("Text Editor Software", ref txtTextEditor);
        }
    }
}

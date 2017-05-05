using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class frmKeystoneCredentials : Form
    {
        public string UserName {  get { return txtUserName.Text; } }
        public string Password { get { return txtPassword.Text; } }

        public frmKeystoneCredentials()
        {
            InitializeComponent();
        }

        private void frmKeystoneCredentials_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.DefaultUserName))
            {
                txtUserName.Text = CHaMPWorkbench.Properties.Settings.Default.DefaultUserName;
                txtPassword.Select();
            }

            lblForgotUserName.LinkArea = new LinkArea(0, lblForgotUserName.Text.Length);
        }

        private DialogResult ValidateForm()
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                if (MessageBox.Show("You must provide your GeoOptix user name to continue. Contact Carol Volk (carol@southforkresearch.org), the CHaMP QA Lead, if you do not have or know your GeoOptix user name.", "GeoOptix User Name Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    return DialogResult.Cancel;
                else
                {
                    txtUserName.Select();
                    return DialogResult.None;
                }
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                if (MessageBox.Show("You must provide your GeoOptix password to continue.", "GeoOptix Password Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    return DialogResult.Cancel;
                else
                {
                    txtPassword.Select();
                    return DialogResult.None;
                }
            }

            return DialogResult.OK;
        }

        private void lblForgotUserName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://keystone.sitkatech.com/Account/ForgotUsername?ApplicationID=3");
        }

        private void lblForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://keystone.sitkatech.com/Account/ForgotPassword?ApplicationID=3");
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = ValidateForm();
            if (DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult;
                return;
            }


        }
    }
}

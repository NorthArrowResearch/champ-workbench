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
    public partial class frmToolResults : Form
    {
        private string FormTitle;
        private string Message;
        private List<string> DetailedMessages;

        public frmToolResults(string sFormTitle, string sMessage, ref List<string> lDetailedMessages)
        {
            InitializeComponent();

            FormTitle = sFormTitle;
            Message = sMessage;
            DetailedMessages = lDetailedMessages;
        }

        private void frmToolResults_Load(object sender, EventArgs e)
        {
            this.Text = FormTitle;
            lblMessage.Text = Message;

            if (DetailedMessages is List<string> && DetailedMessages.Count > 0)
                txtDetails.Text = string.Join(System.Environment.NewLine, DetailedMessages);
            else
            {
                txtDetails.Visible = false;
                int nFormBorder = this.Width - cmdOK.Right;
                this.Height = this.Height - txtDetails.Height - nFormBorder;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            }
        }
    }
}

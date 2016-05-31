using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace CHaMPWorkbench.Validation
{
    /// <summary>
    /// This is a really dumb form to help us select things that get used in a report. 
    /// It takes in a list and spits back what the user selects.
    /// </summary>
    public partial class frmSelectHelper : Form
    {
        /// <summary>
        /// The first string is the display name, the ListItem is the value (needed for formattedRBT versions)
        /// </summary>
        public List<ListItem> SelectedItems { get; set; }

        public ListItem SelectedItem {
            get{ return SelectedItems.First(); }
        }

        public frmSelectHelper(List<ListItem> incomingList, string sHelperText, bool bAllowMultiple)
        {
            InitializeComponent();
            label1.Text = sHelperText;
            listSelector.SelectionMode = bAllowMultiple ? SelectionMode.MultiSimple : SelectionMode.One;
            listSelector.Items.AddRange(incomingList.ToArray());
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedItems = listSelector.SelectedItems.Cast<ListItem>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}

using System;
using System.Collections.Generic;
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
        public List<naru.db.NamedObject> SelectedItems { get; set; }

        public naru.db.NamedObject SelectedItem {
            get{ return SelectedItems.First(); }
        }

        public frmSelectHelper(List<naru.db.NamedObject> incomingList, string sFormTitle, string sHelperText, bool bAllowMultiple)
        {
            InitializeComponent();
            this.Text = sFormTitle;
            label1.Text = sHelperText;
            listSelector.SelectionMode = bAllowMultiple ? SelectionMode.MultiSimple : SelectionMode.One;
            listSelector.Items.AddRange(incomingList.ToArray());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedItems = listSelector.SelectedItems.Cast<naru.db.NamedObject>().ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.OleDb;

using System.Windows.Forms;

namespace CHaMPWorkbench.Validation
{
    public partial class frmSelectWatershed : Form
    {

        public ListItem SelectedWatershed { get; set; }

        public frmSelectWatershed(List<ListItem> watershedList)
        {
            InitializeComponent();
            foreach(ListItem watershedItem in watershedList)
            {
                listWatershed.Items.Add(watershedItem);
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedWatershed = (ListItem)listWatershed.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

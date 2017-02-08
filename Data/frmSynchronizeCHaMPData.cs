using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHaMPWorkbench.Data
{
    public partial class frmSynchronizeCHaMPData : Form
    {
        public frmSynchronizeCHaMPData()
        {
            InitializeComponent();
        }

        private void frmSynchronizeCHaMPData_Load(object sender, EventArgs e)
        {
            naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstPrograms, naru.db.sqlite.DBCon.ConnectionString, "SELECT ProgramID, Title FROM LookupPrograms WHERE (API IS NOT NULL) ORDER BY Title", true);
        }
    }
}

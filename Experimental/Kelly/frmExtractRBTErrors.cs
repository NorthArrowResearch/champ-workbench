using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Experimental.Kelly
{
    public partial class frmExtractRBTErrors : Form
    {
        private OleDbConnection m_dbCon;

        public frmExtractRBTErrors(OleDbConnection dbCon)
        {
            InitializeComponent();
            m_dbCon = dbCon;
        }

        private void btnBrowseInputDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select CHaMP_MetricEngineStatus Access Database";
            dlg.Filter = "Access Databases (*.mdb, *.accdb)|*.mdb;*.accdb";

            if (!String.IsNullOrWhiteSpace(txtDatabase.Text) && System.IO.File.Exists(txtDatabase.Text))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(txtDatabase.Text);
                dlg.FileName = System.IO.Path.GetFileName(txtDatabase.Text);
            }
            else
            {
                //System.Data.OleDb.OleDbConnectionStringBuilder oCon = new System.Data.OleDb.OleDbConnectionStringBuilder(m_dbCon.ConnectionString);
                //dlg.InitialDirectory = System.IO.Path.GetDirectoryName(oCon.DataSource);
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDatabase.Text = dlg.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtDatabase.Text) || !System.IO.File.Exists(txtDatabase.Text))
            {
                MessageBox.Show("Please enter a valid path to the CHaMP exported 'CHaMP_MetricEngineStatus' Access database.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                String sMsg = (txtDatabase.Text);
                MessageBox.Show(sMsg, CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Processing VisitInfo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private String extractRBTErrors(string sDatabase)
        {
            if (m_dbCon.State == ConnectionState.Closed)
            {
                m_dbCon.Open();
            }
            RBTWorkbenchDataSet ds = new RBTWorkbenchDataSet();

            RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter daWatersheds = new RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            daWatersheds.Connection = m_dbCon;
            daWatersheds.Fill(ds.CHAMP_Watersheds);

            RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter daSites = new RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            daSites.Connection = m_dbCon;
            daSites.Fill(ds.CHAMP_Sites);

            RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter daVisits = new RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
            daVisits.Connection = m_dbCon;
            daVisits.Fill(ds.CHAMP_Visits);

            RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter daSegments = new RBTWorkbenchDataSetTableAdapters.CHaMP_SegmentsTableAdapter();
            daSegments.Connection = m_dbCon;
            daSegments.Fill(ds.CHaMP_Segments);

            RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter daChannelUnits = new RBTWorkbenchDataSetTableAdapters.CHAMP_ChannelUnitsTableAdapter();
            daChannelUnits.Connection = m_dbCon;
            daChannelUnits.Fill(ds.CHAMP_ChannelUnits);

            String sDB = CHaMPWorkbench.Properties.Resources.DBConnectionStringBase.Replace("Source=", "Source=" + sDatabase);
            using (OleDbConnection dbCHaMP = new OleDbConnection(sDB))
            {
                dbCHaMP.Open();

                String sSQL = "";
                using (OleDbCommand dbCom = new OleDbCommand(sSQL, dbCHaMP))
                {
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {

                        //UpdateWatersheds(dbCHaMP, daWatersheds, ds.CHAMP_Watersheds);
                        //UpdateSites(dbCHaMP, daSites, ds.CHAMP_Sites);
                        //UpdateVisits(dbCHaMP, daVisits, ds.CHAMP_Visits);
                        //UpdateSegmentsAndUnits(dbCHaMP, daSegments, daChannelUnits, ds);
                    }
                }
            }


            return "true";
        }
    
    }
}

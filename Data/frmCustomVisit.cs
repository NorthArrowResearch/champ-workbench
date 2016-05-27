using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class frmCustomVisit : Form
    {
        public string DBCon { get; internal set; }

        private BindingList<ChannelUnit> bsChannelUnits;

        public frmCustomVisit(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;

            bsChannelUnits = new BindingList<ChannelUnit>();
        }

        private void frmCustomVisit_Load(object sender, EventArgs e)
        {
            ListItem.LoadComboWithListItems(ref cboProtocol, DBCon, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8 ORDER BY Title");
            ListItem.LoadComboWithListItems(ref cboWatershed, DBCon, "SELECT WatershedID, WatershedName FROM CHaMP_Watersheds ORDER BY WatershedName");

            if (DateTime.Now.Year >= valFieldSeason.Minimum && DateTime.Now.Year <= valFieldSeason.Maximum)
                valFieldSeason.Value = DateTime.Now.Year;

            // Set the visit ID to the next largest available visit ID
            // Check the Visit ID doesn't already exist
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT Max(VisitID) FROM CHaMP_Visits", dbCon);
                object obj = dbCom.ExecuteScalar();
                if (obj != null && obj is Int32)
                {
                    int nMaxVisitID = (int)obj;
                    valVisitID.Value = Math.Max(valVisitID.Minimum, nMaxVisitID + 1);
                }
            }

            // Populate the data grid tier columns with the tier names
            LoadDataGridComboBox(grdChannelUnits.Columns["colTier1"], "Tier1");
            LoadDataGridComboBox(grdChannelUnits.Columns["colTier2"], "Tier2");

            grdChannelUnits.DataSource = bsChannelUnits;
        }

        private void LoadDataGridComboBox(DataGridViewColumn theCol, string sWorkbenchColName)
        {
            DataGridViewComboBoxColumn cboCol = (DataGridViewComboBoxColumn)theCol;

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand(string.Format("SELECT {0} FROM CHAMP_ChannelUnits GROUP BY {0}", sWorkbenchColName), dbCon);
                OleDbDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    cboCol.Items.Add(dbRead[0]);
            }
        }

        private void cboWatershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem.LoadComboWithListItems(ref cboSite, DBCon, "SELECT SiteID, SiteName FROM CHaMP_Sites ORDER BY SiteName");
        }

        private bool ValidateForm()
        {
            // Check the Visit ID doesn't already exist
            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbCommand dbCom = new OleDbCommand("SELECT VisitID FROM CHaMP_Visits WHERE VisitID = @VisitID", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", (int)valVisitID.Value);
                object obj = dbCom.ExecuteScalar();
                if (obj != null && obj is Int32)
                {
                    MessageBox.Show(string.Format("A visit already exists with the visit ID {0}", valVisitID.Value), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (cboWatershed.SelectedItem == null)
            {
                MessageBox.Show("The custom visit must be associated with a watershed. Either select an existing watershed or enter a placeholder name if you do not have one.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboSite.SelectedItem == null)
            {
                MessageBox.Show("The custom visit must be associated with a site. Either select an existing site or enter a placeholder name if you do not have one.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboProtocol.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a protocol for the custom visit.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();
                OleDbTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    int nWatershedID = 0;
                    if (cboWatershed.SelectedItem is ListItem)
                        nWatershedID = ((ListItem)cboWatershed.SelectedItem).Value;
                    else
                    {
                        OleDbCommand dbCom = new OleDbCommand("INSERT INTO CHaMP_Watersheds (WatershedName) VALUES (@WatershedName)", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("@WatershedName", cboWatershed.Text);
                        if (dbCom.ExecuteNonQuery() == 1)
                        {
                            dbCom = new OleDbCommand("SELECT @@Identity FROM CHaMP_Watersheds", dbCon, dbTrans);
                            nWatershedID = (int)dbCom.ExecuteScalar();
                        }
                        else
                            throw new Exception("Failed to create new watershed");
                    }

                    int nSiteID = 0;
                    if (cboSite.SelectedItem is ListItem)
                        nSiteID = ((ListItem)cboSite.SelectedItem).Value;
                    else
                    {
                        OleDbCommand dbCom = new OleDbCommand("INSERT INTO CHaMP_Sites (SiteName, WatershedID) VALUES (@SiteName, @WatershedID)", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("@SiteName", cboSite.Text);
                        dbCom.Parameters.AddWithValue("@WatershedID", nWatershedID);
                        if (dbCom.ExecuteNonQuery() == 1)
                        {
                            dbCom = new OleDbCommand("SELECT @@Identity FROM CHaMP_Sites", dbCon, dbTrans);
                            nSiteID = (int)dbCom.ExecuteScalar();
                        }
                        else
                            throw new Exception("Failed to create new site");
                    }

                    OleDbCommand comVisit = new OleDbCommand("INSERT INTO CHaMP_Visits (VisitID, SiteID, VisitYear, ProtocolID, Organization) VALUES (@VisitID, @SiteID, @VisitYear, @ProtocolID, @Organization)", dbCon, dbTrans);
                    comVisit.Parameters.AddWithValue("@VisitID", (int)valVisitID.Value);
                    comVisit.Parameters.AddWithValue("@SiteID", nSiteID);
                    comVisit.Parameters.AddWithValue("@VisitYear", (int)valFieldSeason.Value);
                    comVisit.Parameters.AddWithValue("@ProtocolID", ((ListItem)cboProtocol.SelectedItem).Value);
                    OleDbParameter pOrganization = comVisit.Parameters.Add("@Organization", OleDbType.VarChar);
                    if (string.IsNullOrEmpty(txtOrganization.Text))
                        pOrganization.Value = DBNull.Value;
                    else
                        pOrganization.Value = txtOrganization.Text;

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Channel segments and units
                    Dictionary<int, int> dSegmentNumbers = new Dictionary<int, int>(); // segment number key to database segment ID in DB values

                    foreach (DataGridViewRow drv in grdChannelUnits.Rows)
                    {
                        foreach (ChannelUnit ch in bsChannelUnits)
                        {
                            int nSegmentID = 0;
                            if (dSegmentNumbers.ContainsKey(ch.SegmentNumber))
                            {
                                nSegmentID = dSegmentNumbers[ch.SegmentNumber];
                            }
                            else
                            {
                                // Create new segment
                                OleDbCommand comSegment = new OleDbCommand("INSERT INTO CHaMP_Segments (VisitID, SegmentNumber, SegmentName) VALUES (@VisitID, @SegmentNumber, @SegmentName)", dbCon, dbTrans);
                                comSegment.Parameters.AddWithValue("@VisitID", (int) valVisitID.Value);
                                comSegment.Parameters.AddWithValue("@SegmentNumber", ch.SegmentNumber);
                                comSegment.Parameters.AddWithValue("@SegmentName", string.Format("Segment {0}", ch.SegmentNumber));
                                if (comSegment.ExecuteNonQuery() == 1)
                                {
                                    comSegment = new OleDbCommand("SELECT @@Identity FROM CHaMP_Segments", dbCon, dbTrans);
                                    nSegmentID = (int)comSegment.ExecuteScalar();
                                }
                                else
                                    throw new Exception(string.Format("Error inserting new channel segment {0}", ch.SegmentNumber));
                            }

                            // Now insert the channel units
                            OleDbCommand comUnit = new OleDbCommand("INSERT INTO CHaMP_ChannelUnit (SegmentID, ChannelUnitNumber, Tier1, Tier2) VALUES (@SegmentID, @ChannelUnitNumber, @Tier1, @Tier2)", dbCon, dbTrans);
                            comUnit.Parameters.AddWithValue("@SegmentID", nSegmentID);
                            comUnit.Parameters.AddWithValue("@ChannelUnitNumber", ch.UnitNumber);
                            comUnit.Parameters.AddWithValue("@Tier1", ch.Tier1);
                            comUnit.Parameters.AddWithValue("@Tier2", ch.Tier2);
                            if (comUnit.ExecuteNonQuery() != 1)
                                throw new Exception(string.Format("Error inserting new channel unit {0}", ch.UnitNumber));
                        }
                    }

                    if (comVisit.ExecuteNonQuery() == 1)
                        dbTrans.Commit();
                    else
                        throw new Exception("Failed to insert custom visit.");
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    Classes.ExceptionHandling.NARException.HandleException(ex);

                }
            }
        }

        private class ChannelUnit
        {
            public int UnitNumber { get; set; }
            public int SegmentNumber { get; set; }
            public string Tier1 { get; set; }
            public string Tier2 { get; set; }

            public ChannelUnit()
            {
                UnitNumber = 1;
                SegmentNumber = 1;
            }

            public ChannelUnit(int nUnitNumber, int nSegmentNumber, string sTier1, string sTier2)
            {
                UnitNumber = nUnitNumber;
                SegmentNumber = nSegmentNumber;
                Tier1 = sTier1;
                Tier2 = sTier2;
            }
        }

        private void grdChannelUnits_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!ValidateChannelUnitNumber(e.RowIndex))
            {
                e.Cancel = true;
                return;
            }

            if (!ValidateSegmentUnitNumber(e.RowIndex))
            {
                e.Cancel = true;
                return;
            }

            string[] sTiers = { "1", "2" };
            foreach (string sTier in sTiers)
            {
                object obj = grdChannelUnits.Rows[e.RowIndex].Cells[string.Format("colTier{0}", sTier)].Value;

                if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                {
                    MessageBox.Show(string.Format("You must provide a tier {0} classification for the channel unit.", sTier), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                    return;
                }
            }           
        }

        private bool ValidateChannelUnitNumber(int nRowIndex)
        {
            object obj = grdChannelUnits.Rows[nRowIndex].Cells["colUnitNumber"].Value;

            if (obj == null)
                return false;

            int nNumber = 0;
            if (int.TryParse(obj.ToString(), out nNumber))
            {
                if (nNumber < 1)
                {
                    MessageBox.Show("The channel unit number must be positive.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    // Check unique
                    for (int i = 0; i < grdChannelUnits.Rows.Count; i++)
                    {
                        if (grdChannelUnits.Rows[i].DataBoundItem is ChannelUnit && i != nRowIndex)
                        {
                            if (((ChannelUnit)grdChannelUnits.Rows[i].DataBoundItem).UnitNumber == nNumber)
                            {
                                MessageBox.Show(string.Format("There is already a channel unit number {0}. Each channel unit must have a unique number.", nNumber), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("You must provide a positive integer channel unit number.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private bool ValidateSegmentUnitNumber(int nRowIndex)
        {
            int nNumber = 0;
            if (int.TryParse(grdChannelUnits.Rows[nRowIndex].Cells["colSegmentNumber"].Value.ToString(), out nNumber))
            {
                if (nNumber < 1)
                {
                    MessageBox.Show("The segment number must be positive.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("You must provide a positive integer segment unit number.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }
    }
}

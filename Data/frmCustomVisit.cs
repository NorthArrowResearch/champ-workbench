using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Esri.FileGDB;

namespace CHaMPWorkbench.Data
{
    public partial class frmCustomVisit : Form
    {
        // Names of the fields in the survey GDB channel units feature class
        private const string SurveyGDB_ChannelUnitsTableName = "Channel_Units";
        private const string SurveyGDB_UnitNumberField = "Unit_Number";
        private const string SurveyGDB_SegmentField = "Segment_Number";
        private const string SurveyGDB_Tier1Field = "Tier1";
        private const string SurveyGDB_Tier2Field = "Tier2";

        // Used if the survey GDB channel units are missing tier information and the
        // user chooses to populate with random tier types
        private Random RandomNumber;

        public string DBCon { get; internal set; }

        // This is the list of channel units bound to the data grid view.
        private BindingList<ChannelUnit> bsChannelUnits;

        // Make the visit ID publicly available to the parent form so that
        // it can select it in the main grid after the insert.
        public int VisitID { get { return (int)valVisitID.Value; } }

        public frmCustomVisit(string sDBCon)
        {
            InitializeComponent();
            DBCon = sDBCon;
            RandomNumber = new Random(DateTime.Now.Second);

            bsChannelUnits = new BindingList<ChannelUnit>();
        }

        private void frmCustomVisit_Load(object sender, EventArgs e)
        {
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboProtocol, DBCon, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8 ORDER BY Title");
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboWatershed, DBCon, "SELECT WatershedID, WatershedName FROM CHaMP_Watersheds ORDER BY WatershedName");
            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboProgram, DBCon, "SELECT ProgramID, Title FROM LookupPrograms ORDER BY Title");

            if (DateTime.Now.Year >= valFieldSeason.Minimum && DateTime.Now.Year <= valFieldSeason.Maximum)
                valFieldSeason.Value = DateTime.Now.Year;

            // Set the visit ID to the next largest available visit ID
            // Check the Visit ID doesn't already exist
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT Max(VisitID) FROM CHaMP_Visits", dbCon);
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

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand(string.Format("SELECT {0} FROM CHAMP_ChannelUnits GROUP BY {0}", sWorkbenchColName), dbCon);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    cboCol.Items.Add(dbRead[0]);
            }
        }

        private void cboWatershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            cboSite.Items.Clear();
            if (cboWatershed.SelectedItem is naru.db.NamedObject)
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT SiteID, SiteName FROM CHaMP_Sites WHERE WatershedID = @WatershedID ORDER BY SiteName", dbCon);
                    dbCom.Parameters.AddWithValue("@WatershedID", ((naru.db.NamedObject)cboWatershed.SelectedItem).ID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                        cboSite.Items.Add(new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("SiteID")), dbRead.GetString(dbRead.GetOrdinal("SiteName"))));
                }
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private bool ValidateForm()
        {
            // Check the Visit ID doesn't already exist
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT VisitID FROM CHaMP_Visits WHERE VisitID = @VisitID", dbCon);
                dbCom.Parameters.AddWithValue("@VisitID", (int)valVisitID.Value);
                object obj = dbCom.ExecuteScalar();
                if (obj != null && obj is Int32)
                {
                    MessageBox.Show(string.Format("A visit already exists with the visit ID {0}", valVisitID.Value), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(cboWatershed.Text))
            {
                MessageBox.Show("The custom visit must be associated with a watershed. Either select an existing watershed or enter a placeholder name if you do not have one.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(cboSite.Text))
            {
                MessageBox.Show("The custom visit must be associated with a site. Either select an existing site or enter a placeholder name if you do not have one.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboProtocol.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a protocol for the custom visit.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cboProgram.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a program for the custom visit.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (bsChannelUnits.Count < 1)
            {
                switch (MessageBox.Show("If you proceed and create this visit without any channel units then it will not be possible to use this visit with the RBT or batch substrate builder. Do you want to proceed and create this visit without channel units?", "No Channel Units Defined", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.No:
                        return false;
                }
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

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    long nWatershedID = 0;
                    if (cboWatershed.SelectedItem is naru.db.NamedObject)
                        nWatershedID = ((naru.db.NamedObject)cboWatershed.SelectedItem).ID;
                    else
                    {
                        // Watershed ID is not auto-increment.
                        SQLiteCommand dbCom = new SQLiteCommand("SELECT Max(WatershedID) FROM CHaMP_Watersheds", dbCon, dbTrans);
                        object objWSID = dbCom.ExecuteScalar();
                        if (objWSID is Int32)
                            nWatershedID = Math.Max(((int)objWSID) + 1, 9000);
                        else
                            throw new Exception("Failed to retrieve highest watershed ID from database.");

                        dbCom = new SQLiteCommand("INSERT INTO CHaMP_Watersheds (WatershedID, WatershedName) VALUES (@WatershedID, @WatershedName)", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("@WatershedID", nWatershedID);
                        dbCom.Parameters.AddWithValue("@WatershedName", cboWatershed.Text);
                        if (dbCom.ExecuteNonQuery() != 1)
                            throw new Exception("Failed to create new watershed");
                    }

                    long nSiteID = 0;
                    if (cboSite.SelectedItem is naru.db.NamedObject)
                        nSiteID = ((naru.db.NamedObject)cboSite.SelectedItem).ID;
                    else
                    {
                        SQLiteCommand dbCom = new SQLiteCommand("SELECT Max(SiteID) FROM CHaMP_Sites", dbCon, dbTrans);
                        object objSID = dbCom.ExecuteScalar();
                        if (objSID is Int32)
                            nSiteID = Math.Max(((int)objSID) + 1, 9000);
                        else
                            throw new Exception("Failed to retrieve highest Site ID from database.");

                        dbCom = new SQLiteCommand("INSERT INTO CHaMP_Sites (SiteID, SiteName, WatershedID) VALUES (@SiteID, @SiteName, @WatershedID)", dbCon, dbTrans);
                        dbCom.Parameters.AddWithValue("@SiteID", nSiteID);
                        dbCom.Parameters.AddWithValue("@SiteName", cboSite.Text);
                        dbCom.Parameters.AddWithValue("@WatershedID", nWatershedID);
                        if (dbCom.ExecuteNonQuery() != 1)
                            throw new Exception("Failed to create new site");
                    }

                    SQLiteCommand comVisit = new SQLiteCommand("INSERT INTO CHaMP_Visits (VisitID, SiteID, VisitYear, ProtocolID, Organization, Remarks, ProgramID) VALUES (@VisitID, @SiteID, @VisitYear, @ProtocolID, @Organization, @Remarks, @ProgramID)", dbCon, dbTrans);
                    comVisit.Parameters.AddWithValue("@VisitID", (long)valVisitID.Value);
                    comVisit.Parameters.AddWithValue("@SiteID", nSiteID);
                    comVisit.Parameters.AddWithValue("@VisitYear", (long)valFieldSeason.Value);
                    comVisit.Parameters.AddWithValue("@ProtocolID", ((naru.db.NamedObject)cboProtocol.SelectedItem).ID);
                    comVisit.Parameters.AddWithValue("@ProgramID", ((naru.db.NamedObject)cboProgram.SelectedItem).ID);
                    SQLiteParameter pOrganization = comVisit.Parameters.Add("@Organization", DbType.String);
                    if (string.IsNullOrEmpty(txtOrganization.Text))
                        pOrganization.Value = DBNull.Value;
                    else
                        pOrganization.Value = txtOrganization.Text;

                    SQLiteParameter pRemarks = comVisit.Parameters.Add("@Remarks", DbType.String);
                    if (string.IsNullOrEmpty(txtNotes.Text))
                        pRemarks.Value = DBNull.Value;
                    else
                        pRemarks.Value = txtNotes.Text;

                    if (comVisit.ExecuteNonQuery() != 1)
                        throw new Exception("Failed to insert custom visit.");

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Channel segments and units
                    Dictionary<int, int> dSegmentNumbers = new Dictionary<int, int>(); // segment number key to database segment ID in DB values

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
                            SQLiteCommand comSegment = new SQLiteCommand("INSERT INTO CHaMP_Segments (VisitID, SegmentNumber, SegmentName) VALUES (@VisitID, @SegmentNumber, @SegmentName)", dbCon, dbTrans);
                            comSegment.Parameters.AddWithValue("@VisitID", (long)valVisitID.Value);
                            comSegment.Parameters.AddWithValue("@SegmentNumber", ch.SegmentNumber);
                            comSegment.Parameters.AddWithValue("@SegmentName", string.Format("Segment {0}", ch.SegmentNumber));
                            if (comSegment.ExecuteNonQuery() == 1)
                            {
                                comSegment = new SQLiteCommand("SELECT @@last_insert_rowid()", dbCon, dbTrans);
                                nSegmentID = (int)comSegment.ExecuteScalar();
                                dSegmentNumbers.Add(ch.SegmentNumber, nSegmentID);
                            }
                            else
                                throw new Exception(string.Format("Error inserting new channel segment {0}", ch.SegmentNumber));
                        }

                        // Now insert the channel units
                        SQLiteCommand comUnit = new SQLiteCommand("INSERT INTO CHaMP_ChannelUnits (SegmentID, ChannelUnitNumber, Tier1, Tier2) VALUES (@SegmentID, @ChannelUnitNumber, @Tier1, @Tier2)", dbCon, dbTrans);
                        comUnit.Parameters.AddWithValue("@SegmentID", dSegmentNumbers[ch.SegmentNumber]);
                        comUnit.Parameters.AddWithValue("@ChannelUnitNumber", ch.UnitNumber);
                        comUnit.Parameters.AddWithValue("@Tier1", ch.Tier1);
                        comUnit.Parameters.AddWithValue("@Tier2", ch.Tier2);
                        if (comUnit.ExecuteNonQuery() != 1)
                            throw new Exception(string.Format("Error inserting new channel unit {0}", ch.UnitNumber));
                    }

                    dbTrans.Commit();
                    MessageBox.Show(string.Format("Custom visit {0} inserted successfully with {1} channel units.", valVisitID.Value, bsChannelUnits.Count), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (bsChannelUnits.Count > 0)
            {
                switch (MessageBox.Show("Do you want to clear the existing list of channel units and reload them from the selected survey geodatabase?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.No:
                        return;

                    case System.Windows.Forms.DialogResult.Cancel:
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        return;
                }
            }

            // Clear any existing channel units
            bsChannelUnits.Clear();

            FolderBrowserDialog frm = new FolderBrowserDialog();
            frm.Description = "Select Survey Geodatabase";
            frm.ShowNewFolderButton = false;

            if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder) && System.IO.Directory.Exists(CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder))
                frm.SelectedPath = CHaMPWorkbench.Properties.Settings.Default.MonitoringDataFolder;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.SelectedPath.ToLower().EndsWith(".gdb"))
                {
                    Geodatabase surveyGDB = Geodatabase.Open(frm.SelectedPath);
                    Table chTable = surveyGDB.OpenTable(SurveyGDB_ChannelUnitsTableName);
                    if (chTable is Table)
                    {
                        if (chTable.RowCount < 1)
                        {
                            MessageBox.Show("The channel unit feature class is empty.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        FieldInfo fInfo = chTable.FieldInformation;
                        List<string> sFields = new List<string>();
                        for (int i = 0; i < fInfo.Count; i++)
                        {
                            if (string.Compare(fInfo.GetFieldName(i), SurveyGDB_Tier1Field, true) == 0)
                                sFields.Add(SurveyGDB_Tier1Field);
                            else if (string.Compare(fInfo.GetFieldName(i), SurveyGDB_Tier2Field, true) == 0)
                                sFields.Add(SurveyGDB_Tier2Field);
                            else if (string.Compare(fInfo.GetFieldName(i), SurveyGDB_UnitNumberField, true) == 0)
                                sFields.Add(SurveyGDB_UnitNumberField);
                            else if (string.Compare(fInfo.GetFieldName(i), SurveyGDB_SegmentField, true) == 0)
                                sFields.Add(SurveyGDB_SegmentField);
                        }

                        if (!sFields.Contains(SurveyGDB_UnitNumberField))
                        {
                            MessageBox.Show(string.Format("Unable to find the channel unit number field called '{0}' in the {1} feature class inside the file geodatabase.", SurveyGDB_UnitNumberField, SurveyGDB_ChannelUnitsTableName), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (!sFields.Contains(SurveyGDB_SegmentField))
                        {
                            if (MessageBox.Show(string.Format("Unable to find the segment number field called '{0}' in the feature class. Do you want to proceed and assign all channel units to segment 1?", SurveyGDB_SegmentField), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == System.Windows.Forms.DialogResult.No)
                                return;
                        }

                        if (!sFields.Contains(SurveyGDB_Tier1Field))
                        {
                            if (MessageBox.Show("The channel units feature class is missing tier 1 classifications. Do you want to assign random tier 1 classifications instead?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == System.Windows.Forms.DialogResult.No)
                                return;
                        }

                        if (!sFields.Contains(SurveyGDB_Tier2Field))
                        {
                            if (MessageBox.Show("The channel units feature class is missing tier 2 classifications. Do you want to assign random tier 2 classifications instead?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == System.Windows.Forms.DialogResult.No)
                                return;
                        }

                        RowCollection attrQueryRows = chTable.Search(string.Join(",", sFields), string.Empty, RowInstance.Recycle);
                        foreach (Row attrQueryRow in attrQueryRows)
                        {
                            if (attrQueryRow.IsNull(SurveyGDB_UnitNumberField))
                            {
                                MessageBox.Show("One or more rows in the channel unit feature class possess null channel unit numbers. All features must possess a positive integer channel unit number. Unable to proceed and import channel unit information from this feature class.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            int nSegment = 1;
                            if (sFields.Contains(SurveyGDB_SegmentField) && !attrQueryRow.IsNull(SurveyGDB_SegmentField))
                                nSegment = attrQueryRow.GetInteger(SurveyGDB_SegmentField);

                            string sTier1 = GetTierName(ref sFields, attrQueryRow, SurveyGDB_Tier1Field, 1);
                            string sTier2 = GetTierName(ref sFields, attrQueryRow, SurveyGDB_Tier2Field, 2);

                            ChannelUnit ch = new ChannelUnit(attrQueryRow.GetShort(SurveyGDB_UnitNumberField), nSegment, sTier1, sTier2);
                            bsChannelUnits.Add(ch);
                        }
                    }
                    else
                        MessageBox.Show(string.Format("Unable to find the channel units table called '{0}' in the file geodatabase.", SurveyGDB_ChannelUnitsTableName), CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("The selected folder does not appear to be a file geodatabase.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetTierName(ref List<string> theFields, Row theRow, string sFeatureClassFieldName, int nTier)
        {
            if (theFields.Contains(sFeatureClassFieldName) && !theRow.IsNull(sFeatureClassFieldName))
                return theRow.GetString(sFeatureClassFieldName);
            else
            {
                DataGridViewComboBoxColumn theCol = (DataGridViewComboBoxColumn)grdChannelUnits.Columns[string.Format("colTier{0}", nTier)];
                int nItem = (int)Math.Round((RandomNumber.NextDouble() * ((double)theCol.Items.Count - 1)));
                return theCol.Items[nItem].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSelectVisitID frm = new frmSelectVisitID(DBCon);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (bsChannelUnits.Count > 0)
                {
                    switch (MessageBox.Show("Do you want to clear the existing list of channel units and reload them from the selected survey geodatabase?", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case System.Windows.Forms.DialogResult.No:
                            return;

                        case System.Windows.Forms.DialogResult.Cancel:
                            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                            return;
                    }
                }

                // Clear any existing channel units
                bsChannelUnits.Clear();

                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT ChannelUnitNumber, SegmentNumber, Tier1, Tier2 FROM CHaMP_Segments INNER JOIN CHAMP_ChannelUnits ON CHaMP_Segments.SegmentID = CHAMP_ChannelUnits.SegmentID WHERE VisitID = @VisitID ORDER BY ChannelUnitNumber", dbCon);
                    dbCom.Parameters.AddWithValue("@VisitID", frm.SelectedVisitID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        bsChannelUnits.Add(new ChannelUnit(
                            dbRead.GetInt32(dbRead.GetOrdinal("ChannelUnitNumber"))
                            , dbRead.GetInt32(dbRead.GetOrdinal("SegmentNumber"))
                            , dbRead.GetString(dbRead.GetOrdinal("Tier1"))
                            , dbRead.GetString(dbRead.GetOrdinal("Tier2"))
                            ));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

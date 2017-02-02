using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench.Data
{
    public partial class ucUserFeedback : UserControl
    {
        public string DBCon { get; set; }
        public int LogID { get; set; }

        public string ItemReviewed
        {
            get
            {
                return cboItemReviewed.Text;
            }
            set
            {
                cboItemReviewed.Text = value;
            }
        }

        public ucUserFeedback()
        {
            InitializeComponent();
            LogID = 0;
        }

        private void ucUserFeedback_Load(object sender, EventArgs e)
        {
            if (!(this.Parent is Form))
            {
                // Placed in metric review user control. Hide the cancel button
                cmdCancel.Visible = false;
                cmdOK.Left = cmdCancel.Left;
            }


            if (string.IsNullOrEmpty(DBCon))
                return;

            int nQualityID = 0;
            int nWatershedID = 0;
            int nSiteID = 0;
            int nVisitID = 0;

            if (LogID > 0)
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                {
                    dbCon.Open();

                    SQLiteCommand dbCom = new SQLiteCommand("SELECT UserName, QualityRatingID, ItemReviewed, WatershedID, SiteID, VisitID, Description, ReviewedOn FROM LogFeedback WHERE LogID = @LogID", dbCon);
                    dbCom.Parameters.AddWithValue("LogID", LogID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    if (!dbRead.Read())
                    {
                        Exception ex = new Exception("Failed to retrieve user feedback item from database.");
                        ex.Data["LogID"] = LogID.ToString();
                        ex.Data["Conncetion"] = DBCon;
                        throw ex;
                    }

                    nQualityID = dbRead.GetInt32(dbRead.GetOrdinal("QualityRatingID"));
                    txtUserName.Text = dbRead.GetString(dbRead.GetOrdinal("UserName"));
                    cboItemReviewed.Text = dbRead.GetString(dbRead.GetOrdinal("ItemReviewed"));

                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("Description")))
                        txtDescription.Text = dbRead.GetString(dbRead.GetOrdinal("Description"));

                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("WatershedID")))
                        nWatershedID = dbRead.GetInt32(dbRead.GetOrdinal("WatershedID"));

                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("SiteID")))
                        nSiteID = dbRead.GetInt32(dbRead.GetOrdinal("SiteID"));

                    if (!dbRead.IsDBNull(dbRead.GetOrdinal("VisitID")))
                        nVisitID = dbRead.GetInt32(dbRead.GetOrdinal("VisitID"));
                }

            }
            else
            {
                // prepare form for new record
                if (!string.IsNullOrEmpty(CHaMPWorkbench.Properties.Settings.Default.DefaultUserName))
                    txtUserName.Text = CHaMPWorkbench.Properties.Settings.Default.DefaultUserName;

                dtDateTime.Value = DateTime.Now;
            }

            naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboQualityRating, DBCon, "SELECT ItemID, Title FROM LookupListITems WHERE ListID = 13", nQualityID);

            // Note that selecting a watershed will trigger loading of site. And site will trigger loading of visit
            cboWatershed.SelectedIndexChanged += new System.EventHandler(this.Watershed_SelectedIndexChanged);
            cboSite.SelectedIndexChanged += new System.EventHandler(this.Site_SelectedIndexChanged);

           naru.db.sqlite.NamedObject.LoadComboWithListItems(ref cboWatershed, DBCon, "SELECT WatershedID, WatershedName FROM CHAMP_Watersheds ORDER BY WatershedName", nWatershedID);
            if (nWatershedID > 0)
            {
                if (nSiteID > 0)
                {
                    // Selecting a site will also filter the visits
                    cboSite.SelectedValue = nSiteID;
                    if (nVisitID > 0)
                        cboVisit.SelectedValue = nVisitID;
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("You must provide a user name to store user feedback.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Select();
                return false;
            }

            if (!(cboQualityRating.SelectedItem is naru.db.NamedObject))
            {
                MessageBox.Show("You must select a quality rating to store user feedback.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboQualityRating.Select();
                return false;
            }

            if (string.IsNullOrEmpty(cboItemReviewed.Text))
            {
                MessageBox.Show("You must specify the item being reviewed to store user feedback.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboItemReviewed.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                switch (MessageBox.Show("Are you sure that you want to store user feedback without providing a description?",
                    CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.Yes:
                        return true;

                    case DialogResult.No:
                        return false;
                }
            }

            return true;
        }

        #region Watershed, Site and Visit Changes

        private void LoadSites(long nWatershedID = 0)
        {
            cboSite.Items.Clear();
            cboVisit.Items.Clear();
            if (nWatershedID > 0)
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT SiteID, SiteName FROM CHAMP_Sites WHERE WatershedID = @WatershedID ORDER BY SiteName", dbCon);
                    dbCom.Parameters.AddWithValue("WatershedID", nWatershedID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        cboSite.Items.Add(new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("SiteID")), dbRead.GetString(dbRead.GetOrdinal("SiteName"))));
                    }
                }
            }
        }

        private void Watershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            long nWatershedID = 0;
            if (cboWatershed.SelectedItem is naru.db.NamedObject)
                nWatershedID = ((naru.db.NamedObject)cboWatershed.SelectedItem).ID;

            LoadSites(nWatershedID);
        }

        private void Site_SelectedIndexChanged(object sender, EventArgs e)
        {
            long nSiteID = 0;
            if (cboSite.SelectedItem is naru.db.NamedObject)
                nSiteID = ((naru.db.NamedObject)cboSite.SelectedItem).ID;

            LoadVisits(nSiteID);
        }

        private void LoadVisits(long nSiteID = 0)
        {
            cboVisit.Items.Clear();
            if (nSiteID > 0)
            {
                using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
                {
                    dbCon.Open();
                    SQLiteCommand dbCom = new SQLiteCommand("SELECT VisitID, VisitYear, Organization FROM CHAMP_Visits WHERE SiteID = @SiteID ORDER BY CHAMP_Visits.VisitID", dbCon);
                    dbCom.Parameters.AddWithValue("SiteID", nSiteID);
                    SQLiteDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string sVisit = string.Format("VisitID {0} in {1}", dbRead.GetInt32(dbRead.GetOrdinal("VisitID")), dbRead.GetInt16(dbRead.GetOrdinal("VisitYear")));
                        if (!dbRead.IsDBNull(dbRead.GetOrdinal("Organization")))
                            sVisit = string.Format("{0} by {1}", sVisit, dbRead.GetString(dbRead.GetOrdinal("Organization")));

                        cboVisit.Items.Add(new naru.db.NamedObject(dbRead.GetInt64(dbRead.GetOrdinal("VisitID")), sVisit));
                    }
                }
            }
        }

        #endregion

        private void cmdOK_Click(object sender, EventArgs e)
        {

            if (!ValidateForm())
                return;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();

                SQLiteCommand dbCom = null;
                if (LogID > 0)
                {
                    dbCom = new SQLiteCommand("UPDATE LogFeedback SET UserName = @UserName, QualityRatingID = @QualityRatingID, ItemReviewed = @ItemReviewed, WatershedID = @WatershedID, SiteID = @SiteID, VisitID = @VisitID, Description = @Description, ReviewedOn = @ReviewedON WHERE LogID = @LogID", dbCon);
                }
                else
                    dbCom = new SQLiteCommand("INSERT INTO LogFeedback (UserName, QualityRatingID, ItemReviewed, WatershedID, SiteID, VisitID, Description, ReviewedOn)" +
                        " VALUES (@UserName, @QualityRatingID, @ItemReviewed, @WatershedID, @SiteID, @VisitID, @Description, @ReviewedOn)", dbCon);

                dbCom.Parameters.AddWithValue("UserName", txtUserName.Text);
                dbCom.Parameters.AddWithValue("QualityRatingID", ((naru.db.NamedObject)cboQualityRating.SelectedItem).ID);
                dbCom.Parameters.AddWithValue("ItemReviewed", cboItemReviewed.Text);

                SQLiteParameter pWatershedID = dbCom.Parameters.Add("WatershedID", DbType.Int64);
                if (cboWatershed.SelectedItem is naru.db.NamedObject)
                    pWatershedID.Value = ((naru.db.NamedObject)cboWatershed.SelectedItem).ID;
                else
                    pWatershedID.Value = DBNull.Value;

                SQLiteParameter pSiteID = dbCom.Parameters.Add("SiteID", DbType.Int64);
                if (cboSite.SelectedItem is naru.db.NamedObject)
                    pSiteID.Value = ((naru.db.NamedObject)cboSite.SelectedItem).ID;
                else
                    pSiteID.Value = DBNull.Value;

                SQLiteParameter pVisitID = dbCom.Parameters.Add("VisitID", DbType.Int64);
                if (cboVisit.SelectedItem is naru.db.NamedObject)
                    pVisitID.Value = ((naru.db.NamedObject)cboVisit.SelectedItem).ID;
                else
                    pVisitID.Value = DBNull.Value;

                SQLiteParameter pDescription = dbCom.Parameters.Add("Description", DbType.String);
                if (string.IsNullOrEmpty(txtDescription.Text))
                    pDescription.Value = DBNull.Value;
                else
                {
                    pDescription.Value = txtDescription.Text;
                    pDescription.Size = txtDescription.Text.Length;
                }

                SQLiteParameter pReviewedOn = dbCom.Parameters.Add("ReviewedOn", DbType.DateTime);
                pReviewedOn.Value = dtDateTime.Value;

                if (LogID > 0)
                    dbCom.Parameters.AddWithValue("LogID", LogID);

                try
                {
                    dbCom.ExecuteNonQuery();
                    if (this.Parent is Form)
                    {
                        ((Form)this.Parent).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("User feedback saved.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Now clear the form to make it ready for the next item.
                        txtDescription.Text = string.Empty;
                        cboQualityRating.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    Classes.ExceptionHandling.NARException.HandleException(ex);
                }
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Form tmp = this.FindForm();
            tmp.Close();
            tmp.Dispose();
        }

        public void SelectVisit(int nVisitID)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(DBCon))
            {
                dbCon.Open();
                SQLiteCommand dbCom = new SQLiteCommand("SELECT S.WatershedID, S.SiteID FROM CHAMP_Sites S INNER JOIN CHAMP_Visits V ON S.SiteID = V.SiteID WHERE (V.VisitID = @VisitID)", dbCon);
                dbCom.Parameters.AddWithValue("VisitID", nVisitID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                if (!dbRead.Read())
                    throw new Exception(string.Format("Error retrieving watershed and site ID for visit {0}", nVisitID));

                // The event handlers will ensure the comboboxes are reloaded with the correct items as the assignment occurs
                naru.db.sqlite.NamedObject.SelectItem(ref cboWatershed, dbRead.GetInt32(dbRead.GetOrdinal("WatershedID")));
                naru.db.sqlite.NamedObject.SelectItem(ref cboSite, dbRead.GetInt32(dbRead.GetOrdinal("SiteID")));
                naru.db.sqlite.NamedObject.SelectItem(ref cboVisit, nVisitID);
            }
        }
    }
}

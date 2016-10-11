using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CHaMPWorkbench.Data
{
    public partial class ucUserFeedback : UserControl
    {
        public string DBCon { get; set; }
        public int LogID { get; set; }

        public ucUserFeedback()
        {
            InitializeComponent();
            LogID = 0;
        }

        private void ucUserFeedback_Load(object sender, EventArgs e)
        {
            int nQualityID = 0;
            int nWatershedID = 0;
            int nSiteID = 0;
            int nVisitID = 0;

            if (LogID > 0)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();

                    OleDbCommand dbCom = new OleDbCommand("SELECT UserName, QualityRatingID, ItemReviewed, WatershedID, SiteID, VisitID, Description, ReviewedOn FROM LogFeedback WHERE LogID = @LogID", dbCon);
                    dbCom.Parameters.AddWithValue("LogID", LogID);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    if (!dbRead.Read())
                    {
                        Exception ex = new Exception("Failed to retrieve user feedback item from database.");
                        ex.Data["LogID"] = LogID.ToString();
                        ex.Data["Conncetion"] = DBCon;
                        throw ex;
                    }

                    nQualityID = dbRead.GetInt32(dbRead.GetOrdinal("QualityRatingID"));
                    txtUserName.Text = dbRead.GetString(dbRead.GetOrdinal("UserName"));

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

            ListItem.LoadComboWithListItems(ref cboQualityRating, DBCon, "SELECT ItemID, Title FROM LookupListITems WHERE ListID = 13", nQualityID);

            // Note that selecting a watershed will trigger loading of site. And site will trigger loading of visit
            cboWatershed.SelectedIndexChanged += new System.EventHandler(this.Watershed_SelectedIndexChanged);
            cboSite.SelectedIndexChanged += new System.EventHandler(this.Site_SelectedIndexChanged);

            ListItem.LoadComboWithListItems(ref cboWatershed, DBCon, "SELECT WatershedID, WatershedName FROM CHAMP_Watersheds ORDER BY WatershedName", nWatershedID);
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

            if (!(cboQualityRating.SelectedItem is ListItem))
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

        private void LoadSites(int nWatershedID = 0)
        {
            cboSite.Items.Clear();
            cboVisit.Items.Clear();
            if (nWatershedID > 0)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand("SELECT SiteID, SiteName FROM CHAMP_Sites WHERE WatershedID = @WatershedID ORDER BY SiteName", dbCon);
                    dbCom.Parameters.AddWithValue("WatesrhedID", nWatershedID);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        cboSite.Items.Add(new ListItem(dbRead.GetString(dbRead.GetOrdinal("SiteName")), dbRead.GetInt32(dbRead.GetOrdinal("SiteID"))));
                    }
                }
            }
        }

        private void Watershed_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nWatershedID = 0;
            if (cboWatershed.SelectedItem is ListItem)
                nWatershedID = ((ListItem)cboWatershed.SelectedItem).Value;

            LoadSites(nWatershedID);
        }

        private void Site_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSiteID = 0;
            if (cboSite.SelectedItem is ListItem)
                nSiteID = ((ListItem)cboSite.SelectedItem).Value;

            LoadVisits(nSiteID);
        }

        private void LoadVisits(int nSiteID = 0)
        {
            cboVisit.Items.Clear();
            if (nSiteID > 0)
            {
                using (OleDbConnection dbCon = new OleDbConnection(DBCon))
                {
                    dbCon.Open();
                    OleDbCommand dbCom = new OleDbCommand("SELECT VisitID, VisitYear, Organization FROM CHAMP_Visits WHERE SiteID = @SiteID ORDER BY CHAMP_Visits.VisitID", dbCon);
                    dbCom.Parameters.AddWithValue("SiteID", nSiteID);
                    OleDbDataReader dbRead = dbCom.ExecuteReader();
                    while (dbRead.Read())
                    {
                        string sVisit = string.Format("VisitID {0} in {1}", dbRead.GetInt32(dbRead.GetOrdinal("VisitID")), dbRead.GetInt16(dbRead.GetOrdinal("VisitYear")));
                        if (!dbRead.IsDBNull(dbRead.GetOrdinal("Organization")))
                            sVisit = string.Format("{0} by {1}", sVisit, dbRead.GetString(dbRead.GetOrdinal("Organization")));

                        cboVisit.Items.Add(new ListItem(sVisit, dbRead.GetInt32(dbRead.GetOrdinal("VisitID"))));
                    }
                }
            }
        }

        #endregion

        private void cmdOK_Click(object sender, EventArgs e)
        {

            if (!ValidateForm())
                return;

            using (OleDbConnection dbCon = new OleDbConnection(DBCon))
            {
                dbCon.Open();

                OleDbCommand dbCom = null;
                if (LogID > 0)
                {
                    dbCom = new OleDbCommand("UPDATE LogFeedback SET UserName = @UserName, QualityRatingID = @QualityRatingID, ItemReviewed = @ItemReviewed, WatershedID = @WatershedID, SiteID = @SiteID, VisitID = @VisitID, Description = @Description, ReviewedOn = @ReviewedON WHERE LogID = @LogID", dbCon);
                    dbCom.Parameters.AddWithValue("LogID", LogID);
                }
                else
                    dbCom = new OleDbCommand("INSERT INTO LogFeedback (UserName, QualityRatingID, ItemReviewed, WatershedID, SiteID, VisitID, Description, ReviewedOn)" +
                        " VALUES (@UserName, @QualityRatingID, @ItemReviewed, @WatershedID, @SiteID, @VisitID, @Description, @ReviewedOn)", dbCon);

                dbCom.Parameters.AddWithValue("UserName", txtUserName.Text);
                dbCom.Parameters.AddWithValue("QualityRatingID", ((ListItem)cboQualityRating.SelectedItem).Value);
                dbCom.Parameters.AddWithValue("ItemReviewed", cboItemReviewed.Text);

                OleDbParameter pWatershedID = dbCom.Parameters.Add("WatershedID", OleDbType.Integer);
                if (cboWatershed.SelectedItem is ListItem)
                    pWatershedID.Value = ((ListItem)cboWatershed.SelectedItem).Value;
                else
                    pWatershedID.Value = DBNull.Value;

                OleDbParameter pSiteID = dbCom.Parameters.Add("SiteID", OleDbType.Integer);
                if (cboSite.SelectedItem is ListItem)
                    pSiteID.Value = ((ListItem)cboSite.SelectedItem).Value;
                else
                    pSiteID.Value = DBNull.Value;

                OleDbParameter pVisitID = dbCom.Parameters.Add("VisitID", OleDbType.Integer);
                if (cboVisit.SelectedItem is ListItem)
                    pVisitID.Value = ((ListItem)cboVisit.SelectedItem).Value;
                else
                    pVisitID.Value = DBNull.Value;

                OleDbParameter pDescription = dbCom.Parameters.Add("Description", OleDbType.LongVarChar);
                if (string.IsNullOrEmpty(txtDescription.Text))
                    pDescription.Value = DBNull.Value;
                else
                {
                    pDescription.Value = txtDescription.Text;
                    pDescription.Size = txtDescription.Text.Length;
                }

                OleDbParameter pReviewedOn = dbCom.Parameters.Add("ReviewedOn", OleDbType.Date);
                pReviewedOn.Value = dtDateTime.Value;

                try
                {
                    dbCom.ExecuteNonQuery();
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
    }
}

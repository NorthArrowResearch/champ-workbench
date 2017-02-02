using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CHaMPWorkbench
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblVersion.Text = CHaMPWorkbench.Properties.Resources.MyApplicationNameLong + " version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (string.IsNullOrEmpty(DBCon.DatabasePath))
            {
                lblDBVersion.Visible = false;
            }
            else
            {
                try
                {
                    using (SQLiteConnection dbCon = new SQLiteConnection(DBCon.ConnectionString))
                    {
                        dbCon.Open();
                        SQLiteCommand dbCom = new SQLiteCommand("SELECT ValueInfo FROM VersionInfo WHERE Key = 'DatabaseVersion'", dbCon);
                        String sVersion = (string)dbCom.ExecuteScalar();
                        if (String.IsNullOrWhiteSpace(sVersion))
                            throw new Exception("Error retrieving database version");
                        lblDBVersion.Text = "Database version: " + sVersion;
                    }

                }
                catch (Exception ex)
                {
                    Exception ex2 = new Exception("Error retrieving database version.", ex);
                    Classes.ExceptionHandling.NARException.HandleException(ex2);
                }
            }

            lblWebSite.Text = "Web Site: " + CHaMPWorkbench.Properties.Resources.WebSiteURL;
            lblWebSite.LinkArea = new LinkArea(lblWebSite.Text.Length - CHaMPWorkbench.Properties.Resources.WebSiteURL.Length, CHaMPWorkbench.Properties.Resources.WebSiteURL.Length);
            lblWebSite.Links[0].LinkData = CHaMPWorkbench.Properties.Resources.WebSiteURL;
            lblWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);

        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }
    }
}

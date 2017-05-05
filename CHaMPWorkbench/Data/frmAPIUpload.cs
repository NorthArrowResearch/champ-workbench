using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CHaMPWorkbench.Data
{
    public partial class frmAPIUpload : Form
    {

        private naru.ui.SortableBindingList<ProjectProperty> ProjectProperties { get; set; }
        OpenFileDialog frmBrowseProject;
        private frmKeystoneCredentials CredentialsForm;
        private string UserName { get; set; }
        private string Password { get; set; }

        public frmAPIUpload()
        {
            InitializeComponent();
        }

        private void frmAPIUpload_Load(object sender, EventArgs e)
        {
            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.AllowUserToOrderColumns = false;
            grdData.AllowUserToResizeRows = false;
            grdData.RowHeadersVisible = false;

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.Name = "colName";
            colName.HeaderText = "Property";
            colName.ReadOnly = true;
            colName.DataPropertyName = "PropertyName";
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grdData.Columns.Add(colName);

            DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
            colValue.Name = "colValue";
            colValue.HeaderText = "Value";
            colValue.ReadOnly = true;
            colValue.DataPropertyName = "PropertyValue";
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grdData.Columns.Add(colValue);

            MakeProgressVisible(false);
        }

        private void txtProjectFile_TextChanged(object sender, EventArgs e)
        {
            ProjectProperties = new naru.ui.SortableBindingList<ProjectProperty>();
            grdData.DataSource = ProjectProperties;

            if (string.IsNullOrEmpty(txtProjectFile.Text) || !System.IO.File.Exists(txtProjectFile.Text))
                return;

            XmlDocument xmlProj = new XmlDocument();
            xmlProj.Load(txtProjectFile.Text);

            LoadProjectProperty(ref xmlProj, "Project Created On", "/Project/MetaData/Meta[@name='CreatedOn']");
            LoadProjectProperty(ref xmlProj, "Site Name", "/Project/MetaData/Meta[@name='SiteName']");
            LoadProjectProperty(ref xmlProj, "Field Season", "/Project/MetaData/Meta[@name='FieldSeason']");
            LoadProjectProperty(ref xmlProj, "Watershed", "/Project/MetaData/Meta[@name='Watershed']");
            LoadProjectProperty(ref xmlProj, "Stream Name", "/Project/MetaData/Meta[@name='StreamName']");
            LoadProjectProperty(ref xmlProj, "Protocol", "/Project/MetaData/Meta[@name='Protocol']");
            LoadProjectProperty(ref xmlProj, "Organization", "/Project/MetaData/Meta[@name='Organization']");
            LoadProjectProperty(ref xmlProj, "Survey Crew", "/Project/MetaData/Meta[@name='Survey Crew']");
            LoadProjectProperty(ref xmlProj, "Visit Type", "/Project/MetaData/Meta[@name='VisitType']");

        }

        private void MakeProgressVisible(Boolean bVisible)
        {
            int nResize = 0;
            if (grpProgress.Visible && !bVisible)
                nResize = grpProgress.Top - cmdStart.Top;
            else if (!grpProgress.Visible && bVisible)
                nResize = grpProgress.Height + cmdStart.Top - groupBox1.Bottom;

            grpProgress.Visible = bVisible;

            this.Height += nResize;
        }

        private void LoadProjectProperty(ref XmlDocument xmlProject, string sPropertyDisplayName, string sXPath)
        {
            XmlNode nodProperty = xmlProject.SelectSingleNode(sXPath);
            if (nodProperty is XmlNode)
            {
                string sValue = nodProperty.InnerText;
                DateTime dtValue;
                if (!string.IsNullOrEmpty(sValue) && DateTime.TryParse(sValue, out dtValue))
                    ProjectProperties.Add(new ProjectProperty(sPropertyDisplayName, string.Format("{0:dd-MMM-yyyy}", dtValue)));
                else
                    ProjectProperties.Add(new ProjectProperty(sPropertyDisplayName, sValue));
            }
        }

        private class ProjectProperty
        {
            public string PropertyName { get; internal set; }
            public string PropertyValue { get; internal set; }

            public ProjectProperty(string sName, string sValue)
            {
                PropertyName = sName;
                PropertyValue = sValue;
            }
        }

        private void cmdBrowseProject_Click(object sender, EventArgs e)
        {
            if (frmBrowseProject == null)
            {
                frmBrowseProject = new OpenFileDialog();
                frmBrowseProject.Title = "Select Topo Survey Project File";
                frmBrowseProject.Filter = "Topo Survey Project Files (*.rs.xml)|*.rs.xml";
                frmBrowseProject.AddExtension = true;
            }

            if (!string.IsNullOrEmpty(txtProjectFile.Text) && System.IO.File.Exists(txtProjectFile.Text))
            {
                frmBrowseProject.InitialDirectory = System.IO.Path.GetDirectoryName(txtProjectFile.Text);
                frmBrowseProject.FileName = System.IO.Path.GetFileNameWithoutExtension(txtProjectFile.Text);
            }

            if (frmBrowseProject.ShowDialog() == DialogResult.OK)
            {
                txtProjectFile.Text = frmBrowseProject.FileName;
            }
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (CredentialsForm == null)
                CredentialsForm = new frmKeystoneCredentials();

            if (CredentialsForm.ShowDialog() == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            MakeProgressVisible(true);

            try
            {
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = DialogResult.None;
            }

        }

        private void MessagePosted(Classes.APIZipUploader.MessageEventArgs e)
        {
            StringBuilder sbr = new StringBuilder(txtProjectFile.Text);
            sbr.AppendLine(e.Message);
            txtProjectFile.Text = sbr.ToString();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Classes.APIZipUploader zip = new Classes.APIZipUploader(naru.db.sqlite.DBCon.ConnectionString, this.UserName, this.Password);
            zip.Run(new System.IO.DirectoryInfo(txtProjectFile.Text));
        }
    }
}

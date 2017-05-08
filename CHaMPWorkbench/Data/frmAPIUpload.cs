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

        private StringBuilder sbMessages;

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

            bgWorker.WorkerReportsProgress = true;
            cmdBrowseProject.Select();
        }

        private void txtProjectFile_TextChanged(object sender, EventArgs e)
        {
            ProjectProperties = new naru.ui.SortableBindingList<ProjectProperty>();
            grdData.DataSource = ProjectProperties;

            if (string.IsNullOrEmpty(txtProjectFile.Text) || !System.IO.File.Exists(txtProjectFile.Text))
                return;

            if (!txtProjectFile.Text.ToLower().EndsWith(".rs.xml"))
            {
                MessageBox.Show("The selected file does not appear to be a valid topo survey project file. Please choose a file that ends with *.rs.xml", Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtProjectFile.TextChanged -= txtProjectFile_TextChanged;
                txtProjectFile.Text = string.Empty;
                txtProjectFile.TextChanged += txtProjectFile_TextChanged;
                cmdBrowseProject.Select();
                return;
            }

            XmlDocument xmlProj = new XmlDocument();

            try
            {
                xmlProj.Load(txtProjectFile.Text);

                LoadProjectProperty(ref xmlProj, "Project Created On", "/Project/MetaData/Meta[@name='CreatedOn']");
                LoadProjectProperty(ref xmlProj, "Visit ID", "/Project/MetaData/Meta[@name='VisitID']");
                LoadProjectProperty(ref xmlProj, "Site Name", "/Project/MetaData/Meta[@name='SiteName']");
                LoadProjectProperty(ref xmlProj, "Field Season", "/Project/MetaData/Meta[@name='FieldSeason']");
                LoadProjectProperty(ref xmlProj, "Watershed", "/Project/MetaData/Meta[@name='Watershed']");
                LoadProjectProperty(ref xmlProj, "Stream Name", "/Project/MetaData/Meta[@name='StreamName']");
                LoadProjectProperty(ref xmlProj, "Protocol", "/Project/MetaData/Meta[@name='Protocol']");
                LoadProjectProperty(ref xmlProj, "Organization", "/Project/MetaData/Meta[@name='Organization']");
                LoadProjectProperty(ref xmlProj, "Survey Crew", "/Project/MetaData/Meta[@name='Survey Crew']");
                LoadProjectProperty(ref xmlProj, "Visit Type", "/Project/MetaData/Meta[@name='VisitType']");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load topo survey project file. Ensure that the selected file is a valid topo survey project file ending with *.rs.xml", Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtProjectFile.TextChanged -= txtProjectFile_TextChanged;
                txtProjectFile.Text = string.Empty;
                txtProjectFile.TextChanged += txtProjectFile_TextChanged;
                cmdBrowseProject.Select();
            }

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
            else
                ProjectProperties.Add(new ProjectProperty(sPropertyDisplayName, string.Empty));
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
            if (string.IsNullOrEmpty(txtProjectFile.Text) || !System.IO.File.Exists(txtProjectFile.Text))
            {
                MessageBox.Show("You must choose a valid topo survey project file before you can proceed.", Properties.Resources.MyApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (CredentialsForm == null)
                CredentialsForm = new frmKeystoneCredentials();

            if (CredentialsForm.ShowDialog() == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            cmdCancel.Enabled = false;
            cmdStart.Enabled = false;
            cmdBrowseProject.Enabled = false;

            try
            {
                bgWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
                this.DialogResult = DialogResult.None;
                cmdStart.Visible = true;
                cmdStart.Enabled = true;
                cmdCancel.Enabled = true;
            }
        }

        private void MessagePosted(object sender, EventArgs e)
        {
            sbMessages.AppendLine(((Classes.APIZipUploader.MessageEventArgs)e).Message);
            bgWorker.ReportProgress(0);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Classes.APIZipUploader zip = new Classes.APIZipUploader(naru.db.sqlite.DBCon.ConnectionString, this.UserName, this.Password);

            zip.MessagePosted += new EventHandler(MessagePosted);
            sbMessages = new StringBuilder();
            zip.Run(new System.IO.FileInfo(txtProjectFile.Text));
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtMessages.Text = sbMessages.ToString();
            txtMessages.SelectionStart = txtMessages.Text.Length;
            txtMessages.ScrollToCaret();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdStart.Visible = false;
            cmdCancel.Text = "Close";
            cmdCancel.Enabled = true;
            cmdCancel.Select();
        }
    }
}

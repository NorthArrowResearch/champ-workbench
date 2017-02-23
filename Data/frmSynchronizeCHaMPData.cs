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
        BindingList<CHaMPData.Program> Programs;

        public frmSynchronizeCHaMPData()
        {
            InitializeComponent();
        }

        private void frmSynchronizeCHaMPData_Load(object sender, EventArgs e)
        {
            Programs = new BindingList<CHaMPData.Program>(CHaMPData.Program.Load(naru.db.sqlite.DBCon.ConnectionString).Values.ToList<CHaMPData.Program>());

            lstPrograms.DataSource = Programs;
            lstPrograms.DisplayMember = "Name";
            lstPrograms.ValueMember = "ID";

            //naru.db.sqlite.CheckedListItem.LoadCheckListbox(ref lstPrograms, naru.db.sqlite.DBCon.ConnectionString, "SELECT ProgramID, Title FROM LookupPrograms WHERE (API IS NOT NULL) ORDER BY Title", true);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                CHaMPData.DataSynchronizer sync = new CHaMPData.DataSynchronizer();

                foreach (CHaMPData.Program aProgram in lstPrograms.CheckedItems)
                    sync.Run(aProgram);

                // DO API things here.

                //Keystone.API.KeystoneApiHelper key = new Keystone.API.KeystoneApiHelper()

                //GeoOptix.API.ApiHelper api = new GeoOptix.API.ApiHelper("https://qa.champmonitoring.org/api/v1/visits"
                //    , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
                //    , "NorthArrowDev"
                //    , "C0116A2B-9508-485D-8C22-4373296FF60E"
                //    , "MattReimer"
                //    , "Q1FE!O52&RpBv!s%");

                //api.AuthToken.

                //GeoOptix.API.ApiResponse<GeoOptix.API.Model.VisitSummaryModel[]> visit = api.Get<GeoOptix.API.Model.VisitSummaryModel[]>(); //GeoOptix.API.ApiResponse<GeoOptix.API.Model.VisitSummaryModel>
                //GeoOptix.API.Model.VisitSummaryModel aVisit = visit.Payload;

                //GeoOptix.API.ApiHelper api2 = new GeoOptix.API.ApiHelper(aVisit.SiteUrl, api.AuthToken);
                //GeoOptix.API.ApiResponse<GeoOptix.API.Model.SiteModel> aSiteResp = api.Get<GeoOptix.API.Model.SiteModel>();
                //GeoOptix.API.Model.SiteModel aSite = aSiteResp.Payload;

                Console.Write("hi");

            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
            }
        }
    }
}

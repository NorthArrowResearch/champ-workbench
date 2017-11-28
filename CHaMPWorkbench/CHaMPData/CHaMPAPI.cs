using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench.CHaMPData
{
    public class CHaMPAPI
    {
        public GeoOptix.API.ApiHelper APIHelper;
        public CHaMPData.Program APIHelperProgram;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="program">The program we're going to call</param>
        /// <param name="usr">Username</param>
        /// <param name="pwd">Password</param>
        public CHaMPAPI(CHaMPData.Program program, string usr, string pwd)
        {
            APIHelperProgram = program;
            APIHelper = new GeoOptix.API.ApiHelper(APIHelperProgram.API, APIHelperProgram.Keystone,
                                Properties.Settings.Default.GeoOptixClientID,
                                Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(),
                                usr, pwd);
        }


        /// <summary>
        /// GeoOptix doesn't handle file downloads so this should do it.
        /// </summary>
        /// <param name="url"></param>
        public void Downloadfile(APIFileFolder ff, FileInfo localpath)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers["Authorization"] = "Bearer " + APIHelper.AuthToken.AccessToken;
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new Uri(string.Format("{0}?Download",ff.URL)), localpath.FullName);
            }

        }

        /// <summary>
        /// This is an example of how we can track progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar.Value = e.ProgressPercentage;
        }


    }
}

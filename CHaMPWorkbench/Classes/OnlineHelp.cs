using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench
{
    class OnlineHelp
    {
        public static void FormHelp(string sFormName)
        {
            string sFormHelpURL = string.Empty;
            try
            {
                sFormHelpURL = CHaMPWorkbench.Properties.Resources.ResourceManager.GetString("Help_" + sFormName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WARNING: Missing help URL for form '{0}'.", sFormName);
                return;
            }

            if (string.IsNullOrEmpty(sFormHelpURL))
            {
                System.Windows.Forms.MessageBox.Show("There is no online help page for this topic.", CHaMPWorkbench.Properties.Resources.MyApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {

                Uri baseUri = new Uri(CHaMPWorkbench.Properties.Resources.WebSiteURL);
                Uri myUri = new Uri(baseUri, sFormHelpURL);
                System.Diagnostics.Process.Start(myUri.AbsoluteUri);
            }
        }
    }
}
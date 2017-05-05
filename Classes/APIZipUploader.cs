using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHaMPWorkbench.Classes
{
    public class APIZipUploader
    {
        public string DBCon { get; internal set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        public event EventHandler MessagePosted;

        public APIZipUploader(string sDBCon, string sUserName, string sPassword)
        {
            DBCon = sDBCon;
            UserName = sUserName;
            Password = sPassword;
        }

        public bool Run(System.IO.DirectoryInfo diProject)
        {
            if (!diProject.Exists)
                throw new Exception("The project folder does not exist");

            System.IO.FileInfo fiZip = GetUniqueFilePath(System.IO.Path.GetTempPath(), "TopoData", "zip");

            if (ProjectFileLocksExist(diProject))
            {
                OnMessagePosted(new MessageEventArgs("WARNING: Ensure that all ESRI software is closed, including ArcGIS ArcMap."));
                return false;
            }

            return true;
        }

        protected virtual void OnMessagePosted(MessageEventArgs e)
        {
            EventHandler handler = MessagePosted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private System.IO.FileInfo GetUniqueFilePath(string sDirectory, string sFileName, string sExtension)
        {

            int nCounter = 0;
            string sPath = string.Empty;

            do
            {
                sPath = sFileName;
                if (nCounter > 0)
                {
                    string.Format("{0}{1:0000}", sPath, nCounter);
                }

                sPath = System.IO.Path.Combine(sDirectory, sPath);
                sPath = System.IO.Path.ChangeExtension(sPath, sExtension);
            } while (System.IO.File.Exists(sPath));

            return new System.IO.FileInfo(sPath);

        }

        private bool ProjectFileLocksExist(System.IO.DirectoryInfo diProject)
        {
            bool bLockedFilesExist = false;
            foreach (System.IO.FileInfo fiFile in diProject.GetFiles("*", System.IO.SearchOption.AllDirectories))
            {
                if (FileInUse(fiFile))
                {
                    OnMessagePosted(new MessageEventArgs(string.Format("WARNING: Locked file at {0}", fiFile)));
                    bLockedFilesExist = true;
                }
            }

            return bLockedFilesExist;
        }

        public bool FileInUse(System.IO.FileInfo fiFile)
        {
            if (fiFile.Exists)
            {
                try
                {
                    System.IO.FileStream F = System.IO.File.Open(fiFile.FullName, System.IO.FileMode.Append, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Write);
                    F.Close();
                }
                catch
                {
                    return true;
                }
            }
            return false;
        }

        public class MessageEventArgs : EventArgs
        {
            public string Message { get; set; }

            public MessageEventArgs(string sMessage)
            {
                Message = sMessage;
            }
        }
    }
}

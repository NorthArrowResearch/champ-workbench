using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    public class APIZipUploader
    {
        public string DBCon { get; internal set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        #region Message Handling

        // Forms using this class should add handler for this event to rerieve message updates.
        public event EventHandler MessagePosted;

        /// <summary>
        /// Report messages back to UI forms that have added a handler to listen for messages.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessagePosted(MessageEventArgs e)
        {
            EventHandler handler = MessagePosted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public class MessageEventArgs : EventArgs
        {
            public string Message { get; set; }

            public MessageEventArgs(string sMessage)
            {
                Message = sMessage;
            }
        }

        #endregion

        public APIZipUploader(string sDBCon, string sUserName, string sPassword)
        {
            DBCon = sDBCon;
            UserName = sUserName;
            Password = sPassword;
        }

        public void Run(System.IO.FileInfo fiProjectFile)
        {
            if (!fiProjectFile.Exists)
                throw new Exception("The project file does not exist");

            try
            {
                long nVisitID = 0;
                GetProjectVisitID(fiProjectFile, out nVisitID);

                if (ProjectFileLocksExist(fiProjectFile.Directory))
                {
                    throw new Exception("One or more project files are locked. Ensure that all ESRI products are closed (including ArcMap and ArcCatalog)" +
                        " as well as other software that might be using project files, and then try again.");
                }

                ZipProject(fiProjectFile.Directory, nVisitID);

                OnMessagePosted(new MessageEventArgs("Process completed successfully."));
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs("Process aborted due to errors."));
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="diProject"></param>
        /// <param name="VisitID"></param>
        /// <returns></returns>
        /// <remarks>File Size formatting taken from
        /// http://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
        /// </remarks>
        private string ZipProject(System.IO.DirectoryInfo diProject, long VisitID)
        {
            string sZipFilePath = GetZipFilePath(System.IO.Path.GetTempPath(), VisitID);
            OnMessagePosted(new MessageEventArgs(string.Format("Compressing project to {0}", sZipFilePath)));

            try
            {
                System.IO.Compression.ZipFile.CreateFromDirectory(diProject.FullName, sZipFilePath, System.IO.Compression.CompressionLevel.Optimal, false);

                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = new System.IO.FileInfo(sZipFilePath).Length;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }

                OnMessagePosted(new MessageEventArgs(string.Format ("Project successfully compressed to {0:0.##} {1} temporary file.", len, sizes[order])));
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs(ex.Message));
                throw;
            }

            return sZipFilePath;
        }

        private string GetZipFilePath(string sFolder, long VisitID)
        {
            int nCounter = 1;
            string sFilePath = string.Empty;
            do
            {
                sFilePath = System.IO.Path.Combine(sFolder, string.Format("TopoData_Visit_{0}_{1:0000}.zip", VisitID, nCounter));
                nCounter++;
            } while (System.IO.File.Exists(sFilePath));

            return sFilePath;
        }

        private void GetProjectVisitID(System.IO.FileInfo fiProjectFile, out long VisitID)
        {
            VisitID = 0;

            try
            {
                XmlDocument xmlProj = new XmlDocument();
                xmlProj.Load(fiProjectFile.FullName);
                XmlNode nodProperty = xmlProj.SelectSingleNode("/Project/MetaData/Meta[@name='VisitID']");
                if (nodProperty is XmlNode)
                {
                    if (string.IsNullOrEmpty(nodProperty.InnerText))
                    {
                        throw new Exception("ERROR: Empty Visit ID metadata property in topo survey project file.");
                    }
                    else
                    {
                        if (!long.TryParse(nodProperty.InnerText, out VisitID))
                        {
                            throw new Exception("ERROR: The Visit ID XML metadata property could not be parsed as long integer from the topo survey project file.");
                        }
                    }
                }
                else
                    throw new Exception("ERROR: The Visit ID XML metadata property is missing from the topo survey project file.");
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs(ex.Message));
                throw;
            }

            OnMessagePosted(new MessageEventArgs(string.Format("Topo project identified as Visit ID {0}", VisitID)));
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
                    System.IO.FileStream F = System.IO.File.Open(fiFile.FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Write);
                    F.Close();
                }
                catch (Exception ex)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

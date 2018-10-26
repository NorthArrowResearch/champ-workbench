using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Keystone.API;
using GeoOptix.API;

namespace CHaMPWorkbench.Classes
{
    public class APIZipUploader : GeoOptixFeature
    {
        private enum DataSetTypes : int
        {
            AuxiliaryDataFiles = 2,
            TopographicData = 3,
            SitePhotos = 4,
            AirTempReadings = 5,
            StreamTempReadings = 6,
            SolarInputPhotos = 7,
            ScannedPaperFormsandMaps = 8
        }

        private static int CHUNK_SIZE = (int)Math.Pow(20, 6); // 20Mb default

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
                string sValidationStatus = string.Empty;

                VerifyValidationStatus(fiProjectFile, out sValidationStatus);

                if (ProjectFileLocksExist(fiProjectFile.Directory))
                {
                    throw new Exception("One or more project files are locked. Ensure that all ESRI products are closed (including ArcMap and ArcCatalog)" +
                        " as well as other software that might be using project files, and then try again.");
                }

                string sZipSize = string.Empty;
                CHaMPData.VisitBasic visit;
                CHaMPData.Program program;

                visit = GetProjectVisit(fiProjectFile);
                program = GetProgram(visit);

                System.IO.FileInfo fiZipFile = ZipProject(fiProjectFile.Directory, visit.ID, out sZipSize);

                string sDescription = string.Format("{0} Visit {1} topo data project zip ({2}) with validation status of '{3}'.", program, visit.ID, sZipSize, sValidationStatus);
                UploadZipFile(fiZipFile, visit, program, sDescription);

                OnMessagePosted(new MessageEventArgs("Process completed successfully."));
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs("Process aborted due to errors."));
            }
        }

        private void UploadZipFile(System.IO.FileInfo fiZipFile, CHaMPData.VisitBasic visit, CHaMPData.Program program, string sDescription)
        {
            // Determine if the program is pointing at QA or Production and use the corresponding keystone
            string keystoneURL = "https://keystone.sitkatech.com/core/connect/token";
            if (program.API.Contains("https://qa."))
                keystoneURL = keystoneURL.Replace("https://", "https://qa.");

            try
            {
                ApiHelper helper = new ApiHelper(program.API, keystoneURL, GeoOptixClientID, GeoOptixClientSecret.ToUpper(), UserName, Password);

                // First we go see if there is a file there already. with GET/visits/1/fieldFolders/Topo/files/Filename.zip
                var hashcode = ApiHelper.GetFileHashCode(fiZipFile.FullName);
                var transferDetail = new GeoOptix.API.Model.TransferDetail
                {
                    datasetName = "Topo",
                    visitId = (int)visit.ID,
                    manifest = new[] { new GeoOptix.API.Model.TransferManifestFile { hash = hashcode, name = "TopoData.zip" }, }
                };

                var response = helper.CreateTransfer(transferDetail);
                var transfer = response.Payload;
                var resp = helper.UploadTransferFile(transfer.id, fiZipFile.FullName, CHUNK_SIZE);
                helper.CloseTransfer(transfer.id);
                Console.WriteLine("Here");
            }
            catch (Exception ex)
            {
                Classes.ExceptionHandling.NARException.HandleException(ex);
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
        private System.IO.FileInfo ZipProject(System.IO.DirectoryInfo diProject, long VisitID, out string sZipSize)
        {
            sZipSize = string.Empty;
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

                sZipSize = string.Format(" {0:0.##} {1}", len, sizes[order]);
                OnMessagePosted(new MessageEventArgs(string.Format("Project successfully compressed to {0} temporary file.", sZipSize)));
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs(ex.Message));
                throw;
            }

            return new System.IO.FileInfo(sZipFilePath);
        }

        private string GetZipFilePath(string sFolder, long VisitID)
        {
            string filePath = System.IO.Path.Combine(sFolder, string.Format("TopoUpload_Visit{0}_{1:yyyyMMddHHmmss}", VisitID, DateTime.Now));
            System.IO.Directory.CreateDirectory(filePath);
            filePath = System.IO.Path.Combine(filePath, "TopoData.zip");
            return filePath;
        }

        private CHaMPData.Program GetProgram(CHaMPData.VisitBasic visit)
        {
            CHaMPData.Program theProgram = null;
            try
            {
                Dictionary<long, CHaMPData.Program> programs = CHaMPData.Program.Load(DBCon);
                theProgram = programs[visit.ProgramID];

                if (string.IsNullOrEmpty(theProgram.API))
                    throw new Exception(string.Format("Visit {0} is associated with the {1} program which is missing an API URL. Contact the {2} developers.", visit.ID, theProgram, Properties.Resources.MyApplicationNameLong));

                OnMessagePosted(new MessageEventArgs(string.Format("Visit associated with the {0} program.", theProgram)));
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs(string.Format("Error determining visit program: {0}", ex.Message)));
                throw;
            }

            return theProgram;
        }

        private CHaMPData.VisitBasic GetProjectVisit(System.IO.FileInfo fiProjectFile)
        {
            CHaMPData.VisitBasic visit = null;

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
                        long nVisitID = 0;
                        if (long.TryParse(nodProperty.InnerText, out nVisitID))
                        {
                            visit = CHaMPData.VisitBasic.Load(nVisitID);
                        }
                        else
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

            OnMessagePosted(new MessageEventArgs(string.Format("Topo project identified as Visit ID {0}", visit.ID)));
            return visit;
        }

        private void VerifyValidationStatus(System.IO.FileInfo fiProjectFile, out string sValidationStatus)
        {
            sValidationStatus = string.Empty;
            string sMessage = string.Empty;

            try
            {
                XmlDocument xmlProj = new XmlDocument();
                xmlProj.Load(fiProjectFile.FullName);
                XmlNode nodProperty = xmlProj.SelectSingleNode("/Project/MetaData/Meta[@name='ValidationStatus']");
                if (nodProperty is XmlNode)
                {
                    if (string.IsNullOrEmpty(nodProperty.InnerText))
                    {
                        throw new Exception("ERROR: Empty validation status metadata property in topo survey project file.");
                    }
                    else
                    {
                        sValidationStatus = nodProperty.InnerText;
                    }
                }

                switch (sValidationStatus.ToLower())
                {
                    case "pass":
                    case "review":
                        OnMessagePosted(new MessageEventArgs(string.Format("Topo project validation status confirmed as '{0}'.", nodProperty.InnerText)));
                        break;

                    case "fail":
                        throw new Exception("Project failed topo data validation. Use the CHaMP Topo Toolbar to fix all validation issues. Then re-publish the survey and attempt to upload again.");

                    default:
                        throw new Exception("The topo project is missing a validation status. Use the CHaMP Topo Toolbar to run validation and re-publish the survey. Then attempt to upload again.");
                }
            }
            catch (Exception ex)
            {
                OnMessagePosted(new MessageEventArgs(ex.Message));
                throw;
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

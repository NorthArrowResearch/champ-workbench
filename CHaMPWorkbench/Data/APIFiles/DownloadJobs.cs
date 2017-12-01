using System;
using System.Threading.Tasks;
using CHaMPWorkbench.CHaMPData;
using System.IO;
using System.Net;
using System.Threading;

namespace CHaMPWorkbench.Data.APIFiles
{
    /// <summary>
    /// These helper classs come in super useful
    /// 
    /// There are 4 types:
    ///    - Job : The base class. All others are derivatives of it
    ///    - GetFolderFilesJob: Get files from API folders and throw them on the queue
    ///    - GetFieldFolderFilesJob:  Get field files from API folders and throw them on the queue
    ///    - DownloadJob: Download the actual file
    /// </summary>
    abstract class Job
    {
        protected APIFileFolder ff;
        protected FileInfo fiLocalfile;
        protected GeoOptix.API.ApiHelper api;
        protected string sRelativePath;
        protected CancellationToken canceltoken;

        public bool bOverwrite;
        public bool bCreateDir;

        public DownloadProgressChangedEventHandler ProgressChanged;

        public event EventHandler<string> LoggerHandler;
        public event EventHandler<Job> AddNewJobHandler;

        public Job(APIFileFolder iff, FileInfo ifiLocalFile, GeoOptix.API.ApiHelper iapi, string isRelativePath, bool bCreated, bool bOverW, CancellationToken ct)
        {
            ff = iff;
            fiLocalfile = ifiLocalFile;
            api = iapi;
            sRelativePath = isRelativePath;
            bOverwrite = bOverW;
            bCreateDir = bCreated;
            canceltoken = ct;
        }
        /// <summary>
        /// This helper only invokes the event if there is something attached to it
        /// </summary>
        /// <param name="msg"></param>
        protected void SendLogMessage(string msg) { LoggerHandler?.Invoke(this, msg); }
        protected void SendJobToQueue(Job job) { AddNewJobHandler?.Invoke(this, job); }

        /// <summary>
        /// All following classes must implement this method
        /// </summary>
        /// <returns></returns>
        public abstract Task Run();
    }
    class GetFolderFilesJob : Job
    {
        public GetFolderFilesJob(APIFileFolder iff, FileInfo ifiLocalFile, GeoOptix.API.ApiHelper iapi, string isRelativePath, bool bCreateDir, bool bOverwrite, CancellationToken ct)
            : base(iff, ifiLocalFile, iapi, isRelativePath, bCreateDir, bOverwrite, ct) { }

        public override Task Run()
        {
            SendLogMessage(String.Format("{0}Collecting files for: {1}...", Environment.NewLine, sRelativePath));
            foreach (APIFileFolder ffile in VisitWithFiles.GetFolderFiles(ff, api))
                SendJobToQueue(new DownloadJob(ffile, new FileInfo(Path.Combine(fiLocalfile.FullName, ffile.Name)), api, sRelativePath, bCreateDir, bOverwrite, canceltoken));

            return Task.CompletedTask;
        }
    }
    class GetFieldFolderFilesJob : Job
    {
        public GetFieldFolderFilesJob(APIFileFolder iff, FileInfo ifiLocalFile, GeoOptix.API.ApiHelper iapi, string isRelativePath, bool bCreateDir, bool bOverwrite, CancellationToken ct)
            : base(iff, ifiLocalFile, iapi, isRelativePath, bCreateDir, bOverwrite, ct) { }
        public override Task Run()
        {
            SendLogMessage(String.Format("{0}Collecting field files for: {1}...", Environment.NewLine, sRelativePath));
            foreach (APIFileFolder ffile in VisitWithFiles.GetFieldFolderFiles(ff, api))
                SendJobToQueue(new DownloadJob(ffile, new FileInfo(Path.Combine(fiLocalfile.FullName, ffile.Name)), api, sRelativePath, bCreateDir, bOverwrite, canceltoken));
            return Task.CompletedTask;
        }
    }
    class DownloadJob : Job
    {
        public DownloadJob(APIFileFolder iff, FileInfo ifiLocalFile, GeoOptix.API.ApiHelper iapi, string isRelativePath, bool bCreateDir, bool bOverwrite, CancellationToken ct)
         : base(iff, ifiLocalFile, iapi, isRelativePath, bCreateDir, bOverwrite, ct) { }

        public override async Task Run()
        {
            SendLogMessage(String.Format("{0}Downloading {1}...", Environment.NewLine, sRelativePath));

            // Create a folder if we need to 
            if (!fiLocalfile.Directory.Exists)
            {
                if (bCreateDir) fiLocalfile.Directory.Create();
                else
                {
                    SendLogMessage(String.Format("{0}No folder {1}...", Environment.NewLine, sRelativePath));
                    return;
                }
            }

            // Delete an existing file if we need to
            if (fiLocalfile.Exists)
            {
                if (bOverwrite) fiLocalfile.Delete();
                else
                {
                    SendLogMessage(String.Format("{0}Skipping existing {1}...", Environment.NewLine, sRelativePath));
                    return;
                }
            }

            WebClient wc = new WebClient();
            canceltoken.Register(() => wc.CancelAsync());
            wc.Headers["Authorization"] = "Bearer " + api.AuthToken.AccessToken;
            wc.DownloadProgressChanged += ProgressChanged;
            await wc.DownloadFileTaskAsync(new Uri(string.Format("{0}?Download", ff.URL)), fiLocalfile.FullName);
            SendLogMessage(String.Format("{0}Complete {1}", Environment.NewLine, Path.Combine(sRelativePath, ff.Name)));

        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using CHaMPWorkbench.CHaMPData;
using System.ComponentModel;
using System.IO;

namespace CHaMPWorkbench.Data.APIFiles
{

    public class VisitWithFiles : VisitBasic
    {
        public BindingList<VisitBasic> visits { get; internal set; }
        CHaMPData.Program theProg;

        public List<APIFileFolder> FilesAndFolders;

        public FileInfo Destination { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aVisit"></param>
        public VisitWithFiles(VisitBasic aVisit, List<APIFileFolder> allfilefolders, CHaMPData.Program program) : base(aVisit, naru.db.DBState.Unchanged)
        {
            FilesAndFolders = allfilefolders;

            theProg = program;
        }

        public List<string> GetNames { get { return FilesAndFolders.Select(ff => ff.Name).ToList(); } }

        /// <summary>
        /// Get the list of files inside the folder using GeoOptix
        /// </summary>
        /// <param name="ff">APIFileFolder FOLDER to look into</param>
        /// <param name="api">Api helper to use for this operation</param>
        /// <returns></returns>
        public static List<APIFileFolder> GetFolderFiles(APIFileFolder ff, GeoOptix.API.ApiHelper api)
        {
            List<APIFileFolder> retVal = new List<APIFileFolder>();
            GeoOptix.API.ApiResponse<GeoOptix.API.Model.FileSummaryModel[]> filelist = api.GetFiles(ff.Name);

            if (filelist.Payload == null) return retVal;

            foreach (GeoOptix.API.Model.FileSummaryModel file in filelist.Payload)
                if (file.Name != null && file.Url != null)
                    retVal.Add(new APIFileFolder(file.Name, file.Url, ff.Name, true, false, naru.db.DBState.New));

            return retVal;
        }

        /// <summary>
        /// Get the list of field files inside the field folders
        /// </summary>
        /// <param name="ff">APIFileFolder FIELD FOLDER to look into</param>
        /// <param name="api">Api helper to use for this operation</param>
        /// <returns></returns>
        public static List<APIFileFolder> GetFieldFolderFiles(APIFileFolder ff, GeoOptix.API.ApiHelper api)
        {
            List<APIFileFolder> retVal = new List<APIFileFolder>();
            GeoOptix.API.ApiResponse<GeoOptix.API.Model.FileSummaryModel[]> filelist = api.GetFieldFiles(ff.Name);

            if (filelist.Payload == null) return retVal;

            foreach (GeoOptix.API.Model.FileSummaryModel file in filelist.Payload)
                if (file.Name != null && file.Url != null)
                    retVal.Add(new APIFileFolder(file.Name, file.Url, ff.Name, true, true, naru.db.DBState.New));

            return retVal;
        }
    }
    

}

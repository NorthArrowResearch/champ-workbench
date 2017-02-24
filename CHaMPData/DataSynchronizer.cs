using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoOptix.API;
using System.Data.SQLite;
using System.Net.Http;

namespace CHaMPWorkbench.CHaMPData
{
    class DataSynchronizer
    {
        // private Dictionary<long, Watershed> Watersheds;
        private Dictionary<long, Site> Sites;

        private Dictionary<string, long> Protocols;

        // API dictionaries that key the API URL to the item ID
        private Dictionary<string, long> WatershedURLs;
        private Dictionary<string, long> SiteURLs;
        private Dictionary<long, List<string>> VisitURLs; // Program ID keyed to list of visitURLs

        private int TotalNumberVisits;

        public string CurrentProcess { get; internal set; }
        public int Progress { get; internal set; }

        public delegate void ProgressUpdate(int value, string sCurrentProcess);
        public event ProgressUpdate OnProgressUpdate;

        public DataSynchronizer()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                Protocols = LoadLookupList(dbCon, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8");
            }
        }

        public void Run(IEnumerable<CHaMPData.Program> lPrograms)
        {
            WatershedURLs = new Dictionary<string, long>();
            SiteURLs = new Dictionary<string, long>();
            TotalNumberVisits = 0;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();


                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    Keystone.API.AuthResponseModel authToken = null;
                    OnProgressUpdate(0, "Initializing");

                    foreach (Program aProgram in lPrograms)
                    {
                        authToken = SyncWatersheds(ref dbTrans, aProgram);
                        SyncSites(ref dbTrans, aProgram);
                        TotalNumberVisits += GetListOfVisitURLs(aProgram);
                    }

                    // Load all existing visits
                    Dictionary<long, Visit> dvisits = Visit.Load(naru.db.sqlite.DBCon.ConnectionString);

                    // Now synchronize all visits
                    int nVisitCounter = 0;
                    foreach (long nProgramID in VisitURLs.Keys)
                    {
                        foreach (string sVisitURL in VisitURLs[nProgramID])
                        {
                            SyncVisits(ref dbTrans, ref authToken, ref dvisits, sVisitURL, nProgramID);
                            nVisitCounter += 1;
                            OnProgressUpdate(nVisitCounter, sVisitURL);
                        }
                    }

                    // Save the updated list of visits (pass all visits, not just those that have changed because channel units might need updating)
                    OnProgressUpdate(100, "Saving visits to database");
                    CHaMPData.Visit.Save(ref dbTrans, dvisits.Values.ToList<CHaMPData.Visit>());
                                        
                    dbTrans.Commit();
                    OnProgressUpdate(100, "Process Complete");
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
        }



        /// <summary>
        /// General utility method for loading Workbench lookup lists needed as foreign keys when storing visits etc.
        /// </summary>
        /// <param name="dbCon"></param>
        /// <param name="sSQL">First field is ID and second is name</param>
        /// <returns></returns>
        private Dictionary<string, long> LoadLookupList(SQLiteConnection dbCon, string sSQL)
        {
            Dictionary<string, long> dItems = new Dictionary<string, long>();
            using (SQLiteCommand dbCom = new SQLiteCommand(sSQL, dbCon))
            {
                SQLiteDataReader dbRead = dbCom.ExecuteReader();
                while (dbRead.Read())
                    dItems[dbRead.GetString(1)] = dbRead.GetInt64(0);
                dbRead.Close();
            }
            return dItems;
        }

        private Keystone.API.AuthResponseModel SyncWatersheds(ref SQLiteTransaction dbTrans, Program theProgrm)
        {
            Dictionary<long, Watershed> dWatersheds = Watershed.Load(naru.db.sqlite.DBCon.ConnectionString);

            ApiHelper api = new ApiHelper(string.Format("{0}/watersheds", theProgrm.API)
               , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
               , "NorthArrowDev"
               , "C0116A2B-9508-485D-8C22-4373296FF60E"
               , "MattReimer"
               , "Q1FE!O52&RpBv!s%");


            ApiResponse<GeoOptix.API.Model.WatershedSummaryModel[]> response = api.Get<GeoOptix.API.Model.WatershedSummaryModel[]>();
            foreach (GeoOptix.API.Model.WatershedSummaryModel apiWatershed in response.Payload)
            {
                if (dWatersheds.ContainsKey((long)apiWatershed.Id))
                {
                    dWatersheds[(long)apiWatershed.Id].Name = apiWatershed.Name;
                }
                else
                {
                    dWatersheds[(long)apiWatershed.Id] = new Watershed(apiWatershed.Id, apiWatershed.Name, naru.db.DBState.New);
                }
                WatershedURLs[apiWatershed.Url] = (long)apiWatershed.Id;
            }

            Watershed.Save(ref dbTrans, dWatersheds.Values.ToList<Watershed>());


            return api.AuthToken;
        }

        private void SyncSites(ref SQLiteTransaction dbTrans, Program theProgram)
        {
            Dictionary<long, Site> dSites = Site.Load(naru.db.sqlite.DBCon.ConnectionString);

            ApiHelper api = new ApiHelper(string.Format("{0}/sites", theProgram.API)
               , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
               , "NorthArrowDev"
               , "C0116A2B-9508-485D-8C22-4373296FF60E"
               , "MattReimer"
               , "Q1FE!O52&RpBv!s%");

            ApiResponse<GeoOptix.API.Model.SiteSummaryModel[]> response = api.Get<GeoOptix.API.Model.SiteSummaryModel[]>();
            foreach (GeoOptix.API.Model.SiteSummaryModel apiSite in response.Payload)
            {
                if (WatershedURLs.ContainsKey(apiSite.WatershedUrl))
                {
                    if (dSites.ContainsKey((long)apiSite.Id))
                    {
                        dSites[(long)apiSite.Id].Name = apiSite.Name;
                    }
                    else
                    {
                        Nullable<double> fLongitude = new Nullable<double>();
                        Nullable<double> fLatitude = new Nullable<double>();

                        dSites[(long)apiSite.Id] = new CHaMPData.Site(apiSite.Id, apiSite.Name, WatershedURLs[apiSite.WatershedUrl], string.Empty, string.Empty, string.Empty, false, false, false, false, false, false, fLatitude, fLongitude, null, naru.db.DBState.New);
                    }

                    SiteURLs[apiSite.Url] = apiSite.Id;
                }
                else
                {
                    Console.Write("stop");
                }
            }

            Site.Save(ref dbTrans, dSites.Values.ToList<Site>());
        }

        private int GetListOfVisitURLs(Program theProgram)
        {
            if (VisitURLs == null)
                VisitURLs = new Dictionary<long, List<string>>();

            ApiHelper api = new ApiHelper(string.Format("{0}/visits", theProgram.API)
             , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
             , "NorthArrowDev"
             , "C0116A2B-9508-485D-8C22-4373296FF60E"
             , "MattReimer"
             , "Q1FE!O52&RpBv!s%");

            ApiResponse<GeoOptix.API.Model.VisitSummaryModel[]> response = api.Get<GeoOptix.API.Model.VisitSummaryModel[]>();
            foreach (GeoOptix.API.Model.VisitSummaryModel apiVisit in response.Payload)
            {
                if (SiteURLs.ContainsKey(apiVisit.SiteUrl) && apiVisit.SampleYear.HasValue)
                {
                    if (!VisitURLs.ContainsKey(theProgram.ID))
                        VisitURLs[theProgram.ID] = new List<string>();

                    VisitURLs[theProgram.ID].Add(apiVisit.Url);
                }
            }

            int nVisits = 0;
            if (VisitURLs.ContainsKey(theProgram.ID))
                nVisits = VisitURLs[theProgram.ID].Count;
            return nVisits;
        }

        private void SyncVisits(ref SQLiteTransaction dbTrans, ref Keystone.API.AuthResponseModel authToken, ref Dictionary<long, Visit> dvisits, string sVisitURL, long nProgramID)
        {
            ApiHelper api2 = new ApiHelper(sVisitURL, authToken);
            ApiResponse<GeoOptix.API.Model.VisitModel> apiVisitResponse = api2.Get<GeoOptix.API.Model.VisitModel>();
            GeoOptix.API.Model.VisitModel apiVisitDetails = apiVisitResponse.Payload;

            if (apiVisitDetails == null || apiVisitDetails.SampleYear.HasValue)
                return;

            ApiResponse<GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>>> res = api2.GetMeasurement<Dictionary<string, string>>("Visit Information");
            GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>> meas = res.Payload;
            IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> vals = meas.MeasValues;
            GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mv = vals.First<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>>();
            Dictionary<string, string> dMeasurements = mv.Measurement;

            CHaMPData.Visit theVisit = null;
            if (dvisits.ContainsKey((long)apiVisitDetails.Id))
            {
                theVisit = dvisits[(long)apiVisitDetails.Id];
            }
            else
            {
                theVisit = new Visit((long)apiVisitDetails.Id, 0, string.Empty, SiteURLs[apiVisitDetails.SiteUrl], string.Empty, apiVisitDetails.SampleYear.Value, nProgramID, string.Empty, naru.db.DBState.New);
                dvisits[(long)apiVisitDetails.Id] = theVisit;
            }

            theVisit.Hitch = apiVisitDetails.HitchName;
            theVisit.Organization = apiVisitDetails.OrganizationName;
            theVisit.Crew = apiVisitDetails.HitchName;
            theVisit.SampleDate = apiVisitDetails.SampleDate;
            theVisit.ProgramID = nProgramID;
            theVisit.Discharge = GetVisitInfoValue(ref dMeasurements, "TotalDischarge");
            theVisit.D84 = GetVisitInfoValue(ref dMeasurements, "D84");
            theVisit.HasStreamTempLogger = GetVisitInfoValueBool(ref dMeasurements, "HasStreamTempLogger");
            theVisit.HasFishData = GetVisitInfoValueBool(ref dMeasurements, "HasFishData");
            theVisit.ProtocolID = GetLookupListItemID(ref dbTrans, ref Protocols, 8, apiVisitDetails.Protocol);

            theVisit.Panel = apiVisitDetails.Panel;
            theVisit.VisitStatus = apiVisitDetails.Status;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Channel Units
            ApiResponse<GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>>> resCU = api2.GetMeasurement<Dictionary<string, string>>("Channel Unit");
            GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>> measCU = resCU.Payload;
            IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> valsCU = measCU.MeasValues;
            //GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mvCU = valsCU.First<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>>();
            //Dictionary<string, string> dChannelUnits = mvCU.Measurement;
            ChannelUnits(ref theVisit, valsCU);
        }

        private void ChannelUnits(ref CHaMPData.Visit theVisit, IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> lUnits)
        {
            foreach (GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mvCU in lUnits)
            {
                long nChannelUnitNumber = long.Parse(mvCU.Measurement["ChannelUnitNumber"]);
                long nChannelUnitID = long.Parse(mvCU.Measurement["ChannelUnitID"]);
                long nSegmentNumber = long.Parse(mvCU.Measurement["ChannelSegmentID"]);
                string sTier1 = mvCU.Measurement["Tier1"];
                string sTier2 = mvCU.Measurement["Tier2"];

                theVisit.ChannelUnits[nChannelUnitID] = new ChannelUnit(nChannelUnitID, theVisit.ID, nChannelUnitNumber, nSegmentNumber, sTier1, sTier2, naru.db.DBState.New);
            }
        }

        private long? GetLookupListItemID(ref SQLiteTransaction dbTrans, ref Dictionary<string, long> dLookupListValues, long nListID, string sValue)
        {
            long? result = null;
            if (!string.IsNullOrEmpty(sValue))
            {
                if (dLookupListValues.ContainsKey(sValue))
                    result = dLookupListValues[sValue];
                else
                {
                    // Need new lookup item.
                    SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO LookupListItems (Title, ListID) VALUES (@Title, @ListID)", dbTrans.Connection, dbTrans);
                    dbCom.Parameters.AddWithValue("Title", sValue);
                    dbCom.Parameters.AddWithValue("ListID", nListID);
                    dbCom.ExecuteNonQuery();

                    dbCom = new SQLiteCommand("SELECT last_insert_rowid()", dbTrans.Connection, dbTrans);
                    result = (long)dbCom.ExecuteScalar();

                    // Remember to update the dictionary to speed up subsequent uses of this item
                    dLookupListValues[sValue] = result.Value;
                }
            }
            return result;
        }

        private double? GetVisitInfoValue(ref Dictionary<string, string> dMeasurements, string sPropertyName)
        {
            double? fDischarge = null;
            foreach (string sValue in dMeasurements.Keys)
            {
                Console.Write(sValue);
                if (string.Compare(sValue, sPropertyName, true) == 0)
                    if (dMeasurements[sValue] != null)
                        fDischarge = double.Parse(dMeasurements[sValue]);
            }

            return fDischarge;
        }

        private bool GetVisitInfoValueBool(ref Dictionary<string, string> dMeasurements, string sPropertyName)
        {
            bool bValue = false;
            foreach (string sValue in dMeasurements.Keys)
            {
                Console.Write(sValue);
                if (string.Compare(sValue, sPropertyName, true) == 0)
                    if (dMeasurements[sValue] != null)
                        bValue = bool.Parse(dMeasurements[sValue]);
            }
            return bValue;
        }
    }
}

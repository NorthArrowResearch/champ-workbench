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
        private Dictionary<string, long> Protocols;

        public DataSynchronizer()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                Protocols = LoadLookupList(dbCon, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8");
            }
        }

        public void Run(CHaMPData.Program program)
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();


                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    Dictionary<string, long> dWatershedURLs = Watersheds(ref dbTrans);
                    Dictionary<string, long> dSiteURLs = Sites(ref dbTrans, ref dWatershedURLs);

                    Visits(ref dbTrans, ref dSiteURLs, program);

                    dbTrans.Commit();
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

        private Dictionary<string, long> Watersheds(ref SQLiteTransaction dbTrans)
        {
            Dictionary<long, Watershed> dWatersheds = Watershed.Load(naru.db.sqlite.DBCon.ConnectionString);
            Dictionary<string, long> dWatershedURLs = new Dictionary<string, long>();

            ApiHelper api = new ApiHelper("https://qa.champmonitoring.org/api/v1/watersheds"
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
                dWatershedURLs[apiWatershed.Url] = (long)apiWatershed.Id;
            }

            Watershed.Save(ref dbTrans, dWatersheds.Values.ToList<Watershed>());
            return dWatershedURLs;
        }

        private Dictionary<string, long> Sites(ref SQLiteTransaction dbTrans, ref Dictionary<string, long> dWatershedURLs)
        {
            Dictionary<long, Site> dSites = Site.Load(naru.db.sqlite.DBCon.ConnectionString);
            Dictionary<string, long> dSiteURLs = new Dictionary<string, long>();

            ApiHelper api = new ApiHelper("https://qa.champmonitoring.org/api/v1/sites"
               , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
               , "NorthArrowDev"
               , "C0116A2B-9508-485D-8C22-4373296FF60E"
               , "MattReimer"
               , "Q1FE!O52&RpBv!s%");

            ApiResponse<GeoOptix.API.Model.SiteSummaryModel[]> response = api.Get<GeoOptix.API.Model.SiteSummaryModel[]>();
            foreach (GeoOptix.API.Model.SiteSummaryModel apiSite in response.Payload)
            {
                if (dWatershedURLs.ContainsKey(apiSite.WatershedUrl)) // && apisiteDetails != null)
                {
                    long nWatershedID = dWatershedURLs[apiSite.WatershedUrl];

                    if (dSites.ContainsKey((long)apiSite.Id))
                    {
                        dSites[(long)apiSite.Id].Name = apiSite.Name;
                    }
                    else
                    {
                        Nullable<double> fLongitude = new Nullable<double>();
                        Nullable<double> fLatitude = new Nullable<double>();

                        dSites[(long)apiSite.Id] = new CHaMPData.Site(apiSite.Id, apiSite.Name, nWatershedID, string.Empty, string.Empty, string.Empty, false, false, false, false, false, false, fLatitude, fLongitude, null, naru.db.DBState.New);
                    }

                    dSiteURLs[apiSite.Url] = apiSite.Id;
                }
                else
                {
                    Console.Write("stop");
                }
            }

            Site.Save(ref dbTrans, dSites.Values.ToList<Site>());
            return dSiteURLs;
        }

        private void Visits(ref SQLiteTransaction dbTrans, ref Dictionary<string, long> dSitesURLs, Program program)
        {
            Dictionary<long, Visit> dvisits = Visit.Load(naru.db.sqlite.DBCon.ConnectionString);

            ApiHelper api = new ApiHelper("https://qa.champmonitoring.org/api/v1/visits"
               , "https://qa.keystone.sitkatech.com/OAuth2/Authorize"
               , "NorthArrowDev"
               , "C0116A2B-9508-485D-8C22-4373296FF60E"
               , "MattReimer"
               , "Q1FE!O52&RpBv!s%");

            ApiResponse<GeoOptix.API.Model.VisitSummaryModel[]> response = api.Get<GeoOptix.API.Model.VisitSummaryModel[]>();
            foreach (GeoOptix.API.Model.VisitSummaryModel apiVisit in response.Payload)
            {

                ApiHelper api2 = new ApiHelper(apiVisit.Url, api.AuthToken);
                ApiResponse<GeoOptix.API.Model.VisitModel> apiVisitResponse = api2.Get<GeoOptix.API.Model.VisitModel>();
                GeoOptix.API.Model.VisitModel apiVisitDetails = apiVisitResponse.Payload;

                if (apiVisitDetails != null)
                {
                    if (apiVisitDetails.SampleYear.HasValue)
                    {
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
                            long nSiteID = dSitesURLs[apiVisitDetails.SiteUrl];
                            theVisit = new Visit((long)apiVisitDetails.Id, 0, string.Empty, nSiteID, string.Empty, apiVisitDetails.SampleYear.Value, program.ID, string.Empty, naru.db.DBState.New);
                            dvisits[(long)apiVisitDetails.Id] = theVisit;
                        }

                        theVisit.Hitch = apiVisitDetails.HitchName;
                        theVisit.Organization = apiVisitDetails.OrganizationName;
                        theVisit.Crew = apiVisitDetails.HitchName;
                        theVisit.SampleDate = apiVisitDetails.SampleDate;
                        theVisit.ProgramID = program.ID;
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
                        GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mvCU = valsCU.First<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>>();
                        Dictionary<string, string> dChannelUnits = mvCU.Measurement;
                        ChannelUnits(ref theVisit, dChannelUnits);

                        Console.Write("visit");

                    }

                    Console.Write("visit");
                }
            }

            CHaMPData.Visit.Save(ref dbTrans, dvisits.Values.ToList<CHaMPData.Visit>());
        }

        private void ChannelUnits(ref CHaMPData.Visit theVisit, Dictionary<string,string> dChannelUnits)
        {
            long nSegmentID = long.Parse(dChannelUnits["SegmentNumber"]);

            CHaMPData.ChannelSegment theSegment = null;
            if (theVisit.Segments.ContainsKey(nSegmentID))
                theSegment = theVisit.Segments[nSegmentID];
            else
                theSegment = new CHaMPData.ChannelSegment(nSegmentID, nSegmentID.ToString(), nSegmentID)




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

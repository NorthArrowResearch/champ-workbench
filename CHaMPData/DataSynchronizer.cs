using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoOptix.API;
using System.Data.SQLite;

namespace CHaMPWorkbench.CHaMPData
{
    class DataSynchronizer
    {
        public void Run()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    Dictionary<string, long> dWatershedURLs = Watersheds(ref dbTrans);
                    Dictionary<string, long> dSiteURLs = Sites(ref dbTrans, ref dWatershedURLs);

                    Visits(ref dbTrans, ref dSiteURLs);

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
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
                ApiHelper api2 = new ApiHelper(apiSite.Url, api.AuthToken);
                ApiResponse<GeoOptix.API.Model.SiteModel> apiSiteDetailsresp = api2.Get<GeoOptix.API.Model.SiteModel>();
                GeoOptix.API.Model.SiteModel apisiteDetails = apiSiteDetailsresp.Payload;

                if (apisiteDetails != null && dWatershedURLs.ContainsKey(apisiteDetails.WatershedUrl))
                {
                    long nWatershedID = dWatershedURLs[apisiteDetails.WatershedUrl];

                    Nullable<double> fLatitude = new Nullable<double>();
                    if (!string.IsNullOrEmpty(apisiteDetails.Latitude))
                        fLatitude = double.Parse(apisiteDetails.Latitude);

                    Nullable<double> fLongitude = new Nullable<double>();
                    if (!string.IsNullOrEmpty(apisiteDetails.Longitude))
                        fLongitude = double.Parse(apisiteDetails.Longitude);


                    if (dSites.ContainsKey((long)apiSite.Id))
                    {
                        dSites[(long)apiSite.Id].Name = apisiteDetails.Name;
                        //dSites[(long)apiSite.Id].UTMZone = string.Empty;
                        dSites[(long)apiSite.Id].Latitude = fLatitude;
                        dSites[(long)apiSite.Id].Longitude = fLongitude;
                    }
                    else
                    {
                        dSites[(long)apiSite.Id] = new CHaMPData.Site(apiSite.Id, apisiteDetails.Name, nWatershedID, string.Empty, string.Empty, string.Empty, false, false, false, false, false, false, fLatitude, fLongitude, null, naru.db.DBState.New);
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

        private void Visits(ref SQLiteTransaction dbTrans, ref Dictionary<string, long> dSitesURLs)
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
                Console.Write("visit");
            }
        }
    }
}

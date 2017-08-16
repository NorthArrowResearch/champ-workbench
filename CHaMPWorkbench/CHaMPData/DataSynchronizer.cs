using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoOptix.API;
using System.Data.SQLite;
using System.Net.Http;
using IdentityModel.Client;
using Keystone.API;

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

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;

        public DataSynchronizer()
        {
            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                Protocols = LoadLookupList(dbCon, "SELECT ItemID, Title FROM LookupListItems WHERE ListID = 8");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lPrograms"></param>
        /// <param name="lWatersheds">Empty list implies that all watersheds will be processed</param>
        public void Run(IEnumerable<CHaMPData.Program> lPrograms, Dictionary<long, CHaMPData.Watershed> WatershedsToProcess, string UserName, string Password)
        {
            WatershedURLs = new Dictionary<string, long>();
            SiteURLs = new Dictionary<string, long>();
            TotalNumberVisits = 0;
            TokenResponse AuthToken = null;

            using (SQLiteConnection dbCon = new SQLiteConnection(naru.db.sqlite.DBCon.ConnectionString))
            {
                dbCon.Open();

                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    ReportProgress(0, string.Format("Retrieving the list of watersheds, sites and visits for {0} program{1}...", lPrograms.Count<Program>(), lPrograms.Count<Program>() > 1 ? "s" : ""));

                    foreach (Program aProgram in lPrograms)
                    {
                        try
                        {
                            CurrentProcess = "Authenticating user with Keystone API";

                            // Determine if the program is pointing at QA or Production and use the corresponding keystone
                            string keystoneURL = "https://keystone.sitkatech.com/core/connect/token";
                            if (aProgram.API.Contains("https://qa."))
                                keystoneURL = keystoneURL.Replace("https://", "https://qa.");

                            //System.Windows.Forms.MessageBox.Show(string.Format("{0}\n{1}\n{2}\n{3}", aProgram.API, keystoneURL, Properties.Settings.Default.GeoOptixClientID, Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper()));
                            //System.Windows.Forms.MessageBox.Show(string.Format("{0}\n{1}", UserName, Password));

                            ApiHelper keystoneApiHelper = new ApiHelper(aProgram.API, keystoneURL, Properties.Settings.Default.GeoOptixClientID,
                                Properties.Settings.Default.GeoOptixClientSecret.ToString().ToUpper(), UserName, Password);

                            if (keystoneApiHelper.AuthToken.IsError)
                            {
                                Exception ex = new Exception(keystoneApiHelper.AuthToken.ErrorDescription, keystoneApiHelper.AuthToken.Exception);
                                ex.Data["JSON"] = keystoneApiHelper.AuthToken.Json.ToString();
                                throw ex;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to authenticate user with Keystone API", ex);
                        }


                        ReportProgress(0, "Synchonizing watersheds");
                        SyncWatersheds(ref dbTrans, aProgram, ref WatershedsToProcess, ref AuthToken);

                        ReportProgress(0, "Synchonizing sites");
                        SyncSites(ref dbTrans, aProgram, ref AuthToken);
                        TotalNumberVisits += GetListOfVisitURLs(aProgram, ref AuthToken);
                    }

                    // Load all existing visits
                    Dictionary<long, Visit> dvisits = Visit.Load(naru.db.sqlite.DBCon.ConnectionString);

                    // Now synchronize all visits
                    int nVisitCounter = 0;
                    foreach (long nProgramID in VisitURLs.Keys)
                    {
                        foreach (string sVisitURL in VisitURLs[nProgramID])
                        {
                            SyncVisits(ref dbTrans, ref dvisits, sVisitURL, nProgramID, ref AuthToken);
                            nVisitCounter += 1;
                            CurrentProcess = sVisitURL;
                            ReportProgress(ProgressPercent(nVisitCounter), sVisitURL);
                        }
                    }

                    // Save the updated list of visits (pass all visits, not just those that have changed because channel units might need updating)
                    ReportProgress(100, "Saving visits to database");
                    CHaMPData.Visit.Save(ref dbTrans, dvisits.Values.ToList<CHaMPData.Visit>());

                    dbTrans.Commit();
                    ReportProgress(100, "Process Complete");
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw;
                }
            }
        }

        private void ReportProgress(int value, string sMessage)
        {
            CurrentProcess = sMessage;
            OnProgressUpdate(value);
        }

        private int ProgressPercent(int nVisitCounter)
        {
            if (TotalNumberVisits == 0)
                return 0;
            else
            {
                if (nVisitCounter == 0)
                    return 0;
                else
                    if (nVisitCounter == TotalNumberVisits)
                    return 100;
                else
                    return (int)(100 * nVisitCounter / TotalNumberVisits);
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

        private TokenResponse SyncWatersheds(ref SQLiteTransaction dbTrans, Program theProgrm, ref Dictionary<long, Watershed> WatershedsToProcess, ref TokenResponse AuthToken)
        {
            Dictionary<long, Watershed> dWatersheds = Watershed.Load(naru.db.sqlite.DBCon.ConnectionString);

            ApiHelper api = new ApiHelper(string.Format("{0}/watersheds", theProgrm.API), AuthToken);
            ApiResponse<GeoOptix.API.Model.WatershedSummaryModel[]> response = api.Get<GeoOptix.API.Model.WatershedSummaryModel[]>();

            if (api.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Exception ex = new Exception(string.Format("API Error with Status Code: {0}", api.StatusCode.ToString()));
                ex.Data["Status Code"] = api.StatusCode.ToString();
                ex.Data["API Address"] = theProgrm.API;
                throw ex;
            }

            foreach (GeoOptix.API.Model.WatershedSummaryModel apiWatershed in response.Payload)
            {
                if (WatershedsToProcess.Count == 0 || WatershedsToProcess.ContainsKey(apiWatershed.Id))
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
            }

            Watershed.Save(ref dbTrans, dWatersheds.Values.ToList<Watershed>());


            return api.AuthToken;
        }

        private void SyncSites(ref SQLiteTransaction dbTrans, Program theProgram, ref TokenResponse AuthToken)
        {
            Dictionary<long, Site> dSites = Site.Load(naru.db.sqlite.DBCon.ConnectionString);

            ApiHelper api = new ApiHelper(string.Format("{0}/sites", theProgram.API), AuthToken);
            ApiResponse<GeoOptix.API.Model.SiteSummaryModel[]> response = api.Get<GeoOptix.API.Model.SiteSummaryModel[]>();
            foreach (GeoOptix.API.Model.SiteSummaryModel apiSite in response.Payload)
            {
                if (WatershedURLs.ContainsKey(apiSite.WatershedUrl))
                {
                    if (dSites.ContainsKey((long)apiSite.Id))
                    {
                        dSites[(long)apiSite.Id].Name = apiSite.Name;
                        dSites[(long)apiSite.Id].StreamName = GetStreamName(apiSite.Url);
                    }
                    else
                    {
                        Nullable<double> fLongitude = new Nullable<double>();
                        Nullable<double> fLatitude = new Nullable<double>();

                        dSites[(long)apiSite.Id] = new CHaMPData.Site(apiSite.Id, apiSite.Name, WatershedURLs[apiSite.WatershedUrl], apiSite.Locale, string.Empty, string.Empty, false, false, false, false, false, false, fLatitude, fLongitude, null, naru.db.DBState.New);
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

        private int GetListOfVisitURLs(Program theProgram, ref TokenResponse AuthToken)
        {
            if (VisitURLs == null)
                VisitURLs = new Dictionary<long, List<string>>();

            ApiHelper api = new ApiHelper(string.Format("{0}/visits", theProgram.API), AuthToken);
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

        private string GetStreamName(string sSiteDetailsURL)
        {
            if (string.IsNullOrEmpty(sSiteDetailsURL))
                return string.Empty;

            string sStreamName = string.Empty;
            ApiHelper api2 = new ApiHelper(sSiteDetailsURL, null);
            ApiResponse<GeoOptix.API.Model.SiteModel> apiSiteResponse = api2.Get<GeoOptix.API.Model.SiteModel>();
            GeoOptix.API.Model.SiteModel apiSiteDetails = apiSiteResponse.Payload;
            if (apiSiteDetails != null)
                sStreamName = apiSiteDetails.Locale;

            return sStreamName;
        }

        private void SyncVisits(ref SQLiteTransaction dbTrans, ref Dictionary<long, Visit> dvisits, string sVisitURL, long nProgramID, ref TokenResponse AuthToken)
        {
            ApiHelper api2 = new ApiHelper(sVisitURL, AuthToken);
            ApiResponse<GeoOptix.API.Model.VisitModel> apiVisitResponse = api2.Get<GeoOptix.API.Model.VisitModel>();
            GeoOptix.API.Model.VisitModel apiVisitDetails = apiVisitResponse.Payload;

            if (apiVisitDetails == null || !apiVisitDetails.SampleYear.HasValue)
                return;


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
            theVisit.ProtocolID = GetLookupListItemID(ref dbTrans, ref Protocols, 8, apiVisitDetails.Protocol);
            theVisit.CategoryName = apiVisitDetails.Category;

            theVisit.Panel = apiVisitDetails.Panel;
            theVisit.VisitStatus = apiVisitDetails.Status;
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Visit Information Attributes (this requires authentication)

            ApiResponse<GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>>> res = api2.GetMeasurement<Dictionary<string, string>>("Visit Information");
            GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>> meas = res.Payload;
            if (meas != null)
            {
                IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> vals = meas.MeasValues;
                GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mv = vals.First<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>>();
                Dictionary<string, string> dMeasurements = mv.Measurement;

                theVisit.Discharge = GetVisitInfoValue(ref dMeasurements, "TotalDischarge");
                theVisit.D84 = GetVisitInfoValue(ref dMeasurements, "D84");
                theVisit.HasStreamTempLogger = GetVisitInfoValueBool(ref dMeasurements, "HasStreamTempLogger");
                theVisit.HasFishData = GetVisitInfoValueBool(ref dMeasurements, "HasFishData");
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Channel Units
            ApiResponse<GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>>> resCU = api2.GetMeasurement<Dictionary<string, string>>("Channel Unit");
            GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>> measCU = resCU.Payload;
            if (measCU != null)
            {
                IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> valsCU = measCU.MeasValues;
                //GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mvCU = valsCU.First<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>>();
                //Dictionary<string, string> dChannelUnits = mvCU.Measurement;
                ChannelUnits(ref theVisit, valsCU);

                // Now do the substrate cover and update the grain sizes
                ApiResponse<GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>>> resSubstrate = api2.GetMeasurement<Dictionary<string, string>>("Substrate Cover");
                GeoOptix.API.Model.MeasurementModel<Dictionary<string, string>> measSubstrate = resSubstrate.Payload;
                if (measSubstrate != null)
                {
                    IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> valsSubstrate = measSubstrate.MeasValues;
                    SubstrateCover(ref theVisit, valsSubstrate);
                }
            }
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

                if (!theVisit.ChannelUnits.ContainsKey(nChannelUnitNumber))
                    theVisit.ChannelUnits[nChannelUnitNumber] = new ChannelUnit(nChannelUnitID, theVisit.ID, nChannelUnitNumber, nSegmentNumber, sTier1, sTier2, naru.db.DBState.New);
                else
                {
                    theVisit.ChannelUnits[nChannelUnitNumber].Tier1 = mvCU.Measurement["Tier1"];
                    theVisit.ChannelUnits[nChannelUnitNumber].Tier2 = mvCU.Measurement["Tier2"];
                    theVisit.ChannelUnits[nChannelUnitNumber].SegmentNumber = nSegmentNumber;
                }
            }
        }

        private void SubstrateCover(ref CHaMPData.Visit theVisit, IEnumerable<GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>>> lSbustrateCover)
        {
            foreach (GeoOptix.API.Model.MeasValueModel<Dictionary<string, string>> mvSub in lSbustrateCover)
            {
                //long nChannelUnitNumber = long.Parse(mvCU.Measurement["ChannelUnitNumber"]);
                long nChannelUnitID = long.Parse(mvSub.Measurement["ChannelUnitID"]);
                Dictionary<string, string> dValues = mvSub.Measurement;
                if (theVisit.ChannelUnits.ContainsKey(nChannelUnitID))
                {

                    theVisit.ChannelUnits[nChannelUnitID].Bedrock = GetSubstrateValue(ref dValues, "Bedrock");
                    theVisit.ChannelUnits[nChannelUnitID].BouldersGT256 = GetSubstrateValue(ref dValues, "Boulders");
                    theVisit.ChannelUnits[nChannelUnitID].Cobbles65255 = GetSubstrateValue(ref dValues, "Cobbles");
                    theVisit.ChannelUnits[nChannelUnitID].CoarseGravel1764 = GetSubstrateValue(ref dValues, "CourseGravel");
                    theVisit.ChannelUnits[nChannelUnitID].FineGravel316 = GetSubstrateValue(ref dValues, "FineGravel");
                    theVisit.ChannelUnits[nChannelUnitID].Sand0062 = GetSubstrateValue(ref dValues, "Sand");
                    theVisit.ChannelUnits[nChannelUnitID].FinesLT006 = GetSubstrateValue(ref dValues, "Fines");
                    theVisit.ChannelUnits[nChannelUnitID].SumSubstrateCover = GetSubstrateValue(ref dValues, "SumSubstrateCover");
                }
                else
                    Console.Write("stop");
            }
        }

        private long? GetSubstrateValue(ref Dictionary<string, string> dValues, string sKey)
        {
            long? result = null;
            if (dValues.ContainsKey(sKey))
            {
                if (!string.IsNullOrEmpty(dValues[sKey]) && string.Compare(dValues[sKey], "null", true) != 0)
                {
                    try
                    {
                        result = long.Parse(dValues[sKey]);
                    }
                    catch (Exception ex)
                    {
                        Console.Write("stop");
                    }
                }
            }
            return result;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Xml;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace CHaMPWorkbench.Experimental.James
{
    class USGS_StreamData
    {
        private string DBConnectionString;
        private long m_iGageID;
        private bool m_bSiteHasGageID;
        private bool m_bDatabaseHasDischargeData;
        private List<StreamFlowSample> m_lStreamData = new List<StreamFlowSample>();

        public USGS_StreamData(string sDBCon, long nSiteID)
        {
            DBConnectionString = sDBCon;

            //check and get gage id if site has one
            m_iGageID = GetGageID(nSiteID);
            if (m_iGageID > 0)
            {
                m_bSiteHasGageID = true;
                //Check if the site has data
                m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_iGageID);
            }
            else if (m_iGageID == 0) // site does not have a gage associated with it
            {
                m_bSiteHasGageID = false;
                m_bDatabaseHasDischargeData = false;
            }
        }

        public USGS_StreamData(string sDBCon, int iGageID, bool bGetData = true)
        {
            DBConnectionString = sDBCon;
            m_iGageID = iGageID;
            //Check if the site has data
            m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_iGageID);
            if (bGetData == true)
            {
                //GetUSGS_DischargeData()
            }
        }

        public List<StreamFlowSample> StreamData
        {
            get { return m_lStreamData; }
        }

        public long GageNumber
        {
            get { return m_iGageID; }
        }

        public bool DatabaseHasDischargeData
        {
            get { return m_bDatabaseHasDischargeData; }
        }

        public bool SiteHasGageID
        {
            get { return m_bSiteHasGageID; }
        }

        public void CheckCHaMP_SiteForAssociatedGage(long nSiteID)
        {
            //check and get gage id if site has one
            m_iGageID = GetGageID(nSiteID);
            if (m_iGageID > 0)
            {
                m_bSiteHasGageID = true;
                //Check if the site has data
                m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_iGageID);

            }
            else if (m_iGageID == 0) // site does not have a gage associated with it
            {
                m_bSiteHasGageID = false;
                m_bDatabaseHasDischargeData = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iGageID"></param>
        /// <returns></returns>
        /// <remarks>Sample web service string
        /// http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&site={0}&parameterCd=00060&siteType=ST&startDT=2011-01-01&endDT=2016-01-01</remarks>
        public List<StreamFlowSample> GetUSGS_DischargeData(long iGageID)
        {
            m_iGageID = iGageID;
            m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(iGageID);
            if (m_bDatabaseHasDischargeData)
            {
                //site has data so get it from the db
                m_lStreamData = RetreiveDischargeDataFromDB(iGageID);
            }
            else
            {
                DateTime dtStart = new DateTime(2011, 1, 1);
                try
                {
                    dtStart = CHaMPWorkbench.Properties.Settings.Default.HydroGraphStart;
                }
                catch (Exception ex)
                {
                    dtStart = new DateTime(2011, 1, 1);
                    Console.WriteLine(ex);
                }

                DateTime dtEnd = new DateTime(2011, 1, 1);
                try
                {
                    dtEnd = CHaMPWorkbench.Properties.Settings.Default.HydroGraphEnd;
                }
                catch (Exception ex)
                {
                    dtEnd = new DateTime(DateTime.Now.Year, 1, 1);
                    Console.WriteLine(ex);
                }

                //site does not have data go to use usgs api to download and load data to db
                string URLString = String.Format("http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&site={0:00000000}&parameterCd=00060&siteType=ST&startDT={1:yyyy-MM-dd}&endDT={2:yyyy-MM-dd}", m_iGageID, dtStart, dtEnd);
                Console.WriteLine("Hydrograph Web Service call: " + URLString);
                string HTTPResponse = GetUrl(URLString, null, true);

                m_lStreamData = LoadUSGS_XML_toDB(HTTPResponse, iGageID);
                if (m_lStreamData.Count > 1)
                {
                    m_bDatabaseHasDischargeData = true;
                }
            }
            return m_lStreamData;
        }


        private bool CheckIfDischargesTableContainsData(long iGageID)
        {
            bool bContainsData = false;
            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT TheDate FROM USGS_Discharges WHERE GageID = @GageID", dbCon);
                comFS.Parameters.AddWithValue("@GageID", iGageID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();

                while (dbRead.Read())
                {
                    if (dbRead[0] != System.DBNull.Value)
                    {
                        bContainsData = true;
                    }
                }
                dbRead.Close();
            }
            return bContainsData;
        }

        /// <summary>
        /// Check if discharge data exist in DB for this gage
        /// </summary>
        /// <param name="sCHaMPSiteName"></param>
        /// <returns></returns>
        private long GetGageID(long nSiteID)
        {
            long iGageID = new long();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();

                SQLiteCommand comFS = new SQLiteCommand("SELECT GageID FROM CHaMP_Sites WHERE SiteID = @SiteID", dbCon);
                comFS.Parameters.AddWithValue("@SiteID", nSiteID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();

                while (dbRead.Read())
                {
                    if (dbRead[0] != System.DBNull.Value)
                    {
                        iGageID = Convert.ToInt64(dbRead[0]);
                    }
                }
            }
            return iGageID;
        }

        public bool VerifyGageID(string iGageID)
        {
            int iOutputParse;
            if (!Int32.TryParse(iGageID, out iOutputParse))
            {
                return false;
            }

            bool bContainsData = false;

            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT Description FROM USGS_Gages WHERE GageID = @GageID", dbCon);
                comFS.Parameters.AddWithValue("@GageID", iGageID);

                SQLiteDataReader dbRead = comFS.ExecuteReader();
                while (dbRead.Read())
                {
                    if (dbRead[0] != System.DBNull.Value)
                    {
                        bContainsData = true;
                    }
                }
                dbRead.Close();
            }
            return bContainsData;
        }

        public bool VerifyGageID(int iGageID)
        {
            bool bContainsData = false;
            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT Description FROM USGS_Gages WHERE GageID = @GageID", dbCon);
                comFS.Parameters.AddWithValue("@GageID", iGageID);
                SQLiteDataReader dbRead = comFS.ExecuteReader();

                while (dbRead.Read())
                {
                    if (dbRead[0] != System.DBNull.Value)
                    {
                        bContainsData = true;
                    }
                }
                dbRead.Close();
            }

            return bContainsData;
        }

        private List<StreamFlowSample> RetreiveDischargeDataFromDB(long iGageID)
        {
            List<StreamFlowSample> lStreamData = new List<StreamFlowSample>();
            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();
                SQLiteCommand comFS = new SQLiteCommand("SELECT TheDate, Discharge FROM USGS_Discharges WHERE GageID = @GageID", dbCon);
                comFS.Parameters.AddWithValue("@GageID", iGageID);

                SQLiteDataReader dbRead = comFS.ExecuteReader();
                while (dbRead.Read())
                {
                    //add each record to the list of stream sample data

                    StreamFlowSample sample = new StreamFlowSample(Convert.ToDouble(dbRead[1]), dbRead.GetDateTime(0));
                    lStreamData.Add(sample);
                }
                dbRead.Close();
            }

            return lStreamData;
        }

        /// <summary>
        /// Simple routine to retrieve HTTP Content as a string with
        /// optional POST data and GZip encoding.
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="PostData"></param>
        /// <param name="GZip"></param>
        /// <returns></returns>
        private string GetUrl(string Url, string PostData, bool GZip)
        {
            HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);

            if (GZip)
                Http.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            if (!string.IsNullOrEmpty(PostData))
            {
                Http.Method = "POST";
                byte[] lbPostBuffer = Encoding.Default.GetBytes(PostData);

                Http.ContentLength = lbPostBuffer.Length;

                Stream PostStream = Http.GetRequestStream();
                PostStream.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                PostStream.Close();
            }

            HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

            Stream responseStream = responseStream = WebResponse.GetResponseStream();
            if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

            StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

            string Html = Reader.ReadToEnd();

            WebResponse.Close();
            responseStream.Close();

            return Html;
        }

        private List<StreamFlowSample> LoadUSGS_XML_toDB(string HTTPResponse, long iGageID)
        {
            List<StreamFlowSample> lStreamData = new List<StreamFlowSample>();

            using (SQLiteConnection dbCon = new SQLiteConnection(DBConnectionString))
            {
                dbCon.Open();
                SQLiteTransaction dbTrans = dbCon.BeginTransaction();

                try
                {
                    var xmlDoc = System.Xml.Linq.XDocument.Parse(HTTPResponse);
                    System.Xml.Linq.XNamespace usgs_namespace = "http://www.cuahsi.org/waterML/1.1/";

                    DateTime queryDate = Convert.ToDateTime("2011-01-01T12:00:00.000");

                    var queryStreamData = from elem in xmlDoc.Descendants(usgs_namespace + "value")
                                          select new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.Attribute("dateTime").Value));

                    lStreamData = queryStreamData.ToList();
                    if (lStreamData.Count > 0)
                    {

                        //http://stackoverflow.com/questions/14418142/linq-select-group-by

                        //get average flow for each day
                        var aggregatedStreamFlow = from d in lStreamData
                                                   group d by d.Date.Date into agg
                                                   select new StreamFlowSample(agg.Average(x => x.Flow), agg.Key);



                        foreach (StreamFlowSample sample in aggregatedStreamFlow)
                        {

                            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO USGS_Discharges (GageID, TheDate, Discharge)" +
                                                                    " VALUES (@GageID, @TheDate, @Discharge)", dbTrans.Connection, dbTrans);

                            dbCom.Parameters.AddWithValue("@GageID", iGageID);
                            dbCom.Parameters.AddWithValue("@TheDate", sample.Date);
                            dbCom.Parameters.AddWithValue("@Discharge", sample.Flow);

                            dbCom.ExecuteNonQuery();

                        }
                        dbTrans.Commit();
                    }
                    else
                    {

                    }
                }
                catch
                {
                    dbTrans.Rollback();
                    throw;
                }
            }

            return lStreamData;
        }
    }

    class StreamFlowSample
    {
        public double Flow { get; internal set; }
        public DateTime Date { get; internal set; }

        public StreamFlowSample(double dFlow, DateTime dtDate)
        {
            Flow = dFlow;
            Date = dtDate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace CHaMPWorkbench.Experimental.James
{
    class USGS_StreamData
    {
        private OleDbConnection m_dbCon;
        private int m_iGageID;
        private bool m_bSiteHasGageID;
        private bool m_bDatabaseHasDischargeData;
        private List<StreamFlowSample> m_lStreamData = new List<StreamFlowSample>();

        public USGS_StreamData(OleDbConnection dbCon, string sSiteName)
        {
            m_dbCon = dbCon;

            //check and get gage id if site has one
            m_iGageID = GetGageID(m_dbCon, sSiteName);
            if (m_iGageID > 0)
            {
                m_bSiteHasGageID = true;
                //Check if the site has data
                m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_dbCon, m_iGageID);

            }
            else if (m_iGageID == 0) // site does not have a gage associated with it
            {
                m_bSiteHasGageID = false;
                m_bDatabaseHasDischargeData = false;
            }

        }

        public USGS_StreamData(OleDbConnection dbCon, int iGageID, bool bGetData = true)
        {
            m_dbCon = dbCon;
            m_iGageID = iGageID;
            //Check if the site has data
            m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(dbCon, m_iGageID);
            if (bGetData == true)
            {
                //GetUSGS_DischargeData()
            }
        }

        public List<StreamFlowSample> StreamData
        {
            get { return m_lStreamData; }
        }

        public int GageNumber
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

        public void CheckCHaMP_SiteForAssociatedGage(string sCHaMPSite)
        {
            //check and get gage id if site has one
            m_iGageID = GetGageID(m_dbCon, sCHaMPSite);
            if (m_iGageID > 0)
            {
                m_bSiteHasGageID = true;
                //Check if the site has data
                m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_dbCon, m_iGageID);

            }
            else if (m_iGageID == 0) // site does not have a gage associated with it
            {
                m_bSiteHasGageID = false;
                m_bDatabaseHasDischargeData = false;
            }
        }

        public List<StreamFlowSample> GetUSGS_DischargeData(int iGageID)
        {
            m_iGageID = iGageID;
            m_bDatabaseHasDischargeData = CheckIfDischargesTableContainsData(m_dbCon, iGageID);
            if (m_bDatabaseHasDischargeData == true)
            {
                //site has data so get it from the db
                m_lStreamData = RetreiveDischargeDataFromDB(m_dbCon, iGageID);
            }
            else if (m_bDatabaseHasDischargeData == false)
            {
                //site does not have data go to use usgs api to download and load data to db
                string URLString = String.Format("http://nwis.waterservices.usgs.gov/nwis/iv/?format=waterml,1.1&site={0}&parameterCd=00060&siteType=ST&startDT=2011-01-01&endDT=2016-01-01", m_iGageID.ToString());
                string HTTPResponse = GetUrl(URLString, null, true);

                m_lStreamData = LoadUSGS_XML_toDB(HTTPResponse, iGageID);
                if (m_lStreamData.Count > 1)
                {
                    m_bDatabaseHasDischargeData = true;
                }
            }
            return m_lStreamData;
        }


        private bool CheckIfDischargesTableContainsData(OleDbConnection dbCon, int iGageID)
        {
            bool bContainsData = false;

            //Check if data exists in db
            string sGroupFields = " TheDate";
            string sSQL = "SELECT " + sGroupFields +
                          " FROM USGS_Discharges  " +
                          " WHERE GageID = " + iGageID;

            OleDbCommand comFS = new OleDbCommand(sSQL, dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();

            if (dbRead.HasRows)
            {
                //Get GageID
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

        private int GetGageID(OleDbConnection dbCon, string sCHaMPSiteName)
        {
            int iGageID = new int();

            //Check if data exists in db
            sCHaMPSiteName = String.Format("\"{0}\"", sCHaMPSiteName);
            string sGroupFields = " GageID";
            string sSQL = "SELECT " + sGroupFields +
                          " FROM CHAMP_Sites " +
                          " WHERE SiteName = " + sCHaMPSiteName;

            OleDbCommand comFS = new OleDbCommand(sSQL, dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();

            if (dbRead.HasRows)
            {
                //Get GageID
                while (dbRead.Read())
                {
                    if (dbRead[0] != System.DBNull.Value)
                    {
                        iGageID = Convert.ToInt32(dbRead[0]);
                    }
                }
                dbRead.Close();
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

            //Check if data exists in db
            string sGroupFields = " Description";
            string sSQL = "SELECT " + sGroupFields +
                          " FROM USGS_Gages  " +
                          " WHERE GageID = " + iGageID;

            OleDbCommand comFS = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();

            if (dbRead.HasRows)
            {
                //Get GageID
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

            //Check if data exists in db
            string sGroupFields = " Description";
            string sSQL = "SELECT " + sGroupFields +
                          " FROM USGS_Gages  " +
                          " WHERE GageID = " + iGageID;

            OleDbCommand comFS = new OleDbCommand(sSQL, m_dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();

            if (dbRead.HasRows)
            {
                //Get GageID
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

        private List<StreamFlowSample> RetreiveDischargeDataFromDB(OleDbConnection dbCon, int iGageID)
        {
            string sGroupFields = "TheDate, Discharge";
            string sSQL = "SELECT " + sGroupFields +
                    " FROM USGS_Discharges " +
                    " WHERE GageID = " + iGageID;

            OleDbCommand comFS = new OleDbCommand(sSQL, dbCon);
            OleDbDataReader dbRead = comFS.ExecuteReader();


            List<StreamFlowSample> lStreamData = new List<StreamFlowSample>();
            if (dbRead.HasRows)
            {
                while (dbRead.Read())
                {
                    //add each record to the list of stream sample data

                    StreamFlowSample sample = new StreamFlowSample(Convert.ToDouble(dbRead[1]), dbRead.GetDateTime(0));
                    lStreamData.Add(sample);
                }
            }

            dbRead.Close();

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

        private List<StreamFlowSample> LoadUSGS_XML_toDB(string HTTPResponse, int iGageID)
        {
            List<StreamFlowSample> lStreamData = new List<StreamFlowSample>();
            try
            {
                var xmlDoc = System.Xml.Linq.XDocument.Parse(HTTPResponse);
                System.Xml.Linq.XNamespace usgs_namespace = "http://www.cuahsi.org/waterML/1.1/";

                DateTime queryDate = Convert.ToDateTime("2011-01-01T12:00:00.000");

                var queryStreamData = from elem in xmlDoc.Descendants(usgs_namespace + "value")
                                      select new StreamFlowSample(double.Parse(elem.Value), DateTime.Parse(elem.FirstAttribute.Value.ToString()));//, elem.Attribute("dateTime"));

                lStreamData = queryStreamData.ToList();
                if (lStreamData.Count > 0)
                {

                    //http://stackoverflow.com/questions/14418142/linq-select-group-by

                    //get average flow for each day
                    var aggregatedStreamFlow = from d in lStreamData
                                               group d by d.Date.Date into agg
                                               select new StreamFlowSample(agg.Average(x => x.Flow), agg.Key);



                    OleDbTransaction dbTrans = m_dbCon.BeginTransaction();
                    foreach (StreamFlowSample sample in aggregatedStreamFlow)
                    {

                        OleDbCommand dbCom = new OleDbCommand("INSERT INTO USGS_Discharges (GageID, TheDate, Discharge)" +
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

            }

            return lStreamData;

        }
    }

    class StreamFlowSample
    {
        private double _flow;
        private DateTime _date;

        public StreamFlowSample(double dFlow, DateTime dtDate)
        {
            _flow = dFlow;
            _date = dtDate;
        }

        public double Flow
        {
            get { return _flow; }
            set { _flow = value; }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

    }

}

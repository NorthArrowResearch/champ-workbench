using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml;

namespace CHaMPWorkbench.Habitat
{
    class HabitatResultsScavenger
    {
        /// <summary>
        /// These constants correspond to the LookupListItem IDs in the 
        /// Workbench database for "Scavenge Types" and "Metric Types"
        /// </summary>
        private const int m_nRBTScavengeTypeID = 1;

        private const int m_nVisitMetricTypeID = 3;
        private const int m_nTier1MetricTypeID = 4;
        private const int m_nTier2MetricTypeID = 5;
        private const int m_nChannelUnitTypeID = 6;

        /// <summary>
        /// 
        /// </summary>
        private string m_sDBCon;
        public HabitatResultsScavenger(string sDBCon)
        {
            m_sDBCon = sDBCon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sResultFilePath"></param>
        /// <returns></returns>
        public int ScavengeHSOutputFile(string sResultFilePath)
        {
            if (string.IsNullOrEmpty(sResultFilePath) || !System.IO.File.Exists(sResultFilePath))
            {
                Exception ex = new Exception("the habitat result file does not exist.");
                ex.Data["result file"] = sResultFilePath;
                throw ex;
            }

            XmlDocument xmlResults = new XmlDocument();

            try
            {
                xmlResults.Load(sResultFilePath);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error loading Habitat result XML file into XMLDocument object.", ex);
                ex2.Data["Result File"] = sResultFilePath;
                throw ex2;
            }

            int nResultID = 0;
            using (SQLiteConnection DBCon = new SQLiteConnection(m_sDBCon))
            {
                // Open the database and use a transaction for this entire Habitat result file.
                // If anything goes wrong with any metric then the whole file is abandoned
                // and no changes are stored to the database.
                DBCon.Open();
                SQLiteTransaction dbTrans = DBCon.BeginTransaction();

                try
                {
                    // Insert the result file record and retrieve the unique ID that represents that result.
                    nResultID = InsertResultRecord(ref dbTrans, sResultFilePath, ref xmlResults);
                    if (nResultID > 0)
                    {
                        ScavengeVisitMetrics(ref dbTrans, ref xmlResults, nResultID);
                        dbTrans.Commit();
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("Skipping result. Failed to insert result ID: " + sResultFilePath);
                    }

                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw ex;
                }
            }

            return nResultID;
        }


        /// <summary>
        /// Performs the actual retrieval of metric values from the result XML file and inserts them into the database
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="xmlResults">RBT result XML document</param>
        /// <param name="nResultID">The parent ResultID that represents the XML result file record in Metric_Results</param>
        private int ScavengeVisitMetrics(ref SQLiteTransaction dbTrans, ref XmlDocument xmlResults,
            int nResultID)
        {
            int nSpeciesLifestage = -1;
            int nModelType = -1;
            List<ScavengeMetric> lVisitMetrics = GetMetrics(m_nVisitMetricTypeID);
            if (lVisitMetrics.Count < 1)
                return 0;

            XmlNode nodSimulation = xmlResults.SelectSingleNode("//log//simulations//simulation");

            // Get the type from the lookup table
            string sType = string.Empty;
            if (nodSimulation is XmlNode && !string.IsNullOrEmpty(nodSimulation.Attributes["type"].Value))
            {
                sType = nodSimulation.Attributes["type"].Value;
                // Gotta go lookup the CHamP Model Type
                SQLiteCommand dbTypeCom = new SQLiteCommand("SELECT ItemID FROM LookupListItems WHERE ListID = @LISTID AND Title = @TITLE", dbTrans.Connection, dbTrans);
                dbTypeCom.Parameters.AddWithValue("@LISTID", CHaMPWorkbench.Properties.Settings.Default.LookupList_CHaMPModel); // 'CHaMP Models' is lookup list 4
                dbTypeCom.Parameters.AddWithValue("@TITLE", sType);
                SQLiteDataReader dbRead = dbTypeCom.ExecuteReader();
                int counter = 0;
                while (dbRead.Read())
                {
                    nModelType = dbRead.GetInt32(dbRead.GetOrdinal("ItemID"));
                    counter++;
                }
                if (counter != 1)
                    throw new Exception(String.Format("Could not find habitat model type: {0}", sType));
            }
            else
            {
                throw new Exception("Simulation in XML file is missing \"type\" attribute");
            }

            // Try to determine the species and lifestage from the XML file.
            XmlNode nodSpecies = xmlResults.SelectSingleNode("//log//simulations//simulation//sim_meta//meta[@key='species']");
            XmlNode nodLifestage = xmlResults.SelectSingleNode("//log//simulations//simulation//sim_meta//meta[@key='lifestage']");
            if (!(nodSpecies is XmlNode) || string.IsNullOrEmpty(nodSpecies.InnerText))
            {
                throw new Exception("Could not determine species from XML file.");
            }
            else if (!(nodLifestage is XmlNode) || string.IsNullOrEmpty(nodLifestage.InnerText))
            {
                throw new Exception("Could not determine lifestage from XML file.");
            }
            else
            {
                string sSpeciesLifestage = string.Format("{0} {1}", nodSpecies.InnerText, nodLifestage.InnerText);

                // Gotta go lookup the species / lifestage
                SQLiteCommand dbSpeciesLifestage = new SQLiteCommand("SELECT ItemID FROM LookupListItems WHERE ListID = @LISTID AND Title = @TITLE", dbTrans.Connection, dbTrans);
                dbSpeciesLifestage.Parameters.AddWithValue("@LISTID", CHaMPWorkbench.Properties.Settings.Default.LookupList_SpeciesLifestage);  // 'Species and Lifestage' is lookup list 10
                dbSpeciesLifestage.Parameters.AddWithValue("@TITLE", sSpeciesLifestage);
                SQLiteDataReader dbRead = dbSpeciesLifestage.ExecuteReader();
                int counter = 0;
                while (dbRead.Read())
                {
                    nSpeciesLifestage = dbRead.GetInt32(dbRead.GetOrdinal("ItemID"));
                    counter++;
                }
                if (counter != 1)
                    throw new Exception(String.Format("Could not identify species lifestage based on the XML species: \"{0}\" and lifestage: \"{1}\"", nodSpecies.InnerText, nodLifestage.InnerText));

            }

            // Determine the flow type from the XML
            XmlNode nodFlow = xmlResults.SelectSingleNode("//log//simulations//simulation//sim_meta//meta[@key='flow']");
            string sFlow = CHaMPWorkbench.Properties.Settings.Default.DefaultFlow;
            if (nodFlow is XmlNode && !string.IsNullOrEmpty(nodFlow.InnerText))
            {
                sFlow = nodFlow.InnerText;
            }

            SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Habitat (ResultID, ModelID, SpeciesLifeStageID, MetricID, FlowType, MetricValue) VALUES (@ResultID, @ModelID, @SpeciesLifeStageID, @MetricID, @FlowType, @MetricValue)",
                dbTrans.Connection,
                dbTrans);
            SQLiteParameter pResultID = dbCom.Parameters.Add("@ResultID", System.Data.DbType.Int64);
            SQLiteParameter pModelID = dbCom.Parameters.Add("@ModelID", System.Data.DbType.Int64);
            SQLiteParameter pSpeciesLifeStageID = dbCom.Parameters.Add("@SpeciesLifeStageID", System.Data.DbType.Int64);
            SQLiteParameter pMetricID = dbCom.Parameters.Add("@MetricID", System.Data.DbType.Int64);
            SQLiteParameter pFlowType = dbCom.Parameters.Add("@FlowType", System.Data.DbType.String);
            SQLiteParameter pMetricValue = dbCom.Parameters.Add("@MetricValue", System.Data.DbType.Double);

            pResultID.Value = nResultID;
            pModelID.Value = nModelType;
            pSpeciesLifeStageID.Value = nSpeciesLifestage;
            pFlowType.Value = sFlow;

            int nMetricsScavenged = 0;
            foreach (ScavengeMetric aMetric in lVisitMetrics)
            {
                XmlNode metricNode = xmlResults.SelectSingleNode(aMetric.XPath);
                if (metricNode is XmlNode)
                {
                    pMetricID.Value = aMetric.MetricID;
                    double fMetricValue;
                    if (!string.IsNullOrEmpty(metricNode.InnerText) && double.TryParse(metricNode.InnerText, out fMetricValue))
                        pMetricValue.Value = fMetricValue;
                    else
                        pMetricValue.Value = DBNull.Value;

                    nMetricsScavenged += dbCom.ExecuteNonQuery();
                }
            }

            return nMetricsScavenged;
        }

        /// <summary>
        /// Retrieves all metric definitions that have the argument TypeID, have an XPath defined, and are also tied to CHaMP monitoring IDs
        /// </summary>
        /// <param name="nMetricTypeID">Workbench LookupListItemID of the particular type of metrics to retrieve. See constants at top of this file.</param>
        /// <returns></returns>
        private List<ScavengeMetric> GetMetrics(int nMetricTypeID)
        {
            List<ScavengeMetric> lMetrics = new List<ScavengeMetric>();
            using (SQLiteConnection DBCon = new SQLiteConnection(m_sDBCon))
            {
                DBCon.Open();

                SQLiteCommand dbCom = new SQLiteCommand("SELECT MetricID, Title, CMMetricID, RBTResultXMLTag FROM Metric_Definitions WHERE (TypeID = @TypeID) AND (RBTResultXMLTag Is Not Null)", DBCon);
                dbCom.Parameters.AddWithValue("@TypeID", nMetricTypeID);
                SQLiteDataReader dbRead = dbCom.ExecuteReader();

                while (dbRead.Read())
                {
                    int nMetricID = dbRead.GetInt32(dbRead.GetOrdinal("MetricID"));
                    int nCMMetricID = dbRead.IsDBNull(dbRead.GetOrdinal("CMMetricID")) ? -1 : dbRead.GetInt32(dbRead.GetOrdinal("CMMetricID"));
                    string sTitle = dbRead.GetString(dbRead.GetOrdinal("Title"));
                    string sXpath = dbRead.GetString(dbRead.GetOrdinal("RBTResultXMLTag"));

                    lMetrics.Add(new ScavengeMetric(nMetricID, nCMMetricID, sTitle, sXpath));
                }
            }

            return lMetrics;
        }


        /// <summary>
        /// This class is used to keep a list of all metrics in memory that need processing from an RBT result XML file.
        /// </summary>
        /// <remarks>Typically a list of these metrics is loaded for just a particular TypeID (visit or channel unit tier 1 level etc)</remarks>
        private class ScavengeMetric
        {
            public int MetricID { get; internal set; }
            public int CMMetricID { get; internal set; }
            public string Name { get; internal set; }
            public string XPath { get; internal set; }

            public ScavengeMetric(int nMetricID, int nCMMetricID, string sName, string sXPath)
            {
                MetricID = nMetricID;
                if (nCMMetricID >= 0)
                    CMMetricID = nCMMetricID;
                Name = sName;
                XPath = sXPath;
            }

            public override string ToString()
            {
                return Name;
            }
        }




        /// <summary>
        /// Inserts the record that represents this RBT result file.
        /// </summary>
        /// <param name="dbTrans">Database transaction</param>
        /// <param name="sResultFile">Full path to the RBT result file.</param>
        /// <param name="xmlResults">XML document object representing the result file. This is used to get some basic properties of the RBT run.</param>
        /// <returns>The ID of the RBT result file record. This is used as the foreign key for inserting metrics.</returns>
        /// <remarks>
        /// This method only saves the record if the RBT version, run date time and VisitID values can be obtained from the result XML file.</remarks>
        private int InsertResultRecord(ref SQLiteTransaction dbTrans, string sResultFile, ref XmlDocument xmlResults)
        {
            int nResultID = 0;

            XmlNode nodVersion = xmlResults.SelectSingleNode("//log//habitat_version");
            if (nodVersion is XmlNode && !string.IsNullOrEmpty(nodVersion.InnerText))
            {
                XmlNode nodCreated = xmlResults.SelectSingleNode("//log//run_timestamp");
                DateTime dtCreated;
                if (nodCreated is XmlNode && !string.IsNullOrEmpty(nodCreated.InnerText) && DateTime.TryParse(nodCreated.InnerText, out dtCreated))
                {
                    XmlNode nodVisitID = xmlResults.SelectSingleNode("//log//simulations//simulation//sim_meta//meta[@key='visit']");
                    int nVisitID;
                    if (nodVisitID is XmlNode && !string.IsNullOrEmpty(nodVisitID.InnerText) && int.TryParse(nodVisitID.InnerText, out nVisitID))
                    {
                        SQLiteCommand dbCom = new SQLiteCommand("INSERT INTO Metric_Results (ResultFile, ModelVersion, VisitID, RunDateTime, ScavengeTypeID)" +
                            " VALUES (@ResultFile, @ModelVersion, @VisitID, @RBTRunDateTime, @ScavengeTypeID)", dbTrans.Connection, dbTrans);

                        dbCom.Parameters.AddWithValue("@ResultFile", sResultFile);
                        dbCom.Parameters.AddWithValue("@ModelVersion", nodVersion.InnerText);
                        dbCom.Parameters.AddWithValue("@VisitID", nVisitID);
                        dbCom.Parameters.AddWithValue("@RunDateTime", dtCreated.ToString());
                        dbCom.Parameters.AddWithValue("@ScavengeTypeID", m_nRBTScavengeTypeID);

                        dbCom.ExecuteNonQuery();

                        dbCom = new SQLiteCommand("SELECT @@Identity FROM Metric_Results", dbTrans.Connection, dbTrans);
                        object objResultID = dbCom.ExecuteScalar();
                        if (objResultID != null && objResultID != DBNull.Value && objResultID is int)
                        {
                            nResultID = (int)objResultID;
                        }
                    }
                }
            }

            return nResultID;
        }
    }
}

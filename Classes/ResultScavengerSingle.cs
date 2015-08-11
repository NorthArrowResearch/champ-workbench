using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Xml;

namespace CHaMPWorkbench.Classes
{
    class ResultScavengerSingle
    {
        private OleDbConnection m_dbCon;

        public ResultScavengerSingle(ref OleDbConnection dbCon)
        {
            m_dbCon = dbCon;
        }

        public int ScavengeResultFile(string sResultFilePath)
        {
            if (m_dbCon.State == System.Data.ConnectionState.Closed)
                m_dbCon.Open();

            int nResultID = 0;

            if (!string.IsNullOrEmpty(sResultFilePath) && System.IO.File.Exists(sResultFilePath))
            {
                XmlDocument xmlR = new XmlDocument();
                xmlR.Load(sResultFilePath);

                XmlNode metricNode = xmlR.SelectSingleNode("rbt_results/metric_results");
                if (metricNode is XmlNode && metricNode.HasChildNodes)
                {
                    try
                    {
                        System.Diagnostics.Debug.Print("Scavenging result file: " + sResultFilePath.ToString());
                        nResultID = PopulateTable_Visit(m_dbCon, metricNode, sResultFilePath);
                    }
                    catch (Exception ex)
                    {
                        //
                        // these are legimitate RBT XML result files that have errors. Add them
                        // to the running list of problems and continue with next file.
                        //
                        ex.Data["Result File"] = sResultFilePath;
                        throw ex;
                    }
                }
            }
            return nResultID;
        }

        public void ScavengeLogFile(int nResultID, String sLogFile, String sResultFilePath)
        {
            if (!string.IsNullOrEmpty(sLogFile) && !System.IO.File.Exists(sLogFile))
                return;

            XmlDocument xmlR = new XmlDocument();
            xmlR.Load(sLogFile);

            OleDbCommand dbCom = new OleDbCommand("INSERT INTO LogFiles (ResultID, LogfilePath, ResultFilePath, MetaDataInfo) VALUES (@ResultID, @LogFilePath, @ResultFilePath, @MetaDataInfo)", m_dbCon);

            if (nResultID > 0)
                dbCom.Parameters.AddWithValue("ResultID", nResultID);
            else
                dbCom.Parameters.AddWithValue("ResultID", DBNull.Value);

            dbCom.Parameters.AddWithValue("LogFilePath", sLogFile);

            OleDbParameter pResultFile = dbCom.Parameters.Add("ResultFilePath", OleDbType.VarChar);
            if (string.IsNullOrEmpty(sResultFilePath))
            {
                pResultFile.Value = DBNull.Value;
            }
            else
            {
                pResultFile.Value = sResultFilePath;
                pResultFile.Size = sResultFilePath.Length;
            }

            XmlNode xMeta = xmlR.SelectSingleNode("rbt/meta_data");
            OleDbParameter pMeta = dbCom.Parameters.Add("MetaDataInfo", OleDbType.VarChar);
            pMeta.Value = DBNull.Value;
            if (xMeta is XmlNode)
            {
                if (!string.IsNullOrEmpty(xMeta.InnerXml))
                {
                    pMeta.Value = xMeta.InnerXml;
                    pMeta.Size = xMeta.InnerXml.Length;
                }
            }
            dbCom.ExecuteNonQuery();
            //
            // Get the ID of this log file entry
            //
            dbCom = new OleDbCommand("SELECT @@Identity FROM LogFiles", m_dbCon);
            int nLogID = (int)dbCom.ExecuteScalar();
            if (nLogID > 0)
            {
                //
                // Now insert all the status messages and errors/warnings
                //
                dbCom = new OleDbCommand("INSERT INTO LogMessages (LogID, MessageType, LogSeverity, VisitID, LogDateTime, LogMessage, LogException, LogSolution)" +
                                                                " VALUES (@LogID, @MessageType, @MessageSeverity, @VisitID, @LogDateTime, @LogMessage, @LogException, @LogSolution)", m_dbCon);
                dbCom.Parameters.AddWithValue("LogID", nLogID);
                OleDbParameter pMessageType = dbCom.Parameters.Add("MessageType", OleDbType.VarChar);
                OleDbParameter pMessageSeverity = dbCom.Parameters.Add("MessageSeverity", OleDbType.VarChar);
                  OleDbParameter pVisitID = dbCom.Parameters.Add("VisitID", OleDbType.BigInt);
             OleDbParameter pLogDateTime = dbCom.Parameters.Add("LogDateTime", OleDbType.Date);
                OleDbParameter pLogMessage = dbCom.Parameters.Add("LogMessage", OleDbType.VarChar);
                OleDbParameter pLogException = dbCom.Parameters.Add("LogException", OleDbType.VarChar);
                OleDbParameter pLogSolution = dbCom.Parameters.Add("LogSolution", OleDbType.VarChar);
   
                foreach (XmlNode MessageNode in xmlR.SelectNodes("rbt/message"))
                {
                    XmlAttribute att = MessageNode.Attributes["severity"];
                    pMessageSeverity.Value = DBNull.Value;
                    if (att is XmlAttribute)
                    {
                        if (!string.IsNullOrEmpty(att.InnerText))
                        {
                            if (!string.IsNullOrEmpty(att.InnerText))
                            {
                                pMessageSeverity.Value = att.InnerText;
                                pMessageSeverity.Size = att.InnerText.Length;
                            }
                        }
                    }

                    att = MessageNode.Attributes["type"];
                    pMessageType.Value = DBNull.Value;
                    if (att is XmlAttribute)
                    {
                        if (!string.IsNullOrEmpty(att.InnerText))
                        {
                            if (!string.IsNullOrEmpty(att.InnerText))
                            {
                                pMessageType.Value = att.InnerText;
                                pMessageType.Size = att.InnerText.Length;
                            }
                        }
                    }
                    
                    att = MessageNode.Attributes["time"];
                    pLogDateTime.Value = DBNull.Value;
                    if (att is XmlAttribute)
                    {
                        if (!string.IsNullOrEmpty(att.InnerText))
                        {
                            DateTime aTime = default(DateTime);
                            if (DateTime.TryParse(att.InnerText, out aTime))
                            {
                                pLogDateTime.Value = aTime;
                            }
                        }
                    }

                    XmlNode aChildNode = MessageNode.SelectSingleNode("description");
                    pLogMessage.Value = DBNull.Value;
                    if (aChildNode is XmlNode)
                    {
                        if (!string.IsNullOrEmpty(aChildNode.InnerText))
                        {
                            pLogMessage.Value = aChildNode.InnerText.Trim();
                            //pLogMessage.Size = aChildNode.InnerText.Length;
                        }
                    }

                    aChildNode = MessageNode.SelectSingleNode("exception");
                    pLogException.Value = DBNull.Value;
                    if (aChildNode is XmlNode)
                    {
                        if (!string.IsNullOrEmpty(aChildNode.InnerText))
                        {
                            pLogException.Value = aChildNode.InnerText.Trim();
                            //pLogException.Size = aChildNode.InnerText.Length;
                        }
                    }

                    aChildNode = MessageNode.SelectSingleNode("solution");
                    pLogSolution.Value = DBNull.Value;
                    if (aChildNode is XmlNode)
                    {
                        if (!string.IsNullOrEmpty(aChildNode.InnerText))
                        {
                            pLogSolution.Value = aChildNode.InnerText.Trim();
                            //pLogSolution.Size = aChildNode.InnerText.Length;
                        }
                    }

                    aChildNode = MessageNode.SelectSingleNode("visit");
                    pVisitID.Value = DBNull.Value;
                    if (aChildNode is XmlNode)
                    {
                        if (!string.IsNullOrEmpty(aChildNode.InnerText))
                        {
                            long nVisitID = 0;
                            if (long.TryParse(aChildNode.InnerText,out nVisitID))
                                pVisitID.Value = nVisitID;
                        }
                    }

                    dbCom.ExecuteNonQuery();
                }
            }
        }

        private int PopulateTable_Visit(OleDbConnection dbCon, XmlNode xmlTopNode, string sRBTResultFilePath)
        {

            string sSQL = null;
            sSQL = "INSERT INTO Metric_SiteMetrics (ResultFile, VisitName, VisitID, FieldSeason, SiteName, RBTRunDateTime, RBTInputFile, RBTVersion, Artifacts" +
                ", LinearUnits, ReachLengthThalweg, ThalwegIncrementDistance, ReachWidthWetted, CoordAProjected, CoordAGeographic, CoordKProjected, CoordKGeographic, " +
                "SiteWaterSurfaceSlope, AreaSum, RP100, PoolTailCrestDepthAvg, PoolMaxDepthAvg, XBFHeight, XBFWidth, BnkFullChCap, AvgXSecArea, AcgXSecAreaRect, AvgChCap, " +
                "DEM_Left, DEM_Right, DEM_Top, DEM_Bottom, SiteGradient, SiteWaterSurfaceGradient, SiteSinuosity, SiteSinuosityCL, SiteArea, SiteAreaWetted, SiteAreaBankfull, " +
                "WettedVolume, SiteLengthWetted, SiteLengthBankfull, SiteLengthThalweg, ThalwegCLLengthRatio, IntegratedWettedWidth, IntegratedBankfullWidth, SiteBankAngleMean, " +
                "SiteBankAngleDeviation, DetrendedDEMStDev, BankfullVolume, WaterDepthStDev, BankfullMaxDepth, BankfullMeanDepth";

            sSQL += ") VALUES (";
            sSQL += "@RBTResultFile";
            // "'" & sRBTResultFilePath.Replace("'", "''") & "'"
            AddStringValue(ref sSQL, xmlTopNode, "./visit");
            AddNumericValue(ref sSQL, xmlTopNode, "./visitid");
            AddNumericValue(ref sSQL, xmlTopNode, "./field_season");
            AddStringValue(ref sSQL, xmlTopNode, "./site");
            sSQL += ", @RBTRunDateTime";
            sSQL += ", @RBTInputFile";
            AddStringValue(ref sSQL, xmlTopNode, "../meta_data/rbt_version"); // RBT Version
            AddStringValue(ref sSQL, xmlTopNode, "./meta_data/artifacts");
            AddStringValue(ref sSQL, xmlTopNode, "./linear_units");
            AddNumericValue(ref sSQL, xmlTopNode, "./reach_length_thalweg");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg_increment_distance");
            AddNumericValue(ref sSQL, xmlTopNode, "./reach_width_wetted");
            AddStringValue(ref sSQL, xmlTopNode, "./coord_a_projected");
            AddStringValue(ref sSQL, xmlTopNode, "./coord_a_geographic");
            AddStringValue(ref sSQL, xmlTopNode, "./coord_k_projected");
            AddStringValue(ref sSQL, xmlTopNode, "./coord_k_geographic");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_water_surface_slope");
            AddNumericValue(ref sSQL, xmlTopNode, "./areasum");
            AddNumericValue(ref sSQL, xmlTopNode, "./rp100");
            AddNumericValue(ref sSQL, xmlTopNode, "./pool_tail_crest_depth_avg");
            AddNumericValue(ref sSQL, xmlTopNode, "./pool_max_depth_avg");
            AddNumericValue(ref sSQL, xmlTopNode, "./xbfheight");
            AddNumericValue(ref sSQL, xmlTopNode, "./xbfwidth");
            AddNumericValue(ref sSQL, xmlTopNode, "./bnkfullchcap");
            AddNumericValue(ref sSQL, xmlTopNode, "./avgxsecarea");
            AddNumericValue(ref sSQL, xmlTopNode, "./avgxsecarearect");
            AddNumericValue(ref sSQL, xmlTopNode, "./avgchcap");
            AddNumericValue(ref sSQL, xmlTopNode, "./dem/left");
            AddNumericValue(ref sSQL, xmlTopNode, "./dem/right");
            AddNumericValue(ref sSQL, xmlTopNode, "./dem/top");
            AddNumericValue(ref sSQL, xmlTopNode, "./dem/bottom");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_gradient");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_water_surface_gradient");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_sinuosity");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_sinuosity_centerline");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_area");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_area_wetted");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_area_bankfull");
            AddNumericValue(ref sSQL, xmlTopNode, "./wetted_volume");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_length_wetted");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_length_bankfull");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_length_thalweg");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg_centerline_length_ratio");
            AddNumericValue(ref sSQL, xmlTopNode, "./integrated_wetted_width");
            AddNumericValue(ref sSQL, xmlTopNode, "./integrated_bankfull_width");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_bank_angle_mean");
            AddNumericValue(ref sSQL, xmlTopNode, "./site_bank_angle_standard_deviation");
            AddNumericValue(ref sSQL, xmlTopNode, "./detrended_dem_standard_deviation");
            AddNumericValue(ref sSQL, xmlTopNode, "./bankfull_volume");
            AddNumericValue(ref sSQL, xmlTopNode, "./water_depth_standard_deviation");
            AddNumericValue(ref sSQL, xmlTopNode, "./bankfull_max_depth");
            AddNumericValue(ref sSQL, xmlTopNode, "./bankfull_mean_depth");

            sSQL += ")";
            System.Diagnostics.Debug.WriteLine(sSQL);

            int nVisitID = 0;
            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.Parameters.AddWithValue("RBTResultFile", sRBTResultFilePath);

                XmlNode xNode = xmlTopNode.SelectSingleNode("../meta_data/date_time_created");
                OleDbParameter pRunTime = dbCom.Parameters.Add("RBTRunDateTime", OleDbType.Date);
                pRunTime.Value = DBNull.Value;
                if (xNode is XmlNode)
                {
                    if (!string.IsNullOrEmpty(xNode.InnerText))
                    {
                        DateTime aTime = default(DateTime);
                        if (DateTime.TryParse(xNode.InnerText, out aTime))
                        {
                            pRunTime.Value = aTime;
                        }
                    }
                }

                xNode = xmlTopNode.SelectSingleNode("../meta_data/inputs");
                OleDbParameter pInputfile = dbCom.Parameters.Add("RBTInputFile", OleDbType.VarChar, 255);
                pInputfile.Value = DBNull.Value;
                if (xNode is XmlNode)
                {
                    if (!string.IsNullOrEmpty(xNode.InnerText))
                    {
                        pInputfile.Value = xNode.InnerText;
                        pInputfile.Size = xNode.InnerText.Length;
                    }
                }

                dbCom.ExecuteNonQuery();

                dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_SiteMetrics", m_dbCon);
                OleDbDataReader dbRdr = dbCom.ExecuteReader();
                if (dbRdr.Read())
                {
                    if (!System.Convert.IsDBNull(dbRdr[0]))
                    {
                        nVisitID = (int)dbRdr[0];
                    }
                }
                dbRdr.Close();

            }
            catch (OleDbException ex)
            {
                throw new Exception("Error generating visit database record", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating visit database record", ex);
            }
            //
            // Now do all the child tables of the visit
            //
            if (nVisitID > 0)
            {
                PopulateTable_ChannelSegments(dbCon, xmlTopNode, nVisitID);

                //PopulateTable_ChannelUnits(dbCon, xmlTopNode, nVisitID);
                PopulateTable_ChannelUnitSummary(dbCon, xmlTopNode, nVisitID, "tier1");
                PopulateTable_ChannelUnitSummary(dbCon, xmlTopNode, nVisitID, "tier2");
                PopulateTable_FlowPoints(dbCon, xmlTopNode, nVisitID, "in_flow_point");
                PopulateTable_FlowPoints(dbCon, xmlTopNode, nVisitID, "out_flow_point");
                PopulateTable_ThalwegEmap(dbCon, xmlTopNode, nVisitID);
                PopulateTable_Thalweg(dbCon, xmlTopNode, nVisitID);
                PopulateTable_Centerlines(dbCon, xmlTopNode, "wetted", nVisitID);
                PopulateTable_Centerlines(dbCon, xmlTopNode, "bankfull", nVisitID);

                PopulateTable_Islands(dbCon, xmlTopNode, "wetted", nVisitID);
                PopulateTable_Islands(dbCon, xmlTopNode, "bankfull", nVisitID);
                PopulateTable_IslandSummary(dbCon, xmlTopNode, "wetted", nVisitID);
                PopulateTable_IslandSummary(dbCon, xmlTopNode, "bankfull", nVisitID);

                PopulateTable_Profiles(dbCon, xmlTopNode, nVisitID);
                PopulateTable_ChangeDetection(dbCon, xmlTopNode, nVisitID);
            }

            return nVisitID;
        }


        private void PopulateTable_ChangeDetection(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID)
        {

            foreach (XmlNode dodNode in xmlTopNode.SelectNodes("./change_detection_results/dod"))
            {
                string sSQL = null;
                sSQL = "INSERT INTO Metric_ChangeDetection (ResultID, NewVisit, NewfieldSeason, NewVisitID, OldVisit, OldFieldSeason, OldVisitID, Epoch, ThresholdType, Threshold, SpatialCoherence";

                sSQL += ") VALUES (" + nVisitID.ToString();

                AddStringValue(ref sSQL, dodNode, "./new_visit_name");
                AddNumericValue(ref sSQL, dodNode, "./new_visit_year");
                AddNumericValue(ref sSQL, dodNode, "./new_visit_id");
                AddStringValue(ref sSQL, dodNode, "./old_visit_name");
                AddNumericValue(ref sSQL, dodNode, "./old_visit_year");
                AddNumericValue(ref sSQL, dodNode, "./old_visit_id");

                XmlAttribute xmlAtt = dodNode.Attributes["epoch"];
                if (xmlAtt is XmlAttribute && !string.IsNullOrEmpty(xmlAtt.InnerText))
                {
                    sSQL += ", '" + xmlAtt.InnerText.Replace("'", "") + "'";
                }
                else
                {
                    sSQL += "NULL";
                }

                xmlAtt = dodNode.Attributes["type"];
                if (xmlAtt is XmlAttribute && !string.IsNullOrEmpty(xmlAtt.InnerText))
                {
                    sSQL += ", '" + xmlAtt.InnerText.Replace("'", "") + "'";
                }
                else
                {
                    sSQL += "NULL";
                }

                xmlAtt = dodNode.Attributes["threshold"];
                if (xmlAtt is XmlAttribute && !String.IsNullOrWhiteSpace(xmlAtt.InnerText))
                {
                    sSQL += ", " + xmlAtt.InnerText;
                }
                else
                {
                    sSQL += "NULL";
                }

                xmlAtt = dodNode.Attributes["spatial_coherence"];
                if (xmlAtt is XmlAttribute && !String.IsNullOrWhiteSpace(xmlAtt.InnerText))
                {
                    sSQL += ", " + xmlAtt.InnerText;
                }
                else
                {
                    sSQL += "NULL";
                }

                sSQL += ")";
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                try
                {
                    dbCom.ExecuteNonQuery();

                    int nChangeDetectionID = 0;
                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_ChangeDetection", dbCon);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            nChangeDetectionID = (int)dbRdr[0];
                            PopulateTable_BudgetSegegration(dbCon, dodNode, nChangeDetectionID);
                        }
                    }
                    dbRdr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inserting change detection");
                }
            }
        }

        private void AddDoubleValueUpdate(ref string sSQL, XmlNode xmlContainingNode, string sTag, string sFieldName)
        {
            XmlNode aNode = xmlContainingNode.SelectSingleNode("./" + sTag);
            if (aNode is XmlNode)
            {
                if (!String.IsNullOrWhiteSpace(aNode.InnerText))
                {
                    //if (System.Convert.IsNumeric(aNode.InnerText))
                    {
                        sSQL += ", " + sFieldName + " = " + aNode.InnerText;
                    }
                }
            }
        }


        private void PopulateTable_BudgetSegegration(OleDbConnection dbCon, XmlNode xmlTopNode, int nChangeDetectionID)
        {
            //("./change_detection/dod")
            foreach (XmlNode aBudgetSegNode in xmlTopNode.ChildNodes)
            {

                if (aBudgetSegNode.Name.ToLower().Contains("site") || aBudgetSegNode.Name.ToLower().Contains("tier") || aBudgetSegNode.Name.ToLower().Contains("bankfull"))
                {
                    string sSQL = "INSERT INTO Metric_BudgetSegregations (";
                    sSQL += "ChangeDetectionID" + ", Mask";

                    sSQL += ") VALUES (";
                    sSQL += nChangeDetectionID.ToString() + ", ";
                    sSQL += "'" + aBudgetSegNode.Name.Replace("'", "") + "'";
                    sSQL += ")";

                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                    dbCom.ExecuteNonQuery();

                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_BudgetSegregations", dbCon);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            int nBudgetSegragationID = (int)dbRdr[0];

                            if (aBudgetSegNode.Name.ToLower().Contains("site"))
                            {
                                PopulateTable_BudgetSegragationValues(dbCon, aBudgetSegNode, nBudgetSegragationID, "site");
                            }
                            else
                            {
                                foreach (XmlNode aChannelUnitTypeNode in aBudgetSegNode.ChildNodes)
                                {
                                    PopulateTable_BudgetSegragationValues(dbCon, aChannelUnitTypeNode, nBudgetSegragationID, aChannelUnitTypeNode.Name);
                                }
                            }

                        }
                    }
                    dbRdr.Close();
                }
            }

        }


        private void PopulateTable_BudgetSegragationValues(OleDbConnection dbCon, XmlNode xmlBudgetNode, int nBudgetSegragationID, string sMaskValueName)
        {
            string sSQL = "INSERT INTO Metric_BudgetSegregationValues (BudgetID" + ", MaskValueName" + ", RawAreaErosion" + ", RawAreaDeposition" + ", ThresholdAreaErosion" + ", ThresholdAreaDeposition" + ", AreaDetectableChange" + ", AreaOfInterestRaw" + ", PercentAreaOfInterestDetectableChange" + ", RawVolumeErosion" + ", ThresholdVolumeErosion" + ", ErrorVolumeErosion" + ", ThresholdPercentErosion" + ", RawVolumeDeposition" + ", ThresholdVolumeDeposition" + ", ErrorVolumeDeposition" + ", ThresholdPercentDeposition" + ", RawVolumeDifference" + ", ThresholdedVolumeDifference" + ", ErrorVolumeDifference" + ", VolumeDifferencePercent" + ", AverageDepthErosionRaw" + ", AverageDepthErosionThreshold" + ", AverageDepthErosionError" + ", AverageDepthErosionPercent" + ", AverageDepthDepositionRaw" + ", AverageDepthDepositionThreshold" + ", AverageDepthDepositionError" + ", AverageDepthDepositionPercent" + ", AverageThicknessDifferenceAOIRaw" + ", AverageThicknessDifferenceAOIThresholded" + ", AverageThicknessDifferenceAOIError" + ", AverageThicknessDifferenceAOIPercent" + ", AverageNetThicknessDifferenceAOIRaw" + ", AverageNetThicknessDifferenceAOIThresholded" + ", AverageNetThicknessDifferenceAOIError" + ", AverageNetThicknessDifferenceAOIPercent" + ", AverageThicknessDifferenceADCThresholded" + ", AverageThicknessDifferenceADCError" + ", AverageThicknessDifferenceADCPercent" + ", AverageNetThicknessDifferenceADCThresholded" + ", AverageNetThicknessDifferenceADCError" + ", AverageNetThicknessDifferenceADCPercent" + ", PercentErosionRaw" + ", PercentErosionThresholded" + ", PercentDepositionRaw" + ", PercentDepositionThresholded" + ", PercentImbalanceRaw" + ", PercentImbalanceThresholded" + ", PercentNetVolumeRatioRaw" + ", PercentNetVolumeRatioThresholded" + ") VALUES (" + nBudgetSegragationID;

            sSQL += ", '" + sMaskValueName.Replace("'", "") + "'";
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_area_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_area_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_area_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_area_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "area_detectable_change");
            AddNumericValue(ref sSQL, xmlBudgetNode, "area_of_interest_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_area_of_interest_detectable_change");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_percent_erosion");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_percent_deposition");
            AddNumericValue(ref sSQL, xmlBudgetNode, "raw_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "thresholded_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "error_volume_difference");
            AddNumericValue(ref sSQL, xmlBudgetNode, "volume_difference_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_erosion_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_depth_deposition_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_aoi_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_aoi_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_thickness_difference_adc_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_error");
            AddNumericValue(ref sSQL, xmlBudgetNode, "average_net_thickness_difference_adc_percent");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_erosion_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_erosion_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_deposition_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_deposition_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_imbalance_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_imbalance_thresholded");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_net_volume_ratio_raw");
            AddNumericValue(ref sSQL, xmlBudgetNode, "percent_net_volume_ratio_thresholded");

            sSQL += ")";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting budget segregation values " + nBudgetSegragationID.ToString());
            }
        }
        private void PopulateTable_ChannelSegments(OleDbConnection dbCon, XmlNode xmlTopNode, int nResultID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_ChannelSegments (SegmentNumber, ResultID, MaxDepth, Area, Volume, UnitCount) VALUES (";
            sSQL += "@SegmentNumber, " + nResultID.ToString() + ", @MaxDepth, @Area, @Volume, @UnitCount)";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                OleDbParameter pSegmentNumber = dbCom.Parameters.Add("SegmentNumber", OleDbType.BigInt);
                OleDbParameter pMaxDepth = dbCom.Parameters.Add("MaxDepth", OleDbType.Single);
                OleDbParameter pArea = dbCom.Parameters.Add("Area", OleDbType.Single);
                OleDbParameter pVolume = dbCom.Parameters.Add("Volume", OleDbType.Single);
                OleDbParameter pUnitCount = dbCom.Parameters.Add("UnitCount", OleDbType.Integer);

                foreach (XmlNode aNode in xmlTopNode.SelectNodes("./channel_segments/segment"))
                {
                    GetIntegerValueFromNode(pSegmentNumber, aNode, "./segment_number");
                    GetStringValueFromNode(pMaxDepth, aNode, "./max_depth");
                    GetDoubleValueFromNode(pArea, aNode, "./area");
                    GetDoubleValueFromNode(pVolume, aNode, "./volume");
                    GetDoubleValueFromNode(pUnitCount, aNode, "./unit_count");
                    dbCom.ExecuteNonQuery();

                    int nSegmentID = 0;

                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_ChannelSegments", m_dbCon);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            nSegmentID = (int)dbRdr[0];
                        }
                    }
                    dbRdr.Close();

                    PopulateTable_ChannelUnits(dbCon, aNode, (int)nSegmentID);
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating channel segments", ex);
                ex2.Data.Add("Visit", nResultID.ToString());
                throw ex2;
            }
        }

        private void PopulateTable_ChannelUnits(OleDbConnection dbCon, XmlNode xmlSegmentNode, int nSegmentID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_ChannelUnits (SegmentID, UnitNumber, Tier1, Tier2, Area, Volume, PC, MaxDepth, DepthThalwegExit, ResidualDepth) VALUES (";

            sSQL += nSegmentID.ToString() + ", @UnitNumber, @Tier1, @Tier2, @Area, @Volume, @Percent, @MaxDepth, @DepthThalwegExit, @ResidualDepth)";
            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                OleDbParameter pUnitNumber = dbCom.Parameters.Add("UnitNumber", OleDbType.BigInt);
                OleDbParameter pTier1 = dbCom.Parameters.Add("Tier1", OleDbType.VarChar, 255);
                OleDbParameter pTier2 = dbCom.Parameters.Add("Tier2", OleDbType.VarChar, 255);
                OleDbParameter pArea = dbCom.Parameters.Add("Area", OleDbType.Double);
                OleDbParameter pVolume = dbCom.Parameters.Add("Volume", OleDbType.Double);
                OleDbParameter pPercent = dbCom.Parameters.Add("Percent", OleDbType.Double);
                OleDbParameter pMaxDepth = dbCom.Parameters.Add("MaxDepth", OleDbType.Double);
                OleDbParameter pDepthThalwegExit = dbCom.Parameters.Add("DepthThalwegExit", OleDbType.Double);
                OleDbParameter pResidualDepth = dbCom.Parameters.Add("ResidualDepth", OleDbType.Double);

                foreach (XmlNode aNode in xmlSegmentNode.SelectNodes("./channel_units/unit"))
                {
                    GetIntegerValueFromNode(pUnitNumber, aNode, "./unit_number");
                    GetStringValueFromNode(pTier1, aNode, "./tier1");
                    GetStringValueFromNode(pTier2, aNode, "./tier2");
                    GetDoubleValueFromNode(pArea, aNode, "./area");
                    GetDoubleValueFromNode(pVolume, aNode, "./volume");
                    GetDoubleValueFromNode(pPercent, aNode, "./percent");
                    GetDoubleValueFromNode(pMaxDepth, aNode, "./max_depth");
                    GetDoubleValueFromNode(pDepthThalwegExit, aNode, "./depth_thalweg_exit");
                    GetDoubleValueFromNode(pResidualDepth, aNode, "./residual_depth");

                    dbCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating channel units", ex);
                ex2.Data.Add("Segment ID", nSegmentID.ToString());
                throw ex2;
            }
        }

        private void PopulateTable_ChannelUnitSummary(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID, string sType)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_ChannelUnitSummary (VisitID,Tier,Title,Area,Volume,UnitCount,Frequency,Spacing,PC,AvgMaxDepth,AvgDepthThalwegExit,AvgResidualDepth) VALUES (";
            sSQL += nVisitID.ToString() + ", '" + sType + "'" + ", @Title" + ", @Area" + ", @Volume" + ", @Count" + ", @Frequency" + ", @Spacing" + ", @Percent" + ", @MaxDepth" + ", @DepthThalwegExit" + ", @ResidualDepth" + ")";
            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                OleDbParameter pTitle = dbCom.Parameters.Add("Title", OleDbType.VarChar, 255);
                OleDbParameter pArea = dbCom.Parameters.Add("Area", OleDbType.Double);
                OleDbParameter pVolume = dbCom.Parameters.Add("Volume", OleDbType.Double);
                OleDbParameter pCount = dbCom.Parameters.Add("Count", OleDbType.BigInt);
                OleDbParameter pFrequency = dbCom.Parameters.Add("Frequency", OleDbType.Double);
                OleDbParameter pSpacing = dbCom.Parameters.Add("Spacing", OleDbType.Double);
                OleDbParameter pPercent = dbCom.Parameters.Add("Percent", OleDbType.Double);
                OleDbParameter pMaxDepth = dbCom.Parameters.Add("MaxDepth", OleDbType.Double);
                OleDbParameter pDepthThalwegExit = dbCom.Parameters.Add("DepthThalwegExit", OleDbType.Double);
                OleDbParameter pResidualDepth = dbCom.Parameters.Add("ResidualDepth", OleDbType.Double);

                foreach (XmlNode aNode in xmlTopNode.SelectNodes("./habitat_summary/" + sType + "/unit"))
                {
                    GetStringValueFromNode(pTitle, aNode, "./name");
                    GetDoubleValueFromNode(pArea, aNode, "./area");
                    GetDoubleValueFromNode(pVolume, aNode, "./volume");
                    GetIntegerValueFromNode(pCount, aNode, "./count");
                    GetDoubleValueFromNode(pFrequency, aNode, "./frequency");
                    GetDoubleValueFromNode(pSpacing, aNode, "./spacing");
                    GetDoubleValueFromNode(pPercent, aNode, "./percent");
                    GetDoubleValueFromNode(pMaxDepth, aNode, "./avg_max_depth");
                    GetDoubleValueFromNode(pDepthThalwegExit, aNode, "./avg_depth_thalweg_exit");
                    GetDoubleValueFromNode(pResidualDepth, aNode, "./avg_residual_depth");
                    dbCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating channel unit summary", ex);
                ex2.Data.Add("Visit", nVisitID.ToString());
                throw ex2;
            }
        }

        private void PopulateTable_Islands(OleDbConnection dbCon, XmlNode xmlTopNode, string sWaterExtent, int nResultID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_Islands (ResultID, WaterExtent, Type, Area, Circumference) VALUES (";
            sSQL += nResultID.ToString() + ", @WaterExtent, @Type, @Area, @Circumference)";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.Parameters.AddWithValue("WaterExtent", sWaterExtent);

                OleDbParameter pType = dbCom.Parameters.Add("Type", OleDbType.VarChar, 255);
                OleDbParameter pArea = dbCom.Parameters.Add("Area", OleDbType.Double);
                OleDbParameter pCircumference = dbCom.Parameters.Add("Circumference", OleDbType.Double);

                string sXPathRoot = "./" + sWaterExtent.ToLower() + "_channel/islands/islands";

                foreach (XmlNode aNode in xmlTopNode.SelectNodes(sXPathRoot + "/island"))
                {
                    GetStringValueFromNode(pType, aNode, "./type");
                    GetDoubleValueFromNode(pArea, aNode, "./area");
                    GetDoubleValueFromNode(pCircumference, aNode, "./circumference");
                    dbCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating channel units", ex);
                ex2.Data.Add("Result ID", nResultID.ToString());
                throw ex2;
            }
        }

        private void PopulateTable_IslandSummary(OleDbConnection dbCon, XmlNode xmlTopNode, string sWaterExtent, int nResultID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_IslandSummary (ResultID, WaterExtent, TotalCount, QualifyingCount, NonQualifyingCount, QualifyingArea, NonQualifyingArea) VALUES (";
            sSQL += nResultID.ToString() + ", @WaterExtent, @TotalCount, @QualifyingCount, @NonQualifyingCount, @QualifyingArea, @NonQualifyingArea)";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.Parameters.AddWithValue("WaterExtent", sWaterExtent);

                OleDbParameter pTotalCount = dbCom.Parameters.Add("TotalCount", OleDbType.Integer);
                OleDbParameter pQualifyingCount = dbCom.Parameters.Add("QualifyingCount", OleDbType.Integer);
                OleDbParameter pNonQualifyingCount = dbCom.Parameters.Add("NonQualifyingCount", OleDbType.Integer);
                OleDbParameter pQualifyingArea = dbCom.Parameters.Add("QualifyingArea", OleDbType.Double);
                OleDbParameter pNonQualifyingArea = dbCom.Parameters.Add("NonQualifyingArea", OleDbType.Double);

                string sXPathRoot = "./" + sWaterExtent.ToLower() + "_channel/islands/islands";
                foreach (XmlNode aNode in xmlTopNode.SelectNodes(sXPathRoot + "/summary"))
                {
                    GetIntegerValueFromNode(pTotalCount, aNode, "./island_count");
                    GetIntegerValueFromNode(pQualifyingCount, aNode, "./qualifying_count");
                    GetIntegerValueFromNode(pNonQualifyingCount, aNode, "./nonqualifying_count");
                    GetDoubleValueFromNode(pQualifyingArea, aNode, "./qualifying_area");
                    GetDoubleValueFromNode(pNonQualifyingArea, aNode, "./nonqualifying_area");
                    dbCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating channel units", ex);
                ex2.Data.Add("Result ID", nResultID.ToString());
                throw ex2;
            }
        }

        private void PopulateTable_FlowPoints(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID, string sType)
        {
            string sSQL = "INSERT INTO Metric_FlowPoints (VisitID, Type, X, Y, Latitude, Longitude) VALUES (";
            sSQL += nVisitID.ToString();
            sSQL += ", '" + sType + "'";
            AddNumericValue(ref sSQL, xmlTopNode, "./" + sType + "/x");
            AddNumericValue(ref sSQL, xmlTopNode, "./" + sType + "/y");
            AddNumericValue(ref sSQL, xmlTopNode, "./" + sType + "/latitude");
            AddNumericValue(ref sSQL, xmlTopNode, "./" + sType + "/longitude");

            sSQL += ")";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating flow points", ex);
                ex2.Data.Add("Visit", nVisitID.ToString());
                ex2.Data.Add("Type", sType);
                throw ex2;
            }

        }

        private void PopulateTable_ThalwegEmap(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_ThalwegEmap (" + "VisitID" + ", Distance" + ", X" + ", Y" + ", Latitude" + ", Longitude" + ", Elevation" + ", Depth" + ", ResidualDepth" + ", DepthToResidualSurface";

            sSQL += ") VALUES (" + nVisitID.ToString() + ", @Distance" + ", @X" + ", @Y" + ", @Latitude" + ", @Longitude" + ", @Elevation" + ", @Depth" + ", @ResidualDepth" + ", @DepthToResidualSurface" + ")";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                OleDbParameter pDistance = dbCom.Parameters.Add("Distance", OleDbType.BigInt);
                OleDbParameter pX = dbCom.Parameters.Add("X", OleDbType.Double);
                OleDbParameter pY = dbCom.Parameters.Add("Y", OleDbType.Double);
                OleDbParameter pLatitude = dbCom.Parameters.Add("Latitude", OleDbType.Double);
                OleDbParameter pLongitude = dbCom.Parameters.Add("Longitude", OleDbType.Double);
                OleDbParameter pElevation = dbCom.Parameters.Add("Elevation", OleDbType.Double);
                OleDbParameter pDepth = dbCom.Parameters.Add("Depth", OleDbType.Double);
                OleDbParameter pResidualDepth = dbCom.Parameters.Add("ResidualDepth", OleDbType.Double);
                OleDbParameter pDepthToResidualSurface = dbCom.Parameters.Add("DepthToResidualSurface", OleDbType.Double);

                XmlNodeList xmlPoints = xmlTopNode.SelectNodes("./thalweg_emap/point");

                foreach (XmlNode aNode in xmlPoints)
                {
                    pDistance.Value = DBNull.Value;
                    XmlAttribute xmlDistance = aNode.Attributes["distance"];
                    string sValue = "";
                    double fValue = 0;

                    if (xmlDistance is XmlAttribute)
                    {
                        // old XML structure with distance as attribute
                        sValue = xmlDistance.InnerText;
                    }
                    else
                    {
                        // new XML structure with distance as a node
                        XmlNode pNewPointNode = aNode.SelectSingleNode("./distance");
                        if (pNewPointNode is XmlNode)
                        {
                            sValue = pNewPointNode.InnerText;
                        }
                    }

                    if (double.TryParse(sValue, out fValue))
                    {
                        pDistance.Value = fValue;
                        GetDoubleValueFromNode(pX, aNode, "./x");
                        GetDoubleValueFromNode(pY, aNode, "./y");
                        GetDoubleValueFromNode(pLatitude, aNode, "./latitude");
                        GetDoubleValueFromNode(pLongitude, aNode, "./longitude");
                        GetDoubleValueFromNode(pElevation, aNode, "./elevation");
                        GetDoubleValueFromNode(pDepth, aNode, "./depth");
                        GetDoubleValueFromNode(pResidualDepth, aNode, "./residual_depth");
                        GetDoubleValueFromNode(pDepthToResidualSurface, aNode, "./depth_to_residual_surface");
                        dbCom.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating Thalweg points", ex);
                ex2.Data.Add("Visit", nVisitID);
                throw ex2;
            }

        }


        private void PopulateTable_Thalweg(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_Thalweg (" + "VisitID" + ", FromX" + ", FromY" + ", FromLat" + ", FromLng" + ", ToX" + ", ToY" + ", ToLat" + ", ToLng" + ", FromBedElevation" + ", ToBedElevation" + ", FromWSElevation" + ", ToWSElevation" + ", Length" + ", Depth";

            sSQL += ") VALUES (" + nVisitID.ToString();

            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/x");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/y");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/latitude");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/longitude");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/x");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/y");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/latitude");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/longitude");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/bed_elevation");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/bed_elevation");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/from_point/water_surface_elevation");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/to_point/water_surface_elevation");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/length");
            AddNumericValue(ref sSQL, xmlTopNode, "./thalweg/depth");

            sSQL += ")";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating Thalweg points", ex);
                ex2.Data.Add("Visit", nVisitID);
                throw ex2;
            }

        }


        private void PopulateTable_Centerlines(OleDbConnection dbCon, XmlNode xmlTopNode, string sType, int nResultID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO Metric_Centerlines (ResultID, Type, CountMain, CountSide, CountTotal, LengthMain, LengthSide, LengthTotal,braidedness) VALUES (";
            sSQL += nResultID.ToString() + ", '" + sType.Replace("'", "") + "'";

            string sXPathRoot = "./" + sType.ToLower() + "_channel/centerline";
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/mainstem_count");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/side_channel_count");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/channel_count");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/mainstem_length");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/side_channel_length");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/total_channel_length");
            AddNumericValue(ref sSQL, xmlTopNode, sXPathRoot + "/summary/braidedness");

            sSQL += ")";

            try
            {
                OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                dbCom.ExecuteNonQuery();

                int nCenterlineID = 0;
                dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Metric_Centerlines", dbCon);
                OleDbDataReader dbRdr = dbCom.ExecuteReader();
                if (dbRdr.Read())
                {
                    if (!System.Convert.IsDBNull(dbRdr[0]))
                    {
                        nCenterlineID = (int)dbRdr[0];
                    }
                }
                dbRdr.Close();

                PopulateTable_CenterlineParts(dbCon, xmlTopNode.SelectSingleNode("./" + sType.ToLower() + "_channel/centerline"), nCenterlineID);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating centerlines", ex);
                ex2.Data.Add("Visit", nResultID);
                throw ex2;
            }
        }

        private void PopulateTable_CenterlineParts(OleDbConnection dbCon, XmlNode xmlTopNode, int nCenterlineID)
        {
            if (!(xmlTopNode is XmlNode))
                return;

            string sSQL = "INSERT INTO Metric_CenterlineParts (CenterlineID, PartNumber, Type, Length, Sinuosity) VALUES (";
            sSQL += nCenterlineID.ToString() + ", @PartNumber, @Type, @Length, @Sinuosity)";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            OleDbParameter pPartNumber = dbCom.Parameters.Add("PartNumber", OleDbType.Integer);
            OleDbParameter pType = dbCom.Parameters.Add("Type", OleDbType.VarChar);
            OleDbParameter pLength = dbCom.Parameters.Add("Length", OleDbType.Double);
            OleDbParameter pSinuosity = dbCom.Parameters.Add("Sinuosity", OleDbType.Single);

            try
            {
                foreach (XmlNode aNode in xmlTopNode.SelectNodes("./parts/part"))
                {
                    string sPartNumber = aNode.SelectSingleNode("./id").InnerText;
                    if (string.IsNullOrWhiteSpace(sPartNumber))
                    {
                        Exception ex = new Exception("The part number is missing for a segment.");
                        ex.Data["Centerline"] = nCenterlineID.ToString();
                        throw ex;
                    }
                    else
                    {
                        int nPartNumber = 0;
                        if (int.TryParse(sPartNumber, out nPartNumber))
                            pPartNumber.Value = nPartNumber;
                        else
                        {
                            Exception ex = new Exception("The part number cannot be parsed to an integer.");
                            ex.Data["Centerline"] = nCenterlineID.ToString();
                            throw ex;
                        }
                    }

                    string sType = aNode.SelectSingleNode("type").InnerText;
                    if (string.IsNullOrEmpty(sType))
                    {
                        pType.Value = "Unknown";
                    }
                    else
                    {
                        pType.Value = sType;
                    }
                    pType.Size = sType.Length;

                    pLength.Value = DBNull.Value;
                    string sValue = aNode.SelectSingleNode("length").InnerText;
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        double fValue = 0;
                        if (double.TryParse(sValue, out fValue))
                        {
                            pLength.Value = fValue;
                        }
                    }

                    pSinuosity.Value = DBNull.Value;
                    sValue = aNode.SelectSingleNode("sinuosity").InnerText;
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        double fValue = 0;
                        if (double.TryParse(sValue, out fValue))
                        {
                            pSinuosity.Value = fValue;
                        }
                    }

                    dbCom.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating centerline segments", ex);
                ex2.Data.Add("CenterlineID", nCenterlineID);
                throw ex2;
            }
        }

        private void PopulateTable_Profiles(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID)
        {
            AddProfile(dbCon, xmlTopNode, nVisitID, "gradient");
            AddProfile(dbCon, xmlTopNode, nVisitID, "water_surface_gradient");
            AddProfile(dbCon, xmlTopNode, nVisitID, "thalweg_elevation_profile");
            AddProfile(dbCon, xmlTopNode, nVisitID, "thalweg_depth_profile");
            AddProfile(dbCon, xmlTopNode, nVisitID, "thalweg_water_surface_profile");

            AddProfile(dbCon, xmlTopNode, nVisitID, "centerline_elevation_profile");
            AddProfile(dbCon, xmlTopNode, nVisitID, "centerline_depth_profile");
            AddProfile(dbCon, xmlTopNode, nVisitID, "centerline_water_surface_profile");

            AddProfile(dbCon, xmlTopNode, nVisitID, "bankfull_width");
            AddProfile(dbCon, xmlTopNode, nVisitID, "bankfull_width_constriction");
            AddProfile(dbCon, xmlTopNode, nVisitID, "bankfull_width_to_max_depth");
            AddProfile(dbCon, xmlTopNode, nVisitID, "bankfull_width_to_avg_depth");
            AddProfile(dbCon, xmlTopNode, nVisitID, "wetted_width");
            AddProfile(dbCon, xmlTopNode, nVisitID, "wetted_width_constriction");
            AddProfile(dbCon, xmlTopNode, nVisitID, "wetted_width_to_max_depth");
            AddProfile(dbCon, xmlTopNode, nVisitID, "wetted_width_to_avg_depth");
        }

        private void AddProfile(OleDbConnection dbCon, XmlNode xmlTopNode, int nVisitID, string sProfileName)
        {
            string sSQLBase = "INSERT INTO Metric_Profiles (VisitID, ProfileName, Filtering, ProfileMean, ProfileStDev, ProfileCV, ProfileCount, ProfileMin, ProfileMax) VALUES (";
            sSQLBase += nVisitID.ToString() + ", '" + sProfileName.Replace("'", "''") + "'";

            List<string> sFiltering = new List<string>();
            sFiltering.Add("none");
            sFiltering.Add("crew");
            sFiltering.Add("best");
            sFiltering.Add("auto");

            foreach (string sFilter in sFiltering)
            {
                string sSQL = sSQLBase + ",'" + sFilter + "'";
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/mean");
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/standard_deviation");
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/coefficient_of_variation");
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/count");
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/minimum");
                AddNumericValue(ref sSQL, xmlTopNode, "./" + sProfileName + "/statistics[@filtering='" + sFilter + "']/maximum");

                sSQL += ");";

                try
                {
                    OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
                    dbCom.ExecuteNonQuery();
                    /*
                    int nProfileID = 0;
                    dbCom = new OleDbCommand("SELECT @@IDENTITY FROM Profiles", dbCon);
                    OleDbDataReader dbRdr = dbCom.ExecuteReader();
                    if (dbRdr.Read())
                    {
                        if (!System.Convert.IsDBNull(dbRdr[0]))
                        {
                            nProfileID = (int)dbRdr[0];
                        }
                    }
                    dbRdr.Close();

                    PopulateTable_ProfileValues(dbCon, xmlTopNode, sProfileName, nProfileID);
                    */

                }
                catch (Exception ex)
                {
                    Exception ex2 = new Exception("Error generating profile", ex);
                    ex2.Data.Add("Visit", nVisitID.ToString());
                    ex2.Data.Add("Profile", sProfileName);
                    throw ex2;
                }
            }


        }

        private void PopulateTable_ProfileValues(OleDbConnection dbCon, XmlNode xmlTopNode, string sProfileName, int nProfileID)
        {
            string sSQL = null;
            sSQL = "INSERT INTO ProfileValues (ProfileID, Distance, ProfileValue) VALUES (";
            sSQL += nProfileID.ToString() + ", @Distance" + ", @ProfileValue" + ")";

            OleDbCommand dbCom = new OleDbCommand(sSQL, dbCon);
            OleDbParameter pDistance = dbCom.Parameters.Add("Distance", OleDbType.Double);
            OleDbParameter pProfileValue = dbCom.Parameters.Add("ProfileValue", OleDbType.Double);

            try
            {
                foreach (XmlNode aNode in xmlTopNode.SelectNodes("./" + sProfileName + "_profile/item"))
                {
                    pDistance.Value = DBNull.Value;
                    string sValue = aNode.SelectSingleNode("interval").InnerText;
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        double fValue = 0;
                        if (double.TryParse(sValue, out fValue))
                        {
                            pDistance.Value = fValue;
                        }
                    }

                    pProfileValue.Value = DBNull.Value;
                    string sProfileValue = aNode.SelectSingleNode("value").InnerText;
                    if (!string.IsNullOrEmpty(sProfileValue))
                    {
                        double fValue = 0;
                        if (double.TryParse(sProfileValue, out fValue))
                        {
                            pProfileValue.Value = fValue;
                        }
                    }

                    dbCom.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating profile values", ex);
                ex2.Data.Add("ProfileID", nProfileID);
                ex2.Data.Add("Profile", sProfileName);
                throw ex2;
            }

        }

        private void GetDoubleValueFromNode(OleDbParameter pParam, XmlNode xmlParent, string xmlNodeName)
        {
            pParam.Value = DBNull.Value;
            XmlNode aNode = xmlParent.SelectSingleNode(xmlNodeName);
            if (aNode is XmlNode)
            {
                string sValue = aNode.InnerText;
                if (!string.IsNullOrEmpty(sValue))
                {
                    double fValue = 0;
                    if (double.TryParse(sValue, out fValue))
                    {
                        if (!(double.IsNegativeInfinity(fValue) || double.IsPositiveInfinity(fValue) || double.IsNaN(fValue)))
                        {
                            pParam.Value = fValue;
                        }
                    }
                }
            }
        }


        private void GetStringValueFromNode(OleDbParameter pParam, XmlNode xmlParent, string xmlNodeName)
        {
            pParam.Value = DBNull.Value;
            XmlNode aNode = xmlParent.SelectSingleNode(xmlNodeName);
            if (aNode is XmlNode)
            {
                string sValue = aNode.InnerText;
                if (!string.IsNullOrEmpty(sValue))
                {
                    pParam.Value = sValue;
                }
            }
        }


        private void GetIntegerValueFromNode(OleDbParameter pParam, XmlNode xmlParent, string xmlNodeName)
        {
            pParam.Value = DBNull.Value;
            XmlNode aNode = xmlParent.SelectSingleNode(xmlNodeName);
            if (aNode is XmlNode)
            {
                string sValue = aNode.InnerText;
                if (!string.IsNullOrEmpty(sValue))
                {
                    int fValue = 0;
                    if (int.TryParse(sValue, out fValue))
                    {
                        pParam.Value = fValue;
                    }
                }
            }
        }


        private void AddStringValue(ref string sSQL, XmlNode xmlTopNode, string xmlTag)
        {
            XmlNode xmlNode = xmlTopNode.SelectSingleNode(xmlTag);
            if (xmlNode is XmlNode)
            {
                string sValue = xmlNode.InnerText;
                if (string.IsNullOrEmpty(sValue))
                {
                    sSQL += ", NULL";
                }
                else
                {
                    sSQL += ", '" + sValue.Replace("'", "''") + "'";
                }
            }
            else
            {
                sSQL += ", NULL";
            }

        }


        private void AddNumericValue(ref string sSQL, XmlNode xmlTopNode, string xmlTag)
        {
            string sValue = "";
            XmlNode xmlNode = xmlTopNode.SelectSingleNode(xmlTag);
            if (xmlNode is XmlNode)
            {
                sValue = xmlNode.InnerText;
            }

            double fValue = 0;
            if (double.TryParse(sValue, out fValue) && !double.IsNaN(fValue))
            {
                sSQL += ", " + fValue.ToString();
            }
            else
            {
                sSQL += ", NULL";
            }
        }

    }
}

CREATE TABLE LookupLists (ListID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) UNIQUE NOT NULL, IsEditableByUser BOOLEAN NOT NULL DEFAULT (1));
CREATE TABLE LookupListItems (ItemID INTEGER PRIMARY KEY NOT NULL, ListID INTEGER REFERENCES LookupLists (ListID) NOT NULL, Title VARCHAR (255) NOT NULL, IsEditableByUser BOOLEAN NOT NULL DEFAULT (1));
CREATE TABLE Metric_Plots (PlotID INTEGER PRIMARY KEY NOT NULL, PlotTitle VARCHAR (1000) UNIQUE NOT NULL, XMetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, YMetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, PlotTypeID INTEGER REFERENCES LookupListItems (ItemID) NOT NULL);
CREATE TABLE User_Queries (QueryID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) NOT NULL UNIQUE, QueryText VARCHAR (1000) NOT NULL, Remarks VARCHAR (1000), CreatedBy VARCHAR (255) NOT NULL, CreatedOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE USGS_Gages (GageID INTEGER PRIMARY KEY NOT NULL, Description VARCHAR (60), Latitude DOUBLE, Longitude DOUBLE, p25 DOUBLE, p50 DOUBLE, p75 DOUBLE, average DOUBLE, stdev DOUBLE, huc INTEGER, huc_region INTEGER);
CREATE TABLE USGS_Discharges (DischargeID INTEGER PRIMARY KEY NOT NULL, GageID INTEGER REFERENCES USGS_Gages (GageID) ON DELETE CASCADE NOT NULL, TheDate DATETIME NOT NULL, Discharge DOUBLE);
CREATE TABLE VersionInfo ("Key" VARCHAR (255) NOT NULL, ValueInfo VARCHAR (255) NOT NULL);
CREATE TABLE Model_Batches (ID INTEGER PRIMARY KEY NOT NULL, BatchName VARCHAR (255) NOT NULL, CreatedOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE VersionChangeLog (ChangeLogID INTEGER PRIMARY KEY NOT NULL, DateCreated DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP), Version INTEGER NOT NULL, Description VARCHAR (1000) NOT NULL);
CREATE TABLE CHaMP_Watersheds (WatershedID INTEGER PRIMARY KEY NOT NULL, WatershedName VARCHAR (50) UNIQUE NOT NULL);
CREATE TABLE LogFeedback (LogID INTEGER PRIMARY KEY NOT NULL, UserName VARCHAR (50) NOT NULL, QualityRatingID INTEGER REFERENCES LookupListItems (ItemID) NOT NULL, ItemReviewed VARCHAR (255) NOT NULL, WatershedID INTEGER REFERENCES CHaMP_Watersheds (WatershedID) ON DELETE CASCADE, SiteID INTEGER REFERENCES CHaMP_Sites (SiteID) ON DELETE CASCADE, VisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE, Description VARCHAR (1000), ReviewedOn DATETIME NOT NULL, AddedOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE Model_BatchRuns (ID INTEGER PRIMARY KEY NOT NULL, BatchID INTEGER REFERENCES Model_Batches (ID) ON DELETE CASCADE, ModelTypeID INTEGER REFERENCES LookupListItems (ItemID), PrimaryVisitID INTEGER REFERENCES CHaMP_Visits ON DELETE CASCADE, Run BOOLEAN NOT NULL DEFAULT (1), Summary VARCHAR (255), InputFile VARCHAR (255) NOT NULL, DateTimeStarted DATETIME, DateTimeCompleted DATETIME, Priority INTEGER);
CREATE TABLE LogMessages (LogMessageID INTEGER PRIMARY KEY NOT NULL, LogID INTEGER REFERENCES LogFiles (LogID) ON DELETE CASCADE, TargetVisitID INTEGER REFERENCES CHaMP_Visits ON DELETE CASCADE, SourceVisitID INTEGER REFERENCES CHaMP_Visits ON DELETE CASCADE, MessageType VARCHAR (20), LogSeverity VARCHAR (20), LogDateTime DATETIME, LogMessage VARCHAR (1000), LogException VARCHAR (1000), LogSolution VARCHAR (1000));
CREATE TABLE Metric_Habitat (ResultID INTEGER REFERENCES Metric_Results ON DELETE CASCADE NOT NULL, ModelID INTEGER REFERENCES LookupListItems (ItemID) NOT NULL, LifeStageID INTEGER REFERENCES LookupListItems (ItemID) ON DELETE CASCADE NOT NULL, MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, MetricValue DOUBLE);
CREATE TABLE Metric_BudgetSegregations (BudgetID INTEGER PRIMARY KEY NOT NULL, ChangeDetectionID INTEGER REFERENCES Metric_ChangeDetection ON DELETE CASCADE NOT NULL, Mask VARCHAR (255));
CREATE TABLE Metric_BudgetSegregationValues (BudgetSegregationClass INTEGER PRIMARY KEY NOT NULL, BudgetID INTEGER REFERENCES Metric_BudgetSegregations (BudgetID) ON DELETE CASCADE, MaskValueName VARCHAR (255), RawAreaErosion DOUBLE
, RawAreaDeposition DOUBLE
, ThresholdAreaErosion DOUBLE
, ThresholdAreaDeposition DOUBLE
, RawVolumeErosion DOUBLE
, RawVolumeDeposition DOUBLE
, ThresholdVolumeErosion DOUBLE
, ThresholdVolumeDeposition DOUBLE
, RawVolumeDifference DOUBLE
, ThresholdPercentErosion DOUBLE
, ThresholdPercentDeposition DOUBLE
, ErrorVolumeErosion DOUBLE
, ErrorVolumeDeposition DOUBLE
, ErrorVolumeDifference DOUBLE
, AreaDetectableChange DOUBLE
, AreaOfInterestRaw DOUBLE
, PercentAreaOfInterestDetectableChange DOUBLE
, ThresholdedVolumeDifference DOUBLE
, VolumeDifferencePercent DOUBLE
, AverageDepthErosionRaw DOUBLE
, AverageDepthErosionThreshold DOUBLE
, AverageDepthErosionError DOUBLE
, AverageDepthErosionPercent DOUBLE
, AverageDepthDepositionRaw DOUBLE
, AverageDepthDepositionThreshold DOUBLE
, AverageDepthDepositionError DOUBLE
, AverageDepthDepositionPercent DOUBLE
, AverageThicknessDifferenceAOIRaw DOUBLE
, AverageThicknessDifferenceAOIThresholded DOUBLE
, AverageThicknessDifferenceAOIError DOUBLE
, AverageThicknessDifferenceAOIPercent DOUBLE
, AverageNetThicknessDifferenceAOIRaw DOUBLE
, AverageNetThicknessDifferenceAOIThresholded DOUBLE
, AverageNetThicknessDifferenceAOIError DOUBLE
, AverageNetThicknessDifferenceAOIPercent DOUBLE
, AverageNetThicknessDifferenceADCError DOUBLE
, AverageNetThicknessDifferenceADCThresholded DOUBLE
, AverageNetThicknessDifferenceADCPercent DOUBLE
, AverageThicknessDifferenceADCError DOUBLE
, AverageThicknessDifferenceADCThresholded DOUBLE
, AverageThicknessDifferenceADCPercent DOUBLE
, PercentErosionRaw DOUBLE
, PercentErosionThresholded DOUBLE
, PercentDepositionRaw DOUBLE
, PercentDepositionThresholded DOUBLE
, PercentImbalanceRaw DOUBLE
, PercentImbalanceThresholded DOUBLE
, PercentNetVolumeRatioRaw DOUBLE
, PercentNetVolumeRatioThresholded DOUBLE);
CREATE TABLE User_Selections (SelectionID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) NOT NULL, Remarks VARCHAR (1000), AddedOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE User_SelectionVisits (ID INTEGER PRIMARY KEY NOT NULL, SelectionID INTEGER NOT NULL REFERENCES User_Selections (SelectionID) ON DELETE CASCADE, VisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE NOT NULL);
CREATE TABLE LookupPrograms (ProgramID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) NOT NULL UNIQUE, WebSiteURL VARCHAR (255), FTPURL VARCHAR (255), AWSBucket VARCHAR (255), API VARCHAR (255), Remarks VARCHAR (255));
CREATE TABLE LogFiles (LogID INTEGER PRIMARY KEY NOT NULL, ResultID INTEGER REFERENCES Metric_Results (ResultID) ON DELETE CASCADE, Status VARCHAR (255), LogFilePath VARCHAR (255), VisitID INTEGER REFERENCES CHaMP_Visits (VisitID), DateRun DATETIME, ModelVersion VARCHAR (10), DateScavenged DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP), ResultFilePath VARCHAR (255), MetaDataInfo VARCHAR (1000), BatchRunID INTEGER);
CREATE TABLE CHaMP_Visits (VisitID INTEGER PRIMARY KEY NOT NULL, SiteID INTEGER REFERENCES CHaMP_Sites ON DELETE CASCADE NOT NULL, VisitYear INTEGER NOT NULL, HitchName VARCHAR (255), Organization VARCHAR (255), CrewName VARCHAR (255), SampleDate DATETIME, ProtocolID INTEGER, ProgramID INTEGER REFERENCES LookupPrograms (ProgramID) NOT NULL, IsPrimary BOOLEAN NOT NULL DEFAULT (0), QCVisit BOOLEAN NOT NULL DEFAULT (0), PanelName VARCHAR (64), CategoryName VARCHAR (100), VisitPhase VARCHAR (100), VisitStatus VARCHAR (100), HasStreamTempLogger BOOLEAN NOT NULL DEFAULT (0), HasFishData BOOLEAN NOT NULL DEFAULT (0), Discharge DOUBLE, D84 DOUBLE, Remarks VARCHAR (255));
CREATE TABLE CHaMP_ChannelUnits (ID INTEGER PRIMARY KEY NOT NULL, VisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE NOT NULL, SegmentNumber INTEGER DEFAULT (1) NOT NULL, ChannelUnitNumber INTEGER NOT NULL, Tier1 VARCHAR (255), Tier2 VARCHAR (255), BouldersGT256 INTEGER, Cobbles65255 INTEGER, CoarseGravel1764 INTEGER, FineGravel316 INTEGER, Sand0062 INTEGER, FinesLT006 INTEGER, SumSubstrateCover INTEGER, Bedrock INTEGER, LargeWoodCount INTEGER NOT NULL DEFAULT (0));
CREATE TABLE LogGCDReview (ID INTEGER PRIMARY KEY NOT NULL, NewVisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE NOT NULL, OldVisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE NOT NULL, MaskValueName VARCHAR (255), FlagReason VARCHAR (255), ValidResults INTEGER NOT NULL DEFAULT (0), ErrorType VARCHAR (55), ErrorDEM VARCHAR (55), Comments VARCHAR (1000), EnteredBy VARCHAR (30), DateModified DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP), Processed INTEGER NOT NULL DEFAULT (0));
CREATE TABLE Metric_ChangeDetection (ChangeDetectionID INTEGER PRIMARY KEY NOT NULL, ResultID INTEGER REFERENCES Metric_Results ON DELETE CASCADE NOT NULL, NewVisit VARCHAR (255), NewFieldSeason INTEGER, NewVisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE, OldVisit VARCHAR (255), OldFieldSeason INTEGER, OldVisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE, Epoch VARCHAR (255), ThresholdType VARCHAR (255), Threshold DOUBLE, SpatialCoherence VARCHAR (255));
CREATE TABLE CHaMP_Sites (SiteID INTEGER UNIQUE NOT NULL PRIMARY KEY, WatershedID INTEGER REFERENCES CHaMP_Watersheds (WatershedID) ON DELETE CASCADE NOT NULL, SiteName VARCHAR (255) NOT NULL, StreamName VARCHAR (255), UTMZone CHAR (4), UC_Chin BOOLEAN NOT NULL DEFAULT (0), SN_Chin BOOLEAN NOT NULL DEFAULT (0), LC_Steel BOOLEAN NOT NULL DEFAULT (0), MC_Steel BOOLEAN NOT NULL DEFAULT (0), UC_Steel BOOLEAN NOT NULL DEFAULT (0), SN_Steel BOOLEAN NOT NULL DEFAULT (0), Latitude DOUBLE, Longitude DOUBLE, GageID INTEGER);
CREATE TABLE Metric_Definition_Programs (MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, ProgramID INTEGER REFERENCES LookupPrograms (ProgramID) NOT NULL);
CREATE TABLE Metric_Schema_Definitions (SchemaID INTEGER REFERENCES Metric_Schemas (SchemaID) ON DELETE CASCADE NOT NULL, MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL);
CREATE TABLE Metric_Schemas (SchemaID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) NOT NULL UNIQUE, ProgramID INTEGER REFERENCES LookupPrograms (ProgramID) NOT NULL, RootXPath VARCHAR (255), DatabaseTable VARCHAR (50), AddedOn DATETIME DEFAULT (CURRENT_TIMESTAMP) NOT NULL, UpdatedOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE Metric_TierMetrics (InstanceID INTEGER REFERENCES Metric_Instances (InstanceID) ON DELETE CASCADE NOT NULL, MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, TierID INTEGER REFERENCES LookupListItems (ItemID) ON DELETE NO ACTION NOT NULL, MetricValue DOUBLE, CONSTRAINT PK_Metric_TierMetrics2 PRIMARY KEY (InstanceID, MetricID, TierID)) WITHOUT ROWID;
CREATE TABLE Metric_VisitMetrics (InstanceID INTEGER REFERENCES Metric_Instances (InstanceID) ON DELETE CASCADE NOT NULL, MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, MetricValue DOUBLE, PRIMARY KEY (InstanceID, MetricID)) WITHOUT ROWID;
CREATE TABLE Metric_ChannelUnitMetrics (InstanceID INTEGER REFERENCES Metric_Instances (InstanceID) ON DELETE CASCADE NOT NULL, ChannelUnitNumber INTEGER NOT NULL, MetricID INTEGER REFERENCES Metric_Definitions (MetricID) ON DELETE CASCADE NOT NULL, MetricValue DOUBLE, CONSTRAINT PK_Metric_ChannelMetricUnitMetrics2 PRIMARY KEY (InstanceID, ChannelUnitNumber, MetricID)) WITHOUT ROWID;
CREATE TABLE Metric_Batches (BatchID INTEGER PRIMARY KEY, SchemaID INTEGER REFERENCES Metric_Schemas (SchemaID) ON DELETE CASCADE NOT NULL, ScavengeTypeID INTEGER REFERENCES LookupListItems (ItemID) NOT NULL, Title VARCHAR (255), Remarks VARCHAR (1000), WorkbenchInsertionOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE TABLE Metric_Definitions (MetricID INTEGER PRIMARY KEY NOT NULL, Title VARCHAR (255) NOT NULL, CMMetricID INTEGER, ModelID INTEGER REFERENCES LookupListItems (ItemID), GCDTypeID INTEGER REFERENCES LookupListItems (ItemID), XPath VARCHAR (255), Threshold DOUBLE DEFAULT (0.05), MinValue DOUBLE, MaxValue DOUBLE, IsActive BOOLEAN NOT NULL DEFAULT (1), DisplayNameShort VARCHAR (50), Precision INTEGER, DataTypeID INTEGER NOT NULL, DisplayOrderID INTEGER, IsDisplayable BOOLEAN, MetricGroupID INTEGER REFERENCES LookupListItems (ItemID), MetricChannelGroupID INTEGER REFERENCES LookupListItems (ItemID), Description VARCHAR (1000), MMLink VARCHAR (255), AltLink VARCHAR (255), YearIntroduced INTEGER NOT NULL, VersionChanged VARCHAR (10), VersionAdded VARCHAR (10), Calculation VARCHAR (1000), ResultXMLTag VARCHAR (255), AddedOn DATETIME DEFAULT (CURRENT_TIMESTAMP) NOT NULL, UpdatedOn DATETIME DEFAULT (CURRENT_TIMESTAMP) NOT NULL);
CREATE TABLE Metric_Instances (InstanceID INTEGER PRIMARY KEY NOT NULL, BatchID INTEGER REFERENCES Metric_Batches (BatchID) ON DELETE CASCADE NOT NULL, VisitID INTEGER REFERENCES CHaMP_Visits (VisitID) ON DELETE CASCADE NOT NULL, ModelVersion VARCHAR (10), MetricsCalculatedOn DATETIME, APIInsertionOn DATETIME, WorkbenchInsertionOn DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP));
CREATE INDEX FX_LookupListItems_ListID ON LookupListItems (ListID);
CREATE UNIQUE INDEX UX_LookupListItems_Title ON LookupListItems (ListID, Title);
CREATE INDEX FX_Metric_Plots_XMetricID ON Metric_Plots (XMetricID);
CREATE INDEX FX_Metric_Plots_YMetricID ON Metric_Plots (YMetricID);
CREATE INDEX FX_Metric_Plots_PlotTypeID ON Metric_Plots (PlotTypeID);
CREATE INDEX FX_USGS_Discharges_GageID ON USGS_Discharges (GageID);
CREATE INDEX IX_USGS_Discharges_Date ON USGS_Discharges (TheDate ASC);
CREATE INDEX FX_LogFeedback_QualityRating ON LogFeedback (QualityRatingID ASC);
CREATE INDEX FX_LogFeedback_WatershedID ON LogFeedback (WatershedID ASC);
CREATE INDEX FX_LogFeedback_SiteID ON LogFeedback (SiteID ASC);
CREATE INDEX FX_LogFeedback_VisitID ON LogFeedback (VisitID ASC);
CREATE INDEX IX_LogFeedback_ReviewedOn ON LogFeedback (ReviewedOn DESC);
CREATE INDEX FX_ModelBatchRuns_BatchID ON Model_BatchRuns (BatchID);
CREATE INDEX FX_ModelBatchRuns_ModelTypeID ON Model_BatchRuns (ModelTypeID);
CREATE INDEX FX_ModelBatchRuns_PrimaryVisitID ON Model_BatchRuns (PrimaryVisitID);
CREATE INDEX FX_ModelBatchRuns_Priority ON Model_BatchRuns (BatchID, Priority);
CREATE INDEX FX_LogMessages_LogID ON LogMessages (LogID ASC);
CREATE INDEX FX_LogMessages_TargetVisitID ON LogMessages (TargetVisitID DESC);
CREATE INDEX FX_LogMessages_SourceVisitID ON LogMessages (SourceVisitID DESC);
CREATE INDEX IX_LogMessages_MessageType ON LogMessages (MessageType ASC);
CREATE INDEX IX_LogMessages_LogSeverity ON LogMessages (LogSeverity ASC);
CREATE INDEX IX_LogMessages_LogDateTime ON LogMessages (LogDateTime ASC);
CREATE UNIQUE INDEX UX_Metric_Habitat_Metric ON Metric_Habitat (ResultID, ModelID, LifeStageID, MetricID);
CREATE INDEX FX_Metric_BudgetSegregations ON Metric_BudgetSegregations (ChangeDetectionID);
CREATE INDEX FX_Metric_BudgetSegregationValues_BudgetID ON Metric_BudgetSegregationValues (BudgetID);
CREATE INDEX FX_LogFiles_ResultID ON LogFiles (ResultID DESC);
CREATE INDEX IX_LogFiles_Status ON LogFiles (Status ASC);
CREATE INDEX FX_LogFiles_VisitID ON LogFiles (VisitID DESC);
CREATE INDEX IX_LogFiles_DateRun ON LogFiles (DateRun DESC);
CREATE INDEX IX_LogFiles_ModelVersion ON LogFiles (ModelVersion ASC);
CREATE INDEX FX_LogFiles_BatchRunID ON LogFiles (BatchRunID ASC);
CREATE INDEX IX_CHaMP_Visits_VisitYear ON CHaMP_Visits (VisitYear DESC);
CREATE INDEX FX_CHaMP_Visits_SiteID ON CHaMP_Visits (SiteID DESC);
CREATE INDEX IX_CHaMP_Visits_Organization ON CHaMP_Visits (Organization ASC);
CREATE INDEX FX_CHaMP_Visits_ProtocolID ON CHaMP_Visits (ProtocolID ASC);
CREATE INDEX IX_CHaMP_Visits_PanelName ON CHaMP_Visits (PanelName);
CREATE INDEX IX_CHaMP_Visits_CategoryName ON CHaMP_Visits (CategoryName ASC);
CREATE INDEX IX_CHaMP_Visits_VisitPhase ON CHaMP_Visits (VisitPhase ASC);
CREATE INDEX IX_CHaMP_Visits_VisitStatus ON CHaMP_Visits (VisitStatus ASC);
CREATE INDEX IX_CHaMP_ChannelUnits_Segment ON CHaMP_ChannelUnits (SegmentNumber ASC);
CREATE UNIQUE INDEX IX_CHaMP_ChannelUnits_CUN ON CHaMP_ChannelUnits (ChannelUnitNumber ASC, VisitID);
CREATE INDEX FX_CHaMP_ChannelUnits_VisitID ON CHaMP_ChannelUnits (VisitID ASC);
CREATE INDEX FX_LogGCDReview_NewVisitID ON LogGCDReview (NewVisitID ASC);
CREATE INDEX FX_LogGCDReview_OldVisitID ON LogGCDReview (OldVisitID ASC);
CREATE UNIQUE INDEX UX_LogGCDReview_GCDResult ON LogGCDReview (NewVisitID, OldVisitID, MaskValueName);
CREATE INDEX FX_Metric_ChangeDetection_ResultID ON Metric_ChangeDetection (ResultID);
CREATE INDEX FX_Metric_ChangeDetection_NewVisitID ON Metric_ChangeDetection (NewVisitID);
CREATE INDEX FX_Metric_ChangeDetection_OldVisitID ON Metric_ChangeDetection (OldVisitID);
CREATE INDEX FX_CHaMP_Sites_WatershedID ON CHaMP_Sites (WatershedID ASC);
CREATE INDEX IX_CHaMP_Sites_SiteID ON CHaMP_Sites (SiteName ASC);
CREATE INDEX IX_CHaMP_Sites_StreamName ON CHaMP_Sites (StreamName);
CREATE UNIQUE INDEX UX_Metric_Definition_Programs_Metric ON Metric_Definition_Programs (MetricID, ProgramID);
CREATE UNIQUE INDEX UX_Metric_Schema_Definitions ON Metric_Schema_Definitions (SchemaID, MetricID);
CREATE UNIQUE INDEX UX_Metric_Definitions_CMMetricID ON Metric_Definitions (CMMetricID);
CREATE INDEX FX_Metric_Definitions_ModelID ON Metric_Definitions (ModelID);
CREATE INDEX FX_Metric_Definitions_GCDTypeID ON Metric_Definitions (GCDTypeID);
CREATE UNIQUE INDEX UX_Metric_Definitions_RBTResultXMLTag ON Metric_Definitions (ResultXMLTag);
CREATE INDEX IX_Metric_Definitions_DisplayOrderID ON Metric_Definitions (DisplayOrderID);
CREATE INDEX IX_Metric_Definitions_MetricGroupID ON Metric_Definitions (MetricGroupID);
CREATE INDEX IX_Metric_Definitions_MetricChannelGroupID ON Metric_Definitions (MetricChannelGroupID);
CREATE VIEW vwVisits AS SELECT
    V.*
    , W.WatershedID AS WatershedID
    , W.WatershedName
    , S.SiteName
    , S.UTMZone
    , P.Title AS ProtocolName
FROM CHaMP_Visits V
    INNER JOIN CHaMP_Sites S ON V.SiteID = S.SiteID
    INNER JOIN CHaMP_Watersheds W ON S.WatershedID = W.WatershedID
    LEFT JOIN LookupListItems P ON V.ProtocolID = P.ItemID;
CREATE VIEW vwMainVisitList AS SELECT W.WatershedID AS WatershedID, WatershedName, V.VisitID AS VisitID, VisitYear, SampleDate, HitchName, CrewName, PanelName, S.SiteID AS SiteID, SiteName, StreamName, Organization, QCVisit, CategoryName, VisitPhase, VisitStatus, P.ProgramID, P.Title AS ProgramName, HasStreamTempLogger, HasFishData, IsPrimary, Count(C.VisitID) AS ChannelUnits FROM CHAMP_Watersheds AS W INNER JOIN CHaMP_Sites AS S ON W.WatershedID = S.WatershedID INNER JOIN CHaMP_Visits AS V ON S.SiteID = V.SiteID INNER JOIN LookupPrograms AS P ON V.ProgramID = P.ProgramID LEFT JOIN CHaMP_ChannelUnits AS C ON V.VisitID = C.VisitID GROUP BY W.WatershedID, WatershedName, V.VisitID, VisitYear, SampleDate, HitchName, CrewName, PanelName, S.SiteID, SiteName, StreamName, Organization, QCVisit, CategoryName, VisitPhase, VisitStatus, NULL, HasStreamTempLogger, HasFishData, IsPrimary, ProgramName, P.ProgramID;
CREATE TRIGGER Metric_Schemas_Updated AFTER UPDATE OF Title, RootXPath ON Metric_Schemas BEGIN UPDATE Metric_Schemas SET UpdatedOn = CURRENT_TIMESTAMP WHERE SchemaID = NEW.SchemaID; END;
CREATE VIEW vwMetricDefinitions AS SELECT
    MetricID
    , MD.Title
    , DisplayNameShort
    , ModelID
    , M.Title AS ModelName
    , XPath, MD.IsActive
    , MD.DataTypeID
    , D.Title AS DataTypeName
    , Precision
    , Threshold
    , MinValue
    , MaxValue
    , MMLink
    , AltLink
    , MD.AddedOn
    , MD.UpdatedOn
    , PG.Title AS MetricParentGroup
    , CG.Title AS MetricChildGroup
    , CMMetricID
FROM Metric_Definitions MD
    INNER JOIN LookupListItems M ON MD.ModelID = M.ItemID
    INNER JOIN LookupListItems D ON MD.DataTypeID = D.ItemID
    LEFT JOIN LookupListItems PG ON MD.MetricGroupID = PG.ItemID
    LEFT JOIN LookupListItems CG ON MD.MetricChannelGroupID = CG.ItemID;
CREATE TRIGGER Metric_Definitions_OnUpdated AFTER UPDATE OF Title, CMMetricID, ModelID, GCDTypeID, XPath, Threshold, MinValue, MaxValue, IsActive, DisplayNameShort, Precision, DataTypeID, DisplayOrderID, IsDisplayable, MetricGroupID, MetricChannelGroupID, Description, MMLink, YearIntroduced, VersionChanged, VersionAdded, Calculation, ResultXMLTag ON Metric_Definitions BEGIN UPDATE Metric_Definitions SET UpdatedOn = CURRENT_TIMESTAMP WHERE MetricID = NEW.MetricID; END;
CREATE VIEW vwActiveVisitMetrics AS SELECT D.MetricID, D.Title, D.DisplayNameShort, P.ProgramID, R.ResultID, ModelVersion, MetricValue, S.WatershedID, S.SiteID, V.VisitID, V.VisitYear FROM Metric_Definitions D INNER JOIN Metric_Definition_Programs P ON D.MetricID = P.MetricID INNER JOIN Metric_VisitMetrics VM ON D.MetricID = VM.MetricID INNER JOIN Metric_Results R ON R.ResultID = VM.ResultID INNER JOIN CHaMP_Visits V ON V.VisitID = R.VisitID INNER JOIN CHaMP_Sites S ON S.SiteID = V.SiteID WHERE (IsActive <> 0) AND (IsDisplayable <> 0) AND (NULL = 3);

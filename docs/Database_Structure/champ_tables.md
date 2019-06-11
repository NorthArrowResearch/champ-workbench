---
title: CHaMP Tables
---

[Workbench database](database_structure.html) tables that store information from [CHaMP monitoring](https://www.champmonitoring.org) possess the prefix `CHAMP_`. The following list describes the key fields. There are more fields in the actual database tables, some of which are out of date and no longer used by the Workbench.

* `CHaMP_Watersheds` - List of all CHaMP watersheds.
    * Watershed ID (*this is a workbench ID and unrelated to CHaMP*)
    * Name
* `CHaMP_Sites` - List of all CHaMP sites.
    * Watershed ID
    * Site ID (*this is a workbench ID and unrelated to CHaMP*)
    * SiteName - the official CHaMP site name
* `CHaMP_Visits` - List of all CHaMP visits.
    * VisitID - this is the official CHaMP visit ID
    * SiteID
    * VisitYear - field season in which the visit occurred
    * HitchName - CHaMP hitch name
    * CrewName - CHaMP crew name
    * SampleDate - Date on which te visit was sampled
    * IsPrimary - True if this is visit is by the organization that manages the watershed.
* `CHaMP_Segments` - list of channel segments within a visit
    * Segment ID (*this is a workbench ID and unrelated to CHaMP*)
    * VisitID
    * SegmentNumber - CHaMP segment number
    * SegmentName - CHaMP segment name
* `CHaMP_ChannelUnits` - list of all channel units within each channel segment
    * ID (*this is a workbench ID and unrelated to CHaMP*)
    * Segment ID
    * ChannelUnitNumber - CHaMP channel unit number
    * Tier 1 - channel unit tier 1 designation    
    * Tier 2 - channel unit tier 2 designation
    * *Grain Size fields*
* `CHaMP_Measurements` - field measurements collected by crews during visits
    * MeasurementID (*this is a workbench ID and unrelated to CHaMP*)
    * VisitID - the visit to which the measurement pertains.
    * MeasurementTypeID - the type of measurement. This is a lookup to the `LookupListItems` table (ListID 19).
    * Value - the measurement data stored in JSON format.

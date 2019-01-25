---
title: Download Metrics
---

The CHaMP Workbench downloads metrics from [champmonitoring.org](https://champmonitoring.org) directly into the Workbench database via the API.

## Process:  

1. Open the CHaMP Workbench.
1. Make sure that you are connected to the correct local Workbench database.
1. Select/highlight the visits for metric download. 
1. Click the Data main menu item and choose Download Metrics. ![Download Metrics]({{ site.url }}/assets/images/DownloadMetricMenu.png)
1. Select the metric datasets (aka 'schemas') to download from the list. Note that downloading large batches of metrics from multiple watersheds, years and schemas may trigger a timeout error on access to the API. If this occurs, download smaller batches of metrics. ![Download Metrics]({{ site.url }}/assets/images/MetricDownload_SchemaList.png)

Active copies of Draft Metrics are available in the QA schemas (below).  Note that ONLY ONE copy of metrics from these schemas is available in the Workbench at any time. **Any metric download will replace existing data in these schemas**.

* QA - Aux Channel Metrics (CHaMP)  
* QA - Aux Tier 1 Metrics (CHaMP)  
* QA - Aux Visit Metrics (CHaMP)  
* QA - Topo Channel Metrics (CHaMP)  
* QA - Topo Tier 1 Metrics (CHaMP)  
* QA - Topo Tier 2 Metrics (CHaMP)  
* QA - Topo Visit Metrics (CHaMP)  
* QA - TopoAux Tier 1 Metrics (CHaMP)  
* QA - TopoAux Visit Metrics (CHaMP)  

Once metrics have been reviewed by crews after each field season, a snapshot of the metrics are moved to the Final schemas. These schemas are updated periodically and are tracked by date.

* Final - Visit Metrics  
* Final - Tier 1 Metrics  
* Final - Tier 2 Metrics  
* Final - Channel Metrics  

After Schemas have been selected for download, enter your credentials for CHaMP Monitoring and click OK.
Contact [Sitka Technologies](http://www.sitkatech.com/) if you receive a message stating that you are unable to authenticate.



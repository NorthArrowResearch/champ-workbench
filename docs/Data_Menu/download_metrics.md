---
title: Download Metrics
---

The CHaMP Workbench downloads metrics from champmonitoring.org directly into the Workbench database via the API.

Process:
Open the CHaMP Workbench.
Make sure that you are connected to the correct local Workbench database.
Select/Highlight the Visits for metric download
Click the Data main menu item and choose Download Metrics


Select the metric datasets (aka 'schemas') to download from the list. Note that downloading large batches of metrics from multiple watersheds, years and schemas may trigger a timeout error on access to the API.  If this occurs, download smaller batches of metrics. 



Enter your credentials for CHaMP Monitoring and click OK.
Contact Carol Volk (carol@shouthforkresearch.org) if you receive a message stating that you are unable to authenticate.

![Synchronize CHaMP Data]({{ site.url }}/assets/images/sync/sync.png)




Note that the Action and Effective Montiroing (AEM) program does not use distinguish visits by watershed. You can leave all watersheds unchecked or select the Basinwide item when synchronizing AEM data.

---
title: Synchronize CHaMP Visit Info
---

The CHaMP Workbench maintains lists of watersheds, sites, visits and channel units that it uses to then perform various operations. This local copy of these items can get out of date and require updating from time to time. This is especially true during the summer field season when visit information changes as crews return from the field and upload data onto [CHaMP Monitoring](https://www.champmonitoring.org).

The Workbench contains a feature that updates the lists stored in the Workbench database by connecting directly to [CHaMP Monitoring](https://www.champmonitoring.org) and retrieving the relevant information over the internet. This feature currently **only** synchronizes the lists of watersheds, sites, visits and channel units. It does **not** retrieve auxilliary measurements or metrics!

## Synchronizing CHaMP Data

1. Open the CHaMP Workbench.
2. Make sure that you are connected to the correct local Workbench database.
3. click the `Data` main menu item and choose `Synchronize CHaMP Data...`.
4. Select the relevant program(s) and watersheds and then click `Synchronize`. Note that the first time you run this tool with a new Workbench database you will only have programs. There will be no watersheds to choose from and all watersheds will be synchronized for the selected program(s).
5. Enter your credentials for [CHaMP Monitoring](https://www.champmonitoring.org) and click `OK`.

Contact Sitka Technologies, the organization that maintains CHaMP Monitoring, if you receive a message stating that you are unable to authenticate.

![Synchronize CHaMP Data]({{ site.url }}/assets/images/sync/sync.png)

## What Data Are Included?

The following attributes are retrieved when the synchronization is run:

* **Watersheds**
	* Watershed name
* **Sites**
	* Site name
	* Watershed name
	* Stream name
	* UTMzZone
	* Latitude
	* Longitude
* **Visits**
	* Visit ID
	* site name
	* Visit year
	* Hitch name
	* Organization
	* Crew name
	* Sample date
	* Protocol
	* Program
	* Is primary
	* Panel
	* Visit status
	* Has stream temp logger
	* Discharge

## Notes

* This process can be slow, especially if you choose multiple watersheds.
* Leaving all watersheds unchecked is the same as selecting *all* watersheds.
* Right click on the watersheds list to access a shortcut menu to select all or none watersheds.
* Should anything go wrong with the process the local Workbench database will be reverted to the state that it was in before the synchronization tool was run. i.e. If any visit fails anywhere in the process then all visit data is rejected and the process aborted.
* Note that the [Action and Effective Montiroing](https://www.aemonitoring.org) (AEM) program does not use distinguish visits by watershed. You can leave all watersheds unchecked or select the `Basinwide` item when synchronizing AEM data.

---
title: Synchronize CHaMP Data
---

The CHaMP Workbench maintains lists of watersheds, sites, visits and channel units that it uses to then perform various operations. This local copy of these items can get out of date and require updating from time to time. This is especially true during the summar field season when visit information changes as crews return from the field and upload data onto [CHaMP Monitoring](http://champmonitoring.org).

New for 2017, the Workbench contains a feature that update the lists stored in the Workbench database by connecting to the central CHaMP database directly over the internet and retrieving the relevant information.

## Synchronizing CHaMP Data

1. Open the CHaMP Workbench.
1. Make sure that your are connected to the correct local Workbench database
1. click the `Data` main menu item and choose `Synchronize CHaMP Data...`.
1. Select the relevant program(s) and watersheds and then click `Synchronize`.
1. Enter your credentials for [CHaMP Monitoring](http://champmonitoring.org).

![Synchronize CHaMP Data](/assets/images/sync/sync.png)

## Notes

* This process can be slow, especially if you choose multiple watersheds.
* Leaving all watersheds unchecked is the same as selecting *all* watersheds.
* Right click on the watersheds list to access a shortcut menu to select all or none watersheds.
* Should anything go wrong with the process the local Workbench database will be reverted to the state that it was in before the synchronization tool was run. i.e. If any visit fails anywhere in the process then all visit data is rejected and the process aborted.
* Note that the [Action and Effective Montiroing](http://aemonitoring.org) (AEM) program does not use distinguish visits by watershed. You can leave all watersheds unchecked or selected the `Basinwide` item when synchronizing AEM data.


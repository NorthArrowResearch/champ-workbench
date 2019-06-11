---
title: Measurements
---

The CHaMP Workbench can store the measurements collected by field crews. There are no automated processes or tools for downloading or manipulating these data. The Workbench merely has a single table, `CHaMP_Measurements`, for storing the raw values. Users can either download the measurements themselves and put the data in this table, or [download a copy]({{ site.baseurl}}/download.html) of the workbench database that already has the data downloaded and ready for use.

The `CHaMP_Measurements` table has the following columns:

* MeasurementID - a unique identifier for each instance of each measurement type.
* VisitID - the visit to which the measurement pertains.
* MeasurementTypeID - the type of measurement. This is a lookup to the `LookupListItems` table (ListID 19).
* Value - the measurement data stored in JSON format.

The CHaMP measurement data stored in the workbench database are intended to be accessed via a scripting language such as Python or R. Users can write queries to retrieve the data that they need and either parse the JSON values to extract the measurement data, or write the JSON data to text file for consumption by other tools.

Note that the structure of the data for each measurement type sometimes varied over time as the CHaMP protocol evolved. This is why the data are stored as a individual JSON text strings rather than a complex series of database tables that cover the myriad data structure over time. This storage is more flexible and allows measurements of different types to be stored in a single table, although it is space inefficient which is why the [database with measurements for download]({{ site.baseurl}}/download.html) is so large.

## Sample Script

Below shows a simple script to export all temperature measurements for the 2015 field season in the Asotin watershed and write them to JSON text files:

```python


```
---
title: Working With SQLite Databases
---

In 2017 the CHaMP Workbench was updated to use [SQLite](https://www.sqlite.org/) as the underlying database instead of Microsoft Access. There are several options available should you want to interact directly with the Workbench database directly (writing custom SQL queries etc):

## SQLite

<div class="float-right">
<a href="https://sqlitestudio.pl/ss/full_49.png"><img src="https://sqlitestudio.pl/ss/mini_49.png" alt="sqlite"></a>
</div>
Although SQLite is natively a command line application with no user interface there are several user-friendly products available that provide rich user interfaces. [SQLite Studio](https://sqlitestudio.pl) is one such free user interface that possesses an excellent database browser and query functionality.

SQLite Studio does not need to be installed. You simply download the software, extract the zip archive onto your system and then run it. It works on both MacOS and Windows.

## Microsoft Access

With a little effort, you can use Microsoft Access as the user interface to view and edit data in a SQLite database. To do this you have to set up an ODBC file source to the SQLite Workbench database and then add linked tablesin an Access database  to the SQLite  workbench database. Essentially Access becomes the user interface through which you interact with your data, but the actual data itself are stored in SQLite.

The following instructions describe the steps:

1) Download and install the 64 bit version of the [SQLite ODBC Driver](http://www.ch-werner.de/sqliteodbc/sqliteodbc_w64.exe).

2) Open ODBC. This is a built-in piece of Windows software that allows you to connect to virtually any database format. The easiest way to open ODBC is to press the Windows key and then type "odbc". Windows comes with both a 63 and 64 version and make sure that you select the **64 bit version of ODBC**! 

![odbc](/assets/images/sqlite/odbc.png)

3) Make sure that you are on the "User DSN" tab and then click the `Add...` button.

4) Scroll down and choose the `SQLite3 ODBC Driver`.

![ODBC Type](/assets/images/sqlite/odbc_type.png)

5) Provide a meaningful name for this data source, something like "CHaMP Workbench" and then browse to the workbench SQLite database. All other settings can be left untouched.

![ODBC Config](/assets/images/sqlite/odbc_config.png)

6) Click OK to create the ODBC data source. You will return to the list of datasources with the new one selected.

7) Open Microsoft Access.

8) Create a new, blank Access database. You can save it anywhere but it makes sense to place it in the same folder as the SQLite Workbench database.

9) Click the `External Data` tab.

10) Click the `ODBC Database` button.

![Access Link](/assets/images/sqlite/access_link.png)

11) Click the Option to `Link to the datasource by creating a linked table`.

12) In the `Select Data Source` popup that appears, switch to the `Machine Data Source` tab and select the ODBC data source that you created in the earlier step. Then click `OK`.

![Access ODBC](/assets/images/sqlite/access_odbc.png)

13) A window will appear that shows all the tables in the CHaMP Worbkench SQLite database. Select the ones that you are interested in. Hold the `Shift` key to multi-select, or click the `Select All` button to link to all the SQLite tables. 

14) Click OK.

The end result of all these steps is a very small Access database that links to the relevant tables in the SQLite database. You can query, add, edit, delete data in Access and the changes will actually occur back in the SQLite database. You can also use all of Accesses features to filter records, create new queries etc. Note that the Access database is extremely small. It doesn't contain any actual data, but simply the links back to the SQLite database. (This is relevant if you want to email or share the Access database between colleagues or computers.)

![Access Result](/assets/images/sqlite/access_result.png)

## SQLite Programming with Python

SQLite is natively supported by the Python programming language ([documentation](https://docs.python.org/2/library/sqlite3.html)). Here's a simple script to select all CHaMP Visits:

```python
import sqlite3

conn = sqlite3.connect('C:\CHaMP\workbench.db')
curs = conn.cursor()
curs.execute('SELECT * FROM CHaMP_Visits')
for row in cur.fetchall():
    print(row)

conn.close()
```

## SQLite Programming with R

The R programming language can also [connect to SQLite databases](https://db.rstudio.com/databases/sqlite/).
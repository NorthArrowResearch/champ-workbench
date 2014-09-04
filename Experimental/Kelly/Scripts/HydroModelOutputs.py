#-------------------------------------------------------------------------------
# Name:        CHaMP Hydraulic Model Input Conversion Tool
# Purpose:     Prepare Input data for CHaMP Hydro Models
#
# Author:      Kelly Whitehead
#              South Fork Research, Inc
#              Seattle, WA
#
# Created:     Aug 04 2014
# Version:     2.0
# Modified:    
#-------------------------------------------------------------------------------
#!

# # Import Modules # #
import sys
import traceback
import csv
import argparse
import arcpy
from arcpy import sa
import CHaMP

# # Script Parameters # #
NoDataValue = -9999 ## csv no data value
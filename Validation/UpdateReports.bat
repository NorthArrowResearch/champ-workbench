 @echo off
   rem ***********************************************************************
   rem This bat file Goes and gets the lastest (and previous) C++ libraries
   rem In order for this to work you must have installed and configured
   rem The Amazon AWS Command line utilities: https://aws.amazon.com/cli/
   rem 
   rem Contact: Philip Bailey or Matt Reimer to gain access and keys necessary
   rem
   rem ***********************************************************************
 
   echo "----------------------------------------------------"
   echo "  UPDATING .xsl Files"
   echo "----------------------------------------------------"
   if not exist "./ReportTransforms" ( mkdir "./ReportTransforms" )
   aws s3 cp s3://releases.northarrowresearch.com/reports/master/xsl/rbt.xsl ./ReportTransforms/rbt.xsl
   aws s3 cp s3://releases.northarrowresearch.com/reports/master/xsl/rbt_manual.xsl ./ReportTransforms/rbt_manual.xsl
   aws s3 cp s3://releases.northarrowresearch.com/reports/master/xsl/watershed.xsl ./ReportTransforms/watershed.xsl
   pause
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace CHaMPWorkbench.Classes.ExceptionHandling
{
    /// <summary>
    /// This class serves to purposes:
    /// 
    /// 1) The NARException inherits from .net Exception and can grow over time to include
    /// more advanced properties etc.
    /// 
    /// 2) The public static method HandleException() should be used throughout the software
    /// to handle any kind of exception and a) show the standard user interface form and also
    /// log the error to AWS Cloud Watch.
    /// </summary>
    public class NARException : Exception
    {
        /// <summary>
        /// The AWS Cloud Watch Events will use the first part of the main, outer exception
        /// message as one of the top level keys that can be used to filter events. This constant
        /// caps the max length of the message that is used.
        /// </summary>
        private const int nMaxAWSMessage = 50;

        /// <summary>
        /// Use this static method to handle exceptions throughout the product.
        /// </summary>
        /// <param name="theException">The outermost exception to be handled.</param>
        /// <param name="bShowUserMessage">True shows the standard user interface form. False shows nothing to the user.</param>
        public static void HandleException(Exception theException, bool bShowUserMessage = true)
        {
            // Attempt to parse the exception information into a JSON object for sending to AWS Cloud Watch.
            string sExceptionJSON;
            try
            {
                sExceptionJSON = Classes.ExceptionHandling.NARException.ToJSON(theException, AWSCloudWatch.AWSCloudWatchSingleton.Instance.InstallationGUID);
            }
            catch (Exception ex)
            {
                // Something went wrong with the JSON, revert to just using the exception message.
                sExceptionJSON = ex.Message;
            }

            if (CHaMPWorkbench.Properties.Settings.Default.AWSLoggingEnabled)
            {
                // Truncate the main exception message to the max allowed length and log the exception to AWS.
                // Note that the AWS Singleton must already be instantiated. 
                // Note that this logging to AWS will fail silently if an internet connection is not available.
                int nMessageLength = Math.Min(theException.Message.Length, nMaxAWSMessage);
                AWSCloudWatch.AWSCloudWatchSingleton.Instance.Listener.WriteLine(string.Format("[Exception] [{0}] {1}", theException.Message.Substring(0, nMessageLength).Replace("[", "").Replace("]", "").Replace("\n", " "), sExceptionJSON));
                AWSCloudWatch.AWSCloudWatchSingleton.Instance.Listener.Flush();
            }

            if (bShowUserMessage)
            {
                // Cancel any wait cursor
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                // Show the standard user interface form to the user.
                // TODO: format the exception to plain English instead of JSON.
                String prettyString = Classes.ExceptionHandling.NARException.ToString(theException, AWSCloudWatch.AWSCloudWatchSingleton.Instance.InstallationGUID);
                CHaMPWorkbench.Classes.ExceptionHandling.frmException frm = new CHaMPWorkbench.Classes.ExceptionHandling.frmException(theException.Message, prettyString);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// Static function capable of parsing an exception into JSON format.
        /// </summary>
        /// <param name="ex">Exception to be parsed into JSON</param>
        /// <param name="installationKey">This is available from the AWS singleton object</param>
        /// <returns></returns>
        /// <remarks>Override and then call this function if you want to enhance the JSON information
        /// with more product specific information.</remarks>
        public static string ToJSON(Exception ex, Guid installationKey)
        {
            // Add the details of the outermost exception to a dictionary of JSON objects.
            // This will also recursively add the details of all inner exceptions
            Dictionary<string, object> row = new Dictionary<string, object>();
            AddExceptionDetailRows(ref row, ex);
            row.Add("ProductVersion", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            row.Add("OSVersion", Environment.OSVersion.ToString());
            row.Add("InstallationKey", installationKey.ToString());

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return serializer.Serialize(row);
        }

        /// <summary>
        /// We want a nice output for
        /// </summary>
        /// <returns></returns>
        public static string ToString(Exception ex, Guid installationKey)
        {
            // Add the details of the outermost exception to a dictionary of JSON objects.
            // This will also recursively add the details of all inner exceptions
            Dictionary<string, object> rows = new Dictionary<string, object>();
            AddExceptionDetailRows(ref rows, ex);
            rows.Add("ProductVersion", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            rows.Add("OSVersion", Environment.OSVersion.ToString());
            String prettyString = String.Empty;
            String prettyExData = String.Empty;
            foreach (KeyValuePair<string, object> row in rows)
            {
                if (row.Key == "ExceptionData")
                {
                    Dictionary<string, object> exDataContainer = (Dictionary<string, object>)row.Value;
                    prettyExData = String.Format("EXCEPTION DATA: {0}", Environment.NewLine);
                    foreach (KeyValuePair<string, object> exData in exDataContainer)
                    {
                        prettyExData += String.Format("  {0}: {1}{2}", exData.Key, exData.Value.ToString(), Environment.NewLine);

                    }
                    prettyString += prettyExData + Environment.NewLine;
                }
                else
                {
                    prettyString += String.Format("{0}: {1}{2}", row.Key, row.Value.ToString(), Environment.NewLine);
                }
            }
            return prettyString;
        }

        /// <summary>
        /// Recursive function that adds the native details of an exception to the JSON serializer dictionary
        /// </summary>
        /// <param name="row">Existing dictionary of strings to objects that will be serialized</param>
        /// <param name="ex">Exception. First called this should be the "this" pointer, from there on recursion re-calls this method on inner exception</param>
        private static void AddExceptionDetailRows(ref Dictionary<string, object> row, Exception ex)
        {
            // Start with the most basic information
            row.Add("Message", ex.Message);
            row.Add("Type", "Exception");
            row.Add("Severity", "Error");

            if (ex.Source != null)
                row.Add("ExceptionSource", ex.Source);

            // I don't think inner exceptions will have a TargetSite
            if (ex.TargetSite != null)
                row.Add("ErrorInMethod", ex.TargetSite.Name);

            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                try
                {
                    // The stack trace needs to remove the start of file paths that refer to the 
                    // computer on which the code was compiled. Important!!!! For this to work, 
                    // the top level namespace of the project needs to match the folder in which
                    // the code is located on the computer on which it is built.
                    Type myType = typeof(NARException);
                    string sName = myType.Namespace.ToString().Substring(0, myType.Namespace.ToString().IndexOf("."));
                    string sRegEx = string.Format("[A-Z]:[\\/]*.*{0}", sName);
                    Regex theRegEx = new Regex(sRegEx);
                    string sStackTrace = ex.StackTrace;
                    Match theMatch = theRegEx.Match(ex.StackTrace);
                    if (theMatch.Groups.Count > 0 && theMatch.Length > 0)
                        sStackTrace = sStackTrace.Replace(theMatch.Groups[0].Value.ToString(), "");
                    else
                        sStackTrace = ex.StackTrace;

                    row.Add("StackTrace", sStackTrace);
                }
                catch (Exception exStackTrace)
                {
                    row.Add("StackTrace", string.Format("Error getting stack trace: {0}", exStackTrace.Message));
                }
            }

            // Append any data dictionary items to the JSON dictionary.
            if (ex.Data.Count > 0)
            {
                Dictionary<string, object> exceptionData = new Dictionary<string, object>();
                foreach (string sKey in ex.Data.Keys)
                    exceptionData.Add(sKey, ex.Data[sKey].ToString());
                row.Add("ExceptionData", exceptionData);
            }

            // Recursively add any inner exceptions.
            if (ex.InnerException is Exception)
            {
                Dictionary<string, object> theInnerException = new Dictionary<string, object>();
                AddExceptionDetailRows(ref theInnerException, ex.InnerException);
                row.Add("InnerException", theInnerException);
            }
        }
    }
}

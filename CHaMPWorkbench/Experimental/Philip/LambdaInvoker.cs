using System.Collections.Generic;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using System.IO;
using System.Web.Script.Serialization;
using System.ComponentModel;

namespace CHaMPWorkbench.Experimental.Philip
{
    public class LambdaInvoker
    {
        public string Tool { get; internal set; }
        public string Queue { get; internal set; }
        public string Function { get; internal set; }
        public string Bucket { get; internal set; }

        public List<CHaMPData.VisitBasic> SuccessVisits { get; internal set; }
        public List<CHaMPData.VisitBasic> ErrorVisits { get; internal set; }

        private BackgroundWorker bgWorker;

        /// <summary>
        /// AWS Lambda Invoke for CHaMP Automation worker
        /// </summary>
        /// <param name="bg">User interface background worker</param>
        /// <param name="sTool">CHaMP automation Tool name e.g. RUN_TOPOMETRICS</param>
        /// <param name="sQueue">AWS SQS queue in which to put the worker e.g. CHaMPWatcher</param>
        /// <param name="sFunction">Function call e.g. CHaMPWatcher</param>
        /// <param name="sBucket">AWS S3 bucket in which to find the data e.g. sfr-champdata</param>
        public LambdaInvoker(ref BackgroundWorker bg, string sTool, string sQueue, string sFunction, string sBucket)
        {
            Tool = sTool;
            Queue = sQueue;
            Function = sFunction;
            Bucket = sBucket;
            bgWorker = bg;
        }

        /// <summary>
        /// Run the Lambda invoke for the argument list of visits
        /// </summary>
        /// <param name="lVisits">List of visits on which to run AWS Lambda</param>
        /// <remarks>
        /// https://forums.aws.amazon.com/thread.jspa?messageID=771901&tstart=0
        /// </remarks>
        public void Run(List<CHaMPData.VisitBasic> lVisits)
        {
            SuccessVisits = new List<CHaMPData.VisitBasic>();
            ErrorVisits = new List<CHaMPData.VisitBasic>();

            for (int i = 0; i < lVisits.Count; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                CHaMPData.VisitBasic visit = lVisits[i];

                // Format the visit key that is the S3 path together with the tool that is to be run
                string visitKey = string.Format("QA/{0}/{1}/{2}/VISIT_{3}/{4}", visit.VisitYear, visit.Site.Watershed, visit.Site, visit.ID, Tool).Replace(" ", "");

                // Build the JSON object that will be passes to Lambda. Use C# classes with public
                // member properties to avoid the messy work of escaping strings and braces etc.
                LambaArgs args = new LambaArgs(Bucket, visitKey, Queue);
                string json = new JavaScriptSerializer().Serialize(args).Replace("objectPlaceholder", "object");

                using (var client = new AmazonLambdaClient(RegionEndpoint.USWest2))
                {
                    var request = new InvokeRequest { FunctionName = Function, Payload = json, InvocationType = "RequestResponse" };
                    var response = client.Invoke(request);

                    string result;
                    using (var sr = new StreamReader(response.Payload))
                    {
                        result = sr.ReadToEnd();
                        if (result.ToLower().Contains("success") || result.ToLower().Contains("throttlingexception"))
                        {
                            SuccessVisits.Add(visit);
                        }
                        else
                        {
                            ErrorVisits.Add(visit);
                        }
                    }
                }

                bgWorker.ReportProgress(naru.ui.ProgressBar.ProgressPercent(i + 1, lVisits.Count));
            }
        }

        #region Lambda Arguments
        /// <summary>
        /// All this code does is structure the arguments to the lambda process as C# classes
        /// with properties so that it can be serialized into a JSON object.
        /// </summary>

        public class LambaArgs
        {
            public List<RecordArgs> Records { get; internal set; }

            public LambaArgs(string sBucket, string sVisitKey, string sQueue)
            {
                Records = new List<RecordArgs>();
                Records.Add(new RecordArgs(sBucket, sVisitKey, sQueue));
            }

            public class RecordArgs
            {
                public string eventSource { get; internal set; }
                public string eventName { get; internal set; }
                public string SQSQueue { get; internal set; }
                public S3Args s3 { get; internal set; }

                public RecordArgs(string sBucket, string sVisitKey, string sQueue)
                {
                    eventSource = "aws:s3";
                    eventName = "ObjectCreated:WBInvoke";
                    SQSQueue = sQueue;
                    s3 = new S3Args(sBucket, sVisitKey);
                }

                public class S3Args
                {
                    public BucketArgs bucket { get; internal set; }
                    public ObjectArgs objectPlaceholder { get; internal set; }

                    public S3Args(string sBucket, string sVisitKey)
                    {
                        bucket = new BucketArgs(sBucket);
                        objectPlaceholder = new ObjectArgs(sVisitKey);
                    }

                    public class BucketArgs
                    {
                        public string name { get; internal set; }

                        public BucketArgs(string sBucket)
                        {
                            name = sBucket;
                        }
                    }

                    public class ObjectArgs
                    {
                        public string key { get; internal set; }
                        public int size { get; internal set; }

                        public ObjectArgs(string sVisitKey)
                        {
                            key = sVisitKey;
                            size = 99999999;
                        }
                    }
                }

            }

            #endregion
        }
    }
}

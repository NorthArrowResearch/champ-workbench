using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Amazon.CloudWatchLogs.Model;
using System.Threading;

namespace CHaMPWorkbench.Classes.AWSCloudWatch
{

    /// <summary>
    /// <para>
    /// CloudWatchLogsTraceListener writes trace logs to AWS CloudWatchLogs Service.
    /// The Group and Stream must already exist. You can specify unique credentials 
    /// and a different region in the config file, or use the default credentials 
    /// and region used by the rest of the application.
    /// </para>
    /// 
    /// <para>
    /// Example of an app.config entry setting up the listener with all possible configurations specified:
    /// <code>
    /// &lt;system.diagnostics&gt;
    ///   &lt;trace&gt;
    ///     &lt;listeners&gt;
    ///       &lt;add name=&quot;CloudWatchListener&quot; type=&quot;BrianBeach.CloudWatchLogsTraceListener, CloudWatchLogs&quot;
    ///              AWSRegion=&quot;us-east-1&quot;
    ///              AWSAccessKey=&quot;XXXXXXXXXXXXXXXXXXXXX&quot;
    ///              AWSSecretKey=&quot;YYYYYYYYYYYYYYYYYYYYY&quot;
    ///              AWSStreamName=&quot;CloudWatchLogsTraceListener&quot;
    ///              AWSStreamName=&quot;CloudWatchLogsTraceListener&quot;
    ///         /&gt;
    ///     &lt;/listeners&gt;
    ///   &lt;/trace&gt;
    /// &lt;/system.diagnostics&gt;
    /// </code>
    /// </para>
    /// </summary>
    public class CloudWatchLogsTraceListener : System.Diagnostics.TraceListener
    {

        //Limit Attributes
        private const int MAX_TIME_BETWEEN_PUBLISH = 15000; //If it's been more than 15 seconds, publish.
        private const int MAX_EVENTS_PER_BATCH = 100;       //If the buffer exceeds 100 events, publish.
        private const int MAX_MESSAGE_SIZE = (4 * 1024);    //Limit an individual message to 4K characters or about 8KB. 
        private const int MAX_BUFFER_SIZE = (32 * 1024);    //Limit the batch to 32K characters of about 64KB.

        //State Tracking
        private bool _Connected = false;                    //Are we connected to AWS yet
        private bool _ConnectionFailed = false;             //True if we tried to connect but it failed

        //Event Buffer - Debug statements are added to the queue and a flush reads from the queue
        private ConcurrentQueue<InputLogEvent> _EventQueue = new ConcurrentQueue<InputLogEvent>();

        //CloudWatch Logs Client
        private AmazonCloudWatchLogsClient _Client = null;

        //tracks the current position in the log
        private string _Token = null;

        //Used to signal background thread that there are events to publish
        private AutoResetEvent _EventsAvailable = new AutoResetEvent(false);

        //Bufers calls to write until you call WriteLine() 
        private StringBuilder _Buffer = new StringBuilder();

        #region Properties

        /// <summary>
        /// Gets the GroupName used to connect publish events CloudWatchLogs.
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// Gets the GroupName used to connect publish events CloudWatchLogs.
        /// </summary>
        public string StreamName { get; private set; }

        /// <summary>
        /// If FailOnError is true the trace listener will throw an error on failure.
        /// The default behaviour is ignore errors.  The assumption is that it is better
        /// to keep running even if we cannot connect to the SloudWatch logs service.
        /// If you depend on the logs for auditing then set this property to true, but
        /// keep in mind that a connectivity issue will cause the entire application to fail.
        /// </summary>
        public bool FailOnError { get; private set; }

        #endregion    

        #region Constructors

        /// <summary>
        /// Constructs an CloudWatchLogsTraceListener object.
        /// Assumes the groupName, streamName, region, accessKey and secretKey will be read from the app.config file or use default credentials.
        /// </summary>
        public CloudWatchLogsTraceListener()
            : this(null, null, false)
        {
            //Assumes that all configuration will be read from the config file.
        }

        /// <summary>
        /// Constructs an CloudWatchLogsTraceListener object with groupName, streamName.
        /// Assumes the region, accessKey and secretKey will be read from the app.config file or use default credentials.
        /// </summary>
        /// <param name="groupName">Optional. CloudWatch Logs group name.</param>
        /// <param name="streamName">Optional. CloudWatch Logs group name.</param>
        public CloudWatchLogsTraceListener(string groupName, string streamName)
            : this(groupName, streamName, false)
        {
            //Assumes the region, access and secret key will be read from the config file.
        }

        /// <summary>
        /// Constructs an CloudWatchLogsTraceListener object with groupName, streamName, region, accessKey, secretKey, and failOnError.
        /// You can leave any parameter null.  If null the value will be read from the app.config file or use default credentials.
        /// </summary>
        /// <param name="groupName">Optional. CloudWatch Logs group name.</param>
        /// <param name="streamName">Optional. CloudWatch Logs group name.</param>
        /// <param name="failOnError">Optional. If true the trace listner will throw errors when it cannot connect to the CloudWatch Logs service.</param>
        public CloudWatchLogsTraceListener(string groupName, string streamName, bool failOnError = false)
        {
            //You cannot read from the configuration file until the object is created. 
            //Therefore, store the configuration and connect the first time flush is called 
            GroupName = groupName;
            StreamName = streamName;
            FailOnError = failOnError;

            //Start the publisher thread that will occasional flush the buffer
            (new Thread(PublisherThread) { IsBackground = true }).Start();
        }

        #endregion


        private void Connect()
        {

            try
            {
                _Client = new AmazonCloudWatchLogsClient(
                    CHaMPWorkbench.Properties.Settings.Default.AWSKey,
                    CHaMPWorkbench.Properties.Settings.Default.AWSSecret,
                    Amazon.RegionEndpoint.GetBySystemName(CHaMPWorkbench.Properties.Settings.Default.AWSRegion));

                //Let's try to connect to the AWS Cloud with Group and Steam provided
                var response = _Client.DescribeLogStreams(new DescribeLogStreamsRequest()
                {
                    LogGroupName = GroupName,
                    LogStreamNamePrefix = StreamName
                });

                // If we can't find a stream with this name then make one. 
                if (response.LogStreams.Count == 0)
                {
                    _Client.CreateLogStream(new CreateLogStreamRequest() {
                        LogGroupName = GroupName,
                        LogStreamName = StreamName
                    });
                    // try again now
                    response = _Client.DescribeLogStreams(new DescribeLogStreamsRequest()
                    {
                        LogGroupName = GroupName,
                        LogStreamNamePrefix = StreamName
                    });
                }

                 //Get the next token needed to publish
                 _Token = response.LogStreams[0].UploadSequenceToken;
            }
            catch (Exception ex)
            {
                _ConnectionFailed = true;
                if (FailOnError) throw;
            }

            if (!_ConnectionFailed)
            {
                _Connected = true;
            }
        }

        /// <summary>
        /// Writes a message to CloudWatchLogs.  Calls to Write() will be buffered until you call 
        /// WriteLine().  Each call to WriteLine() will result in a CloudWatchLogs event.  
        /// If you call flush() before calling WriteLine() the event will include a partial line.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public override void Write(string message)
        {
            if (!_ConnectionFailed)
            {
                _Buffer.Append(message);
            }
        }

        /// <summary>
        /// Writes a message to CloudWatchLogs.  Each call to WriteLine() will result in a CloudWatchLogs event.  
        /// </summary>
        /// <param name="message">The message to write.</param>
        public override void WriteLine(string message)
        {
            if (!_ConnectionFailed)
            {
                if (_Buffer.Length > 0)
                {
                    _Buffer.Append(message);
                    message = _Buffer.ToString();
                    _Buffer.Clear();
                }

                //Check message length
                if (message.Length > MAX_MESSAGE_SIZE)
                {
                    //Message is too big, so trim it
                    message = message.Substring(0, MAX_MESSAGE_SIZE);
                }

                //Add the the event to the queue
                _EventQueue.Enqueue(new InputLogEvent()
                {
                    Timestamp = DateTime.Now,
                    Message = message
                });

                if (_EventQueue.Count >= MAX_EVENTS_PER_BATCH)
                {
                    //We hit the event count limit, so publish
                    _EventsAvailable.Set();
                }
            }
        }

        /// <summary>
        /// This TraceListener will queue events and publish either every 15 seconds 
        /// or when the number of events in the queue exceeds 100 to minimize network traffic. 
        /// Calling Flush() will publish the queu.  If you have called Write() without 
        /// calling WriteLine() then an explicit call to Flush() will break the line across 
        /// two events.  Note that you must call Flush() on shutdown to ensure that the last
        /// few events are uploaded to CloudWatch Logs before exiting.
        /// </summary>
        public override void Flush()
        {
            if (_Buffer.Length > 0) this.WriteLine("");
            Publish();
        }

        /// <summary>
        /// This method will upload the event queue to CloudWatch Logs
        /// </summary>
        private void Publish()
        {
            base.Flush();

            //If we were unable to connect then bail out
            if (_ConnectionFailed) return;

            //Typically Publish will be called by the PublisherThread, but it can be called explicitly 
            //with a Flush. If two threads publish at the same time the Token will get out of sync.
            //Therefore let's lock this section to be sure we only publish one batch at a time.
            lock (_EventQueue)
            {
                //If this is the first time we published, we need to connect to get the token.
                if (!_Connected) Connect();

                //If we are not connected then do nothing. This is just debugging information
                if (_Connected)
                {

                    //We are going to publish in batches upto the max message size until the queue is empty
                    while (_EventQueue.Count > 0)
                    {
                        //Buffer for a batch of messages
                        List<InputLogEvent> publishBuffer = new List<InputLogEvent>();
                        int buffersize = 0;

                        //Read events from the queue and put them in the buffer
                        //We will either read the entire queue or exceed the max message size
                        InputLogEvent logEvent;
                        while (_EventQueue.TryDequeue(out logEvent))
                        {
                            //Keep track of the buffer size
                            buffersize += logEvent.Message.Length;
                            //Add the event to the buffer
                            publishBuffer.Add(logEvent);
                            //If the buffer gets too big, stop and publish
                            if (buffersize > MAX_BUFFER_SIZE) break;
                        }

                        //If there are any messages in the buffer, publish it
                        if (publishBuffer.Count > 0)
                        {
                            try
                            {
                                var response = _Client.PutLogEvents(new PutLogEventsRequest()
                                {
                                    LogGroupName = GroupName,
                                    LogStreamName = StreamName,
                                    SequenceToken = _Token,
                                    LogEvents = publishBuffer
                                }
                                );
                                //Update the token needed for the next publish
                                _Token = response.NextSequenceToken;
                            }
                            catch
                            {
                                if (FailOnError) throw;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is the background thread that periodically flushed the queue.
        /// </summary>
        private void PublisherThread()
        {
            while (true)
            {
                //Wait until it's time to publish. This occurs when the timer runs out or
                //the main threads signals us because there are too many events in the queue.
                _EventsAvailable.WaitOne(MAX_TIME_BETWEEN_PUBLISH);
                Publish();
            }
        }
    }
}


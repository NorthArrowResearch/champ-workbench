﻿using System;

namespace CHaMPWorkbench.Classes.AWSCloudWatch
{
    /// <summary>
    /// This class is designed to be instantiated once within the parent application (i.e. the name "singleton").
    /// It's purpose is to have an AWS Cloud Watch listener that sticks around and is used by all calls to AWS.
    /// 
    /// The software product using this class needs several "Settings" defined before this class can be used. See below.
    /// The constructor for this class looks in the software settings to see if an installation key (GUID) is already
    /// defined. If it is, then the constructor uses the GUID to construct a listener. If a GUID has not been asigned
    /// by the parent product then the singleton is created but no listener instantiated.
    /// </summary>
    public class AWSCloudWatchSingleton : Secrets
    {
        private static AWSCloudWatchSingleton instance;
        private static CloudWatchLogsTraceListener m_listener;
        private static Guid m_InstallationGUID;

        public CloudWatchLogsTraceListener Listener { get { return m_listener; } }

        public static AWSCloudWatchSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AWSCloudWatchSingleton();
                }
                return instance;
            }
        }

        public Guid InstallationGUID
        {
            get
            {
                return m_InstallationGUID;
            }

            private set
            {
                m_InstallationGUID = value;
            }
        }

        public static bool HasInstallationGUID
        {
            get
            {
                return !Properties.Settings.Default.AWSCloudWatchGUID.Equals(new Guid("00000000-0000-0000-0000-000000000000"));
            }
        }

        public AWSCloudWatchSingleton()
        {
            if (!AWSCloudWatchSingleton.HasInstallationGUID)
            {
                InstallationGUID = Guid.NewGuid();
                Properties.Settings.Default.AWSCloudWatchGUID = InstallationGUID;
                Properties.Settings.Default.Save();
            }
            else
                m_InstallationGUID = CHaMPWorkbench.Properties.Settings.Default.AWSCloudWatchGUID;

            string sStreamName = string.Format("{0}", Properties.Settings.Default.AWSCloudWatchGUID);

            // Fail silently in release modes
            bool explicitfail = false;
#if DEBUG
            explicitfail = true;
#endif
            m_listener = new CloudWatchLogsTraceListener(AWSGroupName,
                sStreamName, explicitfail);

        }
    }
}
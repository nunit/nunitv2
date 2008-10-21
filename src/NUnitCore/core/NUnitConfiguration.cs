// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Threading;

namespace NUnit.Core
{
    public class NUnitConfiguration
    {
        #region Class Constructor
        /// <summary>
        /// Class constructor initializes fields from config file
        /// </summary>
        static NUnitConfiguration()
        {
            try
            {
                NameValueCollection settings = GetConfigSection("NUnit/TestCaseBuilder");
                if (settings != null)
                {
                    string oldStyle = settings["OldStyleTestCases"];
                    if (oldStyle != null)
                            allowOldStyleTests = Boolean.Parse(oldStyle);
                }

                settings = GetConfigSection("NUnit/TestRunner");
                if (settings != null)
                {
                    string apartment = settings["ApartmentState"];
                    if (apartment != null)
                        apartmentState = (ApartmentState)
                            System.Enum.Parse(typeof(ApartmentState), apartment, true);

                    string priority = settings["ThreadPriority"];
                    if (priority != null)
                        threadPriority = (ThreadPriority)
                            System.Enum.Parse(typeof(ThreadPriority), priority, true);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Invalid configuration setting in {0}",
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                throw new ApplicationException(msg, ex);
            }
        }

        private static NameValueCollection GetConfigSection( string name )
        {
#if NET_2_0
            return (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(name);
#else
			return (NameValueCollection)System.Configuration.ConfigurationSettings.GetConfig(name);
#endif
        }
        #endregion

        #region AllowOldStypeTests
        private static bool allowOldStyleTests = false;
        public static bool AllowOldStyleTests
        {
            get { return allowOldStyleTests; }
        }
        #endregion

        #region ThreadPriority
        private static ThreadPriority threadPriority = ThreadPriority.Normal;
        public static ThreadPriority ThreadPriority
        {
            get { return threadPriority; }
        }
        #endregion

        #region ApartmentState
        private static ApartmentState apartmentState = ApartmentState.Unknown;
        public static ApartmentState ApartmentState
        {
            get { return apartmentState; }
            //set { apartmentState = value; }
        }
        #endregion

        #region BuildConfiguration
        public static string BuildConfiguration
        {
            get
            {
#if DEBUG
                    return "Debug";
#else
					return "Release";
#endif
            }
        }
        #endregion

        #region NUnitDirectory
        private static string nunitDirectory;
        public static string NUnitDirectory
        {
            get
            {
                if (nunitDirectory == null)
                {
                    nunitDirectory =
                        AssemblyHelper.GetDirectoryName(Assembly.GetExecutingAssembly());
                }

                return nunitDirectory;
            }
        }
        #endregion

        #region AddinDirectory
        private static string addinDirectory;
        public static string AddinDirectory
        {
            get
            {
                if (addinDirectory == null)
                {
                    addinDirectory = System.IO.Path.Combine(NUnitDirectory, "addins");

                    // Special handling for running NUnit in the VS tree
                    if (addinDirectory.EndsWith("bin\\" + BuildConfiguration) && !Directory.Exists(addinDirectory))
                    {
                        DirectoryInfo fi = new DirectoryInfo(addinDirectory).Parent.Parent.Parent.Parent;
                        addinDirectory = Path.Combine(fi.FullName, "GuiRunner/nunit-gui-exe/bin/" + BuildConfiguration + "/addins");
                    }
                }

                return addinDirectory;
            }
        }
        #endregion

        #region TestAgentExePath
        private static string testAgentExePath;
        public static string TestAgentExePath
        {
            get
            {
                if (testAgentExePath == null)
                {
                    string agentDir = NUnitDirectory;

                    // Special handling for running NUnit in the VS tree
                    if (agentDir.EndsWith("bin\\" + BuildConfiguration))
                    {
                        DirectoryInfo fi = new DirectoryInfo(agentDir).Parent.Parent.Parent.Parent;
                        agentDir = Path.Combine(fi.FullName, "NUnitTestServer/nunit-agent-exe/bin/" + BuildConfiguration);
                    }

                    testAgentExePath = Path.Combine(agentDir, "nunit-agent.exe");
                }

                return testAgentExePath;
            }
        }
        #endregion
    }
}

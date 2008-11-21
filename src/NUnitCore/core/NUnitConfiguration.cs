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
using Microsoft.Win32;

namespace NUnit.Core
{
    /// <summary>
    /// Provides static methods for accessing the NUnit config
    /// file 
    /// </summary>
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

        #region Public Properties

        #region AllowOldStyleTests
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
                    addinDirectory = Path.Combine(GetGuiDirectory(), "addins");
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
                    testAgentExePath = Path.Combine(GetAgentDirectory(), "nunit-agent.exe");

                return testAgentExePath;
            }
        }
        #endregion

        #region ApplicationDataDirectory
        private static string applicationDirectory;
        public static string ApplicationDirectory
        {
            get
            {
                if (applicationDirectory == null)
                {
                    applicationDirectory = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "NUnit");
                }

                return applicationDirectory;
            }
        }
        #endregion

        #region InstallDirectory
        private static string installDir;
        public static string InstallDirectory
        {
            get
            {
                if (installDir == null)
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(
                        @"Software\nunit.org\2.4");
                    if (key != null)
                        installDir = key.GetValue("InstallDir") as string;

                }
                 
                return installDir;
            }
        }
        #endregion

        #endregion

        #region Private Properties and Methods
        private static bool RunningFromSourceTree
        {
            get { return NUnitDirectory.EndsWith( Path.Combine( "bin", BuildConfiguration ) ); }
        }

        private static string GetAgentDirectory()
        {
            return RunningFromSourceTree
                ? Path.Combine(SourceTreeRoot, "NUnitTestServer/nunit-agent-exe/bin/" + BuildConfiguration)
                : NUnitDirectory;
        }

        private static string GetGuiDirectory()
        {
            return RunningFromSourceTree
                ? Path.Combine(SourceTreeRoot, "GuiRunner/nunit-gui-exe/bin/" + BuildConfiguration)
                : NUnitDirectory;
        }

        private static string SourceTreeRoot
        {
            get
            {
                return new DirectoryInfo(NUnitDirectory).Parent.Parent.Parent.Parent.FullName;
            }
        }
        #endregion
    }
}

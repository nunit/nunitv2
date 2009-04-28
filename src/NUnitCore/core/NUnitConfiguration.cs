// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Configuration;
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

        #region NUnitLibDirectory
        private static string nunitLibDirectory;
        /// <summary>
        /// Gets the path to the lib directory for the version and build
        /// of NUnit currently executing.
        /// </summary>
        public static string NUnitLibDirectory
        {
            get
            {
                if (nunitLibDirectory == null)
                {
                    nunitLibDirectory =
                        AssemblyHelper.GetDirectoryName(Assembly.GetExecutingAssembly());
                }

                return nunitLibDirectory;
            }
        }
        #endregion

        #region NUnitBinDirectory
        private static string nunitBinDirectory;
        public static string NUnitBinDirectory
        {
            get
            {
                if (nunitBinDirectory == null)
                {
                    nunitBinDirectory = Path.GetDirectoryName(NUnitLibDirectory);
                }

                return nunitBinDirectory;
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
                    addinDirectory = Path.Combine(NUnitBinDirectory, "addins");
                }

                return addinDirectory;
            }
        }
        #endregion

        #region TestAgentExePath
        //private static string testAgentExePath;
        //private static string TestAgentExePath
        //{
        //    get
        //    {
        //        if (testAgentExePath == null)
        //            testAgentExePath = Path.Combine(NUnitBinDirectory, "nunit-agent.exe");

        //        return testAgentExePath;
        //    }
        //}
        #endregion

        #region MonoExePath
        private static string monoExePath;
        public static string MonoExePath
        {
            get
            {
                if (monoExePath == null)
                {
                    if (RuntimeFramework.CurrentFramework.IsMono)
                        return AssemblyHelper.GetAssemblyPath(Assembly.GetEntryAssembly());
                    
                    // Assume it's windows for now
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Novell\Mono");
                    if (key != null)
                    {
                        string version = key.GetValue("DefaultCLR") as string;
                        if (version != null)
                        {
                            key = key.OpenSubKey(version);
                            if (key != null)
                            {
                                string installDir = key.GetValue("SdkInstallRoot") as string;
                                if (installDir != null)
                                    monoExePath = Path.Combine(installDir, @"bin\mono.exe");
                            }
                        }
                    }
                }

                return monoExePath;
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

        #region HelpUrl
        public static string HelpUrl
        {
            get
            {
#if NET_2_0
                string helpUrl = ConfigurationManager.AppSettings["helpUrl"];
#else
                string helpUrl = ConfigurationSettings.AppSettings["helpUrl"];
#endif

                if (helpUrl == null)
                {
                    helpUrl = "http://nunit.org";
                    string dir = Path.GetDirectoryName(NUnitBinDirectory);
                    if ( dir != null )
                    {
                        dir = Path.GetDirectoryName(dir);
                        if ( dir != null )
                        {
                            string localPath = Path.Combine(dir, @"doc/index.html");
                            if (File.Exists(localPath))
                            {
                                UriBuilder uri = new UriBuilder();
                                uri.Scheme = "file";
                                uri.Host = "localhost";
                                uri.Path = localPath;
                                helpUrl = uri.ToString();
                            }
                        }
                    }
                }

                return helpUrl;
            }
        }
        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the NUnit Bin Directory for a particular
        /// runtime version, or null if it's not installed.
        /// </summary>
        public static string GetNUnitBinDirectory(Version v)
        {
            string dir = NUnitBinDirectory;

            if ( Environment.Version.Major == v.Major )
                return dir;

            string current = Environment.Version.ToString(2);
            string target = v.ToString(2);
            if (current == "1.0") current = "1.1";
            if (target == "1.0") target = "1.1";
            if (target == current)
                return dir;

            if (dir.IndexOf(current) < 0)
                return null;

            dir = dir.Replace(current, target);

            return Directory.Exists(dir) ? dir : null;
        }

        public static string GetTestAgentExePath(Version v)
        {
            string binDir = GetNUnitBinDirectory(v);
            if ( binDir == null ) return null;

            string agentExePath = Path.Combine(binDir, "nunit-agent.exe");
            return File.Exists(agentExePath) ? agentExePath : null;
        }

        #endregion
    }
}

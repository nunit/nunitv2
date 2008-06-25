using System;
using System.Collections.Specialized;
using System.Threading;

namespace NUnit.Core
{
    public class NUnitConfiguration
    {
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

        #region Properties
        private static bool allowOldStyleTests = false;
        public static bool AllowOldStyleTests
        {
            get { return allowOldStyleTests; }
        }

        private static ThreadPriority threadPriority = ThreadPriority.Normal;
        public static ThreadPriority ThreadPriority
        {
            get { return threadPriority; }
        }

        private static ApartmentState apartmentState = ApartmentState.Unknown;
        public static ApartmentState ApartmentState
        {
            get { return apartmentState; }
            set { apartmentState = value; }
        }
        #endregion

        #region BuildConfiguration
        public static string BuildConfiguration
        {
            get
            {
#if DEBUG
                if (Environment.Version.Major == 2)
                    return "Debug2005";
                else
                    return "Debug";
#else
				if (Environment.Version.Major == 2)
					return "Release2005";
				else
					return "Release";
#endif
            }
        }
        #endregion
    }
}

// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System.Collections.Specialized;
using System.Configuration;

namespace NUnit.Core
{
    public abstract class LogCapture : TextCapture
    {
        private string defaultThreshold;

        /// <summary>
        /// The default threshold for log capture
        /// is read from the config file. If not
        /// found, we use "Off".
        /// </summary>
        public override string DefaultThreshold
        {
            get
            {
                if (defaultThreshold == null)
                {
                    defaultThreshold = "Off";

                    NameValueCollection settings = (NameValueCollection)
#if CLR_2_0 || CLR_4_0
                    ConfigurationManager.GetSection("NUnit/TestRunner");
#else
                    ConfigurationSettings.GetConfig("NUnit/TestRunner");
#endif

                    if (settings != null)
                    {
                        string level = settings["DefaultLogThreshold"];
                        if (level != null)
                            defaultThreshold = level;
                    }
                }

                return defaultThreshold;
            }
        }
    }
}

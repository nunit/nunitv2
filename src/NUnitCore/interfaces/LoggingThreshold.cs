// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

namespace NUnit.Core
{
    /// <summary>
    /// Enumeration expressing the level of text messages to be 
    /// captured by NUnit and sent to the runner.
    /// </summary>
    public enum LoggingThreshold
    {
        Off = 0,
        Fatal = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 5,
        All = 6,
    }
}

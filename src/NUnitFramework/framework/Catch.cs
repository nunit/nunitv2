// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Text;

namespace NUnit.Framework
{
    /// <summary>
    /// The Catch class is used to capture an exception.
    /// </summary>
    class Catch
    {
        /// <summary>
        /// Capture any exception that is thrown by the delegate
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <returns>The exception thrown, or null</returns>
        public static Exception Exception(TestDelegate code)
        {
            try
            {
                code();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}

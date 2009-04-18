// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System.Collections;
using System.Reflection;

namespace NUnit.Core.Extensibility
{
    /// <summary>
    /// The ITestCaseProvider interface is used by extensions
    /// that provide data for a single test parameter.
    /// </summary>
    public interface IDataPointProvider
    {
        /// <summary>
        /// Determine whether any data is available for a parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>True if any data is available, otherwise false.</returns>
        bool HasDataFor(ParameterInfo parameter);

        /// <summary>
        /// Return an IEnumerable providing data for use with the
        /// supplied parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>An IEnumerable providing the required data</returns>
        IEnumerable GetDataFor(ParameterInfo parameter);
    }
}

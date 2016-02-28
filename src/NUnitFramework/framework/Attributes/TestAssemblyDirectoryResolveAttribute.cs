// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org
// ****************************************************************

using System;

namespace NUnit.Framework
{
    /// <summary>
    /// TestAssemblyDirectoryResolveAttribute is used to mark a test assembly as needing a
    /// special assembly resolution hook that will explicitly search the test assembly's
    /// directory for dependent assemblies. This works around a conflict between mixed-mode
    /// assembly initialization and tests running in their own AppDomain in some cases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public class TestAssemblyDirectoryResolveAttribute : Attribute
    {
    }
}

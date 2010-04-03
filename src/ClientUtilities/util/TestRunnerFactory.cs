// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************
using System;
using NUnit.Core;

namespace NUnit.Util
{
    /// <summary>
    /// Handles creation of a suitable test runner for a given package
    /// </summary>
    public class TestRunnerFactory
    {
        private RuntimeFrameworkSelector selector = new RuntimeFrameworkSelector();

        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>A TestRunner</returns>
        public TestRunner MakeTestRunner(TestPackage package)
        {
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework targetFramework = selector.SelectRuntimeFramework(package);

            ProcessModel processModel = (ProcessModel)package.GetSetting("ProcessModel", ProcessModel.Default);
            if ( processModel == ProcessModel.Default )
                if ( !targetFramework.MatchesClr( currentFramework ) )
                    processModel = ProcessModel.Separate;

            switch (processModel)
            {
                case ProcessModel.Multiple:
                    package.Settings.Remove("ProcessModel");
                    return new MultipleTestProcessRunner();
                case ProcessModel.Separate:
                    package.Settings.Remove("ProcessModel");
                    return new ProcessRunner();
                default:
                    DomainUsage domainUsage = 
                        (DomainUsage)package.GetSetting("DomainUsage", DomainUsage.Default);
                    switch (domainUsage)
                    {
                        case DomainUsage.Multiple:
                            package.Settings.Remove("DomainUsage");
                            return new MultipleTestDomainRunner();
                        case DomainUsage.None:
                            return new RemoteTestRunner();
                        case DomainUsage.Single:
                        default:
                            return new TestDomain();
                    }
            }
        }

    }
}

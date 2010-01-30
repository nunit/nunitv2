// ****************************************************************
// Copyright 2008, Charlie Poole
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
        static Logger log = InternalTrace.GetLogger(typeof(TestRunnerFactory));

        /// <summary>
        /// Returns a test runner based on the settings in a TestPackage.
        /// Any setting that is "consumed" by the factory is removed, so
        /// that downstream runners using the factory will not repeatedly
        /// create the same type of runner.
        /// </summary>
        /// <param name="package">The TestPackage to be loaded and run</param>
        /// <returns>A TestRunner</returns>
        public static TestRunner MakeTestRunner(TestPackage package)
        {
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
            RuntimeFramework targetFramework = package.Settings["RuntimeFramework"] as RuntimeFramework;
            if (targetFramework == null)
                targetFramework = currentFramework;
            else if (targetFramework.Runtime == RuntimeType.Any)
            {
                targetFramework = currentFramework.ReplaceRuntimeType(currentFramework.Runtime);
                package.Settings["RuntimeFramework"] = targetFramework;
            }

            log.Debug("Test requires {0} framework", targetFramework);

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
                            break;
                        case DomainUsage.None:
                            return new RemoteTestRunner();
                            break;
                        case DomainUsage.Single:
                        default:
                            return new TestDomain();
                            break;
                    }
            }
        }
    }
}

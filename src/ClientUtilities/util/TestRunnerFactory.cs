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
            string targetRuntime = package.Settings["RuntimeFramework"] as string;

            RuntimeFramework runtimeFramework = targetRuntime == null
                ? RuntimeFramework.CurrentFramework
                : new RuntimeFramework( targetRuntime );
            RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;

            log.Debug("Test requires {0} framework", runtimeFramework.Name);

            ProcessModel processModel = (ProcessModel)package.GetSetting("ProcessModel", ProcessModel.Default);
            if ( processModel == ProcessModel.Default )
                if (runtimeFramework.Name != currentFramework.Name )
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
                    if (domainUsage == DomainUsage.Multiple)
                    {
                        package.Settings.Remove("DomainUsage");
                        return new MultipleTestDomainRunner();
                    }
                    else
                        return new TestDomain();
            }
        }
    }
}

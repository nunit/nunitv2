using System;
using NUnit.Core;

namespace NUnit.Util
{
    public class TestRunnerFactory
    {
        public static TestRunner MakeTestRunner(TestPackage package)
        {
            string targetRuntime = package.Settings["RuntimeFramework"] as string;

            RuntimeFramework runtimeFramework = targetRuntime == null
                ? RuntimeFramework.CurrentFramework
                : new RuntimeFramework( targetRuntime );

            if ( package.GetSetting("SeparateProcess", false) ||
                 runtimeFramework.Runtime != RuntimeFramework.CurrentFramework.Runtime ||
                 runtimeFramework.Version != RuntimeFramework.CurrentFramework.Version)
            {
                package.Settings.Remove("SeparateProcess");
                return new ProcessRunner();
            }

            if ( package.GetSetting("MultiDomain", false) )
            {
                package.Settings.Remove("MultiDomain");
                return new MultipleTestDomainRunner();
            }

            return new TestDomain();
        }
    }
}

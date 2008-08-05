using System;
using NUnit.Core;

namespace NUnit.Util
{
    public class TestRunnerFactory
    {
        public static TestRunner MakeTestRunner(TestPackage package)
        {
            if ( package.GetSetting("SeparateProcess", false) )
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

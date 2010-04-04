using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
    [TestFixture]
    public class TestRunnerFactoryTests
    {
        private RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;

        [Test]
        public void SameFrameworkUsesTestDomain()
        {
            TestPackage package = new TestPackage("nunit.util.tests.dll");
            Assert.That( new TestRunnerFactory().MakeTestRunner(package),
                Is.TypeOf(typeof(TestDomain)));
        }
    }
}

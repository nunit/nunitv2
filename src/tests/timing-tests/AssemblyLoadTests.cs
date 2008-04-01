using System;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Tests.TimingTests
{
    [TestFixture]
    public class LoadTimingTests
    {
        private TestRunner runner;
        private TestLoader loader;

        [TestFixtureSetUp]
        public void InstallDomainManager()
        {
            if (Services.DomainManager == null)
                ServiceManager.Services.AddService(new DomainManager());
        }

        [TearDown]
        public void UnloadTests()
        {
            if (runner != null)
                runner.Unload();
            if (loader != null)
                loader.UnloadProject();
        }

        [Test]
        public void Load1000TestsInSameDomain()
        {
            runner = new SimpleTestRunner();
            int start = Environment.TickCount;
            Assert.IsTrue(runner.Load(new TestPackage("loadtest-assembly.dll")));
            ITest test = runner.Test;
            Assert.AreEqual(1000, test.TestCount);
            int ms = Environment.TickCount - start;
            Assert.LessOrEqual(ms, 4000);
        }

        [Test]
        public void Load1000TestsInTestDomain()
        {
            runner = new TestDomain();
            int start = Environment.TickCount;
            Assert.IsTrue(runner.Load(new TestPackage("loadtest-assembly.dll")));
            ITest test = runner.Test;
            Assert.AreEqual(1000, test.TestCount);
            int ms = Environment.TickCount - start;
            Assert.LessOrEqual(ms, 4000);
        }

        [Test]
        public void Load1000TestsUsingTestLoader()
        {
            loader = new TestLoader();
            int start = Environment.TickCount;
            loader.LoadProject("loadtest-assembly.dll");
            Assert.IsTrue(loader.IsProjectLoaded);
            loader.LoadTest();
            Assert.IsTrue(loader.IsTestLoaded);
            Assert.AreEqual(1000, loader.TestCount);
            int ms = Environment.TickCount - start;
            Assert.LessOrEqual(ms, 4000);
        }
    }
}

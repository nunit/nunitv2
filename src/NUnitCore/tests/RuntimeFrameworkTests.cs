using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class RuntimeFrameworkTests
    {
        static RuntimeType currentRuntime = 
            Type.GetType("Mono.Runtime", false) != null ? RuntimeType.Mono : RuntimeType.Net;

        [Test]
        public void CanGetCurrentFramework()
        {
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;

            Assert.That(framework.Runtime, Is.EqualTo(currentRuntime));
            Assert.That(framework.ClrVersion, Is.EqualTo(Environment.Version));
        }

        [Test]
        public void CurrentFrameworkMustBeAvailable()
        {
            Assert.That(RuntimeFramework.IsAvailable(RuntimeFramework.CurrentFramework));
        }

        [Test]
        public void CanListAvailableFrameworks()
        {
            RuntimeFramework[] available = RuntimeFramework.AvailableFrameworks;
            Assert.That(available, Has.Length.GreaterThan(0) );
            bool foundCurrent = false;
            foreach (RuntimeFramework framework in available)
            {
                Console.WriteLine("Available: {0}", framework.DisplayName);
                foundCurrent |= RuntimeFramework.CurrentFramework.MatchesClr(framework);
            }
            Assert.That(foundCurrent, "CurrentFramework not listed");
        }

        [TestCase(RuntimeType.Net, "1.0", "1.0.3705")]
        [TestCase(RuntimeType.Net, "1.0.3705", "1.0.3705")]
        [TestCase(RuntimeType.Net, "1.1", "1.1.4322")]
        [TestCase(RuntimeType.Net, "1.1.4322", "1.1.4322")]
        [TestCase(RuntimeType.Net, "2.0", "2.0.50727")]
        [TestCase(RuntimeType.Net, "2.0.40607", "2.0.40607")]
        [TestCase(RuntimeType.Net, "2.0.50727", "2.0.50727")]
        [TestCase(RuntimeType.Net, "3.0", "2.0.50727")]
        [TestCase(RuntimeType.Net, "3.5", "2.0.50727")]
        [TestCase(RuntimeType.Net, "4.0", "4.0.21006")]
        [TestCase(RuntimeType.Mono, "1.0", "1.1.4322")]
        [TestCase(RuntimeType.Mono, "2.0", "2.0.50727")]
        [TestCase(RuntimeType.Mono, "2.0.50727", "2.0.50727")]
        [TestCase(RuntimeType.Mono, "3.5", "2.0.50727")]
        public void CanCreateNewRuntimeFramework(RuntimeType runtime, string v, string clr)
        {
            Version frameworkVersion = new Version(v);
            Version clrVersion = new Version(clr);
            RuntimeFramework framework = new RuntimeFramework(runtime, frameworkVersion);
            Assert.AreEqual(runtime, framework.Runtime);
            Assert.AreEqual(clrVersion, framework.ClrVersion);
        }

        [TestCaseSource("matchData")]
        public bool CanMatchRuntimes(RuntimeFramework f1, RuntimeFramework f2)
        {
            return f1.MatchesClr(f2);
        }

        static TestCaseData[] matchData = new TestCaseData[] {
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0))) 
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0,50727))) 
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0,50727)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0))) 
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0,50727)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0))) 
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(3,5)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0))) 
                .Returns(true),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0)), 
                new RuntimeFramework(RuntimeType.Mono, new Version(2,0))) 
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0)), 
                new RuntimeFramework(RuntimeType.Net, new Version(1,1))) 
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Net, new Version(2,0,50727)), 
                new RuntimeFramework(RuntimeType.Net, new Version(2,0,40607))) 
                .Returns(false),
            new TestCaseData(
                new RuntimeFramework(RuntimeType.Mono, new Version(1,1)), // non-existent version but it works
                new RuntimeFramework(RuntimeType.Mono, new Version(1,0))) 
                .Returns(true)
            };
    }
}

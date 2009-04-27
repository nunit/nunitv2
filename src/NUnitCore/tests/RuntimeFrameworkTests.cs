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
            Assert.That(framework.Version, Is.EqualTo(Environment.Version));
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
                foundCurrent |= RuntimeFramework.CurrentFramework.Matches(framework);
            }
            Assert.That(foundCurrent, "CurrentFramework not listed");
        }
    }
}

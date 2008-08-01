using System;
using System.Diagnostics;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
    [TestFixture]
    public class RemoteTestAgentTests
    {
        [Test]
        public void AgentReturnsProcessId()
        {
            RemoteTestAgent agent = new RemoteTestAgent("dummy");
            Assert.AreEqual(Process.GetCurrentProcess().Id, agent.ProcessId);
        }

        [Test]
        public void CanLocateAgentExecutable()
        {
            string path = TestAgency.TestAgentExePath;
            Assert.That(System.IO.File.Exists(path), "Cannot find " + path);
        }
    }
}

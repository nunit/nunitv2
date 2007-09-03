using System;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class TestAgentTests
	{
		[Test]
		public void CanCreateAgent()
		{
			TestAgent agent = new TestAgent(123);
			Assert.That(agent.ID == 123);
		}

		[Test]
		public void AgentRegistersOnStart()
		{
			using( TestAgentManager registry = new TestAgentManager("TestAgentManagerForTesting", 9200))
			{
				registry.Start();
				Assert.IsNull( registry.GetTestRunner(123) );

				TestAgent agent = new TestAgent(123, "tcp://localhost:9200/TestAgentManagerForTesting");
				agent.Start();

				Assert.IsNotNull( registry.GetTestRunner(123) );
			}
		}
	}
}

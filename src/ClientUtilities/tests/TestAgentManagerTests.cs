using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class TestAgentManagerTests
	{
		[Test]
		public void CanConnectToRegistry()
		{
			using ( TestAgentManager registry = new TestAgentManager("TestAgentManagerForTesting", 9200) )
			{
				registry.Start();

				//ChannelServices.RegisterChannel( ServerUtilities.GetTcpChannel( "TestAgentManagerTestChannel" ) );

				object obj = Activator.GetObject( typeof( TestAgentManager ), "tcp://127.0.0.1:9200/TestAgentManagerForTesting" );
				Assert.IsNotNull( obj );
				Assert.That( obj is TestAgentManager );
			}
		}

		[Test]
		public void RegistryStoresRunnersById()
		{
			object o = new object();

			using ( TestAgentManager registry = new TestAgentManager("TestAgentManagerForTesting", 9200) )
			{
				registry.Start();
				registry.Register( o, 911 );
				Assert.AreEqual( o, registry.GetTestRunner(911) );
			}
		}
	}
}

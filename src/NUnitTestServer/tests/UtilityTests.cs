using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using NUnit.Framework;

namespace NUnit.TestServer.Tests
{
	/// <summary>
	/// Summary description for RemotingUtilitiesTests.
	/// </summary>
	[TestFixture]
	public class UtilityTests
	{
		private static readonly string channelName = "test";

		[Test]
		public void GetTcpChannel()
		{
			Hashtable props = new Hashtable();
			props.Add( "port", 0 );
			props.Add( "name", channelName );
			IChannel channel = ServerUtilities.GetTcpChannel( props );
			Assert.AreEqual( channelName, channel.ChannelName );
		}
	}
}

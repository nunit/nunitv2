// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.TestServer.Tests
{
	/// <summary>
	/// Summary description for TestServerTests.
	/// </summary>
	[TestFixture, Ignore( "Stack overflow problem" )]
	public class TestServerTests
	{
		[Test]
		public void CanConnect()
		{
			using( TestServer server = new TestServer( "TestServer" ) )
			{
				server.Start();
//				object obj = Activator.GetObject( typeof(TestRunner), "tcp://localhost:9000/TestServer" );
//				Assert.IsNotNull( obj, "Unable to connect" );
			}
		}

//		[Test]
		public void CanConnectOutOfProcess()
		{
			Process.Start( "nunit-server.exe", "TestServer" );
			System.Threading.Thread.Sleep( 1000 );
			object obj = Activator.GetObject( typeof(TestRunner), "tcp://localhost:9000/TestServer" );
			Assert.IsNotNull( obj, "Unable to connect" );
		}
	}
}

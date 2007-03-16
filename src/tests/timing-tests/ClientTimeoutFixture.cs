// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace NUnit.Tests.TimingTests
{
	/// <summary>
	/// Summary description for ClientTimeoutFixture.
	/// </summary>
	[TestFixture]
	public class ClientTimeoutFixture
	{
		// Test using timeout greater than default of five minutes
		private readonly TimeSpan timeout = TimeSpan.FromMinutes( 6 );

		/// <summary>
		/// Test that listener is connected after
		/// a long delay. When run from GUI, this
		/// tests UIActions. When run from console
		/// runner it tests ConsoleUI.
		/// </summary>
		[Test]
		public void ListenerTimeoutTest()
		{
			// Time delay
			Thread.Sleep( timeout );
		}

		[Test]
		public void WriterTimeoutTest()
		{
			// Time delay
			Thread.Sleep( timeout );

			Console.WriteLine( "Message from WriterTimeoutTest" );
		}
	}
}

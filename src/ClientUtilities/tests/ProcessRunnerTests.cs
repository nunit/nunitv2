// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System.Diagnostics;
using System.IO;
using NUnit.Core;
using NUnit.Core.Tests;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for ProcessRunnerTests.
	/// </summary>
	// TODO: Reinstate after release is complete
	//[TestFixture, Explicit]
	public class ProcessRunnerTests : BasicRunnerTests
	{
		private ProcessRunner myRunner;

		protected override TestRunner CreateRunner( int runnerID )
		{
			myRunner = new ProcessRunner( runnerID );
			myRunner.Start();
			return myRunner;
		}

		[TearDown]
		public void StopServer()
		{
			if ( myRunner != null )
				myRunner.Stop();
		}

		[TestFixtureSetUp]
		public void CheckServerAvailable()
		{
			Assert.IsTrue( File.Exists( "nunit-server.exe" ), "Can't find server" );
		}

		[Test]
		public void CreatedRunnerInSeparateProcess()
		{
			Assert.AreNotEqual( Process.GetCurrentProcess().Id, myRunner.Process.Id, 
				"Runner is in same process" );
		}
	}
}

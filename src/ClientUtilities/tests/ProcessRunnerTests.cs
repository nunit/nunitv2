using System;
using System.IO;
using System.Diagnostics;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Core.Tests;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for ProcessRunnerTests.
	/// </summary>
#if DEBUG
	[TestFixture]
#endif
	public class ProcessRunnerTests : BasicRunnerTests
	{
		private ProcessRunner myRunner;

		public override TestRunner CreateRunner()
		{
			myRunner = new ProcessRunner();
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

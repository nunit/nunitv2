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

// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core.Tests
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Threading;
	using NUnit.Framework;

	[TestFixture]
	public class AssemblyTests 
	{
		private string thisDll;

		[SetUp]
		public void InitStrings()
		{
			thisDll = this.GetType().Module.Name;
		}

		// TODO: Review and remove unnecessary tests

		[Test]
		public void RunSetsCurrentDirectory()
		{
			Assert.IsTrue( File.Exists( thisDll ), "Run does not set current directory" );
		}

		[Test]
		public void LoadAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test suite = builder.Build( new TestPackage( thisDll ) );
			Assert.IsNotNull( suite );
			//Assert.IsNotNull(testAssembly, "should be able to load assembly");
			Assert.IsTrue( File.Exists( thisDll ), "Load does not set current Directory" );
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void LoadAssemblyNotFound()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			builder.Build( new TestPackage( "XXXX.dll" ) );
		}

		[Test]
		public void LoadAssemblyWithoutTestFixtures()
		{
			string fileName = "notestfixtures-assembly.dll";
			TestSuiteBuilder builder = new TestSuiteBuilder();
			Test suite = builder.Build( new TestPackage( fileName ) );
			Assert.IsNotNull( suite,"Should not be null" );
			Assert.AreEqual( RunState.NotRunnable, suite.RunState );
			Assert.AreEqual( suite.IgnoreReason, "Has no TestFixtures" );
			Assert.AreEqual( 0, suite.Tests.Count );
		}

		[Test]
		public void LoadTestFixtureFromAssembly()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			TestPackage package = new TestPackage( thisDll );
			package.TestName = this.GetType().FullName;
			Test suite= builder.Build( package );
			Assert.IsNotNull(suite, "Should not be Null");
		}

		[Test]
		public void AppSettingsLoaded()
		{
			string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			Assert.IsTrue( File.Exists( configFile ), string.Format( "{0} does not exist", configFile ) );
			Assert.IsNull(ConfigurationSettings.AppSettings["tooltip.ShowAlways"]);
			Assert.AreEqual("54321",ConfigurationSettings.AppSettings["test.setting"]);
		}
	}
}

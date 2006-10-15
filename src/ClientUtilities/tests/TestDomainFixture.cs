#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class TestDomainFixture
	{
		private static TestDomain testDomain; 
		private static ITest loadedTest;

		[TestFixtureSetUp]
		public void MakeAppDomain()
		{
			testDomain = new TestDomain();
			testDomain.Load( "mock-assembly.dll" );
			loadedTest = testDomain.Test;
		}

		[TestFixtureTearDown]
		public void UnloadTestDomain()
		{
			loadedTest = null;
			//testDomain.Unload(); //TEMP
			testDomain = null;
		}
			
		[Test]
		public void AssemblyIsLoadedCorrectly()
		{
			Assert.IsNotNull(loadedTest, "Test not loaded");
			Assert.AreEqual(MockAssembly.Tests, loadedTest.TestCount );
		}

		[Test]
		public void AppDomainIsSetUpCorrectly()
		{
			AppDomain domain = testDomain.AppDomain;
			AppDomainSetup setup = testDomain.AppDomain.SetupInformation;
			
			Assert.AreEqual( "Tests", setup.ApplicationName, "ApplicationName" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.ApplicationBase, "ApplicationBase" );
			Assert.AreEqual( "mock-assembly.dll.config", Path.GetFileName( setup.ConfigurationFile ), "ConfigurationFile" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.PrivateBinPath, "PrivateBinPath" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.ShadowCopyDirectories, "ShadowCopyDirectories" );

			Assert.AreEqual( Environment.CurrentDirectory, domain.BaseDirectory, "BaseDirectory" );
			Assert.AreEqual( "domain-mock-assembly.dll", domain.FriendlyName, "FriendlyName" );
			Assert.IsTrue( testDomain.AppDomain.ShadowCopyFiles, "ShadowCopyFiles" );
		}	

		[Test]
		public void CanRunMockAssemblyTests()
		{
			TestResult result = testDomain.Run( NullListener.NULL );
			Assert.IsNotNull(result);
			Assert.AreEqual(false, result.IsFailure, "Test run failed");
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(MockAssembly.Tests - MockAssembly.NotRun, summarizer.ResultCount);
			Assert.AreEqual(MockAssembly.Ignored, summarizer.TestsNotRun);
		}
	}

	[TestFixture]
	public class TestDomainRunnerTests : NUnit.Core.Tests.BasicRunnerTests
	{
		protected override TestRunner CreateRunner(int runnerID)
		{
			return new TestDomain(runnerID);
		}

	}

	[TestFixture]
	public class TestDomainTests
	{ 
		private static TestDomain testDomain;

		[SetUp]
		public void SetUp()
		{
			testDomain = new TestDomain();
		}

		[TearDown]
		public void TearDown()
		{
			testDomain.Unload();
		}

		[Test]
		[Platform(Exclude="Linux")]
		public void BinPath()
		{
			string[] assemblies = new string[]
				{ @"h:\app1\bin\debug\test1.dll", @"h:\app2\bin\debug\test2.dll", @"h:\app1\bin\debug\test3.dll" };

			Assert.AreEqual( @"h:\app1\bin\debug;h:\app2\bin\debug", 
				TestDomain.GetBinPath( assemblies ) );
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void FileNotFound()
		{
			testDomain.Load("xxxx");
		}

		[Test]
		public void InvalidTestFixture()
		{
			Assert.IsFalse( testDomain.Load( "mock-assembly.dll", "NUnit.Tests.Assemblies.Bogus" ) );
		}

		// Doesn't work under .NET 2.0 Beta 2
		//[Test]
		//[ExpectedException(typeof(BadImageFormatException))]
		public void FileFoundButNotValidAssembly()
		{
			string badfile = "x.dll";
			//FileInfo file = new FileInfo( badfile );
			try
			{
				StreamWriter sw = new StreamWriter( badfile );
				//StreamWriter sw = file.AppendText();

				sw.WriteLine("This is a new entry to add to the file");
				sw.WriteLine("This is yet another line to add...");
				sw.Flush();
				sw.Close();
				testDomain.Load( badfile );
			}
			finally
			{
				if ( File.Exists( badfile ) )
					File.Delete( badfile );
			}

		}

		[Test]
		public void SpecificTestFixture()
		{
			testDomain.Load( "mock-assembly.dll", "NUnit.Tests.Assemblies.MockTestFixture" );

			TestResult result = testDomain.Run( NullListener.NULL );
			Assert.AreEqual(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(MockTestFixture.Tests - MockTestFixture.NotRun, summarizer.ResultCount);
			Assert.AreEqual(MockTestFixture.Ignored, summarizer.TestsNotRun);
		}

		[Test]
		public void ProjectConfigFileOverrideIsHonored()
		{
			NUnitProject project = new NUnitProject( "MyProject.nunit" );
			ProjectConfig config = new ProjectConfig( "default" );
			config.Assemblies.Add( Path.GetFullPath( "mock-assembly.dll" ) );
			project.Configs.Add( config );
			project.ActiveConfig.ConfigurationFile = "Override.config";

			testDomain.Load( project );

			Assert.AreEqual( "Override.config", 
				Path.GetFileName( testDomain.AppDomain.SetupInformation.ConfigurationFile ) );
		}

		[Test]
		public void ProjectBinPathOverrideIsHonored()
		{
			NUnitProject project = new NUnitProject( "MyProject.nunit" );
			ProjectConfig config = new ProjectConfig( "default" );
			config.Assemblies.Add( Path.GetFullPath( "mock-assembly.dll" ) );
			project.Configs.Add( config );
			project.ActiveConfig.PrivateBinPath = "dummy;junk";

			testDomain.Load( project );

			Assert.AreEqual( "dummy;junk", 
				testDomain.AppDomain.SetupInformation.PrivateBinPath );
		}

		// Turning off shadow copy only works when done for the primary app domain
		// So this test can only work if it's already off
		// This doesn't seem to be documented anywhere
		[Test]
		public void TurnOffShadowCopy()
		{
			testDomain.Settings["ShadowCopyFiles"] = false;
			testDomain.Load( "mock-assembly.dll" );
			Assert.IsFalse( testDomain.AppDomain.ShadowCopyFiles );
					
//			// Prove that shadow copy is really off
//			string location = "NOT_FOUND";
//			foreach( Assembly assembly in testDomain.AppDomain.GetAssemblies() )
//			{
//				if ( assembly.FullName.StartsWith( "mock-assembly" ) )
//				{
//					location = Path.GetDirectoryName( assembly.Location );
//					break;
//				}
//			}
//		
//			//TODO: Find a non-platform-dependent way to do this
//			Assert.AreEqual( Environment.CurrentDirectory.ToLower(), location.ToLower() );
		}
	}
}

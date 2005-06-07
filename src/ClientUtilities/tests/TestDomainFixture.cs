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
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.Tests
{
	[TestFixture]
	public class TestDomainFixture
	{
		private static TestDomain testDomain; 
		private static Test loadedTest;

		[TestFixtureSetUp]
		public void MakeAppDomain()
		{
			testDomain = new TestDomain();

			loadedTest = testDomain.Load( "mock-assembly.dll" );
		}

		[TestFixtureTearDown]
		public void UnloadTestDomain()
		{
			loadedTest = null;
			testDomain.Unload();
			testDomain = null;
		}
			
		[Test]
		public void LoadAssembly()
		{
			Assert.IsNotNull(loadedTest, "Test not loaded");
		}

		[Test]
		public void AppDomainSetup()
		{
			AppDomain domain = testDomain.AppDomain;
			AppDomainSetup setup = testDomain.AppDomain.SetupInformation;
			
			Assert.AreEqual( "Tests", setup.ApplicationName, "ApplicationName" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.ApplicationBase, "ApplicationBase" );
			Assert.AreEqual( Path.GetFullPath( "mock-assembly.dll.config" ), setup.ConfigurationFile, "ConfigurationFile" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.PrivateBinPath, "PrivateBinPath" );
			Assert.AreEqual( Environment.CurrentDirectory, setup.ShadowCopyDirectories, "ShadowCopyDirectories" );

			Assert.AreEqual( Environment.CurrentDirectory, domain.BaseDirectory, "BaseDirectory" );
			Assert.AreEqual( "domain-mock-assembly.dll", domain.FriendlyName, "FriendlyName" );
			Assert.IsTrue( testDomain.AppDomain.ShadowCopyFiles, "ShadowCopyFiles" );
		}

		// Turning off shadow copy only works when done for the primary app domain
		// So this test can only work if it's already off
		// This doesn't seem to be documented anywhere
		//		[Test]
		//		public void TurnOffShadowCopy()
		//		{
		//			testDomain.ShadowCopyFiles = false;
		//			testDomain.Load( "mock-assembly.dll" );
		//			Assert.IsFalse( testDomain.AppDomain.ShadowCopyFiles );
		//			
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
		//		}

		

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TurnOffShadowCopyFailsAfterLoad()
		{
			testDomain.ShadowCopyFiles = false;
		}

		[Test]
		public void CountTestCases()
		{
			Assert.AreEqual(MockAssembly.Tests, loadedTest.CountTestCases());
		}

		[Test]
		public void RunMockAssembly()
		{
			TestResult result = testDomain.Run( NullListener.NULL );
			Assert.IsNotNull(result);
		}

		[Test]
		public void MockAssemblyResults()
		{
			TestResult result = testDomain.Run( NullListener.NULL );
			Assert.AreEqual(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(MockAssembly.Tests - MockAssembly.NotRun, summarizer.ResultCount);
			Assert.AreEqual(MockAssembly.NotRun, summarizer.TestsNotRun);
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

		[Test]
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
			Test test = testDomain.Load("xxxx");
		}

		[Test]
		public void InvalidTestFixture()
		{
			Test test = testDomain.Load( "mock-assembly.dll", "NUnit.Tests.Assemblies.Bogus" );
			Assert.IsNull(test, "test should be null");
		}
		[Test]
		[ExpectedException(typeof(BadImageFormatException))]
		public void FileFoundButNotValidAssembly()
		{
			FileInfo file = new FileInfo( "x.dll" );
			try
			{
				StreamWriter sw = file.AppendText();

				sw.WriteLine("This is a new entry to add to the file");
				sw.WriteLine("This is yet another line to add...");
				sw.Flush();
				sw.Close();
				testDomain.Load( "x.dll" );
			}
			finally
			{
				if ( file.Exists )
					file.Delete();
			}

		}

		[Test]
		public void SpecificTestFixture()
		{
			Test test = testDomain.Load( "mock-assembly.dll", "NUnit.Tests.Assemblies.MockTestFixture" );

			TestResult result = testDomain.Run( NullListener.NULL );
			Assert.AreEqual(true, result.IsSuccess);
			
			ResultSummarizer summarizer = new ResultSummarizer(result);
			Assert.AreEqual(MockTestFixture.Tests - MockTestFixture.NotRun, summarizer.ResultCount);
			Assert.AreEqual(MockTestFixture.NotRun, summarizer.TestsNotRun);
		}
	}
}

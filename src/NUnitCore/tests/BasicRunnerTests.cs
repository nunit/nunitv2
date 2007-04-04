// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;

using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Base class for tests of various kinds of runners
	/// </summary>
	public abstract class BasicRunnerTests
	{
		private static readonly string testsDll = "nonamespace-assembly.dll";
		private static readonly string mockDll = "mock-assembly.dll";
		private readonly string[] assemblies = new string[] { testsDll, mockDll };

		private TestRunner runner;

		[SetUp]
		public void SetUpRunner()
		{
			runner = CreateRunner( 123 );
		}

        [TearDown]
        public void TearDownRunner()
        {
            if (runner != null)
                runner.Unload();
        }

		protected abstract TestRunner CreateRunner( int runnerID );

        [Test]
        public void CheckRunnerID()
        {
            Assert.AreEqual(123, runner.ID);
        }

        [Test]
		public void LoadAssembly() 
		{
			Assert.IsTrue(runner.Load( new TestPackage( mockDll ) ), "Unable to load assembly" );
		}

		[Test]
		public void LoadAssemblyWithoutNamespaces()
		{
			TestPackage package = new TestPackage( mockDll );
			package.Settings["AutoNamespaceSuites"] = false;
			Assert.IsTrue(runner.Load( package ), "Unable to load assembly" );
			ITest test = runner.Test;
			Assert.IsNotNull( test );
			Assert.AreEqual( MockAssembly.Fixtures, test.Tests.Count );
			Assert.AreEqual( "MockTestFixture", ((ITest)test.Tests[0]).TestName.Name );
		}

		[Test]
		public void LoadAssemblyWithFixture()
		{
			TestPackage package = new TestPackage( mockDll );
			package.TestName = "NUnit.Tests.Assemblies.MockTestFixture";
			Assert.IsTrue( runner.Load( package ) );
		}

		[Test]
		public void LoadAssemblyWithSuite()
		{
			TestPackage package = new TestPackage( mockDll );
			package.TestName = "NUnit.Tests.Assemblies.MockSuite";
			runner.Load( package );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void CountTestCases()
		{
			runner.Load( new TestPackage( mockDll ) );
			Assert.AreEqual( MockAssembly.Tests, runner.Test.TestCount );
		}

		[Test]
		public void LoadMultipleAssemblies()
		{
			runner.Load( new TestPackage( "TestSuite", assemblies ) );
			Assert.IsNotNull( runner.Test, "Unable to load assemblies" );
		}

		[Test]
		public void LoadMultipleAssembliesWithFixture()
		{
			TestPackage package = MakePackage( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockTestFixture" );
			runner.Load( package );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void LoadMultipleAssembliesWithSuite()
		{
			TestPackage package = MakePackage( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockSuite" );
			runner.Load( package );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void CountTestCasesAcrossMultipleAssemblies()
		{
			runner.Load( new TestPackage( "TestSuite", assemblies ) );
			Assert.AreEqual( NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				runner.Test.TestCount );			
		}

		[Test]
		public void RunAssembly()
		{
			runner.Load( new TestPackage( mockDll ) );
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunAssemblyUsingBeginAndEndRun()
		{
			runner.Load( new TestPackage( mockDll ) );
			runner.BeginRun( NullListener.NULL );
			TestResult result = runner.EndRun();
			Assert.IsNotNull( result );
			ResultSummarizer summary = new ResultSummarizer( result );
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			runner.Load( new TestPackage( "TestSuite", assemblies ) );
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}

		[Test]
		public void RunMultipleAssembliesUsingBeginAndEndRun()
		{
			runner.Load( new TestPackage( "TestSuite", assemblies ) );
			runner.BeginRun( NullListener.NULL );
			TestResult result = runner.EndRun();
			Assert.IsNotNull( result );
			ResultSummarizer summary = new ResultSummarizer( result );
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}

		private TestPackage MakePackage( string name, IList assemblies, string testName )
		{
			TestPackage package = new TestPackage( name, assemblies );
			package.TestName = testName;

			return package;
		}
	}
}

using System;

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
			runner = CreateRunner();
		}

		public abstract TestRunner CreateRunner();

		[Test]
		public void LoadAssembly() 
		{
			TestNode test = runner.Load(testsDll);
			Assert.IsNotNull(test, "Unable to load assembly" );
		}

		[Test]
		public void LoadAssemblyWithFixture()
		{
			TestNode test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadAssemblyWithSuite()
		{
			TestNode test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCases()
		{
			TestNode test = runner.Load( mockDll );
			Assert.AreEqual( MockAssembly.Tests, test.TestCount );
		}

		[Test]
		public void LoadMultipleAssemblies()
		{
			TestNode test = runner.Load( "TestSuite", assemblies );
			Assert.IsNotNull( test, "Unable to load assemblies" );
		}

		[Test]
		public void LoadMultipleAssembliesWithFixture()
		{
			TestNode test = runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockTestFixture"  );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadMultipleAssembliesWithSuite()
		{
			TestNode test = runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCasesAcrossMultipleAssemblies()
		{
			TestNode test = runner.Load( "TestSuite", assemblies );
			Assert.AreEqual( NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				test.TestCount );			
		}

		[Test]
		public void RunAssembly()
		{
			TestNode test = runner.Load(mockDll);
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunAssemblyUsingBeginAndEndRun()
		{
			TestNode test = runner.Load(mockDll);
			runner.BeginRun( NullListener.NULL );
			TestResult[] results = runner.EndRun();
			Assert.IsNotNull( results );
			Assert.AreEqual( 1, results.Length );
			ResultSummarizer summary = new ResultSummarizer( results[0] );
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestNode test = runner.Load( "TestSuite", assemblies );
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}

		[Test]
		public void RunMultipleAssembliesUsingBeginAndEndRun()
		{
			TestNode test = runner.Load( "TestSuite", assemblies );
			runner.BeginRun( NullListener.NULL );
			TestResult[] results = runner.EndRun();
			Assert.IsNotNull( results );
			Assert.AreEqual( 1, results.Length );
			ResultSummarizer summary = new ResultSummarizer( results[0] );
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}
	}
}

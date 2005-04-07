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
			Test test = runner.Load(testsDll);
			Assert.IsNotNull(test, "Unable to load assembly" );
		}

		[Test]
		public void LoadAssemblyWithFixture()
		{
			Test test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockTestFixture" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadAssemblyWithSuite()
		{
			Test test = runner.Load( mockDll, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCases()
		{
			Test test = runner.Load( mockDll );
			Assert.AreEqual( MockAssembly.Tests, test.CountTestCases() );
		}

		[Test]
		public void LoadMultipleAssemblies()
		{
			Test test = runner.Load( new TestProject( "TestSuite", assemblies ) );
			Assert.IsNotNull( test, "Unable to load assemblies" );
		}

		[Test]
		public void LoadMultipleAssembliesWithFixture()
		{
			Test test = runner.Load( new TestProject( "TestSuite", assemblies), "NUnit.Tests.Assemblies.MockTestFixture"  );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void LoadMultipleAssembliesWithSuite()
		{
			Test test = runner.Load( new TestProject( "TestSuite", assemblies ), "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(test, "Unable to build suite");
		}

		[Test]
		public void CountTestCasesAcrossMultipleAssemblies()
		{
			assemblies[0] = "nonamespace-assembly.dll";
			Test test = runner.Load( new TestProject( "TestSuite", assemblies ) );
			Assert.AreEqual( NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				test.CountTestCases() );			
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			assemblies[0] = "nonamespace-assembly.dll";
			Test test = runner.Load( new TestProject( "TestSuite", assemblies ) );
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}
	}
}

using System;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Tests of BaseTestRunner
	/// </summary>
	[TestFixture]
	public class BaseTestRunnerTests
	{
		private readonly string testsDll = "nonamespace-assembly.dll";
		private readonly string mockDll = "mock-assembly.dll";
		private TestRunner runner;
		private string[] assemblies;

		[SetUp]
		public void CreateRunner()
		{
			runner = new BaseTestRunner();
			assemblies = new string[] { testsDll, mockDll };
		}

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
			RecordingListener recorder = new RecordingListener();
			assemblies[0] = "nonamespace-assembly.dll";
			Test test = runner.Load( new TestProject( "TestSuite", assemblies ) );
			TestResult result = runner.Run( recorder );
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				recorder.testStarted.Count );
		}
	}
}

using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests
{
	[TestFixture]
	public class MultipleAssembliesTest
	{
		private readonly string testsDll = "nonamespace-assembly.dll";
		private readonly string mockDll = "mock-assembly.dll";

		private TestSuite suite;

		[SetUp]
		public void LoadSuite()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();

			ArrayList assemblies = new ArrayList();
			assemblies.Add(testsDll);
			assemblies.Add(mockDll);

			suite = builder.Build( "TestSuite", assemblies);
		}

		[Test]
		public void BuildSuite()
		{
			Assert.NotNull(suite);
		}

		[Test]
		public void RootNode()
		{
			Assert.True( suite is RootTestSuite );
			Assert.Equals( "TestSuite", suite.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			Assert.True( suite.Tests[0] is AssemblyTestSuite );
			Assert.True( suite.Tests[1] is AssemblyTestSuite );
		}

		[Test]
		public void TestCaseCount()
		{
			Assert.Equals(10, suite.CountTestCases);
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = suite.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.Equals(8, summary.ResultCount);
		}

	}
}

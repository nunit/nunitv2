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

			suite = builder.Build(assemblies);
		}

		[Test]
		public void BuildSuite()
		{
			Assertion.AssertNotNull(suite);
		}

		[Test]
		public void TestCaseCount()
		{
			Assertion.AssertEquals(10, suite.CountTestCases);
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = suite.Run(NullListener.NULL);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assertion.AssertEquals(8, summary.ResultCount);
		}

	}
}

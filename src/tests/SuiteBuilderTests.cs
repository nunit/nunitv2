using System;
using Nunit.Framework;
using Nunit.Core;

namespace Nunit.Tests
{
	/// <summary>
	/// Summary description for SuiteBuilderTests.
	/// </summary>
	/// 
	[TestFixture]
	public class SuiteBuilderTests
	{
		private string testsDll = "Nunit.Tests.dll";

		[Test]
		public void LoadTestSuiteFromAssembly()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.AllTests", testsDll);
			Assertion.Assert(suite != null);
		}

		[Test]
		public void BuildTestSuiteFromAssembly() 
		{
			TestSuite suite = TestSuiteBuilder.Build(testsDll);
			Assertion.Assert(suite != null);
		}


		class Suite
		{
			[Suite]
			public static TestSuite MockSuite
			{
				get 
				{
					TestSuite testSuite = new TestSuite("TestSuite");
					return testSuite;
				}
			}
		}

		[Test]
		public void DiscoverSuite()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.SuiteBuilderTests+Suite",testsDll);
			Assertion.AssertNotNull("Could not discover suite attribute",suite);
		}

		class NonConformingSuite
		{
			[Suite]
			public static int Integer
			{
				get 
				{
					return 5;
				}
			}
		}

		[Test]
		public void WrongReturnTypeSuite()
		{
			TestSuite suite = TestSuiteBuilder.Build("Nunit.Tests.assemblies.AssemblyTests+NonConformingSuite",testsDll);
			Assertion.AssertNull("Suite propertye returns wrong type",suite);
		}
	}
}

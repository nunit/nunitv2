using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;


namespace NUnit.Tests.Attributes
{
	[TestFixture(Description = "Fixture Description")]
	internal class MockFixture
	{
		[Test(Description = "Test Description")]
		public void Method()
		{}

		[Test]
		public void NoDescriptionMethod()
		{}
	}

	internal class DescriptionVisitor : ResultVisitor
	{
		private string name; 
		private string description;

		public DescriptionVisitor(string name, string description)
		{
			this.name = name;
			this.description = description;
		}

		public void Visit(TestCaseResult caseResult)
		{
			if(caseResult.Name.Equals(name))
				Assert.AreEqual(description, caseResult.Description);
		}

		public void Visit(TestSuiteResult suiteResult)
		{
			Console.WriteLine(suiteResult.Name);
			if(suiteResult.Name.Equals(name))
				Assert.AreEqual(description, suiteResult.Description);

			foreach (TestResult result in suiteResult.Results)
			{
				result.Accept(this);
			}
		}
	}


	[TestFixture]
	public class TestAttributeFixture
	{
		MockFixture fixture = new MockFixture();
		NUnit.Core.TestCase testCase; 

		[SetUp]
		public void Init()
		{
			testCase = TestCaseBuilder.Make(fixture, "Method");
		}

		[Test]
		public void ReflectionTest()
		{
			Assert.IsTrue(testCase.ShouldRun);
		}

		[Test]
		public void Description()
		{
			Assert.AreEqual("Test Description", testCase.Description);
		}

		[Test]
		public void DescriptionInResult()
		{
			TestSuite suite = new TestSuite("Mock Fixture");
			suite.Add(fixture);
			TestResult result = suite.Run(NullListener.NULL);

			DescriptionVisitor visitor = new DescriptionVisitor("NUnit.Tests.Attributes.MockFixture.Method", "Test Description");
			result.Accept(visitor);

			visitor = 
				new DescriptionVisitor("NUnit.Tests.Attributes.MockFixture.NoDescriptionMethod", null);
			result.Accept(visitor);
		}

		[Test]
		public void NoDescription()
		{
			NUnit.Core.TestCase testCase = TestCaseBuilder.Make(fixture, "NoDescriptionMethod");
			Assert.IsNull(testCase.Description);
		}

		[Test]
		public void FixtureDescription()
		{
			NUnit.Core.TestSuite suite = new TestSuite("suite");
			suite.Add(new MockFixture());

			ArrayList tests = suite.Tests;
			TestSuite mockFixtureSuite = (TestSuite)tests[0];

			Assert.AreEqual("Fixture Description", mockFixtureSuite.Description);
		}

		[Test]
		public void FixtureDescriptionInResult()
		{
			TestSuite suite = new TestSuite("Mock Fixture");
			suite.Add(fixture);
			TestResult result = suite.Run(NullListener.NULL);

			DescriptionVisitor visitor = new DescriptionVisitor("MockFixture", "Fixture Description");
			result.Accept(visitor);
		}
	}
}

// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Reflection;
using System.Collections;
using NUnit.Framework;
using NUnit.Core.Builders;
using NUnit.TestUtilities;
using NUnit.TestData.AttributeDescriptionFixture;

namespace NUnit.Core.Tests
{
	// TODO: Review to see if we need these tests

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
		static readonly Type FixtureType = typeof( MockFixture );

		[Test]
		public void ReflectionTest()
		{
			TestCase testCase = TestBuilder.MakeTestCase( FixtureType, "Method" );
			Assert.AreEqual( RunState.Runnable, testCase.RunState );
		}

        [Test]
        public void Description()
        {
            TestCase testCase = TestBuilder.MakeTestCase(FixtureType, "Method");
            Assert.AreEqual("Test Description", testCase.Description);
        }

        [Test]
        public void DescriptionInResult()
        {
            TestSuite suite = new TestSuite("Mock Fixture");
            suite.Add(TestBuilder.MakeFixture(typeof(MockFixture)));
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
			TestCase testCase = TestBuilder.MakeTestCase( FixtureType, "NoDescriptionMethod" );
			Assert.IsNull(testCase.Description);
		}

		[Test]
		public void FixtureDescription()
		{
			TestSuite suite = new TestSuite("suite");
			suite.Add( TestBuilder.MakeFixture( typeof( MockFixture ) ) );

			IList tests = suite.Tests;
			TestSuite mockFixtureSuite = (TestSuite)tests[0];

			Assert.AreEqual("Fixture Description", mockFixtureSuite.Description);
		}

		[Test]
		public void FixtureDescriptionInResult()
		{
			TestSuite suite = new TestSuite("Mock Fixture");
			suite.Add( TestBuilder.MakeFixture( typeof( MockFixture ) ) );
			TestResult result = suite.Run(NullListener.NULL);

			DescriptionVisitor visitor = new DescriptionVisitor("MockFixture", "Fixture Description");
			result.Accept(visitor);
		}

        [Test]
        public void SeparateDescriptionAttribute()
        {
            TestCase testCase = TestBuilder.MakeTestCase(FixtureType, "SeparateDescriptionMethod");
            Assert.AreEqual("Separate Description", testCase.Description);
        }

        [Test]
        public void SeparateDescriptionInResult()
        {
            TestSuite suite = new TestSuite("Mock Fixture");
            suite.Add(TestBuilder.MakeFixture(typeof(MockFixture)));
            TestResult result = suite.Run(NullListener.NULL);

            DescriptionVisitor visitor = new DescriptionVisitor("NUnit.Tests.Attributes.MockFixture.SeparateDescriptionMethod", "Separate Description");
            result.Accept(visitor);
        }

    }
}

// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnit.Core.Extensions.RowTest.UnitTests
{
	[TestFixture]
	public class RowTestCaseTest : BaseTestFixture
	{
		public RowTestCaseTest()
		{
		}
	
		[Test]
		public void Initialize()
		{
			MethodInfo method = GetRowTestMethodWith2Rows();
			object[] arguments = new object[] { 4, 5 };
			
			RowTestCase testCase = new RowTestCase(method, null, arguments);
			
			Assert.That(testCase.Arguments, Is.SameAs(arguments));
			Assert.That(testCase.Method, Is.SameAs(method));
			Assert.That(testCase.FixtureType, Is.SameAs(typeof (TestClass)));
		}
	
		[Test]
		public void Initialize_TestNameIsMethodName()
		{
			MethodInfo method = GetRowTestMethodWith2Rows();
			object[] arguments = new object[] { 4, 5 };
			
			RowTestCase testCase = new RowTestCase(method, null, arguments);

			string expectedTestName = Method_RowTestMethodWith2Rows + "(4, 5)";
			string expectedFullTestName = typeof(TestClass).FullName + "." + expectedTestName;
			Assert.That(testCase.TestName.Name, Is.EqualTo(expectedTestName));
			Assert.That(testCase.TestName.FullName, Is.EqualTo(expectedFullTestName));
		}
		
		[Test]
		public void Initialize_TestNameIsProvided()
		{
			MethodInfo method = GetRowTestMethodWith2Rows();
			object[] arguments = new object[] { 4, 5 };
			string testName = "UnitTest";
			
			RowTestCase testCase = new RowTestCase(method, testName, arguments);

			string expectedTestName = testName + "(4, 5)";
			string expectedFullTestName = typeof(TestClass).FullName + "." + expectedTestName;
			Assert.That(testCase.TestName.Name, Is.EqualTo(expectedTestName));
			Assert.That(testCase.TestName.FullName, Is.EqualTo(expectedFullTestName));
		}
		
		[Test]
		public void RunTestMethod_WithArguments()
		{
			object[] arguments = new object[] { 42, 53 };
			TestClass testFixture = new TestClass();
			RowTestCase testCase = CreateRowTestCase(testFixture, Method_RowTestMethodWith2Rows, arguments);
			TestResult result = new TestResult( testCase );
			
			testCase.RunTestMethod(result);
			
			Assert.That(testFixture.Arguments, Is.Not.Null);
			Assert.That(testFixture.Arguments[0], Is.EqualTo(arguments[0]));
			Assert.That(testFixture.Arguments[1], Is.EqualTo(arguments[1]));
		}

#if NET_2_0 || MONO_2_0
        [Test]
		public void RunTestMethod_WithNormalAndNullArguments()
		{
			object[] arguments = new object[] { 42, null };
			TestClass testFixture = new TestClass();
			RowTestCase testCase = CreateRowTestCase(testFixture, Method_RowTestMethodWithNormalAndNullArgument, arguments);
			TestResult result = new TestResult(testCase);
			
			testCase.RunTestMethod(result);
			
			Assert.That(testFixture.Arguments, Is.Not.Null);
			Assert.That(testFixture.Arguments[0], Is.EqualTo(arguments[0]));
			Assert.That(testFixture.Arguments[1], Is.Null);
		}
#endif
		
		[Test]
		public void RunTestMethod_WithNullArgument()
		{
			object[] arguments = new object[] { null };
			TestClass testFixture = new TestClass();
			RowTestCase testCase = CreateRowTestCase(testFixture, Method_RowTestMethodWithNullArgument, arguments);
			TestResult result = new TestResult(testCase);
			
			testCase.RunTestMethod(result);
			
			Assert.That(testFixture.Arguments, Is.Not.Null);
			Assert.That(testFixture.Arguments[0], Is.Null);
		}
		
		private RowTestCase CreateRowTestCase(TestClass fixture, string methodName, params object[] arguments)
		{
			MethodInfo method = GetTestClassMethod(methodName);
			
			NUnitTestFixture nunitTestFixture = new NUnitTestFixture(fixture.GetType());
			nunitTestFixture.Fixture = fixture;
			
			TestSuite suite = new TestSuite(nunitTestFixture.TestName.Name, method.Name);
			suite.Parent = nunitTestFixture;
			suite.Fixture = fixture;

			RowTestCase testCase = new RowTestCase(method, method.Name, arguments);
			testCase.Fixture = fixture;
			suite.Add(testCase);
			
			return testCase;
		}
	}
}

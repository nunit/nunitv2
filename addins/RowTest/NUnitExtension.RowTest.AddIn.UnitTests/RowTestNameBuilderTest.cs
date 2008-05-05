// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class RowTestNameBuilderTest : BaseTestFixture
	{
		[Test]
		public void Initialize()
		{
			MethodInfo method = GetRowTestMethodWith2Rows();
			object[] arguments = new object[] { 5, 6 };
			string baseTestName = "UnitTest";
			
			RowTestNameBuilder testNameBuilder = new RowTestNameBuilder(method, baseTestName, arguments);
			
			Assert.That(testNameBuilder.Method, Is.SameAs(method));
			Assert.That(testNameBuilder.BaseTestName, Is.EqualTo(baseTestName));
			Assert.That(testNameBuilder.Arguments, Is.SameAs(arguments));
		}
		
		[Test]
		public void TestName_IsMethodName()
		{
			RowTestNameBuilder testNameBuilder = CreateRowTestNameBuilder(GetRowTestMethodWith2Rows(), 5, 6);

			string testName = testNameBuilder.TestName;
			
			string expectedTestName = Method_RowTestMethodWith2Rows + "(5, 6)";
			Assert.That(testName, Is.EqualTo(expectedTestName));
		}
		
		[Test]
		public void TestName_IsProvided()
		{
			string providedTestName = "UnitTest";
			
			RowTestNameBuilder testNameBuilder =
					CreateRowTestNameBuilder(providedTestName, GetRowTestMethodWith2Rows(), 5, 6);

			string testName = testNameBuilder.TestName;
			
			string expectedTestName = providedTestName + "(5, 6)";
			Assert.That(testName, Is.EqualTo(expectedTestName));
		}
		
		[Test]
		public void TestName_ProvidedTestNameIsEmpty()
		{
			RowTestNameBuilder testNameBuilder = 
					CreateRowTestNameBuilder(string.Empty, GetRowTestMethodWith2Rows(), 5, 6);

			string testName = testNameBuilder.TestName;
			
			string expectedTestName = Method_RowTestMethodWith2Rows + "(5, 6)";
			Assert.That(testName, Is.EqualTo(expectedTestName));
		}
		
		[Test]
		public void FullTestName()
		{
			RowTestNameBuilder testNameBuilder = CreateRowTestNameBuilder(GetRowTestMethodWith2Rows(), 5, 6);

			string fullTestName = testNameBuilder.FullTestName;
			
			string expectedTestName = typeof(TestClass).FullName + "." + Method_RowTestMethodWith2Rows + "(5, 6)";
			Assert.That(fullTestName, Is.EqualTo(expectedTestName));
		}
		
		[Test]
		public void TestName_NullArgument()
		{
			RowTestNameBuilder testNameBuilder = CreateRowTestNameBuilder(GetRowTestMethodWithNullArgument(), null);
			
			string testName = testNameBuilder.TestName;
			
			string expectedTestName = Method_RowTestMethodWithNullArgument + "(null)";
			Assert.That(testName, Is.EqualTo(expectedTestName));
		}
		
#if NET_2_0
		[Test]
		public void TestName_SecondArgumentIsNull()
		{
			RowTestNameBuilder testNameBuilder = CreateRowTestNameBuilder(GetRowTestMethodWithNormalAndNullArgument(), 5, null);
			
			string testName = testNameBuilder.TestName;
			
			string expectedTestName = Method_RowTestMethodWithNormalAndNullArgument + "(5, null)";
			Assert.That(testName, Is.EqualTo(expectedTestName));
		}
#endif
		
		private RowTestNameBuilder CreateRowTestNameBuilder(params object[] arguments)
		{
			return CreateRowTestNameBuilder(GetRowTestMethodWith2Rows(), arguments);
		}
		
		private RowTestNameBuilder CreateRowTestNameBuilder(MethodInfo method, params object[] arguments)
		{
			return CreateRowTestNameBuilder(null, method, arguments);
		}
		
		private RowTestNameBuilder CreateRowTestNameBuilder(
				string baseTestName,
				MethodInfo method,
				params object[] arguments)
		{
			return new RowTestNameBuilder(method, baseTestName, arguments);
		}
	}
}

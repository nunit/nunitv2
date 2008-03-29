// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Extensions;
using NUnit.Framework.SyntaxHelpers;

namespace NUnit.Core.Extensions.RowTest.UnitTests
{
	[TestFixture]
	public class RowTestFrameworkTest : BaseTestFixture
	{
		[Test]
		public void GetRowArguments()
		{
			object[] rowArguments = new object[] { 4, 5 };
			RowAttribute row = new RowAttribute(rowArguments);
			
			object[] extractedRowArguments = RowTestFramework.GetRowArguments(row);
			
			Assert.That(extractedRowArguments, Is.SameAs(rowArguments));
		}
		
		[Test]
		public void IsSpecialValue_True()
		{
			bool isSpecialValue = RowTestFramework.IsSpecialValue(SpecialValue.Null);
			
			Assert.That(isSpecialValue, Is.True);
		}
		
		[Test]
		public void IsSpecialValue_False()
		{
			bool isSpecialValue = RowTestFramework.IsSpecialValue(42);
			
			Assert.That(isSpecialValue, Is.False);
		}
		
		[Test]
		public void IsSpecialValue_Null()
		{
			bool isSpecialValue = RowTestFramework.IsSpecialValue(null);
			
			Assert.That(isSpecialValue, Is.False);
		}

		[Test]
		public void GetExpectedExceptionType()
		{
			Type expectedExceptionType = typeof(ArgumentException);
			RowAttribute row = CreateRowAttribute();
			row.ExpectedException = expectedExceptionType;
			
			Type extractedExpectedExceptionType = RowTestFramework.GetExpectedExceptionType(row);
			
			Assert.That(extractedExpectedExceptionType, Is.SameAs(expectedExceptionType));
		}
		
		[Test]
		public void GetExpectedExceptionMessage()
		{
			string expectedExceptionMessage = "Expected Exception Message.";
			Type expectedExceptionType = typeof(ArgumentException);
			RowAttribute row = CreateRowAttribute();
			row.ExceptionMessage = expectedExceptionMessage;
			
			string extractedExceptionMessage = RowTestFramework.GetExpectedExceptionMessage(row);
			
			Assert.That(extractedExceptionMessage, Is.SameAs(expectedExceptionMessage));
		}
		
		[Test]
		public void GetTestName()
		{
			string testName = "UnitTest";
			RowAttribute row = CreateRowAttribute();
			row.TestName = testName;
			
			string extractedTestName = RowTestFramework.GetTestName(row);
			
			Assert.That(extractedTestName, Is.EqualTo(testName));
		}
		
		[Test]
		public void IsRowTest_False()
		{
			MethodInfo method = GetTestClassMethod("MethodWithoutRowTestAttribute");

			bool isRowTest = RowTestFramework.IsRowTest(method);
			
			Assert.That(isRowTest, Is.False);
		}
		
		[Test]
		public void IsRowTest_True()
		{
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");

			bool isRowTest = RowTestFramework.IsRowTest(method);
			
			Assert.That(isRowTest, Is.True);
		}
		
		[Test]
		public void IsRowTest_MethodIsNull()
		{
			bool isRowTest = RowTestFramework.IsRowTest(null);
			
			Assert.That(isRowTest, Is.False);
		}
		
		[Test]
		public void GetRowAttributes_NoRows()
		{
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");
			
			Attribute[] rowAttributes = RowTestFramework.GetRowAttributes(method);
			
			Assert.That(rowAttributes.Length, Is.EqualTo(0));
		}
		
		[Test]
		public void GetRowAttributes_TwoRows()
		{
			MethodInfo method = GetTestClassMethod("RowTestMethodWith2Rows");
			
			Attribute[] rowAttributes = RowTestFramework.GetRowAttributes(method);
			
			Assert.That(rowAttributes.Length, Is.EqualTo(2));
		}
		
		private RowAttribute CreateRowAttribute()
		{
			return new RowAttribute(4, 5);
		}
	}
}

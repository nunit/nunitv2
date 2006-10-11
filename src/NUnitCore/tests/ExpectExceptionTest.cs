#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Reflection;
using System.Runtime.Serialization;
using NUnit.Framework;
using NUnit.TestUtilities;
using NUnit.TestData.ExpectExceptionTest;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class ExpectExceptionTest 
	{

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CanSpecifyExceptionType()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void CanSpecifyExceptionName()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException),"argument exception")]
		public void CanSpecifyExceptionTypeAndMessage()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException),"argument exception", MessageMatch.Exact)]
		public void CanSpecifyExceptionTypeAndExactMatch()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException),"invalid", MessageMatch.Contains)]
		public void CanSpecifyExceptionTypeAndContainsMatch()
		{
			throw new ArgumentException("argument invalid exception");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException),"exception$", MessageMatch.Regex)]
		public void CanSpecifyExceptionTypeAndRegexMatch()
		{
			throw new ArgumentException("argument invalid exception");
		}

		[Test]
		[ExpectedException("System.ArgumentException","argument exception")]
		public void CanSpecifyExceptionNameAndMessage()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException("System.ArgumentException","argument exception",MessageMatch.Exact)]
		public void CanSpecifyExceptionNameAndExactMatch()
		{
			throw new ArgumentException("argument exception");
		}

		[Test]
		[ExpectedException("System.ArgumentException","invalid", MessageMatch.Contains)]
		public void CanSpecifyExceptionNameAndContainsMatch()
		{
			throw new ArgumentException("argument invalid exception");
		}

		[Test]
		[ExpectedException("System.ArgumentException","exception$", MessageMatch.Regex)]
		public void CanSpecifyExceptionNameAndRegexMatch()
		{
			throw new ArgumentException("argument invalid exception");
		}

		[Test]
		public void TestBaseException()
		{
			Type fixtureType = typeof(BaseException);
			Test test = TestBuilder.MakeTestCase( fixtureType, "BaseExceptionTest" );
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "BaseExceptionTest should have failed");
			Assert.AreEqual("Expected: System.ArgumentException but was System.Exception", result.Message);
		}

		[Test]
		public void TestMismatchedExceptionType()
		{
			Type fixtureType = typeof(MismatchedException);
			Test test = TestBuilder.MakeTestCase( fixtureType, "MismatchedExceptionType" );
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "MismatchedExceptionType should have failed");
			Assert.AreEqual("Expected: System.ArgumentException but was System.ArgumentOutOfRangeException", result.Message);
		}

		[Test]
		public void TestMismatchedExceptionName()
		{
			Type fixtureType = typeof(MismatchedException);
			Test test = TestBuilder.MakeTestCase( fixtureType, "MismatchedExceptionName" );
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "MismatchedExceptionName should have failed");
			Assert.AreEqual("Expected: System.ArgumentException but was System.ArgumentOutOfRangeException", result.Message);
		}

		[Test]
		public void TestExceptionTypeNotThrown()
		{
			Type fixtureType = typeof(TestDoesNotThrowExceptionFixture);
			Test test = TestBuilder.MakeTestCase( fixtureType, "TestDoesNotThrowExceptionType" );
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "MismatchedExceptionType should have failed");
			Assert.AreEqual("System.ArgumentException was expected", result.Message);
		}

		[Test]
		public void TestExceptionNameNotThrown()
		{
			Type fixtureType = typeof(TestDoesNotThrowExceptionFixture);
			Test test = TestBuilder.MakeTestCase( fixtureType, "TestDoesNotThrowExceptionName" );
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "MismatchedExceptionName should have failed");
			Assert.AreEqual("System.ArgumentException was expected", result.Message);
		}

		[Test] 
		public void MethodThrowsException()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( TestThrowsExceptionFixture ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test] 
		public void MethodThrowsRightExceptionMessage()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( TestThrowsExceptionWithRightMessage ) );
			Assert.AreEqual(true, result.IsSuccess);
		}

		[Test]
		public void MethodThrowsArgumentOutOfRange()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( TestThrowsArgumentOutOfRangeException ) );
			Assert.AreEqual(true, result.IsSuccess );
		}

		[Test] 
		public void MethodThrowsWrongExceptionMessage()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( TestThrowsExceptionWithWrongMessage ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void SetUpThrowsSameException()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( SetUpExceptionTests ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void TearDownThrowsSameException()
		{
			TestResult result = TestBuilder.RunTestFixture( typeof( TearDownExceptionTests ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void AssertFailBeforeException() 
		{ 
			TestSuiteResult suiteResult = (TestSuiteResult)TestBuilder.RunTestFixture( typeof (TestAssertsBeforeThrowingException) );
			Assert.AreEqual( true, suiteResult.IsFailure );
			TestResult result = (TestResult)suiteResult.Results[0];
			Assert.AreEqual( "private message", result.Message );
		} 

		internal class MyAppException : System.Exception
		{
			public MyAppException (string message) : base(message) 
			{}

			public MyAppException(string message, Exception inner) :
				base(message, inner) 
			{}

			protected MyAppException(SerializationInfo info, 
				StreamingContext context) : base(info,context)
			{}
		}

		[Test]
		[ExpectedException(typeof(MyAppException))] 
		public void ThrowingMyAppException() 
		{ 
			throw new MyAppException("my app");
		}

		[Test]
		[ExpectedException(typeof(MyAppException), "my app")] 
		public void ThrowingMyAppExceptionWithMessage() 
		{ 
			throw new MyAppException("my app");
		}

		[Test]
		[ExpectedException(typeof(NunitException))]
		public void ThrowNunitException()
		{
			throw new NunitException("Nunit exception");
		}
	}
}

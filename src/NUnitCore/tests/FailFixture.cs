#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright  2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, 
' Charlie Poole or Copyright  2000-2002 Philip A. Craig
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
using NUnit.Framework;
using NUnit.TestUtilities;
using NUnit.TestData.FailFixture;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class FailFixture
	{
		[Test]
		public void VerifyFailWorks()
		{
			TestResult result = TestBuilder.RunTestCase( 
				typeof(VerifyFailThrowsException), 
				"CallAssertFail" );
			Assert.IsTrue(result.IsFailure, "Should have failed");
			Assert.AreEqual(
				VerifyFailThrowsException.failureMessage, 
				result.Message);
		}
		[Test]
		public void VerifyAssertionFailWorks()
		{
			TestResult result = TestBuilder.RunTestCase( 
				typeof(VerifyFailThrowsException), 
				"CallAssertionFail" );
			Assert.IsTrue(result.IsFailure, "Should have failed");
			Assert.AreEqual(
				VerifyFailThrowsException.failureMessage, 
				result.Message);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void FailThrowsAssertionException()
		{
			Assert.Fail(String.Empty);
		}

		[Test]
		public void FailInheritsFromSystemException() 
		{
			try 
			{
				Assert.Fail();
			} 
			catch (System.Exception) 
			{
				return;
			}

			throw new AssertionException("fail"); // You can't call fail() here
		}

		[Test]
		public void FailRecordsInnerException()
		{
			Type fixtureType = typeof(VerifyTestResultRecordsInnerExceptions);
			string expectedMessage ="System.Exception : Outer Exception" + Environment.NewLine + "  ----> System.Exception : Inner Exception";
			NUnit.Core.TestResult result = TestBuilder.RunTestCase(fixtureType, "ThrowInnerException");
			Assert.IsTrue(result.IsFailure, "Should have failed");
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[Test]
		public void BadStackTraceIsHandled()
		{
			TestResult result = TestBuilder.RunTestCase( typeof( BadStackTraceFixture ), "TestFailure" );
			Assert.AreEqual( true, result.IsFailure );
			Assert.AreEqual( "NUnit.TestData.FailFixture.ExceptionWithBadStackTrace : thrown by me", result.Message );
			Assert.AreEqual( "No stack trace available", result.StackTrace );
		}

		[Test]
		public void CustomExceptionIsHandled()
		{
			TestResult result = TestBuilder.RunTestCase( typeof( CustomExceptionFixture ), "ThrowCustomException" );
			Assert.AreEqual( true, result.IsFailure );
			Assert.AreEqual( "NUnit.TestData.FailFixture.CustomExceptionFixture+CustomException : message", result.Message );
		}
	}
}

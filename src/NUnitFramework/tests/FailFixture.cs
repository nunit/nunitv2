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
using NUnit.Core.Builders;

namespace NUnit.Core.Tests
{
	[TestFixture]
	public class FailFixture
	{
		private TestResult RunTestCase( Type fixtureType, string methodName )
		{
			TestCase test = TestCaseBuilder.Make( fixtureType, methodName );		
			TestSuite suite = TestFixtureBuilder.Make( fixtureType );
			suite.Add( test );
			return test.Run( NullListener.NULL );
		}

		[TestFixture]
		internal class VerifyFailThrowsException
		{
			internal static string failureMessage = "This should call fail";

			[Test]
			public void CallAssertionFail()
			{
				Assert.Fail(failureMessage);
			}
		}

		[Test]
		public void FailWorks()
		{
			TestResult result = RunTestCase( 
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

		[TestFixture]
		internal class VerifyTestResultRecordsInnerExceptions
		{
			[Test]
			public void ThrowInnerException()
			{
				throw new Exception("Outer Exception", new Exception("Inner Exception"));
			}
		}

		[Test]
		public void FailRecordsInnerException()
		{
			Type fixtureType = typeof(VerifyTestResultRecordsInnerExceptions);
			string expectedMessage ="System.Exception : Outer Exception" + Environment.NewLine + "  ----> System.Exception : Inner Exception";
			NUnit.Core.TestResult result = RunTestCase(fixtureType, "ThrowInnerException");
			Assert.IsTrue(result.IsFailure, "Should have failed");
			Assert.AreEqual(expectedMessage, result.Message);
		}

		[TestFixture]
		private class BadStackTraceFixture
		{
			[Test]
			public void TestFailure()
			{
				throw new ExceptionWithBadStackTrace("thrown by me");
			}
		}

		private class ExceptionWithBadStackTrace : Exception
		{
			public ExceptionWithBadStackTrace( string message )
				: base( message ) { }

			public override string StackTrace
			{
				get
				{
					throw new InvalidOperationException( "Simulated failure getting stack trace" );
				}
			}
		}

		[Test]
		public void BadStackTraceIsHandled()
		{
			TestResult result = RunTestCase( typeof( BadStackTraceFixture ), "TestFailure" );
			Assert.AreEqual( true, result.IsFailure );
			Assert.AreEqual( "NUnit.Core.Tests.FailFixture+ExceptionWithBadStackTrace : thrown by me", result.Message );
			Assert.AreEqual( "No stack trace available", result.StackTrace );
		}

		[Test]
		public void CustomExceptionIsHandled()
		{
			TestResult result = RunTestCase( typeof( CustomExceptionFixture ), "ThrowCustomException" );
			Assert.AreEqual( true, result.IsFailure );
			Assert.AreEqual( "NUnit.Core.Tests.FailFixture+CustomExceptionFixture+CustomException : message", result.Message );
		}

		[TestFixture]
		private class CustomExceptionFixture
		{
			[Test]
			public void ThrowCustomException()
			{
				throw new CustomException( "message", new CustomType() );
			}

			private class CustomType
			{
			}

			private class CustomException : Exception
			{
				private CustomType custom;

				public CustomException( string msg, CustomType custom ) : base( msg )
				{
					this.custom = custom;
				}
			}
		}
	}
}

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
using System.Text;
using NUnit.Framework;

namespace NUnit.Tests.Assertions
{
	[TestFixture]
	public class FailFixture
	{
		[TestFixture]
		internal class VerifyFailThrowsException
		{
			internal string failureMessage;

			[Test]
			public void CallAssertionFail()
			{
				Assert.Fail(failureMessage);
			}
		}
		[TestFixture]
		internal class VerifyTestResultRecordsInnerExceptions
		{
			internal string failureMessage ="System.Exception : Outer Exception" + Environment.NewLine + "  ----> System.Exception : Inner Exception";
			[Test]
			public void ThrowInnerException()
			{
				throw new Exception("Outer Exception", new Exception("Inner Exception"));
			}
		}
		[Test]
		public void FailWorks()
		{
			string failureMessage = "This should call fail";
			
			VerifyFailThrowsException verifyFail = new VerifyFailThrowsException();
			verifyFail.failureMessage = failureMessage;

			NUnit.Core.Test test = NUnit.Core.TestCaseBuilder.Make(verifyFail, "CallAssertionFail");
			NUnit.Core.TestResult result = test.Run(NUnit.Core.NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "VerifyFailThrowsException should have failed");
			Assert.AreEqual(failureMessage, result.Message);
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
		public void FailRecordInnerException()
		{
			VerifyTestResultRecordsInnerExceptions verifyInner = new VerifyTestResultRecordsInnerExceptions();
				
			string failureMessage = verifyInner.failureMessage;

			NUnit.Core.Test test = NUnit.Core.TestCaseBuilder.Make(verifyInner, "ThrowInnerException");
			NUnit.Core.TestResult result = test.Run(NUnit.Core.NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "VerifyTestResultRecordsInnerExceptions should have failed");
			Assert.AreEqual(failureMessage, result.Message);
		}

	}
}

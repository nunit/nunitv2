#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Tests 
{
	using System;
	using System.Runtime.Serialization;
	using NUnit.Framework;
	using NUnit.Core;


	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class ExpectExceptionTest 
	{
		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestSingle()
		{
			throw new Exception("single exception");
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestSpecificException()
		{
			throw new ArgumentException("argument exception");
		}

		[TestFixture]
		internal class BaseException
		{
			[Test]
			[ExpectedException(typeof(ArgumentException))]
			public void BaseExceptionTest()
			{
				throw new Exception();
			}
		}

		[Test]
		public void TestBaseException()
		{
			BaseException be = new BaseException();
			Test test = TestCaseBuilder.Make(be, "BaseExceptionTest");
			TestResult result = test.Run(NullListener.NULL);
			Assertion.Assert("BaseExceptionTest should have failed", result.IsFailure);
			Assertion.AssertEquals("Expected: ArgumentException but was Exception", result.Message);
		}

		[Test]
		public void TestMismatchedException()
		{
			MismatchedException me = new MismatchedException();
			Test test = TestCaseBuilder.Make(me, "MismatchedExceptionTest");
			TestResult result = test.Run(NullListener.NULL);
			Assertion.Assert("MismatchedExceptionTest should have failed", result.IsFailure);
			Assertion.AssertEquals("Expected: ArgumentException but was ArgumentOutOfRangeException", result.Message);
		}

		[TestFixture]
		internal class MismatchedException
		{
			[Test]
			[ExpectedException(typeof(ArgumentException))]
			public void MismatchedExceptionTest()
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		[TestFixture]
		public class SetUpExceptionTests  
		{
			[SetUp]
			public void Init()
			{
				throw new ArgumentException("SetUp Exception");
			}

			[Test]
			[ExpectedException(typeof(ArgumentException))]
			public void Test() 
			{
			}
		}

		[TestFixture]
		public class TearDownExceptionTests
		{
			[TearDown]
			public void CleanUp()
			{
				throw new ArgumentException("TearDown Exception");
			}

			[Test]
			[ExpectedException(typeof(ArgumentException))]
			public void Test() 
			{}
		}

		[TestFixture]
		internal class TestThrowsExceptionFixture
		{
			[Test]
			public void TestThrow()
			{
				throw new Exception();
			}
		}

		[Test] 
		public void MethodThrowsException()
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object testFixture = builder.BuildTestFixture(typeof(TestThrowsExceptionFixture));
			TestSuite suite = new TestSuite("mock suite");
			suite.Add(testFixture);	
	
			TestResult result = suite.Run(NUnit.Core.NullListener.NULL);
			Assertion.AssertEquals(true, result.IsFailure);
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
		[ExpectedException(typeof(NunitException))]
		public void ThrowNunitException()
		{
			throw new NunitException("Nunit exception");
		}
	}
}

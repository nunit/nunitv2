/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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
		/// <summary>
		/// 
		/// </summary>
		/// 
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

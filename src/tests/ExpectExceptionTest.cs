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
using System.Runtime.Serialization;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
{
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
			Assert.IsTrue(result.IsFailure, "BaseExceptionTest should have failed");
			Assert.AreEqual("Expected: ArgumentException but was Exception", result.Message);
		}

		[Test]
		public void TestMismatchedException()
		{
			MismatchedException me = new MismatchedException();
			Test test = TestCaseBuilder.Make(me, "MismatchedExceptionTest");
			TestResult result = test.Run(NullListener.NULL);
			Assert.IsTrue(result.IsFailure, "MismatchedExceptionTest should have failed");
			Assert.AreEqual("Expected: ArgumentException but was ArgumentOutOfRangeException", result.Message);
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
		internal class SetUpExceptionTests  
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
		internal class TearDownExceptionTests
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

		[TestFixture]
		internal class TestThrowsExceptionWithRightMessage
		{
			[Test]
			[ExpectedException(typeof(Exception), "the message")]
			public void TestThrow()
			{
				throw new Exception("the message");
			}
		}

		[TestFixture]
		internal class TestThrowsArgumentOutOfRangeException
		{
			[Test]
			[ExpectedException(typeof(ArgumentOutOfRangeException),
				 "the message\r\nParameter name: param\r\nActual value was actual value.")]
			public void TestThrow()
			{
				throw new ArgumentOutOfRangeException("param", "actual value", "the message");
			}
		}

		[TestFixture]
			internal class TestThrowsExceptionWithWrongMessage
		{
			[Test]
			[ExpectedException(typeof(Exception), "not the message")]
			public void TestThrow()
			{
				throw new Exception("the message");
			}
		}

		[TestFixture]
		internal class TestAssertsBeforeThrowingException
		{
			[Test]
			[ExpectedException(typeof(Exception))]
			public void TestAssertFail()
			{
				Assert.Fail( "private message" );
			}
		}

		[Test] 
		public void MethodThrowsException()
		{
			TestResult result = RunInternalTest( typeof( TestThrowsExceptionFixture ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test] 
		public void MethodThrowsRightExceptionMessage()
		{
			TestResult result = RunInternalTest( typeof( TestThrowsExceptionWithRightMessage ) );
			Assert.AreEqual(true, result.IsSuccess);
		}

		[Test]
		public void MethodThrowsArgumentOutOfRange()
		{
			TestResult result = RunInternalTest( typeof( TestThrowsArgumentOutOfRangeException ) );
			Assert.AreEqual(true, result.IsSuccess );
		}

		[Test] 
		public void MethodThrowsWrongExceptionMessage()
		{
			TestResult result = RunInternalTest( typeof( TestThrowsExceptionWithWrongMessage ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void SetUpThrowsSameException()
		{
			TestResult result = RunInternalTest( typeof( SetUpExceptionTests ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void TearDownThrowsSameException()
		{
			TestResult result = RunInternalTest( typeof( TearDownExceptionTests ) );
			Assert.AreEqual(true, result.IsFailure);
		}

		[Test]
		public void AssertFailBeforeException() 
		{ 
			TestSuiteResult suiteResult = (TestSuiteResult)RunInternalTest( typeof (TestAssertsBeforeThrowingException) );
			Assert.AreEqual( true, suiteResult.IsFailure );
			suiteResult = (TestSuiteResult)suiteResult.Results[0];
			TestResult result = (TestResult)suiteResult.Results[0];
			Assert.AreEqual( "private message", result.Message );
		} 

		private TestResult RunInternalTest( Type type )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();
			object testFixture = builder.BuildTestFixture( type );
			TestSuite suite = new TestSuite("mock suite");
			suite.Add( testFixture );

			return suite.Run( NUnit.Core.NullListener.NULL );
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

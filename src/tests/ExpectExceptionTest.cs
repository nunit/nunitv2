//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests 
{
	using System;
	using Nunit.Framework;
	using Nunit.Core;

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
	}
}

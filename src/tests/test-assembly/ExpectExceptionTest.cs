using System;
using NUnit.Framework;

namespace NUnit.TestData.ExpectExceptionTest
{
	[TestFixture]
	public class BaseException
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void BaseExceptionTest()
		{
			throw new Exception();
		}
	}

	[TestFixture]
	public class DerivedException
	{
		[Test]
		[ExpectedException(typeof(Exception))]
		public void DerivedExceptionTest()
		{
			throw new ArgumentException();
		}
	}

	[TestFixture]
	public class MismatchedException
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MismatchedExceptionType()
		{
			throw new ArgumentOutOfRangeException();
		}

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void MismatchedExceptionName()
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
	public class TestThrowsExceptionFixture
	{
		[Test]
		public void TestThrow()
		{
			throw new Exception();
		}
	}

	[TestFixture]
	public class TestDoesNotThrowExceptionFixture
	{
		[Test, ExpectedException("System.ArgumentException")]
		public void TestDoesNotThrowExceptionName()
		{
		}
		[Test, ExpectedException( typeof( System.ArgumentException ) )]
		public void TestDoesNotThrowExceptionType()
		{
		}

		[Test, ExpectedException]
		public void TestDoesNotThrowUnspecifiedException()
		{
		}
	}

	[TestFixture]
	public class TestThrowsExceptionWithRightMessage
	{
		[Test]
		[ExpectedException(typeof(Exception), "the message")]
		public void TestThrow()
		{
			throw new Exception("the message");
		}
	}

	[TestFixture]
	public class TestThrowsArgumentOutOfRangeException
	{
		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException)) ]
		public void TestThrow()
		{
			throw new ArgumentOutOfRangeException("param", "actual value", "the message");
		}
	}

	[TestFixture]
	public class TestThrowsExceptionWithWrongMessage
	{
		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage="not the message")]
		public void TestThrow()
		{
			throw new Exception("the message");
		}
	}

	[TestFixture]
	public class TestAssertsBeforeThrowingException
	{
		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestAssertFail()
		{
			Assert.Fail( "private message" );
		}
	}
}

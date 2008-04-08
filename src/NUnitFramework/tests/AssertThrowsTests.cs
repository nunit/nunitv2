using System;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class AssertThrowsTests : MessageChecker
	{
		[Test]
		public void CorrectExceptionThrown()
		{
			Assert.Throws(typeof(ArgumentException),
				new TestSnippet( ThrowsArgumentException ) );

#if NET_2_0
            Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException(); });
            Assert.Throws(typeof(ArgumentException), 
                delegate { throw new ArgumentException(); } );
			Assert.Throws<ArgumentException>( ThrowsArgumentException );
#endif
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void NoExceptionThrown()
		{
			expectedMessage =
				"  Expected: <System.ArgumentException>" + Environment.NewLine +
				"  But was:  null" + Environment.NewLine;
#if NET_2_0
			Assert.Throws<ArgumentException>( ThrowsNothing );
#else
			Assert.Throws( typeof(ArgumentException),
				new TestSnippet( ThrowsNothing ) );
#endif
		}

        [Test, ExpectedException(typeof(AssertionException))]
        public void UnrelatedExceptionThrown()
        {
            expectedMessage =
                "  Expected: <System.ArgumentException>" + Environment.NewLine +
                "  But was:  <System.ApplicationException>" + Environment.NewLine;
#if NET_2_0
			Assert.Throws<ArgumentException>(ThrowsApplicationException);
#else
			Assert.Throws( typeof(ArgumentException),
				new TestSnippet(ThrowsApplicationException) );
#endif
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void BaseExceptionThrown()
        {
            expectedMessage =
                "  Expected: <System.ArgumentException>" + Environment.NewLine +
                "  But was:  <System.Exception>" + Environment.NewLine;
#if NET_2_0
			Assert.Throws<ArgumentException>(ThrowsException);
#else
            Assert.Throws( typeof(ArgumentException),
                new TestSnippet( ThrowsException) );
#endif
        }

        [Test,ExpectedException(typeof(AssertionException))]
        public void DerivedExceptionThrown()
        {
            expectedMessage =
                "  Expected: <System.Exception>" + Environment.NewLine +
                "  But was:  <System.ArgumentException>" + Environment.NewLine;
#if NET_2_0
			Assert.Throws<Exception>(ThrowsArgumentException);
#else
            Assert.Throws( typeof(Exception),
				new TestSnippet( ThrowsArgumentException) );
#endif
        }

		void ThrowsArgumentException()
		{
			throw new ArgumentException();
		}

		void ThrowsApplicationException()
		{
			throw new ApplicationException();
		}

		void ThrowsException()
		{
			throw new Exception();
		}

		void ThrowsNothing()
		{
		}
	}
}

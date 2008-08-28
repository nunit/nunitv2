using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class AssertThrowsTests : MessageChecker
	{
		[Test]
		public void CorrectExceptionThrown()
		{
#if NET_2_0
            Assert.Throws(typeof(ArgumentException), ThrowsArgumentException);
            Assert.Throws(typeof(ArgumentException),
                delegate { throw new ArgumentException(); });

            Assert.That(ThrowsArgumentException,
                Throws.Exception<ArgumentException>());

            Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException(); });
			Assert.Throws<ArgumentException>( ThrowsArgumentException );

		    Assert.That(ThrowsArgumentException,
		                Throws.Exception<ArgumentException>().Property("ParamName").EqualTo("myParam"));
            Assert.That(
                delegate { throw new ArgumentException("mymessage", "myparam"); },
                Throws.Exception<ArgumentException>().Property("ParamName").EqualTo("myparam")
                    .And.Property("Message").StartsWith("mymessage"));

#else
			Assert.Throws(typeof(ArgumentException),
				new TestDelegate( ThrowsArgumentException ) );

            Assert.That( new TestDelegate( ThrowsArgumentException ),  
                Throws.Exception(typeof(ArgumentException)));
#endif
        }

		[Test]
		public void CorrectExceptionIsReturnedToMethod()
		{
			ArgumentException ex = Assert.Throws(typeof(ArgumentException),
				new TestDelegate( ThrowsArgumentException ) ) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, StartsWith("myMessage"));
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));

#if NET_2_0
            ex = Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException("myMessage", "myParam"); }) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, StartsWith("myMessage"));
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));

			ex = Assert.Throws(typeof(ArgumentException), 
                delegate { throw new ArgumentException("myMessage", "myParam"); } ) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, StartsWith("myMessage"));
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));

			ex = Assert.Throws<ArgumentException>( ThrowsArgumentException ) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, StartsWith("myMessage"));
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
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
				new TestDelegate( ThrowsNothing ) );
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
				new TestDelegate(ThrowsApplicationException) );
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
                new TestDelegate( ThrowsException) );
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
				new TestDelegate( ThrowsArgumentException) );
#endif
        }

        [Test]
        public void DoesNotThrowSuceeds()
        {
#if NET_2_0
            Assert.DoesNotThrow(ThrowsNothing);
#else
            Assert.DoesNotThrow( new TestDelegate( ThrowsNothing ) );

			Assert.That( new TestDelegate( ThrowsNothing ), Throws.Nothing );
#endif
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void DoesNotThrowFails()
        {
#if NET_2_0
            Assert.DoesNotThrow(ThrowsArgumentException);
#else
            Assert.DoesNotThrow( new TestDelegate( ThrowsArgumentException ) );
#endif
        }

        #region Methods Called by Tests
        static void ThrowsArgumentException()
		{
			throw new ArgumentException("myMessage", "myParam");
		}

        static void ThrowsApplicationException()
		{
			throw new ApplicationException();
		}

        static void ThrowsException()
		{
			throw new Exception();
		}

        static void ThrowsNothing()
		{
        }
        #endregion
    }
}

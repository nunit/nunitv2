using System;

namespace NUnit.Framework
{
	/// <summary>
	/// NotEqualAsserter is the asserter class that handles 
	/// inequality assertions.
	/// </summary>
	public class NotEqualAsserter : EqualityAsserter
	{
		/// <summary>
		/// Constructor for NotEqualAsserter
		/// </summary>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="message">The message to be printed when the two objects are the same object.</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public NotEqualAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		/// <summary>
		/// Assert that the two objects are not equal, failing
		/// if they are found to be equal.
		/// </summary>
		public override void Assert()
		{
			if ( expected == null && actual == null ) Fail();
			if ( expected == null || actual == null ) return;

			if ( expected.GetType().IsArray && actual.GetType().IsArray )
			{
				if ( ArraysEqual( (Array)expected, (Array)actual ) )
					Fail();
			}
			else if ( ObjectsEqual( expected, actual ) )
				Fail();
		}

		/// <summary>
		/// Fail by throwing an AssertionException with the message 
		/// provided by the user.
		/// </summary>
		/// <returns></returns>
		public bool Fail()
		{
			throw new AssertionException( FormattedMessage );
		}
	}
}

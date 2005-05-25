using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Asserter that verifies two objects are the same.
	/// </summary>
	public class SameAsserter : ComparisonAsserter
	{
		/// <summary>
		/// Construct a SameAsserter object
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="message">A user-defined message for use in reporting errors</param>
		/// <param name="args">Arguments to be used in formatting the user-defined message</param>
		public SameAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public override bool Test()
		{
			return object.ReferenceEquals( expected, actual );
		}

		public override string Message
		{
			get
			{
				CreateFailureMessage().Write( "expected same" );
				return failureMessage.ToString();
			}
		}
	}
}

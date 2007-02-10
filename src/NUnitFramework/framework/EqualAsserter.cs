using System;
using System.Text;
using System.Collections;

namespace NUnit.Framework
{
	/// <summary>
	/// Class to assert that two objects are equal 
	/// </summary>
	public class EqualAsserter : EqualityAsserter
	{
		/// <summary>
		/// Constructs an EqualAsserter for two objects
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public EqualAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		/// <summary>
		/// Constructs an EqualAsserter for two doubles and a tolerance
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="tolerance">The tolerance used in making the comparison</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public EqualAsserter( double expected, double actual, double tolerance, string message, params object[] args )
			: base( expected, actual, tolerance, message, args ) { }

		/// <summary>
		/// Test whether the objects are equal, building up
		/// the failure message for later use if they are not.
		/// </summary>
		/// <returns>True if the objects are equal</returns>
		public override bool Test()
		{
			if ( !ObjectsEqual( expected, actual ) )
			{
				if ( failurePoint >= 0 )
				{
					if ( expected.GetType().IsArray && actual.GetType().IsArray )
						FailureMessage.DisplayArrayDifferences( (Array)expected, (Array)actual, failurePoint );
					else
						FailureMessage.DisplayCollectionDifferences( (ICollection)expected, (ICollection)actual, failurePoint );
				}
				else
				{
					if ( expected is double && actual is double )
						FailureMessage.DisplayDifferencesWithTolerance( (double)expected, (double)actual, delta );
					else
						FailureMessage.DisplayDifferences( expected, actual, false );
				}

				return false;
			}

			return true;
		}
	}
}

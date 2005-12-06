using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Class to assert that the actual value is greater than the expected value.
	/// </summary>
	public class GreaterAsserter : ComparisonAsserter
	{
		/// <summary>
		/// Constructs a GreaterAsserter for two objects
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public GreaterAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		/// <summary>
		/// Test whether the actual is greater than the expected, building up
		/// the failure message for later use if they are not.
		/// </summary>
		/// <returns>True if actual is greater than expected</returns>
		public override bool Test()
		{
			if (ImplementsIComparable(actual) && ImplementsIComparable(expected))
			{
				IComparable cActual = actual as IComparable;

				if (cActual.CompareTo(expected) > 0) return true;
			}
			else
			{
				if (!ImplementsIComparable(actual))
					throw new ArgumentException("Must implement IComparable.","actual");
				if (!ImplementsIComparable(expected))
					throw new ArgumentException("Must implement IComparable.","expected");
			}

			DisplayDifferences();
			return false;
		}

		public bool ImplementsIComparable(object obj)
		{
			try
			{
				System.Reflection.InterfaceMapping x = obj.GetType().GetInterfaceMap(typeof(IComparable));
				return true;
			}
			catch( ArgumentException )
			{
				return false;
			}
		}

		private void DisplayDifferences()
		{
			FailureMessage.AddLine("\tThe value of actual is not greater than the value of expected.",actual, expected);
			FailureMessage.AddActualLine(actual.ToString());
			FailureMessage.AddExpectedLine(expected.ToString());
		}
	}
}



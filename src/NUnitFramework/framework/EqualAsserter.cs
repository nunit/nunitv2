using System;
using System.Text;

namespace NUnit.Framework
{
	/// <summary>
	/// Class to assert that two objects are equal 
	/// </summary>
	public class EqualAsserter : ComparisonAsserter
	{
		private double delta;
//		private int failingIndex = -1;

		/// <summary>
		/// Constructs an EqualAsserter for two objects
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public EqualAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		public EqualAsserter( double expected, double actual, double delta, string message, params object[] args )
			: base( expected, actual, message, args )
		{
			this.delta = delta;
		}

		/// <summary>
		/// Assert that the objects are equal
		/// </summary>
		/// <returns>True if they are equal, false if not</returns>
		public override void Assert()
		{
			if ( expected == null && actual == null ) return;
			if ( expected == null || actual == null )
				FailNotEquals();

			// For now, dynamically call array assertion if necessary. Try to move
			// this into the ObjectsEqual method later on.
			if ( expected.GetType().IsArray && actual.GetType().IsArray )
			{
				Array expectedArray = expected as Array;
				Array actualArray = actual as Array;

				if ( expectedArray.Rank != actualArray.Rank )
					FailNotEquals();
				
				if ( expectedArray.Rank != 1 )
					NUnit.Framework.Assert.Fail( "Multi-dimension array comparison is not supported" );

				int iLength = Math.Min( expectedArray.Length, actualArray.Length );
				for( int i = 0; i < iLength; i++ )
					if ( !ObjectsEqual( expectedArray.GetValue( i ), actualArray.GetValue( i ) ) )
						FailArraysNotEqual( i );
	
				if ( expectedArray.Length != actualArray.Length )
					FailArraysNotEqual( iLength );
			}
			else 
			{
				if ( !ObjectsEqual( expected, actual ) )
					FailNotEquals();
			}
		}

		/// <summary>
		/// Used to compare two objects.  Two nulls are equal and null
		/// is not equal to non-null. Comparisons between the same
		/// numeric types are fine (Int32 to Int32, or Int64 to Int64),
		/// but the Equals method fails across different types so we
		/// use <c>ToString</c> and compare the results.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <returns></returns>
		protected virtual bool ObjectsEqual( Object expected, Object actual )
		{
			if ( expected == null && actual == null ) return true;
			if ( expected == null || actual == null ) return false;

//			if ( expected.GetType().IsArray && actual.GetType().IsArray )
//				return ArraysEqual( (System.Array)expected, (System.Array)actual );

			if ( expected is double && actual is double )
			{
				// handle infinity specially since subtracting two infinite values gives 
				// NaN and the following test fails. mono also needs NaN to be handled
				// specially although ms.net could use either method.
				if (double.IsInfinity((double)expected) || double.IsNaN((double)expected) || double.IsNaN((double)actual))
					return (double)expected == (double)actual;
				else 
					return Math.Abs((double)expected-(double)actual) <= this.delta;
			}

//			if ( expected is float && actual is float )
//			{
//				// handle infinity specially since subtracting two infinite values gives 
//				// NaN and the following test fails. mono also needs NaN to be handled
//				// specially although ms.net could use either method.
//				if (float.IsInfinity((float)expected) || float.IsNaN((float)expected) || float.IsNaN((float)actual))
//					return (float)expected == (float)actual;
//				else 
//					return Math.Abs((float)expected-(float)actual) <= (float)this.delta;
//			}

			if ( expected.GetType() != actual.GetType() &&
				IsNumericType( expected )  && IsNumericType( actual ) )
			{
				//
				// Convert to strings and compare result to avoid
				// issues with different types that have the same
				// value
				//
				string sExpected = expected.ToString();
				string sActual   = actual.ToString();
				return sExpected.Equals( sActual );
			}
			return expected.Equals(actual);
		}

		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a numeric type</returns>
		private bool IsNumericType( Object obj )
		{
			if( null != obj )
			{
				if( obj is byte    ) return true;
				if( obj is sbyte   ) return true;
				if( obj is decimal ) return true;
				if( obj is double  ) return true;
				if( obj is float   ) return true;
				if( obj is int     ) return true;
				if( obj is uint    ) return true;
				if( obj is long    ) return true;
				if( obj is short   ) return true;
				if( obj is ushort  ) return true;

				if( obj is System.Byte    ) return true;
				if( obj is System.SByte   ) return true;
				if( obj is System.Decimal ) return true;
				if( obj is System.Double  ) return true;
				if( obj is System.Single  ) return true;
				if( obj is System.Int32   ) return true;
				if( obj is System.UInt32  ) return true;
				if( obj is System.Int64   ) return true;
				if( obj is System.UInt64  ) return true;
				if( obj is System.Int16   ) return true;
				if( obj is System.UInt16  ) return true;
			}
			return false;
		}

		private void FailNotEquals()
		{
//			AssertionFailureMessage msg = new AssertionFailureMessage( message, args );
//			if( InputsAreStrings( expected, actual ) )
//			{
//				msg.BuildStringsDifferentMessage( 
//					(string)expected, 
//					(string)actual );
//			}
//			else
//			{
//				msg.AppendExpectedAndActual( expected, actual );
//			}
//			return msg.ToString();
			throw new AssertionException( 
				AssertionFailureMessage.FormatMessageForFailNotEquals(
					expected,
					actual,
					message,
					args ) );
		}

		private void FailArraysNotEqual( int index )
		{
			throw new AssertionException( 
				AssertionFailureMessage.FormatMessageForFailArraysNotEqual( 
					index,
					(Array)expected, 
					(Array)actual, 
					message,
					args ) );
		}
	}
}

#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;

namespace NUnit.Framework
{
	public class Assert
	{
		/// <summary>
		/// Asserts that a condition is true. If it isn't it throws
		/// an <see cref="AssertionFailedError"/>.
		/// </summary>
		/// <param name="message">The message to display is the condition
		/// is false</param>
		/// <param name="condition">The evaluated condition</param>
		static public void True(string message, bool condition) 
		{
			if (!condition)
				Assert.Fail(message);
		}
    
		/// <summary>
		/// Asserts that a condition is true. If it isn't it throws
		/// an <see cref="AssertionFailedError"/>.
		/// </summary>
		/// <param name="condition">The evaluated condition</param>
		static public void True(bool condition) 
		{
			Assert.True(string.Empty, condition);
		}

		/// <summary>
		/// Asserts that a condition is false. If it isn't it throws
		/// an <see cref="AssertionFailedError"/>.
		/// </summary>
		/// <param name="message">The message to display is the condition
		/// is false</param>
		/// <param name="condition">The evaluated condition</param>
		static public void False(string message, bool condition) 
		{
			if (condition)
				Assert.Fail(message);
		}
		
		/// <summary>
		/// Asserts that a condition is false. If it isn't it throws
		/// an <see cref="AssertionFailedError"/>.
		/// </summary>
		/// <param name="condition">The evaluated condition</param>
		static public void False(bool condition) 
		{
			Assert.False(string.Empty, condition);
		}


		/// <summary>
		/// /// Asserts that two doubles are equal concerning a delta. If the
		/// expected value is infinity then the delta value is ignored.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="delta">The maximum acceptable difference between the
		/// the expected and the actual</param>
		static public void Equals(double expected, double actual, double delta) 
		{
			Assert.Equals(string.Empty, expected, actual, delta);
		}
		/// <summary>
		/// /// Asserts that two singles are equal concerning a delta. If the
		/// expected value is infinity then the delta value is ignored.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="delta">The maximum acceptable difference between the
		/// the expected and the actual</param>
		static public void Equals(float expected, float actual, float delta) 
		{
			Assert.Equals(string.Empty, expected, actual, delta);
		}

		/// <summary>Asserts that two objects are equal. If they are not
		/// an <see cref="AssertionFailedError"/> is thrown.</summary>
		static new public void Equals(Object expected, Object actual) 
		{
			Assert.Equals(string.Empty, expected, actual);
		}

		static public void Equals(int expected, int actual) 
		{
			Assert.Equals(string.Empty, expected, actual);
		}

		static public void Equals(string message, int expected, int actual) 
		{
			if (expected != actual)
				Assert.FailNotEquals(message, expected, actual);
		}
		
		/// <summary>Asserts that two doubles are equal concerning a delta.
		/// If the expected value is infinity then the delta value is ignored.
		/// </summary>
		static public void Equals(string message, double expected, 
			double actual, double delta) 
		{
			// handle infinity specially since subtracting two infinite values gives 
			// NaN and the following test fails
			if (double.IsInfinity(expected)) 
			{
				if (!(expected == actual))
					Assert.FailNotEquals(message, expected, actual);
			} 
			else if (!(Math.Abs(expected-actual) <= delta))
				Assert.FailNotEquals(message, expected, actual);
		}
		
		/// <summary>Asserts that two floats are equal concerning a delta.
		/// If the expected value is infinity then the delta value is ignored.
		/// </summary>
		static public void Equals(string message, float expected, 
			float actual, float delta) 
		{
			// handle infinity specially since subtracting two infinite values gives 
			// NaN and the following test fails
			if (float.IsInfinity(expected)) 
			{
				if (!(expected == actual))
					Assert.FailNotEquals(message, expected, actual);
			} 
			else if (!(Math.Abs(expected-actual) <= delta))
				Assert.FailNotEquals(message, expected, actual);
		}

		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a numeric type.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		static private bool IsNumericType( Object obj )
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

		/// <summary>
		/// Used to compare numeric types.  Comparisons between
		/// same types are fine (Int32 to Int32, or Int64 to Int64),
		/// but the Equals method fails across different types.
		/// This method was added to allow any numeric type to
		/// be handled correctly, by using <c>ToString</c> and
		/// comparing the result
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <returns></returns>
		static private bool ObjectsEqual( Object expected, Object actual )
		{
			if( IsNumericType( expected )  &&
				IsNumericType( actual ) )
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
		/// Asserts that two objects are equal.  Two objects are considered
		/// equal if both are null, or if both have the same value.  Numeric
		/// types are compared via string comparision on their contents to
		/// avoid problems comparing values between different types.  All
		/// non-numeric types are compared by using the <c>Equals</c> method.
		/// If they are not equal an <see cref="AssertionFailedError"/> is thrown.
		/// </summary>
		static public void Equals(string message, Object expected, Object actual)
		{
			if (expected == null && actual == null)
			{
				return;
			}
			if (expected != null && actual != null )
			{
				if( ObjectsEqual( expected, actual ) )
				{
					return;
				}
			}
			Assert.FailNotEquals(message, expected, actual);
		}
    
		/// <summary>Asserts that an object isn't null.</summary>
		static public void NotNull(Object anObject) 
		{
			Assert.NotNull(string.Empty, anObject);
		}
    
		/// <summary>Asserts that an object isn't null.</summary>
		static public void NotNull(string message, Object anObject) 
		{
			Assert.True(message, anObject != null); 
		}
    
		/// <summary>Asserts that an object is null.</summary>
		static public void Null(Object anObject) 
		{
			Assert.Null(string.Empty, anObject);
		}
    
		/// <summary>Asserts that an object is null.</summary>
		static public void Null(string message, Object anObject) 
		{
			Assert.True(message, anObject == null); 
		}
    
		/// <summary>Asserts that two objects refer to the same object. If they
		/// are not the same an <see cref="AssertionFailedError"/> is thrown.
		/// </summary>
		static public void Same(Object expected, Object actual) 
		{
			Assert.Same(string.Empty, expected, actual);
		}
    
		/// <summary>Asserts that two objects refer to the same object. 
		/// If they are not an <see cref="AssertionFailedError"/> is thrown.
		/// </summary>
		static public void Same(string message, Object expected, Object actual)
		{
			if (expected == actual)
				return;
			Assert.FailNotSame(message, expected, actual);
		}
    
		/// <summary>Fails a test with no message.</summary>
		static public void Fail() 
		{
			Assert.Fail(string.Empty);
		}
    
		/// <summary>Fails a test with the given message.</summary>
		static public void Fail(string message) 
		{
			if (message == null)
				message = string.Empty;
			throw new AssertionException(message);
		}

		/// <summary>
		/// Called when two objects have been compared and found to be
		/// different.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		static private void FailNotEquals(string message, Object expected, Object actual) 
		{
			Assert.Fail( 
				AssertionFailureMessage.FormatMessageForFailNotEquals( 
				message, 
				expected, 
				actual) );
		}
    
		static private void FailNotSame(string message, Object expected, Object actual) 
		{
			string formatted=string.Empty;
			if (message != null)
				formatted= message+" ";
			Assert.Fail(formatted+"expected same");
		}
	}
}
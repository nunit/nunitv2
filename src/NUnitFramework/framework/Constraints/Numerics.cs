// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
	/// <summary>
    /// Modes in which the tolerance value for a comparison can
    /// be interpreted.
    /// </summary>
    public enum ToleranceMode
    {
        /// <summary>
        /// The tolerance is used as a numeric range within which
        /// two compared values are considered to be equal.
        /// </summary>
        Linear,
        /// <summary>
        /// Interprets the tolerance as the percentage by which
        /// the two compared values my deviate from each other.
        /// </summary>
        Percent,
        /// <summary>
        /// Compares two values based in their distance in
        /// representable numbers.
        /// </summary>
        Ulps
    }

	/// <summary>
	/// The Numerics class contains common operations on numeric values.
	/// </summary>
	public class Numerics
	{
		#region Numeric Type Recognition
		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a numeric type</returns>
		public static bool IsNumericType(Object obj)
		{
			return IsFloatingPointNumeric( obj ) || IsFixedPointNumeric( obj );
		}

		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a floating point numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a floating point numeric type</returns>
		public static bool IsFloatingPointNumeric(Object obj)
		{
			if (null != obj)
			{
				if (obj is System.Double) return true;
				if (obj is System.Single) return true;
			}
			return false;
		}
		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a fixed point numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a fixed point numeric type</returns>
		public static bool IsFixedPointNumeric(Object obj)
		{
			if (null != obj)
			{
				if (obj is System.Byte) return true;
				if (obj is System.SByte) return true;
				if (obj is System.Decimal) return true;
				if (obj is System.Int32) return true;
				if (obj is System.UInt32) return true;
				if (obj is System.Int64) return true;
				if (obj is System.UInt64) return true;
				if (obj is System.Int16) return true;
				if (obj is System.UInt16) return true;
			}
			return false;
		}
		#endregion

		#region Numeric Equality
        /// <summary>
        /// Test two numeric values for equality, performing the usual numeric 
        /// conversions and using a provided or default tolerance. If the value 
        /// referred to by tolerance is null, this method may set it to a default.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="tolerance">A reference to the numeric tolerance in effect</param>
        /// <returns>True if the values are equal</returns>
		public static bool AreEqual( object expected, object actual, ref object tolerance )
		{
            return AreEqual( expected, actual, ToleranceMode.Linear, ref tolerance );
        }

        /// <summary>
        /// Test two numeric values for equality, performing the usual numeric 
        /// conversions and using a provided or default tolerance. If the value 
        /// referred to by tolerance is null, this method may set it to a default.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="mode">How to tolerance is to be interpreted</param>
        /// <param name="tolerance">A reference to the numeric tolerance in effect</param>
        /// <returns>True if the values are equal</returns>
		public static bool AreEqual( object expected, object actual, ToleranceMode mode, ref object tolerance )
		{
            if ( expected is double || actual is double )
                return AreEqual( Convert.ToDouble(expected), Convert.ToDouble(actual), mode, ref tolerance );

            if ( expected is float || actual is float )
                return AreEqual( Convert.ToSingle(expected), Convert.ToSingle(actual), mode, ref tolerance );

			if ( expected is decimal || actual is decimal )
				return AreEqual( Convert.ToDecimal(expected), Convert.ToDecimal(actual), mode, tolerance );
			
			if ( expected is ulong || actual is ulong )
				return AreEqual( Convert.ToUInt64(expected), Convert.ToUInt64(actual), mode, tolerance );
		
			if ( expected is long || actual is long )
				return AreEqual( Convert.ToInt64(expected), Convert.ToInt64(actual), mode, tolerance );
			
			if ( expected is uint || actual is uint )
				return AreEqual( Convert.ToUInt32(expected), Convert.ToUInt32(actual), mode, tolerance );

			return AreEqual( Convert.ToInt32(expected), Convert.ToInt32(actual), mode, tolerance );
		}

        private static bool AreEqual( double expected, double actual, ToleranceMode mode, ref object tolerance )
		{
            if (double.IsNaN(expected) && double.IsNaN(actual))
                return true;

            // Handle infinity specially since subtracting two infinite values gives 
            // NaN and the following test fails. mono also needs NaN to be handled
            // specially although ms.net could use either method. Also, handle
            // situation where no tolerance is used.
            if (double.IsInfinity(expected) || double.IsNaN(expected) || double.IsNaN(actual) ||
                tolerance == null && GlobalSettings.DefaultFloatingPointTolerance == 0.0d)
            {
                return expected.Equals(actual);
            }

            if (tolerance == null)
                tolerance = GlobalSettings.DefaultFloatingPointTolerance;

            switch (mode)
            {
                case ToleranceMode.Linear:
                {
                    return Math.Abs(expected - actual) <= Convert.ToDouble(tolerance);
                }
                case ToleranceMode.Percent:
                {
                    if (expected == 0.0)
                        return expected.Equals(actual);
                    
                    double relativeError = Math.Abs((expected - actual) / expected);
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                case ToleranceMode.Ulps:
                {
                    return FloatingPointNumerics.AreAlmostEqualUlps(
                        expected, actual, Convert.ToInt64(tolerance)
                    );
                }
                default:
                {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
        }

        private static bool AreEqual( float expected, float actual, ToleranceMode mode, ref object tolerance )
		{
            if ( float.IsNaN(expected) && float.IsNaN(actual) )
                return true;

            // handle infinity specially since subtracting two infinite values gives 
            // NaN and the following test fails. mono also needs NaN to be handled
            // specially although ms.net could use either method.
            if (float.IsInfinity(expected) || float.IsNaN(expected) || float.IsNaN(actual) ||
                 tolerance == null && GlobalSettings.DefaultFloatingPointTolerance == 0.0d)
            {
                return expected.Equals(actual);
            }

            if (tolerance == null)
                tolerance = GlobalSettings.DefaultFloatingPointTolerance;

            switch (mode)
            {
                case ToleranceMode.Linear:
                {
                    return Math.Abs(expected - actual) <= Convert.ToDouble(tolerance);
                }
                case ToleranceMode.Percent:
                {
                    if (expected == 0.0f)
                        return expected.Equals(actual);

                    float relativeError = Math.Abs((expected - actual) / expected);
                    return (relativeError <= Convert.ToSingle(tolerance) / 100.0f);
                }
                case ToleranceMode.Ulps:
		        {
                    return FloatingPointNumerics.AreAlmostEqualUlps(
                        expected, actual, Convert.ToInt32(tolerance)
                    );
                }
                default:
                {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
		}


        private static bool AreEqual( decimal expected, decimal actual, ToleranceMode mode, object tolerance )
        {
            switch (mode)
            {
                case ToleranceMode.Ulps:
                case ToleranceMode.Linear:
                {
                    decimal decimalTolerance = Convert.ToDecimal(tolerance);
                    if(decimalTolerance > 0m)
                      return Math.Abs(expected - actual) <= decimalTolerance;
				
			        return expected.Equals( actual );
		        }
                case ToleranceMode.Percent:
                {
                    if(expected == 0m)
                        return expected.Equals(actual);

                    double relativeError = Math.Abs(
                        (double)(expected - actual) / (double)expected);
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                default:
		        {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
        }

		private static bool AreEqual( ulong expected, ulong actual, ToleranceMode mode, object tolerance )
		{
            switch (mode)
            {
                case ToleranceMode.Ulps:
                case ToleranceMode.Linear:
                {
                    ulong ulongTolerance = Convert.ToUInt64(tolerance);
                    if(ulongTolerance > 0ul)
			        {
				        ulong diff = expected >= actual ? expected - actual : actual - expected;
                        return diff <= ulongTolerance;
			        }

			        return expected.Equals( actual );
		        }
                case ToleranceMode.Percent:
                {
                    if (expected == 0ul)
                        return expected.Equals(actual);

                    // Can't do a simple Math.Abs() here since it's unsigned
                    ulong difference = Math.Max(expected, actual) - Math.Min(expected, actual);
                    double relativeError = Math.Abs(
                        (double)difference / (double)expected
                    );
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                default:
		{
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
		}

		private static bool AreEqual( long expected, long actual, ToleranceMode mode, object tolerance )
		{
            switch (mode)
            {
                case ToleranceMode.Ulps:
                case ToleranceMode.Linear:
                {
                    long longTolerance = Convert.ToInt64(tolerance);
                    if(longTolerance > 0L)
				        return Math.Abs(expected - actual) <= longTolerance;

			        return expected.Equals( actual );
		        }
                case ToleranceMode.Percent:
                {
                    if(expected == 0L)
                        return expected.Equals(actual);

                    double relativeError = Math.Abs(
                        (double)(expected - actual) / (double)expected);
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                default:
		        {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
		}

		private static bool AreEqual( uint expected, uint actual, ToleranceMode mode, object tolerance )
		{
            switch (mode)
            {
                case ToleranceMode.Ulps:
                case ToleranceMode.Linear:
			    {
                    uint uintTolerance = Convert.ToUInt32(tolerance);
                    if(uintTolerance > 0)
			        {
				        uint diff = expected >= actual ? expected - actual : actual - expected;
                        return diff <= uintTolerance;
			        }
				
			        return expected.Equals( actual );
		        }
                case ToleranceMode.Percent:
                {
                    if(expected == 0u)
                        return expected.Equals(actual);

                    // Can't do a simple Math.Abs() here since it's unsigned
                    uint difference = Math.Max(expected, actual) - Math.Min(expected, actual);
                    double relativeError = Math.Abs(
                        (double)difference / (double)expected );
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                default:
		        {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
		}

		private static bool AreEqual( int expected, int actual, ToleranceMode mode, object tolerance )
		{
            switch (mode)
            {
                case ToleranceMode.Ulps:
                case ToleranceMode.Linear:
                {
                    int intTolerance = Convert.ToInt32(tolerance);
                    if(intTolerance > 0)
				        return Math.Abs(expected - actual) <= intTolerance;
				
			        return expected.Equals( actual );
		        }
                case ToleranceMode.Percent:
                {
                    if(expected == 0)
                        return expected.Equals(actual);

                    double relativeError = Math.Abs(
                        (double)(expected - actual) / (double)expected);
                    return (relativeError <= Convert.ToDouble(tolerance) / 100.0);
                }
                default:
                {
                    throw new ArgumentException("Unknown tolerance mode specified", "mode");
                }
            }
		}
		#endregion

		#region Numeric Comparisons 
        /// <summary>
        /// Compare two numeric values, performing the usual numeric conversions.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <returns>The relationship of the values to each other</returns>
		public static int Compare( IComparable expected, object actual )
		{
			if ( expected == null )
				throw new ArgumentException( "Cannot compare using a null reference", "expected" );

			if ( actual == null )
				throw new ArgumentException( "Cannot compare to null reference", "actual" );

			if( IsNumericType( expected ) && IsNumericType( actual ) )
			{
				if ( IsFloatingPointNumeric(expected) || IsFloatingPointNumeric(actual) )
					return Convert.ToDouble(expected).CompareTo(Convert.ToDouble(actual));

				if ( expected is decimal || actual is decimal )
					return Convert.ToDecimal(expected).CompareTo(Convert.ToDecimal(actual));
			
				if ( expected is ulong || actual is ulong )
					return Convert.ToUInt64(expected).CompareTo(Convert.ToUInt64(actual));
		
				if ( expected is long || actual is long )
					return Convert.ToInt64(expected).CompareTo(Convert.ToInt64(actual));
			
				if ( expected is uint || actual is uint )
					return Convert.ToUInt32(expected).CompareTo(Convert.ToUInt32(actual));

				return Convert.ToInt32(expected).CompareTo(Convert.ToInt32(actual));
			}
			else
				return expected.CompareTo(actual);
		}
		#endregion

		private Numerics()
		{
		}
	}
}

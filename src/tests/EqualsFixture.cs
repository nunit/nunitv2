#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, 
' Charlie Poole or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using NUnit.Framework;

namespace NUnit.Tests.Assertions
{
	[TestFixture]
	public class EqualsFixture
	{
		[Test]
		public void Equals()
		{
			string nunitString = "Hello NUnit";
			string expected = nunitString;
			string actual = nunitString;

			Assert.True(expected == actual);
			Assert.Equals(expected, actual);
		}

		[Test]
		public void EqualsNull() 
		{
			Assert.Equals(null, null);
		}
		
		[Test]
		public void Bug575936Int32Int64Comparison()
		{
			long l64 = 0;
			int i32 = 0;
			Assert.Equals(i32, l64);
		}
		
		[Test]
		public void IntegerLongComparison()
		{
			Assert.Equals(1, 1L);
			Assert.Equals(1L, 1);
		}

		[Test]
		public void IntergerEquals()
		{
			int val = 42;
			Assert.Equals(val, 42);
		}

		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void EqualsFail()
		{
			string junitString = "Goodbye JUnit";
			string expected = "Hello NUnit";

			Assert.False(expected.Equals(junitString));
			Assert.Equals(expected, junitString);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void EqualsNaNFails() 
		{
			Assert.Equals(1.234, Double.NaN, 0.0);
		}    


		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void NanEqualsFails() 
		{
			Assert.Equals(Double.NaN, 1.234, 0.0);
		}     
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void NanEqualsNaNFails() 
		{
			Assert.Equals(Double.NaN, Double.NaN, 0.0);
		}     

		[Test]
		public void NegInfinityEqualsInfinity() 
		{
			Assert.Equals(Double.NegativeInfinity, Double.NegativeInfinity, 0.0);
		}

		[Test]
		public void PosInfinityEqualsInfinity() 
		{
			Assert.Equals(Double.PositiveInfinity, Double.PositiveInfinity, 0.0);
		}
		
		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void PosInfinityNotEquals() 
		{
			Assert.Equals(Double.PositiveInfinity, 1.23, 0.0);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void PosInfinityNotEqualsNegInfinity() 
		{
			Assert.Equals(Double.PositiveInfinity, Double.NegativeInfinity, 0.0);
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]	
		public void SinglePosInfinityNotEqualsNegInfinity() 
		{
			Assert.Equals(float.PositiveInfinity, float.NegativeInfinity, (float)0.0);
		}
		
		[Test]
		public void Float() 
		{
			float val = (float)1.0;
			float expected = val;
			float actual = val;

			Assert.True(expected == actual);
			Assert.Equals(expected, actual, (float)0.0);
		}

		[Test]
		public void Byte() 
		{
			byte val = 1;
			byte expected = val;
			byte actual = val;

			Assert.True(expected == actual);
			Assert.Equals(expected, actual);
		}

		[Test]
		public void String() 
		{
			string s1 = "test";
			string s2 = new System.Text.StringBuilder(s1).ToString();

			Assert.True(s1.Equals(s2));
			Assert.Equals(s1,s2);
		}

		[Test]
		public void Short() 
		{
			short val = 1;
			short expected = val;
			short actual = val;

			Assert.True(expected == actual);
			Assert.Equals(expected, actual);
		}

		[Test]
		public void Int() 
		{
			int val = 1;
			int expected = val;
			int actual = val;

			Assert.True(expected == actual);
			Assert.Equals(expected, actual);
		}

		
		/// <summary>
		/// Checks to see that a value comparison works with all types.
		/// Current version has problems when value is the same but the
		/// types are different...C# is not like Java, and doesn't automatically
		/// perform value type conversion to simplify this type of comparison.
		/// 
		/// Related to Bug575936Int32Int64Comparison, but covers all numeric
		/// types.
		/// </summary>
		[Test]
		public void EqualsSameTypes()
		{
			byte      b1 = 35;
			sbyte    sb2 = 35;
			decimal   d4 = 35;
			double    d5 = 35;
			float     f6 = 35;
			int       i7 = 35;
			uint      u8 = 35;
			long      l9 = 35;
			short    s10 = 35;
			ushort  us11 = 35;
		
			System.Byte    b12  = 35;  
			System.SByte   sb13 = 35; 
			System.Decimal d14  = 35; 
			System.Double  d15  = 35; 
			System.Single  s16  = 35; 
			System.Int32   i17  = 35; 
			System.UInt32  ui18 = 35; 
			System.Int64   i19  = 35; 
			System.UInt64  ui20 = 35; 
			System.Int16   i21  = 35; 
			System.UInt16  i22  = 35;
		
			Assert.Equals( 35, b1 );
			Assert.Equals( 35, sb2 );
			Assert.Equals( 35, d4 );
			Assert.Equals( 35, d5 );
			Assert.Equals( 35, f6 );
			Assert.Equals( 35, i7 );
			Assert.Equals( 35, u8 );
			Assert.Equals( 35, l9 );
			Assert.Equals( 35, s10 );
			Assert.Equals( 35, us11 );
		
			Assert.Equals( 35, b12  );
			Assert.Equals( 35, sb13 );
			Assert.Equals( 35, d14  );
			Assert.Equals( 35, d15  );
			Assert.Equals( 35, s16  );
			Assert.Equals( 35, i17  );
			Assert.Equals( 35, ui18 );
			Assert.Equals( 35, i19  );
			Assert.Equals( 35, ui20 );
			Assert.Equals( 35, i21  );
			Assert.Equals( 35, i22  );
		}
	}
}


//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Tests
{
	using System;
	using Nunit.Framework;

	[TestFixture]
	public class AssertionTest
	{
		private string expected;

		[SetUp]
		public void SetUp() 
		{
			expected = "Hello NUnit";
		}

		[Test]
		public void AssertEquals()
		{
			Assertion.AssertEquals(expected, expected);
		}

		[Test]
		public void IntegerLongComparison()
		{
			Assertion.AssertEquals(1, 1L);
			Assertion.AssertEquals(1L, 1);
		}

		[Test]
		public void AssertEqualsFail()
		{
			string actual = "Goodbye JUnit";
			Assertion.Assert(!expected.Equals(actual));
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AssertEqualsTestCaseFail()
		{
			string actual = "Goodbye JUnit";
			Assertion.AssertEquals("should not be equal", expected, actual);
		}


		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertEqualsNaNFails() 
		{
			try 
			{
				Assertion.AssertEquals(1.234, Double.NaN, 0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}    

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertEqualsNull() 
		{
			Assertion.AssertEquals(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertNanEqualsFails() 
		{
			try 
			{
				Assertion.AssertEquals(Double.NaN, 1.234, 0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}     

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertNanEqualsNaNFails() 
		{
			try 
			{
				Assertion.AssertEquals(Double.NaN, Double.NaN, 0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}     

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertNegInfinityEqualsInfinity() 
		{
			Assertion.AssertEquals(Double.NegativeInfinity, Double.NegativeInfinity, 0.0);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertPosInfinityEqualsInfinity() 
		{
			Assertion.AssertEquals(Double.PositiveInfinity, Double.PositiveInfinity, 0.0);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertPosInfinityNotEquals() 
		{
			try 
			{
				Assertion.AssertEquals(Double.PositiveInfinity, 1.23, 0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertPosInfinityNotEqualsNegInfinity() 
		{
			try 
			{
				Assertion.AssertEquals(Double.PositiveInfinity, Double.NegativeInfinity, 0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertSinglePosInfinityNotEqualsNegInfinity() 
		{
			try 
			{
				Assertion.AssertEquals(float.PositiveInfinity, float.NegativeInfinity, (float)0.0);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertSingle() 
		{
			Assertion.AssertEquals((float)1.0, (float)1.0, (float)0.0);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertByte() 
		{
			Assertion.AssertEquals((byte)1, (byte)1);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertString() 
		{
			string s1 = "test";
			string s2 = new System.Text.StringBuilder(s1).ToString();
			Assertion.AssertEquals(s1,s2);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertShort() 
		{
			Assertion.AssertEquals((short)1,(short)1);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertNull() 
		{
			Assertion.AssertNull(null);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertNullNotEqualsNull() 
		{
			try 
			{
				Assertion.AssertEquals(null, new Object());
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertSame() 
		{
			Object o= new Object();
			Assertion.AssertSame(o, o);
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void AssertSameFails() 
		{
			try 
			{
				object one = 1;
				object two = 1;
				Assertion.AssertSame(one, two);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void Fail() 
		{
			try 
			{
				Assertion.Fail();
			} 
			catch (AssertionException) 
			{
				return;
			}
			throw new AssertionException("fail"); // You can't call fail() here
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void FailAssertNotNull() 
		{
			try 
			{
				Assertion.AssertNotNull(null);
			} 
			catch (AssertionException) 
			{
				return;
			}
			Assertion.Fail();
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		[Test]
		public void SucceedAssertNotNull() 
		{
			Assertion.AssertNotNull(new Object());
		}
	}
}

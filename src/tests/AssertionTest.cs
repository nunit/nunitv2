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

namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;

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
        public void Bug575936Int32Int64Comparison()
        {
            long l64 = 0;
            int i32 = 0;
            Assertion.AssertEquals(i32, l64);
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

        [Test]
        public void Bug561909FailInheritsFromSystemException() 
        {
            try 
            {
                Assertion.Fail();
            } 
            catch (System.Exception) 
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

        [TestFixture]
            internal class VerifyFailThrowsException
        {
            internal string failureMessage;

            [Test]
            public void CallAssertionFail()
            {
                Assertion.Fail(failureMessage);
            }
        }

        [Test]
        public void VerifyFailIsCalled()
        {
            string failureMessage = "this should call fail";
            VerifyFailThrowsException verifyFail = new VerifyFailThrowsException();
            verifyFail.failureMessage = failureMessage;

            NUnit.Core.Test test = NUnit.Core.TestCaseBuilder.Make(verifyFail, "CallAssertionFail");
            NUnit.Core.TestResult result = test.Run(NUnit.Core.NullListener.NULL);
            Assertion.Assert("VerifyFailThrowsException should have failed", result.IsFailure);
            Assertion.AssertEquals(failureMessage, result.Message);
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
        public void AssertEqualsSameTypes()
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

            Assertion.AssertEquals( 35, b1 );
            Assertion.AssertEquals( 35, sb2 );
            Assertion.AssertEquals( 35, d4 );
            Assertion.AssertEquals( 35, d5 );
            Assertion.AssertEquals( 35, f6 );
            Assertion.AssertEquals( 35, i7 );
            Assertion.AssertEquals( 35, u8 );
            Assertion.AssertEquals( 35, l9 );
            Assertion.AssertEquals( 35, s10 );
            Assertion.AssertEquals( 35, us11 );

            Assertion.AssertEquals( 35, b12  );
            Assertion.AssertEquals( 35, sb13 );
            Assertion.AssertEquals( 35, d14  );
            Assertion.AssertEquals( 35, d15  );
            Assertion.AssertEquals( 35, s16  );
            Assertion.AssertEquals( 35, i17  );
            Assertion.AssertEquals( 35, ui18 );
            Assertion.AssertEquals( 35, i19  );
            Assertion.AssertEquals( 35, ui20 );
            Assertion.AssertEquals( 35, i21  );
            Assertion.AssertEquals( 35, i22  );
        }
    }
}

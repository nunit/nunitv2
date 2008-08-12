using System;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        public class EqualityTests
        {
            [Test]
            public void SimpleEqualityTests()
            {
                int[] i3 = new int[] { 1, 2, 3 };
                double[] d3 = new double[] { 1.0, 2.0, 3.0 };
                int[] iunequal = new int[] { 1, 3, 2 };

                Assert.AreEqual(4, 2 + 2);
                Assert.AreEqual(i3, d3);
                Assert.AreNotEqual(5, 2 + 2);
                Assert.AreNotEqual(i3, iunequal);
#if NET_2_0
                List<string> list = new List<string>();
                list.Add("foo");
                list.Add("bar");
                Assert.AreEqual(new string[] { "foo", "bar" }, list);
#endif
            }

            [Test]
            public void EqualityTestsWithTolerance()
            {
                Assert.AreEqual(5.0d, 4.99d, 0.05d);
                Assert.AreEqual(5.0f, 4.99f, 0.05f);
            }

            [Test]
            public void EqualityTestsUsingDefaultFloatingPointTolerance()
            {
                GlobalSettings.DefaultFloatingPointTolerance = 0.05d;

                try
                {
                    Assert.AreEqual(5.0d, 4.99d);
                    Assert.AreEqual(5.0f, 4.99f);
                }
                finally
                {
                    GlobalSettings.DefaultFloatingPointTolerance = 0.0d;
                }
            }
        }
    }

    namespace Helpers
    {
        public class EqualityTests
        {
            [Test]
            public void SimpleEqualityTests()
            {
                int[] i3 = new int[] { 1, 2, 3 };
                double[] d3 = new double[] { 1.0, 2.0, 3.0 };
                int[] iunequal = new int[] { 1, 3, 2 };

                Assert.That(2 + 2, Is.EqualTo(4));
                Assert.That(2 + 2 == 4);
                Assert.That(i3, Is.EqualTo(d3));
                Assert.That(2 + 2, Is.Not.EqualTo(5));
                Assert.That(i3, Is.Not.EqualTo(iunequal));
#if NET_2_0
                List<string> list = new List<string>();
                list.Add("foo");
                list.Add("bar");
                Assert.That(list, Is.EqualTo(new string[] { "foo", "bar" }));
#endif
            }

            [Test]
            public void EqualityTestsWithTolerance()
            {
                Assert.That(4.99d, Is.EqualTo(5.0d).Within(0.05d));
                Assert.That(4.0d, Is.Not.EqualTo(5.0d).Within(0.5d));
                Assert.That(4.99f, Is.EqualTo(5.0f).Within(0.05f));
                Assert.That(4.99m, Is.EqualTo(5.0m).Within(0.05m));
                Assert.That(3999999999u, Is.EqualTo(4000000000u).Within(5u));
                Assert.That(499, Is.EqualTo(500).Within(5));
                Assert.That(4999999999L, Is.EqualTo(5000000000L).Within(5L));
                Assert.That(5999999999ul, Is.EqualTo(6000000000ul).Within(5ul));
            }

            [Test]
            public void EqualityTestsWithTolerance_MixedFloatAndDouble()
            {
                // Bug Fix 1743844
                Assert.That(2.20492d, Is.EqualTo(2.2d).Within(0.01f),
                    "Double actual, Double expected, Single tolerance");
                Assert.That(2.20492d, Is.EqualTo(2.2f).Within(0.01d),
                    "Double actual, Single expected, Double tolerance");
                Assert.That(2.20492d, Is.EqualTo(2.2f).Within(0.01f),
                    "Double actual, Single expected, Single tolerance");
                Assert.That(2.20492f, Is.EqualTo(2.2f).Within(0.01d),
                    "Single actual, Single expected, Double tolerance");
                Assert.That(2.20492f, Is.EqualTo(2.2d).Within(0.01d),
                    "Single actual, Double expected, Double tolerance");
                Assert.That(2.20492f, Is.EqualTo(2.2d).Within(0.01f),
                    "Single actual, Double expected, Single tolerance");
            }

            [Test]
            public void EqualityTestsWithTolerance_MixingTypesGenerally()
            {
                // Extending tolerance to all numeric types
                Assert.That(202d, Is.EqualTo(200d).Within(2),
                    "Double actual, Double expected, int tolerance");
                Assert.That(4.87m, Is.EqualTo(5).Within(.25),
                    "Decimal actual, int expected, Double tolerance");
                Assert.That(4.87m, Is.EqualTo(5ul).Within(1),
                    "Decimal actual, ulong expected, int tolerance");
                Assert.That(487, Is.EqualTo(500).Within(25),
                    "int actual, int expected, int tolerance");
                Assert.That(487u, Is.EqualTo(500).Within(25),
                    "uint actual, int expected, int tolerance");
                Assert.That(487L, Is.EqualTo(500).Within(25),
                    "long actual, int expected, int tolerance");
                Assert.That(487ul, Is.EqualTo(500).Within(25),
                    "ulong actual, int expected, int tolerance");
            }

            [Test]
            public void EqualityTestsUsingDefaultFloatingPointTolerance()
            {
                GlobalSettings.DefaultFloatingPointTolerance = 0.05d;

                try
                {
                    Assert.That(4.99d, Is.EqualTo(5.0d));
                    Assert.That(4.0d, Is.Not.EqualTo(5.0d));
                    Assert.That(4.99f, Is.EqualTo(5.0f));
                }
                finally
                {
                    GlobalSettings.DefaultFloatingPointTolerance = 0.0d;
                }
            }
        }
    }

    namespace Inherited
    {
        public class EqualityTests : AssertionHelper
        {
            [Test]
            public void SimpleEqualityTests()
            {
                int[] i3 = new int[] { 1, 2, 3 };
                double[] d3 = new double[] { 1.0, 2.0, 3.0 };
                int[] iunequal = new int[] { 1, 3, 2 };

                Expect(2 + 2, EqualTo(4));
                Expect(2 + 2 == 4);
                Expect(i3, EqualTo(d3));
                Expect(2 + 2, Not.EqualTo(5));
                Expect(i3, Not.EqualTo(iunequal));
#if NET_2_0
                List<string> list = new List<string>();
                list.Add("foo");
                list.Add("bar");
                Expect(list, EqualTo(new string[] { "foo", "bar" }));
#endif
            }

            [Test]
            public void EqualityTestsWithTolerance()
            {
                Expect(4.99d, EqualTo(5.0d).Within(0.05d));
                Expect(4.0d, Not.EqualTo(5.0d).Within(0.5d));
                Expect(4.99f, EqualTo(5.0f).Within(0.05f));
                Expect(4.99m, EqualTo(5.0m).Within(0.05m));
                Expect(499u, EqualTo(500u).Within(5u));
                Expect(499, EqualTo(500).Within(5));
                Expect(4999999999L, EqualTo(5000000000L).Within(5L));
                Expect(5999999999ul, EqualTo(6000000000ul).Within(5ul));
            }

            [Test]
            public void EqualityTestsWithTolerance_MixedFloatAndDouble()
            {
                // Bug Fix 1743844
                Expect(2.20492d, EqualTo(2.2d).Within(0.01f),
                    "Double actual, Double expected, Single tolerance");
                Expect(2.20492d, EqualTo(2.2f).Within(0.01d),
                    "Double actual, Single expected, Double tolerance");
                Expect(2.20492d, EqualTo(2.2f).Within(0.01f),
                    "Double actual, Single expected, Single tolerance");
                Expect(2.20492f, EqualTo(2.2f).Within(0.01d),
                    "Single actual, Single expected, Double tolerance");
                Expect(2.20492f, EqualTo(2.2d).Within(0.01d),
                    "Single actual, Double expected, Double tolerance");
                Expect(2.20492f, EqualTo(2.2d).Within(0.01f),
                    "Single actual, Double expected, Single tolerance");
            }

            [Test]
            public void EqualityTestsWithTolerance_MixingTypesGenerally()
            {
                // Extending tolerance to all numeric types
                Expect(202d, EqualTo(200d).Within(2),
                    "Double actual, Double expected, int tolerance");
                Expect(4.87m, EqualTo(5).Within(.25),
                    "Decimal actual, int expected, Double tolerance");
                Expect(4.87m, EqualTo(5ul).Within(1),
                    "Decimal actual, ulong expected, int tolerance");
                Expect(487, EqualTo(500).Within(25),
                    "int actual, int expected, int tolerance");
                Expect(487u, EqualTo(500).Within(25),
                    "uint actual, int expected, int tolerance");
                Expect(487L, EqualTo(500).Within(25),
                    "long actual, int expected, int tolerance");
                Expect(487ul, EqualTo(500).Within(25),
                    "ulong actual, int expected, int tolerance");
            }

            [Test]
            public void EqualityTestsUsingDefaultFloatingPointTolerance()
            {
                GlobalSettings.DefaultFloatingPointTolerance = 0.05d;

                try
                {
                    Expect(4.99d, EqualTo(5.0d));
                    Expect(4.0d, Not.EqualTo(5.0d));
                    Expect(4.99f, EqualTo(5.0f));
                }
                finally
                {
                    GlobalSettings.DefaultFloatingPointTolerance = 0.0d;
                }
            }
        }
    }
}

// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class NumericsTest
    {
        [TestCase(123456789)]
        [TestCase(123456789U)]
        [TestCase(123456789L)]
        [TestCase(123456789UL)]
        [TestCase(1234.5678f)]
        [TestCase(1234.5678)]
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ErrorOnInvalidToleranceMode(object value)
        {
            object tolerance = 0;
            Assert.IsTrue(Numerics.AreEqual(value, value, (ToleranceMode)(-1), ref tolerance));
        }

        // Separate test case because you can't use decimal in an attribute (24.1.3)
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ErrorOnInvalidToleranceModeForDecimal()
        {
            object tolerance = 0;
            Assert.IsTrue(Numerics.AreEqual(123m, 123m, (ToleranceMode)(-1), ref tolerance));
        }

        [TestCase(123456789)]
        [TestCase(123456789U)]
        [TestCase(123456789L)]
        [TestCase(123456789UL)]
        [TestCase(1234.5678f)]
        [TestCase(1234.5678)]
        [Test]
        public void CanMatchWithoutToleranceMode(object value)
        {
            object tolerance = 0;
            Assert.IsTrue(Numerics.AreEqual(value, value, ref tolerance));
        }

        // Separate test case because you can't use decimal in an attribute (24.1.3)
        [Test]
        public void CanMatchDecimalWithoutToleranceMode()
        {
            object tolerance = 0;
            Assert.IsTrue(Numerics.AreEqual(123m, 123m, ref tolerance));
        }

        [TestCase((int)9500)]
        [TestCase((int)10000)]
        [TestCase((int)10500)]
        [TestCase((uint)9500)]
        [TestCase((uint)10000)]
        [TestCase((uint)10500)]
        [TestCase((long)9500)]
        [TestCase((long)10000)]
        [TestCase((long)10500)]
        [TestCase((ulong)9500)]
        [TestCase((ulong)10000)]
        [TestCase((ulong)10500)]
        [Test]
        public void CanMatchIntegralsWithPercentage(object value)
        {
            object tolerance = 10.0;
            Assert.IsTrue(Numerics.AreEqual(10000, value, ToleranceMode.Percent, ref tolerance));
        }

        [Test]
        public void CanMatchDecimalWithPercentage()
        {
            object tolerance = 10.0;
            Assert.IsTrue(Numerics.AreEqual(10000m, 9500m, ToleranceMode.Percent, ref tolerance));
            Assert.IsTrue(Numerics.AreEqual(10000m, 10000m, ToleranceMode.Percent, ref tolerance));
            Assert.IsTrue(Numerics.AreEqual(10000m, 10500m, ToleranceMode.Percent, ref tolerance));
        }

        [TestCase((int)8500)]
        [TestCase((int)11500)]
        [TestCase((uint)8500)]
        [TestCase((uint)11500)]
        [TestCase((long)8500)]
        [TestCase((long)11500)]
        [TestCase((ulong)8500)]
        [TestCase((ulong)11500)]
        [Test, ExpectedException(typeof(AssertionException))]
        public void FailsOnIntegralsOutsideOfPercentage(object value)
        {
            object tolerance = 10.0;
            Assert.IsTrue(Numerics.AreEqual(10000, value, ToleranceMode.Percent, ref tolerance));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailsOnDecimalBelowPercentage()
        {
            object tolerance = 10.0;
            Assert.IsTrue(Numerics.AreEqual(10000m, 8500m, ToleranceMode.Percent, ref tolerance));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void FailsOnDecimalAbovePercentage()
        {
            object tolerance = 10.0;
            Assert.IsTrue(Numerics.AreEqual(10000m, 11500m, ToleranceMode.Percent, ref tolerance));
        }

    }
}
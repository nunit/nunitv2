// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Collections;
#if CLR_2_0 || CLR_4_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class EqualityComparerTests
    {
        private Tolerance tolerance;
        private NUnitEqualityComparer comparer;

        [SetUp]
        public void Setup()
        {
            tolerance = Tolerance.Zero;
            comparer = new NUnitEqualityComparer();
        }

        [Test]
        public void CanCompareArrayContainingSelfToSelf()
        {
            object[] array = new object[1];
            array[0] = array;

            Assert.True(comparer.AreEqual(array, array, ref tolerance));
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void SelfContainingRecursiveEnumerablesAreNotEqual()
        {
            var a1 = new object[1];
            var a2 = new object[1];
			a1[0] = a1;
			a2[0] = a2;

            Assert.False(comparer.AreEqual(a1, a2, ref tolerance));
        }

        [Test]
        public void CrossReferencingRecursiveEnumerablesAreNotEqual()
        {
            var a1 = new object[1];
            var a2 = new object[1];
            a1[0] = a2;
            a2[0] = a1;

            Assert.False(comparer.AreEqual(a1, a2, ref tolerance));
        }

		[Test]
		public void RecursionCheckDoesNotRelyOnValueEquality()
		{
			var a1 = new StructEnumerable<int>[2];
			var a2 = new StructEnumerable<int>[2];

			a1[0] = a2[0] = a1[1] = a2[1] = new StructEnumerable<int>(1);

			Assert.True(comparer.AreEqual(a1, a2, ref tolerance));
		}

        [Test]
        public void IEquatableSuccess()
        {
            var x = new IEquatableWithoutEqualsOverridden(1);
            var y = new IEquatableWithoutEqualsOverridden(1);

            Assert.IsTrue(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableDifferentTypesSuccess_WhenActualImplementsIEquatable()
        {
            var x = 1;
            var y = new Int32IEquatable(1);

            // y.Equals(x) is what gets actually called
            // TODO: This should work both ways
            Assert.IsTrue(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableDifferentTypesSuccess_WhenExpectedImplementsIEquatable()
        {
            var x = 1;
            var y = new Int32IEquatable(1);

            // y.Equals(x) is what gets actually called
            // TODO: This should work both ways
            Assert.IsTrue(comparer.AreEqual(y, x, ref tolerance));
        }

        [Test]
        public void ReferenceEqualityHasPrecedenceOverIEquatable()
        {
            var z = new NeverEqualIEquatable();

            Assert.IsTrue(comparer.AreEqual(z, z, ref tolerance));
        }

        [Test]
        public void IEquatableHasPrecedenceOverDefaultEquals()
        {
            var x = new NeverEqualIEquatableWithOverriddenAlwaysTrueEquals();
            var y = new NeverEqualIEquatableWithOverriddenAlwaysTrueEquals();

            Assert.IsFalse(comparer.AreEqual(x, y, ref tolerance));
        }
#endif
    }

#if CLR_2_0 || CLR_4_0
    public class NeverEqualIEquatableWithOverriddenAlwaysTrueEquals : IEquatable<NeverEqualIEquatableWithOverriddenAlwaysTrueEquals>
    {
        public bool Equals(NeverEqualIEquatableWithOverriddenAlwaysTrueEquals other)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Int32IEquatable : IEquatable<int>
    {
        private readonly int value;

        public Int32IEquatable(int value)
        {
            this.value = value;
        }

        public bool Equals(int other)
        {
            return value.Equals(other);
        }
    }

    public class NeverEqualIEquatable : IEquatable<NeverEqualIEquatable>
    {
        public bool Equals(NeverEqualIEquatable other)
        {
            return false;
        }
    }

    public class IEquatableWithoutEqualsOverridden : IEquatable<IEquatableWithoutEqualsOverridden>
    {
        private readonly int value;

        public IEquatableWithoutEqualsOverridden(int value)
        {
            this.value = value;
        }

        public bool Equals(IEquatableWithoutEqualsOverridden other)
        {
            return value.Equals(other.value);
        }
    }

	struct StructEnumerable<T> : IEnumerable<T>
	{
		public readonly T Value;

		public StructEnumerable(T value)
		{
			Value = value;
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return Value;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
#endif
}

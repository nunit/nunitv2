// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************
using System;
using NUnit.Framework;

namespace NUnit.Framework.Tests
{
    [TestFixture]
    public class AssumeThatTests
    {
        [Test]
        public void AssumptionPasses_Boolean()
        {
            Assume.That(2 + 2 == 4);
        }

        [Test]
        public void AssumptionPasses_ActualAndConstraint()
        {
            Assume.That(2 + 2, Is.EqualTo(4));
        }

        [Test]
        public void AssumptionPasses_ReferenceAndConstraint()
        {
            bool value = true;
            Assume.That(ref value, Is.True);
        }

        [Test]
        public void AssumptionPasses_DelegateAndConstraint()
        {
            Assume.That(new Constraints.ActualValueDelegate(ReturnsFour), Is.EqualTo(4));
        }

        private object ReturnsFour()
        {
            return 4;
        }

        [Test, ExpectedException(typeof(InconclusiveException))]
        public void FailureThrowsInconclusiveException_Boolean()
        {
            Assume.That(2 + 2 == 5);
        }

        [Test, ExpectedException(typeof(InconclusiveException))]
        public void FailureThrowsInconclusiveException_ActualAndConstraint()
        {
            Assume.That(2 + 2, Is.EqualTo(5));
        }

        [Test, ExpectedException(typeof(InconclusiveException))]
        public void FailureThrowsInconclusiveException_ReferenceAndConstraint()
        {
            bool value = false;
            Assume.That(ref value, Is.True);
        }

        [Test, ExpectedException(typeof(InconclusiveException))]
        public void FailureThrowsInconclusiveException_DelegateAndConstraint()
        {
            Assume.That(new Constraints.ActualValueDelegate(ReturnsFive), Is.EqualTo(4));
        }

        private object ReturnsFive()
        {
            return 5;
        }
    }
}

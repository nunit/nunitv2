using System;

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        public class TypeConstraintTests
        {
            [Test]
            public void ExactType()
            {
                // Classic syntax workarounds
                Assert.AreEqual(typeof(string), "Hello".GetType());
                Assert.AreEqual("System.String", "Hello".GetType().FullName);
                Assert.AreNotEqual(typeof(int), "Hello".GetType());
                Assert.AreNotEqual("System.Int32", "Hello".GetType().FullName);
            }

            [Test]
            public void InstanceOfType()
            {
                Assert.IsInstanceOfType(typeof(string), "Hello");
                Assert.IsNotInstanceOfType(typeof(string), 5);
            }

            [Test]
            public void AssignableFromType()
            {
                Assert.IsAssignableFrom(typeof(string), "Hello");
                Assert.IsNotAssignableFrom(typeof(string), 5);
            }
        }
    }

    namespace Helpers
    {
        public class TypeConstraintTests
        {
            [Test]
            public void ExactType()
            {
                Assert.That("Hello", Is.TypeOf(typeof(string)));
                Assert.That("Hello", Is.Not.TypeOf(typeof(int)));
            }

            [Test]
            public void InstanceOfType()
            {
                Assert.That("Hello", Is.InstanceOfType(typeof(string)));
                Assert.That(5, Is.Not.InstanceOfType(typeof(string)));
            }

            [Test]
            public void AssignableFromType()
            {
                Assert.That("Hello", Is.AssignableFrom(typeof(string)));
                Assert.That(5, Is.Not.AssignableFrom(typeof(string)));
            }
        }
    }

    namespace Inherited
    {
        public class TypeConstraintTests : AssertionHelper
        {
            [Test]
            public void ExactType()
            {
                Expect("Hello", TypeOf(typeof(string)));
                Expect("Hello", Not.TypeOf(typeof(int)));
            }

            [Test]
            public void InstanceOfType()
            {
                Expect("Hello", InstanceOfType(typeof(string)));
                Expect(5, Not.InstanceOfType(typeof(string)));
            }

            [Test]
            public void AssignableFromType()
            {
                Expect("Hello", AssignableFrom(typeof(string)));
                Expect(5, Not.AssignableFrom(typeof(string)));
            }
        }
    }
}

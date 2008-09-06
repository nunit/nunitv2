using System;

namespace NUnit.Framework.Syntax
{
    [TestFixture]
    public class ThrowsTests
    {
        [Test]
        public void ThrowsException()
        {
            Assert.AreEqual(
                "<throws>",
                Throws.Exception.ToString());
        }

        [Test]
        public void ThrowsExceptionWithConstraint()
        {
            Assert.AreEqual(
                @"<throws <property ParamName <equal ""myParam"">>>",
                Throws.Exception.With.Property("ParamName").EqualTo("myParam").ToString());
        }

        [Test]
        public void ThrowsExceptionTypeOf()
        {
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                Throws.Exception.TypeOf(typeof(ArgumentException)).ToString());
        }

        [Test]
        public void ThrowsTypeOf()
        {
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                Throws.TypeOf(typeof(ArgumentException)).ToString());
        }

        [Test]
        public void ThrowsTypeOfAndConstraint()
        {
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                Throws.TypeOf(typeof(ArgumentException)).And.Property("ParamName").EqualTo("myParam").ToString());
        }

        [Test]
        public void ThrowsExceptionTypeOfAndConstraint()
        {
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                Throws.Exception.TypeOf(typeof(ArgumentException)).And.Property("ParamName").EqualTo("myParam").ToString());
        }

        [Test]
        public void ThrowsTypeOfWithConstraint()
        {
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                Throws.TypeOf(typeof(ArgumentException)).With.Property("ParamName").EqualTo("myParam").ToString());
        }

        [Test]
        public void ThrowsInstanceOf()
        {
            Assert.AreEqual(
                "<throws <instanceof System.ArgumentException>>",
                Throws.InstanceOf(typeof(ArgumentException)).ToString());
        }

        [Test]
        public void ThrowsExceptionInstanceOf()
        {
            Assert.AreEqual(
                "<throws <instanceof System.ArgumentException>>",
                Throws.Exception.InstanceOf(typeof(ArgumentException)).ToString());
        }
    }
}

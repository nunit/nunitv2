using System;

namespace NUnit.Framework.Syntax
{
    [TestFixture]
    public class ThrowsTests
    {
        [Test]
        public void ThrowsExceptionTest()
        {
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                Throws.Exception(typeof(ArgumentException)).ToString() );
        }

        [Test]
        public void ThrowsTypeOf()
        {
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                Throws.TypeOf(typeof(ArgumentException)).ToString());
        }

        [Test]
        public void ThrowsTypeOfWithConstraint_UsingAnd()
        {
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                Throws.TypeOf(typeof(ArgumentException)).And.Property("ParamName").EqualTo("myParam").ToString());
        }

        //[Test]
        //public void ThrowsTypeOfWithConstraint_UsingWith()
        //{
        //    Assert.AreEqual(
        //        @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
        //        Throws.TypeOf(typeof(ArgumentException)).With.Property("ParamName").EqualTo("myParam").ToString());
        //}

        [Test]
        public void ThrowsInstanceOf()
        {
            Assert.AreEqual(
                "<throws <instanceof System.ArgumentException>>",
                Throws.InstanceOf(typeof(ArgumentException)).ToString() );
        }
    }
}

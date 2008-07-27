#if NET_2_0
using System;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace NUnit.Core.Tests.Generic
{
    [Category("Generics")]
    [TestFixture(typeof(double))]
    public class TypeParameterUsedWithTestMethod<T>
    {
        [TestCase(5)]
        [TestCase(1.23)]
        public void TestMyArgType(T x)
        {
            Assert.That(x, Is.TypeOf(typeof(T)));
        }
    }
}
#endif
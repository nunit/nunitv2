#if NET_2_0
using System;
using NUnit.Framework;

namespace NUnit.Core.Tests.Generic
{
    [Category("Generics")]
    [TestFixture(100.0, 42)]
    [TestFixture(42, 100.0)]
    public class DeduceTypeArgsFromArgs<T1, T2>
    {
        T1 t1;
        T2 t2;

        public DeduceTypeArgsFromArgs(T1 t1, T2 t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }

        [TestCase(5, 7)]
        [TestCase(1.23, 0)]
        public void TestMyArgTypes(T1 t1, T2 t2)
        {
            Assert.That(t1, Is.TypeOf<T1>());
            Assert.That(t2, Is.TypeOf<T2>());
        }
    }
}
#endif


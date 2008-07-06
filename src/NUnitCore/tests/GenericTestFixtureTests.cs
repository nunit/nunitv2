#if NET_2_0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture(typeof(List<int>))]
    [TestFixture(typeof(ArrayList))]
    public class GenericTestFixtureTests<T> where T : IList, new()
    {
        [Test]
        public void TestCollectionCount()
        {
            IList list = new T();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }
    }
}
#endif
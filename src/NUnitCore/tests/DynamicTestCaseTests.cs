using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class DynamicTestCaseTests
    {
        // TODO: Make this work for instance data
        static object[] data;

        [SetUp]
        public void InitializeData()
        {
            data = new object[] {
                new int[] { 3, 3 },
                new int[] { 7, 7 } };
        }
        
        [DynamicTest, Factory("data")]
        public void DynamicTestCanRunFromFactoryInitializedAtRuntime( int x, int y )
        {
            Assert.IsNotNull(data);
            Assert.AreEqual(x, y);
        }
    }
}

using NUnit.Framework;

namespace NUnit.Core.Tests
{
    [TestFixture]
    public class AssertPassFixture
    {
        [Test]
        public void AssertPassReturnsSuccess()
        {
            Assert.Pass("This test is OK!");
        }

        [Test]
        public void SubsequentFailureIsIrrelevant()
        {
            Assert.Pass("This test is OK!");
            Assert.Fail("No it's NOT!");
        }
    }
}

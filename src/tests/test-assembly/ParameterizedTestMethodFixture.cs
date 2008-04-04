using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class ParameterizedTestMethodFixture
    {
        [TestCase(2,3,4,Description="My Description")]
        public void MethodHasDescriptionSpecified(int x, int y, int z)
        {}

        [TestCase(2,3,4,TestName="XYZ")]
        public void MethodHasTestNameSpecified(int x, int y, int z)
        {}
    }
}

using NUnit.Framework;
using NUnit.Framework.Extensions;

namespace NUnit.TestData
{
    [TestFixture]
    public class MaxTimeFixture
    {
        [Test, MaxTime(1)]
        public void MaxTimeExceeded()
        {
            System.Threading.Thread.Sleep(1000);
        }
    }
}

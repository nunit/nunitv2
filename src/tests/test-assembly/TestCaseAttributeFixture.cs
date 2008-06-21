using System;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class TestCaseAttributeFixture
    {
        [TestCase(2,3,4,Description="My Description")]
        public void MethodHasDescriptionSpecified(int x, int y, int z)
        {}

		[TestCase(2,3,4,TestName="XYZ")]
		public void MethodHasTestNameSpecified(int x, int y, int z)
		{}
 
		[TestCase(2, 2000000, Result=4)]
		public int MethodCausesConversionOverflow(short x, short y)
		{
			return x + y;
		}

		[TestCase("12-Octobar-1942")]
		public void MethodHasInvalidDateFormat(DateTime dt)
		{}
	}
}

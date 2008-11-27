// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
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

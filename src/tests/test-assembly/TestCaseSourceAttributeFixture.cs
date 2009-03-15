// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************
using System;
using NUnit.Framework;

namespace NUnit.TestData
{
    [TestFixture]
    public class TestCaseSourceAttributeFixture
    {
        [TestCaseSource("source")]
        public void MethodThrowsExpectedException(int x, int y, int z)
        {
            throw new ArgumentNullException();
        }

        [TestCaseSource("source")]
        public void MethodThrowsWrongException(int x, int y, int z)
        {
            throw new ArgumentException();
        }

        [TestCaseSource("source")]
        public void MethodThrowsNoException(int x, int y, int z)
        {
        }

        [TestCaseSource("source")]
        public void MethodCallsIgnore(int x, int y, int z)
        {
            Assert.Ignore("Ignore this");
        }

        private static object[] source = new object[] {
            new TestCaseData( 2, 3, 4 ).Throws(typeof(ArgumentNullException)) };
    }
}

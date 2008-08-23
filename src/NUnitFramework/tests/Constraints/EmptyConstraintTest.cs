// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class EmptyConstraintTest : ConstraintTestBaseWithInvalidDataTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EmptyConstraint();
            expectedDescription = "<empty>";
        }

        static object[] GoodData = new object[] 
        {
            string.Empty,
            new object[0],
            new ArrayList(),
#if NET_2_0
            new System.Collections.Generic.List<int>()
#endif  
        };

        static object[] BadData = new object[]
        {
            "Hello",
            new object[] { 1, 2, 3 }
        };

        static object[] FailureMessages= new object[]
        {
            "\"Hello\"",
            "< 1, 2, 3 >"
        };

        static object[] InvalidData = new object[]
            {
                null,
                5
            };
    }
}
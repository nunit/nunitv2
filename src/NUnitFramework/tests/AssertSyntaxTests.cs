// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using NUnit.Framework.Constraints;
#if NET_2_0
using System.Collections.Generic;
#endif

/// <summary>
/// These test fixtures attempt to exercise all the syntactic
/// variations of Assert without getting into failures, errors 
/// or corner cases. Thus, some of the tests may be duplicated 
/// in other fixtures.
/// 
/// Each test performs the same operations using the classic
/// syntax (if available) and the new syntax in both the
/// helper-based and inherited forms.
/// 
/// These tests will eventually be duplicated in other
/// supported languages. 
/// </summary>
namespace NUnit.Framework.Syntax
{
    #region Invalid Code Tests
    public class InvalidCodeTests : AssertionHelper
    {
		// This method contains assertions that should not compile
		// You can check by uncommenting the code.
        public void WillNotCompile()
		{
		//    Assert.That(42, Is.Not);
		//    Assert.That(42, Is.All);
		//    Assert.That(42, Is.Null.Not);
		//    Assert.That(42, Is.Not.Null.GreaterThan(10));
		//    Assert.That(42, Is.GreaterThan(10).LessThan(99));

		//    object[] c = new object[0];
		//    Assert.That(c, Is.Null.All);
		//    Assert.That(c, Is.Not.All);
		//    Assert.That(c, Is.All.Not);
        }
	}
    #endregion
}

// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Represents a test suite constructed from a type that has a static Suite property
	/// </summary>
	public class LegacySuite : TestSuite
	{
		public LegacySuite( Type fixtureType ) : base( fixtureType )
		{
            this.fixtureSetUp = NUnitFramework.GetFixtureSetUpMethod(fixtureType);
            this.fixtureTearDown = NUnitFramework.GetFixtureTearDownMethod(fixtureType);
		}
	}
}

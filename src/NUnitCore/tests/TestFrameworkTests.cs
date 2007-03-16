// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework;
using System.Reflection;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for TestFrameworkTests.
	/// </summary>
	[TestFixture]
	public class TestFrameworkTests
	{
		[Test]
		public void NUnitFrameworkIsKnownAndReferenced()
		{
			foreach( AssemblyName assemblyName in CoreExtensions.Host.TestFrameworks.GetReferencedFrameworks( Assembly.GetExecutingAssembly() ) )
				if ( assemblyName.Name == "nunit.framework" ) return;
			Assert.Fail("Cannot find nunit.framework");
		}
	}
}

//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
namespace NUnit.Tests
{
	using System;
	using NUnit.Framework;
	using NUnit.Core;

	public class AllTests
	{
		[Suite]
		public static TestSuite Suite
		{
			get 
			{
				TestSuite suite = new TestSuite("All Tests");
				suite.Add(new OneTestCase());
				suite.Add(new Assemblies.AssemblyTests());
				suite.Add(new AssertionTest());
				return suite;
			}
		}
	}
}

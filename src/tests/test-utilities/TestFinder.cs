// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.TestUtilities
{
	/// <summary>
	/// Utility class used to locate tests by name in a test tree
	/// </summary>
	public class TestFinder
	{
		public static Test Find(string name, Test test)
		{
			Test result = null;
			if (test.TestName.Name == name)
				result = test;
			else if (test.Tests != null)
			{
				foreach(Test t in test.Tests) 
				{
					result = Find(name, t);
					if (result != null)
						break;
				}
			}

			return result;
		}
		
		public static Test FindChildTest(string name, Test test)
		{
			if ( test.Tests != null )
				foreach(Test t in test.Tests )
				{
					if (t.TestName.Name == name)
						return t;
				}

			return null;
		}

		public static Test RequiredChildTest(string name, Test test)
		{
			Test t = FindChildTest(name, test);
			if ( t == null )
				Assert.Fail("Test not found: " + name );
			return t;
		}

		public static TestResult Find(string name, TestResult result) 
		{
			if (result.Test.TestName.Name == name)
				return result;

			if ( result.HasResults )
			{
				foreach( TestResult r in result.Results ) 
				{
					TestResult myResult = Find( name, r );
					if ( myResult != null )
						return myResult;
				}
			}

			return null;
		}

		public static TestResult FindChildResult(string name, TestResult result)
		{
			if ( result.HasResults )
				foreach(TestResult r in result.Results )
					if (r.Name == name)
						return r;

			return null;
		}

		private TestFinder() { }
	}
}

// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Core;

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
		
		public static TestResult Find(string name, TestResult result) 
		{
			if (result.Test.TestName.Name == name)
				return result;

			TestSuiteResult suiteResult = result as TestSuiteResult;
			if ( suiteResult != null )
			{
				foreach( TestResult r in suiteResult.Results ) 
				{
					TestResult myResult = Find( name, r );
					if ( myResult != null )
						return myResult;
				}
			}

			return null;
		}

		private TestFinder() { }
	}
}

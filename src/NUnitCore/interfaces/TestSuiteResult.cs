// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core
{
	using System;
	using System.Collections;

	/// <summary>
	/// TestSuiteResult represents the result of running a 
	/// TestSuite. It adds a set of child results to the
	/// base TestResult class.
	/// </summary>
	/// 
	[Serializable]
	public class TestSuiteResult : TestResult
	{
		/// <summary>
		/// Construct a TestSuiteResult from a test
		/// </summary>
		/// <param name="test"></param>
		public TestSuiteResult(TestInfo test) 
			: base(test) { }
	}
}

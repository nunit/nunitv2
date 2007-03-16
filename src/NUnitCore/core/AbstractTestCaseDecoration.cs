// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestCaseDecorator is used to add functionality to
	/// another TestCase, which it aggregates.
	/// </summary>
	public abstract class AbstractTestCaseDecoration : TestCase
	{
		protected TestCase testCase;

		public AbstractTestCaseDecoration( TestCase testCase )
			: base( testCase.TestName.FullName, testCase.TestName.Name )
		{
			this.testCase = testCase;
			this.RunState = testCase.RunState;
			this.IgnoreReason = testCase.IgnoreReason;
		}

		public override int TestCount
		{
			get { return testCase.TestCount; }
		}
	}
}

//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Core
{
	using System;
	using System.Diagnostics;
	using System.Reflection;

	/// <summary>
	/// Summary description for ExpectedExceptionTestCase.
	/// </summary>
	public class ExpectedExceptionTestCase : TemplateTestCase
	{
		private Type expectedException;

		public ExpectedExceptionTestCase(object fixture, MethodInfo info, Type expectedException)
			: base(fixture, info)
		{
			this.expectedException = expectedException;
		}

		protected override internal void ProcessException(Exception exception, TestCaseResult testResult)
		{
			if (expectedException.Equals(exception.GetType()))
			{
				testResult.Success();
			}
			else
			{
				string message = "Expected: " + expectedException.Name + " but was " + exception.GetType().Name;
				testResult.Failure(message, exception.StackTrace);
			}

			return;
		}

		protected override internal void ProcessNoException(TestCaseResult testResult)
		{
			testResult.Failure(expectedException.Name + " was expected", null);
		}
	}
}

//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;
	using System.Reflection;

	/// <summary>
	/// Summary description for TestCase.
	/// </summary>
	public class NormalTestCase : TemplateTestCase
	{
		public NormalTestCase(object fixture, MethodInfo method) : base(fixture, method)
		{}

		protected internal override void ProcessNoException(TestCaseResult testResult)
		{
			testResult.Success();
		}
		
		protected internal override void ProcessException(Exception exception, TestCaseResult testResult)
		{
			if(exception.GetType().IsAssignableFrom(typeof(NUnit.Framework.AssertionException)))
			{
				NUnit.Framework.AssertionException error = (NUnit.Framework.AssertionException)exception;
				testResult.Failure(error.Message, error.StackTrace);
			}
			else
			{
				testResult.Failure(exception.Message, exception.StackTrace);
			}
		}
	}
}


using System;
using System.Reflection;
using System.Diagnostics;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for NotRunnableTestCase.
	/// </summary>
	public class NotRunnableTestCase : TestCase
	{
		public NotRunnableTestCase(MethodInfo method, string reason) : base(method.DeclaringType.FullName, method.Name)
		{
			ShouldRun = false;
			IgnoreReason = reason;
		}

		public override void Run(TestCaseResult result)
		{
			result.NotRun(base.IgnoreReason);
		}
	}
}

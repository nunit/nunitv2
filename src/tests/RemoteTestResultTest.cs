using System;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Tests
{
	[TestFixture]
	public class RemoteTestResultTest
	{
		[Test]
		public void ResultStillValidAfterDomainUnload() 
		{
			TestDomain domain = new TestDomain();
			Test test = domain.LoadAssembly("mock-assembly.dll");
			Assert.NotNull(test);
			TestResult result = domain.Run(new NullListener(), Console.Out, Console.Error);
			TestSuiteResult suite = result as TestSuiteResult;
			Assert.NotNull(suite);
			TestCaseResult caseResult = findCaseResult(suite);
			Assert.NotNull(caseResult);
			TestResultItem item = new TestResultItem(caseResult);
			domain.Unload();
			string message = item.GetMessage();
			Assert.NotNull(message);
		}

		private TestCaseResult findCaseResult(TestSuiteResult suite) 
		{
			foreach (TestResult r in suite.Results) 
			{
				if (r is TestCaseResult)
				{
					return (TestCaseResult) r;
				}
				else 
				{
					TestCaseResult result = findCaseResult((TestSuiteResult)r);
					if (result != null)
						return result;
				}

			}

			return null;
		}
	}
}

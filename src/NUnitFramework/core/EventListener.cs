namespace NUnit.Core
{
	using System;

	/// <summary>
	/// Summary description for EventListener.
	/// </summary>
	public interface EventListener
	{
		void TestStarted(TestCase testCase);
			
		void TestFinished(TestCaseResult result);

		void SuiteStarted(TestSuite suite);

		void SuiteFinished(TestSuiteResult result);
	}
}

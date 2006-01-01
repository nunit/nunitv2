namespace NUnit.Core
{
	using System;

	public class ProxyEventListener : EventListener
	{
		EventListener eventListener;

		public ProxyEventListener(EventListener eventListener)
		{
			this.eventListener = eventListener;
		}

		public virtual void RunStarted(TestInfo[] tests)
		{
			this.eventListener.RunStarted(tests);
		}

		public virtual void RunFinished(TestResult[] results)
		{
			this.eventListener.RunFinished(results);
		}

		public virtual void RunFinished(Exception exception)
		{
			this.eventListener.RunFinished(exception);
		}

		public virtual void TestStarted(TestInfo testCase)
		{
			this.eventListener.TestStarted(testCase);
		}

		public virtual void TestFinished(TestCaseResult result)
		{
			this.eventListener.TestFinished(result);
		}

		public virtual void SuiteStarted(TestInfo suite)
		{
			this.eventListener.SuiteStarted(suite);
		}

		public virtual void SuiteFinished(TestSuiteResult result)
		{
			this.eventListener.SuiteFinished(result);
		}

		public virtual void UnhandledException(Exception exception)
		{
			this.eventListener.UnhandledException(exception);
		}

		public virtual void TestOutput(TestOutput testOutput)
		{
			this.eventListener.TestOutput(testOutput);
		}
	}
}

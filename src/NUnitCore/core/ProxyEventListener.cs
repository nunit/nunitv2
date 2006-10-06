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

		public virtual void RunStarted(string name, int testCount)
		{
			this.eventListener.RunStarted(name, testCount);
		}

		public virtual void RunFinished(TestResult result)
		{
			this.eventListener.RunFinished(result);
		}

		public virtual void RunFinished(Exception exception)
		{
			this.eventListener.RunFinished(exception);
		}

		public virtual void TestStarted(TestName testName)
		{
			this.eventListener.TestStarted(testName);
		}

		public virtual void TestFinished(TestCaseResult result)
		{
			this.eventListener.TestFinished(result);
		}

		public virtual void SuiteStarted(TestName testName)
		{
			this.eventListener.SuiteStarted(testName);
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

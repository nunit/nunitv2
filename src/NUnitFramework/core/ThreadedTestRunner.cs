namespace NUnit.Core
{
	using System;
	using System.Threading;

	public class ThreadedTestRunner : ProxyTestRunner
	{
		TestRunner testRunner;

		public ThreadedTestRunner(TestRunner testRunner) : base(testRunner)
		{
			this.testRunner = testRunner;
		}

		public override TestResult Run(EventListener listener)
		{
			using(PumpingEventListener pumpingEventListener = new PumpingEventListener(listener))
			{
				TestRunnerThread thread = new TestRunnerThread(this.testRunner);
				thread.Run(pumpingEventListener);
				while(thread.Results == null)
				{
					pumpingEventListener.DoEvents();
					Thread.Sleep(1000 / 50);
				}
				return thread.Results[0];
			}
		}

		public override TestResult[] Run(EventListener listener, string[] testNames)
		{
			using(PumpingEventListener pumpingEventListener = new PumpingEventListener(listener))
			{
				TestRunnerThread thread = new TestRunnerThread(this.testRunner);
				thread.Run(pumpingEventListener, testNames);
				while(thread.Results == null)
				{
					pumpingEventListener.DoEvents();
					Thread.Sleep(1000 / 50);
				}
				return thread.Results;
			}
		}
	}
}

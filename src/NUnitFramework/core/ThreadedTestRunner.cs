namespace NUnit.Core
{
	using System.Threading;

	public class ThreadedTestRunner : ProxyTestRunner
	{
		TestRunner testRunner;
		TestRunnerThread thread;

		public ThreadedTestRunner(TestRunner testRunner) : base(testRunner)
		{
			this.testRunner = testRunner;
		}

		public override TestResult Run(EventListener listener)
		{
			this.thread = new TestRunnerThread(this.testRunner);
			try
			{
				using(PumpingEventListener pumpingEventListener = new PumpingEventListener(listener))
				{
					this.thread.Run(pumpingEventListener);
					while(this.thread.IsAlive)
					{
						pumpingEventListener.DoEvents();
						Thread.Sleep(1000 / 50);
					}

					if(this.thread.Results == null)
					{
						return null;
					}
					return this.thread.Results[0];
				}
			}
			finally
			{
				this.thread = null;
			}
		}

		public override TestResult[] Run(EventListener listener, string[] testNames)
		{
			this.thread = new TestRunnerThread(this.testRunner);
			try
			{
				using(PumpingEventListener pumpingEventListener = new PumpingEventListener(listener))
				{
					this.thread.Run(pumpingEventListener, testNames);
					while(this.thread.IsAlive)
					{
						pumpingEventListener.DoEvents();
						Thread.Sleep(1000 / 50);
					}
					return this.thread.Results;
				}
			}
			finally
			{
				this.thread = null;
			}
		}

		public override void CancelRun()
		{
			if(this.thread != null)
			{
				this.thread.Cancel();
			}
		}
	}
}

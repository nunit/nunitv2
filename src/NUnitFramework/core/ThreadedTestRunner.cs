namespace NUnit.Core
{
	using System;
	using System.Threading;
	using System.Collections.Specialized;

	/// <summary>
	/// ThreadedTestRunner creates a TestRunnerThread to run the
	/// tests and at the same time sets up a PumpingEventListener
	/// to ensure that events are sent back to the caller on the
	/// same thread that originally called Run. The status of
	/// the runner thread is checked periodically rather than
	/// using Wait. This ensures that a gui calling thread is
	/// available to process any events that are sent.
	/// </summary>
	public class ThreadedTestRunner : ProxyTestRunner, TestRunner
	{
		#region Instance Variables

		/// <summary>
		/// The TestRunnerThread that runs our tests
		/// </summary>
		TestRunnerThread thread;
		
		#endregion

		#region Constructor

		public ThreadedTestRunner(TestRunner testRunner) : base( testRunner ) { }
		
		#endregion

		#region Override Run Methods

		protected override TestResult[] doRun(EventListener listener, string[] testNames)
		{
			this.thread = new TestRunnerThread(this.testRunner);
			try
			{
				using(PumpingEventListener pumpingEventListener = new PumpingEventListener(listener))
				{
					this.thread.StartRun( pumpingEventListener, testNames );
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

		void TestRunner.CancelRun()
		{
			if(this.thread != null)
			{
				this.thread.Cancel();
			}
		}

		#endregion
	}
}

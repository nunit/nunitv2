using System;

namespace NUnit.Core
{

	public delegate TestResult[] RunDelegate(NUnit.Core.EventListener listener, string[] testNames);

	[Serializable]
	public class ThreadedRemoteRunner : RemoteTestRunner
	{
		/// <summary>
		/// TestRunner thread used for asynchronous running
		/// </summary>
		private TestRunnerThread runningThread;

		public override TestResult Run( EventListener listener )
		{
			TestResult[] results = Run(listener, null);
			if (results != null)
				return results[0];

			return null;
		}

		public override TestResult[] Run(NUnit.Core.EventListener listener, string[] testNames)
		{
			try 
			{
				runningThread = new TestRunnerThread( new RunDelegate(RunTestInThread), listener, testNames );
			}
			catch( Exception ex )
			{
				listener.RunFinished( ex );
				return null;
			}

			runningThread.StartRun( listener, testNames );

			//runningThread.Wait();

			return runningThread.Results;
		}

		private TestResult[] RunTestInThread(NUnit.Core.EventListener listener, string[] testNames)
		{
			return base.Run(listener, testNames);
		}

		public override void CancelRun()
		{
			if ( runningThread != null )
				runningThread.Cancel();
		}

		public override void Wait()
		{
			if ( runningThread != null )
				runningThread.Wait();
		}
	}
}

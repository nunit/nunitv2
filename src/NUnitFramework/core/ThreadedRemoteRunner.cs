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

		public override void StartRun( EventListener listener )
		{
			StartRun(listener, null);
		}

		public override void StartRun(NUnit.Core.EventListener listener, string[] testNames)
		{
			try 
			{
				runningThread = new TestRunnerThread( new RunDelegate(RunTestInThread), listener, testNames );
			}
			catch( Exception ex )
			{
				listener.RunFinished( ex );
			}

			runningThread.StartRun( listener, testNames );

			//runningThread.Wait();

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

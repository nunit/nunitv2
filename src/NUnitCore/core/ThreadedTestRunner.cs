namespace NUnit.Core
{
	using System;
	using System.Threading;
	using System.Collections.Specialized;

	/// <summary>
	/// ThreadedTestRunner overrides the Run and BeginRun methods 
	/// so that they are always run on a separate thread. The actual
	/// </summary>
	public class ThreadedTestRunner : ProxyTestRunner
	{
		#region Instance Variables
		private TestRunnerThread testRunnerThread;
		#endregion

		#region Constructors
		public ThreadedTestRunner( TestRunner testRunner ) : base ( testRunner ) { }
		#endregion

		#region Overrides

		public override TestResult Run( EventListener listener )
		{
			BeginRun( listener );
			return EndRun();
		}

//		public override TestResult[] Run( EventListener listener, string[] testNames )
//		{
//			BeginRun( listener, testNames );
//			return EndRun();
//		}

		public override void BeginRun( EventListener listener )
		{
			testRunnerThread = new TestRunnerThread( this.TestRunner );

			testRunnerThread.StartRun( listener, null );
		}

//		public override void BeginRun( EventListener listener, string[] testNames )
//		{
//			testRunnerThread = new TestRunnerThread( this.TestRunner );
//
//			if ( testNames == null || testNames.Length == 0 )
//				testRunnerThread.StartRun( listener, null );
//			else
//				testRunnerThread.StartRun( listener, testNames );
//		}

		public override TestResult EndRun()
		{
			this.Wait();
			return this.TestRunner.TestResult;
		}


		public override void Wait()
		{
			if ( testRunnerThread != null )
				testRunnerThread.Wait();
		}

		public override void CancelRun()
		{
			if ( testRunnerThread != null )
				testRunnerThread.Cancel();
		}

		#endregion
	}
}

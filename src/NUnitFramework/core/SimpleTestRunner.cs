using System;
using System.IO;
using System.Threading;

namespace NUnit.Core
{
	/// <summary>
	/// SimpleTestRunner is the simplest direct-running TestRunner. It
	/// passes the event listener interface that is provided on to the tests
	/// to use directly and does nothing to redirect text output.
	/// </summary>
	public class SimpleTestRunner : AbstractTestRunner
	{
		#region Instance Variables
#if STARTRUN_SUPPORT
		/// <summary>
		/// TestRunnerThread used for asynchronous runs
		/// </summary>
		private TestRunnerThread startRunThread;
#endif

		/// <summary>
		/// The thread on which Run was called
		/// </summary>
		private Thread runThread;
			
		#endregion

		#region Properties
		public override bool Running
		{
			get 
			{ 
				return 
#if STARTRUN_SUPPORT
				startRunThread != null && startRunThread.IsAlive ||
#endif
				runThread != null && runThread.IsAlive; 
			}
		}
		#endregion

		#region Method Overrides
		/// <summary>
		/// Override method to run a set of tests. This routine is the workhorse
		/// that is called anytime tests are run.
		/// </summary>
		protected override TestResult[] doRun( EventListener listener, Test[] tests )
		{
			//			AddinManager.Addins.Save();
			//			AddinManager.Addins.Clear();

			try
			{
				// Take note of the fact that we are running
				this.runThread = Thread.CurrentThread;

				// Create an array for the results
				testResults = new TestResult[ tests.Length ];

				// Signal that we are starting the run
				listener.RunStarted( tests );
				
				// Run each test, saving the results
				int index = 0;
				foreach( Test test in tests )
				{
					using( new DirectorySwapper( 
						Path.GetDirectoryName( this.assemblies[test.AssemblyKey] ) ) )
					{
						testResults[index++] = test.Run( listener, base.Filter );
					}
				}

				// Signal that we are done
				listener.RunFinished( testResults );

				// Return result array
				return testResults;
			}
			catch( Exception exception )
			{
				// Signal that we finished with an exception
				listener.RunFinished( exception );
				// Rethrow - should we do this?
				throw;
			}
			finally
			{
				// Flag that we are no longer running
				this.runThread = null;

//				AddinManager.Addins.Restore();
			}
		}

#if STARTRUN_SUPPORT
		protected override void doStartRun( EventListener listener, string[] testNames )
		{
			startRunThread = new TestRunnerThread( this );

			if ( testNames == null || testNames.Length == 0 )
				startRunThread.StartRun( listener, null );
			else
				startRunThread.StartRun( listener, testNames );
		}
#endif

		public override void CancelRun()
		{
#if STARTRUN_SUPPORT
			// Cancel Asynchrous StartRun
			if ( startRunThread != null )
			{
				if ( startRunThread.IsAlive )
					startRunThread.Cancel();
			}
			else 
#endif
			if ( runThread != null )
			{
				// Cancel Synchronous run only if on another thread
				if ( runThread == Thread.CurrentThread )
					throw new InvalidOperationException( "May not CancelRun on same thread that is running the test" );
					
				runThread.Abort();
				
				// Wake up the thread if necessary
				if ( ( runThread.ThreadState & ThreadState.WaitSleepJoin ) != 0 )
					runThread.Interrupt();				
			}
		}
		#endregion
	}
}

using System;
using System.IO;
using System.Threading;

namespace NUnit.Core
{
	/// <summary>
	/// BaseTestRunner is the simplest direct-running TestRunner. It simply
	/// passes the event listener interface that is provided on to the tests
	/// to use directly and does nothing to redirect text output.
	/// </summary>
	public class BaseTestRunner : AbstractTestRunner
	{
		/// <summary>
		/// TestRunnerThread used for asynchronous runs
		/// </summary>
		private TestRunnerThread startRunThread;

		/// <summary>
		/// The thread on which Run was called
		/// </summary>
		private Thread runThread;

		public override bool Running
		{
			get { return runThread != null && runThread.IsAlive || 
				  startRunThread != null && startRunThread.IsAlive; }
		}

		/// <summary>
		/// Override method to run a set of tests. This routine is the workhorse
		/// that is called anytime tests are run.
		/// </summary>
		protected override TestResult[] doRun( EventListener listener, Test[] tests )
		{
			// Save previous state of Console. This is needed because Console.Out and
			// Console.Error are static. In the case where the test itself calls this
			// method, we can lose output if we don't save and restore their values.
			// This is exactly what happens when we are testing NUnit itself.
			TextWriter saveOut = Console.Out;
			TextWriter saveError = Console.Error;

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
				
				// TODO: Get rid of count
				int count = 0;
				foreach( Test test in tests )
					count += test.CountTestCases( testFilter );
		
				// Run each test, saving the results
				int index = 0;
				foreach( Test test in tests )
				{
					using( new DirectorySwapper( 
						Path.GetDirectoryName( this.assemblies[test.AssemblyKey] ) ) )
					{
						//						EventListener flushingListener = new FlushingEventListener(listener, this.displayTestLabels, outWriter, errorWriter);
						//						results[index++] = test.Run( flushingListener, filter );
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
				this.runThread = null;
				Console.SetOut( saveOut );
				Console.SetError( saveError ); 
				//				AddinManager.Addins.Restore();
			}
		}

		protected override void doStartRun( EventListener listener, string[] testNames )
		{
			startRunThread = new TestRunnerThread( this );

			if ( testNames == null || testNames.Length == 0 )
				startRunThread.StartRun( listener, null );
			else
				startRunThread.StartRun( listener, testNames );
		}

		public override void CancelRun()
		{
			// Cancel Asynchrous StartRun
			if ( startRunThread != null )
			{
				if ( startRunThread.IsAlive )
					startRunThread.Cancel();
			}
			else if ( runThread != null )
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
	}
}

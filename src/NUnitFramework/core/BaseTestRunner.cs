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
		#region Instance Variables
		/// <summary>
		/// TestRunnerThread used for asynchronous runs
		/// </summary>
		private TestRunnerThread startRunThread;

		/// <summary>
		/// The thread on which Run was called
		/// </summary>
		private Thread runThread;
		#endregion

		#region Properties
		public override bool Running
		{
			get { return runThread != null && runThread.IsAlive || 
				  startRunThread != null && startRunThread.IsAlive; }
		}
		#endregion

		#region Method Overrides
		/// <summary>
		/// Override method to run a set of tests. This routine is the workhorse
		/// that is called anytime tests are run.
		/// </summary>
		protected override TestResult[] doRun( EventListener listener, Test[] tests )
		{
			// Save static context so we can change and restore Console
			// Out and Error and in case the tests change anything - as
			// happens, for example, in testing NUnit itself.
			TestContext.Save();

			// Set Console to go to our Out and Error properties if they were set.
			// Note that any changes made by the user in the test code or the code 
			// it calls will defeat this.
			if ( this.Out != null )
				TestContext.Out = this.Out;
			if ( Error != null )
				TestContext.Error = this.Error; 

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

				// Restore Console and other static settings
				TestContext.Restore();

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
		#endregion
	}
}

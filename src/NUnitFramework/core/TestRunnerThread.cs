using System;
using System.IO;
using System.Threading;
using System.Configuration;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestRunnerThread.
	/// </summary>
	public class TestRunnerThread
	{
		private TestRunner runner;

		private Thread thread;

		private Exception lastException;

		private NUnit.Core.EventListener listener;
			
		private string[] testNames;
			
		private TestResult[] results;
			
		public TestRunnerThread( TestRunner runner ) 
		{ 
			this.runner = runner;
			this.thread = new Thread( new ThreadStart( TestRunnerThreadProc ) );
		}

		public void Wait()
		{
			if ( this.thread.IsAlive )
				this.thread.Join();
		}

		public void Cancel()
		{
			this.thread.Abort();
			this.thread.Join();
		}

		public void Run( EventListener listener )

		{
			this.listener = listener;

			string apartment = ConfigurationSettings.AppSettings["apartment"];
			if ( apartment == "STA" )
				thread.ApartmentState = ApartmentState.STA;
			thread.Start();}

		public void Run( EventListener listener, string testName )
		{
			this.listener = listener;
			this.testNames = new string[] { testName };

			string apartment = ConfigurationSettings.AppSettings["apartment"];
			if ( apartment == "STA" )
				thread.ApartmentState = ApartmentState.STA;
			thread.Start();		}

		public void Run( EventListener listener, string[] testNames )
		{
			this.listener = listener;
			this.testNames = testNames;

			string apartment = ConfigurationSettings.AppSettings["apartment"];
			if ( apartment == "STA" )
				thread.ApartmentState = ApartmentState.STA;
			thread.Start();
		}

		/// <summary>
		/// The thread proc for our actual test run
		/// </summary>
		private void TestRunnerThreadProc()
		{
			try
			{
				//TODO: do we need a run started event?
				int count = runner.CountTestCases( testNames );

				Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );
				results = runner.Run(listener, testNames );
				
				//TODO: do we need a run finished event?
			}
			catch( Exception exception )
			{
				lastException = exception;
				//TODO: do we need a run finished event?
			}
			finally
			{
				testNames = null;	// Do we need this?
				//runningThread = null;	// Ditto
			}
		}
	}
}

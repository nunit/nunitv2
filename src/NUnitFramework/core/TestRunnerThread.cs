using System;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestRunnerThread.
	/// </summary>
	public class TestRunnerThread
	{
		#region Private Fields

		/// <summary>
		/// The Test runner to be used in running tests on the thread
		/// </summary>
		private TestRunner runner;

		/// <summary>
		/// The System.Threading.Thread created by the object
		/// </summary>
		private Thread thread;

		/// <summary>
		/// Collection of TestRunner settings from the config file
		/// </summary>
		private NameValueCollection settings;

		/// <summary>
		/// The exception that terminated a test run
		/// </summary>
		private Exception lastException;

		/// <summary>
		/// The EventListener interface to receive test events
		/// </summary>
		private NUnit.Core.EventListener listener;
			
		/// <summary>
		/// Array of test names for ues by the thread proc
		/// </summary>
		private string[] testNames;
			
		/// <summary>
		/// Array of returned results
		/// </summary>
		private TestResult[] results;

		#endregion

		#region Properties

		/// <summary>
		/// Array of returned results
		/// </summary>
		public TestResult[] Results
		{
			get { return results; }
		}

		#endregion

		#region Constructor
	
		public TestRunnerThread( TestRunner runner ) 
		{ 
			this.runner = runner;
			this.thread = new Thread( new ThreadStart( TestRunnerThreadProc ) );
			thread.IsBackground = true;

			this.settings = (NameValueCollection)
				ConfigurationSettings.GetConfig( "NUnit/TestRunner" );
	
			if ( settings != null )
			{
				try
				{
					string apartment = (string)settings["ApartmentState"];
					if ( apartment != null )
						thread.ApartmentState = (ApartmentState)
							System.Enum.Parse( typeof( ApartmentState ), apartment, true );
		
					string priority = (string)settings["ThreadPriority"];
					if ( priority != null )
						thread.Priority = (ThreadPriority)
							System.Enum.Parse( typeof( ThreadPriority ), priority, true );
				}
				catch( ArgumentException ex )
				{
					string msg = string.Format( "Invalid configuration setting in {0}", 
						AppDomain.CurrentDomain.SetupInformation.ConfigurationFile );
					throw new ArgumentException( msg, ex );
				}
			}
		}

		#endregion

		#region Public Methods

		public void Wait()
		{
			if ( this.thread.IsAlive )
				this.thread.Join();
		}

		public void Cancel()
		{
			this.thread.Abort();
//			this.thread.Join();
		}

		public void Run( EventListener listener )
		{
			this.listener = listener;

			thread.Start();}

		public void Run( EventListener listener, string testName )
		{
			this.listener = listener;
			this.testNames = new string[] { testName };

			thread.Start();		}

		public void Run( EventListener listener, string[] testNames )
		{
			this.listener = listener;
			this.testNames = testNames;

			thread.Start();
		}

		#endregion

		#region Thread Proc

		/// <summary>
		/// The thread proc for our actual test run
		/// </summary>
		private void TestRunnerThreadProc()
		{
			try
			{
				//TODO: do we need a run started event?

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

		#endregion
	}
}

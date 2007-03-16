// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
namespace NUnit.Core
{
	using System.Collections;
	using System;

	/// <summary>
	/// RemoteTestRunner is tailored for use as the initial runner to
	/// receive control in a remote domain. It provides isolation for the return
	/// value by using a ThreadedTestRunner and for the events through use of
	/// an EventPump.
	/// </summary>
	public class RemoteTestRunner : DelegatingTestRunner
	{
		private bool useThreadedRunner = true;

		#region Constructor
		public RemoteTestRunner() : base( 0 ) { }

		public RemoteTestRunner( int runnerID ) : base( runnerID ) { }
		#endregion

		#region Method Overrides
		public override bool Load(TestPackage package)
		{
			// Initialize ExtensionHost if not already done
			if ( !CoreExtensions.Host.Initialized )
				CoreExtensions.Host.InitializeService();

			// Delayed creation of downstream runner allows us to
			// use a different runner type based on the package
			bool useThreadedRunner = package.GetSetting( "UseThreadedRunner", true );
			
			TestRunner runner = new SimpleTestRunner( this.runnerID );
			if ( useThreadedRunner )
				runner = new ThreadedTestRunner( runner );

			this.TestRunner = runner;

			return base.Load (package);
		}

		public override TestResult Run( EventListener listener )
		{
			return Run( listener, TestFilter.Empty );
		}

		public override TestResult Run( EventListener listener, ITestFilter filter )
		{
			if ( useThreadedRunner )
			{
				QueuingEventListener queue = new QueuingEventListener();

				DirectOutputToListener( queue );

				using( EventPump pump = new EventPump( listener, queue.Events, true ) )
				{
					pump.Start();
					return base.Run( queue, filter );
				}
			}
			else
			{
				DirectOutputToListener( listener );
				return base.Run( listener, filter );
			}
		}

		public override void BeginRun( EventListener listener )
		{
			BeginRun( listener, TestFilter.Empty );
		}

		public override void BeginRun( EventListener listener, ITestFilter filter )
		{
			if ( useThreadedRunner )
			{
				QueuingEventListener queue = new QueuingEventListener();

				DirectOutputToListener( queue );

				EventPump pump = new EventPump( listener, queue.Events, true);
				pump.Start(); // Will run till RunFinished is received
				// TODO: Make sure the thread is cleaned up if we abort the run
			
				base.BeginRun( queue, filter );
			}
			else
			{
				DirectOutputToListener( listener );
				base.BeginRun( listener, filter );
			}
		}

		private void DirectOutputToListener( EventListener queue )
		{
			TestContext.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
			TestContext.Error = new EventListenerTextWriter( queue, TestOutputType.Error );
			TestContext.TraceWriter = new EventListenerTextWriter( queue, TestOutputType.Trace );
			TestContext.Tracing = true;
		}
		#endregion
	}
}

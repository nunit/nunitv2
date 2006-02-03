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
	public class RemoteTestRunner : ProxyTestRunner
	{
		#region Constructor
		public RemoteTestRunner() : this( 0 ) { }

		public RemoteTestRunner( int runnerID ) 
			: base( new ThreadedTestRunner( new SimpleTestRunner( runnerID ) ) ) { }
		#endregion

		#region Method Overrides
		public override TestResult Run( EventListener listener )
		{
			QueuingEventListener queue = new QueuingEventListener();

			TestContext.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
			TestContext.Error = new EventListenerTextWriter( queue, TestOutputType.Error );

			using( EventPump pump = new EventPump( listener, queue.Events, true ) )
			{
				pump.Start();
				return base.Run( queue );
			}
		}

//		public override TestResult[] Run( EventListener listener, string[] testNames )
//		{
//			QueuingEventListener queue = new QueuingEventListener();
//
//			TestContext.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
//			TestContext.Error = new EventListenerTextWriter( queue, TestOutputType.Error );
//
//			using( EventPump pump = new EventPump( listener, queue.Events, true ) )
//			{
//				pump.Start();
//				return base.Run( queue, testNames );
//			}
//		}

		public override void BeginRun( EventListener listener )
		{
			QueuingEventListener queue = new QueuingEventListener();

			TestContext.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
			TestContext.Error = new EventListenerTextWriter( queue, TestOutputType.Error );

			EventPump pump = new EventPump( listener, queue.Events, true);
			pump.Start(); // Will run till RunFinished is received
			// TODO: Make sure the thread is cleaned up if we abort the run
			
			base.BeginRun( queue );
		}

//		public override void BeginRun( EventListener listener, string[] testNames )
//		{
//			QueuingEventListener queue = new QueuingEventListener();
//
//			TestContext.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
//			TestContext.Error = new EventListenerTextWriter( queue, TestOutputType.Error );
//
//			EventPump pump = new EventPump( listener, queue.Events, true);
//			pump.Start(); // Will run till RunFinished is received
//			// TODO: Make sure the thread is cleaned up if we abort the run
//			
//			base.BeginRun( queue, testNames );
//		}

		#endregion
	}
}

namespace NUnit.Core
{
	using System.Collections;

	public class RemoteTestRunner : ProxyTestRunner
	{
		#region Constructor
		public RemoteTestRunner() 
			: base( new BaseTestRunner() ) { }
		#endregion

		#region Method Overrides
		public override TestResult[] doRun( EventListener listener, string[] testNames )
		{
			QueuingEventListener queue = new QueuingEventListener();

			this.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
			this.Error = new EventListenerTextWriter( queue, TestOutputType.Error );

			using( EventPump pump = new EventPump( listener, queue.Events, true ) )
			{
				pump.Start();
				return base.doRun( queue, testNames );
			}
		}

		public override void doStartRun( EventListener listener, string[] testNames )
		{
			QueuingEventListener queue = new QueuingEventListener();

			this.Out = new EventListenerTextWriter( queue, TestOutputType.Out );
			this.Error = new EventListenerTextWriter( queue, TestOutputType.Error );

			EventPump pump = new EventPump( listener, queue.Events, true);
			pump.Start(); // Will run till RunFinished is received
			// TODO: Make sure the thread is cleaned up if we abort the run
			
			base.doStartRun( queue, testNames );
		}
		#endregion
	}
}

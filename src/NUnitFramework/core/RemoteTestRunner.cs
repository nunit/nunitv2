namespace NUnit.Core
{
	using System.Collections;

	public class RemoteTestRunner : ProxyTestRunner
	{
		public RemoteTestRunner() 
			: base( new ThreadedTestRunner(new RealTestRunner() ) ) {	}

		public override void StartRun(EventListener listener)
		{
			base.StartRun (listener);
		}

		public override void StartRun(EventListener listener, string[] testNames)
		{
			base.StartRun (listener, testNames);
		}


	}
}

namespace NUnit.Core
{
	using System.Collections;

	public class RemoteTestRunner : ProxyTestRunner
	{
		public RemoteTestRunner() 
			: base( new ThreadedTestRunner(new RealTestRunner() ) ) {	}

	}
}

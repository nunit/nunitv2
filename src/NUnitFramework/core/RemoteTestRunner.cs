namespace NUnit.Core
{
	using System;
	using System.IO;

	public class RemoteTestRunner : ProxyTestRunner
	{
		public RemoteTestRunner() :
			base(new ThreadedTestRunner(new RealTestRunner()))
		{
		}
	}
}

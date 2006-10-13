using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for IAddinHost.
	/// </summary>
	public interface IAddinHost
	{
		void Install( ISuiteBuilder builder );

		void Install( ITestCaseBuilder builder );

		void Install( ITestDecorator decorator );
	}
}

using System;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for ThreadedTestRunnerTests.
	/// </summary>
	[TestFixture]
	public class ThreadedTestRunnerTests : BasicRunnerTests
	{
		protected override TestRunner CreateRunner( int runnerID )
		{
			return new ThreadedTestRunner( new SimpleTestRunner( runnerID ) );
		}

	}
}

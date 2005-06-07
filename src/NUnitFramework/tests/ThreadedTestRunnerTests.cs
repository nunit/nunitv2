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
		public override TestRunner CreateRunner()
		{
			return new ThreadedTestRunner( new SimpleTestRunner() );
		}

	}
}

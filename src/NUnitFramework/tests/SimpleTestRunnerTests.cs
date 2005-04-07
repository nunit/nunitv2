using System;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Tests of BaseTestRunner
	/// </summary>
	[TestFixture]
	public class SimpleTestRunnerTests : BasicRunnerTests
	{
		public override TestRunner CreateRunner()
		{
			return new SimpleTestRunner();
		}
	}
}

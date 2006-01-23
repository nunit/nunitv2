using System;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for TestRunnerEx.
	/// </summary>
	public interface TestRunnerEx :TestRunner
	{
		bool Load( NUnitProject project );

		bool Load( NUnitProject project, string testName );
	}
}

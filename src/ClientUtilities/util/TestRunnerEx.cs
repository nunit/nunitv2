using System;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// TestRunnerEx is a temporary interface, used on the 
	/// client side until we can move the loading of NUnit 
	/// project files to the core.
	/// </summary>
	public interface TestRunnerEx :TestRunner
	{
		bool Load( NUnitProject project );

		bool Load( NUnitProject project, string testName );
	}
}

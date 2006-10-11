using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for RunState.
	/// </summary>
	public enum RunState
	{
		NotRunnable,
		Runnable,
		Explicit,
		Skipped,
		Ignored,
		Executed
	}
}

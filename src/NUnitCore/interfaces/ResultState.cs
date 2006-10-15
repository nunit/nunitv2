using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for ResultState.
	/// </summary>
	public enum ResultState
	{
		Success,
		Failure,
		Error,
	}

    public enum FailureSite
    {
        Test,
        SetUp,
        TearDown,
        Parent,
        Child
    }

}

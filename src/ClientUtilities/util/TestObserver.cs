using System;

namespace NUnit.Util
{
	/// <summary>
	/// The TestObserver interface is implemented by a class that
	/// subscribes to the events generated in loading and running
	/// tests and uses that information in some way.
	/// </summary>
	public interface TestObserver
	{
		void Subscribe( ITestEvents events );
	}
}

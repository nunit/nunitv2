using System;

namespace NUnit.Util
{
	/// <summary>
	/// ITestEvents interface consists of a set of events that are fired
	/// as test suites are loaded and unloaded and as tests are run.
	/// 
	/// In order to isolate client code from the loading and unloading 
	/// of test domains, events that formerly took a Test, TestCase or 
	/// TestSuite as an argument, now use a UITestNode, which gives
	/// the same information but isn't connected to the remote domain.
	/// 
	/// The UITestNode object may in some cases be created using lazy 
	/// evaluation of child UITestNode objects. Since evaluation of these
	/// objects does cause a cross-domain reference, the client code
	/// should access the full tree immediately, rather than at a later
	/// time, if that is what is needed. This will normally happen if
	/// the client building a tree, for example. However, some clients
	/// may only want the name of the test being run and passing the
	/// fully evaluated tree would be unnecessary for them.
	/// </summary>
	public interface ITestEvents
	{
		// Events related to loading tests. Note that
		// if loading failed for a different assembly
		// from that which was previously loaded, the
		// old one is still valid.
		event TestLoadEventHandler LoadStartingEvent;
		event TestLoadEventHandler LoadCompleteEvent;
		event TestLoadEventHandler LoadFailedEvent;
		
		// Events related to reloading tests
		event TestLoadEventHandler ReloadStartingEvent;
		event TestLoadEventHandler ReloadCompleteEvent;
		event TestLoadEventHandler ReloadFailedEvent;
		
		// Events related to unloading tests.
		event TestLoadEventHandler UnloadStartingEvent;
		event TestLoadEventHandler UnloadCompleteEvent;
		event TestLoadEventHandler UnloadFailedEvent;

		// Events related to a running a set of tests
		event TestEventHandler RunStartingEvent;	
		event TestEventHandler RunFinishedEvent;

		// Events that arise while a test is running
		// These serve the same purpose as the methods
		// defined by the EventListener interface.
		event TestEventHandler SuiteStartingEvent;
		event TestEventHandler SuiteFinishedEvent;
		event TestEventHandler TestStartingEvent;
		event TestEventHandler TestFinishedEvent;
	}
}

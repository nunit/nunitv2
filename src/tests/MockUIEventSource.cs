namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;

	/// <summary>
	/// Summary description for MockUiEventSource.
	/// </summary>
	public class MockUIEventSource : UIEvents
	{
		/// <summary>
		/// The events we are simulating
		/// </summary>
		public event TestSuiteLoadedHandler TestSuiteLoadedEvent;
		public event TestSuiteChangedHandler TestSuiteChangedEvent;
		public event TestSuiteUnloadedHandler TestSuiteUnloadedEvent;
		public event TestSuiteLoadFailureHandler TestSuiteLoadFailureEvent;
		public event RunStartingHandler RunStartingEvent;
		public event SuiteStartedHandler SuiteStartedEvent;
		public event TestStartedHandler TestStartedEvent;
		public event RunFinishedHandler RunFinishedEvent;
		public event SuiteFinishedHandler SuiteFinishedEvent;
		public event TestFinishedHandler TestFinishedEvent;

		/// <summary>
		/// Methods for firing each event
		/// </summary>
		public void FireTestSuiteLoadedEvent( UITestNode test, string assemblyFileName )
		{
			if ( TestSuiteLoadedEvent != null )
				TestSuiteLoadedEvent( test, assemblyFileName );
		}
		
		public void FireTestSuiteChangedEvent( UITestNode test )
		{
			if ( TestSuiteChangedEvent != null )
				TestSuiteChangedEvent( test  );
		}
		
		public void FireTestSuiteUnloadedEvent()
		{
			if ( TestSuiteUnloadedEvent != null )
				TestSuiteUnloadedEvent();
		}
		
		public void FireTestSuiteLoadFailureEvent( string assemblyFileName, Exception exception )
		{
			if ( TestSuiteLoadFailureEvent != null )
				TestSuiteLoadFailureEvent( assemblyFileName, exception );
		}
	
		public void FireRunStartingEvent( UITestNode test )
		{
			if ( RunStartingEvent != null )
				RunStartingEvent( test );
		}

		public void FireSuiteStartedEvent( UITestNode suite )
		{
			if ( SuiteStartedEvent != null )
				SuiteStartedEvent( suite );
		}

		public void FireTestStartedEvent( UITestNode testCase )
		{
			if ( TestStartedEvent != null )
				TestStartedEvent( testCase );
		}

		public void FireRunFinishedEvent( TestResult result )
		{
			if ( RunFinishedEvent != null )
				RunFinishedEvent( result );
		}

		public void FireSuiteFinishedEvent( TestSuiteResult result )
		{
			if ( SuiteFinishedEvent != null )
				SuiteFinishedEvent( result );
		}

		public void FireTestFinishedEvent( TestCaseResult result )
		{
			if ( TestFinishedEvent != null )
				TestFinishedEvent( result );
		}

		public void SimulateTestRun( Test test )
		{
			FireRunStartingEvent( test );

			TestResult result = SimulateTest( test );

			FireRunFinishedEvent( result );
		}

		private TestResult SimulateTest( Test test )
		{
			if ( test is TestSuite )
			{
				FireSuiteStartedEvent( test );

				TestSuiteResult result = new TestSuiteResult( test, test.Name );

				foreach( Test childTest in test.Tests )
					result.AddResult( SimulateTest( childTest ) );

				FireSuiteFinishedEvent( result );

				return result;
			}
			else
			{
				FireTestStartedEvent( test );
				
				TestCaseResult result = new TestCaseResult( test as TestCase );
				result.Executed = test.ShouldRun;
				
				FireTestFinishedEvent( result );

				return result;
			}
		}
	}
}

namespace NUnit.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.UiKit;

	/// <summary>
	/// Summary description for MockUiEventSource.
	/// </summary>
	public class MockUIEventSource : ITestEvents
	{
		/// <summary>
		/// The events we are simulating
		/// </summary>
		public event TestLoadEventHandler LoadStartingEvent;
		public event TestLoadEventHandler LoadCompleteEvent;
		public event TestLoadEventHandler LoadFailedEvent;

		public event TestLoadEventHandler ReloadStartingEvent;
		public event TestLoadEventHandler ReloadCompleteEvent;
		public event TestLoadEventHandler ReloadFailedEvent;

		public event TestLoadEventHandler UnloadStartingEvent;
		public event TestLoadEventHandler UnloadCompleteEvent;
		public event TestLoadEventHandler UnloadFailedEvent;

		public event TestEventHandler RunStartingEvent;
		public event TestEventHandler SuiteStartingEvent;
		public event TestEventHandler TestStartingEvent;
		public event TestEventHandler RunFinishedEvent;
		public event TestEventHandler SuiteFinishedEvent;
		public event TestEventHandler TestFinishedEvent;

		private string assemblyName;
		private Test test;

		public MockUIEventSource( string assemblyName, Test test )
		{
			this.test = test;
			this.assemblyName = assemblyName;
		}

		/// <summary>
		/// Methods for firing each event
		/// </summary>
		 
		private TestLoadEventArgs LoadEventArgs( TestLoadAction action )
		{
			return new TestLoadEventArgs( action, assemblyName, test );
		}

		public void LoadComplete()
		{
			if ( LoadCompleteEvent != null )
				LoadCompleteEvent( this, LoadEventArgs( TestLoadAction.LoadComplete ) );
		}
		
		public void ReloadComplete()
		{
			if ( ReloadCompleteEvent != null )
				ReloadCompleteEvent( this, LoadEventArgs( TestLoadAction.ReloadComplete ) );
		}
		
		public void UnloadComplete()
		{
			if ( UnloadCompleteEvent != null )
				UnloadCompleteEvent( this, LoadEventArgs( TestLoadAction.UnloadComplete ) );
		}
		
		public void LoadFailed( Exception exception )
		{
			if ( LoadFailedEvent != null )
				LoadFailedEvent( this, new TestLoadEventArgs( TestLoadAction.LoadFailed, assemblyName, exception ) );
		}
	
		public void RunStarting( UITestNode test )
		{
			if ( RunStartingEvent != null )
				RunStartingEvent( this, new TestEventArgs( TestAction.RunStarting, test ) );
		}

		public void SuiteStarting( UITestNode suite )
		{
			
			if ( SuiteStartingEvent != null )
				SuiteStartingEvent( this, new TestEventArgs( TestAction.SuiteStarting, suite ) );
		}

		public void TestStarting( UITestNode testCase )
		{
			if ( TestStartingEvent != null )
				TestStartingEvent( this, new TestEventArgs( TestAction.TestStarting, testCase ) );
		}

		public void RunFinished( TestResult result, Exception e )
		{
			if ( RunFinishedEvent != null )
				RunFinishedEvent( this, new TestEventArgs( TestAction.RunFinished, result ) );
		}

		public void SuiteFinished( TestSuiteResult result )
		{
			if ( SuiteFinishedEvent != null )
				SuiteFinishedEvent( this, new TestEventArgs( TestAction.SuiteFinished, result ) );
		}

		public void TestFinished( TestCaseResult result )
		{
			if ( TestFinishedEvent != null )
				TestFinishedEvent( this, new TestEventArgs( TestAction.TestFinished, result ) );
		}

		public void SimulateTestRun()
		{
			RunStarting( test );

			TestResult result = SimulateTest( test );

			RunFinished( result, null );
		}

		private TestResult SimulateTest( Test test )
		{
			if ( test.IsSuite )
			{
				SuiteStarting( test );

				TestSuiteResult result = new TestSuiteResult( test, test.Name );

				foreach( Test childTest in test.Tests )
					result.AddResult( SimulateTest( childTest ) );

				SuiteFinished( result );

				return result;
			}
			else
			{
				TestStarting( test );
				
				TestCaseResult result = new TestCaseResult( test as TestCase );
				result.Executed = test.ShouldRun;
				
				TestFinished( result );

				return result;
			}
		}
	}
}

using System;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Helper class used to dispatch test events
	/// </summary>
	public class TestEventDispatcher : ITestEvents
	{
		#region Events

		// Project loading events
		public event TestEventHandler ProjectLoading;
		public event TestEventHandler ProjectLoaded;
		public event TestEventHandler ProjectUnloading;
		public event TestEventHandler ProjectUnloaded;

		// Test loading events
		public event TestEventHandler TestLoading;	
		public event TestEventHandler TestLoaded;	
		public event TestEventHandler TestLoadFailed;

		public event TestEventHandler TestReloading;
		public event TestEventHandler TestReloaded;
		public event TestEventHandler TestReloadFailed;

		public event TestEventHandler TestUnloading;
		public event TestEventHandler TestUnloaded;
		public event TestEventHandler TestUnloadFailed;

		// Test running events
		public event TestEventHandler RunStarting;	
		public event TestEventHandler RunFinished;
		
		public event TestEventHandler SuiteStarting;
		public event TestEventHandler SuiteFinished;

		public event TestEventHandler TestStarting;
		public event TestEventHandler TestFinished;

		#endregion

		#region Methods for Firing Events
		
		private void Fire( 
			TestEventHandler handler, TestEventArgs e )
		{
			if ( handler != null )
				handler( this, e );
		}

		public void FireProjectLoading( string fileName )
		{
			Fire(
				ProjectLoading,
				new TestEventArgs( TestAction.ProjectLoading, fileName ) );
		}

		public void FireProjectLoaded( string fileName )
		{
			Fire( 
				ProjectLoaded,
				new TestEventArgs( TestAction.ProjectLoaded, fileName ) );
		}

		public void FireProjectUnloading( string fileName )
		{
			Fire( 
				ProjectUnloading,
				new TestEventArgs( TestAction.ProjectUnloading, fileName ) );
		}

		public void FireProjectUnloaded( string fileName )
		{
			Fire( 
				ProjectUnloaded,
				new TestEventArgs( TestAction.ProjectUnloaded, fileName ) );
		}

		public void FireTestLoading( string fileName )
		{
			Fire( 
				TestLoading,
				new TestEventArgs( TestAction.TestLoading, fileName ) );
		}

		public void FireTestLoaded( string fileName, UITestNode test )
		{
			Fire( 
				TestLoaded,
				new TestEventArgs( TestAction.TestLoaded, fileName, test ) );
		}

		public void FireTestLoadFailed( string fileName, Exception exception )
		{
			Fire(
				TestLoadFailed,
				new TestEventArgs( TestAction.TestLoadFailed, fileName, exception ) );
		}

		public void FireTestUnloading( string fileName, UITestNode test )
		{
			Fire(
				TestUnloading,
				new TestEventArgs( TestAction.TestUnloading, fileName, test ) );
		}

		public void FireTestUnloaded( string fileName, UITestNode test )
		{
			Fire(
				TestUnloaded,
				new TestEventArgs( TestAction.TestUnloaded, fileName, test ) );
		}

		public void FireTestUnloadFailed( string fileName, Exception exception )
		{
			Fire(
				TestUnloadFailed, 
				new TestEventArgs( TestAction.TestUnloadFailed, fileName, exception ) );
		}

		public void FireTestReloading( string fileName, UITestNode test )
		{
			Fire(
				TestReloading,
				new TestEventArgs( TestAction.TestReloading, fileName, test ) );
		}

		public void FireTestReloaded( string fileName, UITestNode test )
		{
			Fire(
				TestReloaded,
				new TestEventArgs( TestAction.TestReloaded, fileName, test ) );
		}

		public void FireTestReloadFailed( string fileName, Exception exception )
		{
			Fire(
				TestReloadFailed, 
				new TestEventArgs( TestAction.TestReloadFailed, fileName, exception ) );
		}

		public void FireRunStarting( UITestNode test )
		{
			Fire(
				RunStarting,
				new TestEventArgs( TestAction.RunStarting, test ) );
		}

		public void FireRunFinished( TestResult result )
		{	
			Fire(
				RunFinished,
				new TestEventArgs( TestAction.RunFinished, result ) );
		}

		public void FireRunFinished( Exception exception )
		{
			Fire(
				RunFinished,
				new TestEventArgs( TestAction.RunFinished, exception ) );
		}

		public void FireTestStarting( UITestNode test )
		{
			Fire(
				TestStarting,
				new TestEventArgs( TestAction.TestStarting, test ) );
		}

		public void FireTestFinished( TestResult result )
		{	
			Fire(
				TestFinished,
				new TestEventArgs( TestAction.TestFinished, result ) );
		}

		public void FireSuiteStarting( UITestNode test )
		{
			Fire(
				SuiteStarting,
				new TestEventArgs( TestAction.SuiteStarting, test ) );
		}

		public void FireSuiteFinished( TestResult result )
		{	
			Fire(
				SuiteFinished,
				new TestEventArgs( TestAction.SuiteFinished, result ) );
		}

		#endregion
	}
}

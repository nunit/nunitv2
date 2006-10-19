using System;
using System.Collections;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// EventListenerCollection holds multiple event listeners
	/// and relays all event calls to each of them.
	/// </summary>
	public class EventListenerCollection : EventListener
	{
		private ArrayList listeners = new ArrayList();

		public void Add( EventListener listener )
		{
			listeners.Add( listener );
		}

		public void Remove( EventListener listener )
		{
			listeners.Remove( listener );
		}

		#region EventListener Members
		public void RunStarted(string name, int testCount)
		{
			foreach( EventListener listener in listeners )
				listener.RunStarted( name, testCount );
		}

		public void RunFinished(TestResult result)
		{
			foreach( EventListener listener in listeners )
				listener.RunFinished( result );
		}

		public void RunFinished(Exception exception)
		{
			foreach( EventListener listener in listeners )
				listener.RunFinished( exception );
		}

		public void SuiteStarted(TestName testName)
		{
			foreach( EventListener listener in listeners )
				listener.SuiteStarted( testName );
		}

		public void SuiteFinished(TestSuiteResult result)
		{
			foreach( EventListener listener in listeners )
				listener.SuiteFinished( result );
		}

		public void TestStarted(TestName testName)
		{
			foreach( EventListener listener in listeners )
				listener.TestStarted( testName );
		}

		public void TestFinished(TestCaseResult result)
		{
			foreach( EventListener listener in listeners )
				listener.TestFinished( result );
		}

		public void UnhandledException(Exception exception)
		{
			foreach( EventListener listener in listeners )
				listener.UnhandledException( exception );
		}

		public void TestOutput(TestOutput testOutput)
		{
			foreach( EventListener listener in listeners )
				listener.TestOutput( testOutput );
		}

		#endregion
	}
}

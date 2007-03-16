// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// EventListenerCollection holds multiple event listeners
	/// and relays all event calls to each of them.
	/// </summary>
	public class EventListenerCollection : EventListener, IExtensionPoint
	{
		private ArrayList listeners = new ArrayList();

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

		#region IExtensionPoint Members

		public string Name
		{
			get { return "EventListeners"; }
		}

		public IExtensionHost Host
		{
			get { return CoreExtensions.Host; }
		}

		public void Install(object extension)
		{
			EventListener listener = extension as EventListener;
			if ( listener == null )
				throw new ArgumentException( 
					extension.GetType().FullName + " is not an EventListener", "exception" );

			listeners.Add( listener );
		}

		void NUnit.Core.Extensibility.IExtensionPoint.Remove(object extension)
		{
			listeners.Remove( extension );
		}

		#endregion
	}
}

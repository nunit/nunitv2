using System;
using System.Collections;
using System.Threading;

namespace NUnit.Core
{
	#region Individual Event Classes

	/// <summary>
	/// NUnit.Core.Event is the abstract base for all stored events.
	/// An Event is the stored representation of a call to the 
	/// EventListener interface and is used to record such calls
	/// or to queue them for forwarding on another thread or at
	/// a later time.
	/// </summary>
	public abstract class Event
	{
		abstract public void Send( EventListener listener );
	}

	public class RunStartedEvent : Event
	{
		Test[] tests;

		public RunStartedEvent( Test[] tests )
		{
			this.tests = tests;
		}

		public override void Send( EventListener listener )
		{
			listener.RunStarted(this.tests);
		}
	}

	public class RunFinishedEvent : Event
	{
		TestResult[] results;
		Exception exception;

		public RunFinishedEvent( TestResult[] results )
		{
			this.results = results;
		}

		public RunFinishedEvent( Exception exception )
		{
			this.exception = exception;
		}

		public override void Send( EventListener listener )
		{
			if ( this.exception != null )
				listener.RunFinished( this.exception );
			else
				listener.RunFinished( this.results );
		}
	}

	public class TestStartedEvent : Event
	{
		TestCase testCase;

		public TestStartedEvent( TestCase testCase )
		{
			this.testCase = testCase;
		}

		public override void Send( EventListener listener )
		{
			listener.TestStarted( this.testCase );
		}
	}
			
	public class TestFinishedEvent : Event
	{
		TestCaseResult result;

		public TestFinishedEvent( TestCaseResult result )
		{
			this.result = result;
		}

		public override void Send( EventListener listener )
		{
			listener.TestFinished( this.result );
		}
	}

	public class SuiteStartedEvent : Event
	{
		TestSuite suite;

		public SuiteStartedEvent( TestSuite suite )
		{
			this.suite = suite;
		}

		public override void Send( EventListener listener )
		{
			listener.SuiteStarted( this.suite );
		}
	}

	public class SuiteFinishedEvent : Event
	{
		TestSuiteResult result;

		public SuiteFinishedEvent( TestSuiteResult result )
		{
			this.result = result;
		}

		public override void Send( EventListener listener )
		{
			listener.SuiteFinished( this.result );
		}
	}

	public class UnhandledExceptionEvent : Event
	{
		Exception exception;

		public UnhandledExceptionEvent( Exception exception )
		{
			this.exception = exception;
		}

		public override void Send( EventListener listener )
		{
			listener.UnhandledException( this.exception );
		}
	}

	public class OutputEvent : Event
	{
		TestOutput output;

		public OutputEvent( TestOutput output )
		{
			this.output = output;
		}

		public override void Send( EventListener listener )
		{
			listener.TestOutput( this.output );
		}
	}

	#endregion

	/// <summary>
	/// Implements a queue of work items each of which
	/// is queued as a WaitCallback.
	/// </summary>
	public class EventQueue
	{
		private Queue queue = new Queue();

		public int Count
		{
			get 
			{
				lock( this )
				{
					return this.queue.Count; 
				}
			}
		}

		public void Enqueue( Event e )
		{
			lock( this )
			{
				this.queue.Enqueue( e );
				Monitor.Pulse( this );
			}
		}

		public Event Dequeue()
		{
			lock( this )
			{
				return (Event)this.queue.Dequeue();
			}
		}
	}
}

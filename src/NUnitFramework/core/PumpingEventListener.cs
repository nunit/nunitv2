#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Threading;
	using System.Reflection;
	using System.Collections;

	/// <summary>
	/// Implements a queue of work items each of which
	/// is queued as a WaitCallback.
	/// </summary>
	class WorkItemQueue
	{
		Queue queue;

		public WorkItemQueue()
		{
			this.queue = new Queue();
		}

		public int Count
		{
			get { return this.queue.Count; }
		}

		public void Enqueue( WaitCallback callback )
		{
			this.queue.Enqueue( callback );
		}

		public WaitCallback Dequeue()
		{
			return (WaitCallback)this.queue.Dequeue();
		}
	}

	/// <summary>
	/// This EventListener implementation is used to isolate
	/// the test runner thread in the test app domain/context.
	/// </summary>
	class PumpingEventListener : EventListener, IDisposable
	{
		EventListener eventListener;
		WorkItemQueue workItems;

		public PumpingEventListener(EventListener eventListener)
		{
			this.eventListener = eventListener;
			this.workItems = new WorkItemQueue();
		}

		public void Dispose()
		{
			DoEvents();
		}

		public void DoEvents()
		{
			while(this.workItems.Count > 0)
			{
				WaitCallback callback = this.workItems.Dequeue();
				callback.DynamicInvoke(new object[] {null});
			}
		}

		/// <summary>
		/// Run is starting
		/// </summary>
		/// <param name="tests">Array of tests to be run</param>
		public void RunStarted( Test[] tests )
		{
			RunStartedWorkItem workItem = new RunStartedWorkItem(this.eventListener, tests);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class RunStartedWorkItem
		{
			EventListener eventListener;
			Test[] tests;

			public RunStartedWorkItem(EventListener eventListener, Test[] tests)
			{
				this.eventListener = eventListener;
				this.tests = tests;
			}

			public void Callback(object state)
			{
				this.eventListener.RunStarted(this.tests);
			}
		}

		/// <summary>
		/// Run finished successfully
		/// </summary>
		/// <param name="results">Array of test results</param>
		public void RunFinished( TestResult[] results )
		{
			RunFinishedWorkItem workItem = new RunFinishedWorkItem(this.eventListener, results);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class RunFinishedWorkItem
		{
			EventListener eventListener;
			TestResult[] results;

			public RunFinishedWorkItem(EventListener eventListener, TestResult[] results)
			{
				this.eventListener = eventListener;
				this.results = results;
			}

			public void Callback(object state)
			{
				this.eventListener.RunFinished(this.results);
			}
		}

		/// <summary>
		/// Run was terminated due to an exception
		/// </summary>
		/// <param name="exception">Exception that was thrown</param>
		public void RunFinished( Exception exception )
		{
			RunFinishedExceptionWorkItem workItem = new RunFinishedExceptionWorkItem(this.eventListener, exception);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class RunFinishedExceptionWorkItem
		{
			EventListener eventListener;
			Exception exception;

			public RunFinishedExceptionWorkItem(EventListener eventListener, Exception exception)
			{
				this.eventListener = eventListener;
				this.exception = exception;
			}

			public void Callback(object state)
			{
				this.eventListener.RunFinished(this.exception);
			}
		}

		/// <summary>
		/// A single test case is starting
		/// </summary>
		/// <param name="testCase">The test case</param>
		public void TestStarted(TestCase testCase)
		{
			TestStartedWorkItem workItem = new TestStartedWorkItem(this.eventListener, testCase);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class TestStartedWorkItem
		{
			EventListener eventListener;
			TestCase testCase;

			public TestStartedWorkItem(EventListener eventListener, TestCase testCase)
			{
				this.eventListener = eventListener;
				this.testCase = testCase;
			}

			public void Callback(object state)
			{
				this.eventListener.TestStarted(this.testCase);
			}
		}
			
		/// <summary>
		/// A test case finished
		/// </summary>
		/// <param name="result">Result of the test case</param>
		public void TestFinished(TestCaseResult result)
		{
			TestFinishedWorkItem workItem = new TestFinishedWorkItem(this.eventListener, result);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class TestFinishedWorkItem
		{
			EventListener eventListener;
			TestCaseResult result;

			public TestFinishedWorkItem(EventListener eventListener, TestCaseResult result)
			{
				this.eventListener = eventListener;
				this.result = result;
			}

			public void Callback(object state)
			{
				this.eventListener.TestFinished(this.result);
			}
		}

		/// <summary>
		/// A suite is starting
		/// </summary>
		/// <param name="suite">The suite that is starting</param>
		public void SuiteStarted(TestSuite suite)
		{
			SuiteStartedWorkItem workItem = new SuiteStartedWorkItem(this.eventListener, suite);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class SuiteStartedWorkItem
		{
			EventListener eventListener;
			TestSuite suite;

			public SuiteStartedWorkItem(EventListener eventListener, TestSuite suite)
			{
				this.eventListener = eventListener;
				this.suite = suite;
			}

			public void Callback(object state)
			{
				this.eventListener.SuiteStarted(this.suite);
			}
		}

		/// <summary>
		/// A suite finished
		/// </summary>
		/// <param name="result">Result of the suite</param>
		public void SuiteFinished(TestSuiteResult result)
		{
			SuiteFinishedWorkItem workItem = new SuiteFinishedWorkItem(this.eventListener, result);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class SuiteFinishedWorkItem
		{
			EventListener eventListener;
			TestSuiteResult result;

			public SuiteFinishedWorkItem(EventListener eventListener, TestSuiteResult result)
			{
				this.eventListener = eventListener;
				this.result = result;
			}

			public void Callback(object state)
			{
				this.eventListener.SuiteFinished(this.result);
			}
		}

		/// <summary>
		/// An unhandled exception occured while running a test,
		/// but the test was not terminated.
		/// </summary>
		/// <param name="exception"></param>
		public void UnhandledException( Exception exception )
		{
			UnhandledExceptionWorkItem workItem = new UnhandledExceptionWorkItem(this.eventListener, exception);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class UnhandledExceptionWorkItem
		{
			EventListener eventListener;
			Exception exception;

			public UnhandledExceptionWorkItem(EventListener eventListener, Exception exception)
			{
				this.eventListener = eventListener;
				this.exception = exception;
			}

			public void Callback(object state)
			{
				this.eventListener.UnhandledException(this.exception);
			}
		}

		/// <summary>
		/// A message has been output to the console.
		/// </summary>
		/// <param name="testOutput">A console message</param>
		public void TestOutput( TestOutput output )
		{
			OutputWorkItem workItem = new OutputWorkItem(this.eventListener, output);
			workItems.Enqueue(new WaitCallback(workItem.Callback));
		}

		class OutputWorkItem
		{
			EventListener eventListener;
			TestOutput output;

			public OutputWorkItem(EventListener eventListener, TestOutput output)
			{
				this.eventListener = eventListener;
				this.output = output;
			}

			public void Callback(object state)
			{
				this.eventListener.TestOutput(this.output);
			}
		}
	}
}

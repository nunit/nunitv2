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
	using System.Text;

	/// <summary>
	/// The TestResult abstract class represents
	/// the result of a test and is used to
	/// communicate results across AppDomains.
	/// </summary>
	/// 
	[Serializable]
	public abstract class TestResult
	{
		#region Fields
		/// <summary>
		/// Indicates whether the test was executed or not
		/// </summary>
		private RunState runState;

		private ResultState resultState;

        private FailureSite failureSite;

		/// <summary>
		/// The elapsed time for executing this test
		/// </summary>
		private double time = 0.0;

		/// <summary>
		/// The name of the test
		/// </summary>
		private string name;

		/// <summary>
		/// The test that this result pertains to
		/// </summary>
		private TestInfo test;

		/// <summary>
		/// The stacktrace at the point of failure
		/// </summary>
		private string stackTrace;

		/// <summary>
		/// Description of this test
		/// </summary>
		private string description;

		/// <summary>
		/// Message giving the reason for failure
		/// </summary>
		protected string messageString;

		/// <summary>
		/// Number of asserts executed by this test
		/// </summary>
		private int assertCount = 0;

		#endregion

		#region Protected Constructor
		protected TestResult(TestInfo test, string name)
		{
			this.name = name;
			this.test = test;
            this.RunState = RunState.Runnable;
            if (test != null)
            {
                this.description = test.Description;
                this.runState = test.RunState;
                this.messageString = test.IgnoreReason;
            }
        }
		#endregion

		#region Properties

		public RunState RunState
		{
			get { return runState; }
			set { runState = value; }
		}

		public ResultState ResultState
		{
			get { return resultState; }
		}

        public FailureSite FailureSite
        {
            get { return failureSite; }
        }

		public bool Executed 
		{
			get { return runState == RunState.Executed; }
		}

		public virtual string Name
		{
			get{ return name;}
		}

		public TestInfo Test
		{
			get{ return test;}
		}

		public virtual bool IsSuccess
		{
			// TODO: Redefine this more precisely
			get { return !IsFailure; }
			//get { return resultState == ResultState.Success; }
		}
		
		// TODO: Distinguish errors from failures
		public virtual bool IsFailure
		{
			get { return resultState == ResultState.Failure || resultState == ResultState.Error; }
		}

		public virtual string Description
		{
			get { return description; }
			set { description = value; }
		}

		public double Time 
		{
			get{ return time; }
			set{ time = value; }
		}

		public string Message
		{
			get { return messageString; }
		}

		public virtual string StackTrace
		{
			get 
			{ 
				return stackTrace;
			}
			set 
			{
				stackTrace = value;
			}
		}

		public int AssertCount
		{
			get { return assertCount; }
			set { assertCount = value; }
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Mark the test as succeeding
		/// </summary>
		public void Success() 
		{ 
			this.runState = RunState.Executed;
			this.resultState = ResultState.Success; 
		}

		/// <summary>
		/// Mark the test as ignored.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		public void Ignore(string reason)
		{
			Ignore( reason, null );
		}

		/// <summary>
		/// Mark the test as ignored.
		/// </summary>
		/// <param name="ex">The ignore exception that was thrown</param>
		public void Ignore( Exception ex )
		{
			Ignore( ex.Message, BuildStackTrace( ex ) );
		}

		/// <summary>
		/// Mark the test as ignored.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		/// <param name="stackTrace">Stack trace giving the location of the command</param>
		public void Ignore(string reason, string stackTrace)
		{
			this.runState = RunState.Ignored;
			this.messageString = reason;
			this.stackTrace = stackTrace;
		}

		/// <summary>
		/// Mark the test as skipped.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		public void Skip(string reason)
		{
			Skip( reason, null );
		}

		/// <summary>
		/// Mark the test as ignored.
		/// </summary>
		/// <param name="ex">The ignore exception that was thrown</param>
		public void Skip( Exception ex )
		{
			Skip( ex.Message, BuildStackTrace( ex ) );
		}

		/// <summary>
		/// Mark the test as skipped.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		/// <param name="stackTrace">Stack trace giving the location of the command</param>
		public void Skip(string reason, string stackTrace)
		{
			this.runState = RunState.Skipped;
			this.messageString = reason;
			this.stackTrace = stackTrace;
		}

        public void Failure(string message, string stackTrace)
        {
            Failure(message, stackTrace, FailureSite.Test);
        }

		/// <summary>
		/// Mark the test as a failure due to an
		/// assertion having failed.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="stackTrace">Stack trace giving the location of the failure</param>
		public void Failure(string message, string stackTrace, FailureSite failureSite )
		{
			this.runState = RunState.Executed;
			this.resultState = ResultState.Failure;
            this.failureSite = failureSite;
			this.messageString = message;
			this.stackTrace = stackTrace;
		}

        public void Error(Exception exception)
        {
            Error(exception, FailureSite.Test);
        }

		public void Error( Exception exception, FailureSite failureSite )
		{
			this.runState = RunState.Executed;
			this.resultState = ResultState.Error;
            this.failureSite = failureSite;

            string message = BuildMessage(exception);
            string stackTrace = BuildStackTrace(exception);

            if (failureSite == FailureSite.TearDown)
            {
                message = "TearDown : " + message;
                stackTrace = "--TearDown" + Environment.NewLine + stackTrace;

                if (this.messageString != null)
                    message = this.messageString + Environment.NewLine + message;
                if (this.stackTrace != null)
                    stackTrace = this.stackTrace + Environment.NewLine + stackTrace;
            }

            this.messageString = message;
            this.stackTrace = stackTrace;
		}
		#endregion

		#region Exception Helpers

		private string BuildMessage(Exception exception)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat( "{0} : {1}", exception.GetType().ToString(), exception.Message );

			Exception inner = exception.InnerException;
			while( inner != null )
			{
				sb.Append( Environment.NewLine );
				sb.AppendFormat( "  ----> {0} : {1}", inner.GetType().ToString(), inner.Message );
				inner = inner.InnerException;
			}

			return sb.ToString();
		}
		
		private string BuildStackTrace(Exception exception)
		{
            StringBuilder sb = new StringBuilder( GetStackTrace( exception ) );

            Exception inner = exception.InnerException;
            while( inner != null )
            {
                sb.Append( Environment.NewLine );
                sb.Append( "--" );
                sb.Append( inner.GetType().Name );
                sb.Append( Environment.NewLine );
                sb.Append( GetStackTrace( inner ) );

                inner = inner.InnerException;
            }

            return sb.ToString();
		}

		private string GetStackTrace(Exception exception)
		{
			try
			{
				return exception.StackTrace;
			}
			catch( Exception )
			{
				return "No stack trace available";
			}
		}

		#endregion

		public abstract void Accept(ResultVisitor visitor);
	}
}

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
		/// True if the test executed
		/// </summary>
		private bool executed = false;

		/// <summary>
		/// True if the test was marked as a failure
		/// </summary>
		private bool isFailure = false; 

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
			if(test != null)
				this.description = test.Description;
		}
		#endregion

		#region Properties

		public bool Executed 
		{
			get { return executed; }
			set { executed = value; }
		}

		public virtual bool AllTestsExecuted
		{
			get { return executed; }
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
			get { return !(isFailure); }
		}
		
		public virtual bool IsFailure
		{
			get { return isFailure; }
			set { isFailure = value; }
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
		/// Mark the test as not run.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		public void NotRun(string reason)
		{
			NotRun( reason, null );
		}

		/// <summary>
		/// Mark the test as not run.
		/// </summary>
		/// <param name="reason">The reason the test was not run</param>
		/// <param name="stackTrace">Stack trace giving the location of the command</param>
		public void NotRun(string reason, string stackTrace)
		{
			this.executed = false;
			this.messageString = reason;
			this.stackTrace = stackTrace;
		}

		public void NotRun( Exception ex )
		{
			NotRun( ex.Message, BuildStackTrace( ex ) );
		}

		/// <summary>
		/// Mark the test as a failure due to an
		/// assertion having failed.
		/// </summary>
		/// <param name="message">Message to display</param>
		/// <param name="stackTrace">Stack trace giving the location of the failure</param>
		public void Failure(string message, string stackTrace )
		{
			this.executed = true;
			this.isFailure = true;
			this.messageString = message;
			this.stackTrace = stackTrace;
		}

		public void Error( Exception exception )
		{
			this.executed = true;
			this.isFailure = true;
			this.messageString = BuildMessage( exception );
			this.stackTrace = BuildStackTrace( exception );
		}

		public void TearDownError( Exception exception )
		{
			this.executed = true;
			this.IsFailure = true;
			this.messageString += Environment.NewLine + "TearDown : " + BuildMessage( exception );
			this.stackTrace += Environment.NewLine + "--TearDown" + Environment.NewLine + BuildStackTrace( exception );
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
			if(exception.InnerException!=null)
				return GetStackTrace(exception) + Environment.NewLine + 
					"--" + exception.GetType().Name + Environment.NewLine +
					BuildStackTrace(exception.InnerException);
			else
				return GetStackTrace(exception);
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

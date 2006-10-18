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
	using System.Text.RegularExpressions;
	using System.Reflection;

	/// <summary>
	/// The TestMethod class represents a TestCase implemented as a method
	/// call on a fixture object. At the moment, this is the only way we 
	/// implement a TestCase, but others are expected in the future.
	/// 
	/// Because of how exceptions are handled internally, this class
	/// must incorporate processing of expected exceptions. A change to
	/// the TestCase interface might make it easier to process exceptions
	/// in an object that aggregates a TestMethod in the future.
	/// </summary>
	public abstract class TestMethod : TestCase
	{
		#region Fields
		/// <summary>
		/// The test method
		/// </summary>
		private MethodInfo method;

		/// <summary>
		/// The SetUp method.
		/// </summary>
		protected MethodInfo setUpMethod;

		/// <summary>
		/// The teardown method
		/// </summary>
		protected MethodInfo tearDownMethod;

		/// <summary>
		/// The type of any expected exception
		/// </summary>
		internal Type expectedException;
        
		/// <summary>
		/// The full name of any expected exception type
		/// </summary>
		internal string expectedExceptionName;
        
		/// <summary>
		/// The value of any message associated with an expected exception
		/// </summary>
		internal string expectedMessage;
        
		/// <summary>
		/// A string indicating how to match the expected message
		/// </summary>
		internal string matchType;
		#endregion

		#region Constructors
		public TestMethod( MethodInfo method ) 
			: base( method ) 
		{
			this.method = method;
		}
		#endregion

		#region Properties
		public MethodInfo Method
		{
			get { return method; }
		}

		public Type ExpectedException
		{
			get { return expectedException; }
			set 
			{ 
				expectedException = value;
				expectedExceptionName = expectedException.FullName;
			}
		}

		public string ExpectedExceptionName
		{
			get { return expectedExceptionName; }
			set
			{
				expectedException = null;
				expectedExceptionName = value;
			}
		}

		public string ExpectedMessage
		{
			get { return expectedMessage; }
			set { expectedMessage = value; }
		}

		public string MatchType
		{
			get { return matchType; }
			set { matchType = value; }
		}
		#endregion

		public override void Run(TestCaseResult testResult)
		{ 
            try
            {
                if ( this.Parent != null)
                    Fixture = this.Parent.Fixture;

                if (!testResult.IsFailure)
                {
                    // Temporary... to allow for tests that directly execute a test case
                    if (Fixture == null)
                        Fixture = Reflect.Construct(this.FixtureType);

                    doRun(testResult);
                }
            }
            catch (Exception ex)
            {
                if (ex is NunitException)
                    ex = ex.InnerException;

                RecordException(ex, testResult);
            }
		}

		/// <summary>
		/// The doRun method is used to run a test internally.
		/// It assumes that the caller is taking care of any 
		/// TestFixtureSetUp and TestFixtureTearDown needed.
		/// </summary>
		/// <param name="testResult">The result in which to record success or failure</param>
		public virtual void doRun( TestCaseResult testResult )
		{
			DateTime start = DateTime.Now;

			try 
			{
				if ( setUpMethod != null )
					Reflect.InvokeMethod( setUpMethod, this.Fixture );

				doTestCase( testResult );
			}
			catch(Exception ex)
			{
				if ( ex is NunitException )
					ex = ex.InnerException;

				RecordException( ex, testResult );
			}
			finally 
			{
				doTearDown( testResult );

				DateTime stop = DateTime.Now;
				TimeSpan span = stop.Subtract(start);
				testResult.Time = (double)span.Ticks / (double)TimeSpan.TicksPerSecond;
			}
		}

		#region Invoke Methods by Reflection, Recording Errors

		private void doTearDown( TestCaseResult testResult )
		{
			try
			{
				if ( tearDownMethod != null )
			 		tearDownMethod.Invoke( this.Fixture, new object[0] );
			}
			catch(Exception ex)
			{
				if ( ex is NunitException )
					ex = ex.InnerException;
				// TODO: What about ignore exceptions in teardown?
				testResult.Error( ex,FailureSite.TearDown );
			}
		}

		private void doTestCase( TestCaseResult testResult )
		{
			try
			{
				RunTestMethod(testResult);
				ProcessNoException(testResult);
			}
			catch( Exception ex )
			{
				if ( ex is NunitException )
					ex = ex.InnerException;

				if ( IsIgnoreException( ex ) )
					testResult.Ignore( ex );
				else
					ProcessException(ex, testResult);
			}
		}

		public virtual void RunTestMethod(TestCaseResult testResult)
		{
			Reflect.InvokeMethod( this.method, this.Fixture );
		}

		#endregion

		#region Record Info About An Exception

		protected void RecordException( Exception ex, TestResult testResult )
		{
			if ( IsIgnoreException( ex ) )
				testResult.Ignore( ex.Message );
			else if ( IsAssertException( ex ) )
				testResult.Failure( ex.Message, ex.StackTrace );
			else	
				testResult.Error( ex );
		}

		protected string GetStackTrace(Exception exception)
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

		#region Virtual Methods
		protected internal virtual void ProcessNoException(TestCaseResult testResult)
		{
            if ( ExceptionExpected )
                testResult.Failure(NoExceptionMessage(), null);
            else
			    testResult.Success();
		}
		
		protected internal virtual void ProcessException(Exception exception, TestCaseResult testResult)
		{
            if (ExceptionExpected)
            {
                if (IsExpectedExceptionType(exception))
                {
                    if (IsExpectedMessageMatch(exception))
                    {
                        testResult.Success();
                    }
                    else
                    {
                        testResult.Failure(WrongTextMessage(exception), GetStackTrace(exception));
                    }
                }
                else if (IsAssertException(exception))
                {
                    testResult.Failure(exception.Message, exception.StackTrace);
                }
                else
                {
                    testResult.Failure(WrongTypeMessage(exception), GetStackTrace(exception));
                }
            }
            else
                RecordException(exception, testResult);
		}
		#endregion

		#region Abstract Methods
		protected abstract bool IsAssertException(Exception ex);

		protected abstract bool IsIgnoreException(Exception ex);
		#endregion

        #region Helper Methods
        protected bool ExceptionExpected
        {
            get { return expectedExceptionName != null; }
        }

        protected bool IsExpectedExceptionType(Exception exception)
        {
            return expectedExceptionName.Equals(exception.GetType().FullName);
        }

        protected bool IsExpectedMessageMatch(Exception exception)
        {
            if (expectedMessage == null)
                return true;

            switch (matchType)
            {
                case "Exact":
                default:
                    return expectedMessage.Equals(exception.Message);
                case "Contains":
                    return exception.Message.IndexOf(expectedMessage) >= 0;
                case "Regex":
                    return Regex.IsMatch(exception.Message, expectedMessage);
            }
        }

        protected string NoExceptionMessage()
        {
            return expectedExceptionName + " was expected";
        }

        protected string WrongTypeMessage(Exception exception)
        {
            return "Expected: " + expectedExceptionName + " but was " + exception.GetType().FullName;
        }

        protected string WrongTextMessage(Exception exception)
        {
            switch (matchType)
            {
                default:
                case "Exact":
                    return string.Format("Expected exception message: \"{0}\" but received message \"{1}\"",
                        expectedMessage, exception.Message);
                case "Contains":
                    return string.Format("Expected exception message containing: \"{0}\" but received message \"{1}\"",
                        expectedMessage, exception.Message);
                case "Regex":
                    return string.Format("Expected exception message matching: \"{0}\" but received message \"{1}\"",
                        expectedMessage, exception.Message);
            }
        }
        #endregion
    }
}

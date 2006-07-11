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
	/// The TestMethod class represents
	/// a TestCase implemented as a method call on
	/// a fixture object. At the moment, this is the
	/// only way we implement a TestCase, but others
	/// are expected in the future.
	/// </summary>
	public class TestMethod : TestCase
	{
		#region Fields
		/// <summary>
		/// The Type of the fixture implementing this TestMethod
		/// </summary>
		private Type fixtureType;

		/// <summary>
		/// The test method
		/// </summary>
		private MethodInfo method;

		/// <summary>
		/// The SetUp method.
		/// </summary>
		private MethodInfo setUpMethod;

		/// <summary>
		/// The teardown method
		/// </summary>
		private MethodInfo tearDownMethod;

		/// <summary>
		/// The fixture object constructed for this test
		/// </summary>
		private object fixture;

        internal Type expectedException;
        internal string expectedExceptionName;
        internal string expectedMessage;
        internal string matchType;

        #endregion

		#region Constructors
		public TestMethod( MethodInfo method ) : base( method.ReflectedType.FullName, 
			method.DeclaringType == method.ReflectedType 
			? method.Name : method.DeclaringType.Name + "." + method.Name )
		{
			this.method = method;
			this.fixtureType = method.ReflectedType;
		}

		public TestMethod( MethodInfo method,
			Type expectedException, string expectedMessage, string matchType ) 
			: this( method )
		{
			this.expectedException = expectedException;
			if ( expectedException != null )
				this.expectedExceptionName = expectedException.FullName;
			this.expectedMessage = expectedMessage;
			this.matchType = matchType;
		}
	
		public TestMethod( MethodInfo method,
			string expectedExceptionName, string expectedMessage, string matchType ) 
			: this( method )
		{
			this.expectedExceptionName = expectedExceptionName;
			this.expectedMessage = expectedMessage;
			this.matchType = matchType;
		}
		#endregion

		#region Properties
		public Type FixtureType
		{
			get { return fixtureType; }
		}

		public object Fixture
		{
			get { return fixture; }
			set { fixture = value; }
		}
		#endregion

		public override void Run(TestCaseResult testResult)
		{ 
			TestSuite parentSuite = this.Parent;

            try
            {
                if (parentSuite != null)
                {
                    Fixture = parentSuite.Fixture;

                    if (setUpMethod == null)
                        setUpMethod = parentSuite.SetUpMethod;

                    if (tearDownMethod == null)
                        tearDownMethod = parentSuite.TearDownMethod;
                }

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

        protected virtual bool IsAssertException(Exception ex)
        {
            return NUnitFramework.IsAssertException( ex );
        }

        protected virtual bool IsIgnoreException(Exception ex)
        {
            return NUnitFramework.IsIgnoreException( ex );
        }
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

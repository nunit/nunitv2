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
		/// <summary>
		/// The fixture object, to be used with this test, or null
		/// </summary>
		private object fixture;

		private MethodInfo  method;
		private MethodInfo setUpMethod;
		private MethodInfo tearDownMethod;

		internal Type expectedException;
		internal string expectedMessage;

		public TestMethod( MethodInfo method ) 
			: this( method, null, null ) { }

		public TestMethod( MethodInfo method,
			Type expectedException, string expectedMessage ) 
			: base( method.ReflectedType.FullName, method.Name )
		{
			this.method = method;
			this.testFramework = TestFramework.FromMethod( method );
			this.expectedException = expectedException;
			this.expectedMessage = expectedMessage;
		}
	
		public override void Run(TestCaseResult testResult)
		{ 
			if ( ShouldRun )
			{
				bool needParentTearDown = false;

				try
				{
					if ( Parent != null )
					{
						if ( Parent.SetUpNeeded  )
						{
							Parent.DoOneTimeSetUp( testResult );
							needParentTearDown = Parent.SetUpComplete;
						}
						
						if ( Parent.SetUpFailed )
							testResult.Failure( "TestFixtureSetUp Failed", null );

						if ( fixture == null )
							fixture = Parent.Fixture;

						if ( setUpMethod == null )
							setUpMethod = Parent.SetUpMethod;

						if ( tearDownMethod == null )
							tearDownMethod = Parent.TearDownMethod;
					}

					if ( !testResult.IsFailure )
						doRun( testResult );
				}
				catch(Exception ex)
				{
					if ( ex is NunitException )
						ex = ex.InnerException;

					RecordException( ex, testResult );
//					if ( testFramework.IsIgnoreException( ex ) )
//						testResult.NotRun( ex.Message );
//					else
//						testResult.Error( ex );
				}
				finally
				{
					if ( needParentTearDown )
						Parent.DoOneTimeTearDown( testResult );
				}
			}
			else
			{
				testResult.NotRun(this.IgnoreReason);
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
					Reflect.InvokeMethod( setUpMethod, this.fixture );

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
			 		tearDownMethod.Invoke( this.fixture, new object[0] );
			}
			catch(Exception ex)
			{
				if ( ex is NunitException )
					ex = ex.InnerException;
				// TODO: What about ignore exceptions in teardown?
				RecordException(ex, testResult);
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

				if ( testFramework.IsIgnoreException( ex ) )
					testResult.NotRun(ex.Message, BuildStackTrace(ex));
				else
					ProcessException(ex, testResult);
			}
		}

		public virtual void RunTestMethod(TestCaseResult testResult)
		{
			Reflect.InvokeMethod( this.method, this.fixture );
		}

		#endregion

		#region Record Info About An Exception

		protected void RecordException( Exception ex, TestResult testResult )
		{
			if ( testFramework.IsIgnoreException( ex ) )
				testResult.NotRun( ex.Message );
			else if ( testFramework.IsAssertException( ex ) )
				testResult.Failure( ex.Message, ex.StackTrace );
			else	
				testResult.Failure( BuildMessage(ex), BuildStackTrace(ex) );
		}

//		protected void RecordException( Exception exception, TestCaseResult testResult )
//		{
//			RecordException( exception, testResult, false );
//		}

//		protected void RecordException( Exception exception, TestCaseResult testResult )
//		{
//			StringBuilder msg = new StringBuilder();
//			StringBuilder st = new StringBuilder();
//			
////			if ( inTearDown )
////			{
////				msg.Append( testResult.Message );
////				msg.Append( Environment.NewLine );
////				msg.Append( "TearDown : " );
////				st.Append( testResult.StackTrace );
////				st.Append( Environment.NewLine );
////				st.Append( "--TearDown" );
////				st.Append( Environment.NewLine );
////			}
//
//			msg.Append( BuildMessage( exception ) );
//			st.Append( BuildStackTrace( exception ) );
//			testResult.Failure( msg.ToString(), st.ToString() );
//		}

		private string BuildMessage(Exception exception)
		{
			StringBuilder sb = new StringBuilder();
			if ( testFramework.IsAssertException( exception ) )
				sb.Append( exception.Message );
			else
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

		#region Virtual Methods

		protected internal virtual void ProcessNoException(TestCaseResult testResult)
		{
			if ( expectedException == null )
				testResult.Success();
			else
				testResult.Failure(expectedException.Name + " was expected", null);
		}
		
		protected internal virtual void ProcessException(Exception exception, TestCaseResult testResult)
		{
			if ( expectedException == null )
			{
				RecordException( exception, testResult );
			}
			else if (expectedException.Equals(exception.GetType()))
			{
				if (expectedMessage != null && !expectedMessage.Equals(exception.Message))
				{
					string message = string.Format("Expected exception to have message: \"{0}\" but received message \"{1}\"", 
						expectedMessage, exception.Message);
					testResult.Failure(message, GetStackTrace(exception) );
				} 
				else 
				{
					testResult.Success();
				}
			}
			else if ( testFramework.IsAssertException( exception ) )
			{
				RecordException(exception,testResult);
			}
			else
			{
				string message = "Expected: " + expectedException.Name + " but was " + exception.GetType().Name;
				testResult.Failure( message, GetStackTrace(exception) );
			}

			return;
		}

		#endregion
	}
}

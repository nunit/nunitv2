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
	using System.Reflection;

	/// <summary>
	/// Summary description for TestCase.
	/// </summary>
	public abstract class TemplateTestCase : TestCase
	{
		private object fixture;
		private MethodInfo  method;


		public TemplateTestCase(object fixture, MethodInfo method) : base(fixture.GetType().FullName, method.Name)
		{
			this.fixture = fixture;
			this.method = method;
		}

		private bool suiteRunning 
		{
			get 
			{
				return (Suite != null && Suite.SuiteRunning);
			}
		}

		public override void Run(TestCaseResult testResult )
		{
			if(ShouldRun)
			{
				DateTime start = DateTime.Now;
#if NUNIT_LEAKAGE_TEST
				long before = System.GC.GetTotalMemory( true );
#endif
				bool setupComplete = false;

				try 
				{
					if ( !suiteRunning ) InvokeTestFixtureSetUp();
					InvokeSetUp();
					setupComplete = true;
					InvokeTestCase();
					ProcessNoException(testResult);
				}
				catch(NunitException exception)
				{
					if ( setupComplete )
						ProcessException(exception.InnerException, testResult); 
					else
						RecordException( exception.InnerException, testResult );
				}
				catch(Exception exp)
				{
					if ( setupComplete )
						ProcessException(exp, testResult);
					else
						RecordException( exp, testResult );
				}
				finally 
				{
					try
					{
						InvokeTearDown();
					}
					catch(NunitException exception)
					{
						RecordException(exception.InnerException, testResult); 
					}
					catch(Exception exp)
					{
						RecordException(exp, testResult);
					}

					if ( !suiteRunning ) InvokeTestFixtureTearDown();
					
					DateTime stop = DateTime.Now;
					TimeSpan span = stop.Subtract(start);
					testResult.Time = (double)span.Ticks / (double)TimeSpan.TicksPerSecond;

#if NUNIT_LEAKAGE_TEST
					long after = System.GC.GetTotalMemory( true );
					testResult.Leakage = after - before;
#endif
				}
			}
			else
			{
				testResult.NotRun(this.IgnoreReason);
			}

			return;
		}

		protected void RecordException( Exception exception, TestCaseResult testResult )
		{
			if(exception is NUnit.Framework.AssertionException)
			{
				NUnit.Framework.AssertionException error = (NUnit.Framework.AssertionException)exception;
				testResult.Failure(BuildMessage(error), BuildStackTrace(error));
			}
			else
			{
				testResult.Failure(BuildMessage(exception), BuildStackTrace(exception));
			}
		}

		private string BuildMessage(Exception exception)
		{
			if(exception.InnerException!=null)
				return exception.Message + Environment.NewLine + BuildMessage(exception.InnerException);
			else
				return exception.Message;
		}
		
		private string BuildStackTrace(Exception exception)
		{
			if(exception.InnerException!=null)
				return exception.StackTrace + Environment.NewLine + 
					"--" + exception.GetType().Name + Environment.NewLine +
					BuildStackTrace(exception.InnerException);
			else
				return exception.StackTrace;
		}

		private void InvokeTestFixtureTearDown()
		{
			MethodInfo method = FindTestFixtureTearDownMethod(fixture);
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		private void InvokeTearDown()
		{
			MethodInfo method = FindTearDownMethod(fixture);
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		private MethodInfo FindTearDownMethod(object fixture)
		{			
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.TearDownAttribute));
		}

		private MethodInfo FindTestFixtureTearDownMethod(object fixture)
		{			
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.TestFixtureTearDownAttribute));
		}

		private void InvokeTestFixtureSetUp()
		{
			MethodInfo method = FindTestFixtureSetUpMethod(fixture);
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		private void InvokeSetUp()
		{
			MethodInfo method = FindSetUpMethod(fixture);
			if(method != null)
			{
				InvokeMethod(method, fixture);
			}
		}

		private MethodInfo FindTestFixtureSetUpMethod(object fixture)
		{
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.TestFixtureSetUpAttribute));
		}

		private MethodInfo FindSetUpMethod(object fixture)
		{
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.SetUpAttribute));
		}

		private void InvokeTestCase() 
		{
			try
			{
				method.Invoke(fixture, null);
			}
			catch(TargetInvocationException e)
			{
				Exception inner = e.InnerException;
				throw new NunitException("Rethrown",inner);
			}
		}

		protected internal abstract void ProcessNoException(TestCaseResult testResult);
		
		protected internal abstract void ProcessException(Exception exception, TestCaseResult testResult);
	}
}

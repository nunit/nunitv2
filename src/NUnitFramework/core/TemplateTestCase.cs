/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
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

		public override void Run(TestCaseResult testResult)
		{
			if(ShouldRun)
			{
				DateTime start = DateTime.Now;

				try 
				{
					InvokeSetUp();
					InvokeTestCase();
					ProcessNoException(testResult);
				}
				catch(NunitException exception)
				{
					ProcessException(exception.InnerException, testResult); 
				}
				catch(Exception exp)
				{
					ProcessException(exp, testResult);
				}
				finally 
				{
					try
					{
						InvokeTearDown();
					}
					catch(NunitException exception)
					{
						ProcessException(exception.InnerException, testResult); 
					}
					catch(Exception exp)
					{
						ProcessException(exp, testResult);
					}
					
					DateTime stop = DateTime.Now;
					TimeSpan span = stop.Subtract(start);
					testResult.Time = (double)span.Ticks / (double)TimeSpan.TicksPerSecond;
				}
			}
			else
			{
				testResult.NotRun(this.IgnoreReason);
			}

			return;
		}

		private void InvokeTearDown()
		{
			MethodInfo method = FindTearDownMethod(fixture);
			if(method != null)
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
		}

		private MethodInfo FindTearDownMethod(object fixture)
		{			
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.TearDownAttribute));
		}

		private void InvokeSetUp()
		{
			MethodInfo method = FindSetUpMethod(fixture);
			if(method != null)
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
		}

		private MethodInfo FindSetUpMethod(object fixture)
		{
			return FindMethodByAttribute(fixture, typeof(NUnit.Framework.SetUpAttribute));
		}

		private MethodInfo FindMethodByAttribute(object fixture, Type type)
		{
			foreach(MethodInfo method in fixture.GetType().GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic))
			{
				if(method.IsDefined(type,true)) 
				{
					return method;
				}
			}
			return null;
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

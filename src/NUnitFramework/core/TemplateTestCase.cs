//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
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

//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Core
{
	using System;
	using System.Collections;
	using System.Reflection;
	using Nunit.Framework;

	/// <summary>
	/// Summary description for TestSuite.
	/// </summary>
	public class TestSuite : Test
	{
		private ArrayList tests = new ArrayList();

		public TestSuite(string name) : base(name)
		{
			ShouldRun = true;
		}

		protected internal virtual void Add(Test test) 
		{
			if(test.ShouldRun)
			{
				test.ShouldRun = ShouldRun;
				test.IgnoreReason = IgnoreReason;
			}
			tests.Add(test);
		}

		protected internal virtual TestSuite CreateNewSuite(string name)
		{
			return new TestSuite(name);
		}

		public void Add(InvalidFixture fixture) 
		{
			TestSuite testSuite = CreateNewSuite(fixture.OriginalType.Name);
			Add(testSuite);
			testSuite.ShouldRun=false;
			testSuite.IgnoreReason = "Fixture is invalid";

			MethodInfo [] methods = fixture.OriginalType.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic);
			foreach(MethodInfo method in methods) {
				TestCase testCase = TestCaseBuilder.Make(fixture, method);
				if(testCase != null) 
				{
					testSuite.Add(testCase);
					testCase.ShouldRun = false;
					testCase.IgnoreReason = "Fixture is non-runnable";
				}
			}

			return;
		}

		public void Add(object fixture) 
		{
			TestSuite testSuite = CreateNewSuite(fixture.GetType().Name);
			Add(testSuite);

			Type ignoreMethodAttribute = typeof(Nunit.Framework.IgnoreAttribute);
			object[] attributes = fixture.GetType().GetCustomAttributes(ignoreMethodAttribute, false);
			if(attributes.Length == 1)
			{
				IgnoreAttribute attr = (IgnoreAttribute)attributes[0];
				testSuite.ShouldRun = false;
				testSuite.IgnoreReason = attr.Reason;
			}

			MethodInfo [] methods = fixture.GetType().GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic);
			foreach(MethodInfo method in methods)
			{
				TestCase testCase = TestCaseBuilder.Make(fixture, method);
				if(testCase != null)
					testSuite.Add(testCase);
			}

			if(testSuite.CountTestCases == 0)
			{
				testSuite.ShouldRun = false;
				testSuite.IgnoreReason = testSuite.Name + " does not have any tests";
			}
		}

		public ArrayList Tests 
		{
			get { return tests; }
		}

		public override int CountTestCases 
		{
			get 
			{
				int count = 0;

				foreach(Test test in Tests)
				{
					count += test.CountTestCases;
				}
				return count;
			}
		}

		public override TestResult Run(EventListener listener)
		{
			TestSuiteResult suiteResult = new TestSuiteResult(this, Name);

			listener.SuiteStarted(this);

			suiteResult.Executed = true;

			long startTime = DateTime.Now.Ticks;
		
			RunAllTests(suiteResult,listener);

			long stopTime = DateTime.Now.Ticks;

			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;

			suiteResult.Time = time;
			if(!ShouldRun) suiteResult.NotRun(this.IgnoreReason);

			listener.SuiteFinished(suiteResult);

			return suiteResult;
		}

		protected virtual void RunAllTests(TestSuiteResult suiteResult,EventListener listener)
		{
			foreach(Test test in Tests)
			{
				suiteResult.AddResult(test.Run(listener));
			}
		}
	}
}

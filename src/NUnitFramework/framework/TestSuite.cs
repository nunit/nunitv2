#region Copyright (c) 2002, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
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
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
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
	using System.Collections;
	using System.Reflection;

	/// <summary>
	/// Summary description for TestSuite.
	/// </summary>
	/// 
	[Serializable]
	public class TestSuite : Test
	{
		private ArrayList tests = new ArrayList();

		public TestSuite(string name) : base(name)
		{
			ShouldRun = true;
		}

		public TestSuite(string parentSuiteName, string name) : base(parentSuiteName,name)
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

		protected internal virtual TestSuite CreateNewSuite(Type type)
		{
			return new TestSuite(type.Namespace,type.Name);
		}

		public void Add(InvalidFixture fixture) 
		{
			TestSuite testSuite = CreateNewSuite(fixture.OriginalType);
			Add(testSuite);
			testSuite.ShouldRun = false;
			testSuite.IgnoreReason = fixture.Message;

			MethodInfo [] methods = fixture.OriginalType.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic);
			foreach(MethodInfo method in methods) 
			{
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
			TestSuite testSuite = CreateNewSuite(fixture.GetType());
			Add(testSuite);

			Type ignoreMethodAttribute = typeof(NUnit.Framework.IgnoreAttribute);
			object[] attributes = fixture.GetType().GetCustomAttributes(ignoreMethodAttribute, false);
			if(attributes.Length == 1)
			{
				NUnit.Framework.IgnoreAttribute attr = (NUnit.Framework.IgnoreAttribute)attributes[0];
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

		public override ArrayList Tests 
		{
			get { return tests; }
		}

		public override bool IsSuite
		{
			get { return true; }
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
#if NUNIT_LEAKAGE_TEST
			long before = System.GC.GetTotalMemory( true );
#endif
			RunAllTests(suiteResult,listener);

#if NUNIT_LEAKAGE_TEST
			long after = System.GC.GetTotalMemory( true );
			suiteResult.Leakage = after - before;
#endif

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

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

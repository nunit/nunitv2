namespace NUnit.Core
{
	using System;
	using System.Collections;

	public class ProxyTestRunner : LongLivingMarshalByRefObject, TestRunner
	{
		TestRunner testRunner;

		public ProxyTestRunner(TestRunner testRunner)
		{
			this.testRunner = testRunner;
		}

		public virtual IList TestFrameworks
		{
			get
			{
				return this.testRunner.TestFrameworks;
			}
		}

		public virtual bool DisplayTestLabels
		{
			get
			{
				return this.testRunner.DisplayTestLabels;
			}
			set
			{
				this.testRunner.DisplayTestLabels = value;
			}
		}

		public virtual TestResult[] Results
		{
			get
			{
				return this.testRunner.Results;
			}
		}

		public virtual TestResult Result
		{
			get
			{
				return this.testRunner.Result;
			}
		}

		public virtual Test Load(string assemblyName)
		{
			return this.testRunner.Load(assemblyName);
		}

		public virtual Test Load(string assemblyName, string testName)
		{
			return this.testRunner.Load(assemblyName, testName);
		}

		public virtual Test Load(TestProject testProject)
		{
			return this.testRunner.Load(testProject);
		}

		public virtual Test Load(TestProject testProject, string testName)
		{
			return this.testRunner.Load(testProject, testName);
		}

		public virtual void Unload()
		{
			this.testRunner.Unload();
		}

		public virtual void SetFilter(IFilter filter)
		{
			this.testRunner.SetFilter(filter);
		}

		public virtual int CountTestCases(string testName)
		{
			return this.testRunner.CountTestCases(testName);
		}

		public virtual int CountTestCases(string[] testNames)
		{
			return this.testRunner.CountTestCases(testNames);
		}

		public virtual ICollection GetCategories()
		{
			return this.testRunner.GetCategories();
		}

		public virtual void CancelRun()
		{
			this.testRunner.CancelRun();
		}

		public virtual void Wait()
		{
			this.testRunner.Wait();
		}

		public virtual TestResult Run(EventListener listener)
		{
			return this.testRunner.Run(listener);
		}

		public virtual TestResult[] Run(EventListener listener, string[] testNames)
		{
			return this.testRunner.Run(listener, testNames);
		}
	}
}

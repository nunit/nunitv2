//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
//
namespace Nunit.Core
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Runtime.Remoting;

	/// <summary>
	/// Summary description for RemoteTestRunner.
	/// </summary>
	public class RemoteTestRunner : MarshalByRefObject
	{
		private TestSuite suite;
		private string fullName;
		private Nunit.Core.EventListener handler;
		private string assemblyName;
		private TextWriter stdOutWriter;
		private TextWriter stdErrWriter;

		public RemoteTestRunner(string assemblyName, Nunit.Core.EventListener handler, TextWriter stdOutWriter, TextWriter stdErrOut)
		{
			this.assemblyName = assemblyName;
			suite = TestSuiteBuilder.Build(assemblyName);
			TestName = suite.FullName;

			this.handler = handler;
			this.stdErrWriter = stdErrOut;
			this.stdOutWriter = stdOutWriter;
		}

		public TestResult Run()
		{
			Console.SetOut(stdOutWriter);
			Console.SetError(stdErrWriter);

			Test test = FindByName(suite, fullName);
			TestResult result = test.Run(handler);
			return result;
		}

		private Test FindByName(Test test, string fullName)
		{
			if(test.FullName.Equals(fullName)) return test;
			
			Test result = null;
			if(test is TestSuite)
			{
				TestSuite suite = (TestSuite)test;
				foreach(Test testCase in suite.Tests)
				{
					result = FindByName(testCase, fullName);
					if(result != null) break;
				}
			}

			return result;
		}

		public string TestName 
		{
			get { return fullName; }
			set { fullName = value; }
		}
			
		public Test Test
		{
			get 
			{ return suite; }
		}

	}
}

using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for NullListener.
	/// </summary>
	public class NullListener : EventListener
	{
		public void TestStarted(TestCase testCase){}
			
		public void TestFinished(TestCaseResult result){}

		public void SuiteStarted(TestSuite suite){}

		public void SuiteFinished(TestSuiteResult result){}

		public static EventListener NULL
		{
			get { return new NullListener();}
		}
	}
}

using System;

namespace Nunit.Gui
{
	using Nunit.Core;

	/// <summary>
	/// Summary description for TestResultItem.
	/// </summary>
	public class TestResultItem
	{
		private TestResult testResult;

		public TestResultItem(TestResult result)
		{
			testResult = result;
		}

		public override string ToString()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string GetMessage()
		{
			return String.Format("{0} : {1}", testResult.Test.Name, testResult.Message);
		}

		public string StackTrace
		{
			get 
			{
				string stackTrace = "No stack trace is available";
				if(testResult.StackTrace != null)
					stackTrace = StackTraceFilter.Filter(testResult.StackTrace);

				return stackTrace;
			}
		}
	}
}

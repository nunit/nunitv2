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

	/// <summary>
	/// Summary description for SiummaryVisitor.
	/// </summary>
	public class SummaryVisitor : ResultVisitor
	{
		private int totalCount;
		private int failureCount;
		private int testsNotRun;
		private int suitesNotRun;
		
		private double time;
		private string name;
		private bool initialized;

		public SummaryVisitor()
		{
			totalCount = 0;
			initialized = false;
		}

		public void visit(TestCaseResult caseResult) 
		{
			SetNameandTime(caseResult.Name, caseResult.Time);

			if(caseResult.Executed)
			{
				totalCount++;
				if(caseResult.IsFailure)
					failureCount++;
			}
			else
				testsNotRun++;
		}

		public void visit(TestSuiteResult suiteResult) 
		{
			SetNameandTime(suiteResult.Name, suiteResult.Time);

			
			
			foreach (TestResult result in suiteResult.Results)
			{
				result.Accept(this);
			}
			
			if(!suiteResult.Executed)
				suitesNotRun++;
		}

		public double Time
		{
			get { return time; }
		}

		private void SetNameandTime(string name, double time)
		{
			if(!initialized)
			{
				this.time = time;
				this.name = name;
				initialized = true;
			}
		}

		public bool Success
		{
			get { return (failureCount == 0); }
		}

		public int Count
		{
			get { return totalCount; }
		}

		public int Failures
		{
			get { return failureCount; }
		}

		public int TestsNotRun
		{
			get { return testsNotRun; }
		}

		public int SuitesNotRun
		{
			get { return suitesNotRun; }
		}

		public string Name
		{
			get { return name; }
		}
	}
}

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
namespace NUnit.Util
{
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using NUnit.Core;
	
	/// <summary>
	/// Type safe TreeNode for use in the TestSuiteTreeView. 
	/// NOTE: Hides some methods and properties of base class.
	/// </summary>
	public class TestNode : TreeNode
	{
		#region Instance variables and constant definitions

		private Test theTest;
		private TestResult theResult;

		private static int INIT = 0;
		private static int SUCCESS = 2;
		private static int FAILURE = 1;
		private static int NOT_RUN = 3;

		#endregion

		#region Constructors

		public TestNode(Test test) : base(test.Name)
		{
			theTest = test;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		public TestNode(TestResult result) : base( result.Test.Name )
		{
			theTest = result.Test;
			theResult = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		#endregion

		#region Properties
		public Test Test
		{
			get { return theTest; }
		}

		public TestResult Result
		{
			get { return theResult; }
		}

		public static int InitIndex
		{
			get { return INIT; }
		}

		public static int SuccessIndex
		{
			get { return SUCCESS; }
		}

		public static int FailureIndex
		{
			get { return FAILURE; }
		}

		public static int NotRunIndex
		{
			get { return NOT_RUN; }
		}
		#endregion

		#region Methods

		public void SetResult( TestResult result )
		{
			if ( result.Test.FullName != theTest.FullName )
				throw( new ArgumentException("Attempting to set Result with a value that refers to a different test") );
			theResult = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		public void ClearResult()
		{
			theResult = null;
			ImageIndex = SelectedImageIndex = INIT;
		}

		public void ClearResults()
		{
			ClearResult();

			foreach(TestNode node in Nodes)
				node.ClearResults();
		}

		private int CalcImageIndex()
		{
			if ( theResult == null )
				return INIT;

			// The following code is a kludge to deal
			// with the fact that Executed is implemented
			// separately in TestCaseResult and TestSuiteResult
			// rather than in the base class.
			if ( theResult is TestCaseResult )
			{
				TestCaseResult result = (TestCaseResult)theResult;
				if (!result.Executed)
					return NOT_RUN;
			}		  
			else	// Must be TestSuiteResult
			{
				TestSuiteResult result = (TestSuiteResult)theResult;
				if (!result.Executed)
					return NOT_RUN;
			}

			if ( theResult.IsSuccess )
				return SUCCESS;
			else if ( theResult.IsFailure )
				return FAILURE;
			else
				return NOT_RUN;
		}

		#endregion
	}
}


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

		/// <summary>
		/// The testcase or testsuite represented by this node
		/// </summary>
		private Test theTest;

		/// <summary>
		/// The result from the last run of the test
		/// </summary>
		private TestResult theResult;

		/// <summary>
		/// Image indices for various test states
		/// </summary>
		private static int INIT = 0;
		private static int SUCCESS = 2;
		private static int FAILURE = 1;
		private static int NOT_RUN = 3;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct a TestNode given a test
		/// </summary>
		public TestNode(Test test) : base(test.Name)
		{
			theTest = test;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		/// <summary>
		/// Construct a TestNode given a TestResult
		/// </summary>
		public TestNode(TestResult result) : base( result.Test.Name )
		{
			theTest = result.Test;
			theResult = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// Test represented by this node
		/// </summary>
		public Test Test
		{
			get { return theTest; }
		}

		/// <summary>
		/// Test result for this node
		/// </summary>
		public TestResult Result
		{
			get { return theResult; }
		}

		/// <summary>
		/// Image index for a test that has not been run
		/// </summary>
		public static int InitIndex
		{
			get { return INIT; }
		}

		/// <summary>
		/// Image index for a test that succeeded
		/// </summary>
		public static int SuccessIndex
		{
			get { return SUCCESS; }
		}

		/// <summary>
		/// Image index for a test that failed
		/// </summary>
		public static int FailureIndex
		{
			get { return FAILURE; }
		}

		/// <summary>
		/// Image index for a test that was not run
		/// </summary>
		public static int NotRunIndex
		{
			get { return NOT_RUN; }
		}
		
		#endregion

		#region Methods

		public void UpdateTest( Test test )
		{
			if ( Test.FullName != test.FullName )
				throw( new ArgumentException( "Attempting to update node with an entirely different test" ) );

			theTest = test;
		}

		/// <summary>
		/// Set the result field of this node, throwing if the
		/// result does not match the test we already hold.
		/// </summary>
		/// <param name="result">Result of the test</param>
		public void SetResult( TestResult result )
		{
			if ( result.Test.FullName != theTest.FullName )
				throw( new ArgumentException("Attempting to set Result with a value that refers to a different test") );
			theResult = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		/// <summary>
		/// Clear the result field of this node
		/// </summary>
		public void ClearResult()
		{
			theResult = null;
			ImageIndex = SelectedImageIndex = INIT;
		}

		/// <summary>
		/// Clear the result of this node and all its children
		/// </summary>
		public void ClearResults()
		{
			ClearResult();

			foreach(TestNode node in Nodes)
				node.ClearResults();
		}

		/// <summary>
		/// Calculate the image index based on the node contents
		/// </summary>
		/// <returns>Image index for this node</returns>
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


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

namespace NUnit.UiKit
{
	using System;
	using System.Collections;
	using System.Windows.Forms;
	using NUnit.Core;
	using NUnit.Util;
	
	/// <summary>
	/// Type safe TreeNode for use in the TestSuiteTreeView. 
	/// NOTE: Hides some methods and properties of base class.
	/// </summary>
	public class TestSuiteTreeNode : TreeNode
	{
		#region Instance variables and constant definitions

		/// <summary>
		/// The testcase or testsuite represented by this node
		/// </summary>
		private UITestNode test;

		/// <summary>
		/// The result from the last run of the test
		/// </summary>
		private TestResult result;

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
		public TestSuiteTreeNode( UITestNode test ) : base(test.Name)
		{
			this.test = test;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		/// <summary>
		/// Construct a TestNode given a TestResult
		/// </summary>
		public TestSuiteTreeNode( TestResult result ) : base( result.Test.Name )
		{
			this.test = new UITestNode( result.Test );
			this.result = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// Test represented by this node
		/// </summary>
		public UITestNode Test
		{
			get { return this.test; }
		}

		/// <summary>
		/// Test result for this node
		/// </summary>
		public TestResult Result
		{
			get { return this.result; }
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

		public void UpdateTest( UITestNode test )
		{
			if ( Test.FullName != test.FullName )
				throw( new ArgumentException( "Attempting to update node with an entirely different test" ) );

			this.test = test;
		}

		/// <summary>
		/// Set the result field of this node, throwing if the
		/// result does not match the test we already hold.
		/// </summary>
		/// <param name="result">Result of the test</param>
		public void SetResult( TestResult result )
		{
			if ( result.Test.FullName != this.test.FullName )
				throw( new ArgumentException("Attempting to set Result with a value that refers to a different test") );
			this.result = result;
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		/// <summary>
		/// Clear the result field of this node
		/// </summary>
		public void ClearResult()
		{
			this.result = null;
			ImageIndex = SelectedImageIndex = INIT;
		}

		/// <summary>
		/// Clear the result of this node and all its children
		/// </summary>
		public void ClearResults()
		{
			ClearResult();

			foreach(TestSuiteTreeNode node in Nodes)
				node.ClearResults();
		}

		/// <summary>
		/// Calculate the image index based on the node contents
		/// </summary>
		/// <returns>Image index for this node</returns>
		private int CalcImageIndex()
		{
			if ( this.result == null )
				return INIT;

			if ( !this.result.Executed )
				return NOT_RUN;

			if ( this.result.IsSuccess )
				return SUCCESS;
			else if ( this.result.IsFailure )
				return FAILURE;
			else
				return NOT_RUN;
		}

		#endregion
	}
}


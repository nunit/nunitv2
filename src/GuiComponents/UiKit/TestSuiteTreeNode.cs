#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
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
	using System.Drawing;
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
		/// Private field used for multiple selection
		/// </summary>
		private bool selected;

		/// <summary>
		/// Private field used for inclusion by category
		/// </summary>
		private bool included;

		/// <summary>
		/// Image indices for various test states
		/// </summary>
		private static int INIT = 0;
		private static int SUCCESS = 2;
		private static int FAILURE = 1;
		private static int NOT_RUN = 3;

		// Save info about expansion and check state to be used
		// when the handle is recreated
		private bool wasExpanded;
		private bool wasChecked;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct a TestNode given a test
		/// </summary>
		public TestSuiteTreeNode( ITest test ) : base(test.Name)
		{
			this.test = new UITestNode(test);
			UpdateImageIndex();
		}

		/// <summary>
		/// Construct a TestNode given a TestResult
		/// </summary>
		public TestSuiteTreeNode( TestResult result ) : base( result.Test.Name )
		{
			this.test = new UITestNode( result.Test );
			this.result = result;
			UpdateImageIndex();
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

//		/// <summary>
//		/// Property used for multiple selection
//		/// </summary>
//		public bool Selected
//		{
//			get { return selected; }
//			set
//			{
//	
//				selected=value;
//
//				if ( selected )
//				{
//					this.BackColor = SystemColors.Highlight;
//					this.ForeColor = SystemColors.HighlightText;
//				}
//				else
//				{
//					this.BackColor = SystemColors.Window;
//					this.ForeColor = SystemColors.WindowText;
//				}
//			}
//		}

		public bool Included
		{
			get { return included; }
			set
			{ 
				included = value;
				this.ForeColor = included ? Color.Black : Color.LightBlue;
			}
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

		public bool WasExpanded
		{
			get { return wasExpanded; }
			set { wasExpanded = value; }
		}
		
		public bool WasChecked
		{
			get { return wasChecked; }
			set { wasChecked = value; }
		}
		
		#endregion

		#region Methods

		public void RestoreVisualState()
		{
			if ( wasExpanded != IsExpanded )
			{
				if ( wasExpanded )
					this.Expand();
				else
					this.Collapse();
			}

			this.Checked = wasChecked;

			foreach ( TestSuiteTreeNode child in this.Nodes )
				child.RestoreVisualState();
		}

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
			UpdateImageIndex();
		}

		/// <summary>
		/// UPdate the image index based on the result field
		/// </summary>
		public void UpdateImageIndex()
		{
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
			
			if ( !this.result.Executed  )
				return NOT_RUN;

			if ( this.result.IsFailure )
				return FAILURE;
			else if ( !this.Result.AllTestsExecuted )
				return NOT_RUN;
			else if ( this.result.IsSuccess )
				return SUCCESS;
			else
				return NOT_RUN;
		}

		internal void Accept(TestSuiteTreeNodeVisitor visitor) 
		{
			visitor.Visit(this);
			foreach (TestSuiteTreeNode node in this.Nodes) 
			{
				node.Accept(visitor);
			}
		}

		#endregion
	}

	public abstract class TestSuiteTreeNodeVisitor 
	{
		public abstract void Visit(TestSuiteTreeNode node);
	}
}


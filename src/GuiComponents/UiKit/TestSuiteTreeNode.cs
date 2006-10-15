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
		private ITest test;

		/// <summary>
		/// The result from the last run of the test
		/// </summary>
		private TestResult result;

		/// <summary>
		/// Private field used for inclusion by category
		/// </summary>
		private bool included;

		/// <summary>
		/// Image indices for various test states - the values 
		/// must match the indices of the image list used
		/// </summary>
		public static readonly int InitIndex = 0;
		public static readonly int SkippedIndex = 0; 
		public static readonly int FailureIndex = 1;
		public static readonly int SuccessIndex = 2;
		public static readonly int IgnoredIndex = 3;

		// Save info about expansion and check state to be used
		// when the handle is recreated
		private bool wasExpanded;
		private bool wasChecked;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct a TestNode given a test
		/// </summary>
		public TestSuiteTreeNode( TestInfo test ) : base(test.TestName.Name)
		{
			this.test = test;
			UpdateImageIndex();
		}

		/// <summary>
		/// Construct a TestNode given a TestResult
		/// </summary>
		public TestSuiteTreeNode( TestResult result ) : base( result.Test.TestName.Name )
		{
			this.test = result.Test;
			this.result = result;
			UpdateImageIndex();
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// Test represented by this node
		/// </summary>
		public ITest Test
		{
			get { return this.test; }
			set	{ this.test = value; }
		}

		/// <summary>
		/// Test result for this node
		/// </summary>
		public TestResult Result
		{
			get { return this.result; }
			set 
			{ 
				this.result = value;
				UpdateImageIndex();
			}
		}

		public string TestType
		{
			get { return test.TestType; }
		}

		public string StatusText
		{
			get
			{
				if ( result == null )
					return test.RunState.ToString();

				if ( !result.Executed )
					return result.RunState.ToString();
				
				return result.ResultState.ToString();
			}
		}

		public bool Included
		{
			get { return included; }
			set
			{ 
				included = value;
				this.ForeColor = included ? SystemColors.WindowText : Color.LightBlue;
			}
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
			ImageIndex = SelectedImageIndex = InitIndex;
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
				return InitIndex;
			
			switch( this.result.RunState )
			{
				case RunState.Runnable:
					return InitIndex;
				case RunState.Skipped:
					return SkippedIndex;
				case RunState.Ignored:
				default:
					return IgnoredIndex;
				case RunState.Executed:
					switch( this.result.ResultState )
					{
						case ResultState.Failure:
						case ResultState.Error:
							return FailureIndex;
						default:
						case ResultState.Success:
							int index = SuccessIndex;
							foreach( TestSuiteTreeNode node in this.Nodes )
							{
								if ( node.ImageIndex == FailureIndex )
									return FailureIndex;
								if ( node.ImageIndex == IgnoredIndex )
									index = IgnoredIndex;
							}
							return index;
					}
			}
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


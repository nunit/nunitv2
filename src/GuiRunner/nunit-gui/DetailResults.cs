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

using System;

namespace NUnit.Gui
{
	using System;
	using System.Windows.Forms;
	using NUnit.Core;

	/// <summary>
	/// Summary description for ResultVisitor.
	/// </summary>
	public class DetailResults : ResultVisitor
	{
		private ListBox testDetails;
		private TreeView notRunTree;

		public DetailResults(ListBox listBox, TreeView notRun)
		{
			testDetails = listBox;
			notRunTree = notRun;
		}

		public void DisplayResults( TestResult results )
		{
			notRunTree.BeginUpdate();
			results.Accept(this);
			notRunTree.EndUpdate();

			if( testDetails.Items.Count > 0 )
				testDetails.SelectedIndex = 0;
		}

		public void visit(TestCaseResult result)
		{
			if(result.Executed)
			{
				if(result.IsFailure)
				{
					TestResultItem item = new TestResultItem(result);
					string resultString = String.Format("{0}:{1}", result.Name, result.Message);
					testDetails.BeginUpdate();
					testDetails.Items.Insert(testDetails.Items.Count, item);
					testDetails.EndUpdate();
				}
			}
			else
			{
				notRunTree.Nodes.Add(MakeNotRunNode(result));
			}
		}

		public void visit(TestSuiteResult suiteResult)
		{
			if(!suiteResult.Executed)
				notRunTree.Nodes.Add(MakeNotRunNode(suiteResult));

			foreach (TestResult result in suiteResult.Results)
			{
				result.Accept(this);
			}
		}

		private TreeNode MakeNotRunNode(TestResult result)
		{
			TreeNode node = new TreeNode(result.Name);

			TreeNode reasonNode = new TreeNode("Reason: " + result.Message);
			
			node.Nodes.Add(reasonNode);

			return node;
		}
	}
}

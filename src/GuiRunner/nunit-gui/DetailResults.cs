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

using System;

namespace Nunit.Gui
{
	using System;
	using System.Windows.Forms;
	using Nunit.Core;

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

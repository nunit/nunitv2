//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace Nunit.Gui
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Microsoft.Win32;
	using Nunit.Core;
	using Nunit.Util;


	/// <summary>
	/// Summary description for UIActions.
	/// </summary>
	public class UIActions : MarshalByRefObject, Nunit.Core.EventListener
	{
		private NunitForm form;
		private Hashtable treeMap = new Hashtable();
		private TestNode currentSelectedNode;
		private TestRunner testRunner = null;
		private static RecentAssemblyUtil assemblyUtil;
		
		private static int INIT = 0;
		private static int SUCCESS = 2;
		private static int FAILURE = 1;
		private static int NOT_RUN = 3;

		static UIActions()	
		{
			assemblyUtil = new RecentAssemblyUtil("recent-assemblies");
		}
		
		public UIActions(NunitForm form)
		{
			this.form = form;
		}

		public void TestStarted(TestCase testCase)
		{
			form.status.Text = "Running : " + testCase.Name; 
			form.progressBar.PerformStep();		
		}

		public void SuiteStarted(TestSuite suite)
		{}

		public void SuiteFinished(TestSuiteResult result)
		{
			TreeNode node = (TreeNode)treeMap[result.Test.FullName];
			if(node != null)
			{
				int imageIndex = FAILURE;
				if(result.Executed)
				{
					if(result.IsSuccess)
						imageIndex = SUCCESS;
				}
				else
				{
					imageIndex = NOT_RUN;
				}

				ChangeNodeImage(node, imageIndex);
			}
			else
			{
				Console.Error.WriteLine("Could not locate node: " + result.Test.FullName + " in tree map");
			}
		}

		private void ChangeNodeImage(TreeNode node, int imageId)
		{
			if(node != null)
			{
				if(node == currentSelectedNode)
					form.assemblyViewer.SelectedImageIndex = imageId;
				form.assemblyViewer.BeginUpdate();
				node.ImageIndex = imageId;
				form.assemblyViewer.EndUpdate();
			}
		}

		public void TestFinished(TestCaseResult result)
		{
			TreeNode node = (TreeNode)treeMap[result.Test.FullName];	

			if(!result.Executed)
			{
				if(form.progressBar.ForeColor == Color.Lime)
					form.progressBar.ForeColor = Color.Yellow;

				ChangeNodeImage(node, NOT_RUN);
			}
			else
			{
				if(result.IsSuccess)
					ChangeNodeImage(node, SUCCESS);
				else
				{
					form.progressBar.ForeColor = Color.Red;

					ChangeNodeImage(node, FAILURE);
				}
			}
		}

		public Test TestSelected()
		{
			TestNode node = (TestNode)form.assemblyViewer.SelectedNode;
			if(node != currentSelectedNode)
			{
				currentSelectedNode = node;

				form.assemblyViewer.BeginUpdate();
				form.assemblyViewer.SelectedImageIndex = INIT;
				form.assemblyViewer.EndUpdate();

				form.suiteName.Text = node.Text;
				ChangeSuite(node.Test);
			}

			return node.Test;
		}

		private void ResetProgressBar()
		{
			form.progressBar.ForeColor = Color.Lime;
			form.progressBar.Value = 0;
		}

		public void RunTestSuite(Test suite)
		{
			ResetProgressBar();
			ClearResults();

			form.runButton.Enabled = false;

			TestResult result = testRunner.Run();
			DisplayResults(result);

			form.runButton.Enabled = true;
			if(form.detailList.Items.Count > 0)
				form.detailList.SelectedIndex = 0;
			form.assemblyViewer.SelectedNode.ExpandAll();
		}

		public void DetailItemSelected()
		{
			TestResultItem resultItem = (TestResultItem)form.detailList.SelectedItem;			
			string stackTrace = resultItem.StackTrace;
			form.stackTrace.Text = resultItem.StackTrace;
			form.toolTip.SetToolTip(form.detailList,resultItem.GetMessage());
		}

		private Test LoadTestSuite(string assemblyFileName)
		{
			return testRunner.Test;
		}
		
		public Test LoadAssembly(string assemblyFileName, Test suite)
		{
			testRunner = new TestRunner(assemblyFileName,this,form.stdOutWriter, form.stdErrWriter);
			
			Test test = LoadTestSuite(assemblyFileName);
			if(!UIHelper.CompareTree(suite,test))
			{
				treeMap.Clear();
				form.assemblyViewer.Nodes.Clear();
				form.assemblyViewer.Nodes.Add(BuildTreeNode(test));
				form.assemblyViewer.ExpandAll();
				TreeNode treeNode = form.assemblyViewer.Nodes[0];
				form.assemblyViewer.SelectedNode = treeNode;
			}

			ChangeSuite(test);

			return test;
		}

		private TreeNode BuildTreeNode(Test rootTest)
		{
			TestNode node = new TestNode(rootTest);
			treeMap.Add(rootTest.FullName, node);
			
			if(rootTest is TestSuite)
			{
				TestSuite testSuite = (TestSuite)rootTest;
				foreach(Test test in testSuite.Tests)
				{
					node.Nodes.Add(BuildTreeNode(test));
				}
			}

			return node;
		}


		private void ChangeSuite(Test test)
		{
			ClearResults();

			InitPanels(test);

			string name = test.Name;
			int val = test.Name.LastIndexOf("\\");
			if(val != -1)
				name = test.Name.Substring(val+1);

			form.suiteName.Text = name;

			form.progressBar.Maximum = test.CountTestCases;
			ResetProgressBar();

			Console.Out.WriteLine("Test name {0}", test.FullName);
			testRunner.TestName = test.FullName;
		}

		private void ClearResults()
		{
			form.detailList.Items.Clear();
			form.stackTrace.Text = "";

			ICollection keys = treeMap.Keys;
			foreach(string key in keys)
			{
				TreeNode node = (TreeNode)treeMap[key];
				node.ImageIndex = INIT;
			}

			form.notRunTree.Nodes.Clear();

			form.stdErrTab.Clear();
			form.stdOutTab.Clear();
		}

		private void InitPanels(Test test)
		{
			form.testCaseCount.Text = "Test Cases : " + test.CountTestCases;
			form.failures.Text = "Failures : 0";
			form.testsRun.Text = "Tests Run : 0";
			form.time.Text = "Time : 0";
		}

		private void DisplayResults(TestResult results)
		{
			DetailResults detailResults = new DetailResults(form.detailList, form.notRunTree);
			form.notRunTree.BeginUpdate();
			results.Accept(detailResults);
			form.notRunTree.EndUpdate();

			ResultSummarizer summarizer = new ResultSummarizer(results);

			int failureCount = summarizer.Failures;

			form.failures.Text = "Failures : " + failureCount.ToString();
			form.testsRun.Text = "Tests run : " + summarizer.ResultCount.ToString();
			form.time.Text = "Time : " + summarizer.Time.ToString();
		}

		public static string GetMostRecentAssembly() 
		{
			return assemblyUtil.RecentAssembly;
		}

		public static void SetMostRecentAssembly(string fileName) 
		{
			assemblyUtil.RecentAssembly = fileName;
		}

		public static IList GetMostRecentAssemblies() 
		{
			return assemblyUtil.GetAssemblies();
		}
	}
}

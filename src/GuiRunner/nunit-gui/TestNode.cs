//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Gui
{
	using System;
	using System.Windows.Forms;
	using NUnit.Core;
	
	/// <summary>
	/// Summary description for TestNode.
	/// </summary>
	public class TestNode : TreeNode
	{
		private Test theTest;

		public TestNode(Test test) : base(test.Name)
		{
			theTest = test;
		}

		public Test Test
		{
			get { return theTest; }
		}
	}
}

using System;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for NameFilter.
	/// </summary>
	/// 
	[Serializable]
	public class NameFilter : IFilter
	{
		private ArrayList testNodes;

		public NameFilter(Test node)
		{
			testNodes = new ArrayList();
			testNodes.Add(node);
		}

		public NameFilter(ArrayList nodes) 
		{
			testNodes = nodes;
		}

		public bool Pass(TestSuite suite) 
		{
			bool passed = false;
			foreach (Test node in testNodes) 
			{
				if (suite.IsDescendant(node) || node.IsDescendant(suite) || node == suite) 
				{
					passed = true;
					break;
				}
			}
			return passed;
		}

		public bool Pass(TestCase test) 
		{
			bool passed = false;
			foreach(Test node in testNodes) 
			{
				if (test.IsDescendant(node) || test == node) 
				{
					passed = true;
					break;
				}
			}

			return passed;
		}
	}
}

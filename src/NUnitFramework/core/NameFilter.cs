using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for NameFilter.
	/// </summary>
	public class NameFilter : IFilter
	{
		private Test node;

		public NameFilter(Test node)
		{
			this.node = node;
		}

		public bool Pass(TestSuite suite) 
		{
			return suite.IsDescendant(node) || node.IsDescendant(suite) || node == suite;
		}

		public bool Pass(TestCase test) 
		{
			return test.IsDescendant(node) || test == node;
		}
	}
}

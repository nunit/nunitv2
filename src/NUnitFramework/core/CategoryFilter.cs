using System;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for CategoryFilter.
	/// </summary>
	/// 
	[Serializable]
	public class CategoryFilter : IFilter
	{
		ArrayList categories;

		public CategoryFilter()
		{
			categories = new ArrayList();
		}

		public void AddCategory(string name) 
		{
			categories.Add(name);
		}
		#region IFilter Members

		public bool Pass(TestSuite suite)
		{
			bool pass = false;

			if (CheckCategories(suite))
				return true;

			foreach (Test test in suite.Tests) 
			{
				pass |= test.Filter(this);
				if (pass)
					break;
			}
			return pass;
		}

		public bool Pass(TestCase test)
		{
			if (CheckCategories(test.Parent))
				return true;
			return CheckCategories(test);
		}
		#endregion

		private bool CheckCategories(Test test) 
		{
			if (test.Categories != null) 
			{
				foreach (string name in categories) 
				{
					if (test.Categories.Contains(name))
						return true;
				}
			}

			return false;
		}
	}
}

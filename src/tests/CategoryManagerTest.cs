using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Tests.Core
{
	[TestFixture]
	public class CategoryManagerTest
	{
		[Test]
		public void NoDuplicates() 
		{
			CategoryManager.Clear();
			string name1 = "Name1";
			string name2 = "Name2";
			string duplicate1 = "Name1";

			CategoryManager.Add(name1);
			CategoryManager.Add(name2);
			CategoryManager.Add(duplicate1);

			Assert.AreEqual(2, CategoryManager.Categories.Count);
		}
	}
}

using System;
using System.Collections;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Test Library for the NUnit CollectionAssert class.
	/// </summary>
	[TestFixture()]
	public class CollectionAssertTest
	{
		const string typeErrorMsg = "\r\n\tAll objects are not of actual type.\r\n\tset1.Count: 3\r\n\tactual: String";
		const string notnullErrorMsg = "\r\n\tAt least one object is null.\r\n\tset1.Count: 3";
		const string uniqueErrorMsg = "\r\n\tAt least one object is not unique within set1.\r\n\tset1.Count: 3";
		const string equalErrorMsg = "\r\n\tset1 and set2 are not equal at index 3";
		const string equalCountErrorMsg = "\r\n\tset1 and set2 do not have equal Count properties.";
		const string equivalentOneErrorMsg = "\r\n\tAn item from set1 was not found in set2.";
		const string equivalentTwoErrorMsg = "\r\n\tAn item from set2 was not found in set1.";

		public CollectionAssertTest()
		{
		}

		#region AllItemsAreInstancesOfType
		[Test()]
		public void ItemsOfType()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string));
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test");
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test {0}","1");

			al = new ArrayList();
			al.Add(new System.Data.DataSet());
			al.Add(new System.Data.DataSet());
			al.Add(new System.Data.DataSet());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(System.Data.DataSet));
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(System.Data.DataSet),"test");
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(System.Data.DataSet),"test {0}","1");
		}

		[Test(), ExpectedException(typeof(NUnit.Framework.AssertionException),"test" + typeErrorMsg)]
		public void ItemsOfTypeFailMsg()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test");
		}

		[Test(), ExpectedException(typeof(NUnit.Framework.AssertionException),"test 1" + typeErrorMsg)]
		public void ItemsOfTypeFailMsgParam()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test {0}","1");
		}

		[Test(), ExpectedException(typeof(NUnit.Framework.AssertionException),typeErrorMsg)]
		public void ItemsOfTypeFailNoMsg()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string));
		}
		#endregion

		#region AllItemsAreNotNull
		[Test()]
		public void ItemsNotNull()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al);
			CollectionAssert.AllItemsAreNotNull(al,"test");
			CollectionAssert.AllItemsAreNotNull(al,"test {0}","1");

			al = new ArrayList();
			al.Add(new System.Data.DataSet());
			al.Add(new System.Data.DataSet());
			al.Add(new System.Data.DataSet());

			CollectionAssert.AllItemsAreNotNull(al);
			CollectionAssert.AllItemsAreNotNull(al,"test");
			CollectionAssert.AllItemsAreNotNull(al,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),notnullErrorMsg)]
		public void ItemsNotNullFail()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al);
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + notnullErrorMsg)]
		public void ItemsNotNullFailMsgParam()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test" + notnullErrorMsg)]
		public void ItemsNotNullFailMsg()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al,"test");
		}
		#endregion

		#region AllItemsAreUnique

		[Test]
		public void Unique()
		{
			ArrayList al = new ArrayList();
			al.Add(new object());
			al.Add(new object());
			al.Add(new object());

			CollectionAssert.AllItemsAreUnique(al);
			CollectionAssert.AllItemsAreUnique(al,"test");
			CollectionAssert.AllItemsAreUnique(al,"test {0}","1");

			al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			CollectionAssert.AllItemsAreUnique(al);
			CollectionAssert.AllItemsAreUnique(al,"test");
			CollectionAssert.AllItemsAreUnique(al,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),uniqueErrorMsg)]
		public void UniqueFail()
		{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al);
		}

		[Test, ExpectedException(typeof(AssertionException),"test" + uniqueErrorMsg)]
		public void UniqueFailMsg()
		{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al,"test");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + uniqueErrorMsg)]
		public void UniqueFailMsgParam()
		{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al,"test {0}","1");
		}

		#endregion

		#region AreEqual

		[Test]
		public void AreEqual()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");
			set2.Add("x");
			set2.Add("y");
			set2.Add("z");

			CollectionAssert.AreEqual(set1,set2);
			CollectionAssert.AreEqual(set1,set2,new TestComparer());
			CollectionAssert.AreEqual(set1,set2,"test");
			CollectionAssert.AreEqual(set1,set2,new TestComparer(),"test");
			CollectionAssert.AreEqual(set1,set2,"test {0}","1");
			CollectionAssert.AreEqual(set1,set2,new TestComparer(),"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + equalCountErrorMsg)]
		public void AreEqualFailCount()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");
			set2.Add("x");
			set2.Add("y");
			set2.Add("z");
			set2.Add("a");

			CollectionAssert.AreEqual(set1,set2,new TestComparer(),"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + equalErrorMsg)]
		public void AreEqualFail()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");
			set2.Add("x");
			set2.Add("y");
			set2.Add("a");

			CollectionAssert.AreEqual(set1,set2,new TestComparer(),"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + equalErrorMsg)]
		public void AreEqualFailObject()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();
			System.Data.DataSet a = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);
			set2.Add(x);
			set2.Add(y);
			set2.Add(a);

			CollectionAssert.AreEqual(set1,set2,new TestComparer(),"test {0}","1");
		}

		#endregion

		#region AreEquivalent

		[Test]
		public void Equivalent()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			set2.Add(z);
			set2.Add(y);
			set2.Add(x);

			CollectionAssert.AreEquivalent(set1,set2);
			CollectionAssert.AreEquivalent(set1,set2,"test");
			CollectionAssert.AreEquivalent(set1,set2,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + equivalentOneErrorMsg)]
		public void EquivalentFailOne()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			set2.Add(x);
			set2.Add(y);
			set2.Add(x);

			CollectionAssert.AreEquivalent(set1,set2,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException),"test 1" + equivalentTwoErrorMsg)]
		public void EquivalentFailTwo()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(y);
			set1.Add(x);

			set2.Add(x);
			set2.Add(y);
			set2.Add(z);

			CollectionAssert.AreEquivalent(set1,set2,"test {0}","1");
		}
		#endregion

		#region AreNotEqual

		[Test]
		public void AreNotEqual()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");
			set2.Add("x");
			set2.Add("y");
			set2.Add("x");

			CollectionAssert.AreNotEqual(set1,set2);
			CollectionAssert.AreNotEqual(set1,set2,new TestComparer());
			CollectionAssert.AreNotEqual(set1,set2,"test");
			CollectionAssert.AreNotEqual(set1,set2,new TestComparer(),"test");
			CollectionAssert.AreNotEqual(set1,set2,"test {0}","1");
			CollectionAssert.AreNotEqual(set1,set2,new TestComparer(),"test {0}","1");
		}

		#endregion

		#region AreNotEquivalent

		[Test]
		public void NotEquivalent()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			set2.Add(x);
			set2.Add(y);
			set2.Add(x);

			CollectionAssert.AreNotEquivalent(set1,set2);
			CollectionAssert.AreNotEquivalent(set1,set2,"test");
			CollectionAssert.AreNotEquivalent(set1,set2,"test {0}","1");
		}

		#endregion

		#region Contains
		[Test]
		public void Contains()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();
			System.Data.DataSet a = new System.Data.DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.Contains(al,x);
			CollectionAssert.Contains(al,x,"test");
			CollectionAssert.Contains(al,x,"test {0}","1");
		}
		#endregion

		#region DoesNotContain
		[Test]
		public void DoesNotContain()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();
			System.Data.DataSet a = new System.Data.DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.DoesNotContain(al,a);
			CollectionAssert.DoesNotContain(al,a,"test");
			CollectionAssert.DoesNotContain(al,a,"test {0}","1");
		}
		#endregion

		#region IsSubsetOf
		[Test]
		public void IsSubsetOf()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();
			System.Data.DataSet a = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set2.Add(y);
			set2.Add(z);

			CollectionAssert.IsSubsetOf(set1,set2);
			CollectionAssert.IsSubsetOf(set1,set2,"test");
			CollectionAssert.IsSubsetOf(set1,set2,"test {0}","1");
		}
		#endregion

		#region IsNotSubsetOf
		[Test]
		public void IsNotSubsetOf()
		{
			System.Data.DataSet x = new System.Data.DataSet();
			System.Data.DataSet y = new System.Data.DataSet();
			System.Data.DataSet z = new System.Data.DataSet();
			System.Data.DataSet a = new System.Data.DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set1.Add(y);
			set1.Add(z);
			set2.Add(a);

			CollectionAssert.IsNotSubsetOf(set1,set2);
			CollectionAssert.IsNotSubsetOf(set1,set2,"test");
			CollectionAssert.IsNotSubsetOf(set1,set2,"test {0}","1");
		}
		#endregion
	}

	public class TestComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (x.Equals(y))
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}

		#endregion

	}

}



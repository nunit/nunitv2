using System;
using System.Collections;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Test Library for the NUnit CollectionAssert class.
	/// </summary>
	[TestFixture()]
	//[Platform(Exclude="Linux")]
	public class CollectionAssertTest
	{
		static string typeErrorMsg = "" + System.Environment.NewLine + "\tAll objects are not of actual type." + System.Environment.NewLine + "\tset1.Count: 3" + System.Environment.NewLine + "\tactual: String";
		static string notnullErrorMsg = "" + System.Environment.NewLine + "\tAt least one object is null." + System.Environment.NewLine + "\tset1.Count: 3";
		static string uniqueErrorMsg = "" + System.Environment.NewLine + "\tAt least one object is not unique within set1." + System.Environment.NewLine + "\tset1.Count: 3";
		static string equalErrorMsg = "" + System.Environment.NewLine + "\tset1 and set2 are not equal at index 3";
		static string equalCountErrorMsg = "" + System.Environment.NewLine + "\tset1 and set2 do not have equal Count properties.";
		static string equivalentOneErrorMsg = "" + System.Environment.NewLine + "\tAn item from set1 was not found in set2.";
		static string equivalentTwoErrorMsg = "" + System.Environment.NewLine + "\tAn item from set2 was not found in set1.";

		public CollectionAssertTest()
		{
		}

		private void CheckException(Exception actual, Type expectedExceptionType, string expectedErrorMsg)
		{
			if(actual == null)
				Assert.Fail("Expected " + expectedExceptionType.ToString() + " but no exception was thrown");

			Assert.AreEqual(expectedExceptionType, actual.GetType(), "Expected Exception not thrown");
			Assert.AreEqual(expectedErrorMsg, actual.Message);
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

		[Test()]
		public void ItemsOfTypeFailMsg()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test");
			} 
			catch (Exception caughtException) 
			{
				ex = caughtException;
			}

			CheckException(ex, typeof(NUnit.Framework.AssertionException), "test" + typeErrorMsg);
		}

		[Test()]
		public void ItemsOfTypeFailMsgParam()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string),"test {0}","1");
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}
			
			CheckException(ex, typeof(NUnit.Framework.AssertionException), "test 1" + typeErrorMsg);
		}

		[Test()]
		public void ItemsOfTypeFailNoMsg()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string));
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex,typeof(NUnit.Framework.AssertionException),typeErrorMsg);
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

		[Test]
		public void ItemsNotNullFail()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al);
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),notnullErrorMsg);
		}

		[Test]
		public void ItemsNotNullFailMsgParam()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al,"test {0}","1");
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + notnullErrorMsg);
		}

		[Test]
		public void ItemsNotNullFailMsg()
		{
			Exception ex = null;
			try
			{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			CollectionAssert.AllItemsAreNotNull(al,"test");
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test" + notnullErrorMsg);
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

		[Test]
		public void UniqueFail()
		{
			Exception ex = null;
			try
			{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al);
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),uniqueErrorMsg);
		}

		[Test]
		public void UniqueFailMsg()
		{
			Exception ex = null;
			try
			{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al,"test");
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test" + uniqueErrorMsg);
		}

		[Test]
		public void UniqueFailMsgParam()
		{
			Exception ex = null;
			try
			{
			object x = new object();
			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(new object());
			al.Add(x);

			CollectionAssert.AllItemsAreUnique(al,"test {0}","1");
			}
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + uniqueErrorMsg);
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

		[Test]
		public void AreEqualFailCount()
		{
			Exception ex = null;
			try
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
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + equalCountErrorMsg);
		}

		[Test]
		public void AreEqualFail()
		{
			Exception ex = null;
			try
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
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + equalErrorMsg);
		}

		[Test]
		public void AreEqualFailObject()
		{
			Exception ex = null;
			try
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
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + equalErrorMsg);
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

		[Test]
		public void EquivalentFailOne()
		{
			Exception ex = null;
			try
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
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + equivalentOneErrorMsg);
		}

		[Test]
		public void EquivalentFailTwo()
		{
			Exception ex = null;
			try
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
			catch(Exception actualException)
			{
				ex = actualException;
			}

			CheckException(ex, typeof(AssertionException),"test 1" + equivalentTwoErrorMsg);
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



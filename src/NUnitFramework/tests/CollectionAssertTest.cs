using System;
using System.Collections;
using System.Data;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Test Library for the NUnit CollectionAssert class.
	/// </summary>
	[TestFixture()]
	//[Platform(Exclude="Linux")]
	public class CollectionAssertTest
	{
		static string typeErrorMsg = "\tAll objects are not of actual type." + Environment.NewLine + "\tcollection1.Count: 3" + Environment.NewLine + "\tactual: String" + Environment.NewLine;
		static string notnullErrorMsg = "\tAt least one object is null." + Environment.NewLine + "\tcollection1.Count: 3" + Environment.NewLine;
		static string uniqueErrorMsg = "\tAt least one object is not unique within collection1." + Environment.NewLine + "\tcollection1.Count: 3" + Environment.NewLine;
		static string equalErrorMsg = "" + Environment.NewLine + "\tcollection1 and collection2 are not equal at index 3" + Environment.NewLine;
		static string equalCountErrorMsg = "" + Environment.NewLine + "\tcollection1 and collection2 do not have equal Count properties." + Environment.NewLine;
		static string equivalentOneErrorMsg = "" + Environment.NewLine + "\tAn item from collection1 was not found in collection2." + Environment.NewLine;
		static string equivalentTwoErrorMsg = "" + Environment.NewLine + "\tAn item from collection2 was not found in collection1." + Environment.NewLine;

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
			al.Add(new DataSet());
			al.Add(new DataSet());
			al.Add(new DataSet());
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(DataSet));
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(DataSet),"test");
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(DataSet),"test {0}","1");
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

			CheckException(ex, typeof(NUnit.Framework.AssertionException), "test" + Environment.NewLine + typeErrorMsg);
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
			
			CheckException(ex, typeof(NUnit.Framework.AssertionException), "test 1" + Environment.NewLine + typeErrorMsg);
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
			al.Add(new DataSet());
			al.Add(new DataSet());
			al.Add(new DataSet());

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

			CheckException(ex, typeof(AssertionException), notnullErrorMsg);
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

			CheckException(ex, typeof(AssertionException),"test 1" + Environment.NewLine + notnullErrorMsg);
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

			CheckException(ex, typeof(AssertionException),"test" + Environment.NewLine + notnullErrorMsg);
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

			CheckException(ex, typeof(AssertionException),"test" + Environment.NewLine + uniqueErrorMsg);
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

			CheckException(ex, typeof(AssertionException),"test 1" + Environment.NewLine + uniqueErrorMsg);
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

			Assert.AreEqual(set1,set2);
			//Assert.AreEqual(set1,set2,new TestComparer());
			Assert.AreEqual(set1,set2,"test");
			//Assert.AreEqual(set1,set2,new TestComparer(),"test");
			Assert.AreEqual(set1,set2,"test {0}","1");
			//Assert.AreEqual(set1,set2,new TestComparer(),"test {0}","1");
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
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();
			DataSet a = new DataSet();

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

		[Test]
		public void AreEqual_HandlesNull()
		{
			object[] set1 = new object[3];
			object[] set2 = new object[3];

			CollectionAssert.AreEqual(set1,set2);
			CollectionAssert.AreEqual(set1,set2,new TestComparer());
		}

		#endregion

		#region AreEquivalent

		[Test]
		public void Equivalent()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

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
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

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
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

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

		[Test]
		public void AreEquivalentHandlesNull()
		{
			DataSet x = new DataSet();
			DataSet z = new DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(null);
			set1.Add(z);

			set2.Add(z);
			set2.Add(null);
			set2.Add(x);

			CollectionAssert.AreEquivalent(set1,set2);
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

		[Test, ExpectedException(typeof(AssertionException))]
		public void AreNotEqual_Fails()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");
			set2.Add("x");
			set2.Add("y");
			set2.Add("z");

			CollectionAssert.AreNotEqual(set1,set2);
		}

		[Test]
		public void AreNotEqual_HandlesNull()
		{
			object[] set1 = new object[3];
			ArrayList set2 = new ArrayList();
			set2.Add("x");
			set2.Add("y");
			set2.Add("z");

			CollectionAssert.AreNotEqual(set1,set2);
			//CollectionAssert.AreNotEqual(set1,set2,new TestComparer());
		}

		#endregion

		#region AreNotEquivalent

		[Test]
		public void NotEquivalent()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

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

		[Test, ExpectedException(typeof(AssertionException))]
		public void NotEquivalent_Fails()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			set2.Add(x);
			set2.Add(z);
			set2.Add(y);

			CollectionAssert.AreNotEquivalent(set1,set2);
		}

		[Test]
		public void NotEquivalentHandlesNull()
		{
			DataSet x = new DataSet();
			DataSet z = new DataSet();

			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add(x);
			set1.Add(null);
			set1.Add(z);

			set2.Add(x);
			set2.Add(null);
			set2.Add(x);

			CollectionAssert.AreNotEquivalent(set1,set2);
		}
		#endregion

		#region Contains
		[Test]
		public void Contains()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.Contains(al,x);
			CollectionAssert.Contains(al,x,"test");
			CollectionAssert.Contains(al,x,"test {0}","1");
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();
			DataSet a = new DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.Contains(al,a);
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails_Empty()
		{
			DataSet x = new DataSet();

			ArrayList al = new ArrayList();

			CollectionAssert.Contains(al,x);
		}
		#endregion

		#region DoesNotContain
		[Test]
		public void DoesNotContain()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();
			DataSet a = new DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.DoesNotContain(al,a);
			CollectionAssert.DoesNotContain(al,a,"test");
			CollectionAssert.DoesNotContain(al,a,"test {0}","1");
		}

		[Test]
		public void DoesNotContain_Empty()
		{
			DataSet x = new DataSet();

			ArrayList al = new ArrayList();

			CollectionAssert.DoesNotContain(al,x);
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void DoesNotContain_Fails()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

			ArrayList al = new ArrayList();
			al.Add(x);
			al.Add(y);
			al.Add(z);

			CollectionAssert.DoesNotContain(al,y);
		}
		#endregion

		#region IsSubsetOf
		[Test]
		public void IsSubsetOf()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

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

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsSubsetOf_Fails()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();
			DataSet a = new DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set2.Add(y);
			set2.Add(z);
			set2.Add(a);

			CollectionAssert.IsSubsetOf(set1,set2);
		}

		[Test]
		public void IsSubsetOfHandlesNull()
		{
			DataSet x = new DataSet();
			DataSet y = null;
			DataSet z = new DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set2.Add(y);
			set2.Add(z);

			CollectionAssert.IsSubsetOf(set1,set2);
		}
		#endregion

		#region IsNotSubsetOf
		[Test]
		public void IsNotSubsetOf()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();
			DataSet a = new DataSet();

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

		[Test, ExpectedException(typeof(AssertionException))]
		public void IsNotSubsetOf_Fails()
		{
			DataSet x = new DataSet();
			DataSet y = new DataSet();
			DataSet z = new DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set1.Add(y);
			set1.Add(z);

			CollectionAssert.IsNotSubsetOf(set1,set2);
		}
		
		[Test]
		public void IsNotSubsetOfHandlesNull()
		{
			DataSet x = new DataSet();
			DataSet y = null;
			DataSet z = new DataSet();
			DataSet a = new DataSet();

			ArrayList set1 = new ArrayList();
			set1.Add(x);
			set1.Add(y);
			set1.Add(z);

			ArrayList set2 = new ArrayList();
			set1.Add(y);
			set1.Add(z);
			set2.Add(a);

			CollectionAssert.IsNotSubsetOf(set1,set2);
		}
		#endregion
	}

	public class TestComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if ( x == null && y == null )
				return 0;

			if ( x == null || y == null )
				return -1;

			if (x.Equals(y))
				return 0;

			return -1;
		}

		#endregion

	}

}



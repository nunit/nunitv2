// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using System.Data;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Test Library for the NUnit CollectionAssert class.
	/// </summary>
	[TestFixture()]
	public class CollectionAssertTest : MessageChecker
	{
		#region AllItemsAreInstancesOfType
		[Test()]
		public void ItemsOfType()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");
			CollectionAssert.AllItemsAreInstancesOfType(al,typeof(string));
        }

		[Test,ExpectedException(typeof(AssertionException))]
		public void ItemsOfTypeFailure()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add(new object());

			expectedMessage =
				"  Expected: all items instance of <System.String>" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", <System.Object> >" + Environment.NewLine;
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
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ItemsNotNullFailure()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add(null);
			al.Add("z");

			expectedMessage =
                "  Expected: all items not null" + Environment.NewLine +
                "  But was:  < \"x\", null, \"z\" >" + Environment.NewLine;
			CollectionAssert.AllItemsAreNotNull(al);
		}
		#endregion

		#region AllItemsAreUnique

		[Test]
		public void Unique_IList()
		{
			ArrayList al = new ArrayList();
			al.Add(new object());
			al.Add(new object());
			al.Add(new object());

			CollectionAssert.AllItemsAreUnique(al);

			al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			CollectionAssert.AllItemsAreUnique(al);
		}

		[Test]
		public void Unique_ICollection()
		{
			CollectionAdapter ca = new CollectionAdapter( new string[] { "x", "y", "z" } );
			CollectionAssert.AllItemsAreUnique(ca);
		}

		[Test]
		public void Unique_WithNull()
		{
			CollectionAdapter ca = new CollectionAdapter( new string[] { "x", "y", null, "z" } );
			CollectionAssert.AllItemsAreUnique(ca);
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void UniqueFailure()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("x");

			expectedMessage =
                "  Expected: all items unique" + Environment.NewLine +
                "  But was:  < \"x\", \"y\", \"x\" >" + Environment.NewLine;
			CollectionAssert.AllItemsAreUnique(al);
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void UniqueFailure_WithTwoNulls()
		{
			CollectionAdapter ca = new CollectionAdapter( new string[] { "x", null, "y", null, "z" } );
			CollectionAssert.AllItemsAreUnique(ca);
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

			Assert.AreEqual(set1,set2);
		}

		[Test,ExpectedException(typeof(AssertionException))]
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

			expectedMessage =
                "  Expected and actual are both <System.Collections.ArrayList> with 3 elements" + Environment.NewLine +
                "  Values differ at index [3]" + Environment.NewLine +
                "  Extra:    < \"a\" >";
			CollectionAssert.AreEqual(set1,set2,new TestComparer());
		}

        [Test, ExpectedException(typeof(AssertionException))]
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

			expectedMessage =
                "  Expected and actual are both <System.Collections.ArrayList> with 3 elements" + Environment.NewLine +
                "  Values differ at index [2]" + Environment.NewLine +
                "  String lengths are both 1. Strings differ at index 0." + Environment.NewLine +
                "  Expected: \"z\"" + Environment.NewLine +
                "  But was:  \"a\"" + Environment.NewLine +
                "  -----------^" + Environment.NewLine;
			CollectionAssert.AreEqual(set1,set2,new TestComparer());
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
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			set2.Add("z");
			set2.Add("y");
			set2.Add("x");

			CollectionAssert.AreEquivalent(set1,set2);
		}

        [Test, ExpectedException(typeof(AssertionException))]
        public void EquivalentFailOne()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			set2.Add("x");
			set2.Add("y");
			set2.Add("x");

			expectedMessage =
                "  Expected: equivalent to < \"x\", \"y\", \"z\" >" + Environment.NewLine +
                "  But was:  < \"x\", \"y\", \"x\" >" + Environment.NewLine;
			CollectionAssert.AreEquivalent(set1,set2);
		}

        [Test, ExpectedException(typeof(AssertionException))]
        public void EquivalentFailTwo()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add("y");
			set1.Add("x");

			set2.Add("x");
			set2.Add("y");
			set2.Add("z");

			expectedMessage =
                "  Expected: equivalent to < \"x\", \"y\", \"x\" >" + Environment.NewLine +
                "  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.AreEquivalent(set1,set2);
		}

        [Test]
		public void AreEquivalentHandlesNull()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add(null);
			set1.Add("z");

			set2.Add("z");
			set2.Add(null);
			set2.Add("x");

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

			expectedMessage = 
				"  Expected: not < \"x\", \"y\", \"z\" >" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
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
			CollectionAssert.AreNotEqual(set1,set2,new TestComparer());
		}

		#endregion

		#region AreNotEquivalent

		[Test]
		public void NotEquivalent()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			set2.Add("x");
			set2.Add("y");
			set2.Add("x");

			CollectionAssert.AreNotEquivalent(set1,set2);
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void NotEquivalent_Fails()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			set2.Add("x");
			set2.Add("z");
			set2.Add("y");

			expectedMessage =
				"  Expected: not equivalent to < \"x\", \"y\", \"z\" >" + Environment.NewLine +
				"  But was:  < \"x\", \"z\", \"y\" >" + Environment.NewLine;
			CollectionAssert.AreNotEquivalent(set1,set2);
		}

		[Test]
		public void NotEquivalentHandlesNull()
		{
			ArrayList set1 = new ArrayList();
			ArrayList set2 = new ArrayList();
			
			set1.Add("x");
			set1.Add(null);
			set1.Add("z");

			set2.Add("x");
			set2.Add(null);
			set2.Add("x");

			CollectionAssert.AreNotEquivalent(set1,set2);
		}
		#endregion

		#region Contains
		[Test]
		public void Contains_IList()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			CollectionAssert.Contains(al,"x");
		}

		[Test]
		public void Contains_ICollection()
		{
			CollectionAdapter ca = new CollectionAdapter( new string[] { "x", "y", "z" } );

			CollectionAssert.Contains(ca,"x");
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails_ILIst()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			expectedMessage =
				"  Expected: collection containing \"a\"" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.Contains(al,"a");
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails_ICollection()
		{
			CollectionAdapter ca = new CollectionAdapter( new string[] { "x", "y", "z" } );

			expectedMessage =
				"  Expected: collection containing \"a\"" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.Contains(ca,"a");
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails_EmptyIList()
		{
			ArrayList al = new ArrayList();

			expectedMessage =
				"  Expected: collection containing \"x\"" + Environment.NewLine +
				"  But was:  <empty>" + Environment.NewLine;
			CollectionAssert.Contains(al,"x");
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void ContainsFails_EmptyICollection()
		{
			CollectionAdapter ca = new CollectionAdapter( new object[0] );

			expectedMessage =
				"  Expected: collection containing \"x\"" + Environment.NewLine +
				"  But was:  <empty>" + Environment.NewLine;
			CollectionAssert.Contains(ca,"x");
		}

		[Test]
		public void ContainsNull_IList()
		{
			Object[] oa = new object[] { 1, 2, 3, null, 4, 5 };
			CollectionAssert.Contains( oa, null );
		}

		[Test]
		public void ContainsNull_ICollection()
		{
			CollectionAdapter ca = new CollectionAdapter( new object[] { 1, 2, 3, null, 4, 5 } );
			CollectionAssert.Contains( ca, null );
		}
		#endregion

		#region DoesNotContain
		[Test]
		public void DoesNotContain()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			CollectionAssert.DoesNotContain(al,"a");
		}

		[Test]
		public void DoesNotContain_Empty()
		{
			ArrayList al = new ArrayList();

			CollectionAssert.DoesNotContain(al,"x");
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void DoesNotContain_Fails()
		{
			ArrayList al = new ArrayList();
			al.Add("x");
			al.Add("y");
			al.Add("z");

			expectedMessage = 
				"  Expected: not collection containing \"y\"" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.DoesNotContain(al,"y");
		}
		#endregion

		#region IsSubsetOf
		[Test]
		public void IsSubsetOf()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set2.Add("y");
			set2.Add("z");

			CollectionAssert.IsSubsetOf(set2,set1);
            Expect(set2, SubsetOf(set1));
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsSubsetOf_Fails()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set2.Add("y");
			set2.Add("z");
			set2.Add("a");

			expectedMessage =
				"  Expected: subset of < \"y\", \"z\", \"a\" >" + Environment.NewLine +
				"  But was:  < \"x\", \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.IsSubsetOf(set1,set2);
		}

		[Test]
		public void IsSubsetOfHandlesNull()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add(null);
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set2.Add(null);
			set2.Add("z");

			CollectionAssert.IsSubsetOf(set2,set1);
            Expect(set2, SubsetOf(set1));
		}
		#endregion

		#region IsNotSubsetOf
		[Test]
		public void IsNotSubsetOf()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set1.Add("y");
			set1.Add("z");
			set2.Add("a");

			CollectionAssert.IsNotSubsetOf(set1,set2);
            Expect(set1, Not.SubsetOf(set2));
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void IsNotSubsetOf_Fails()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add("y");
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set2.Add("y");
			set2.Add("z");

			expectedMessage =
				"  Expected: not subset of < \"x\", \"y\", \"z\" >" + Environment.NewLine +
				"  But was:  < \"y\", \"z\" >" + Environment.NewLine;
			CollectionAssert.IsNotSubsetOf(set2,set1);
		}
		
		[Test]
		public void IsNotSubsetOfHandlesNull()
		{
			ArrayList set1 = new ArrayList();
			set1.Add("x");
			set1.Add(null);
			set1.Add("z");

			ArrayList set2 = new ArrayList();
			set1.Add(null);
			set1.Add("z");
			set2.Add("a");

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



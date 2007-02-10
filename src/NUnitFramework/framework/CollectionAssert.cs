using System;
using System.Collections;
using System.ComponentModel;

namespace NUnit.Framework
{
    #region Asserters

    #region CollectionAsserter
    /// <summary>
	/// Abstract base class for all CollectionAsserters
	/// </summary>
	public abstract class CollectionAsserter : AbstractAsserter
	{
        /// <summary>
        /// The first collection to be tested
        /// </summary>
		protected ICollection collection1;

        /// <summary>
        /// The second collection to be tested, or null
        /// </summary>
        protected ICollection collection2;

        /// <summary>
        /// An IComparer for use in testing equality, or null
        /// </summary>
        protected IComparer comparer;

        /// <summary>
        /// Construct a CollectionAsserter supplying a single set
        /// </summary>
        /// <param name="collection1">The set to be tested</param>
        /// <param name="message">A message to issue in case of failure</param>
        /// <param name="args">Parameters used in formatting the message</param>
        public CollectionAsserter(ICollection collection1, string message, params object[] args)
            : base(message, args)
        {
            this.collection1 = collection1;
        }

        /// <summary>
        /// Construct a CollectionAsserter supplying two sets
        /// </summary>
        /// <param name="collection1">The first set to be tested</param>
        /// <param name="collection2">The second set to be tested</param>
        /// <param name="message">A message to issue in case of failure</param>
        /// <param name="args">Parameters used in formatting the message</param>
        public CollectionAsserter(ICollection collection1, ICollection collection2, string message, params object[] args)
            : base(message, args)
        {
            this.collection1 = collection1;
            this.collection2 = collection2;
        }

        /// <summary>
        /// Construct a CollectionAsserter supplying two sets and a comparer
        /// </summary>
        /// <param name="collection1">The first set to be tested</param>
        /// <param name="collection2">The second set to be tested</param>
        /// <param name="comparer">An IComparer object used in testing for equality</param>
        /// <param name="message">A message to issue in case of failure</param>
        /// <param name="args">Parameters used in formatting the message</param>
        public CollectionAsserter(ICollection collection1, ICollection collection2, IComparer comparer, string message, params object[] args) 
            : base(message, args)
		{
            this.collection1 = collection1;
            this.collection2 = collection2;
            this.comparer = comparer;
		}

        #region Utility Methods Used by Derived Classes
        /// <summary>
        /// Test whether two collections are equal, that is, whether
        /// they contain the same elements in the same order.
        /// </summary>
        /// <param name="collection1">The first collection</param>
        /// <param name="collection2">The second collection</param>
        /// <returns>True if the collection are equal, otherwise false</returns>
        protected bool CollectionsEqual(ICollection collection1, ICollection collection2)
        {
            int collection1iteration = 0;
            int collection2iteration = 0;

            if (collection1.Count != collection2.Count)
            {
                FailureMessage.WriteLine("\tcollection1 and collection2 do not have equal Count properties.");
                return false;
            }

            foreach (object collection1Obj in collection1)
            {
                collection2iteration = 0;
                collection1iteration += 1;

                foreach (object collection2Obj in collection2)
                {
                    collection2iteration += 1;

                    if (collection2iteration > collection1iteration) break;
                    if (collection2iteration == collection1iteration)
                    {
                        if (!MembersEqual( collection1Obj, collection2Obj ) )
                        {
                            FailureMessage.WriteLine("\tcollection1 and collection2 are not equal at index {0}", collection1iteration);
                            return false;
                        }
                        break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Test whether two collections are equivalent sets - that is,
        /// whether they contain the same elements in any order.
        /// </summary>
        /// <param name="collection1">The first set</param>
        /// <param name="collection2">The second set</param>
        /// <returns>True if they are equivalent, otherwise false</returns>
        protected bool CollectionsEquivalent(ICollection collection1, ICollection collection2)
        {
            bool found = false;

            foreach (object collection1Obj in collection1)
            {
                found = false;
                foreach (object collection2Obj in collection2)
                {
                    if (MembersEqual( collection1Obj, collection2Obj ) )
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    FailureMessage.WriteLine("\tAn item from collection1 was not found in collection2.");
                    return false;
                }
            }

            foreach (object collection2Obj in collection2)
            {
                found = false;
                foreach (object collection1Obj in collection1)
                {
                    if ( MembersEqual( collection1Obj, collection2Obj ) )
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    FailureMessage.WriteLine("\tAn item from collection2 was not found in collection1.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Test whether one collection is a subset of another
        /// </summary>
        /// <param name="collection1">The first collection</param>
        /// <param name="collection2">The second collection</param>
        /// <returns>True if the second collection is a subset of the first, otherwise false</returns>
        protected bool IsSubsetOf(ICollection collection1, ICollection collection2)
        {
            foreach (object collection2Obj in collection2)
            {
                bool found = false;

                foreach (object collection1Obj in collection1)
                {
                    if ( MembersEqual( collection1Obj, collection2Obj) )
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

		/// <summary>
		/// Helper method to safely compare members
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <returns></returns>
		private bool MembersEqual( object expected, object actual )
		{
			if ( comparer != null )
				return comparer.Compare(expected, actual).Equals(0);

			if ( expected == null && actual == null )
				return true;

			if ( expected == null || actual == null )
				return false;

			return expected.Equals(actual);
		}
		#endregion
    }
	#endregion

	#region ItemsOfTypeAsserter
	/// <summary>
	/// Class to assert that all items in a collection are of a specified type
	/// </summary>
	public class ItemsOfTypeAsserter : CollectionAsserter
	{
        private Type expectedType;

		/// <summary>
		/// Construct an ItemsOfTypeAsserter
		/// </summary>
		/// <param name="collection1">The collection to be examined</param>
		/// <param name="expectedType"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public ItemsOfTypeAsserter( ICollection collection1, Type expectedType, string message, params object[] args ) 
            : base(collection1, message, args)
		{
            this.expectedType = expectedType;
		}

		/// <summary>
		/// Test whether all items in the collection are of the specified type
		/// </summary>
		/// <returns>True if all items are of the specifed type</returns>
		public override bool Test()
		{
			foreach(object loopObj in collection1)
			{
				if (!loopObj.GetType().Equals(expectedType))
				{
					CreateMessage();
					return false;
				}
			}
			return true;
		}

		private void CreateMessage()
		{
			FailureMessage.WriteLine("\tAll objects are not of actual type.");
			FailureMessage.WriteLine("\t{0} {1}","collection1.Count:",collection1.Count.ToString());
			FailureMessage.WriteLine("\t{0} {1}","actual:",this.expectedType.Name);
		}
	}

	#endregion

	#region ItemsNotNullAsserter
    /// <summary>
    /// The ItemsNotNullAsserter class tests a collection to determine that it has no null items.
    /// </summary>
	public class ItemsNotNullAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemsNotNullAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The set to be tested</param>
        /// <param name="message">A message to issue in case of failure</param>
        /// <param name="args">Parameters used in formatting the message</param>
		public ItemsNotNullAsserter( ICollection collection1, string message, params object[] args ) : base(collection1, message, args)
		{
		}

        /// <summary>
        /// Test that the collection contains no null items
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			foreach(object loopObj in collection1)
			{
				if (loopObj == null)
				{
					CreateMessage();
					return false;
				}
			}
			return true;
		}

		private void CreateMessage()
		{
			FailureMessage.WriteLine("\tAt least one object is null.");
			FailureMessage.WriteLine("\t{0} {1}","collection1.Count:",collection1.Count.ToString());
		}
	}

	#endregion

	#region ItemsUniqueAsserter
    /// <summary>
    /// The ItemsUniqueAsserter tests a collection to verify that all items are unique
    /// </summary>
	public class ItemsUniqueAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemsUniqueAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The set to be tested</param>
        /// <param name="message">A message to issue in case of failure</param>
        /// <param name="args">Parameters used in formatting the message</param>
		public ItemsUniqueAsserter( ICollection collection1, string message, params object[] args ) : base(collection1, message, args)
		{
		}

        /// <summary>
        /// Tests the collection to determine whether all contained items are unique.
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			foreach(object loopObj in collection1)
			{
				bool foundOnce = false;
				foreach(object innerObj in collection1)
				{
					if (loopObj.Equals(innerObj))
					{
						if (foundOnce)
						{
							CreateMessage();
							return false;
						}
						else
						{
							foundOnce = true;
						}
					}
				}
			}
			return true;
		}
		private void CreateMessage()
		{
			FailureMessage.WriteLine("\tAt least one object is not unique within collection1.");
			FailureMessage.WriteLine("\t{0} {1}","collection1.Count:",collection1.Count.ToString());
		}
	}

	#endregion

	#region CollectionContains
	/// <summary>
	/// The CollectionContains Asserter is used to test whether a collection
	/// contains a given object.
	/// </summary>
	public class CollectionContains : CollectionAsserter
	{
        private object actual;

		/// <summary>
		/// Construct a CollectionContains object
		/// </summary>
		/// <param name="collection1">The collection</param>
		/// <param name="actual">The object that is being examined</param>
		/// <param name="message">Error message to display in case of failure</param>
		/// <param name="args">Arguments used in formatting the message</param>
		public CollectionContains( ICollection collection1, object actual, string message, params object[] args ) 
			: base(collection1, message, args) 
        {
            this.actual = actual;
        }

		/// <summary>
		/// Test whether the object is contained in the collection
		/// </summary>
		/// <returns>True on success, false otherwise</returns>
		public override bool Test()
		{
			foreach(object loopObj in collection1)
			{
				if (loopObj.Equals(actual))
					return true;
			}

			CreateMessage();
			return false;
		}

		private void CreateMessage()
		{
			FailureMessage.WriteLine("\tThe actual object was not found in collection1.");
			FailureMessage.WriteLine("\t{0} {1}","collection1.Count:",collection1.Count.ToString());
			FailureMessage.WriteActualLine(actual.ToString());
		}
	}

	#endregion

	#region CollectionNotContains
    /// <summary>
    /// The CollectionNotContains asserter is used to verify the absence of an object from a collection.
    /// </summary>
	public class CollectionNotContains : CollectionAsserter
	{
        private object actual;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CollectionNotContains"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public CollectionNotContains( ICollection collection1, object actual, string message, params object[] args ) 
            : base(collection1, message, args)
		{
            this.actual = actual;
		}

        /// <summary>
        /// Tests that the collection does not contain the specified object.
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			foreach(object loopObj in collection1)
			{
				if (loopObj.Equals(actual))
				{
					CreateMessage();
					return false;
				}
			}
			return true;
		}
		private void CreateMessage()
		{
			FailureMessage.WriteLine("\tThe actual object was found in collection1.");
			FailureMessage.WriteLine("\t{0} {1}","collection1.Count:",collection1.Count.ToString());
			FailureMessage.WriteActualLine(actual.ToString());
		}
	}

	#endregion

	#region CollectionEqualAsserter
    /// <summary>
    /// CollectionsEqualAsserter verifies that two collections are identical
    /// </summary>
	public class CollectionEqualAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CollectionEqualAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public CollectionEqualAsserter( ICollection collection1, ICollection collection2, IComparer comparer, string message, params object[] args ) : base(collection1, collection2, comparer, message, args)
		{
		}

        /// <summary>
        /// Tests that the two collections are equal.
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if ( CollectionsEqual( collection1, collection2 ) )
			{
				return true;
			}
			else
			{
				CreateMessage();
				return false;
			}
		}


		private void CreateMessage()
		{
		}
	}

	#endregion

	#region CollectionNotEqualAsserter
    /// <summary>
    /// CollectionNotEqualAsserter tests that two collections are not equal
    /// </summary>
	public class CollectionNotEqualAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CollectionNotEqualAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public CollectionNotEqualAsserter( ICollection collection1, ICollection collection2, IComparer comparer, string message, params object[] args ) : base(collection1, collection2, comparer, message, args)
		{
		}

        /// <summary>
        /// Tests that the two collections are not equal
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if ( CollectionsEqual( collection1, collection2 ) )
			{
				CreateMessage();
				return false;
			}
			else
			{
				return true;
			}
		}


		private void CreateMessage()
		{
		}
	}

	#endregion

	#region CollectionEquivalentAsserter
    /// <summary>
    /// CollectionEquivalentAsserter tests whether one collection contains the same
    /// members as another, without respect to ordering.
    /// </summary>
	public class CollectionEquivalentAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CollectionEquivalentAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public CollectionEquivalentAsserter( ICollection collection1, ICollection collection2, string message, params object[] args ) : base(collection1, collection2, message, args)
		{
		}

        /// <summary>
        /// Test whether the collections are equivalent
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if ( CollectionsEquivalent( collection1, collection2 ) )
			{
				return true;
			}
			else
			{
				CreateMessage();
				return false;
			}
		}

		private void CreateMessage()
		{
		}
	}

	#endregion

	#region CollectionNotEquivalentAsserter
    /// <summary>
    /// CollectionNotEquivalentAsserter tests whether two collections are not equivalent.
    /// </summary>
	public class CollectionNotEquivalentAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CollectionNotEquivalentAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public CollectionNotEquivalentAsserter( ICollection collection1, ICollection collection2, string message, params object[] args ) : base(collection1, collection2, message, args)
		{
		}

        /// <summary>
        /// Test whether the collections are non-equivalent.
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if ( !CollectionsEquivalent( collection1, collection2 ) )
			{
				return true;
			}
			else
			{
				CreateMessage();
				return false;
			}
		}

		private void CreateMessage()
		{
		}
	}

	#endregion

	#region SubsetAsserter
    /// <summary>
    /// SubsetAaserter tests whether one collection is a subset of another
    /// </summary>
	public class SubsetAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SubsetAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public SubsetAsserter( ICollection collection1, ICollection collection2, string message, params object[] args ) : base(collection1, collection2, message, args)
		{
		}

        /// <summary>
        /// Test whether one collection is a subset of another
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if ( IsSubsetOf( collection1, collection2 ) )
			{
				return true;
			}
			else
			{
				CreateMessage();
				return false;
			}
		}

		private void CreateMessage()
		{
		}
	}

	#endregion

	#region NotSubsetAsserter
    /// <summary>
    /// NotSubsetAsserter tests whether one collection is not a suset of another.
    /// </summary>
	public class NotSubsetAsserter : CollectionAsserter
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NotSubsetAsserter"/> class.
        /// </summary>
        /// <param name="collection1">The collection1.</param>
        /// <param name="collection2">The collection2.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
		public NotSubsetAsserter( ICollection collection1, ICollection collection2, string message, params object[] args ) : base(collection1, collection2, message, args)
		{
		}

        /// <summary>
        /// Test that one collection is not a subset of another
        /// </summary>
        /// <returns>True if the test succeeds</returns>
		public override bool Test()
		{
			if (!IsSubsetOf( collection1, collection2 ) )
			{
				return true;
			}
			else
			{
				CreateMessage();
				return false;
			}
		}

		private void CreateMessage()
		{
		}
	}

	#endregion

    #region CollectionEmptyAsserter
    /// <summary>
    /// Class to Assert that a collection is empty
    /// </summary>
    public class CollectionEmptyAsserter : CollectionAsserter
    {
        /// <summary>
        /// Construct an EmptyAsserter for a collection
        /// </summary>
        /// <param name="collection">The collection to be tested</param>
        /// <param name="message">The message to display if the collection is not empty</param>
        /// <param name="args">Arguements to use in formatting the message</param>
        public CollectionEmptyAsserter(ICollection collection, string message, params object[] args)
            : base(collection, message, args) { }

        /// <summary>
        /// Test whether the collection is empty.
        /// </summary>
        /// <returns>True if the test succeeds</returns>
        public override bool Test()
        {
            if (collection1.Count == 0)
                return true;

            FailureMessage.WriteExpectedLine("An empty collection");
            FailureMessage.WriteActualLine(string.Format("A collection containing {0} items", collection1.Count));
            return false;
        }
    }
    #endregion

    #region CollectionNotEmptyAsserter
    /// <summary>
    /// Class to Assert that a collection is empty
    /// </summary>
    public class CollectionNotEmptyAsserter : CollectionAsserter
    {
        /// <summary>
        /// Construct an EmptyAsserter for a collection
        /// </summary>
        /// <param name="collection">The collection to be tested</param>
        /// <param name="message">The message to display if the collection is not empty</param>
        /// <param name="args">Arguements to use in formatting the message</param>
        public CollectionNotEmptyAsserter(ICollection collection, string message, params object[] args)
            : base(collection, message, args) { }

        /// <summary>
        /// Test whether the collections are non-equivalent
        /// </summary>
        /// <returns>True if the test succeeds</returns>
        public override bool Test()
        {
            if (collection1.Count > 0)
                return true;

            FailureMessage.WriteExpectedLine("A non-empty collection");
            FailureMessage.WriteActualLine("An empty collection");
            return false;
        }
    }
    #endregion

    #endregion

	/// <summary>
	/// A set of Assert methods operationg on one or more collections
	/// </summary>
	public class CollectionAssert
	{
		#region Equals and ReferenceEquals

		/// <summary>
		/// The Equals method throws an AssertionException. This is done 
		/// to make sure there is no mistake by calling this function.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new bool Equals(object a, object b)
		{
			throw new AssertionException("Assert.Equals should not be used for Assertions");
		}

		/// <summary>
		/// override the default ReferenceEquals to throw an AssertionException. This 
		/// implementation makes sure there is no mistake in calling this function 
		/// as part of Assert. 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static new void ReferenceEquals(object a, object b)
		{
			throw new AssertionException("Assert.ReferenceEquals should not be used for Assertions");
		}

		#endregion
				
		#region AllItemsAreInstancesOfType
		/// <summary>
		/// Asserts that all items contained in collection are of the type specified by expectedType.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="expectedType">System.Type that all objects in collection must be instances of</param>
		public static void AllItemsAreInstancesOfType (ICollection collection, Type expectedType)
		{
			AllItemsAreInstancesOfType(collection, expectedType, string.Empty, null);
		}

		/// <summary>
		/// Asserts that all items contained in collection are of the type specified by expectedType.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="expectedType">System.Type that all objects in collection must be instances of</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AllItemsAreInstancesOfType (ICollection collection, Type expectedType, string message)
		{
			AllItemsAreInstancesOfType(collection, expectedType, message, null);
		}

		/// <summary>
		/// Asserts that all items contained in collection are of the type specified by expectedType.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="expectedType">System.Type that all objects in collection must be instances of</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AllItemsAreInstancesOfType (ICollection collection, Type expectedType, string message, params object[] args)
		{
			Assert.DoAssert(new ItemsOfTypeAsserter(collection, expectedType, message, args));
		}
		#endregion

		#region AllItemsAreNotNull

		/// <summary>
		/// Asserts that all items contained in collection are not equal to null.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		public static void AllItemsAreNotNull (ICollection collection) 
		{
			AllItemsAreNotNull(collection, string.Empty, null);
		}

		/// <summary>
		/// Asserts that all items contained in collection are not equal to null.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AllItemsAreNotNull (ICollection collection, string message) 
		{
			AllItemsAreNotNull(collection, message, null);
		}

		/// <summary>
		/// Asserts that all items contained in collection are not equal to null.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AllItemsAreNotNull (ICollection collection, string message, params object[] args) 
		{
			Assert.DoAssert(new ItemsNotNullAsserter(collection, message, args));
		}
		#endregion

		#region AllItemsAreUnique

		/// <summary>
		/// Ensures that every object contained in collection exists within the collection
		/// once and only once.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		public static void AllItemsAreUnique (ICollection collection) 
		{
			AllItemsAreUnique(collection, string.Empty, null);
		}

		/// <summary>
		/// Ensures that every object contained in collection exists within the collection
		/// once and only once.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AllItemsAreUnique (ICollection collection, string message) 
		{
			AllItemsAreUnique(collection, message, null);
		}
		
		/// <summary>
		/// Ensures that every object contained in collection exists within the collection
		/// once and only once.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AllItemsAreUnique (ICollection collection, string message, params object[] args) 
		{
			Assert.DoAssert(new ItemsUniqueAsserter(collection, message, args));
		}
		#endregion

		#region AreEqual

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		public static void AreEqual (ICollection expected, ICollection actual) 
		{
			AreEqual(expected, actual, null, string.Empty, null);
		}

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		public static void AreEqual (ICollection expected, ICollection actual, IComparer comparer) 
		{
			AreEqual(expected, actual, comparer, string.Empty, null);
		}

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreEqual (ICollection expected, ICollection actual, string message) 
		{
			AreEqual(expected, actual, null, message, null);
		}

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreEqual (ICollection expected, ICollection actual, IComparer comparer, string message) 
		{
			AreEqual(expected, actual, comparer, message, null);
		}

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreEqual (ICollection expected, ICollection actual, string message, params object[] args) 
		{
			AreEqual(expected, actual, null, message, args);
		}

		/// <summary>
		/// Asserts that expected and actual are exactly equal.  The collections must have the same count, 
		/// and contain the exact same objects in the same order.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreEqual (ICollection expected, ICollection actual, IComparer comparer, string message, params object[] args) 
		{
			Assert.DoAssert(new CollectionEqualAsserter(expected, actual, comparer, message, args));
		}
		#endregion

		#region AreEquivalent

		/// <summary>
		/// Asserts that expected and actual are equivalent, containing the same objects but the match may be in any order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		public static void AreEquivalent (ICollection expected, ICollection actual) 
		{
			AreEquivalent(expected, actual, string.Empty, null);
		}

		/// <summary>
		/// Asserts that expected and actual are equivalent, containing the same objects but the match may be in any order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreEquivalent (ICollection expected, ICollection actual, string message) 
		{
			AreEquivalent(expected, actual, message, null);
		}

		/// <summary>
		/// Asserts that expected and actual are equivalent, containing the same objects but the match may be in any order.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreEquivalent (ICollection expected, ICollection actual, string message, params object[] args) 
		{
			Assert.DoAssert(new CollectionEquivalentAsserter(expected, actual, message, args));
		}
		#endregion

		#region AreNotEqual

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		public static void AreNotEqual (ICollection expected, ICollection actual)
		{
			//AreNotEqual(expected, actual, null, string.Empty, null);
			Assert.AreNotEqual( expected, actual );
		}

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		public static void AreNotEqual (ICollection expected, ICollection actual, IComparer comparer)
		{
			AreNotEqual(expected, actual, comparer, string.Empty, null);
		}

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreNotEqual (ICollection expected, ICollection actual, string message)
		{
			//AreNotEqual(expected, actual, null, message, null);
			Assert.AreNotEqual( expected, actual, message );
		}

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreNotEqual (ICollection expected, ICollection actual, IComparer comparer, string message)
		{
			AreNotEqual(expected, actual, comparer, message, null);
		}

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreNotEqual (ICollection expected, ICollection actual, string message, params object[] args) 
		{
			//AreNotEqual(expected, actual, null, message, args);
			Assert.AreNotEqual( expected, actual, message, args );
		}

		/// <summary>
		/// Asserts that expected and actual are not exactly equal.
		/// If comparer is not null then it will be used to compare the objects.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="comparer">The IComparer to use in comparing objects from each ICollection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreNotEqual (ICollection expected, ICollection actual, IComparer comparer, string message, params object[] args)
		{
			Assert.DoAssert(new CollectionNotEqualAsserter(expected, actual, comparer, message, args));
		}
		#endregion

		#region AreNotEquivalent

		/// <summary>
		/// Asserts that expected and actual are not equivalent.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		public static void AreNotEquivalent (ICollection expected, ICollection actual)
		{
			AreNotEquivalent(expected, actual, string.Empty, null);
		}

		/// <summary>
		/// Asserts that expected and actual are not equivalent.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void AreNotEquivalent (ICollection expected, ICollection actual, string message)
		{
			AreNotEquivalent(expected, actual, message, null);
		}

		/// <summary>
		/// Asserts that expected and actual are not equivalent.
		/// </summary>
		/// <param name="expected">The first ICollection of objects to be considered</param>
		/// <param name="actual">The second ICollection of objects to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void AreNotEquivalent (ICollection expected, ICollection actual, string message, params object[] args)
		{
			Assert.DoAssert(new CollectionNotEquivalentAsserter(expected, actual, message, args));
		}
		#endregion

		#region Contains

		/// <summary>
		/// Asserts that collection contains actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object to be found within collection</param>
		public static void Contains (ICollection collection, Object actual)
		{
			Contains(collection, actual, string.Empty, null);
		}

		/// <summary>
		/// Asserts that collection contains actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object to be found within collection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void Contains (ICollection collection, Object actual, string message)
		{
			Contains(collection, actual, message, null);
		}

		/// <summary>
		/// Asserts that collection contains actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object to be found within collection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void Contains (ICollection collection, Object actual, string message, params object[] args)
		{
			Assert.DoAssert(new CollectionContains(collection, actual, message, args));
		}
		#endregion

		#region DoesNotContain

		/// <summary>
		/// Asserts that collection does not contain actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object that cannot exist within collection</param>
		public static void DoesNotContain (ICollection collection, Object actual)
		{
			DoesNotContain(collection, actual, string.Empty, null);
		}

		/// <summary>
		/// Asserts that collection does not contain actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object that cannot exist within collection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void DoesNotContain (ICollection collection, Object actual, string message)
		{
			DoesNotContain(collection, actual, message, null);
		}

		/// <summary>
		/// Asserts that collection does not contain actual as an item.
		/// </summary>
		/// <param name="collection">ICollection of objects to be considered</param>
		/// <param name="actual">Object that cannot exist within collection</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void DoesNotContain (ICollection collection, Object actual, string message, params object[] args)
		{
			Assert.DoAssert(new CollectionNotContains(collection, actual, message, args));
		}
		#endregion

		#region IsNotSubsetOf

		/// <summary>
		/// Asserts that superset is not a subject of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		public static void IsNotSubsetOf (ICollection subset, ICollection superset)
		{
			IsNotSubsetOf(subset, superset, string.Empty, null);
		}

		/// <summary>
		/// Asserts that superset is not a subject of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void IsNotSubsetOf (ICollection subset, ICollection superset, string message)
		{
			IsNotSubsetOf(subset, superset, message, null);
		}

		/// <summary>
		/// Asserts that superset is not a subject of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void IsNotSubsetOf (ICollection subset, ICollection superset, string message, params object[] args)
		{
			Assert.DoAssert(new NotSubsetAsserter(subset, superset, message, args));
		}
		#endregion

		#region IsSubsetOf

		/// <summary>
		/// Asserts that superset is a subset of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		public static void IsSubsetOf (ICollection subset, ICollection superset)
		{
			IsSubsetOf(subset, superset, string.Empty, null);
		}

		/// <summary>
		/// Asserts that superset is a subset of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		public static void IsSubsetOf (ICollection subset, ICollection superset, string message)
		{
			IsSubsetOf(subset, superset, message, null);
		}

		/// <summary>
		/// Asserts that superset is a subset of subset.
		/// </summary>
		/// <param name="subset">The ICollection superset to be considered</param>
		/// <param name="superset">The ICollection subset to be considered</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public static void IsSubsetOf (ICollection subset, ICollection superset, string message, params object[] args)
		{
			Assert.DoAssert(new SubsetAsserter(subset, superset, message, args));
		}
		#endregion

        #region IsEmpty
        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsEmpty(ICollection collection, string message, params object[] args)
        {
            Assert.DoAssert(new CollectionEmptyAsserter(collection, message, args));
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to be displayed on failure</param>
        public static void IsEmpty(ICollection collection, string message)
        {
            IsEmpty(collection, message, null);
        }

        /// <summary>
        /// Assert that an array,list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        public static void IsEmpty(ICollection collection)
        {
            IsEmpty(collection, string.Empty, null);
        }
        #endregion

        #region IsNotEmpty
        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsNotEmpty(ICollection collection, string message, params object[] args)
        {
            Assert.DoAssert(new CollectionNotEmptyAsserter(collection, message, args));
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to be displayed on failure</param>
        public static void IsNotEmpty(ICollection collection, string message)
        {
            IsNotEmpty(collection, message, null);
        }

        /// <summary>
        /// Assert that an array,list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        public static void IsNotEmpty(ICollection collection)
        {
            IsNotEmpty(collection, string.Empty, null);
        }
        #endregion
    }
}



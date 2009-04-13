// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;
using System.Text;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    #region CollectionConstraint
    /// <summary>
    /// CollectionConstraint is the abstract base class for
    /// constraints that operate on collections.
    /// </summary>
    public abstract class CollectionConstraint : Constraint
    {
        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        public CollectionConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        public CollectionConstraint(object arg) : base(arg) { }

        /// <summary>
        /// Determines whether the specified enumerable is empty.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// 	<c>true</c> if the specified enumerable is empty; otherwise, <c>false</c>.
        /// </returns>
		protected static bool IsEmpty( IEnumerable enumerable )
		{
			ICollection collection = enumerable as ICollection;
			if ( collection != null )
				return collection.Count == 0;
			else
				return !enumerable.GetEnumerator().MoveNext();
		}

		/// <summary>
		/// CollectionTally counts (tallies) the number of
		/// occurences of each object in one or more enuerations.
		/// </summary>
        protected internal class CollectionTally
        {
            // Internal list used to track occurences
            private ArrayList list = new ArrayList();

            /// <summary>
            /// Construct a CollectionTally object from a collection
            /// </summary>
            /// <param name="c"></param>
            public CollectionTally(IEnumerable c)
            {
                foreach (object o in c)
                    list.Add(o);
            }

            public int Count
            {
                get { return list.Count; }
            }

            private bool ObjectsEqual(object expected, object actual)
            {
                if (expected == null && actual == null)
                    return true;

                if (expected == null || actual == null)
                    return false;

                Type expectedType = expected.GetType();
                Type actualType = actual.GetType();

                if (expectedType.IsArray && actualType.IsArray)
                    return ArraysEqual((Array)expected, (Array)actual);

                if (expected is ICollection && actual is ICollection)
                    return CollectionsEqual((ICollection)expected, (ICollection)actual);

                // String must precede IEnumerable since it implements it
                if (expected is string && actual is string)
                    expected.Equals(actual);

                if (expected is IEnumerable && actual is IEnumerable)
                    return EnumerablesEqual((IEnumerable)expected, (IEnumerable)actual);

                return expected.Equals(actual);
            }

            /// <summary>
            /// Helper method to compare two arrays
            /// </summary>
            private bool ArraysEqual(Array expected, Array actual)
            {
                int rank = expected.Rank;

                if (rank != actual.Rank)
                    return false;

                for (int r = 1; r < rank; r++)
                    if (expected.GetLength(r) != actual.GetLength(r))
                        return false;

                return CollectionsEqual((ICollection)expected, (ICollection)actual);
            }

            private bool CollectionsEqual(ICollection expected, ICollection actual)
            {
                IEnumerator expectedEnum = expected.GetEnumerator();
                IEnumerator actualEnum = actual.GetEnumerator();

                int count;
                for (count = 0; expectedEnum.MoveNext() && actualEnum.MoveNext(); count++)
                {
                    if (!ObjectsEqual(expectedEnum.Current, actualEnum.Current))
                        break;
                }

                if (count == expected.Count && count == actual.Count)
                    return true;

                return false;
            }

            private bool EnumerablesEqual(IEnumerable expected, IEnumerable actual)
            {
                IEnumerator expectedEnum = expected.GetEnumerator();
                IEnumerator actualEnum = actual.GetEnumerator();

                int count = 0;
                for (; ; )
                {
                    bool expectedHasData = expectedEnum.MoveNext();
                    bool actualHasData = actualEnum.MoveNext();

                    if (!expectedHasData && !actualHasData)
                        return true;

                    if (expectedHasData != actualHasData ||
                        !ObjectsEqual(expectedEnum.Current, actualEnum.Current))
                    {
                        return false;
                    }
                }
            }

            public bool TryRemove(object o)
            {
                for (int index = 0; index < list.Count; index++)
                    if (ObjectsEqual(list[index], o))
                    {
                        list.RemoveAt(index);
                        return true;
                    }

                return false;
            }

            public bool TryRemove(IEnumerable c)
            {
                foreach (object o in c)
                    if ( !TryRemove( o ) )
                        return false;

                return true;
            }
        }

		/// <summary>
		/// Test whether the constraint is satisfied by a given value
		/// </summary>
		/// <param name="actual">The value to be tested</param>
		/// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			IEnumerable enumerable = actual as IEnumerable;
			if ( enumerable == null )
				throw new ArgumentException( "The actual value must be an IEnumerable", "actual" );
		
			return doMatch( enumerable );
		}

		/// <summary>
		/// Protected method to be implemented by derived classes
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		protected abstract bool doMatch(IEnumerable collection);
    }
    #endregion

	#region EmptyCollectionConstraint
    /// <summary>
    /// EmptyCollectionConstraint tests whether a collection is empty. 
    /// </summary>
    public class EmptyCollectionConstraint : CollectionConstraint
	{
		/// <summary>
		/// Check that the collection is empty
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		protected override bool doMatch(IEnumerable collection)
		{
			return IsEmpty( collection );
		}
	
		/// <summary>
		/// Write the constraint description to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.Write( "<empty>" );
		}
	}
	#endregion

	#region UniqueItemsConstraint
    /// <summary>
    /// UniqueItemsConstraint tests whether all the items in a 
    /// collection are unique.
    /// </summary>
    public class UniqueItemsConstraint : CollectionConstraint
    {
        /// <summary>
        /// Check that all items are unique.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
            ArrayList list = new ArrayList();

            foreach (object o in actual)
            {
                if (list.Contains(o))
                    return false;
                list.Add(o);
            }

            return true;
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("all items unique");
        }
    }
    #endregion

    #region SameTypeConstraint
    /// <summary>
    /// SameTypeConstraint is used to test whether the contents
    /// of a collection are all the same type.
    /// </summary>
    public class AllSameTypeConstraint : CollectionConstraint
    {
        /// <summary>
        /// Test whether the all items are the same type
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
            Type type = null;
            foreach (object obj in actual)
            {
                if (type == null)
                    type = obj.GetType();
                else
                if (type != obj.GetType())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Write a descripton of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("all items of same type");
        }
    }
    #endregion
    
    #region CollectionContainsConstraint
    /// <summary>
    /// CollectionContainsConstraint is used to test whether a collection
    /// contains an expected object as a member.
    /// </summary>
    public class CollectionContainsConstraint : CollectionConstraint
    {
        private object expected;

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public CollectionContainsConstraint(object expected)
            : base(expected)
        {
            this.expected = expected;
            this.DisplayName = "contains";
        }

        /// <summary>
        /// Test whether the expected item is contained in the collection
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
            foreach (object obj in actual)
                if (Object.Equals(obj, expected))
                    return true;

            return false;
        }

        /// <summary>
        /// Write a descripton of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("collection containing");
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion

    #region CollectionEquivalentConstraint
    /// <summary>
    /// CollectionEquivalentCOnstraint is used to determine whether two
    /// collections are equivalent.
    /// </summary>
    public class CollectionEquivalentConstraint : CollectionConstraint
    {
        private IEnumerable expected;

        /// <summary>
        /// Construct a CollectionEquivalentConstraint
        /// </summary>
        /// <param name="expected"></param>
        public CollectionEquivalentConstraint(IEnumerable expected) : base(expected)
        {
            this.expected = expected;
            this.DisplayName = "equivalent";
        }

        /// <summary>
        /// Test whether two collections are equivalent
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
			// This is just an optimization
			if( expected is ICollection && actual is ICollection )
				if( ((ICollection)actual).Count != ((ICollection)expected).Count )
					return false;

            CollectionTally tally = new CollectionTally(expected);
            return tally.TryRemove(actual) && tally.Count == 0;
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("equivalent to");
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion

    #region CollectionSubsetConstraint
    /// <summary>
    /// CollectionSubsetConstraint is used to determine whether
    /// one collection is a subset of another
    /// </summary>
    public class CollectionSubsetConstraint : CollectionConstraint
    {
        private IEnumerable expected;

        /// <summary>
        /// Construct a CollectionSubsetConstraint
        /// </summary>
        /// <param name="expected">The collection that the actual value is expected to be a subset of</param>
        public CollectionSubsetConstraint(IEnumerable expected) : base(expected)
        {
            this.expected = expected;
            this.DisplayName = "subsetof";
        }

        /// <summary>
        /// Test whether the actual collection is a subset of 
        /// the expected collection provided.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
            return new CollectionTally(expected).TryRemove( actual );
        }
        
        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate( "subset of" );
            writer.WriteExpectedValue(expected);
        }
    }
    #endregion

    #region CollectionOrderedConstraint

    /// <summary>
    /// CollectionOrderedConstraint is used to test whether a collection is ordered.
    /// </summary>
    public class CollectionOrderedConstraint : CollectionConstraint
    {
        private ComparisonAdapter comparer = ComparisonAdapter.Default;
        private string comparerName;
        private string propertyName;
        private bool descending;

        /// <summary>
        /// Construct a CollectionOrderedConstraint
        /// </summary>
        public CollectionOrderedConstraint() 
        {
            this.DisplayName = "ordered";
        }

        ///<summary>
        /// If used performs a reverse comparison
        ///</summary>
        public CollectionOrderedConstraint Descending
        {
            get
            {
                descending = true;
                return this;
            }
        }

        public CollectionOrderedConstraint Using(IComparer comparer)
        {
            this.comparer = ComparisonAdapter.For( comparer );
            this.comparerName = comparer.GetType().FullName;
            return this;
        }

#if NET_2_0
        public CollectionOrderedConstraint Using<T>(IComparer<T> comparer)
        {
            this.comparer = ComparisonAdapter.For(comparer);
            this.comparerName = comparer.GetType().FullName;
            return this;
        }

        public CollectionOrderedConstraint Using<T>(Comparison<T> comparer)
        {
            this.comparer = ComparisonAdapter.For(comparer);
            this.comparerName = comparer.GetType().FullName;
            return this;
        }
#endif

		public CollectionOrderedConstraint By(string propertyName)
		{
			this.propertyName = propertyName;
			return this;
		}

        /// <summary>
        /// Test whether the collection is ordered
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable actual)
        {
            object previous = null;
            int index = 0;
            foreach(object obj in actual)
            {
                object objToCompare = obj;
                if (obj == null)
                    throw new ArgumentNullException("actual", "Null value at index " + index.ToString());

                if (this.propertyName != null)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(propertyName);
                    objToCompare = prop.GetValue(obj, null);
                    if (objToCompare == null)
                        throw new ArgumentNullException("actual", "Null property value at index " + index.ToString());
                }

                if (previous != null)
                {
                    //int comparisonResult = comparer.Compare(al[i], al[i + 1]);
                    int comparisonResult = comparer.Compare(previous, objToCompare);

                    if (descending && comparisonResult < 0)
                        return false;
                    if (!descending && comparisonResult > 0)
                        return false;
                }

                previous = objToCompare;
                index++;
            }

            return true;
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (propertyName == null)
                writer.Write("collection ordered");
            else
            {
                writer.WritePredicate("collection ordered by");
                writer.WriteExpectedValue(propertyName);
            }

            if (descending)
                writer.WriteModifier("descending");
        }

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("<ordered");

            if (propertyName != null)
                sb.AppendFormat("by {0}", propertyName);
            if (descending)
                sb.AppendFormat(" descending");
            if (comparerName != null)
                sb.AppendFormat(" {0}", comparerName);

            sb.Append(">");

            return sb.ToString();
        }
    }
    #endregion
}

// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System.Collections;

namespace NUnit.Framework.Constraints
{
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
            return IsEmpty(collection);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("<empty>");
        }
    }
}

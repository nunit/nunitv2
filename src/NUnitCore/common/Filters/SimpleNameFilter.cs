using System;
using System.Collections;
using System.Text;

namespace NUnit.Core.Filters
{
    [Serializable]
    public class SimpleNameFilter : RecursiveTestFilter
    {
        private ArrayList names = new ArrayList();

		/// <summary>
		/// Construct an empty SimpleNameFilter
		/// </summary>
        public SimpleNameFilter() { }

        /// <summary>
        /// Construct a SimpleNameFilter for a single name
        /// </summary>
        /// <param name="name"></param>
		public SimpleNameFilter( string name )
        {
            this.names.Add( name );
        }

		/// <summary>
		/// Add a name to a SimpleNameFilter
		/// </summary>
		/// <param name="testName"></param>
		public void Add( string name )
		{
			names.Add( name );
		}

		/// <summary>
		/// Test the filter on a given test node
		/// </summary>
		/// <param name="test"></param>
		/// <returns></returns>
        //public override bool Pass( ITest test )
        //{
        //    if ( Match( test ) )
        //        return true;

        //    if ( MatchParent( test ) )
        //        return true;

        //    if ( MatchDescendant( test ) )
        //        return true;

        //    return false;
        //}

		public override bool Match( ITest test )
		{
			foreach( string name in names )
				if ( test.FullName == name )
					return true;

			return false;
		}

        //private bool MatchParent( ITest test )
        //{
        //    if ( test.IsExplicit )
        //        return false;

        //    for( ITest parent = test.Parent; parent != null; parent = parent.Parent )
        //    {
        //        if ( Match( parent ) )
        //            return true;

        //        // Don't proceed past a parent marked Explicit
        //        if ( parent.IsExplicit )
        //            return false;
        //    }

        //    return false;
        //}

        //private bool MatchDescendant( ITest test )
        //{
        //    if ( !test.IsSuite || test.Tests == null )
        //        return false;

        //    foreach( ITest child in test.Tests )
        //    {
        //        if ( Match( child ) )
        //            return true;

        //        if ( MatchDescendant( child ) )
        //            return true;
        //    }

        //    return false;
        //}
	}
}

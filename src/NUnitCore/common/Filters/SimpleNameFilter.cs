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

		public override bool Match( ITest test )
		{
			foreach( string name in names )
				if ( test.TestName.FullName == name )
					return true;

			return false;
		}
	}
}

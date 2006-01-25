using System;
using System.Collections;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// Summary description for AndFilter.
	/// </summary>
	public class AndFilter : ITestFilter
	{
		private ArrayList filters = new ArrayList();

		public AndFilter() { }

		public AndFilter( params ITestFilter[] filters )
		{
			this.filters.AddRange( filters );
		}

		// NOTE: Not all languages support use of params
		public AndFilter( ITestFilter f1, ITestFilter f2 )
		{
			this.filters.Add( f1 );
			this.filters.Add( f2 );
		}

		public AndFilter( ITestFilter f1, ITestFilter f2, ITestFilter f3 )
		{
			this.filters.Add( f1 );
			this.filters.Add( f2 );
			this.filters.Add( f3 );
		}

		public AndFilter( ITestFilter f1, ITestFilter f2, ITestFilter f3, ITestFilter f4 )
		{
			this.filters.Add( f1 );
			this.filters.Add( f2 );
			this.filters.Add( f3 );
			this.filters.Add( f4 );
		}

		public void Add( ITest test )
		{
			this.filters.Add( test );
		}

		public bool Pass( ITest test )
		{
			foreach( ITestFilter filter in filters )
				if ( !filter.Pass( test ) )
					return false;

			return true;
		}
	}
}

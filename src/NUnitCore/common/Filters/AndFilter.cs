using System;
using System.Collections;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// Combines multiple filters so that a test must pass all 
	/// of them in order to pass this filter.
	/// </summary>
	[Serializable]
	public class AndFilter : RecursiveTestFilter
	{
		private ArrayList filters = new ArrayList();

		public AndFilter() { }

		public AndFilter( params TestFilter[] filters )
		{
			this.filters.AddRange( filters );
		}

		// NOTE: Not all languages support use of params
		public AndFilter( TestFilter f1, TestFilter f2 )
		{
			this.filters.Add( f1 );
			this.filters.Add( f2 );
		}

		public AndFilter( TestFilter f1, TestFilter f2, TestFilter f3 )
		{
			this.filters.Add( f1 );
			this.filters.Add( f2 );
			this.filters.Add( f3 );
		}

		public AndFilter( TestFilter f1, TestFilter f2, TestFilter f3, TestFilter f4 )
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

		public override bool Match( ITest test )
		{
			foreach( TestFilter filter in filters )
				if ( !filter.Pass( test ) )
					return false;

			return true;
		}
	}
}

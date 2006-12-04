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

		/// <summary>
		/// Constructs an empty AndFilter
		/// </summary>
		public AndFilter() { }

		/// <summary>
		/// Constructs an AndFilter from an array of filters
		/// </summary>
		/// <param name="filters"></param>
		public AndFilter( params TestFilter[] filters )
		{
			this.filters.AddRange( filters );
		}

		/// <summary>
		/// Checks whether the AndFilter is matched by a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if all the component filters match, otherwise false</returns>
		public override bool Match( ITest test )
		{
			foreach( TestFilter filter in filters )
				if ( !filter.Match( test ) )
					return false;

			return true;
		}
	}
}

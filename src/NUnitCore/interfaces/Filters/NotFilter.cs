using System;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// NotFilter negates the operation of another filter
	/// </summary>
	[Serializable]
	public class NotFilter : RecursiveTestFilter
	{
		TestFilter baseFilter;

		/// <summary>
		/// Construct a not filter on another filter
		/// </summary>
		/// <param name="baseFilter">The filter to be negated</param>
		public NotFilter( TestFilter baseFilter)
		{
			this.baseFilter = baseFilter;
		}

		/// <summary>
		/// Gets the base filter
		/// </summary>
		public TestFilter BaseFilter
		{
			get { return baseFilter; }
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
			return test.RunState != RunState.Explicit && !baseFilter.Pass( test );
		}
 	}
}

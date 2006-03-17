using System;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// NotFilter negates the operation of another filter
	/// </summary>
	[Serializable]
	public class NotFilter : TestFilter
	{
		TestFilter baseFilter;

		public NotFilter( TestFilter baseFilter)
		{
			this.baseFilter = baseFilter;
		}

		public override bool Pass( ITest test )
		{
			return !test.IsExplicit && !baseFilter.Pass( test );
		}
 	}
}

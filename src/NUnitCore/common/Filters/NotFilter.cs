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

		public NotFilter( TestFilter baseFilter)
		{
			this.baseFilter = baseFilter;
		}

		public override bool Match( ITest test )
		{
			return !test.IsExplicit && !baseFilter.Pass( test );
		}
 	}
}

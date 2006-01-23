using System;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// NotFilter negates the operation of another filter
	/// </summary>
	public class NotFilter : ITestFilter
	{
		ITestFilter baseFilter;

		public NotFilter( ITestFilter baseFilter)
		{
			this.baseFilter = baseFilter;
		}

		public bool Pass(ITest test)
		{
			return !baseFilter.Pass( test );
		}
 	}
}

using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for EmptyFilter.
	/// </summary>
	public class EmptyFilter : IFilter
	{
		#region IFilter Members

		public bool Pass(TestSuite suite)
		{
			return true;
		}

		bool NUnit.Core.IFilter.Pass(TestCase test)
		{
			return true;
		}

		#endregion

		public static EmptyFilter Empty 
		{
			get { return new EmptyFilter(); }
		}
	}
}

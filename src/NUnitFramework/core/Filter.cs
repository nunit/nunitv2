using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for Filter.
	/// </summary>
	public interface IFilter
	{
		bool Pass(TestSuite suite);

		bool Pass(TestCase test); 
	}
}

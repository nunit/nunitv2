using System;

namespace NUnit.Core
{
	public class TestAssembly : TestSuite
	{
		private string name;

		public TestAssembly(string assembly) : base(assembly)
		{
			name = assembly;
		}
	}
}

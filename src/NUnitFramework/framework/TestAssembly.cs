using System;

namespace NUnit.Core
{
	/// <summary>
	/// Test Suite formed from an assembly. 
	/// Class name changed from TestAssembly
	/// to avoid conflict with namespace.
	/// </summary>
	public class AssemblyTestSuite : TestSuite
	{
		public AssemblyTestSuite( string assembly ) : this( assembly, 0 )
		{
		}

		public AssemblyTestSuite( string assembly, int key) : base( assembly, key )
		{
		}
	}
}

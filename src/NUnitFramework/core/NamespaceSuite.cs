using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestSuite containing all the test classes in a namespace
	/// 
	/// TODO: Add a fixture for holding setup and teardown
	/// </summary>
	public class NamespaceSuite : TestSuite
	{
		public NamespaceSuite( string name, int assemblyKey ) 
			: base( name, assemblyKey ) { }

		public NamespaceSuite( string parentSuiteName, string name, int assemblyKey ) 
			: base( parentSuiteName, name, assemblyKey ) { }

		public NamespaceSuite( Type type, int assemblyKey )
			: base( type, assemblyKey ) { }
	}
}

using System;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Class that can build a tree of automatic namespace
	/// suites from a group of fixtures.
	/// </summary>
	public class NamespaceTreeBuilder
	{
		#region Instance Variables

		/// <summary>
		/// Hashtable of all test suites we have created to represent namespaces.
		/// Used to locate namespace parent suites for fixtures.
		/// </summary>
		Hashtable namespaceSuites  = new Hashtable();

		/// <summary>
		/// The root of the test suite being created by this builder. This
		/// may be a simple TestSuite, an AssemblyTestSuite or a RootTestSuite
		/// encompassing multiple assemblies.
		/// </summary>
		TestSuite rootSuite;

		#endregion

		#region Constructor

		public NamespaceTreeBuilder( TestSuite rootSuite )
		{
			this.rootSuite = rootSuite;
		}

		#endregion

		#region Properties

		public TestSuite RootSuite
		{
			get { return rootSuite; }
		}

		#endregion

		#region Public Methods

		public void Add( IList fixtures )
		{
			foreach( TestSuite fixture in fixtures )
				Add( fixture );
		}

		public void Add( Test fixture )
		{
			string ns = fixture.FullName;
			int index = ns.LastIndexOf( '.' );
			ns = index > 0 ? ns.Substring( 0, index ) : string.Empty;
			TestSuite suite = BuildFromNameSpace( ns, fixture.AssemblyKey );

			suite.Add( fixture );
			//				this.rootSuite.Add( fixture );
		}

		#endregion

		#region Helper Method

		private TestSuite BuildFromNameSpace( string nameSpace, int assemblyKey )
		{
			if( nameSpace == null || nameSpace  == "" ) return rootSuite;
			TestSuite suite = (TestSuite)namespaceSuites[nameSpace];
			if(suite!=null) return suite;

			int index = nameSpace.LastIndexOf(".");
			//string prefix = string.Format( "[{0}]", assemblyKey );
			if( index == -1 )
			{
				suite = new NamespaceSuite( nameSpace, assemblyKey );
				if ( rootSuite == null )
					rootSuite = suite;
				else
					rootSuite.Add(suite);
				namespaceSuites[nameSpace]=suite;
			}
			else
			{
				string parentNameSpace = nameSpace.Substring( 0,index );
				TestSuite parent = BuildFromNameSpace( parentNameSpace, assemblyKey );
				string suiteName = nameSpace.Substring( index+1 );
				suite = new NamespaceSuite( parentNameSpace, suiteName, assemblyKey );
				parent.Add( suite );
				namespaceSuites[nameSpace] = suite;
			}

			return suite;
		}

		#endregion
	}
}

#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Collections;

	/// <summary>
	/// This is the master suite builder for NUnit. It builds a test suite from
	/// one or more assemblies using a list of internal and external suite builders 
	/// to create fixtures from the qualified types in each assembly. It implements
	/// the ISuiteBuilder interface itself, allowing it to be used by other classes
	/// for queries and suite construction.
	/// </summary>
	public class TestSuiteBuilder
	{
		#region Static Fields

		private static readonly string SuiteBuilderAttributeType = "NUnit.Framework.SuiteBuilderAttribute";
		private static readonly string SuiteBuilderInterfaceType = "NUnit.Core.ISuiteBuilder";
		
		#endregion

		#region Instance Fields

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

		/// <summary>
		/// The TestFramework used by the loaded assembly
		/// </summary>
		ITestFramework testFramework = null;

		/// <summary>
		/// Collection of SuiteBuilders that get a shot at building 
		/// each fixture we encounter.
		/// </summary>
		SuiteBuilderCollection builders = new SuiteBuilderCollection();
		//	{ new NUnitTestFixtureBuilder(), new LegacySuiteBuilder() };

		/// <summary>
		/// Our LegacySuite builder, which is only used when a 
		/// fixture has been passed by name on the command line.
		/// </summary>
		ISuiteBuilder legacySuiteBuilder;

		#endregion

		#region Properties

		public ITestFramework Framework
		{
			get { return testFramework; }
		}

		public SuiteBuilderCollection Builders
		{
			get { return builders; }	
		}

		#endregion

		#region ISuiteBuilder Members

		public bool CanBuildFrom(Type type)
		{
			foreach( ISuiteBuilder builder in builders )
				if ( builder.CanBuildFrom( type ) )
					return true;

			return false;
		}

		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			foreach( ISuiteBuilder builder in builders )
				if ( builder.CanBuildFrom( type ) )
					return builder.BuildFrom( type, assemblyKey );

			return null;
		}

		#endregion

		#region Constructor

		public TestSuiteBuilder()
		{
			builders.Add( new NUnit.Core.Builders.TestFixtureBuilder() );

			// TODO: Keeping this separate till we can make
			//it work in all situations.
			legacySuiteBuilder = new NUnit.Core.Builders.LegacySuiteBuilder();
		}

		#endregion

		#region Public Methods

		public TestSuite Build(string projectName, IList assemblies)
		{
			RootTestSuite rootSuite = new RootTestSuite( projectName );

			int assemblyKey = 0;
			foreach(string assembly in assemblies)
			{
				TestSuite suite = Build( assembly, assemblyKey++ );
				rootSuite.Add( suite );
			}

			return rootSuite;
		}

		public TestSuite Build( string assemblyName )
		{
			return Build( assemblyName, 0 );
		}

		public TestSuite Build(string assemblyName, string testName )
		{
			TestSuite suite = null;

			Assembly assembly = Load(assemblyName);

			if(assembly != null)
			{
				// If provided test name is actually a fixture,
				// just build and return that!
				Type testType = assembly.GetType(testName);
				if( testType != null )
					return BuildSingleFixture( testType );

				// Assume that testName is a namespace
				string prefix = testName + '.';
				int testFixtureCount = 0;
				if ( testFramework != null )
				{
					IList testTypes = GetCandidateFixtureTypes( assembly, testName );

					foreach(Type type in testTypes)
					{
						if( CanBuildFrom( type ) && type.Namespace != null )
						{
							if( type.Namespace == testName || type.Namespace.StartsWith(prefix) )
							{
								suite = BuildFromNameSpace( type.Namespace, 0);
						
								suite.Add( BuildFrom( type, 0 ) );
								testFixtureCount++;
							}
						}
					}
				}

				return testFixtureCount == 0 ? null : rootSuite;
			}

			return suite;
		}

		public TestSuite Build( IList assemblies, string testName )
		{
			TestSuite suite = null;

			foreach(string assemblyName in assemblies)
			{
				suite = Build( assemblyName, testName );
				if ( suite != null ) break;
			}

			return suite;
		}

		#endregion

		#region Nested TypeFilter Class

		private class TypeFilter
		{
			private string rootNamespace;

			TypeFilter( string rootNamespace ) 
			{
				this.rootNamespace = rootNamespace;
			}

			public bool Include( Type type )
			{
				if ( type.Namespace == rootNamespace )
					return true;

				return type.Namespace.StartsWith( rootNamespace + '.' );
			}
		}

		#endregion

		#region SuiteBuilderWrapper Class

		private class SuiteBuilderWrapper : ISuiteBuilder
		{
			private object builder;
			private MethodInfo canBuildFromMethod;
			private MethodInfo buildFromMethod;

			public SuiteBuilderWrapper( object builder )
			{
				this.builder = builder;
				this.canBuildFromMethod = Reflect.GetNamedMethod( 
					builder.GetType(), 
					"CanBuildFrom", 
					BindingFlags.Public | BindingFlags.Instance );
				this.buildFromMethod = Reflect.GetNamedMethod(
					builder.GetType(),
					"BuildFrom",
					BindingFlags.Public | BindingFlags.Instance );
				if ( buildFromMethod == null || canBuildFromMethod == null )
					throw new ArgumentException( "Invalid suite builder" );
				// TODO: Check for proper signature - put in Reflect?
			}

			#region ISuiteBuilder Members

			public bool CanBuildFrom(Type type)
			{
				return (bool)canBuildFromMethod.Invoke( builder, new object[] { type } );
			}

			public TestSuite BuildFrom(Type type, int assemblyKey)
			{
				return (TestSuite)buildFromMethod.Invoke( builder, new object[] { type, assemblyKey } );
			}

			#endregion

		}

		#endregion

		#region Helper Methods

		private Assembly Load(string assemblyName)
		{
			// Change currentDirectory in case assembly references unmanaged dlls
			using( new DirectorySwapper( Path.GetDirectoryName( assemblyName ) ) )
			{
				Assembly assembly = AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(assemblyName));

				this.testFramework = TestFramework.FromAssembly( assembly );

				foreach( Type type in assembly.GetExportedTypes() )
				{
					if ( Reflect.HasAttribute( type, SuiteBuilderAttributeType, false )
						&& Reflect.HasInterface( type, SuiteBuilderInterfaceType ) )
					{
						object builderObject = Reflect.Construct( type );
						ISuiteBuilder builder = builderObject as ISuiteBuilder;
						// May not be able to cast, if the builder uses an earlier
						// version of the interface.
						// TODO: Wrap the object and use reflection
						if ( builder != null )
							builders.Add( builder );
						else 
							builders.Add( new SuiteBuilderWrapper( builderObject ) );
						// TODO: Figure out when to unload - this is
						// not important now, since we use a different
						// appdomain for each load, but may be in future.
					}
				}

				return assembly;
			}
		}

		private TestSuite BuildFromNameSpace( string nameSpace, int assemblyKey )
		{
			if( nameSpace == null || nameSpace  == "" ) return rootSuite;
			TestSuite suite = (TestSuite)namespaceSuites[nameSpace];
			if(suite!=null) return suite;

			int index = nameSpace.LastIndexOf(".");
			string prefix = string.Format( "[{0}]", assemblyKey );
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

		private TestSuite Build( string assemblyName, int assemblyKey )
		{
			TestSuiteBuilder builder = new TestSuiteBuilder();

			Assembly assembly = Load( assemblyName );

			builder.rootSuite = 
				new AssemblyTestSuite( assemblyName, assemblyKey );
			int testFixtureCount = 0;

			if ( testFramework != null )
			{
				IList testTypes = testFramework.GetCandidateFixtureTypes( assembly );
				foreach(Type testType in testTypes)
				{
					if( CanBuildFrom( testType ) )
					{
						testFixtureCount++;
						string namespaces = testType.Namespace;
						TestSuite suite = builder.BuildFromNameSpace( namespaces, assemblyKey );

						suite.Add( BuildFrom( testType, assemblyKey ) );
					}
				}
			}

			if(testFixtureCount == 0)
			{
				builder.rootSuite.ShouldRun = false;
				builder.rootSuite.IgnoreReason = "Has no TestFixtures";
			}

			return builder.rootSuite;
		}
		private TestSuite BuildSingleFixture( Type testType )
		{
			// The only place we currently allow legacy suites
			if ( legacySuiteBuilder.CanBuildFrom( testType ) )
				return legacySuiteBuilder.BuildFrom( testType, 0 );

			return BuildFrom( testType, 0 );
		}
		
		private IList GetCandidateFixtureTypes( Assembly assembly, string ns )
		{
			IList types = testFramework.GetCandidateFixtureTypes( assembly );
			if ( ns == null || ns == string.Empty ) return types;

			string prefix = ns + "." ;
			
			ArrayList result = new ArrayList();
			foreach( Type type in types )
				if ( type.FullName.StartsWith( prefix ) )
					result.Add( type );

			return result;
		}

		#endregion
	}
}

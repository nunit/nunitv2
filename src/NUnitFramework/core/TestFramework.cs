using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestFramework.
	/// </summary>
	[Serializable]
	public class TestFramework
	{
		#region FrameworkInfo struct

		[Serializable]
			private struct FrameworkInfo
		{
			public string Name;
			public string AssemblyName;
			public string Namespace;

			public FrameworkInfo( 
				string frameworkName,
				string assemblyName, 
				string ns )
			{
				this.Name = frameworkName;
				this.AssemblyName = assemblyName;
				this.Namespace = ns;
			}
		}

		#endregion

		#region Static Fields

		/// <summary>
		/// List of FrameworkInfo structs for supported frameworks
		/// </summary>
		private static IList testFrameworks;

		/// <summary>
		/// Cache of TestFrameworks already created for an assembly
		/// </summary>
		private static IDictionary frameworkByAssembly = new Hashtable();

		#endregion

		#region Instance Fields

		/// <summary>
		/// The assembly containing the test framework
		/// </summary>
		private Assembly frameworkAssembly;

		/// <summary>
		/// The FrameworkInfo for this TestFramework
		/// </summary>
		private FrameworkInfo frameworkInfo;

		/// <summary>
		/// The original reference that caused this framework assembly
		/// to be loaded. This may differ from what is actually loaded.
		/// </summary>
		private AssemblyName refAssembly;

		#endregion

		#region Construction

		static TestFramework()
		{
			testFrameworks = new ArrayList();
			testFrameworks.Add( new FrameworkInfo( "NUnit", "nunit.framework", "NUnit.Framework" ) );
			testFrameworks.Add( new FrameworkInfo( "csUnit", "csUnit", "csUnit" ) );
			testFrameworks.Add( new FrameworkInfo( "Microsoft Team System",
				"Microsoft.VisualStudio.QualityTools.UnitTestFramework",
				"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework" ) );
		}

		private TestFramework( Assembly frameworkAssembly, FrameworkInfo frameworkInfo, AssemblyName refAssembly )
		{
			this.frameworkAssembly = frameworkAssembly;
			this.frameworkInfo = frameworkInfo;
			this.refAssembly = refAssembly;
		}

		#endregion

		#region Static Methods

		public static IList GetLoadedFrameworks()
		{
			ArrayList loadedAssemblies = new ArrayList();

			foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
			{
				foreach( FrameworkInfo frameworkInfo in testFrameworks )
				{
					if ( assembly.GetName().Name == frameworkInfo.AssemblyName )
					{
						loadedAssemblies.Add( assembly.GetName() );
						break;
					}
				}
			}

			return loadedAssemblies;
		}

		public static TestFramework FromFixture( object fixture )
		{
			return FromType( fixture.GetType() );
		}

		public static TestFramework FromType( Type type )
		{
			return FromAssembly( type.Assembly );
		}

		public static TestFramework FromMethod( MethodBase method )
		{
			return FromType( method.DeclaringType );
		}

		public static TestFramework FromAssembly( Assembly assembly )
		{
			TestFramework framework = (TestFramework)frameworkByAssembly[assembly];

			if ( framework == null )
			{
				foreach( AssemblyName refAssembly in assembly.GetReferencedAssemblies() )
				{
					foreach( FrameworkInfo frameworkInfo in testFrameworks )
					{
						if ( refAssembly.Name == frameworkInfo.AssemblyName )
						{
							Assembly frameworkAssembly = Assembly.Load( refAssembly.Name );
							framework = new TestFramework( 
								frameworkAssembly, frameworkInfo, refAssembly );
							frameworkByAssembly[assembly] = framework;
							return framework;
						}
					}
				}
			}

			return framework;
		}

		#endregion

		#region Properties

		public Assembly FrameworkAssembly
		{
			get { return frameworkAssembly; }
		}

		public string Name
		{
			get { return this.frameworkInfo.Name; }
		}

		public AssemblyName AssemblyName
		{
			get { return frameworkAssembly.GetName(); }
		}

		public Version Version
		{
			get { return AssemblyName.Version; }
		}

		public string Namespace
		{
			get { return frameworkInfo.Namespace; }
		}

		public AssemblyName ReferencedAssembly
		{
			get { return refAssembly; }
		}

		#endregion
	}
}

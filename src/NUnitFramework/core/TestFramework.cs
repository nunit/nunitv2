using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for TestFramework.
	/// </summary>
	[Serializable]
	public class TestFramework : ITestFramework
	{
		#region FrameworkInfo struct

		[Serializable]
		public struct FrameworkInfo
		{
			public string Name;
			public string AssemblyName;
			public string Namespace;
			public string AssertionType;
			public string AssertionExceptionType;
			public string IgnoreExceptionType;
			public bool AllowPrivateTests;

			public FrameworkInfo( 
				string frameworkName,
				string assemblyName, 
				string ns,
				string assertionType,
				string assertionExceptionType,
				string ignoreExceptionType,
				bool allowPrivateTests )
			{
				this.Name = frameworkName;
				this.AssemblyName = assemblyName;
				this.Namespace = ns;
				this.AssertionType = ns + "." + assertionType;
				this.AssertionExceptionType = ns + "." + assertionExceptionType;
				this.IgnoreExceptionType = ns + "." + ignoreExceptionType;
				this.AllowPrivateTests = allowPrivateTests;
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

		/// <summary>
		/// The type that implements asserts in this framework
		/// </summary>
		private Type assertionType;

		#endregion

		#region Construction

		static TestFramework()
		{
			testFrameworks = new ArrayList();
			testFrameworks.Add( new FrameworkInfo( 
				"NUnit", "nunit.framework", "NUnit.Framework", 
				"Assert", "AssertionException", "IgnoreException",
				false ) );
			testFrameworks.Add( new FrameworkInfo( 
				"csUnit", "csUnit", "csUnit", 
				"Assert", "AssertionException", null,
				false ) );
			testFrameworks.Add( new FrameworkInfo( 
				"Microsoft Team System",
				"Microsoft.VisualStudio.QualityTools.UnitTestFramework",
				"Microsoft.VisualStudio.QualityTools.UnitTesting.Framework",
				"Assert", "AssertionException", null,
				true) );
		}

		private TestFramework( Assembly frameworkAssembly, FrameworkInfo frameworkInfo, AssemblyName refAssembly )
		{
			this.frameworkAssembly = frameworkAssembly;
			this.frameworkInfo = frameworkInfo;
			this.refAssembly = refAssembly;
			this.assertionType = frameworkAssembly.GetType( frameworkInfo.AssertionType, false, false );
		}

		#endregion

		#region Static Methods

		public static void Add( FrameworkInfo info )
		{
			testFrameworks.Add( info );
		}

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

		public static ITestFramework FromFixture( object fixture )
		{
			return FromType( fixture.GetType() );
		}

		public static ITestFramework FromType( Type type )
		{
			return FromAssembly( type.Assembly );
		}

		public static ITestFramework FromMethod( MethodBase method )
		{
			return FromType( method.DeclaringType );
		}

		public static ITestFramework FromAssembly( Assembly assembly )
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
							Assembly frameworkAssembly = Assembly.Load( refAssembly );
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

//		public string TestFixtureType
//		{
//			get { return frameworkInfo.TestFixtureType; }
//		}

		#endregion

		#region Instance Methods

		public IList GetCandidateFixtureTypes( Assembly assembly )
		{
			if ( this.frameworkInfo.AllowPrivateTests )
				return assembly.GetTypes();
			else
				return assembly.GetExportedTypes();
		}

		public IList GetCandidateTestMethods( Type fixtureType )
		{
			if ( this.frameworkInfo.AllowPrivateTests )
				return fixtureType.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			else
				return fixtureType.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static );
		}

		public int GetAssertCount()
		{
			int count = 0;

			if ( assertionType != null )
			{
				PropertyInfo property = Reflect.GetNamedProperty( 
					assertionType, 
					"Counter", 
					BindingFlags.Public | BindingFlags.Instance );
				if ( property != null )
					count = (int)property.GetValue( null, new object[0] );
			}

			return count;
		}

		public bool IsAssertException( Exception ex )
		{
			return ex.GetType().FullName == frameworkInfo.AssertionExceptionType;
		}

		public bool IsIgnoreException( Exception ex )
		{
			return ex.GetType().FullName == frameworkInfo.IgnoreExceptionType;
		}

		#endregion
	}

	public interface ITestFramework
	{
		IList GetCandidateFixtureTypes( Assembly assembly );
		IList GetCandidateTestMethods( Type fixtureType );
		int GetAssertCount();
		bool IsAssertException( Exception ex );
		bool IsIgnoreException( Exception ex );
	}

}

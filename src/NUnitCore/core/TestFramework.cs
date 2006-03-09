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

using System;
using System.Reflection;
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// Interface that represents a test framework
	/// </summary>
	public interface ITestFramework
	{
		string Name { get; }
		bool AllowPrivateTests { get; }

		int GetAssertCount();
		bool IsAssertException( Exception ex );
		bool IsIgnoreException( Exception ex );
	}

	/// <summary>
	/// Summary description for TestFramework.
	/// </summary>
	[Serializable]
	public class TestFramework : ITestFramework
	{
		#region FrameworkInfo struct

		/// <summary>
		/// Struct containing information needed to create a TestFramework
		/// </summary>
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

		#region Static Constructor

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
				"vsts",
				"Microsoft.VisualStudio.QualityTools.UnitTestFramework",
				"Microsoft.VisualStudio.TestTools.UnitTesting",
				"Assert", "AssertionException", "AssertInconclusiveException",
				true) );

		}

		#endregion

		#region Static Methods

		public static void Add( FrameworkInfo info )
		{
			testFrameworks.Add( info );
		}

		//		public static IList FrameworkInfo
		//		{
		//			get { return testFrameworks; }
		//		}

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
								frameworkAssembly, frameworkInfo);
							frameworkByAssembly[assembly] = framework;
							return framework;
						}
					}
				}
			}

			return framework;
		}

		#endregion

		#region Private Constructor
		private TestFramework( Assembly frameworkAssembly, FrameworkInfo frameworkInfo)
		{
			this.frameworkInfo = frameworkInfo;
			this.assertionType = frameworkAssembly.GetType( frameworkInfo.AssertionType, false, false );
		}
		#endregion

		#region Instance Fields

		/// <summary>
		/// The FrameworkInfo for this TestFramework
		/// </summary>
		private FrameworkInfo frameworkInfo;

		/// <summary>
		/// The type that implements asserts in this framework.
		/// Used to get the count of assertions if applicable.
		/// </summary>
		private Type assertionType;

		#endregion

		#region ITestFramework Interface

		public string Name
		{
			get { return this.frameworkInfo.Name; }
		}

		public bool AllowPrivateTests
		{
			get { return frameworkInfo.AllowPrivateTests; }
		}

		public int GetAssertCount()
		{
			int count = 0;

			if ( assertionType != null )
			{
				PropertyInfo property = Reflect.GetNamedProperty( 
					assertionType, 
					"Counter", 
					BindingFlags.Public | BindingFlags.Static );
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
}

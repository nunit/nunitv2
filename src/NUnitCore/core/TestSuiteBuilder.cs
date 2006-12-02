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
	using NUnit.Core.Builders;
	using System.Collections;
	using System.Reflection;

	/// <summary>
	/// This is the master suite builder for NUnit. It builds a test suite from
	/// one or more assemblies using a list of internal and external suite builders 
	/// to create fixtures from the qualified types in each assembly. It implements
	/// the ISuiteBuilder interface itself, allowing it to be used by other classes
	/// for queries and suite construction.
	/// </summary>D:\Dev\NUnit\nunit20\src\NUnitFramework\core\TestBuilderAttribute.cs
	public class TestSuiteBuilder
	{
		#region Instance Variables

		private bool autoNamespaceSuites = true;

		private bool mergeAssemblies = false;

		private ArrayList builders = new ArrayList();

		#endregion

		#region Properties
		public IList Assemblies
		{
			get 
			{
				ArrayList assemblies = new ArrayList();
				foreach( TestAssemblyBuilder builder in builders )
					assemblies.Add( builder.Assembly );
				return assemblies; 
			}
		}

		public IList AssemblyInfo
		{
			get
			{
				ArrayList info = new ArrayList();
				foreach( TestAssemblyBuilder builder in this.builders )
					info.Add( builder.AssemblyInfo );

				return info;
			}
		}

		public bool AutoNamespaceSuites
		{
			get { return autoNamespaceSuites; }
			set { autoNamespaceSuites = value; }
		}

		public bool MergeAssemblies
		{
			get { return mergeAssemblies; } 
			set { mergeAssemblies = value; }
		}
		#endregion

		#region Build Methods
		/// <summary>
		/// Build a suite based on a TestPackage
		/// </summary>
		/// <param name="package">The TestPackage</param>
		/// <returns>A TestSuite</returns>
		public TestSuite Build( TestPackage package )
		{
			if ( package.IsSingleAssembly )
				return BuildSingleAssembly( package );

			TestSuite rootSuite = new TestSuite( package.FullName );
			NamespaceTreeBuilder namespaceTree = 
				new NamespaceTreeBuilder( rootSuite );

			builders.Clear();
			foreach(string assemblyName in package.Assemblies)
			{
				TestAssemblyBuilder builder = new TestAssemblyBuilder();
				builder.AutoNamespaceSuites = this.AutoNamespaceSuites && !this.MergeAssemblies;
				builders.Add( builder );

				Test testAssembly =  builder.Build( assemblyName, package.TestName );

				if ( testAssembly != null )
				{
                    if (!mergeAssemblies)
                    {
                        rootSuite.Add(testAssembly);
                    }
                    else if (autoNamespaceSuites)
                    {
                        namespaceTree.Add(testAssembly.Tests);
                        rootSuite = namespaceTree.RootSuite;
                    }
                    else
                    {
                        foreach (Test test in testAssembly.Tests)
                            rootSuite.Add(test);
                    }
				}
			}

			if ( rootSuite.Tests.Count == 0 )
				return null;

			return rootSuite;
		}

		private TestSuite BuildSingleAssembly( TestPackage package )
		{
			TestAssemblyBuilder builder = new TestAssemblyBuilder();
			builder.AutoNamespaceSuites = this.AutoNamespaceSuites; // && !this.MergeAssemblies;
			builders.Clear();
			builders.Add( builder );

			return (TestSuite)builder.Build( package.FullName, package.TestName );
		}
		#endregion
	}
}

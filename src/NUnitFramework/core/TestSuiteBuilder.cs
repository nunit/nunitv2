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
	using System.Collections;
	using NUnit.Core.Builders;

	/// <summary>
	/// This is the master suite builder for NUnit. It builds a test suite from
	/// one or more assemblies using a list of internal and external suite builders 
	/// to create fixtures from the qualified types in each assembly. It implements
	/// the ISuiteBuilder interface itself, allowing it to be used by other classes
	/// for queries and suite construction.
	/// </summary>
	public class TestSuiteBuilder
	{
		#region Instance Variables

		private bool autoNamespaceSuites = true;

		private bool mergeAssemblies = false;

		#endregion

		#region Properties

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

		#region Public Methods

		public TestSuite Build(string projectName, IList assemblies)
		{
			RootTestSuite rootSuite = new RootTestSuite( projectName );
			NamespaceTreeBuilder namespaceTree = 
				new NamespaceTreeBuilder( rootSuite );

			int assemblyKey = 0;
			foreach(string assemblyName in assemblies)
			{
				TestAssemblyBuilder builder = new TestAssemblyBuilder( 
					assemblyName, assemblyKey++, 
					autoNamespaceSuites & !mergeAssemblies );
				TestSuite testAssembly =  builder.Build();

				if ( !mergeAssemblies )
					rootSuite.Add( testAssembly );
				else 
					foreach( Test test in testAssembly.Tests )
						if (autoNamespaceSuites )
							namespaceTree.Add( test );
						else
							rootSuite.Add( test );
			}

			return rootSuite;
		}

		public TestSuite Build( string assemblyName )
		{
			TestAssemblyBuilder builder = 
				new TestAssemblyBuilder( assemblyName, 0, this.autoNamespaceSuites );
			return builder.Build();
		}

		public TestSuite Build(string assemblyName, string testName )
		{
			TestAssemblyBuilder builder = 
				new TestAssemblyBuilder( assemblyName, 0, this.autoNamespaceSuites );
			return builder.Build( testName );
		}

		public TestSuite Build( IList assemblies, string testName )
		{
			TestSuite suite = null;

			foreach(string assemblyName in assemblies)
			{
				TestAssemblyBuilder builder = 
					new TestAssemblyBuilder( assemblyName, 0, this.autoNamespaceSuites );
				suite = builder.Build( testName );
				if ( suite != null ) break;
			}

			return suite;
		}

		#endregion
	}
}

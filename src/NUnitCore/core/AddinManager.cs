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
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnit.Core
{
	public class AddinManager : IAddinHost
	{
		#region CurrentAddinManager Singleton
		private static AddinManager current;

		public static AddinManager CurrentManager
		{
			get 
			{ 
				if ( current == null )
				{
					current = new AddinManager();
					current.InstallBuiltins();
					current.RegisterAddins();
					current.InstallAddins();
				}

				return current;
			}
		}
		#endregion

		#region Constructors
		public AddinManager() { }

		public AddinManager( AddinManager priorState )
		{
			this.priorState = priorState;
			this.suiteBuilders = new SuiteBuilderCollection( priorState.suiteBuilders );
			this.testBuilders = new TestCaseBuilderCollection( priorState.testBuilders );
			this.testDecorators = new TestDecoratorCollection( priorState.testDecorators );
			this.addins = new ArrayList( priorState.Addins );
		}
		#endregion

		#region Instance Fields
		private AddinManager priorState = null;

		private SuiteBuilderCollection suiteBuilders = new SuiteBuilderCollection();
		private TestCaseBuilderCollection testBuilders = new TestCaseBuilderCollection();
		private TestDecoratorCollection testDecorators = new TestDecoratorCollection();
		private ArrayList addins = new ArrayList();

		#endregion

		#region Static Methods
		public static void Save()
		{
			current = new AddinManager( current );
		}

		public static void Restore()
		{
			current = current.PriorState;
		}
		#endregion

		#region Instance Properties
		public AddinManager PriorState
		{
			get { return priorState; }
		}

		public ISuiteBuilder SuiteBuilders
		{
			get { return suiteBuilders; }
		}

		public ITestCaseBuilder TestBuilders
		{
			get { return testBuilders; }
		}

		public ITestDecorator TestDecorators
		{
			get { return testDecorators; }
		}

		public IList Addins
		{
			get
			{
				return addins;
			}
		}

		public IList Names
		{
			get
			{
				ArrayList names = new ArrayList();
		
				foreach( Addin addin in Addins )
					names.Add( addin.Name );

				return names;
			}
		}

		public IList AssemblyQualifiedNames
		{
			get
			{
				ArrayList names = new ArrayList();
			
				foreach( object addin in Addins )
				{
					names.Add( addin.GetType().AssemblyQualifiedName );
				}

				return names;
			}
		}
		#endregion

		#region Addin Registration
		public void RegisterAddins()
		{
			//Figure out the directory from which NUnit is executing
			string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			string nunitDirPath = Path.GetDirectoryName( moduleName );
			string coreExtensions = Path.Combine( nunitDirPath, "nunit.core.extensions.dll" );
			string addinsDirPath = Path.Combine( nunitDirPath, "addins" );

			// Load nunit.core.extensions if available
			if ( File.Exists( coreExtensions ) )
				Register( coreExtensions );

			// Load any extensions in the addins directory
			DirectoryInfo dir = new DirectoryInfo( addinsDirPath );
			if ( dir.Exists )
				foreach( FileInfo file in dir.GetFiles( "*.dll" ) )
					Register( file.FullName );
		}

		public void Register( string path )
		{
			try
			{
				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = Path.GetFileNameWithoutExtension(path);
				assemblyName.CodeBase = path;
				Assembly assembly = Assembly.Load(assemblyName);
				Register( assembly );
			}
			catch( Exception ex )
			{
				// HACK: Where should this be logged? 
				// Don't pollute the trace listeners.
				TraceListener listener = new DefaultTraceListener();
				listener.WriteLine( "Extension not loaded: " + path  );
				listener.WriteLine( ex.ToString() );
				//throw new ApplicationException( "Extension not loaded: " + file.FullName );
			}
		}

		public void Register( Assembly assembly ) 
		{
			foreach( Type type in assembly.GetExportedTypes() )
			{
				if ( NUnitFramework.IsNUnitAddin( type ) )
				{
					Addin addin = new Addin( type );
					addins.Add( addin );
				}
			}
		}

		public void Register( IAddin addin )
		{
			addins.Add( addin );
			addin.Initialize(this);
		}
		#endregion

		#region Addin Installation
		public void InstallBuiltins()
		{
			// Define NUnit Framework
			TestFramework.Register( "NUnit", "nunit.framework" );

			// Install builtin SuiteBuilders - Note that the
			// NUnitTestCaseBuilder is installed whenever
			// an NUnitTestFixture is being populated and
			// removed afterward.
			Install( new Builders.NUnitTestFixtureBuilder() );
			Install( new Builders.SetUpFixtureBuilder() );
		}

		public void InstallAddins()
		{
			foreach( Addin addin in addins )
			{
				addin.Initialize( this );
			}
		}
		#endregion

		#region IAddinHost Members
		public void Install( ISuiteBuilder builder )
		{
			suiteBuilders.Add( builder );
		}

		public void Install( ITestCaseBuilder builder )
		{
			testBuilders.Add( builder );
		}

		public void Install( ITestDecorator decorator )
		{
			testDecorators.Add( decorator );
		}
		#endregion
	}
}

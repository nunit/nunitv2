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
//using NUnit.Core.Interfaces;

namespace NUnit.Core
{
	public class AddinManager : ISuiteBuilder, ITestCaseBuilder, ITestDecorator
	{
		#region Static Fields
		private static AddinManager current = new AddinManager();
		#endregion

		#region Instance Fields
		private AddinManager priorState = null;

		private SuiteBuilderCollection suiteBuilders = new SuiteBuilderCollection();
		private TestCaseBuilderCollection testBuilders = new TestCaseBuilderCollection();
		private TestDecoratorCollection testDecorators = new TestDecoratorCollection();
		private ArrayList addins = new ArrayList();

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

		#region Static Constructor
		static AddinManager()
		{	
			//Figure out the directory from which NUnit is executing
			string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			string nunitDirPath = Path.GetDirectoryName( moduleName );

			// Load nunit.core.extensions if available
			string extensions = Path.Combine( nunitDirPath, "nunit.core.extensions.dll" );
			if ( File.Exists( extensions ) )
			{
				try
				{
					AssemblyName assemblyName = new AssemblyName();
					assemblyName.Name = "nunit.core.extensions";
					assemblyName.CodeBase = extensions;
					Assembly assembly = Assembly.Load(assemblyName);
					CurrentManager.Register( assembly );
				}
				catch( Exception ex )
				{
					// HACK: Where should this be logged? 
					// Don't pollute the trace listeners.

					TraceListener listener = new DefaultTraceListener();
					listener.WriteLine( "Extension not loaded: nunit.core.extensions"  );
					listener.WriteLine( ex.ToString() );
					//throw new ApplicationException( "Extension not loaded: nunit.core.extensions", ex );
				}
			}

			// Load any extensions in the addins directory
			DirectoryInfo dir = new DirectoryInfo( Path.Combine( nunitDirPath, "addins" ) );
			if ( dir.Exists )
				foreach( FileInfo file in dir.GetFiles( "*.dll" ) )
					try
					{
						AssemblyName assemblyName = new AssemblyName();
						assemblyName.CodeBase = file.FullName;
						Assembly assembly = Assembly.Load(assemblyName);
						CurrentManager.Register( assembly );
					}
					catch( Exception ex )
					{
						// HACK: Where should this be logged? 
						// Don't pollute the trace listeners.
						TraceListener listener = new DefaultTraceListener();
						listener.WriteLine( "Extension not loaded: " + file.FullName  );
						listener.WriteLine( ex.ToString() );
						//throw new ApplicationException( "Extension not loaded: " + file.FullName );
					}
		}
		#endregion

		#region Static Properties
		public static AddinManager CurrentManager
		{
			get { return current; }
		}
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

//				foreach( IPlugin plugin in addins )
//					names.Add( plugin.Name );
			
				foreach( object addin in Addins )
				{
					names.Add( addin.GetType().Name );
				}

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

		#region ISuiteBuilder Members
		public bool CanBuildFrom(Type type)
		{
			return suiteBuilders.CanBuildFrom( type );
		}

		public TestSuite BuildFrom(Type type)
		{
			return suiteBuilders.BuildFrom( type );
		}
		#endregion

		#region ITestCaseBuilder Members
		public bool CanBuildFrom(MethodInfo method)
		{
			return testBuilders.CanBuildFrom( method );
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			return testBuilders.BuildFrom( method );
		}
		#endregion

		#region ITestDecorator Members
		public TestCase Decorate(TestCase testCase, MethodInfo method)
		{
			return testDecorators.Decorate( testCase, method );
		}

		public TestSuite Decorate(TestSuite suite, Type fixtureType)
		{
			return testDecorators.Decorate( suite, fixtureType );
		}
		#endregion

		#region Addin Registration
		public void Register( Assembly assembly ) 
		{
			foreach( Type type in assembly.GetExportedTypes() )
			{
//				if ( Reflect.HasInterface( type, "NUnit.Core.Interfaces.IPlugin" ) )
//				{
//					IPlugin plugin = Reflect.Construct( type ) as IPlugin;
//					if  ( plugin != null )
//						addins.Add( plugin );
//				}

				if ( NUnitFramework.IsSuiteBuilder( type ) )
				{
					object builderObject = Reflect.Construct( type );
					ISuiteBuilder builder = builderObject as ISuiteBuilder;
					// May not be able to cast, if the builder uses an earlier
                    // version of the interface, so we use reflection.
                    if (builder != null)
						suiteBuilders.Add( builder );
					// TODO: Figure out when to unload - this is
					// not important now, since we use a different
					// appdomain for each load, but may be in future.
				}
				else if ( NUnitFramework.IsTestCaseBuilder( type ) )
				{
					object builderObject = Reflect.Construct( type );
					ITestCaseBuilder builder = builderObject as ITestCaseBuilder;
                    // May not be able to cast, if the builder uses an earlier
                    // version of the interface, so we use reflection.
                    if (builder != null)
                        testBuilders.Add(builder);
				}
				else if ( NUnitFramework.IsTestDecorator( type ) )
				{
					object decoratorObject = Reflect.Construct( type );
					ITestDecorator decorator = decoratorObject as ITestDecorator;
                    // May not be able to cast, if the decorator uses an earlier
                    // version of the interface, so we use reflection.
                    if (decorator != null)
                        testDecorators.Add(decorator);
				}
			}
		}

		public void Register( ISuiteBuilder builder )
		{
			addins.Add( builder );
			suiteBuilders.Add( builder );
		}

		public void Register( ITestCaseBuilder builder )
		{
			addins.Add( builder );
			testBuilders.Add( builder );
		}

		public void Register( ITestDecorator decorator )
		{
			addins.Add( decorator );
			testDecorators.Add( decorator );
		}

		public void Clear()
		{
			suiteBuilders.Clear();
			testBuilders.Clear();
			testDecorators.Clear();
		}
		#endregion
    }
}

// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnit.Util
{
	public class AddinManager : IService
	{
		#region Instance Fields
		IAddinRegistry addinRegistry;
		#endregion

		#region Constructor
		public AddinManager()
		{
			addinRegistry = Services.AddinRegistry;
		}
		#endregion

		#region Addin Registration
		public void RegisterAddins()
		{
			//Figure out the directory from which NUnit is executing
			string moduleName = new Uri(GetType().Assembly.CodeBase).LocalPath;
			//string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
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

				foreach ( Type type in assembly.GetExportedTypes() )
				{
					if ( type.GetCustomAttributes(typeof(NUnitAddinAttribute), false).Length == 1 )
					{
						Addin addin = new Addin( type );
						addinRegistry.Register( addin );
					}
				}
			}
			catch( Exception ex )
			{
				// NOTE: Since the gui isn't loaded at this point, 
				// the trace output will only show up in Visual Studio
				Trace.WriteLine( "Extension not loaded: " + path  );
				Trace.WriteLine( ex.ToString() );
			}
		}
		#endregion

		#region IService Members

		public void InitializeService()
		{
			RegisterAddins();
		}

		public void UnloadService()
		{
		}

		#endregion
	}
}

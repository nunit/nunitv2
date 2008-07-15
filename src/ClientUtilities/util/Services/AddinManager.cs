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
		}
		#endregion

		#region Addin Registration
		public void RegisterAddins()
		{
			// Load nunit.core.extensions if available
            string coreExtensions = Path.Combine(NUnitConfiguration.NUnitDirectory, "nunit.core.extensions.dll");
            if (File.Exists(coreExtensions))
				Register( coreExtensions );

			// Load any extensions in the addins directory
			DirectoryInfo dir = new DirectoryInfo( NUnitConfiguration.AddinDirectory );
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
				NTrace.Debug( "Loaded " + Path.GetFileName(path) );

				foreach ( Type type in assembly.GetExportedTypes() )
				{
					if ( type.GetCustomAttributes(typeof(NUnitAddinAttribute), false).Length == 1 )
					{
						Addin addin = new Addin( type );
						addinRegistry.Register( addin );
						NTrace.Debug( "Registered addin: " + addin.Name );
					}
				}
			}
			catch( Exception ex )
			{
				// NOTE: Since the gui isn't loaded at this point, 
				// the trace output will only show up in Visual Studio
				NTrace.Error( "Failed to load" + path, ex  );
			}
		}
		#endregion

		#region IService Members

		public void InitializeService()
		{
			addinRegistry = Services.AddinRegistry;
			RegisterAddins();
		}

		public void UnloadService()
		{
		}

		#endregion
	}
}

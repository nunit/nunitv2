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

namespace NUnit.Core.Extensibility
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
				//throw new ApplicationException( "Extension not loaded: " + path );
			}
		}

		public void Register( Assembly assembly )
		{
			foreach ( Type type in assembly.GetExportedTypes() )
			{
				if ( type.GetCustomAttributes(typeof(NUnitAddinAttribute), false).Length == 1 )
				{
					Addin addin = new Addin( type );
					addinRegistry.Register( addin );
				}
			}
		}
		#endregion

        #region Addin Installation
        public void InstallAddins(IExtensionHost host)
        {
            foreach (Addin addin in addinRegistry.Addins)
            {
                if ((host.ExtensionTypes & addin.ExtensionType) != 0)
                {
                    Type type = Type.GetType(addin.TypeName);
					if ( type != null )
					{
						ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
						IAddin theAddin = (IAddin)ctor.Invoke(new object[0]);

						if ( theAddin.Install(host) )
							addinRegistry.SetStatus( addin.Name, AddinStatus.Loaded );
					}
					else
						addinRegistry.SetStatus( addin.Name, AddinStatus.Error );
                }
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

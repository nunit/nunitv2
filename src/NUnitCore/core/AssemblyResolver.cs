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
	using System.IO;
	using System.Reflection;
	using System.Collections;
	using System.Diagnostics;

	/// <summary>
	/// Class adapted from NUnitAddin for use in handling assemblies that are not
    /// found in the test AppDomain.
	/// </summary>
    public class AssemblyResolver : MarshalByRefObject, IDisposable
	{
		private class AssemblyCache
		{
			private Hashtable _resolved = new Hashtable();

			public bool Contains( string name )
			{
				return _resolved.ContainsKey( name );
			}

			public Assembly Resolve( string name )
			{
				if ( _resolved.ContainsKey( name ) )
					return (Assembly)_resolved[name];
				
				return null;
			}

			public void Add( string name, Assembly assembly )
			{
				_resolved[name] = assembly;
			}
		}

		private AssemblyCache _cache = new AssemblyCache();

		private ArrayList _dirs = new ArrayList();

		public AssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		public void Dispose()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		public void AddFile( string file )
		{
			Assembly assembly = Assembly.LoadFrom( file );
			_cache.Add(assembly.GetName().FullName, assembly);
		}

		public void AddFiles( string directory, string pattern )
		{
			if ( Directory.Exists( directory ) )
				foreach( string file in Directory.GetFiles( directory, pattern ) )
					AddFile( file );
		}

		public void AddDirectory( string directory )
		{
			if ( Directory.Exists( directory ) )
				_dirs.Add( directory );
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string fullName = args.Name;
			int index = fullName.IndexOf(',');
			if(index == -1)							// Only resolve using full name.
			{
				Trace.WriteLine( string.Format("Not a strong name: {0}", fullName ),
					"'AssemblyResolver'" );
				return null;
			}

			if ( _cache.Contains( fullName ) )
			{
				Trace.WriteLine( string.Format( "Resolved from Cache: {0}", fullName ), 
					"'AssemblyResolver'" );
				return _cache.Resolve(fullName);
			}

			foreach( string dir in _dirs )
			{
				foreach( string file in Directory.GetFiles( dir ) )
				{
					string fullFile = Path.Combine( dir, file );
					if ( AssemblyName.GetAssemblyName( fullFile ).FullName == fullName )
					{
						Trace.WriteLine( string.Format( "Added to Cache: {0}", fullFile ), 
							"'AssemblyResolver'" );
						AddFile( fullFile );
						return _cache.Resolve( fullName );
					}
				}
			}

			Trace.WriteLine( string.Format( "Not in Cache: {0}", fullName), 
				"'AssemblyResolver'");
			return null;
		}
	}
}

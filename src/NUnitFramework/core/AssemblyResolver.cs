#region Copyright (c) 2004 Jamie L. Cansdale, Charlie Poole
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

		private IList _directories = new ArrayList();

		private IList _files = new ArrayList();

		private AssemblyCache _cache = new AssemblyCache();

		public AssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		public void Dispose()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		public void AddDirectory(string directory)
		{
			_directories.Add(directory);
		}

		public void AddFile( string file )
		{
			_files.Add( file );
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string fullName = args.Name;
			int index = fullName.IndexOf(',');
			if(index == -1)							// Only resolve using full name.
			{
				return null;
			}

			string name = fullName.Substring(0, index);
			if ( _cache.Contains( name ) )
			{
				Trace.WriteLine( string.Format( "Resolved from Cache: {0}", fullName ), 
					"'AssemblyResolver'" );
				return _cache.Resolve( name );
			}

			foreach(string file in _files)
			{
				try
				{
					if(File.Exists(file))
					{
						AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
						if(fullName.ToLower() == assemblyName.FullName.ToLower())
						{
							Trace.WriteLine( string.Format( "Resolved {0}", fullName ), 
								"'AssemblyResolver'" );
							Trace.WriteLine( string.Format( "      as {0}", file ), 
								"'AssemblyResolver'" );
							Assembly assembly = Assembly.LoadFrom( file );
							_cache.Add( name, assembly );
							return assembly;
						}
					}
				}
				catch(Exception e)
				{
					Debug.WriteLine(e);
				}
			}

			foreach(string directory in _directories)
			{
				try
				{
					string file = Path.Combine(directory, name + ".dll");
					if(File.Exists(file))
					{
						AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
						if(fullName.ToLower() == assemblyName.FullName.ToLower())
						{
							Trace.WriteLine( string.Format( "Resolved {0}", fullName ), 
								"'AssemblyResolver'" );
							Trace.WriteLine( string.Format( "      as {0}", file ), 
								"'AssemblyResolver'" );
							Assembly assembly = Assembly.LoadFrom( file );
							_cache.Add( name, assembly );
							return assembly;
						}
					}
				}
				catch(Exception e)
				{
					Debug.WriteLine(e);
				}
			}
			return null;
		}
	}
}

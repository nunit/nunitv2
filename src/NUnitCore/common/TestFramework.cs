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
	/// Summary description for TestFramework.
	/// </summary>
	[Serializable]
	public class TestFramework
	{
		#region Static Fields
		/// <summary>
		/// List of FrameworkInfo structs for supported frameworks
		/// </summary>
		private static Hashtable testFrameworks = new Hashtable();
		#endregion

		#region Instance Fields
		/// <summary>
		/// The name of the framework
		/// </summary>
		public string Name;

		/// <summary>
		/// The file name of the assembly that defines the framwork
		/// </summary>
		public string AssemblyName;
		#endregion

		#region Static Methods
		/// <summary>
		/// Register a framework. NUnit registers itself using this method. Add-ins that
		/// work with or emulate a different framework may register themselves as well.
		/// </summary>
		/// <param name="frameworkName">The name of the framework</param>
		/// <param name="assemblyName">The name of the assembly that framework users reference</param>
		public static void Register( string frameworkName, string assemblyName )
		{
			testFrameworks[frameworkName] = new TestFramework( frameworkName, assemblyName );
		}

		/// <summary>
		/// Get a list of known frameworks referenced by an assembly
		/// </summary>
		/// <param name="assembly">The assembly to be examined</param>
		/// <returns>A list of AssemblyNames</returns>
		public static IList GetReferencedFrameworks( Assembly assembly )
		{
			ArrayList referencedAssemblies = new ArrayList();

			foreach( AssemblyName assemblyRef in assembly.GetReferencedAssemblies() )
			{
				foreach( TestFramework info in testFrameworks.Values )
				{
					if ( assemblyRef.Name == info.AssemblyName )
					{
						referencedAssemblies.Add( assemblyRef );
						break;
					}
				}
			}

			return referencedAssemblies;
		}
		#endregion

		#region Private Constructor
		private TestFramework( string frameworkName, string assemblyName ) 
		{
			this.Name = frameworkName;
			this.AssemblyName = assemblyName;
		}
		#endregion
	}
}

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
	public class TestFramework
	{
		#region FrameworkInfo struct
		/// <summary>
		/// Struct containing information needed to create a TestFramework
		/// </summary>
		[Serializable]
		public struct FrameworkInfo
		{
			public string Name;
			public string AssemblyName;

			public FrameworkInfo( 
				string frameworkName,
				string assemblyName )
			{
				this.Name = frameworkName;
				this.AssemblyName = assemblyName;
			}
		}
		#endregion

		#region Static Fields
		/// <summary>
		/// List of FrameworkInfo structs for supported frameworks
		/// </summary>
		private static IList testFrameworks;
		#endregion

		#region Static Constructor
		static TestFramework()
		{
			testFrameworks = new ArrayList();
			testFrameworks.Add( new FrameworkInfo( "NUnit", "nunit.framework" ) );
		}
		#endregion

		#region Static Methods
		public static void Register( FrameworkInfo info )
		{
			testFrameworks.Add( info );
		}

		public static IList GetLoadedFrameworks()
		{
			ArrayList loadedAssemblies = new ArrayList();

			foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
			{
				foreach( FrameworkInfo info in testFrameworks )
				{
					if ( assembly.GetName().Name == info.AssemblyName )
					{
						loadedAssemblies.Add( assembly.GetName() );
						break;
					}
				}
			}

			return loadedAssemblies;
		}
		#endregion

		#region Private Constructor
		private TestFramework() { }
		#endregion
	}
}

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
using System.Reflection;
using System.Diagnostics;

namespace NUnit.Core
{
	/// <summary>
	/// The Addins class groups together all addin test suite
	/// and test case builders for access through static methods.
	/// </summary>
	public class Addins
	{
		#region Static Fields
		/// <summary>
		/// Addin manager for actual addins
		/// </summary>
		static AddinManager current = new AddinManager();
		#endregion

		#region Static Constructor
		static Addins()
		{	
			//Figure out the directory from which NUnit is exeuting
			string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			string nunitDirPath = Path.GetDirectoryName( moduleName );

			// Load nunit.core.extensions if available
			string extensions = Path.Combine( nunitDirPath, "nunit.core.extensions.dll" );
			if ( File.Exists( extensions ) )
			{
				try
				{
					Assembly assembly = Assembly.LoadFile( extensions );
					current.Register( assembly );
				}
				catch( Exception )
				{
					// HACK: Where should this be logged? 
					// Don't pollute the trace listeners.
					new DefaultTraceListener().WriteLine( "NUnit extensions not loaded" );
				}
			}

			// Load any extensions in the addins directory
			DirectoryInfo dir = new DirectoryInfo( Path.Combine( nunitDirPath, "addins" ) );
			if ( dir.Exists )
				foreach( FileInfo file in dir.GetFiles( "*.dll" ) )
					try
					{
						Assembly assembly = Assembly.LoadFile( file.FullName );
						current.Register( assembly );
					}
					catch( Exception )
					{
						// HACK: Where should this be logged? 
						// Don't pollute the trace listeners.
						new DefaultTraceListener().WriteLine( "Addins in " + file.Name + " not loaded" );
					}
		}
		#endregion

		#region Static Methods
		public static void Register( Assembly assembly )
		{
			current.Register( assembly );
		}

		public static void Register( ISuiteBuilder builder )
		{
			current.Register( builder );
		}

		public static void Register( ITestCaseBuilder builder )
		{
			current.Register( builder );
		}

		public static void Save()
		{
			current = new AddinManager( current );
		}

		public static void Restore()
		{
			current = current.PriorState;
		}
		#endregion

		#region SuiteBuilder Methods
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// any builders to use in building a TestSuite
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		public static bool CanBuildFrom( Type type )
		{
			return current.CanBuildFrom( type );
		}

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite or null</returns>
		public static TestSuite BuildFrom( Type type )
		{
			return current.BuildFrom( type );
		}
		#endregion

		#region TestCaseBuilder Methods
		/// <summary>
		/// Examine a method and determine if it is suitable for
		/// any builders to use in building a TestCase
		/// </summary>
		/// <param name="method">The method to be used</param>
		/// <returns>True if the method can be used to build a TestCase</returns>
		public static bool CanBuildFrom( MethodInfo method )
		{
			return current.CanBuildFrom( method );
		}

		/// <summary>
		/// Build a TestCase from the method provided.
		/// </summary>
		/// <param name="method">The method to be used</param>
		/// <returns>A TestCase or null</returns>
		public static TestCase BuildFrom( MethodInfo method )
		{
			return current.BuildFrom( method );
		}
		#endregion
	}
}

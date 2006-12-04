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

namespace NUnit.Core
{
	/// <summary>
	/// Enumeration identifying a common language 
	/// runtime implementation.
	/// </summary>
	public enum RuntimeType
	{
		/// <summary>Microsoft .NET Framework</summary>
		Net,
		/// <summary>Microsoft .NET Compact Framework</summary>
		NetCF,
		/// <summary>Microsoft Shared Source CLI</summary>
		SSCLI,
		/// <summary>Mono</summary>
		Mono
	}

	/// <summary>
	/// RuntimeFramework represents a particular version
	/// of a common language runtime implementation.
	/// </summary>
	public sealed class RuntimeFramework
	{
		private RuntimeType runtime;
		private Version version;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="runtime">The runtime type of the framework</param>
		/// <param name="version">The version of the framework</param>
		public RuntimeFramework( RuntimeType runtime, Version version )
		{
			this.runtime = runtime;
			this.version = version;
		}

		/// <summary>
		/// Static method to return a RuntimeFramework object
		/// for the frameowrk that is currently in use.
		/// </summary>
		public static RuntimeFramework CurrentFramework
		{
			get 
			{ 
				RuntimeType runtime = Type.GetType( "Mono.Runtime", false ) != null
					? RuntimeType.Mono : RuntimeType.Net;

				return new RuntimeFramework( runtime, Environment.Version );
			}
		}

		/// <summary>
		/// The type of this runtime framework
		/// </summary>
		public RuntimeType Runtime
		{
			get { return runtime; }
		}

		/// <summary>
		/// The version of this runtime framework
		/// </summary>
		public Version Version
		{
			get { return version; }
		}

		/// <summary>
		/// Gets a display string for the particular framework version
		/// </summary>
		/// <returns>A string used to display the framework in use</returns>
		public string GetDisplayName()
		{
			if ( runtime == RuntimeType.Mono )
			{
				Type monoRuntimeType = Type.GetType( "Mono.Runtime", false );
				MethodInfo getDisplayNameMethod = monoRuntimeType.GetMethod(
					"GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding );
				if ( getDisplayNameMethod != null )
					return (string)getDisplayNameMethod.Invoke( null, new object[0] );
			}

			return runtime.ToString() + " " + Version.ToString();
		}
	}
}

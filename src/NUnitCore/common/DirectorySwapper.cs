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

namespace NUnit.Core
{
	/// <summary>
	/// Utility class that allows changing the current directory 
	/// for the duration of some lexical scope and guaranteeing
	/// that it will be restored upon exit.
	/// 
	/// Use it as follows:
	///    using( new DirectorySwapper( @"X:\New\Path" )
	///    {
	///        // Code that operates in the new current directory
	///    }
	///    
	/// Instantiating DirectorySwapper without a path merely
	/// saves the current directory, but does not change it.
	/// </summary>
	public class DirectorySwapper : IDisposable
	{
		private string savedDirectoryName;

		public DirectorySwapper() : this( null ) { }

		public DirectorySwapper( string directoryName )
		{
			savedDirectoryName = Environment.CurrentDirectory;
			
			if ( directoryName != null && directoryName != string.Empty )
				Environment.CurrentDirectory = directoryName;
		}

		public void Dispose()
		{
			Environment.CurrentDirectory = savedDirectoryName;
		}
	}
}

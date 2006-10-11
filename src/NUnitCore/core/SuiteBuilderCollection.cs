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
using System.Collections;

namespace NUnit.Core
{
	/// <summary>
	/// SuiteBuilderCollection maintains a list of SuiteBuilders and
	/// implements the ISuiteBuilder interface itself, passing calls 
	/// on to the individual builders.
	/// 
	/// The collection is actually a stack, so SuiteBuilders added to
	/// the collection take precedence over those added earlier, 
	/// allowing a user to temporarily replace a builder.
	/// </summary>
	public class SuiteBuilderCollection : Stack, ISuiteBuilder
	{
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SuiteBuilderCollection() { }

		/// <summary>
		/// Construct from another SuiteBuilderCollection, copying its contents.
		/// </summary>
		/// <param name="other">The SuiteBuilderCollection to copy</param>
		public SuiteBuilderCollection( SuiteBuilderCollection other ) : base( other ) { }
		#endregion

		#region ISuiteBuilder Members

		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// any SuiteBuilder to use in building a TestSuite
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		public bool CanBuildFrom(Type type)
		{
			foreach( ISuiteBuilder builder in this )
				if ( builder.CanBuildFrom( type ) )
					return true;
			return false;
		}

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite or null</returns>
		public Test BuildFrom(Type type)
		{
			foreach( ISuiteBuilder builder in this )
				if ( builder.CanBuildFrom( type ) )
					return builder.BuildFrom( type );
			return null;
		}

		#endregion

		#region Other Public Methods
		/// <summary>
		/// Add a SuiteBuilder to the collection - provided
		/// as a synonym for Push.
		/// </summary>
		/// <param name="builder">The builder to add</param>
		public void Add( ISuiteBuilder builder )
		{
			Push( builder );
		}
		#endregion
	}
}

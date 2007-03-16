// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using NUnit.Framework.Constraints;

namespace NUnit.Framework.SyntaxHelpers
{
	/// <summary>
	/// Summary description for Has.
	/// </summary>
	public class Has : SyntaxHelper
	{
        /// <summary>
        /// Returns a new PropertyConstraint
        /// </summary>
		public static Constraint Property( string name, object expected )
		{
			return new PropertyConstraint( name, expected );
		}

        /// <summary>
        /// Returns a new PropertyConstraint for the Length property
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
		public static Constraint Length( int length )
		{
			return new PropertyConstraint( "Length", length );
		}
	}
}

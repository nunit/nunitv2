// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// Summary description for MaxTimeAttribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple=false, Inherited=false )]
	public sealed class MaxTimeAttribute : Attribute
	{
		private readonly int maxTime;

		public MaxTimeAttribute( int maxTime )
		{
			this.maxTime = maxTime;
		}

		public int MaxTime
		{
			get { return maxTime; }
		}
	}
}

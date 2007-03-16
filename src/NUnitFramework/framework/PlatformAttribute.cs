// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Framework
{
	/// <summary>
	/// PlatformAttribute is used to mark a test fixture or an
	/// individual method as applying to a particular platform only.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=false)]
	public sealed class PlatformAttribute : Attribute
	{
		private string include;
		private string exclude;
		private string reason;

		/// <summary>
		/// Constructor with no platforms specified, for use
		/// with named property syntax.
		/// </summary>
		public PlatformAttribute() { }

		/// <summary>
		/// Constructor taking one or more platforms
		/// </summary>
		/// <param name="platforms">Comma-deliminted list of platforms</param>
		public PlatformAttribute( string platforms )
		{
			this.include = platforms;
		}

		/// <summary>
		/// Name of the platform that is needed in order for
		/// a test to run. Multiple platforms may be given,
		/// separated by a comma.
		/// </summary>
		public string Include
		{
			get { return this.include; }
			set { include = value; }
		}

		/// <summary>
		/// Name of the platform to be excluded. Multiple platforms
		/// may be given, separated by a comma.
		/// </summary>
		public string Exclude
		{
			get { return this.exclude; }
			set { this.exclude = value; }
		}

		/// <summary>
		/// The reason for including or excluding the test
		/// </summary>
		public string Reason
		{
			get { return reason; }
			set { reason = value; }
		}
	}
}

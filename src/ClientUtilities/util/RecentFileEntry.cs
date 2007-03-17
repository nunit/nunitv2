// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;

namespace NUnit.Util
{
	public class RecentFileEntry
	{
		public static readonly char Separator = ',';

		private string path;
		
		private Version clrVersion;

		public RecentFileEntry( string path )
		{
			this.path = path;
			this.clrVersion = Environment.Version;
		}

		public RecentFileEntry( string path, Version clrVersion )
		{
			this.path = path;
			this.clrVersion = clrVersion;
		}

		public string Path
		{
			get { return path; }
		}

		public Version CLRVersion
		{
			get { return clrVersion; }
		}

		public bool Exists
		{
			get { return path != null && System.IO.File.Exists( path ); }
		}

		public bool IsCompatibleCLRVersion
		{
			get { return clrVersion.Major <= Environment.Version.Major; }
		}

		public override string ToString()
		{
			return Path + Separator + CLRVersion.ToString();
		}

		public static RecentFileEntry Parse( string text )
		{
			int sepIndex = text.IndexOf( Separator );
			if ( sepIndex < 0 )
				return new RecentFileEntry( text );
			else
				return new RecentFileEntry( text.Substring( 0, sepIndex ), 
					new Version( text.Substring( sepIndex + 1 ) ) );
		}
	}
}

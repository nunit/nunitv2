using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
	public abstract class PathConstraint : Constraint
	{
		protected string expected;

		protected PathConstraint( string expected )
		{
			this.expected = expected;
		}

		protected string Canonicalize( string path )
		{
			ArrayList parts = new ArrayList(
				path.Split( directorySeparatorChar, altDirectorySeparatorChar ) );

			for( int index = 0; index < parts.Count; )
			{
				string part = (string)parts[index];
		
				switch( part )
				{
					case ".":
						parts.RemoveAt( index );
						break;
				
					case "..":
						parts.RemoveAt( index );
						if ( index > 0 )
							parts.RemoveAt( --index );
						break;
					default:
						index++;
						break;
				}
			}
	
			return String.Join( directorySeparatorChar.ToString(), 
				(string[])parts.ToArray( typeof( string ) ) );
		}

		protected bool IsSamePath( string path1, string path2 )
		{
			return string.Compare( Canonicalize( expected ), Canonicalize( (string)actual ), treatAsWindows ) == 0;
		}

		protected bool IsSubPath( string path1, string path2, bool allowSamePath )
		{
			path1 = Canonicalize( path1 );
			path2 = Canonicalize( path2 );

			int length1 = path1.Length;
			int length2 = path2.Length;

			// if path1 is longer, then path2 can't be under it
			if ( length1 > length2 )
				return false;

			// if lengths are the same, check for equality
			if ( length1 == length2 )
				return allowSamePath && string.Compare( path1, path2, treatAsWindows ) == 0;

			// path 2 is longer than path 1: see if initial parts match
			if ( string.Compare( path1, path2.Substring( 0, length1 ), treatAsWindows ) != 0 )
				return false;
			
			// must match through or up to a directory separator boundary
			return	path2[length1-1] == directorySeparatorChar ||
				path2[length1] == directorySeparatorChar;
		}
	}

	/// <summary>
	/// Summary description for SamePathConstraint.
	/// </summary>
	public class SamePathConstraint : PathConstraint
	{
		public SamePathConstraint( string expected )
			: base( expected ) { }

		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is string) )
				return false;

			return IsSamePath( expected, (string)actual );
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( "Path matching" );
			writer.WriteExpectedValue( expected );
		}
	}

	public class SubPathConstraint : PathConstraint
	{
		private bool allowSamePath;

		public SubPathConstraint( string expected ) : this( expected, true ) { }

		public SubPathConstraint( string expected, bool allowSamePath ) : base( expected ) 
		{
			this.allowSamePath = allowSamePath;
		}

		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is string) )
				return false;

			return IsSubPath( expected, (string) actual, allowSamePath );
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( allowSamePath ? "Path under or matching" : "Path under" );
			writer.WriteExpectedValue( expected );
		}
	}
}

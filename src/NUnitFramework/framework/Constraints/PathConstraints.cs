using System;
using System.IO;
using System.Collections;

namespace NUnit.Framework.Constraints
{
	/// <summary>
	/// PathConstraint serves as the abstract base of constraints
	/// that operate on paths and provides several helper methods.
	/// </summary>
	public abstract class PathConstraint : Constraint
	{
		/// <summary>
		/// The expected path used in the constraint
		/// </summary>
		protected string expected;

		/// <summary>
		/// Construct a PathConstraint for a give expected path
		/// </summary>
		/// <param name="expected">The expected path</param>
		protected PathConstraint( string expected )
		{
			this.expected = expected;
		}

		/// <summary>
		/// Canonicalize the provided path
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The path in standardized form</returns>
		protected string Canonicalize( string path )
		{
			ArrayList parts = new ArrayList(
				path.Split( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar ) );

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
	
			return String.Join( Path.DirectorySeparatorChar.ToString(), 
				(string[])parts.ToArray( typeof( string ) ) );
		}

		/// <summary>
		/// Test whether two paths are the same
		/// </summary>
		/// <param name="path1">The first path</param>
		/// <param name="path2">The second path</param>
		/// <returns></returns>
		protected bool IsSamePath( string path1, string path2 )
		{
			return string.Compare( Canonicalize( expected ), Canonicalize( (string)actual ), caseInsensitive ) == 0;
		}

		/// <summary>
		/// Test whether one path is the same as or under another path
		/// </summary>
		/// <param name="path1">The first path - supposed to be the parent path</param>
		/// <param name="path2">The second path - supposed to be the child path</param>
		/// <returns></returns>
		protected bool IsSamePathOrUnder( string path1, string path2 )
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
				return string.Compare( path1, path2, caseInsensitive ) == 0;

			// path 2 is longer than path 1: see if initial parts match
			if ( string.Compare( path1, path2.Substring( 0, length1 ), caseInsensitive ) != 0 )
				return false;
			
			// must match through or up to a directory separator boundary
			return	path2[length1-1] == Path.DirectorySeparatorChar ||
				path2[length1] == Path.DirectorySeparatorChar;
		}
	}

	/// <summary>
	/// Summary description for SamePathConstraint.
	/// </summary>
	public class SamePathConstraint : PathConstraint
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SamePathConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected path</param>
		public SamePathConstraint( string expected ) : base( expected ) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is string) )
				return false;

			return IsSamePath( expected, (string)actual );
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( "Path matching" );
			writer.WriteExpectedValue( expected );
		}
	}

    /// <summary>
    /// SamePathOrUnderConstraint tests that one path is under another
    /// </summary>
	public class SamePathOrUnderConstraint : PathConstraint
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SamePathOrUnderConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected path</param>
		public SamePathOrUnderConstraint( string expected ) : base( expected ) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is string) )
				return false;

			return IsSamePathOrUnder( expected, (string) actual );
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate( "Path under or matching" );
			writer.WriteExpectedValue( expected );
		}
	}
}

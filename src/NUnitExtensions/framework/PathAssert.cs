using System;
using System.IO;
using System.Collections;

namespace NUnit.Framework.Extensions
{
	/// <summary>
	/// Basic asserts on paths.
	/// </summary>
	public class PathAssert
	{
		#region SamePath
		/// <summary>
		/// Verifies that two paths are the same. If not, then an 
		/// <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		static public void SamePath( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new SamePathAsserter( expected, actual, message, args ) );
		}

		/// <summary>
		/// Verifies that two paths are the same. If not, then an 
		/// <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message that will be displayed on failure</param>
		static public void SamePath( string expected, string actual, string message )
		{
			SamePath( expected, actual, message, null );
		}

		/// <summary>
		/// Verifies that two paths are the same. If not, then an 
		/// <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		static public void SamePath( string expected, string actual )
		{
			SamePath( expected, actual, string.Empty, null );
		}
		#endregion

		#region NotSamePath
		/// <summary>
		/// Verifies that two paths are not the same. If the are,
		/// then an <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message that will be displayed on failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		static public void NotSamePath( string expected, string actual, string message, params object[] args )
		{
			Assert.DoAssert( new NotSamePathAsserter( expected, actual, message, args ) );
		}

		/// <summary>
		/// Verifies that two paths are not the same. If the are,
		/// then an <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message that will be displayed on failure</param>
		static public void NotSamePath( string expected, string actual, string message )
		{
			NotSamePath( expected, actual, message, null );
		}

		/// <summary>
		/// Verifies that two paths are not the same. If the are,
		/// then an <see cref="AssertionException"/> is thrown.
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		static public void NotSamePath( string expected, string actual )
		{
			NotSamePath( expected, actual, string.Empty, null );
		}
		#endregion
	}

	#region PathAsserter
	public abstract class PathAsserter : StringAsserter
	{
		/// <summary>
		/// Constructs a PathAsserter for two paths
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public PathAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }
	
		protected bool OnWindows
		{
			get { return Path.DirectorySeparatorChar == '\\'; }
		}

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
	
			return String.Join( Path.DirectorySeparatorChar.ToString(), (string[])parts.ToArray( typeof( string ) ) );
		}

		protected bool SamePath( string path1, string path2 )
		{
			return string.Compare( Canonicalize( path1 ), Canonicalize( path2 ), OnWindows ) == 0;
		}
	}
	#endregion

	#region SamePathAsserter
	public class SamePathAsserter : PathAsserter
	{
		/// <summary>
		/// Constructs a SamePathAsserter for two paths
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public SamePathAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }
	
		public override bool Test()
		{
			return SamePath( expected, actual );
		}
	}
	#endregion

	#region NotSamePathAsserter
	public class NotSamePathAsserter : PathAsserter
	{
		/// <summary>
		/// Constructs a NotSamePathAsserter for two paths
		/// </summary>
		/// <param name="expected">The expected path</param>
		/// <param name="actual">The actual path</param>
		/// <param name="message">The message to issue on failure</param>
		/// <param name="args">Arguments to apply in formatting the message</param>
		public NotSamePathAsserter( string expected, string actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }
	
		public override bool Test()
		{
			return !SamePath( expected, actual );
		}
	}
	#endregion
}

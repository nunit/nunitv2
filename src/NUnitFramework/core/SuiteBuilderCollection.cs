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
		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			foreach( ISuiteBuilder builder in this )
				if ( builder.CanBuildFrom( type ) )
					return builder.BuildFrom( type, assemblyKey );
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

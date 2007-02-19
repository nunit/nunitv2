// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;

namespace NUnit.Core.Extensibility
{
	/// <summary>
	/// SuiteBuilderCollection is an ExtensionPoint for SuiteBuilders and
	/// implements the ISuiteBuilder interface itself, passing calls 
	/// on to the individual builders.
	/// 
	/// The builders are added to the collection by inserting them at
	/// the start, as to take precedence over those added earlier. 
	/// </summary>
	public class SuiteBuilderCollection : ISuiteBuilder, IExtensionPoint
	{
		private ArrayList builders = new ArrayList();

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SuiteBuilderCollection() { }

		/// <summary>
		/// Construct from another SuiteBuilderCollection, copying its contents.
		/// </summary>
		/// <param name="other">The SuiteBuilderCollection to copy</param>
		public SuiteBuilderCollection( SuiteBuilderCollection other ) 
		{
			builders.AddRange( other.builders );
		}
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
			foreach( ISuiteBuilder builder in builders )
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
			foreach( ISuiteBuilder builder in builders )
				if ( builder.CanBuildFrom( type ) )
					return builder.BuildFrom( type );
			return null;
		}

		#endregion

		#region IExtensionPoint Members
		public string Name
		{
			get { return "SuiteBuilders"; }
		}

        public IExtensionHost Host
        {
            get { return CoreExtensions.Host; }
        }

		public void Install(object extension)
		{
			ISuiteBuilder builder = extension as ISuiteBuilder;
			if ( builder == null )
				throw new ArgumentException( 
					extension.GetType().FullName + " is not an ITestCaseBuilder", "exception" );

			builders.Insert( 0, builder );
		}

		public void Remove( object extension )
		{
			builders.Remove( extension );
		}
		#endregion
	}
}

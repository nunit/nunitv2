using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// TestCaseBuilderCollection maintains a list of TestCaseBuilders 
	/// and implements the ISuiteBuilder interface itself, passing calls 
	/// on to the individual builders.
	/// 
	/// The collection is actually a stack, so TestCaseBuilders added to
	/// the collection take precedence over those added earlier, 
	/// allowing a user to temporarily replace a builder.
	/// </summary>
	public class TestCaseBuilderCollection : Stack, ITestCaseBuilder
	{
		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestCaseBuilderCollection() { }

		/// <summary>
		/// Construct from another TestCaseBuilderCollection, copying its contents.
		/// </summary>
		/// <param name="other">The TestCaseBuilderCollection to copy</param>
		public TestCaseBuilderCollection( TestCaseBuilderCollection other ) : base( other ) { }
		#endregion
		
		#region ITestCaseBuilder Members

		/// <summary>
		/// Examine the method and determine if it is suitable for
		/// any TestCaseBuilder to use in building a TestCase
		/// </summary>
		/// <param name="method">The method to be used as a test case</param>
		/// <returns>True if the type can be used to build a TestCase</returns>
		public bool CanBuildFrom( MethodInfo method )
		{
			foreach( ITestCaseBuilder builder in this )
				if ( builder.CanBuildFrom( method ) )
					return true;
			return false;
		}

		/// <summary>
		/// Build a TestCase from the method provided.
		/// </summary>
		/// <param name="method">The method to be used</param>
		/// <returns>A TestCase or null</returns>
		public Test BuildFrom( MethodInfo method )
		{
			foreach( ITestCaseBuilder builder in this )
			{
				if ( builder.CanBuildFrom( method ) )
					return builder.BuildFrom( method );
			}

			return null;
		}
		#endregion

		#region Other Public Methods
		/// <summary>
		/// Add a TestCaseBuilder to the collection - provided
		/// as a synonym for Push.
		/// </summary>
		/// <param name="builder">The builder to add</param>
		public void Add( ITestCaseBuilder builder )
		{
			Push( builder );
		}
		#endregion
	}
}

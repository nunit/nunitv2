using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// The Builtins class groups together all builtin test suite
	/// and test case builders for access through static methods.
	/// 
	/// Although builtin builders use the same interfaces as 
	/// addins, they are not located by means of an attribute.
	/// To add a new builtin builder, modify the static 
	/// constructor of this class to add an instance of the
	/// builder to the appropriate builder collection.
	/// </summary>
	public class Builtins
	{
		#region Static Fields
		/// <summary>
		/// Collection of test suite Builders 
		/// </summary>
		private static SuiteBuilderCollection suiteBuilders = new SuiteBuilderCollection();
		
		/// <summary>
		/// Collection of test case builders
		/// </summary>
		private static TestCaseBuilderCollection testBuilders = new TestCaseBuilderCollection();
		#endregion

		#region Static Constructor
		/// <summary>
		/// Static constructor initializes all the builtin builders
		/// </summary>
		static Builtins()
		{
			// Add builtin SuiteBuilders
			suiteBuilders.Add( new Builders.NUnitTestFixtureBuilder() );
			suiteBuilders.Add( new Builders.CSUnitTestFixtureBuilder() );
			suiteBuilders.Add( new Builders.VstsTestFixtureBuilder() );
			suiteBuilders.Add( new Builders.SetUpFixtureBuilder() );

			//Add builtin TestCaseBuilders
			testBuilders.Add( new Builders.NUnitTestCaseBuilder() );
		}
		#endregion

		#region SuiteBuilder Methods
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// any builders to use in building a TestSuite
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		public static bool CanBuildFrom( Type type )
		{
			return suiteBuilders.CanBuildFrom( type );
		}

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite or null</returns>
		public static TestSuite BuildFrom( Type type, int assemblyKey )
		{
			return suiteBuilders.BuildFrom( type, assemblyKey );
		}
		#endregion

		#region TestCaseBuilder Methods
		/// <summary>
		/// Examine a method and determine if it is suitable for
		/// any builders to use in building a TestCase
		/// </summary>
		/// <param name="method">The method to be used</param>
		/// <returns>True if the method can be used to build a TestCase</returns>
		public static bool CanBuildFrom( MethodInfo method )
		{
			return testBuilders.CanBuildFrom( method );
		}

		/// <summary>
		/// Build a TestCase from the method provided.
		/// </summary>
		/// <param name="method">The method to be used</param>
		/// <returns>A TestCase or null</returns>
		public static TestCase BuildFrom( MethodInfo method )
		{
			return testBuilders.BuildFrom( method );
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Private constructor to prevent instantiation
		/// </summary>
		private Builtins() { }
		#endregion
	}
}

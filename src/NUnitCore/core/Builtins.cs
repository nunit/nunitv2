#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

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

		private static TestDecoratorCollection testDecorators = new TestDecoratorCollection();
		#endregion

		#region Static Constructor
		/// <summary>
		/// Static constructor initializes all the builtin builders
		/// </summary>
		static Builtins()
		{
			// Define NUnit Framework
			TestFramework.Register( "NUnit", "nunit.framework" );

			// Add builtin SuiteBuilders
			suiteBuilders.Add( new Builders.NUnitTestFixtureBuilder() );
            //suiteBuilders.Add( new Builders.CSUnitTestFixtureBuilder() );
            //suiteBuilders.Add( new Builders.VstsTestFixtureBuilder() );
			suiteBuilders.Add( new Builders.SetUpFixtureBuilder() );

			//Add builtin TestCaseBuilders
			testBuilders.Add( new Builders.NUnitTestCaseBuilder() );

			//Add builtin TestDecorators
			//testDecorators.Add( new IgnoreDecorator( "NUnit.Framework.IgnoreAttribute" ) );
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
		public static TestSuite BuildFrom( Type type )
		{
			return suiteBuilders.BuildFrom( type );
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

		#region TestDecorator Methods
		public static TestCase Decorate( TestCase testCase, MethodInfo method )
		{
			return testDecorators.Decorate( testCase, method );
		}

		public static TestSuite Decorate( TestSuite suite, Type type )
		{
			return testDecorators.Decorate( suite, type );
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

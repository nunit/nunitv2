using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Core.Builders;

namespace NUnit.Extensions.Tests
{
	#region MockSuiteExtension

	/// <summary>
	/// MockSuiteBuilder knows how to build a MockSuiteExtension
	/// </summary>
	[SuiteBuilder]
	public class MockSuiteBuilder : ISuiteBuilder
	{	
		public MockSuiteBuilder()
		{
			//
			// TODO: Add constructor logic here	//
		}

		#region ISuiteBuilder Members

		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			if ( CanBuildFrom( type ) )
				return new MockSuiteExtension( type, assemblyKey );
			return null;
		}

		public bool CanBuildFrom(Type type)
		{
			return type.IsDefined( typeof( MockSuiteExtensionAttribute ), false );
		}

		#endregion
	}

	/// <summary>
	/// MockSuiteExtensionAttribute is used to identify a MockSuiteExtension fixture
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class MockSuiteExtensionAttribute : Attribute
	{
	}

	/// <summary>
	/// MockSuiteExtension extends test suite and creates a fixture
	/// that runs every test starting with "FunkyTest..." 
	/// </summary>
	class MockSuiteExtension : TestSuite
	{
		public MockSuiteExtension( Type fixtureType, int assemblyKey ) 
			: base( fixtureType, assemblyKey )
		{
			// NOTE: Since we are inheriting from TestSuite, the object
			// will not be created for us, so for now we do it here.
			this.Fixture = Reflect.Construct( fixtureType );

			foreach( MethodInfo method in fixtureType.GetMethods( 
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly ) )
			{
				if ( method.Name.StartsWith( "FunkyTest" ) )
				{
					// NOTE: Do NOT use Tests.Add since it doesn't
					// set the parent up correctly.
					this.Add( new TemplateTestCase( method ) );
				}
			}
		}
	}
	
	/// <summary>
	/// Test class that demonstrates MockSuiteExtension
	/// </summary>
	[MockSuiteExtension]
	public class MockSuiteExtensionTests
	{
		public void FunkyTest1()
		{
			Console.WriteLine( "Hello from funky test 1" );
		}

		public void FunkyTest2()
		{
			Console.WriteLine( "Hello from funky test 2" );
		}

		public void NotATest()
		{
			Console.WriteLine( "I shouldn't be called!" );
		}
	}

	#endregion

	#region MockFixtureExtension

	/// <summary>
	/// MockFixtureExtensionBuilder knows how to build 
	/// a MockFixtureExtension.
	/// </summary>
	[SuiteBuilder]
	public class MockFixtureExtensionBuilder : NUnitTestFixtureBuilder
	{	
		public MockFixtureExtensionBuilder()
		{
			//
			// TODO: Add constructor logic here	//
		}

		#region ISuiteBuilder Members

		public override TestSuite BuildFrom(Type type, int assemblyKey)
		{
			if ( CanBuildFrom( type ) )
				return base.BuildFrom( type, assemblyKey );
			return null;
		}

		public override bool CanBuildFrom(Type type)
		{
			return type.IsDefined( typeof( MockFixtureExtensionAttribute ), false );
		}
		#endregion
	}

	/// <summary>
	/// MockFixtureExtensionAttribute is used to identify a MockFixtureExtension class
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class MockFixtureExtensionAttribute : Attribute
	{
	}

	/// <summary>
	/// MockFixtureExtension extends test fixture and adds a custom setup
	/// before running TestFixtureSetUp and after running TestFixtureTearDown
	/// </summary>
	class MockFixtureExtension : NUnitTestFixture
	{
		public MockFixtureExtension( Type fixtureType, int assemblyKey ) 
			: base( fixtureType, assemblyKey )
		{
			// NOTE: Since we are inheriting from TestFixture we don't 
			// have to do anything if we don't want to. Our tests will 
			// be recognized in the normal way by TestFixture, based
			// on the presence of the Test attribute.
			//
			// Just to have something to do, we override DoSetUp and DoTearDown
			// below to do some special processing before and after the normal
			// TestFixtureSetUp and TestFixtureTearDown
		}

		public override void DoOneTimeSetUp(TestResult suiteResult)
		{
			Console.WriteLine( "Extended Fixture SetUp called" );
			base.DoOneTimeSetUp (suiteResult);
		}

		public override void DoOneTimeTearDown(TestResult suiteResult)
		{
			base.DoOneTimeTearDown (suiteResult);
			Console.WriteLine( "Extended Fixture TearDown called" );
		}
	}

	/// <summary>
	/// Test class that demonstrates MockFixtureExtension
	/// </summary>
	[MockFixtureExtension]
	public class MockFixtureExtensionTests
	{
		[TestFixtureSetUp]
		public void SetUpTests()
		{
			Console.WriteLine( "TestFixtureSetUp called" );
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			Console.WriteLine( "TestFixtureTearDown called" );
		}

		[Test]
		public void SomeTest()
		{
			Console.WriteLine( "Hello from some test" );
		}

		[Test]
		public void AnotherTest()
		{
			Console.WriteLine( "Hello from another test" );
		}

		public void NotATest()
		{
			Console.WriteLine( "I shouldn't be called!" );
		}
	}
	#endregion

}

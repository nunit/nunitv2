using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Extensions.Tests
{
	#region MockSuiteBuilder class

	/// <summary>
	/// MockSuiteBuilder knows how to build three different
	/// types of suite extensions:
	/// 1. MockSuiteExtension extends TestSuite
	/// 2. MockFixtureExtension extends TestFixture
	/// 3. SetUpFixture extends NamespaceSuite
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
			if ( type.IsDefined( typeof( MockSuiteExtensionAttribute ), false ) )
				return new MockSuiteExtension( type, assemblyKey );
			else if ( type.IsDefined( typeof( MockFixtureExtensionAttribute ), false ) )
				return new MockFixtureExtension( type, assemblyKey );
#if SETUP_FIXTURE
			else if ( type.IsDefined( typeof( SetUpFixtureAttribute ), false ) )
				return new SetUpFixture( type );
#endif
			throw new ArgumentException( "MockSuiteBuilder cannot use type " + type.FullName );
		}

		public bool CanBuildFrom(Type type)
		{
			return type.IsDefined( typeof( MockSuiteExtensionAttribute ), false )
				|| type.IsDefined( typeof( MockFixtureExtensionAttribute ), false )
#if SETUP_FIXTURE
				|| type.IsDefined( typeof( SetUpFixtureAttribute ), false )
#endif
				;
		}

		#endregion
	}

	#endregion

	#region MockSuiteExtension

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
					this.Add( new NormalTestCase( fixtureType, method ) );
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
	class MockFixtureExtension : TestFixture
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

		public override void DoSetUp(TestResult suiteResult)
		{
			Console.WriteLine( "Extended Fixture SetUp called" );
			base.DoSetUp (suiteResult);
		}

		public override void DoTearDown(TestResult suiteResult)
		{
			base.DoTearDown (suiteResult);
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

	#region SetUpFixture
#if SETUP_FIXTURE

	// TODO: SetUpFixture requires modifications to TestSuiteBuilder
	// if it is to work. Probably, the basic support for having
	// setup and teardown on a namespace suite should be moved to
	// the NUnit core, and extensions should build on that.

	/// <summary>
	/// SetUpFixtureAttribute is used to identify a SetUpFixture
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class SetUpFixtureAttribute : Attribute
	{
	}

	/// <summary>
	/// SetUpFixture extends NamespaceSuite and allows a namespace to have
	/// a TestFixtureSetup and TestFixtureTearDown.
	/// </summary>
	public class SetUpFixture : NamespaceSuite
	{
		public SetUpFixture( Type type ) : base( type.Namespace )
		{
			this.fixtureType = type;

			// NOTE: Once again, since we are not inheriting from TestFixture,
			// no automatic construction is performed for us, so we do it here.
			this.Fixture = Reflect.Construct( fixtureType );

			this.fixtureSetUp = Reflect.GetMethod( fixtureType, typeof( NUnit.Framework.TestFixtureSetUpAttribute ) );
			this.fixtureTearDown = Reflect.GetMethod( fixtureType, typeof( NUnit.Framework.TestFixtureTearDownAttribute ) );
		}

		public override void DoSetUp(TestResult suiteResult)
		{
			if ( fixtureSetUp != null )
				Reflect.InvokeMethod( fixtureSetUp, this.Fixture );

			base.DoSetUp (suiteResult);
		}

		public override void DoTearDown(TestResult suiteResult)
		{
			base.DoTearDown (suiteResult);
			if (fixtureTearDown != null )
				Reflect.InvokeMethod( fixtureTearDown, this.Fixture );
		}


	}

	/// <summary>
	/// Test class for SetUpFixture
	/// </summary>
	[SetUpFixture]
	public class ExtensionsNamespaceSetUpFixture
	{
		[TestFixtureSetUp]
		public void DoNamespaceSetUp()
		{
			Console.WriteLine( "Namespace SetUp called" );
		}

		[TestFixtureTearDown]
		public void DoNamespaceTearDown()
		{
			Console.WriteLine( "Namespace TearDown called" );
		}
	}
#endif

	#endregion
}

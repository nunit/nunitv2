using System;
using System.Text;
using System.Collections;
using System.Reflection;
using NUnit.Framework;

namespace NUnit.Core
{
	/// <summary>
	/// A TestSuite that wraps a class marked with TestFixtureAttribute
	/// </summary>
	public class TestFixture : TestSuite
	{
		private static readonly Type TestFixtureType = typeof( TestFixtureAttribute );
		private static readonly Type TestType = typeof( TestAttribute );
		private static readonly Type SetUpType = typeof( SetUpAttribute );
		private static readonly Type TearDownType = typeof( TearDownAttribute );
		private static readonly Type FixtureSetUpType = typeof( TestFixtureSetUpAttribute );
		private static readonly Type FixtureTearDownType = typeof( TestFixtureTearDownAttribute );
		private static readonly Type ExplicitType = typeof( ExplicitAttribute );
		private static readonly Type CategoryType = typeof( CategoryAttribute );
		private static readonly Type IgnoreType = typeof( IgnoreAttribute );
		private static readonly Type ExpectedExceptionType = typeof( ExpectedExceptionAttribute );
		private static readonly Type PlatformType = typeof( PlatformAttribute );

		private const string FIXTURE_SETUP_FAILED = "Fixture setup failed";

		#region Constructors

		public TestFixture( object fixture ) : base( fixture, 0 )
		{
			Initialize();
		}

		public TestFixture( object fixture, int assemblyKey ) : base( fixture, assemblyKey )
		{
			Initialize();
		}

		public TestFixture( Type fixtureType ) : base( fixtureType, 0 )
		{
			Initialize();
		}

		public TestFixture( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey )
		{
			Initialize();
		}

		private void Initialize()
		{
			try
			{
				if ( Reflect.GetConstructor( fixtureType ) == null )
					throw new InvalidTestFixtureException(fixtureType.FullName + " does not have a valid constructor");
			
				CheckSetUpTearDownMethod( fixtureType, SetUpType );
				CheckSetUpTearDownMethod( fixtureType, TearDownType );
				CheckSetUpTearDownMethod( fixtureType, FixtureSetUpType );
				CheckSetUpTearDownMethod( fixtureType, FixtureTearDownType );

				object[] attributes = fixtureType.GetCustomAttributes( CategoryType, false );
				IList categories = new ArrayList();

				foreach(CategoryAttribute attribute in attributes) 
					categories.Add(attribute.Name);
			
				CategoryManager.Add( categories );
				this.Categories = categories;

				this.fixtureSetUp = Reflect.GetMethod( fixtureType, FixtureSetUpType );
				this.fixtureTearDown = Reflect.GetMethod( fixtureType, FixtureTearDownType );

				this.IsExplicit = Reflect.HasAttribute( fixtureType, ExplicitType, false );

				attributes = fixtureType.GetCustomAttributes( PlatformType, false );
				if ( attributes.Length > 0 )
				{
					PlatformHelper helper = new PlatformHelper();
					if ( !helper.IsPlatformSupported( (PlatformAttribute[])attributes ) )
					{
						this.ShouldRun = false;
						this.IgnoreReason = "Not running on correct platform";
					}
				}

				IgnoreAttribute ignoreAttribute = (IgnoreAttribute)
					Reflect.GetAttribute( fixtureType, IgnoreType, false );
				if ( ignoreAttribute != null )
				{
					this.ShouldRun = false;
					this.IgnoreReason = ignoreAttribute.Reason;
				}
		
				TestFixtureAttribute fixtureAttribute 
					= (TestFixtureAttribute)Reflect.GetAttribute( fixtureType, TestFixtureType, true );

				// Some of our tests create a fixture without the attribute
				if ( fixtureAttribute != null )
					this.Description = fixtureAttribute.Description;

				MethodInfo [] methods = fixtureType.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static|BindingFlags.NonPublic);
				foreach(MethodInfo method in methods)
				{
					TestCase testCase = TestCaseBuilder.Make( fixtureType, method );
					if(testCase != null)
					{
						testCase.AssemblyKey = this.AssemblyKey;
						this.Add( testCase );
					}
				}

				if( this.CountTestCases() == 0 )
				{
					this.ShouldRun = false;
					this.IgnoreReason = this.Name + " does not have any tests";
				}
			}
			catch( InvalidTestFixtureException exception )
			{
				this.ShouldRun = false;
				this.IgnoreReason = exception.Message;
			}
		}

		private void CheckSetUpTearDownMethod( Type fixtureType, Type attributeType )
		{
			MethodInfo theMethod = Reflect.GetUniqueMethod( fixtureType, attributeType );

			if ( theMethod != null )
			{
				if ( theMethod.IsStatic ||
					theMethod.IsAbstract ||
					!theMethod.IsPublic && !theMethod.IsFamily ||
					theMethod.GetParameters().Length != 0 ||
					!theMethod.ReturnType.Equals( typeof(void) ) )
				{
					throw new InvalidTestFixtureException("Invalid SetUp or TearDown method signature");
				}
			}
		} 

		#endregion

		#region Properties

		public override bool IsFixture
		{
			get { return true; }
		}

		#endregion

		public override void DoSetUp( TestResult suiteResult )
		{
			try 
			{
				if ( Fixture == null )
					Fixture = Reflect.Construct( fixtureType );

				if (this.fixtureSetUp != null)
					Reflect.InvokeMethod(fixtureSetUp, Fixture);
				IsSetUp = true;
			} 
			catch (Exception ex) 
			{
				// Error in TestFixtureSetUp causes the suite and
				// all contained suites to be ignored.
				// TODO: Change this to be a failure?
				NunitException nex = ex as NunitException;
				if (nex != null)
					ex = nex.InnerException;

				if ( ex is NUnit.Framework.IgnoreException )
				{
					this.ShouldRun = false;
					suiteResult.NotRun(ex.Message);
					suiteResult.StackTrace = ex.StackTrace;
					this.IgnoreReason = ex.Message;
				}
				else
				{
					suiteResult.Failure( ex.Message, ex.StackTrace, true );
				}
			}
			finally
			{
				suiteResult.AssertCount = NUnit.Framework.Assert.Counter;
			}
		}

		public override void DoTearDown( TestResult suiteResult )
		{
			if (this.ShouldRun) 
			{
				try 
				{
					IsSetUp = false;
					if (this.fixtureTearDown != null)
						Reflect.InvokeMethod(fixtureTearDown, Fixture);
				} 
				catch (Exception ex) 
				{
					// Error in TestFixtureTearDown causes the
					// suite to be marked as a failure, even if
					// all the contained tests passed.
					NunitException nex = ex as NunitException;
					if (nex != null)
						ex = nex.InnerException;

					suiteResult.Failure( ex.Message, ex.StackTrace);
				}
				finally
				{
					suiteResult.AssertCount += NUnit.Framework.Assert.Counter;
				}
			}

			if (this.IgnoreReason == FIXTURE_SETUP_FAILED) 
			{
				this.ShouldRun = true;
				this.IgnoreReason = null;
			}
		}
	}
}

using System;
using System.Text;
using System.Collections;
using System.Reflection;

// TODO: Lots of stuff in this file for spike purposes. Will split up if 
// it works out.

namespace NUnit.Core
{
	public abstract class GenericTestFixture : TestSuite
	{
		protected string TestFixtureType;
		protected string SetUpType;
		protected string TearDownType;
		protected string FixtureSetUpType;
		protected string FixtureTearDownType;
		protected string ExplicitType;
		protected string CategoryType;
		protected string IgnoreType;
		protected string PlatformType;
	
		private const string FIXTURE_SETUP_FAILED = "Fixture setup failed";

		#region Constructors

		public GenericTestFixture( object fixture ) : base( fixture, 0 ) { }

		public GenericTestFixture( object fixture, int assemblyKey ) : base( fixture, assemblyKey ) { }

		public GenericTestFixture( Type fixtureType ) : base( fixtureType, 0 ) { }

		public GenericTestFixture( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey ) { }

		protected virtual void Initialize()
		{
			try
			{
				if ( Reflect.GetConstructor( fixtureType ) == null )
					throw new InvalidTestFixtureException(fixtureType.FullName + " does not have a valid constructor");
			
				CheckSetUpTearDownMethod( fixtureType, SetUpType );
				CheckSetUpTearDownMethod( fixtureType, TearDownType );
				CheckSetUpTearDownMethod( fixtureType, FixtureSetUpType );
				CheckSetUpTearDownMethod( fixtureType, FixtureTearDownType );

				System.Attribute[] attributes = 
					Reflect.GetAttributes( fixtureType, CategoryType, false );
				IList categories = new ArrayList();

				foreach( Attribute categoryAttribute in attributes ) 
					categories.Add( 
						Reflect.GetPropertyValue( 
						categoryAttribute, 
						"Name", 
						BindingFlags.Public | BindingFlags.Instance ) );
			
				CategoryManager.Add( categories );
				this.Categories = categories;

				this.fixtureSetUp = Reflect.GetMethodWithAttribute( fixtureType, FixtureSetUpType,
					BindingFlags.Public | BindingFlags.Instance );
				this.fixtureTearDown = Reflect.GetMethodWithAttribute( fixtureType, FixtureTearDownType,
					BindingFlags.Public | BindingFlags.Instance );

				this.IsExplicit = Reflect.HasAttribute( fixtureType, ExplicitType, false );

				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( fixtureType ) )
				{
					this.ShouldRun = false;
					this.IgnoreReason = "Not running on correct platform";
				}

				Attribute ignoreAttribute =
					Reflect.GetAttribute( fixtureType, IgnoreType, false );
				if ( ignoreAttribute != null )
				{
					this.ShouldRun = false;
					this.IgnoreReason = (string)Reflect.GetPropertyValue(
						ignoreAttribute, 
						"Reason",
						BindingFlags.Public | BindingFlags.Instance );
				}
		
				Attribute fixtureAttribute =
					Reflect.GetAttribute( fixtureType, TestFixtureType, true );

				// Some of our tests create a fixture without the attribute
				if ( fixtureAttribute != null )
					this.Description = (string)Reflect.GetPropertyValue( 
						fixtureAttribute, 
						"Description",
						BindingFlags.Public | BindingFlags.Instance );

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

		private void CheckSetUpTearDownMethod( Type fixtureType, string attributeName )
		{
			MethodInfo theMethod = Reflect.GetUniqueMethod( fixtureType, attributeName );

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

		#region TestSuite Overrides

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
				NunitException nex = ex as NunitException;
				if (nex != null)
					ex = nex.InnerException;

				if ( ex.GetType().FullName == "NUnit.Framework.IgnoreException" )
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
				suiteResult.AssertCount = NUnit.Framework.Assert.GetAssertCount( true );
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
					suiteResult.AssertCount += NUnit.Framework.Assert.GetAssertCount( true );
				}
			}

			if (this.IgnoreReason == FIXTURE_SETUP_FAILED) 
			{
				this.ShouldRun = true;
				this.IgnoreReason = null;
			}
		}

		#endregion
	}

	/// <summary>
	/// A TestSuite that wraps a class marked with TestFixtureAttribute
	/// </summary>
	public class TestFixture : GenericTestFixture
	{
		#region Constructors

		public TestFixture( object fixture ) : this( fixture, 0 ) { }

		public TestFixture( object fixture, int assemblyKey ) : base( fixture, assemblyKey )
		{
			Initialize();
		}

		public TestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public TestFixture( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey )
		{
			Initialize();
		}

		protected override void Initialize()
		{
			TestFixtureType = "NUnit.Framework.TestFixtureAttribute";
			SetUpType = "NUnit.Framework.SetUpAttribute";
			TearDownType = "NUnit.Framework.TearDownAttribute";
			FixtureSetUpType = "NUnit.Framework.TestFixtureSetUpAttribute";
			FixtureTearDownType = "NUnit.Framework.TestFixtureTearDownAttribute";
			ExplicitType = "NUnit.Framework.ExplicitAttribute";
			CategoryType = "NUnit.Framework.CategoryAttribute";
			IgnoreType = "NUnit.Framework.IgnoreAttribute";
			PlatformType = "NUnit.Framework.PlatformAttribute";

			base.Initialize();
		}

		#endregion
	}

	#region csUnit

	/// <summary>
	/// A TestSuite that wraps a class marked with TestFixtureAttribute
	/// </summary>
	public class CSUnitTestFixture : GenericTestFixture
	{
		#region Constructors

		public CSUnitTestFixture( object fixture ) : this( fixture, 0 ) { }

		public CSUnitTestFixture( object fixture, int assemblyKey ) : base( fixture, assemblyKey )
		{
			Initialize();
		}

		public CSUnitTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public CSUnitTestFixture( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey )
		{
			Initialize();
		}

		protected override void Initialize()
		{
			TestFixtureType = "csUnit.TestFixtureAttribute";
			SetUpType = "csUnit.SetUpAttribute";
			TearDownType = "csUnit.TearDownAttribute";
			FixtureSetUpType = "csUnit.TestFixtureSetUpAttribute";
			FixtureTearDownType = "csUnit.TestFixtureTearDownAttribute";
			ExplicitType = "csUnit.Framework.ExplicitAttribute";
			CategoryType = "csUnit.Framework.CategoryAttribute";
			IgnoreType = "csUnit.Framework.IgnoreAttribute";
			PlatformType = "csUnit.Framework.PlatformAttribute";

			base.Initialize();
		}

		#endregion
	}

	#endregion

	#region VSTS

	/// <summary>
	/// A TestSuite that wraps a class marked with TestFixtureAttribute
	/// </summary>
	public class VstsTestFixture : GenericTestFixture
	{
		#region Constructors

		public VstsTestFixture( object fixture ) : this( fixture, 0 ) { }

		public VstsTestFixture( object fixture, int assemblyKey ) : base( fixture, assemblyKey )
		{
			Initialize();
		}

		public VstsTestFixture( Type fixtureType ) : this( fixtureType, 0 ) { }

		public VstsTestFixture( Type fixtureType, int assemblyKey ) : base( fixtureType, assemblyKey )
		{
			Initialize();
		}

		protected override void Initialize()
		{
			TestFixtureType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestClassAttribute";
			SetUpType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestInitializeAttribute";
			TearDownType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestCleanupAttribute";
			FixtureSetUpType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.ClassInitializeAttribute";
			FixtureTearDownType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.ClassCleanupAttribute";
			ExplicitType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.ExplicitAttribute";
			CategoryType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.CategoryAttribute";
			IgnoreType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.IgnoreAttribute";
			PlatformType = "Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.Framework.PlatformAttribute";

			base.Initialize();
		}

		#endregion
	}

	#endregion
}

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Builders
{
	public abstract class GenericTestFixtureBuilder : ISuiteBuilder
	{
		private TestFixtureParameters parms;

		protected GenericTestCaseBuilder testCaseBuilder;

		protected GenericTestFixtureBuilder( TestFixtureParameters parms, GenericTestCaseBuilder testCaseBuilder )
		{
			this.parms = parms;
			this.testCaseBuilder = testCaseBuilder;
		}

		public virtual bool CanBuildFrom(Type type)
		{
			return !type.IsAbstract 
				&& Reflect.HasAttribute( type, parms.TestFixtureType, true ); // Inheritable
		}

//		protected virtual bool IsSetUpMethod( MethodInfo method )
//		{
//			return Reflect.HasAttribute( method, parms.SetUpType, false );
//		}

		// Provided for convenience of testing - not part of interface
		public TestSuite BuildFrom( Type type )
		{
			return BuildFrom( type, 0 );
		}

		public virtual TestSuite BuildFrom(Type fixtureType, int assemblyKey)
		{
			TestSuite suite = null;

			try
			{

				suite = Construct( fixtureType, assemblyKey );

				if ( Reflect.GetConstructor( fixtureType ) == null )
					throw new InvalidTestFixtureException( 
						string.Format( "{0} does not have a valid constructor", fixtureType.FullName ) );
			
				CheckSetUpTearDownMethod( fixtureType, parms.SetUpType );
				CheckSetUpTearDownMethod( fixtureType, parms.TearDownType );
				CheckSetUpTearDownMethod( fixtureType, parms.FixtureSetUpType );
				CheckSetUpTearDownMethod( fixtureType, parms.FixtureTearDownType );
			
	
				System.Attribute[] attributes = 
					Reflect.GetAttributes( fixtureType, parms.CategoryType, false );
				IList categories = new ArrayList();

				foreach( Attribute categoryAttribute in attributes ) 
					categories.Add( 
						Reflect.GetPropertyValue( 
						categoryAttribute, 
						"Name", 
						BindingFlags.Public | BindingFlags.Instance ) );
			
				CategoryManager.Add( categories );
				suite.Categories = categories;

				suite.IsExplicit = Reflect.HasAttribute( fixtureType, parms.ExplicitType, false );

				PlatformHelper helper = new PlatformHelper();
				if ( !helper.IsPlatformSupported( fixtureType ) )
				{
					suite.ShouldRun = false;
					suite.IgnoreReason = "Not running on correct platform";
				}

				Attribute ignoreAttribute =
					Reflect.GetAttribute( fixtureType, parms.IgnoreType, false );
				if ( ignoreAttribute != null )
				{
					suite.ShouldRun = false;
					suite.IgnoreReason = (string)Reflect.GetPropertyValue(
						ignoreAttribute, 
						"Reason",
						BindingFlags.Public | BindingFlags.Instance );
				}
		
				suite.Description = GetFixtureDescription( fixtureType );

				using( new AddinManagerState( AddinManager.Addins ) )
				{
					AddinManager.Addins.TestBuilders.Add( testCaseBuilder );

					IList methods = GetCandidateTestMethods( fixtureType );
					foreach(MethodInfo method in methods)
					{
						TestCase testCase = TestCaseBuilder.BuildFrom( method );
						//					if ( IsTestMethod( method ) )
						//						testCase = MakeTestCase( method );
						//					else
						//testCase = testCaseBuilder.Make( fixtureType, method );
						//					TestCase testCase =	AddinManager.Addins.BuildFrom( method );
						//					if ( testCase == null )
						//						testCase = AddinManager.Builtins.BuildFrom( method );

						if(testCase != null)
						{
							testCase.AssemblyKey = suite.AssemblyKey;
							suite.Add( testCase );
						}
					}
				}

				if( suite.CountTestCases() == 0 )
				{
					suite.ShouldRun = false;
					suite.IgnoreReason = suite.Name + " does not have any tests";
				}
			}
			catch( InvalidTestFixtureException exception )
			{
				suite.ShouldRun = false;
				suite.IgnoreReason = exception.Message;
			}

			return suite;
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

		protected abstract TestSuite Construct( Type type, int assemblyKey );

		protected virtual string GetFixtureDescription( Type fixtureType )
		{
			Attribute fixtureAttribute =
				Reflect.GetAttribute( fixtureType, parms.TestFixtureType, true );

			// Some of our tests create a fixture without the attribute
			if ( fixtureAttribute != null )
				return (string)Reflect.GetPropertyValue( 
					fixtureAttribute, 
					"Description",
					BindingFlags.Public | BindingFlags.Instance );

			return null;
		}

		protected IList GetCandidateTestMethods( Type fixtureType )
		{
			return fixtureType.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );
		}

	}

}

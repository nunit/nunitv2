using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// A TestSuite that wraps a class marked with TestFixtureAttribute
	/// </summary>
	public class TestFixture : TestSuite
	{
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
				Reflect.CheckFixtureType( fixtureType );

				IList categories = Reflect.GetCategories( fixtureType );
				CategoryManager.Add( categories );
				this.Categories = categories;

				this.fixtureSetUp = Reflect.GetFixtureSetUpMethod( fixtureType );
				this.fixtureTearDown = Reflect.GetFixtureTearDownMethod( fixtureType );

				Type explicitAttribute = typeof(NUnit.Framework.ExplicitAttribute);
				object[] attributes = fixtureType.GetCustomAttributes( explicitAttribute, false );
				this.IsExplicit = attributes.Length > 0;

				Type ignoreMethodAttribute = typeof(NUnit.Framework.IgnoreAttribute);
				attributes = fixtureType.GetCustomAttributes(ignoreMethodAttribute, false);
				if(attributes.Length == 1)
				{
					NUnit.Framework.IgnoreAttribute attr = 
						(NUnit.Framework.IgnoreAttribute)attributes[0];
					this.ShouldRun = false;
					this.IgnoreReason = attr.Reason;
				}

				Type fixtureAttribute = typeof(NUnit.Framework.TestFixtureAttribute);
				attributes = fixtureType.GetCustomAttributes(fixtureAttribute, false);
				if(attributes.Length == 1)
				{
					NUnit.Framework.TestFixtureAttribute fixtureAttr = 
						(NUnit.Framework.TestFixtureAttribute)attributes[0];
					this.Description = fixtureAttr.Description;
				}

				////////////////////////////////////////////////////////////////////////
				// Uncomment the following code block to allow including Suites in the
				// tree of tests. This causes a problem when the same test is added
				// in multiple suites so we need to either fix it or prevent it.
				//
				// See also a line to change in TestSuiteBuilder.cs
				////////////////////////////////////////////////////////////////////////

				PropertyInfo [] properties = fixtureType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
				foreach(PropertyInfo property in properties)
				{
					attributes = property.GetCustomAttributes(typeof(NUnit.Framework.SuiteAttribute),false);
					if(attributes.Length>0)
					{
						MethodInfo method = property.GetGetMethod(true);
						if(method.ReturnType!=typeof(NUnit.Core.TestSuite) || method.GetParameters().Length>0)
						{
							this.ShouldRun = false;
							this.IgnoreReason = "Invalid suite property method signature";
						}
						//					else
						//					{
						//						TestSuite suite = (TestSuite)property.GetValue(null, new Object[0]);
						//						foreach( Test test in suite.Tests )
						//							testSuite.Add( test );
						//					}
					}
				}

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

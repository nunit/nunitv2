using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// TestFixture is a surrogate for a user test fixture class,
	/// containing one or more tests.
	/// </summary>
	public class TestFixture : TestSuite
	{
		#region Constructors
		public TestFixture( Type fixtureType, int assemblyKey )
			: base( fixtureType, assemblyKey ) { }
		#endregion

		#region Properties
		public override bool IsFixture
		{
			get { return true; }
		}
		#endregion

		#region TestSuite Overrides

		public override void DoFixtureSetUp( TestResult suiteResult )
		{
			try 
			{
				if ( Fixture == null )
					Fixture = Reflect.Construct( FixtureType );

				if (this.fixtureSetUp != null)
					Reflect.InvokeMethod(fixtureSetUp, Fixture);
				IsSetUp = true;
			} 
			catch (Exception ex) 
			{
				NunitException nex = ex as NunitException;
				if (nex != null)
					ex = nex.InnerException;

				if ( testFramework.IsIgnoreException( ex ) )
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
				if ( testFramework != null )
					suiteResult.AssertCount = testFramework.GetAssertCount();
			}
		}

		public override void DoFixtureTearDown( TestResult suiteResult )
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
					if ( testFramework != null )
						suiteResult.AssertCount += testFramework.GetAssertCount();
				}
			}
		}

		#endregion
	}
}

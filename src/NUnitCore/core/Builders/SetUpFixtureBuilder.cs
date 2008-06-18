// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// SetUpFixtureBuilder knows how to build a SetUpFixture.
	/// </summary>
	public class SetUpFixtureBuilder : Extensibility.ISuiteBuilder
	{	
		#region ISuiteBuilder Members
		public Test BuildFrom(Type type)
		{
			SetUpFixture fixture = new SetUpFixture( type );

            string reason = null;
            if (!IsValidFixtureType(type, ref reason))
            {
                fixture.RunState = RunState.NotRunnable;
                fixture.IgnoreReason = reason;
            }

            return fixture;
		}

		public bool CanBuildFrom(Type type)
		{
			return Reflect.HasAttribute( type, NUnitFramework.SetUpFixtureAttribute, false );
		}
		#endregion

        private bool IsValidFixtureType(Type type, ref string reason)
        {
            if (!NUnitFramework.IsValidFixtureType(type, ref reason))
                return false;

            if ( !NUnitFramework.IsSetUpMethodValid(type, ref reason) )
                return false;

            if ( !NUnitFramework.IsTearDownMethodValid(type, ref reason) )
                return false;

            if ( NUnitFramework.GetFixtureSetUpMethod(type) != null )
            {
                reason = "TestFixtureSetUp method not allowed on a SetUpFixture";
                return false;
            }

            if ( NUnitFramework.GetFixtureTearDownMethod(type) != null )
            {
                reason = "TestFixtureTearDown method not allowed on a SetUpFixture";
                return false;
            }

            return true;
        }
	}
}

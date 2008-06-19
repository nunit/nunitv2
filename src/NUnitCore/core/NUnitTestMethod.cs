// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// Class to implement an NUnit test method
	/// </summary>
	public class NUnitTestMethod : TestMethod
	{
		#region Constructor
		public NUnitTestMethod(MethodInfo method) : base(method) 
        {
            this.setUpMethod = NUnitFramework.GetSetUpMethod(this.FixtureType);
            this.tearDownMethod = NUnitFramework.GetTearDownMethod(this.FixtureType);
        }
		#endregion

		#region TestMethod Overrides

        /// <summary>
		/// Run a test returning the result. Overrides TestMethod
		/// to count assertions.
		/// </summary>
		/// <param name="testResult"></param>
		public override void Run(TestResult testResult)
		{
			base.Run(testResult);

			testResult.AssertCount = NUnitFramework.Assert.GetAssertCount();
		}

        /// <summary>
        /// Returns a result state for a special exception.
        /// If the exception is not handled specially, returns
        /// ResultState.Error.
        /// </summary>
        /// <param name="ex">The exception to be examined</param>
        /// <returns>A ResultState</returns>
        protected override ResultState GetResultState(Exception ex)
        {
            string name = ex.GetType().FullName;

            if ( name == NUnitFramework.AssertException )
                return ResultState.Failure;
            else 
            if ( name == NUnitFramework.IgnoreException )
                return ResultState.Ignored;
            else 
            if ( name == NUnitFramework.InconclusiveException )
                return ResultState.Inconclusive;
            else 
            if ( name == NUnitFramework.SuccessException )
                return ResultState.Success;
            else
                return ResultState.Error;
        }

        #endregion
	}
}

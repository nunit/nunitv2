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
        #endregion
	}
}

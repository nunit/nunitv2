// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core.Builders
{
	/// <summary>
	/// AbstractFixtureBuilder may serve as a base class for 
	/// implementing a suite builder. It provides a templated
	/// implementation of the BuildFrom method as well as a 
	/// number of useful methods that derived classes may use.
	/// </summary>
	public abstract class AbstractFixtureBuilder : Extensibility.ISuiteBuilder
	{
		#region Instance Fields
		/// <summary>
		/// The TestSuite being constructed;
		/// </summary>
		protected TestSuite suite;
		#endregion

		#region Abstract Methods
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// this builder to use in building a TestSuite.
		/// 
		/// Note that returning false will cause the type to be ignored 
		/// in loading the tests. If it is desired to load the suite
		/// but label it as non-runnable, ignored, etc., then this
		/// method must return true.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		public abstract bool CanBuildFrom(Type type);

		/// <summary>
		/// Method that actually creates a new TestSuite object
		/// 
		/// Derived classes must override this method.
		/// </summary>
		/// <param name="type">The user fixture type</param>
		/// <returns></returns>
		protected abstract TestSuite MakeSuite( Type type );
		#endregion

		#region Virtual Methods
		/// <summary>
		/// Templated implementaton of ISuiteBuilder.BuildFrom. Any
		/// derived builder may choose to override this method in
		/// it's entirety or to let it stand and override some of
		/// the virtual methods that it calls.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual Test BuildFrom(Type type)
		{
			this.suite = MakeSuite(type);

			SetTestSuiteProperties(type, suite);

			InstallTestCaseBuilders(type);
			AddTestCases(type);

			if ( this.suite.RunState != RunState.NotRunnable && this.suite.TestCount == 0)
			{
				this.suite.RunState = RunState.NotRunnable;
				this.suite.IgnoreReason = suite.TestName.Name + " does not have any tests";
			}

			return this.suite;
		}

		/// <summary>
		/// Method that sets properties of the test suite based on the
		/// information in the provided Type.
		/// 
		/// Derived classes normally override this method and should
		/// call the base method or include equivalent code.
		/// </summary>
		/// <param name="type">The type to examine</param>
		/// <param name="suite">The test suite being constructed</param>
		protected virtual void SetTestSuiteProperties( Type type, TestSuite suite )
		{
			string reason = null;
			if (!IsValidFixtureType(type, ref reason) )
			{
				this.suite.RunState = RunState.NotRunnable;
				this.suite.IgnoreReason = reason;
			}
		}

        /// <summary>
        /// Method that may be overridden in order to install any
        /// TestCaseBuilders that should only be available for
        /// the type of fixture being built. The override may
        /// use the Type argument to decide what to install.
        /// </summary>
        /// <param name="type"></param>
        protected virtual void InstallTestCaseBuilders(Type type)
        {
        }

		/// <summary>
		/// Virtual method that returns true if the fixture type is valid
		/// for use by the builder. If not, it returns false and sets
		/// reason to an appropriate message. As implemented in this class,
		/// the method checks that a default constructor is available. You
		/// may override this method in a derived class in order to make 
		/// different or additional checks.
		/// </summary>
		/// <param name="fixtureType">The fixture type</param>
		/// <param name="reason">The reason this fixture is not valid</param>
		/// <returns>True if the fixture type is valid, false if not</returns>
		protected virtual bool IsValidFixtureType( Type fixtureType, ref string reason )
		{
            if (fixtureType.IsAbstract)
            {
                reason = string.Format("{0} is an abstract class", fixtureType.FullName);
                return false;
            }

            if (Reflect.GetConstructor(fixtureType) == null)
			{
				reason = string.Format( "{0} does not have a valid constructor", fixtureType.FullName );
				return false;
			}

			return true;
		}

		/// <summary>
		/// Method to add test cases to the newly constructed suite.
		/// The default implementation looks at each candidate method
		/// and tries to build a test case from it. This is sufficient
		/// for any fixture that only requires the builtin types of
		/// test cases. A derived builder that supports additional
		/// types will generally override this method in order to
		/// wrap it with code that installs its own test case 
		/// builders or decorators and removes them afterward.
		/// </summary>
		/// <param name="fixtureType"></param>
		protected virtual void AddTestCases( Type fixtureType )
		{
			IList methods = GetCandidateTestMethods( fixtureType );
			foreach(MethodInfo method in methods)
			{
				Test test = TestCaseBuilder.BuildFrom( method );

				if(test != null)
				{
					this.suite.Add( test );
				}
			}
		}

		/// <summary>
		/// Method to return all methods in a fixture that should be examined
		/// to see if they are test methods. The default returns all methods
		/// of the fixture: public and private, instance and static, declared
		/// and inherited.
        /// 
        /// While this method may be overridden, it should normally not be.
        /// If it is overridden to eliminate certain methods, they will be
        /// silently ignored. Generally, it is better to include them in the
        /// list and let the TestCaseBuilders decide how to handle them.
		/// </summary>
		/// <param name="fixtureType"></param>
		/// <returns></returns>
		protected IList GetCandidateTestMethods( Type fixtureType )
		{
			return fixtureType.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );
		}
		#endregion
	}
}

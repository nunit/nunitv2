using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// AbstractFixtureBuilder may serve as a base class for 
	/// implementing a suite builder. It provides a templated
	/// implementation of the BuildFrom method as well as a 
	/// number of useful methods that derived classes may use.
	/// </summary>
	public abstract class AbstractFixtureBuilder : ISuiteBuilder
	{
		#region Instance Fields
		/// <summary>
		/// The TestSuite being constructed;
		/// </summary>
		protected TestSuite suite;
		#endregion

		#region ISuiteBuilder Members

		public abstract bool CanBuildFrom(Type type);

		/// <summary>
		/// Templated implementaton of ISuiteBuilder.BuildFrom. Any
		/// derived builder may choose to override this method in
		/// it's entirety or to let it stand and override some of
		/// the virtual methods that it calls.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="assemblyKey"></param>
		/// <returns></returns>
		public virtual TestSuite BuildFrom(Type type, int assemblyKey)
		{
			this.suite = MakeSuite( type, assemblyKey );

			string reason = null;
			if( !IsValidFixtureType( type, ref reason ) ||
				!IsRunnable( type, ref reason ) )
			{
				this.suite.ShouldRun = false;
				this.suite.IgnoreReason = reason;
			}

			this.suite.Description = GetFixtureDescription( type );

			this.AddTestCases( type );

			if( this.suite.CountTestCases() == 0 )
			{
				this.suite.ShouldRun = false;
				this.suite.IgnoreReason = suite.Name + " does not have any tests";
			}

			return this.suite;
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		/// This method must be overridden to return an object of a class
		/// that derives from TestSuite.
		/// </summary>
		/// <param name="type">The user fixture type</param>
		/// <param name="assemblyKey">The index of the containing assembly</param>
		/// <returns></returns>
		protected abstract TestSuite MakeSuite( Type type, int assemblyKey );

		#endregion

		#region Virtual Methods

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
			if ( Reflect.GetConstructor( fixtureType ) == null )
			{
				reason = string.Format( "{0} does not have a valid constructor", fixtureType.FullName );
				return false;
			}

			return true;
		}

		/// <summary>
		/// This method returns true if the fixture is runnable. The default
		/// implementation simply returns true. Usually, this will be overridden
		/// to check for the presence of an ignore attribute of some kind.
		/// </summary>
		/// <param name="fixtureType">The fixture type to check</param>
		/// <param name="reason">Set to the reason for not running</param>
		/// <returns>True if the test is runnable, false if not</returns>
		protected virtual bool IsRunnable( Type fixtureType, ref string reason )
		{
			return true;
		}

		/// <summary>
		/// Method to get any fixture description. Default returnes null.
		/// Override to examine the fixture type and extract the description.
		/// </summary>
		/// <param name="fixtureType"></param>
		/// <returns></returns>
		protected virtual string GetFixtureDescription( Type fixtureType )
		{
			return null;
		}

		/// <summary>
		/// Method to add test cases to the newly constructed suite.
		/// The default implementation looks at each candidate method
		/// and tries to build a test case from it. This is sufficient
		/// for any fixture that only requires the builtin types of
		/// test cases. A derived builder that supports additional
		/// types will generally override this method in order to
		/// wrap it with code that registers its own test case 
		/// builders as addins and removes them afterward.
		/// </summary>
		/// <param name="fixtureType"></param>
		protected virtual void AddTestCases( Type fixtureType )
		{
			IList methods = GetCandidateTestMethods( fixtureType );
			foreach(MethodInfo method in methods)
			{
				TestCase testCase = TestCaseBuilder.Make( method );

				if(testCase != null)
				{
					testCase.AssemblyKey = suite.AssemblyKey;
					this.suite.Add( testCase );
				}
			}
		}

		/// <summary>
		/// Method to return all methods in a fixture that should be examined
		/// to see if they are test methods. The default returns all methods
		/// of the fixture: public and private, instance and static, declared
		/// and inherited.
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

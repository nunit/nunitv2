using System;
using System.Reflection;

namespace NUnit.Core
{
	/// <summary>
	/// AbstractTestCaseBuilder may serve as a base class for 
	/// implementing a test case builder. It provides a templated
	/// implementation of the BuildFrom method as well as a 
	/// number of useful methods that derived classes may use.
	/// </summary>
	public abstract class AbstractTestCaseBuilder : ITestCaseBuilder
	{
		#region Instance Fields
		protected TestCase testCase;
		#endregion

		#region ITestCaseBuilder Members

		public abstract bool CanBuildFrom(System.Reflection.MethodInfo method);

		/// <summary>
		/// Templated implementaton of ITestCaseBuilder.BuildFrom. Any
		/// derived builder may choose to override this method in
		/// it's entirety or to let it stand and override some of
		/// the virtual methods that it calls.
		/// </summary>
		/// <param name="method">The method for which a test case is to be built</param>
		/// <returns>A TestCase or null</returns>
		public virtual TestCase BuildFrom(System.Reflection.MethodInfo method)
		{
			TestCase testCase = null;

			if ( HasValidTestCaseSignature( method ) )
			{
				testCase = MakeTestCase( method );

				string reason = null;
				if ( !IsRunnable( method, ref reason ) )
				{
					testCase.ShouldRun = false;
					testCase.IgnoreReason = reason;
				}

				testCase.Description = GetTestCaseDescription( method );

				SetTestProperties( method );
			}
			else
			{
				testCase = new NotRunnableTestCase( method );
			}

			return testCase;
		}

		#endregion

		#region Abstract Methods
		/// <summary>
		/// Derived classes must override this to return a TestCase
		/// object of the proper class.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		protected abstract TestCase MakeTestCase( MethodInfo method );
		#endregion

		#region Virtual Methods
		/// <summary>
		/// Virtual method that checks the signature of a potential test case.
		/// </summary>
		/// <param name="method">The method to be checked</param>
		/// <returns>True if the method signature is valid, false if not</returns>
		protected bool HasValidTestCaseSignature( MethodInfo method )
		{
			return !method.IsStatic
				&& !method.IsAbstract
				&& method.IsPublic
				&& method.GetParameters().Length == 0
				&& method.ReturnType.Equals(typeof(void) );
		}

		/// <summary>
		/// This method returns true if the test case is runnable. The default
		/// implementation simply returns true. Usually, this will be overridden
		/// to check for the presence of an ignore attribute of some kind.
		/// </summary>
		/// <param name="method">The test method type to check</param>
		/// <param name="reason">Set to the reason for not running</param>
		/// <returns>True if the test is runnable, false if not</returns>
		protected virtual bool IsRunnable( MethodInfo method, ref string reason )
		{
			return true;
		}

		/// <summary>
		/// Method to get any test case description. Default returnes null.
		/// Override to examine the method and extract the description.
		/// </summary>
		/// <param name="method">The test method to examine</param>
		/// <returns>The description or null</returns>
		protected virtual string GetTestCaseDescription( MethodInfo method )
		{
			return null;
		}

		/// <summary>
		/// Override this method to set any additional test case properties
		/// </summary>
		/// <param name="method">The test method to examine</param>
		protected virtual void SetTestProperties( MethodInfo method )
		{
		}
		#endregion
	}
}

//
// Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
//
namespace NUnit.Core
{
	using System;
	using System.Reflection;

	/// <summary>
	/// Summary description for TestCaseBuilder.
	/// </summary>
	public class TestCaseBuilder
	{
		public static TestCase Make(object fixture, MethodInfo method)
		{
			TestCase testCase = null;

			if(HasTestAttribute(method) || HasObsoleteTestName(method))
			{
				if(IsTestMethodSignatureCorrect(method))
				{
					if(!IsExpectedException(method))
						testCase = new NormalTestCase(fixture, method);
					else
					{
						Type expectedException = GetExpectedExceptions(method);
						testCase = new ExpectedExceptionTestCase(fixture, method, expectedException);
					}
					if(HasIgnoreAttribute(method))
					{
						testCase.ShouldRun = false;
						testCase.IgnoreReason = GetIgnoreReason(method);
					}

				}
				else
				{
					string reason = String.Format("Method: {0}'s signature is not correct", method.Name);
					testCase = new NotRunnableTestCase(method, reason);
				}
			}

			return testCase;
		}

		public static TestCase Make(object fixture, string methodName)
		{
			MethodInfo [] methods = fixture.GetType().GetMethods(BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance);
			foreach(MethodInfo method in methods)
			{
				if(method.Name.Equals(methodName))
					return Make(fixture, method);
			}

			return null;
		}

		private static bool IsExpectedException(MethodInfo method)
		{
			Type exceptionAttr = typeof(NUnit.Framework.ExpectedExceptionAttribute);
			object[] attributes = method.GetCustomAttributes(exceptionAttr, false);
			return attributes.Length == 1;
		}

		private static Type GetExpectedExceptions(MethodInfo method)
		{
			Type exceptionAttr = typeof(NUnit.Framework.ExpectedExceptionAttribute);
			object[] attributes = method.GetCustomAttributes(exceptionAttr, false);

			Type returnType = null;

			if(attributes.Length == 1)
			{
				NUnit.Framework.ExpectedExceptionAttribute expectedAttr = 
					(NUnit.Framework.ExpectedExceptionAttribute)attributes[0];
				returnType = expectedAttr.ExceptionType;
			}

			return returnType;
		}

		public static int CountTestCases(object fixture) 
		{
			int testCases = 0;

			MethodInfo [] methods = fixture.GetType().GetMethods();
			foreach(MethodInfo method in methods)
			{
				if(IsTestMethod(method))
					testCases++;
			}

			return testCases;
		}


		public static bool IsTestMethod(MethodInfo methodToCheck) 
		{
			return
				(HasTestAttribute(methodToCheck) || HasObsoleteTestName(methodToCheck))
				&& IsTestMethodSignatureCorrect(methodToCheck);
		}

		private static bool IsTestMethodSignatureCorrect(MethodInfo methodToCheck)
		{
			return 
				!methodToCheck.IsAbstract
				&& methodToCheck.IsPublic
				&& methodToCheck.GetParameters().Length == 0
				&& methodToCheck.ReturnType.Equals(typeof(void));
		}

		private static bool HasTestAttribute(MethodInfo methodToCheck)
		{
			return methodToCheck.IsDefined(typeof(NUnit.Framework.TestAttribute),false);
		}
		
		private static bool HasObsoleteTestName(MethodInfo methodToCheck)
		{
			return methodToCheck.Name.ToLower().StartsWith("test");
		}

		private static bool HasIgnoreAttribute(MethodInfo methodToCheck)
		{
			Type ignoreMethodAttribute = typeof(NUnit.Framework.IgnoreAttribute);
			object[] attributes = methodToCheck.GetCustomAttributes(ignoreMethodAttribute, false);
			return attributes.Length == 1;
		}

		private static string GetIgnoreReason(MethodInfo methodToCheck)
		{
			Type ignoreMethodAttribute = typeof(NUnit.Framework.IgnoreAttribute);
			NUnit.Framework.IgnoreAttribute[] attributes = (NUnit.Framework.IgnoreAttribute[])methodToCheck.GetCustomAttributes(ignoreMethodAttribute, false);
			string result = "no reason";
			if(attributes.Length > 0)
				result = attributes[0].Reason;

			return result;
		}
	}
}

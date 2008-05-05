// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	public class BaseTestFixture
	{
		protected const string Method_RowTestMethodWith2Rows = "RowTestMethodWith2Rows";
		protected const string Method_RowTestMethodWithTestName = "RowTestMethodWithTestName";
		protected const string Method_RowTestMethodWithExpectedException = "RowTestMethodWithExpectedException";
		protected const string Method_RowTestMethodWithExpectedExceptionAndExceptionMessage = "RowTestMethodWithExpectedExceptionAndExceptionMessage";
		protected const string Method_RowTestMethodWithNullArgument = "RowTestMethodWithNullArgument";
		protected const string Method_RowTestMethodWithNormalAndNullArgument = "RowTestMethodWithNormalAndNullArgument";
		
		public BaseTestFixture()
		{
		}
		
		protected MethodInfo GetRowTestMethodWith2Rows()
		{
			return GetTestClassMethod(Method_RowTestMethodWith2Rows);
		}
		
		protected MethodInfo GetRowTestMethodWithTestName()
		{
			return GetTestClassMethod(Method_RowTestMethodWithTestName);
		}
		
		protected MethodInfo GetRowTestMethodWithExpectedException()
		{
			return GetTestClassMethod(Method_RowTestMethodWithExpectedException);
		}
		
		protected MethodInfo GetRowTestMethodWithExpectedExceptionAndExceptionMessage()
		{
			return GetTestClassMethod(Method_RowTestMethodWithExpectedExceptionAndExceptionMessage);
		}
		
		protected MethodInfo GetRowTestMethodWithNullArgument()
		{
			return GetTestClassMethod(Method_RowTestMethodWithNullArgument);
		}
		
		protected MethodInfo GetRowTestMethodWithNormalAndNullArgument()
		{
			return GetTestClassMethod(Method_RowTestMethodWithNormalAndNullArgument);
		}
		
		protected MethodInfo GetTestClassMethod(string methodName)
		{
			Type testClass = typeof(TestClass);
			return testClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
		}
	}
}

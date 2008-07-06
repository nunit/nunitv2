// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	public class ParameterSetFilterTestBase
	{
		protected MethodInfo GetMethod(string methodName)
		{
			return this.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
		}
	}
}

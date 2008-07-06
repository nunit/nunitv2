// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;
using NUnitExtension.RowTest.AddIn;
using NUnitExtension.RowTest;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class SpecialValueParameterSetFilterTest : ParameterSetFilterTestBase
	{
		[Test]
		public void Filter_SpecialValueNull()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { 42, SpecialValue.Null, 21 };
			SpecialValueParameterSetFilter filter = new SpecialValueParameterSetFilter();
			
			filter.Filter(parameterSet, GetMethod());
			
			Assert.That(parameterSet.Arguments, Is.EqualTo(new object[] { 42, null, 21 }).AsCollection);
		}
		
		[Test]
		public void Filter_Null()
		{
 			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { 42, null, 42 };
			SpecialValueParameterSetFilter filter = new SpecialValueParameterSetFilter();
			
			filter.Filter(parameterSet, GetMethod());
			
			Assert.That(parameterSet.Arguments, Is.EqualTo(new object[] { 42, null, 42 }).AsCollection);
		}
		
		private MethodInfo GetMethod()
		{
			return GetMethod("MethodWith3Parameters");
		}
		
		#region Methods for tests
		
		public void MethodWith3Parameters(object a1, object a2, object a3)
		{
		}
		
		#endregion
	}
}

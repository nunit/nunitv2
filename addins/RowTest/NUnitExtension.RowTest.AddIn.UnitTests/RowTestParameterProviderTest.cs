// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NUnit.Mocks;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class RowTestParameterProviderTest
	{
		private RowTestParameterProvider parameterProvider;

        private IList GetParametersForMethodAsList(MethodInfo method)
        {
            ArrayList list = new ArrayList();
            foreach (object o in parameterProvider.GetTestCasesFor(method))
                list.Add(o);
            return list;
        }
		
		[SetUp]
		public void SetUp()
		{
			parameterProvider = new RowTestParameterProvider();
		}
		
		[Test]
		public void HasParametersFor_WithoutRows()
		{
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");
			
			bool hasParameters = parameterProvider.HasTestCasesFor(method);
			
			Assert.That(hasParameters, Is.False);
		}
		
		[Test]
		public void HasParametersFor_WithRows()
		{
			MethodInfo method = GetTestClassMethod("RowTestMethodWith2Rows");
			
			bool hasParameters = parameterProvider.HasTestCasesFor(method);
			
			Assert.That(hasParameters, Is.True);
		}
		
		[Test]
		public void HasParametersFor_WithoutRowTestAttribute()
		{
			MethodInfo method = GetTestClassMethod("MethodWithRowAttribute");
			
			bool hasParameters = parameterProvider.HasTestCasesFor(method);
			
			Assert.That(hasParameters, Is.False);
		}

		[Test]
		public void GetParametersFor_WithoutRows()
		{
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");
			
			IList parameterSets = GetParametersForMethodAsList(method);
			
			Assert.That(parameterSets.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetParametersFor_WithRows()
		{
			MethodInfo method = GetTestClassMethod("RowTestMethodWith2Rows");
			
			IList parameterSets = GetParametersForMethodAsList(method);
			
			Assert.That(parameterSets.Count, Is.EqualTo(2));
			ParameterSet parameterSet1 = (ParameterSet) parameterSets[0];
			ParameterSet parameterSet2 = (ParameterSet) parameterSets[1];
			
			Assert.That(parameterSet1.Arguments.Length, Is.EqualTo(2));
			Assert.That(parameterSet2.Arguments.Length, Is.EqualTo(2));
		}
		
		[Test]
		public void GetParametersFor_Null()
		{
			MethodInfo method = GetTestClassMethod("RowTestMethodWithNullArgument");
			
			IList parameterSets = GetParametersForMethodAsList(method);
			
			Assert.That(parameterSets.Count, Is.EqualTo(1));
			ParameterSet parameterSet = (ParameterSet) parameterSets[0];
			
			Assert.That(parameterSet.Arguments.Length, Is.EqualTo(1));
			Assert.That(parameterSet.Arguments[0], Is.Null);
		}
		
		[Test]
		public void GetParametersFor_CallsParameterSetFilter()
		{
			DynamicMock parameterSetFilterMock = new DynamicMock(typeof(IParameterSetFilter));
			IParameterSetFilter parameterSetFilter = (IParameterSetFilter) parameterSetFilterMock.MockInstance;
			parameterProvider.AddParameterSetFilter(parameterSetFilter);
			
			parameterSetFilterMock.Expect("Filter", Is.TypeOf(typeof(ParameterSet)), Is.Not.Null);
			parameterSetFilterMock.Expect("Filter", Is.TypeOf(typeof(ParameterSet)), Is.Not.Null);
			
			parameterProvider.GetTestCasesFor(GetTestClassMethod("RowTestMethodWith2Rows"));
			
			parameterSetFilterMock.Verify();
		}
		
		[Test]
		[ExpectedException (typeof(ArgumentNullException))]
		public void AddParameterSetFilter_FilterIsNull()
		{
			parameterProvider.AddParameterSetFilter(null);
		}
		
		private MethodInfo GetTestClassMethod(string methodName)
		{
			Type testClass = typeof(TestClass);
			return testClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
		}
	}
}

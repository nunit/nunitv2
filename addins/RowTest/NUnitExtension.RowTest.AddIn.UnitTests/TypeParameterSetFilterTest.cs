// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Collections;
using System.Reflection;
using NUnit.Core.Extensibility;
using NUnitExtension.RowTest.AddIn;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class TypeParameterSetFilterTest : ParameterSetFilterTestBase
	{
		public class TypeWithDefaultConstructor
		{
			public TypeWithDefaultConstructor()
			{
			}
		}
		
		public class TypeWithoutDefaultConstructor
		{
			public TypeWithoutDefaultConstructor(string arg)
			{
			}
		}
			
		TypeParameterSetFilter filter;
		
		[SetUp]
		public void SetUp()
		{
			filter = new TypeParameterSetFilter();
		}
		
		[Test]
		public void Filter_CreatesInstance()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { typeof(TypeWithDefaultConstructor) };
			
			filter.Filter(parameterSet, GetMethod("MethodWithParameter"));
			
			Assert.That(parameterSet.Arguments[0], Is.TypeOf(typeof(TypeWithDefaultConstructor)));
		}
		
		[Test]
		public void Filter_TypeWithoutDefaultConstructor()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { typeof(TypeWithoutDefaultConstructor) };
			
			try
			{
				filter.Filter(parameterSet, GetMethod("MethodWithWrongParameter"));
				Assert.Fail("ArgumentException was expected.");
			}
			catch (ArgumentException exception)
			{
				string expectedMessage = string.Format(
						"Cannot create an instance of type '{0}', "
						+ "because it does not have a default constructor.",
						parameterSet.Arguments[0]);

				Assert.That(exception.Message, Is.EqualTo(expectedMessage));
			}
		}
		
		[Test]
		public void Filter_BaseType()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { typeof(ArrayList) };
			
			filter.Filter(parameterSet, GetMethod("MethodWithIEnumerableParameter"));
			
			Assert.That(parameterSet.Arguments[0], Is.TypeOf(typeof(ArrayList)));
		}
		
		[Test]
		public void Filter_NotAssignableType()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { typeof(Version) };
			
			filter.Filter(parameterSet, GetMethod("MethodWithIEnumerableParameter"));
			
			Assert.That(parameterSet.Arguments[0], Is.SameAs(typeof(Version)));
		}

		#region Methods for tests
		
		public void MethodWithParameter(TypeWithDefaultConstructor instance)
		{
		}
		
		public void MethodWithWrongParameter(TypeWithoutDefaultConstructor instance)
		{
		}
		
		public void MethodWithIEnumerableParameter(IEnumerable enumerable)
		{
		}
		
		#endregion
	}
}

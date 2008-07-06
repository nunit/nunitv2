// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnitExtension.RowTest.AddIn;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class ArgumentListParameterSetFilterTest : ParameterSetFilterTestBase
	{
		private ArgumentListParameterSetFilter filter;
		
		[SetUp]
		public void SetUp()
		{
			filter = new ArgumentListParameterSetFilter();
		}
		
		[Test]
		public void Filter_TwoArguments()
		{
			string arg1 = "arg1";
			int arg2 = 42;
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { arg1, arg2 };
			
			filter.Filter(parameterSet, GetMethod("MethodWithArgumentList"));
			
			Assert.That(parameterSet.Arguments.Length, Is.EqualTo(3));
			Assert.That(parameterSet.Arguments[0], Is.EqualTo(arg1));
			Assert.That(parameterSet.Arguments[1], Is.EqualTo(arg2));
			Assert.That(parameterSet.Arguments[2], Is.TypeOf(typeof(object[])));
			Assert.That(parameterSet.Arguments[2], Is.Empty);
		}
		
		[Test]
		public void Filter_FourArguments()
		{
			string arg1 = "arg1";
			int arg2 = 42;
			string arg3 = "arg3";
			string arg4 = "arg4";
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { arg1, arg2, arg3, arg4 };
			
			filter.Filter(parameterSet, GetMethod("MethodWithArgumentList"));
			
			Assert.That(parameterSet.Arguments.Length, Is.EqualTo(3));
			Assert.That(parameterSet.Arguments[0], Is.EqualTo(arg1));
			Assert.That(parameterSet.Arguments[1], Is.EqualTo(arg2));
			Assert.That(parameterSet.Arguments[2], Is.TypeOf(typeof(object[])));
			object[] argumentList = (object[])parameterSet.Arguments[2];
			Assert.That(argumentList.Length, Is.EqualTo(2));
			Assert.That(argumentList[0], Is.EqualTo(arg3));
			Assert.That(argumentList[1], Is.EqualTo(arg4));
		}
		
		[Test]
		public void Filter_WithoutArgumentList()
		{
			string arg1 = "arg1";
			int arg2 = 42;
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { arg1, arg2 };
			
			filter.Filter(parameterSet, GetMethod("MethodWithoutArgumentList"));
			
			Assert.That(parameterSet.Arguments.Length, Is.EqualTo(2));
			Assert.That(parameterSet.Arguments[0], Is.EqualTo(arg1));
			Assert.That(parameterSet.Arguments[1], Is.EqualTo(arg2));
		}
		
		#region Methods for tests
		
		public void MethodWithArgumentList(string arg1, int arg2, object[] argumentList)
		{
		}
		
		public void MethodWithoutArgumentList(string arg1, int arg2)
		{
		}
		
		#endregion
	}
}

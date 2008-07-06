// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;
using NUnitExtension.RowTest.AddIn;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class ConvertParameterSetFilterTest : ParameterSetFilterTestBase
	{
		private ConvertParameterSetFilter filter;
		
		[SetUp]
		public void SetUp()
		{
			filter = new ConvertParameterSetFilter();
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Filter_ParameterSetIsNull()
		{
			filter.Filter(null, GetMethod("DateTimeMethod"));
		}
		
		[Test]
		public void Filter_DateTime()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { "2008-04-13 16:12:00" };
			
			filter.Filter(parameterSet, GetMethod("DateTimeMethod"));
			
			Assert.That(parameterSet.Arguments[0], Is.EqualTo(new DateTime(2008, 4, 13, 16, 12, 0)));
		}
		
		[Test]
		public void Filter_Decimal()
		{
			ParameterSet parameterSet = new ParameterSet();
			parameterSet.Arguments = new object[] { "4.1266794625719769673704414587" };
			
			filter.Filter(parameterSet, GetMethod("DecimalMethod"));
			
			Assert.That(parameterSet.Arguments[0], Is.EqualTo(4.1266794625719769673704414587m));
		}
		
		#region Methods for tests
		
		public void DateTimeMethod(DateTime dateTime)
		{
		}
		
		public void DecimalMethod(Decimal decimalArgument)
		{
		}
		
		#endregion
	}
}

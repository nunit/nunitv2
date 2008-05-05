// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnit.Framework;
using NUnitExtension.RowTest;

namespace NUnitExtension.RowTest.Tests
{
	[TestFixture]
	public class RowTests
	{
		private static string s_staticField;
		
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			s_staticField = "Class Member";
		}
		
		[RowTest]
		[Row("Other string", "Class Member")]
		[Row("Class Member", "Other string")]
		public void StaticFieldTest (string compareTo, string setNext)
		{
			Assert.AreEqual (s_staticField, compareTo);
			s_staticField = setNext;
		}
		
		[RowTest]
		[Category("Category - 1")]
		[Row( 1000, 10, 100.0000)]
		[Row(-1000, 10, -100.0000)]
		[Row( 1000, 7, 142.85715)]
		[Row( 1000, 0.00001, 100000000)]
		[Row(4195835, 3145729, 1.3338196)]
		[Row( 1000, 0, 0, ExpectedException = typeof(DivideByZeroException))]
		public void DivisionTest(double numerator, double denominator, double result)
		{
		 	if (denominator == 0)
		 		throw new DivideByZeroException();
		
			Assert.AreEqual(result, numerator / denominator, 0.00001);
		}
		
		[RowTest]
		[Category("Category - 2")]
		[Row(1, 2, 3)]
		[Row(2, 3, 5)]
		[Row(3, 4, 8, TestName="Special case")]
		[Row(4, 5, 9)]
		[Row(10, 10, 0, TestName="ExceptionTest1",
		     ExpectedException=typeof(ArgumentException), ExceptionMessage="x and y may not be equal.")]
		[Row(1, 1, 0, TestName="ExceptionTest2",
		     ExpectedException=typeof(ArgumentException), ExceptionMessage="x and y may not be equal.")]
		public void AddTest(int x, int y, int expectedSum)
		{
			int sum = Sum(x, y);

			Assert.AreEqual(expectedSum, sum);
		}
		
		[RowTest]
		[Row(null)]
		public void NullArgument(string argument)
		{
			Assert.IsNull(argument);
		}
		
		[RowTest]
		[Row(SpecialValue.Null)]
		public void SpecialValueNullArgument(string argument)
		{
			Assert.IsNull(argument);
		}
		
		private int Sum(int x, int y)
		{
			int sum = x + y;
			
			if (x == 3 && y == 4)
				sum++;
			
			if (x == y)
				throw new ArgumentException("x and y may not be equal.");
			
			return sum;
		}
	}
}

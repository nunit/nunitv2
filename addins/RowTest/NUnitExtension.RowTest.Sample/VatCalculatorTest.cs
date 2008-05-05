// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NUnitExtension.RowTest;
using NUnitExtension.RowTest.Sample.VatCalculatorApp;

namespace NUnitExtension.RowTest.Sample
{
	[TestFixture]
	public class VatCalculatorTest
	{
		[RowTest]
		[Row(100, VatCategoryType.CategoryA, 20)]
		[Row(100, VatCategoryType.CategoryB, 15)]
		[Row(100, VatCategoryType.CategoryC, 10)]
		[Row(150, VatCategoryType.CategoryA, 30)]
		[Row(200, VatCategoryType.CategoryB, 30)]
		[Row(150, VatCategoryType.CategoryC, 15)]
		public void CalculateVat(int amount, VatCategoryType categoryType, int expectedResult)
		{
			VatCalculator calculator = new VatCalculator();
			
			decimal result = calculator.CalculateVat((decimal)amount, categoryType);
			
			Assert.That(result, Is.EqualTo((decimal)expectedResult));
		}
	}
}

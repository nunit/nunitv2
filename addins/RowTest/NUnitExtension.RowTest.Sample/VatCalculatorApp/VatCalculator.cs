// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;

namespace NUnitExtension.RowTest.Sample.VatCalculatorApp
{
	public class VatCalculator
	{
		public VatCalculator()
		{
		}
		
		public decimal CalculateVat(decimal amount, VatCategoryType categoryType)
		{
			return amount * GetTaxRate(categoryType);
		}
		
		private decimal GetTaxRate(VatCategoryType categoryType)
		{
			switch(categoryType)
			{
				case VatCategoryType.CategoryA:
					return 0.2m;
					
				case VatCategoryType.CategoryB:
					return 0.15m;
					
				case VatCategoryType.CategoryC:
					return 0.1m;
					
				default:
					throw new ArgumentException(string.Format("Invalid VatCategoryType '{0}'.", categoryType));
			}
		}
	}
}

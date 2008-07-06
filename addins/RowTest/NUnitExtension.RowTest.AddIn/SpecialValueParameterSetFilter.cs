// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public class SpecialValueParameterSetFilter : ParameterSetFilterBase
	{
		public const string SpecialValueEnum = "NUnitExtension.RowTest.SpecialValue";

		public SpecialValueParameterSetFilter()
		{
		}
		
		protected override bool CanFilter(object argument, ParameterInfo parameter)
		{
			return argument != null && argument.GetType().FullName == SpecialValueEnum;
		}
		
		protected override object FilterArgument(object argument, ParameterInfo parameter)
		{
			if (argument.ToString() == "Null")
				return null;
			
			return argument;
		}
	}
}

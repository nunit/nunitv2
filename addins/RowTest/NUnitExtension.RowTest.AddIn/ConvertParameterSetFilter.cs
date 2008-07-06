// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Globalization;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public class ConvertParameterSetFilter : ParameterSetFilterBase
	{
		public ConvertParameterSetFilter()
		{
		}
		
		protected override bool CanFilter(object argument, ParameterInfo parameter)
		{
			return argument is IConvertible;
		}
		
		protected override object FilterArgument(object argument, ParameterInfo parameter)
		{
			IFormatProvider formatProvider = GetFormatProvider(parameter.ParameterType);
			return Convert.ChangeType(argument, parameter.ParameterType, formatProvider);
		}
		
		private IFormatProvider GetFormatProvider(Type type)
		{
			if (type == typeof(DateTime))
				return DateTimeFormatInfo.InvariantInfo;
			
			if (IsNumericType(type))
				return NumberFormatInfo.InvariantInfo;
			
			return null;
		}
		
		private bool IsNumericType(Type type)
		{
			return type == typeof(byte)
				|| type == typeof(sbyte)
                || type == typeof(decimal)
                || type == typeof(double)
				|| type == typeof(float)
                || type == typeof(int)
                || type == typeof(uint)
				|| type == typeof(long)
				|| type == typeof(ulong)
                || type == typeof(short)
                || type == typeof(ushort);
		}
	}
}

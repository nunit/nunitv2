// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;

namespace NUnitExtension.RowTest.AddIn
{
	public class TypeParameterSetFilter : ParameterSetFilterBase
	{
		public TypeParameterSetFilter()
		{
		}
		
		protected override bool CanFilter(object argument, ParameterInfo parameter)
		{
			Type type = argument as Type;
			
			return type != null && parameter.ParameterType.IsAssignableFrom(type);
		}
		
		protected override object FilterArgument(object argument, ParameterInfo parameter)
		{
			Type type = argument as Type;
			
			ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
			
			if (constructor == null)
			{
				throw CreateArgumentException(
						"Cannot create an instance of type '{0}', because it does not have a default constructor.",
						type);
			}
			
			return constructor.Invoke (new object[0]);
		}
		
		private static ArgumentException CreateArgumentException(string message, params object[] args)
		{
			return new ArgumentException(string.Format(message, args));
		}
	}
}

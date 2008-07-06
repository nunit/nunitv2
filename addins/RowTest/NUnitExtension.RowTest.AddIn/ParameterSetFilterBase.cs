// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public abstract class ParameterSetFilterBase : IParameterSetFilter
	{
		public ParameterSetFilterBase()
		{
		}
		
		public void Filter(ParameterSet parameterSet, MethodInfo method)
		{
			if (parameterSet == null)
				throw new ArgumentNullException("parameterSet");
			
			ParameterInfo[] parameters = method.GetParameters();

			for (int i = 0; i < parameterSet.Arguments.Length; i++)
			{
				object argument = parameterSet.Arguments[i];
				
				if (CanFilter(argument, parameters[i]))
					parameterSet.Arguments[i] = FilterArgument(argument, parameters[i]);
			}
		}
		
		protected abstract bool CanFilter(object argument, ParameterInfo parameter);
		protected abstract object FilterArgument(object argument, ParameterInfo parameter);
	}
}

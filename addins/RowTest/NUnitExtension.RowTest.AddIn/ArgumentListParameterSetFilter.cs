// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public class ArgumentListParameterSetFilter : IParameterSetFilter
	{
		public ArgumentListParameterSetFilter()
		{
		}
		
		public void Filter(ParameterSet parameterSet, MethodInfo method)
		{
			object[] parameters = parameterSet.Arguments;
			ParameterInfo[] methodParameters = method.GetParameters();
			
			if (methodParameters[methodParameters.Length - 1].ParameterType == typeof(object[]))
				parameterSet.Arguments = GetArguments(parameters, methodParameters);
		}
		
		private object[] GetArguments(object[] parameters, ParameterInfo[] methodParameters)
		{
			object[] newParameterList = new object[methodParameters.Length];
			int argumentListPosition = methodParameters.Length - 1;
			
			for (int i = 0; i < argumentListPosition; i++)
				newParameterList[i] = parameters[i];
			
			int argumentListLength = parameters.Length - methodParameters.Length + 1;
			object[] argumentList = new object[argumentListLength];
			Array.Copy(parameters, argumentListPosition, argumentList, 0, argumentListLength);
			newParameterList[argumentListPosition] = argumentList;
			
			return newParameterList;
		}
	}
}

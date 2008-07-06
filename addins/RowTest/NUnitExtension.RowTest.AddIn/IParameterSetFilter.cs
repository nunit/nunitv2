// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public interface IParameterSetFilter
	{
		void Filter(ParameterSet parameterSet, MethodInfo method);
	}
}

// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Collections;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	public class RowTestParameterProvider : IParameterProvider
	{
		public const string RowTestAttribute = "NUnitExtension.RowTest.RowTestAttribute";
		public const string RowAttribute = "NUnitExtension.RowTest.RowAttribute";
		
		private ArrayList parameterSetFilters;

		public RowTestParameterProvider()
		{
			parameterSetFilters = new ArrayList();
		}
		
		public void AddParameterSetFilter(IParameterSetFilter filter)
		{
			if (filter == null)
				throw new ArgumentNullException("filter");
			
			parameterSetFilters.Add(filter);
		}
		
		public bool HasParametersFor(MethodInfo method)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			
			return Reflect.HasAttribute(method, RowAttribute, false) 
					&& Reflect.HasAttribute(method, RowTestAttribute, false);
		}
		
		public IEnumerable GetParametersFor(MethodInfo method)
		{
			if (method == null)
				throw new ArgumentNullException("method");

            Attribute[] rowAttributes = Reflect.GetAttributes(method, RowAttribute, false);
            ArrayList parameterSets = new ArrayList();

            foreach (Attribute rowAttribute in rowAttributes)
            {
                ParameterSet parameterSet = ParameterSet.FromDataSource(rowAttribute);

                foreach (IParameterSetFilter filter in parameterSetFilters)
                    filter.Filter(parameterSet, method);

                parameterSets.Add(parameterSet);
            }

            return (ParameterSet[])parameterSets.ToArray(typeof(ParameterSet));
		}
	}
}

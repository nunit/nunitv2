// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using NUnit.Core.Extensibility;

namespace NUnitExtension.RowTest.AddIn
{
	[NUnitAddin(Name = "Row Test Extension")]
	public class RowTestAddIn : IAddin
	{
		public RowTestAddIn()
		{
		}
		
		public bool Install(IExtensionHost host)
		{
			if (host == null)
				throw new ArgumentNullException("host");
			
			IExtensionPoint parameterProviders = host.GetExtensionPoint("ParameterProviders");
			if (parameterProviders == null)
				return false;
			
			parameterProviders.Install(CreateParameterProvider());
			return true;
		}
		
		private IParameterProvider CreateParameterProvider()
		{
			RowTestParameterProvider parameterProvider = new RowTestParameterProvider();
			parameterProvider.AddParameterSetFilter(new SpecialValueParameterSetFilter());
			parameterProvider.AddParameterSetFilter(new ConvertParameterSetFilter());
			parameterProvider.AddParameterSetFilter(new TypeParameterSetFilter());
			
			return parameterProvider;
		}
	}
}

// *********************************************************************
// Copyright 2007, Andreas Schlapsi
// This is free software licensed under the MIT license. 
// *********************************************************************
using System;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NUnit.Mocks;

namespace NUnitExtension.RowTest.AddIn.UnitTests
{
	[TestFixture]
	public class RowTestAddInTest
	{
		private DynamicMock extensionHostMock;
		private IExtensionHost extensionHost;
		private RowTestAddIn addIn;
		
		[SetUp]
		public void SetUp()
		{
			extensionHostMock = new DynamicMock(typeof(IExtensionHost));
			extensionHost = (IExtensionHost) extensionHostMock.MockInstance;
			addIn = new RowTestAddIn();
		}
		
		[Test]
		public void Install_Successful()
		{
			DynamicMock extensionPointMock = new DynamicMock(typeof(IExtensionPoint));
			IExtensionPoint extensionPoint = (IExtensionPoint) extensionPointMock.MockInstance;

			extensionHostMock.ExpectAndReturn("GetExtensionPoint", extensionPoint, "ParameterProviders");
			extensionPointMock.Expect("Install");

			bool installed = addIn.Install(extensionHost);
			
			extensionHostMock.Verify();
			extensionPointMock.Verify();
			Assert.That(installed, Is.True);
		}
		
		[Test]
		public void Install_Failure()
		{
			extensionHostMock.ExpectAndReturn("GetExtensionPoint", null, "ParameterProviders");

			bool installed = addIn.Install(extensionHost);
			
			extensionHostMock.Verify();
			Assert.That(installed, Is.False);
		}
	}
}

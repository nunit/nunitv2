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
#if !NMOCK2
using NUnit.Mocks;
#endif

namespace NUnit.Core.Extensions.RowTest.UnitTests
{
	[TestFixture]
	public class RowTestAddInTest : BaseTestFixture
	{
#if NMOCK2
		private NMock2.Mockery _mocks;
		
		[SetUp]
		public void SetUp()
		{
			_mocks = new NMock2.Mockery();
		}
#endif
		[Test]
		public void Install_Successful()
		{
#if NMOCK2
			IExtensionHost extensionHostMock = (IExtensionHost)_mocks.NewMock(typeof(IExtensionHost));
			IExtensionPoint extensionPointMock = (IExtensionPoint)_mocks.NewMock(typeof(IExtensionPoint));
			RowTestAddIn addIn = new RowTestAddIn();

			NMock2.Expect.Once.On(extensionHostMock)
					.Method("GetExtensionPoint").With("TestCaseBuilders")
					.Will(NMock2.Return.Value(extensionPointMock));
			
			NMock2.Expect.Once.On(extensionPointMock)
					.Method("Install").With(addIn);

			bool installed = addIn.Install(extensionHost);

			_mocks.VerifyAllExpectationsHaveBeenMet();
			Assert.That(installed, Is.True);
#else
            DynamicMock extensionHostMock = new DynamicMock(typeof(IExtensionHost));
		    IExtensionHost extensionHost = (IExtensionHost)extensionHostMock.MockInstance;
            DynamicMock extensionPointMock = new DynamicMock(typeof(IExtensionPoint));
		    IExtensionPoint extensionPoint = (IExtensionPoint)extensionPointMock.MockInstance;
            RowTestAddIn addIn = new RowTestAddIn();

            extensionHostMock.ExpectAndReturn("GetExtensionPoint", extensionPointMock.MockInstance, "TestCaseBuilders");
            extensionPointMock.Expect("Install", addIn);

            bool installed = addIn.Install(extensionHost);

            extensionPointMock.Verify();
            extensionHostMock.Verify();
            Assert.That(installed, Is.True);
#endif
		}
		
		[Test]
		public void Install_Failure()
		{
#if NMOCK2
			IExtensionHost extensionHostMock = (IExtensionHost)_mocks.NewMock(typeof(IExtensionHost));
			RowTestAddIn addIn = new RowTestAddIn();
			
			NMock2.Expect.Once.On(extensionHostMock)
					.Method("GetExtensionPoint").With("TestCaseBuilders")
					.Will(NMock2.Return.Value(null));
			
			bool installed = addIn.Install(extensionHostMock);
			
			_mocks.VerifyAllExpectationsHaveBeenMet();
			Assert.That(installed, Is.False);
#else
            DynamicMock extensionHostMock = new DynamicMock(typeof(IExtensionHost));
		    IExtensionHost extensionHost = (IExtensionHost) extensionHostMock.MockInstance;
            RowTestAddIn addIn = new RowTestAddIn();

            extensionHostMock.ExpectAndReturn("GetExtensionPoint",null,"TestCaseBuilders");

		    bool installed = addIn.Install(extensionHost);

            extensionHostMock.Verify();
            Assert.That(installed, Is.False);
#endif
		}
		
		[Test]
		public void CanBuildFrom_False()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("MethodWithoutRowTestAttribute");

			bool canBuildFrom = addIn.CanBuildFrom(method);
			
			Assert.That(canBuildFrom, Is.False);
		}
		
		[Test]
		public void CanBuildFrom_True()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");

			bool canBuildFrom = addIn.CanBuildFrom(method);
			
			Assert.That(canBuildFrom, Is.True);
		}
		
		[Test]
		public void BuildFrom_WithoutRows()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("MethodWithRowTestAttribute");
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(0));
		}
		
		[Test]
		public void BuildFrom_WithRows()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetRowTestMethodWith2Rows();
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(2));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			Assert.That(suite.Tests[1], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase1 = (RowTestCase) suite.Tests[0];
			RowTestCase testCase2 = (RowTestCase) suite.Tests[1];
			
			Assert.That(testCase1.Arguments.Length, Is.EqualTo(2));
			Assert.That(testCase2.Arguments.Length, Is.EqualTo(2));
		}
		
		[Test]
		public void BuildFrom_WithTestName()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetRowTestMethodWithTestName();
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(1));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase = (RowTestCase) suite.Tests[0];

			Assert.That(testCase.TestName.Name, Is.EqualTo("UnitTest(4, 5)"));
		}
		
		[Test]
		public void BuildFrom_WithExpectedException()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetRowTestMethodWithExpectedException();
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(1));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase = (RowTestCase) suite.Tests[0];

			Assert.That(testCase.ExceptionExpected, Is.True);
			Assert.That(testCase.ExpectedExceptionType, Is.EqualTo(typeof(InvalidOperationException)));
		}
		
		[Test]
		public void BuildFrom_WithExpectedExceptionAndExceptionMessage()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetRowTestMethodWithExpectedExceptionAndExceptionMessage();
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(1));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase = (RowTestCase) suite.Tests[0];

			Assert.That(testCase.ExceptionExpected, Is.True);
			Assert.That(testCase.ExpectedExceptionType, Is.EqualTo(typeof(InvalidOperationException)));
			Assert.That(testCase.ExpectedMessage, Is.EqualTo("Expected Exception Message."));
		}
		
		[Test]
		public void BuildFrom_SetsNameCorrectly()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			Type testClass = typeof(TestClass);
			MethodInfo method = testClass.GetMethod(Method_RowTestMethodWith2Rows, BindingFlags.Public | BindingFlags.Instance);
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestName.Name, Is.EqualTo(method.Name));
			Assert.That(suite.TestName.FullName, Is.EqualTo(testClass.FullName + "." + method.Name));
		}
		
		[Test]
		public void BuildFrom_SetsCommonNUnitAttributes()
		{
			RowTestAddIn addin = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("RowTestMethodWithCategory");
			
			Test test = addin.BuildFrom(method);

			Assert.That(test.Categories, Is.Not.Null);
			Assert.That(test.Categories.Count, Is.EqualTo(1));
			Assert.That(test.Categories[0], Is.EqualTo("Category"));
		}
		
		[Test]
		public void BuildFrom_WithSpecialValueNull()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("RowTestMethodWithSpecialValue");
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(1));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase = (RowTestCase) suite.Tests[0];
			
			Assert.That(testCase.Arguments.Length, Is.EqualTo(1));
			Assert.That(testCase.Arguments[0], Is.Null);
		}
		
		[Test]
		public void BuildFrom_WithNull()
		{
			RowTestAddIn addIn = new RowTestAddIn();
			MethodInfo method = GetTestClassMethod("RowTestMethodWithNullArgument");
			
			Test test = addIn.BuildFrom(method);
			
			Assert.That(test, Is.InstanceOfType(typeof(RowTestSuite)));
			RowTestSuite suite = (RowTestSuite) test;
			
			Assert.That(suite.TestCount, Is.EqualTo(1));
			Assert.That(suite.Tests[0], Is.InstanceOfType(typeof(RowTestCase)));
			
			RowTestCase testCase = (RowTestCase) suite.Tests[0];
			
			Assert.That(testCase.Arguments, Is.Null);
		}
	}
}

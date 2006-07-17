#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	public class AddinManager : ISuiteBuilder, ITestCaseBuilder, ITestDecorator
	{
		#region Instance Fields
		private AddinManager priorState = null;

		private SuiteBuilderCollection suiteBuilders = new SuiteBuilderCollection();
		private TestCaseBuilderCollection testBuilders = new TestCaseBuilderCollection();
		private TestDecoratorCollection testDecorators = new TestDecoratorCollection();

		#endregion

		#region Constructors

		public AddinManager() { }

		public AddinManager( AddinManager priorState )
		{
			this.priorState = priorState;
			this.suiteBuilders = new SuiteBuilderCollection( priorState.suiteBuilders );
			this.testBuilders = new TestCaseBuilderCollection( priorState.testBuilders );
			this.testDecorators = new TestDecoratorCollection( priorState.testDecorators );
		}

		#endregion

		#region Properties
		public AddinManager PriorState
		{
			get { return priorState; }
		}

		public IList Addins
		{
			get
			{
				ArrayList addins = new ArrayList();

				addins.AddRange( this.suiteBuilders );
				addins.AddRange( this.testBuilders );
				addins.AddRange( this.testDecorators );

				return addins;
			}
		}

		public IList Names
		{
			get
			{
				ArrayList names = new ArrayList();
			
				foreach( object addin in Addins )
				{
					AddinWrapper wrapper = addin as AddinWrapper;
					if (wrapper != null)
						names.Add( wrapper.Addin.GetType().Name );
					else
						names.Add( addin.GetType().Name );
				}

				return names;
			}
		}

		public IList AssemblyQualifiedNames
		{
			get
			{
				ArrayList names = new ArrayList();
			
				foreach( object addin in Addins )
				{
					AddinWrapper wrapper = addin as AddinWrapper;
					if (wrapper != null)
						names.Add( wrapper.Addin.GetType().AssemblyQualifiedName );
					else
						names.Add( addin.GetType().AssemblyQualifiedName );
				}

				return names;
			}
		}
		#endregion

		#region ISuiteBuilder Members
		public bool CanBuildFrom(Type type)
		{
			return suiteBuilders.CanBuildFrom( type );
		}

		public TestSuite BuildFrom(Type type)
		{
			return suiteBuilders.BuildFrom( type );
		}
		#endregion

		#region ITestCaseBuilder Members
		public bool CanBuildFrom(MethodInfo method)
		{
			return testBuilders.CanBuildFrom( method );
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			return testBuilders.BuildFrom( method );
		}
		#endregion

		#region ITestDecorator Members
		public TestCase Decorate(TestCase testCase, MethodInfo method)
		{
			return testDecorators.Decorate( testCase, method );
		}

		public TestSuite Decorate(TestSuite suite, Type fixtureType)
		{
			return testDecorators.Decorate( suite, fixtureType );
		}
		#endregion

		#region Addin Registration
		public void Register( Assembly assembly ) 
		{
			foreach( Type type in assembly.GetExportedTypes() )
			{
				if ( NUnitFramework.IsSuiteBuilder( type ) )
				{
					object builderObject = Reflect.Construct( type );
					ISuiteBuilder builder = builderObject as ISuiteBuilder;
                    // May not be able to cast, if the builder uses an earlier
                    // version of the interface, so we use reflection.
                    if (builder != null)
						suiteBuilders.Add( builder );
					else 
						suiteBuilders.Add( new SuiteBuilderWrapper( builderObject ) );
					// TODO: Figure out when to unload - this is
					// not important now, since we use a different
					// appdomain for each load, but may be in future.
				}
				else if ( NUnitFramework.IsTestCaseBuilder( type ) )
				{
					object builderObject = Reflect.Construct( type );
					ITestCaseBuilder builder = builderObject as ITestCaseBuilder;
                    // May not be able to cast, if the builder uses an earlier
                    // version of the interface, so we use reflection.
                    if (builder != null)
                        testBuilders.Add(builder);
                    else
                        testBuilders.Add(new TestCaseBuilderWrapper(builderObject));
				}
				else if ( NUnitFramework.IsTestDecorator( type ) )
				{
					object decoratorObject = Reflect.Construct( type );
					ITestDecorator decorator = decoratorObject as ITestDecorator;
                    // May not be able to cast, if the decorator uses an earlier
                    // version of the interface, so we use reflection.
                    if (decorator != null)
                        testDecorators.Add(decorator);
                    //else
                    //    testDecorators.Add(new TestDecoratorWrapper(decoratorObject));
				}
			}
		}

		public void Register( ISuiteBuilder builder )
		{
			suiteBuilders.Add( builder );
		}

		public void Register( ITestCaseBuilder builder )
		{
			testBuilders.Add( builder );
		}

		public void Register( ITestDecorator decorator )
		{
			testDecorators.Add( decorator );
		}

		public void Clear()
		{
			suiteBuilders.Clear();
			testBuilders.Clear();
			testDecorators.Clear();
		}
		#endregion

        #region Nested Wrapper Classes

		#region AddinWrapper
        private class AddinWrapper
        {
            protected object addin;

            public AddinWrapper(object addin)
            {
                this.addin = addin;
            }

			public object Addin
			{
				get { return addin; }
			}
        }
		#endregion

		#region SuiteBuilderWrapper
        private class SuiteBuilderWrapper : AddinWrapper, ISuiteBuilder
        {
            private MethodInfo canBuildFromMethod;
            private MethodInfo buildFromMethod;

            public SuiteBuilderWrapper(object builder) : base( builder )
            {
                this.canBuildFromMethod = Reflect.GetNamedMethod(
                    builder.GetType(),
                    "CanBuildFrom",
                    BindingFlags.Public | BindingFlags.Instance);
                this.buildFromMethod = Reflect.GetNamedMethod(
                    addin.GetType(),
                    "BuildFrom",
                    BindingFlags.Public | BindingFlags.Instance);
                if (buildFromMethod == null || canBuildFromMethod == null)
                    throw new ArgumentException("Invalid suite builder");
                // TODO: Check for proper signature - put in Reflect?
            }

            #region ISuiteBuilder Members

            public bool CanBuildFrom(Type type)
            {
                return (bool)canBuildFromMethod.Invoke(addin, new object[] { type });
            }

            public TestSuite BuildFrom(Type type)
            {
				object suiteObject = buildFromMethod.Invoke(addin, new object[] { type });
				return suiteObject as TestSuite;
            }

            #endregion

        }

        #endregion

        #region TestCaseBuilderWrapper
        private class TestCaseBuilderWrapper : AddinWrapper, ITestCaseBuilder
        {
            private MethodInfo canBuildFromMethod;
            private MethodInfo buildFromMethod;

            public TestCaseBuilderWrapper(object builder) : base( builder )
            {
                this.canBuildFromMethod = Reflect.GetNamedMethod(
                    builder.GetType(),
                    "CanBuildFrom",
                    BindingFlags.Public | BindingFlags.Instance);
                this.buildFromMethod = Reflect.GetNamedMethod(
                    builder.GetType(),
                    "BuildFrom",
                    BindingFlags.Public | BindingFlags.Instance);
                if (buildFromMethod == null || canBuildFromMethod == null)
                    throw new ArgumentException("Invalid suite builder");
                // TODO: Check for proper signature - put in Reflect?
            }

            #region ITestCaseBuilder Members

            public bool CanBuildFrom(MethodInfo method)
            {
                return (bool)canBuildFromMethod.Invoke(addin, new object[] { method });
            }

            public TestCase BuildFrom(MethodInfo method)
            {
                return (TestCase)buildFromMethod.Invoke(addin, new object[] { method });
            }

            #endregion

        }
        #endregion

        #region TestDecoratorWrapper

        private class TestDecoratorWrapper : AddinWrapper, ITestDecorator
        {
            private MethodInfo testCaseDecoratorMethod;
            private MethodInfo testSuiteDecoratorMethod;

            public TestDecoratorWrapper(object decorator) : base( decorator )
            {
                this.testCaseDecoratorMethod = Reflect.GetNamedMethod(
                    decorator.GetType(),
                    "Decorate",
                    new string[] { "NUnit.Core.TestCase", "System.Reflection.MethodInfo" },
                    BindingFlags.Public | BindingFlags.Instance);
                this.testSuiteDecoratorMethod = Reflect.GetNamedMethod(
                    decorator.GetType(),
                    "Decorate",
                    new string[] { "NUnit.Core.TestSuite", "System.Type" },
                    BindingFlags.Public | BindingFlags.Instance);
                if (testCaseDecoratorMethod == null || testSuiteDecoratorMethod == null)
                    throw new ArgumentException("Invalid suite builder");
                // TODO: Check for proper signature - put in Reflect?
            }

            #region ITestDecorator Members

            public TestCase Decorate(TestCase testCase, MethodInfo method)
            {
                return (TestCase)testCaseDecoratorMethod.Invoke(addin, new object[] { testCase, method });
            }

            public TestSuite Decorate(TestSuite suite, Type fixtureType)
            {
                return (TestSuite)testSuiteDecoratorMethod.Invoke(addin, new object[] { suite, fixtureType });
            }

            #endregion

        }

        #endregion

		#endregion
    }
}

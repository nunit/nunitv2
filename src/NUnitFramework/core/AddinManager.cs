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
using System.Reflection;

namespace NUnit.Core
{
	public class AddinManager : ISuiteBuilder, ITestCaseBuilder
	{
		#region Static Fields

		private static readonly string SuiteBuilderAttributeType = typeof( SuiteBuilderAttribute ).FullName;
		private static readonly string SuiteBuilderInterfaceType = typeof( ISuiteBuilder ).FullName;
		private static readonly string TestCaseBuilderAttributeName = typeof( TestCaseBuilderAttribute ).FullName;
		private static readonly string TestCaseBuilderInterfaceName = typeof( ITestCaseBuilder ).FullName;
		
		#endregion

		#region Instance Fields
		private AddinManager priorState = null;

		private SuiteBuilderCollection suiteBuilders = new SuiteBuilderCollection();
		private TestCaseBuilderCollection testBuilders = new TestCaseBuilderCollection();

		#endregion

		#region Constructors

		public AddinManager() { }

		public AddinManager( AddinManager priorState )
		{
			this.priorState = priorState;
			this.suiteBuilders = new SuiteBuilderCollection( priorState.suiteBuilders );
			this.testBuilders = new TestCaseBuilderCollection( priorState.testBuilders );
		}

		#endregion

		#region Properties
		public AddinManager PriorState
		{
			get { return priorState; }
		}
		#endregion

		#region ISuiteBuilder Members
		public bool CanBuildFrom(Type type)
		{
			return suiteBuilders.CanBuildFrom( type );
		}

		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			return suiteBuilders.BuildFrom( type, assemblyKey );
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

		#region Addin Registration
		public void Register( Assembly assembly ) 
		{
			foreach( Type type in assembly.GetExportedTypes() )
			{
				if ( Reflect.HasAttribute( type, SuiteBuilderAttributeType, false )
					&& Reflect.HasInterface( type, SuiteBuilderInterfaceType ) )
				{
					object builderObject = Reflect.Construct( type );
					ISuiteBuilder builder = builderObject as ISuiteBuilder;
					// May not be able to cast, if the builder uses an earlier
					// version of the interface.
					// TODO: Wrap the object and use reflection
					if ( builder != null )
						suiteBuilders.Add( builder );
					else 
						suiteBuilders.Add( new SuiteBuilderWrapper( builderObject ) );
					// TODO: Figure out when to unload - this is
					// not important now, since we use a different
					// appdomain for each load, but may be in future.
				}
				else if ( Reflect.HasAttribute( type, TestCaseBuilderAttributeName, false )
					&& Reflect.HasInterface( type, TestCaseBuilderInterfaceName ) )
				{
					object builderObject = Reflect.Construct( type );
					ITestCaseBuilder builder = (ITestCaseBuilder)builderObject;
					if ( builder != null )
						testBuilders.Add( builder );
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

		public void Clear()
		{
			suiteBuilders.Clear();
			testBuilders.Clear();
		}

		#endregion

		#region Nested SuiteBuilderWrapper Class

		private class SuiteBuilderWrapper : ISuiteBuilder
		{
			private object builder;
			private MethodInfo canBuildFromMethod;
			private MethodInfo buildFromMethod;

			public SuiteBuilderWrapper( object builder )
			{
				this.builder = builder;
				this.canBuildFromMethod = Reflect.GetNamedMethod( 
					builder.GetType(), 
					"CanBuildFrom", 
					BindingFlags.Public | BindingFlags.Instance );
				this.buildFromMethod = Reflect.GetNamedMethod(
					builder.GetType(),
					"BuildFrom",
					BindingFlags.Public | BindingFlags.Instance );
				if ( buildFromMethod == null || canBuildFromMethod == null )
					throw new ArgumentException( "Invalid suite builder" );
				// TODO: Check for proper signature - put in Reflect?
			}

			#region ISuiteBuilder Members

			public bool CanBuildFrom(Type type)
			{
				return (bool)canBuildFromMethod.Invoke( builder, new object[] { type } );
			}

			public TestSuite BuildFrom(Type type, int assemblyKey)
			{
				return (TestSuite)buildFromMethod.Invoke( builder, new object[] { type, assemblyKey } );
			}

			#endregion

		}

		#endregion
	}
}

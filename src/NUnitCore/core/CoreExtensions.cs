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
using System.IO;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace NUnit.Core
{
	public abstract class ExtensionHost : IExtensionHost
	{
		protected IExtensionPoint[] extensionPoints;

		#region IExtendable Interface
		public IExtensionPoint GetExtensionPoint( string name )
		{
			foreach ( IExtensionPoint extensionPoint in extensionPoints )
				if ( extensionPoint.Name == name )
					return extensionPoint;

			return null;
		}
		#endregion
	}

	/// <summary>
	/// The Addins class groups together all addin test suite and
	/// test case builders for access using the Singleton pattern.
	/// </summary>
	public class CoreExtensions : ExtensionHost
	{
		#region CoreExtensions Singleton
		private static CoreExtensions current;

		public static CoreExtensions Current
		{
			get 
			{ 
				if ( current == null )
				{
					current = new CoreExtensions();
					current.InstallBuiltins();
					current.InstallAddins();
				}

				return current;
			}
		}
		#endregion

		#region Instance Fields
		private CoreExtensions priorState = null;

		private SuiteBuilderCollection suiteBuilders;
		private TestCaseBuilderCollection testBuilders;
		private TestDecoratorCollection testDecorators;
		#endregion

		#region Constructors
		public CoreExtensions() 
		{ 
			suiteBuilders = new SuiteBuilderCollection();
			testBuilders = new TestCaseBuilderCollection();
			testDecorators = new TestDecoratorCollection();

			extensionPoints = new IExtensionPoint[]
				{ suiteBuilders, testBuilders, testDecorators };
		}

		private CoreExtensions( CoreExtensions other )
		{
			this.priorState = other.priorState;

			this.suiteBuilders = new SuiteBuilderCollection( other.suiteBuilders );
			this.testBuilders = new TestCaseBuilderCollection( other.testBuilders );
			this.testDecorators = new TestDecoratorCollection( other.testDecorators );

			extensionPoints = new IExtensionPoint[]
				{ suiteBuilders, testBuilders, testDecorators };
		}
		#endregion

		#region Properties
		public CoreExtensions PriorState
		{
			get { return priorState; }
		}

		public ISuiteBuilder SuiteBuilders
		{
			get { return suiteBuilders; }
		}

		public TestCaseBuilderCollection TestBuilders
		{
			get { return testBuilders; }
		}

		public ITestDecorator TestDecorators
		{
			get { return testDecorators; }
		}
		#endregion

		#region Methods
		public void SaveState()
		{
			priorState = new CoreExtensions( this );
		}

		public void RestoreState()
		{
			this.suiteBuilders = priorState.suiteBuilders;
			this.testBuilders = priorState.testBuilders;
			this.testDecorators = priorState.testDecorators;

			this.priorState = priorState.priorState;
		}
		
		public void InstallBuiltins()
		{
			// Define NUnit Framework
			TestFramework.Register( "NUnit", "nunit.framework" );

			// Install builtin SuiteBuilders - Note that the
			// NUnitTestCaseBuilder is installed whenever
			// an NUnitTestFixture is being populated and
			// removed afterward.
			Install( new Builders.NUnitTestFixtureBuilder() );
			Install( new Builders.SetUpFixtureBuilder() );
		}

		public void InstallAddins()
		{
			foreach( Addin addin in AddinManager.CurrentManager.Addins )
			{
				addin.Install( this );
			}
		}
		#endregion

		#region Type Safe Install Helpers
		internal void Install( ISuiteBuilder builder )
		{
			suiteBuilders.Install( builder );
		}

		internal void Install( ITestCaseBuilder builder )
		{
			testBuilders.Install( builder );
		}

		internal void Install( ITestDecorator decorator )
		{
			testDecorators.Install( decorator );
		}
		#endregion
	}
}

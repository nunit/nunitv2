using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	public interface ITestCaseBuilder
	{
		bool CanBuildFrom( MethodInfo method );
		TestCase BuildFrom( MethodInfo method );
	}

	public class TestCaseBuilderAttribute : System.Attribute
	{
	}

	public class AddinManagerState : IDisposable
	{
		private AddinManager manager;

		public AddinManagerState( AddinManager manager )
		{
			this.manager = manager;
			manager.Save();
		}

		public void Dispose()
		{
			if ( manager != null )
				manager.Restore();
			manager = null;
		}
	}

	/// <summary>
	/// Summary description for AddinManager.
	/// </summary>
	public class AddinManager : ISuiteBuilder, ITestCaseBuilder
	{
		#region Static Fields

		private static readonly string SuiteBuilderAttributeType = "NUnit.Framework.SuiteBuilderAttribute";
		private static readonly string SuiteBuilderInterfaceType = "NUnit.Core.ISuiteBuilder";
		private static readonly string TestCaseBuilderAttributeName = "NUnit.Core.TestCaseBuilderAttribute";
		private static readonly string TestCaseBuilderInterfaceName = "NUnit.Core.ITestCaseBuilder";
		private static readonly string TestDecoratorAttributeName = "NUnit.Core.TestDecoratorAttribute";
		private static readonly string TestDecoratorInterfaceName = "NUnit.Core.ITestDecorator";
		
		/// <summary>
		/// Addin manager that handles built in builders
		/// </summary>
		static AddinManager builtins = new AddinManager();

		/// <summary>
		/// Addin manager for actual addins
		/// </summary>
		static AddinManager addins = new AddinManager();

		#endregion

		#region Static Properties

		static public AddinManager Builtins
		{
			get { return builtins; }
		}

		static public AddinManager Addins
		{
			get { return addins; }
		}

		#endregion

		#region Static Constructor

		static AddinManager()
		{
			builtins.SuiteBuilders.Add( new NUnit.Core.Builders.NUnitTestFixtureBuilder() );
			builtins.SuiteBuilders.Add( new NUnit.Core.Builders.CSUnitTestFixtureBuilder() );
			builtins.SuiteBuilders.Add( new NUnit.Core.Builders.VstsTestFixtureBuilder() );

			// SuiteBuilders genrally add their test builders to addins while
			// the fixture is being built. But we add the nunit test case builder
			// here in order to support some tests that build test cases directly.
			builtins.TestBuilders.Add( new NUnit.Core.NUnitTestCaseBuilder() );
			
			// Load nunit extensions if available
			try
			{
				Assembly assembly = AppDomain.CurrentDomain.Load( "nunit.extensions" );
				addins.Register( assembly );
				System.Diagnostics.Trace.WriteLine( "NUnit extensions loaded" );
			}
			catch( Exception )
			{
				System.Diagnostics.Trace.WriteLine( "NUnit extensions not loaded" );
			}
		}

		#endregion

		#region Constructor

		private AddinManager() { }

		#endregion

		#region Instance Fields

		private InternalState current = new InternalState();

		#endregion

		#region Instance Properties

		public IList SuiteBuilders
		{
			get { return current.suiteBuilders; }
		}

		public IList TestBuilders
		{
			get { return current.testBuilders; }
		}

		#endregion

		#region ISuiteBuilder Members

		public bool CanBuildFrom(Type type)
		{
			foreach( ISuiteBuilder builder in SuiteBuilders )
				if ( builder.CanBuildFrom( type ) )
					return true;

			return false;
		}

		public TestSuite BuildFrom(Type type, int assemblyKey)
		{
			foreach( ISuiteBuilder builder in SuiteBuilders )
				if ( builder.CanBuildFrom( type ) )
					return builder.BuildFrom( type, assemblyKey );

			return null;
		}

		#endregion

		#region ITestCaseBuilder Members

		public bool CanBuildFrom(MethodInfo method)
		{
			foreach( ITestCaseBuilder builder in TestBuilders )
				if ( builder.CanBuildFrom( method ) )
					return true;

			return false;
		}

		public TestCase BuildFrom(MethodInfo method)
		{
			foreach( ITestCaseBuilder builder in TestBuilders )
				if ( builder.CanBuildFrom( method ) )
					return builder.BuildFrom( method );

			return null;
		}

		#endregion

		#region State Management

		private class InternalState
		{
			public InternalState priorState = null;

			public IList suiteBuilders = new ArrayList();
			public IList testBuilders = new ArrayList();

			public InternalState() { }

			public InternalState( InternalState priorState )
			{
				this.priorState = priorState;
				this.suiteBuilders = new ArrayList( priorState.suiteBuilders );
				this.testBuilders = new ArrayList( priorState.testBuilders );
			}

			public void Clear()
			{
				this.suiteBuilders = new ArrayList();
				this.testBuilders = new ArrayList();
			}
		}

		public void Save()
		{
			current = new InternalState( current );
		}

		public void Restore()
		{
			current = current.priorState;
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
						SuiteBuilders.Add( builder );
					else 
						SuiteBuilders.Add( new SuiteBuilderWrapper( builderObject ) );
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
						TestBuilders.Add( builder );
				}
			}
		}

		public void Add( ISuiteBuilder builder )
		{
			SuiteBuilders.Add( builder );
		}

		public void Add( ITestCaseBuilder builder )
		{
			TestBuilders.Add( builder );
		}

		public void Clear()
		{
			current.Clear();
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

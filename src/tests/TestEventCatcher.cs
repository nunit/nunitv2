using System;
using System.Collections;
using NUnit.Util;

namespace NUnit.Tests
{
	/// <summary>
	/// Summary description for TestEventCatcher.
	/// </summary>
	public class TestEventCatcher
	{
		public class TestEventArgsCollection : ReadOnlyCollectionBase
		{
			public TestEventArgs this[int index]
			{
				get { return (TestEventArgs)InnerList[index]; }
			}

			public void Add( TestEventArgs e )
			{
				InnerList.Add( e );
			}
		}

		private TestEventArgsCollection events;

		public TestEventCatcher( ITestEvents source )
		{
			events = new TestEventArgsCollection();

			source.ProjectLoading	+= new TestEventHandler( OnTestEvent );
			source.ProjectLoaded	+= new TestEventHandler( OnTestEvent );
			source.ProjectUnloading	+= new TestEventHandler( OnTestEvent );
			source.ProjectUnloaded	+= new TestEventHandler( OnTestEvent );

			source.TestLoading		+= new TestEventHandler( OnTestEvent );
			source.TestLoaded		+= new TestEventHandler( OnTestEvent );
			source.TestLoadFailed	+= new TestEventHandler( OnTestEvent );

			source.TestUnloading	+= new TestEventHandler( OnTestEvent );
			source.TestUnloaded		+= new TestEventHandler( OnTestEvent );
			source.TestUnloadFailed	+= new TestEventHandler( OnTestEvent );
		
			source.TestReloading	+= new TestEventHandler( OnTestEvent );
			source.TestReloaded		+= new TestEventHandler( OnTestEvent );
			source.TestReloadFailed	+= new TestEventHandler( OnTestEvent );

			source.RunStarting		+= new TestEventHandler( OnTestEvent );
			source.RunFinished		+= new TestEventHandler( OnTestEvent );

			source.TestStarting		+= new TestEventHandler( OnTestEvent );
			source.TestFinished		+= new TestEventHandler( OnTestEvent );
		
			source.SuiteStarting	+= new TestEventHandler( OnTestEvent );
			source.SuiteFinished	+= new TestEventHandler( OnTestEvent );
		}

		public TestEventArgsCollection Events
		{
			get { return events; }
		}

		private void OnTestEvent( object sender, TestEventArgs e )
		{
			events.Add( e );
		}
	}
}

#region Copyright (c) 2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright  2000-2002 Philip A. Craig
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
' Portions Copyright  2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright  2000-2002 Philip A. Craig
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
using NUnit.Core;
using NUnit.Util;

namespace NUnit.Tests.Util
{
	/// <summary>
	/// Summary description for TestEventCatcher.
	/// </summary>
	public class TestEventCatcher
	{
		public class TestEventArgsCollection : ReadOnlyCollectionBase
		{
			public EventArgs this[int index]
			{
				get { return (EventArgs)InnerList[index]; }
			}

			public void Add( EventArgs e )
			{
				InnerList.Add( e );
			}
		}

		private TestEventArgsCollection events;

		public TestEventCatcher( IProjectEvents source )
		{
			events = new TestEventArgsCollection();

			source.ProjectLoading	+= new TestProjectEventHandler( OnTestProjectEvent );
			source.ProjectLoaded	+= new TestProjectEventHandler( OnTestProjectEvent );
			source.ProjectLoadFailed+= new TestProjectEventHandler( OnTestProjectEvent );
			source.ProjectUnloading	+= new TestProjectEventHandler( OnTestProjectEvent );
			source.ProjectUnloaded	+= new TestProjectEventHandler( OnTestProjectEvent );
			source.ProjectUnloadFailed+= new TestProjectEventHandler( OnTestProjectEvent );

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

		private void OnTestProjectEvent( object sender, TestProjectEventArgs e )
		{
			events.Add( e );
		}
	}
}

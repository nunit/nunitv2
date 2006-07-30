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
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Gui.Tests
{
	/// <summary>
	/// Summary description for OptionSettingsTests.
	/// </summary>
	[TestFixture]
	public class OptionSettingsTests
	{
		private OptionSettings opts;

		[SetUp]
		public void Init()
		{
			opts = new UserSettings( new MemorySettingsStorage() ).Options;
		}

		[TearDown]
		public void Cleanup()
		{
			opts.Dispose();
		}

		[Test]
		public void LoadLastProject()
		{
			Assert.AreEqual( true, opts.LoadLastProject );
			opts.LoadLastProject = true;
			Assert.AreEqual( true, opts.LoadLastProject );
			opts.LoadLastProject = false;
			Assert.AreEqual( false, opts.LoadLastProject );
		}

		[Test]
		public void LoadLastInitialTreeDisplay()
		{
			Assert.AreEqual( 0, opts.InitialTreeDisplay );
			opts.InitialTreeDisplay = 1;
			Assert.AreEqual( 1, opts.InitialTreeDisplay );
			opts.InitialTreeDisplay = 2;
			Assert.AreEqual( 2, opts.InitialTreeDisplay );
		}

		[Test]
		public void ReloadOnChange()
		{
			if ( Environment.OSVersion.Platform == System.PlatformID.Win32NT )
			{
				Assert.AreEqual( true, opts.ReloadOnChange );
				opts.ReloadOnChange = true;
				Assert.AreEqual( true, opts.ReloadOnChange );
				opts.ReloadOnChange = false;
				Assert.AreEqual( false, opts.ReloadOnChange );
			}
			else
			{
				Assert.AreEqual( false, opts.ReloadOnChange );
				opts.ReloadOnChange = true;
				Assert.AreEqual( false, opts.ReloadOnChange );
			}
			
		}

		[Test]
		public void ReloadOnRun()
		{
			Assert.AreEqual( true, opts.ReloadOnRun );
			opts.ReloadOnRun = true;
			Assert.AreEqual( true, opts.ReloadOnRun );
			opts.ReloadOnRun = false;
			Assert.AreEqual( false, opts.ReloadOnRun );
			
		}

		[Test]
		public void ClearResults()
		{
			Assert.AreEqual( true, opts.ClearResults );
			opts.ClearResults = true;
			Assert.AreEqual( true, opts.ClearResults );
			opts.ClearResults = false;
			Assert.AreEqual( false, opts.ClearResults );

		}

		[Test]
		public void VisualStudioSupport()
		{
			Assert.AreEqual( false, opts.VisualStudioSupport );
			opts.VisualStudioSupport = true;
			Assert.AreEqual( true, opts.VisualStudioSupport );
			opts.VisualStudioSupport = false;
			Assert.AreEqual( false, opts.VisualStudioSupport );
		}
	}
}

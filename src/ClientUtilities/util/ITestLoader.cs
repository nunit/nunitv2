#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
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
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// The ITestLoader interface supports the loading and running
	/// of tests in a remote domain. In addition to methods for
	/// performing these operations, it inherits from the ITestEvents
	/// interface to provide appropriate events. The two interfaces
	/// are kept separate so that client objects not intended to
	/// issue commands can just handle the first interface.
	/// </summary>
	public interface ITestLoader
	{
		#region Properties

		// See if a project is loaded
		bool IsProjectLoaded { get; }

		// See if a test has been loaded from the project
		bool IsTestLoaded { get; }

		// True if there is a pending reload request
		bool IsReloadPending { get; }

		// See if a test is running
		bool IsTestRunning { get; }

		// The loaded test project
		NUnitProject TestProject { get; set; }

		string TestFileName { get; }

		// Our last test result
		TestResult LastResult { get; }

		#endregion

		#region Methods

		// Create a new empty project using a default name
		void NewProject();

		// Create a new project given a filename
		void NewProject( string filename );

		// Load a project given a filename
		bool LoadProject( string filename );

		// Load a project given a filename and config
		bool LoadProject( string filename, string configname );

		// Load a project given an array of assemblies
		bool LoadProject( string[] assemblies );

		// Unload current project
		void UnloadProject();

		// Load a project given a filename and then
		// load the tests for the active config.
		bool LoadTest( string filename );

		// Create a project for a list of assemblies
		// and then load the tests from the assemblies
		bool LoadTest( string[] assemblies );

		// Load tests for current project and config
		bool LoadTest();

		// Unload current test
		void UnloadTest();
		
		// Reload current test
		void ReloadTest();

		// Run a test suite
		void RunTestSuite( UITestNode test );

		// Cancel the running test
		void CancelTestRun();

		#endregion
	}
}
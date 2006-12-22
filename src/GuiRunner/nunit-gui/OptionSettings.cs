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
using NUnit.Util;

namespace NUnit.Gui
{
	public class Options
	{
		public static readonly string LoadLastProject = "LoadLastProject";
		public static readonly string InitialTreeDisplay = "InitialTreeDisplay";
		public static readonly string ReloadOnRun = "ReloadOnRun";
		public static readonly string ShowCheckBoxes = "ShowCheckBoxes";
		public static readonly string ReloadOnChange = "ReloadOnChange";
		public static readonly string RerunOnChange = "RerunOnChange";
		public static readonly string MergeAssemblies = "MergeAssemblies";
		public static readonly string AutoNamespaceSuites = "AutoNamespaceSuites";
		public static readonly string MultiDomain = "MultiDomain";
		public static readonly string ClearResults = "ClearResults";
		public static readonly string TestLabels = "TestLabels";
		public static readonly string FailureToolTips = "FailureToolTips";
		public static readonly string EnableWordWrapForFailures = "EnableWordWrapForFailures";
		public static readonly string VisualStudioSupport = "VisualStudioSupport";
	}

	/// <summary>
	/// Summary description for OptionSettings.
	/// </summary>
	public class OptionSettings : SettingsGroup
	{
		public OptionSettings( ISettingsStorage storage ) : base( storage ) { }

		public bool LoadLastProject
		{
			get { return GetSetting( Options.LoadLastProject, true ); }
			set { SaveSetting( Options.LoadLastProject, value ); }
		}

		public int InitialTreeDisplay
		{
			get { return GetSetting( Options.InitialTreeDisplay, 0 ); }
			set { SaveSetting( Options.InitialTreeDisplay, value ); }
		}

		public bool ReloadOnRun
		{
			get { return GetSetting( Options.ReloadOnRun, true ); }
			set { SaveSetting( Options.ReloadOnRun, value ); }
		}

		public bool ShowCheckBoxes
		{
			get { return GetSetting( Options.ShowCheckBoxes, false ); }
			set { SaveSetting( Options.ShowCheckBoxes, value ); }
		}

		public bool ReloadOnChange
		{
			get
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return false;

				return GetSetting( Options.ReloadOnChange, true ); 
			}

			set 
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return;

				SaveSetting( Options.ReloadOnChange, value );

				if ( value == false )
					RerunOnChange = false;
			}
		}

		public bool RerunOnChange
		{
			get
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return false;

				return GetSetting( Options.RerunOnChange, false );
			}

			set
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return;

				SaveSetting( Options.RerunOnChange, value );

				if ( value == true )
					ReloadOnChange = true;
			}
		}

		public bool MergeAssemblies
		{
			get { return GetSetting( Options.MergeAssemblies, false ); }
			set { SaveSetting( Options.MergeAssemblies, value ); }
		}

		public bool AutoNamespaceSuites
		{
			get { return GetSetting( Options.AutoNamespaceSuites, true ); }
			set { SaveSetting( Options.AutoNamespaceSuites, value ); }
		}

		public bool MultiDomain
		{
			get { return GetSetting( Options.MultiDomain, false ); }
			set { SaveSetting( Options.MultiDomain, value ); }
		}

		public bool ClearResults
		{
			get { return GetSetting( Options.ClearResults, true ); }
			set { SaveSetting( Options.ClearResults, value ); }
		}

		public bool TestLabels
		{
			get { return GetSetting( Options.TestLabels, false ); }
			set { SaveSetting( Options.TestLabels, value ); }
		}

		public bool FailureToolTips
		{
			get { return GetSetting( Options.FailureToolTips, true ); }
			set { SaveSetting( Options.FailureToolTips, value ); }
		}

		public bool EnableWordWrapForFailures
		{
			get { return GetSetting( Options.EnableWordWrapForFailures, true ); }
			set { SaveSetting( Options.EnableWordWrapForFailures, value ); }
		}

		public bool VisualStudioSupport
		{
			get { return GetSetting( Options.VisualStudioSupport, false ); }
			set { SaveSetting( Options.VisualStudioSupport, value ); }
		}
	}
}

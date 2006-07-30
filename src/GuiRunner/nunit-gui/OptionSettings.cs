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
	/// <summary>
	/// Summary description for OptionSettings.
	/// </summary>
	public class OptionSettings : SettingsGroup
	{
		public OptionSettings( ISettingsStorage storage ) : base( storage ) { }

		public bool LoadLastProject
		{
			get { return LoadBooleanSetting( "LoadLastProject", true ); }
			set { SaveBooleanSetting( "LoadLastProject", value ); }
		}

		public int InitialTreeDisplay
		{
			get { return LoadIntSetting( "InitialTreeDisplay", 0 ); }
			set { SaveIntSetting( "InitialTreeDisplay", value ); }
		}

		public bool ReloadOnRun
		{
			get { return LoadBooleanSetting( "ReloadOnRun", true ); }
			set { SaveBooleanSetting( "ReloadOnRun", value ); }
		}

		public bool ShowCheckBoxes
		{
			get { return LoadBooleanSetting( "ShowCheckBoxes", false ); }
			set { SaveBooleanSetting( "ShowCheckBoxes", value ); }
		}

		public bool ReloadOnChange
		{
			get
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return false;

				return LoadBooleanSetting( "ReloadOnChange", true ); 
			}

			set 
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return;

				SaveBooleanSetting( "ReloadOnChange", value );

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

				return LoadBooleanSetting( "RerunOnChange", false );
			}

			set
			{
				if ( Environment.OSVersion.Platform != System.PlatformID.Win32NT )
					return;

				SaveBooleanSetting( "RerunOnChange", value );

				if ( value == true )
					ReloadOnChange = true;
			}
		}

		public bool MergeAssemblies
		{
			get { return LoadBooleanSetting( "MergeAssemblies", false ); }
			set { SaveBooleanSetting( "MergeAssemblies", value ); }
		}

		public bool AutoNamespaceSuites
		{
			get { return LoadBooleanSetting( "AutoNamespaceSuites", true ); }
			set { SaveBooleanSetting( "AutoNamespaceSuites", value ); }
		}

		public bool MultiDomain
		{
			get { return LoadBooleanSetting( "MultiDomain", false ); }
			set { SaveBooleanSetting( "MultiDomain", value ); }
		}

		public bool ClearResults
		{
			get { return LoadBooleanSetting( "ClearResults", true ); }
			set { SaveBooleanSetting( "ClearResults", value ); }
		}

		public bool TestLabels
		{
			get { return LoadBooleanSetting( "TestLabels", false ); }
			set { SaveBooleanSetting( "TestLabels", value ); }
		}

		public bool FailureToolTips
		{
			get { return LoadBooleanSetting( "FailureToolTips", true ); }
			set { SaveBooleanSetting( "FailureToolTips", value ); }
		}

		public bool EnableWordWrapForFailures
		{
			get { return LoadBooleanSetting( "EnableWordWrapForFailures", true ); }
			set { SaveBooleanSetting( "EnableWordWrapForFailures", value ); }
		}

		public bool VisualStudioSupport
		{
			get { return LoadBooleanSetting( "VisualStudioSupport", false ); }
			set { SaveBooleanSetting( "VisualStudioSupport", value ); }
		}
	}
}

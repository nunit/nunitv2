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

namespace NUnit.Core
{
	/// <summary>
	/// Struct used to define behavior of a GenericTestFixtureBuilder
	/// </summary>
	public struct TestFixtureParameters
	{
		public string RequiredFramework;
		public string TestFixtureType;
		public string TestFixturePattern;
		public string TestCaseType;
		public string TestCasePattern;
		public string ExpectedExceptionType;
		public string SetUpType;
		public string TearDownType;
		public string FixtureSetUpType;
		public string FixtureTearDownType;
		public string IgnoreType;
		public bool InheritTestFixtureType;
		public bool InheritTestCaseType;
		public bool InheritSetUpAndTearDownTypes;

		public TestFixtureParameters(
			string RequiredFramework,
			string Namespace,
			string TestFixtureType,
			string TestFixturePattern,
			string TestCaseType,
			string TestCasePattern,
			string ExpectedExceptionType,
			string SetUpType,
			string TearDownType,
			string FixtureSetUpType,
			string FixtureTearDownType,
			string IgnoreType,
			bool InheritTestFixtureType,
			bool InheritTestCaseType,
			bool InheritSetUpAndTearDownTypes )
		{
			this.RequiredFramework = RequiredFramework;
			this.TestFixtureType = Namespace + "." + TestFixtureType;
			this.TestFixturePattern = TestFixturePattern;
			this.TestCaseType = Namespace + "." + TestCaseType;
			this.TestCasePattern = TestCasePattern;
			this.ExpectedExceptionType = Namespace + "." + ExpectedExceptionType;
			this.SetUpType = Namespace + "." + SetUpType;
			this.TearDownType = Namespace + "." + TearDownType;
			this.FixtureSetUpType = Namespace + "." + FixtureSetUpType;
			this.FixtureTearDownType = Namespace + "." + FixtureTearDownType;
			this.IgnoreType = Namespace + "." + IgnoreType;
			this.InheritTestFixtureType = InheritTestFixtureType;
			this.InheritTestCaseType = InheritTestCaseType;
			this.InheritSetUpAndTearDownTypes = InheritSetUpAndTearDownTypes;
		}

		public bool HasRequiredFramework
		{
			get { return IsValid( this.RequiredFramework ); }
		}

		public bool HasTestFixtureType
		{
			get { return IsValid( this.TestFixtureType ); }
		}

		public bool HasTestFixturePattern
		{
			get { return IsValid( this.TestFixturePattern ); }
		}

		public bool HasTestCaseType
		{
			get { return IsValid( this.TestCaseType ); }
		}

		public bool HasTestCasePattern
		{
			get { return IsValid( this.TestCasePattern ); }
		}

		public bool HasExpectedExceptionType
		{
			get { return IsValid( this.ExpectedExceptionType ); }
		}

		public bool HasSetUpType
		{
			get { return IsValid( this.SetUpType ); }
		}

		public bool HasTearDownType
		{
			get { return IsValid( this.TearDownType ); }
		}

		public bool HasFixtureSetUpType
		{
			get { return IsValid( this.FixtureSetUpType ); }
		}

		public bool HasFixtureTearDownType
		{
			get { return IsValid( this.FixtureTearDownType ); }
		}

		public bool HasIgnoreType
		{
			get { return IsValid( this.IgnoreType ); }
		}

		private bool IsValid( string s )
		{
			return s != null && s != string.Empty;
		}
	}
}

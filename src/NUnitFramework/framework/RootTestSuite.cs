using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestSuite forming the root of a test project
	/// </summary>
	public class RootTestSuite : TestSuite
	{
		public RootTestSuite( string projectName ) : base( projectName ) 
		{ 
		}
	}
}
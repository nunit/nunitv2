using System;
using System.IO;

namespace NUnit.Tests
{
	/// <summary>
	/// Base class for test fixtures that require data
	/// </summary>
	public class FixtureBase
	{
		static private string solutionPath;
		static private readonly string[] trySolution =
			{ @"..\nunit.sln", @"..\src\nunit.sln", @"..\..\nunit.sln", @"..\..\..\nunit.sln", @"..\..\..\..\nunit.sln" };

		static private string sourcePath;

		static private string testDataPath;

		static private string samplesPath;

		public string SolutionPath
		{
			get
			{
				if ( solutionPath == null )
				{
					foreach ( string path in trySolution )
						if ( File.Exists( path ) )
						{
							solutionPath = Path.GetFullPath( path );
							break;
						}

					if ( solutionPath == null )
						throw new ApplicationException( "Unable to locate NUnit source files for testing" );
				}

				return solutionPath;
			}
		}

		public string SourcePath
		{
			get
			{
				if ( sourcePath == null )
					sourcePath = Path.GetDirectoryName( SolutionPath );

				return sourcePath;
			}
		}

		public string TestDataPath
		{
			get
			{
				if ( testDataPath == null )
					testDataPath = Path.Combine( SourcePath, "TestData" );

				return testDataPath;
			}
		}

		public string SamplesPath
		{
			get
			{
				if ( samplesPath == null )
					samplesPath = Path.Combine( SourcePath, "samples" );

				return samplesPath;
			}
		}

//		public string GetTestDataPath( string fileName )
//		{
//			return Path.Combine( TestDataPath, fileName );
//		}

		public string GetSamplesPath( string fileName )
		{
			return Path.Combine( SamplesPath, fileName );
		}
		
	}
}

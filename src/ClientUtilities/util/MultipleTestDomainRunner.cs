using System;
using NUnit.Core;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for MultipleTestDomainRunner.
	/// </summary>
	public class MultipleTestDomainRunner : AggregatingTestRunner
	{
		#region Constructors
		public MultipleTestDomainRunner() : base( 0 ) { }

		public MultipleTestDomainRunner( int runnerID ) : base( runnerID ) { }
		#endregion

		#region Load Method Overrides
		public override bool Load( string assemblyName)
		{
			return Load(assemblyName, string.Empty);
		}

		public override bool Load(string assemblyName, string testName)
		{
			CreateRunners( 1 );
			return runners[0].Load(assemblyName, testName);
		}

		public override bool Load(TestPackage package)
		{
			this.projectName = package.ProjectPath;
			this.testName.FullName = this.testName.Name = projectName;
			CreateRunners( package.Count );

			bool result = true;
			int index = 0;
			foreach( string assembly in package )
				if ( !runners[index++].Load( assembly ) )
					result = false;

			return result;
		}

		public override bool Load( TestPackage package, string testName )
		{
			this.projectName = package.ProjectPath;
			this.testName.FullName = this.testName.Name = projectName;
			CreateRunners( package.Count );

			//TODO: Loading a namespace or fixture needs work
			bool result = false;
			int index = 0;
			foreach( string assembly in package )
				if ( runners[index++].Load( assembly, testName ) )
					result = true;

			return result;
		}

		private void CreateRunners( int count )
		{
			runners = new TestRunner[count];
			for( int index = 0; index < count; index++ )
			{
				TestDomain runner = new TestDomain( this.runnerID * 100 + index + 1 );
				foreach( string key in this.Settings.Keys )
					runner.Settings[key] = this.Settings[key];
				runners[index] = runner;
			}
		}
		#endregion
	}
}

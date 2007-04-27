// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

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
		public override bool Load(TestPackage package)
		{
			this.projectName = package.FullName;
			this.testName.FullName = this.testName.Name = projectName;
			CreateRunners( package.Assemblies.Count );

			int nfound = 0;
			int index = 0;
			foreach( string assembly in package.Assemblies )
			{
				TestPackage p = new TestPackage( assembly );
				p.TestName = package.TestName;
				foreach( object key in package.Settings.Keys )
					p.Settings[key] = package.Settings[key];
				if ( runners[index++].Load( p ) )
					nfound++;
			}

			if ( package.TestName == null )
				return nfound == package.Assemblies.Count;
			else
				return nfound > 0;
		}

		private void CreateRunners( int count )
		{
			runners = new TestRunner[count];
			for( int index = 0; index < count; index++ )
			{
				TestDomain runner = new TestDomain( this.runnerID * 100 + index + 1 );
				runners[index] = runner;
			}
		}
		#endregion
	}
}

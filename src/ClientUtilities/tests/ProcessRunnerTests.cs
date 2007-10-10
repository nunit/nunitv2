// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System.Diagnostics;
using System.IO;
using NUnit.Core;
using NUnit.Core.Tests;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
	/// <summary>
	/// Summary description for ProcessRunnerTests.
	/// </summary>
	[TestFixture,Platform(Exclude="Mono",Reason="Process Start not working correctly")]
	public class ProcessRunnerTests : BasicRunnerTests
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private ProcessRunner myRunner;

		protected override TestRunner CreateRunner( int runnerID )
		{
			myRunner = new ProcessRunner( runnerID );
			log.Debug( "Creating ProcessRunner" );
			return myRunner;
		}

		[TestFixtureTearDown]
		public void DestroyRunner()
		{
			log.Debug( "Destroying ProcessRunner" );
			if ( myRunner != null )
				myRunner.Dispose();
		}
	}
}

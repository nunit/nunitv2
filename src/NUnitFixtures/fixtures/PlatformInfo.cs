using System;

namespace NUnit.Fixtures
{
	/// <summary>
	/// PlatformFixture simply displays info about the platform
	/// we are running on.
	/// </summary>
	public class PlatformInfo : fit.Fixture
	{
		public override void doTable(fit.Parse table)
		{
			table.parts.more =
				tr( td( "platform", td( Environment.OSVersion.ToString(), null ) ),
				tr( td( "clrVersion", td( Environment.Version.ToString(), null ) ),
				null ) );
				
		}

		private fit.Parse tr( fit.Parse parts, fit.Parse more) 
		{
			return new fit.Parse ("tr", null, parts, more);
		}

		private fit.Parse td(string body, fit.Parse more) 
		{
			return new fit.Parse ("td", info(body), null, more);
		}
	}
}

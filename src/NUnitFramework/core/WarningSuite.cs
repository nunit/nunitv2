using System;

namespace Nunit.Core
{
	/// <summary>
	/// Summary description for WarningSuite.
	/// </summary>
	public class WarningSuite : TestSuite
	{
		public WarningSuite(string name) : base(name) {
			ShouldRun=false;
		}

		protected internal override void Add(Test test)
		{
			base.Add(test);
			test.ShouldRun = false;
			test.IgnoreReason = "Containing Suite cannot be run";
		}

		protected internal override TestSuite CreateNewSuite(string name) 
		{
			return new WarningSuite(name);
		}
	}
}

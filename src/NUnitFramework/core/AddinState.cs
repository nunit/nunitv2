using System;

namespace NUnit.Core
{
	/// <summary>
	/// Helper class used to save and restore the
	/// state of the addins. Instantiate in
	/// a using statement and any changes made
	/// within its scope will be restored on exit.
	/// </summary>
	public class AddinState : IDisposable
	{
		public AddinState()
		{
			Addins.Save();
		}

		public void Dispose()
		{
			Addins.Restore();
		}
	}
}

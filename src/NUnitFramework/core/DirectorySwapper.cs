using System;

namespace NUnit.Core
{
	/// <summary>
	/// Summary description for DirectorySwapper.
	/// </summary>
	public class DirectorySwapper : IDisposable
	{
		private string savedDirectoryName;

		public DirectorySwapper() : this( null ) { }

		public DirectorySwapper( string directoryName )
		{
			savedDirectoryName = Environment.CurrentDirectory;
			
			if ( directoryName != null && directoryName != string.Empty )
				Environment.CurrentDirectory = directoryName;
		}

		public void Dispose()
		{
			Environment.CurrentDirectory = savedDirectoryName;
		}
	}
}

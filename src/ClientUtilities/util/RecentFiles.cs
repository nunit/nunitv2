using System;
using System.Collections;

namespace NUnit.Util
{
	/// <summary>
	/// The RecentFiles interface is used to isolate the app
	/// from various implementations of recent files.
	/// </summary>
	public interface RecentFiles
	{ 
		/// <summary>
		/// The max number of files saved
		/// </summary>
		int MaxFiles { get; set; }

		/// <summary>
		/// Get or set the most recent file name, reordering
		/// the saved names as needed and removing the oldest
		/// if the max number of files would be exceeded.
		/// </summary>
		string RecentFile { get; set; }

		/// <summary>
		/// Get a list of all the files
		/// </summary>
		/// <returns>The most recent file list</returns>
		IList GetFiles();

		/// <summary>
		/// Clear the list of files
		/// </summary>
		void Clear();

		/// <summary>
		/// Remove a file from the list
		/// </summary>
		/// <param name="fileName">The name of the file to remove</param>
		void Remove( string fileName );
	}
}
